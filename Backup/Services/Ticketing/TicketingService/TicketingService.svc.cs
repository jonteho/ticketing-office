using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.TicketingService.Contracts;
using TicketingOffice.Common.Helpers;
using TicketingOffice.CrmService.Contracts;
using TicketingOffice.Common.Properties;
using TicketingOffice.ShowsService.Contracts;
using TicketingOffice.HallState.Contracts;
using TicketingOffice.TicketingService.BusinessLogic;
using TicketingOffice.PaymentService.Contracts;
using TicketingOffice.Pricing.Contracts;
using System.Transactions;
using TicketingOffice.Printing.BusinessLogic;
using TicketingOffice.CurrencyExchange.Contract;
using TicketingOffice.ClientNotification;
using TicketingOffice.ClientNotification.Contract;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;


namespace TicketingOffice.TicketingService
{
    /// <summary>
    /// The ticketing service orchestrate the ticketing use cases.
    /// Using the ticketing service you can order a ticket, pay for a ticket, cancel a ticket etc.
    /// The ticketing service calls all the other ticketing services to implement the ticketing use cases.
    /// </summary>
    // TODO: Ex3 - Decorate the GeneralTicketingService service with a ServiceBehavior attribute
    [ServiceBehavior(TransactionAutoCompleteOnSessionClose = true)]
    public class GeneralTicketingService : ServiceBase, ITicketingService, INotificationManager, IRegisterForDuplexNotification
    {
        ChannelFactory<ICrmBase> crmChf;
        ChannelFactory<IShowsService> showsChf;
        ChannelFactory<IReservationService> reservationsChf;
        ChannelFactory<IPricingService> pricingChf;
        ChannelFactory<IPaymentService> paymentChf;

        ITicketingManager manager = new TicketingManager();
        IPrinting PrintingManager;

        #region ITicketingService Members
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public Guid OrderTicket(Contracts.Order newOrder, SeatIndex[] seats)
        {
            //1. validate the order is valid
            //validate newOrder.EventInfo and newOrder.CustomerInfo
            ICrmBase crmProx = null;
            IShowsService showsProx = null;
            IPricingService pricingProx = null;
            IReservationService reservationsProx = null;
            Guid reservationID;

            if (newOrder == null)
                throw new NullReferenceException(StringsResource.NullOrder);
            if (newOrder.CustomerInfo == null)
                throw new NullReferenceException(StringsResource.NullCustomer);
            if (newOrder.EventInfo == null)
                throw new NullReferenceException(StringsResource.NullEvent);
            if ((seats == null) || (seats.Count() == 0))
                throw new ArgumentException(StringsResource.NoSeatsInOrder);

            #region Validate Customer
            //Contact the Crm service to ensure the customer exist.
            lock (this)
            {
                if (crmChf == null)
                    crmChf = new ChannelFactory<ICrmBase>("CrmCertEP");
            }

            try
            {
                crmProx = crmChf.CreateChannel();
                var customer = crmProx.GetCustomerByID(newOrder.CustomerInfo.ID);
                if (customer == null)
                    throw new TicketingException(string.Format(StringsResource.CustomerNotFound, newOrder.CustomerInfo.ID));
                newOrder.CustomerInfo = CustomerMapper.MapToTicketingCustomer(customer);

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new TicketingException(StringsResource.FailedToContactCrm, ex);
            }
            finally
            {
                var channel = crmProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }

            #endregion

            #region Validate Event

            ShowsService.Contracts.Event _event;

            //1. contact the shows service to fetch information about the event.
            lock (this)
            {
                if (showsChf == null)
                    showsChf = new ChannelFactory<IShowsService>("ShowEP");
            }

            try
            {
                showsProx = showsChf.CreateChannel();
                _event = showsProx.FindEventByID(newOrder.EventInfo.EventID);
                if (_event == null)
                    throw new TicketingException(string.Format(StringsResource.EventNotFound, newOrder.EventInfo.EventID));
                newOrder.EventInfo = _event;
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new TicketingException(StringsResource.FailedToContactShows + " " + ex.Message, ex);
            }
            finally
            {
                var channel = showsProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }

            //2. check the event exist and it is open
            if ((_event == null) || (_event.State != EventState.Opened))
                throw new TicketingException(StringsResource.InvalidOrClosedEvent);
            #endregion

            #region Call Pricing to calculate the total price
            lock (this)
            {
                if (pricingChf == null)
                    pricingChf = new ChannelFactory<IPricingService>("PricingEP");
            }

            try
            {
                pricingProx = pricingChf.CreateChannel();
                var price = pricingProx.CalculatePrice(reductionCode: newOrder.CustomerInfo.ReductionCode.Value,
                                                            policyName: newOrder.EventInfo.PricingPolicy,
                                                            listPrice: (int)newOrder.EventInfo.ListPrice,
                                                            numberOfTickets: seats.Count(), currency: null);
                newOrder.TotalPrice = price * seats.Count();

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new TicketingException(StringsResource.FailedToContactPricing + " " + ex.Message, ex);
            }
            finally
            {
                var channel = pricingProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
            #endregion

            #region Call the resrevation service and reserve seats
            //Contact the HallState service to perform the reservation.
            lock (this)
            {
                if (reservationsChf == null)
                    reservationsChf = new ChannelFactory<IReservationService>("ReservationsEP");
            }

            try
            {
                reservationsProx = reservationsChf.CreateChannel();
                reservationID = reservationsProx.CreateResevation(new Reservation()
                {
                    ID = Guid.NewGuid(),
                    CustomerID = newOrder.CustomerInfo.ID,
                    EventID = newOrder.EventInfo.EventID,
                    Seats = seats.ToList(),
                    Remarks = newOrder.Remarks
                });

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new TicketingException(StringsResource.FailedToContactHallState + " " + ex.Message, ex);
            }
            finally
            {
                var channel = reservationsProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }

            #endregion


            newOrder.ID = reservationID;
            newOrder.State = OrderState.Created;
            manager.OrderTicket(newOrder);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.TicketingNotification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications(new OrderMessage()
                {
                    Content = "Order Created",
                    OrderID = reservationID
                }, clientRegistrations);

            #endregion

            return newOrder.ID;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public Payment PayForTicket(Guid orderID, Guid payingCustomerID, double amount, PaymentType methodOfPayment, Currencies? currency, string creditCard)
        {
            IPaymentService paymentProx = null;
            IExchangeService exchangeProx = null;

            Payment paymentConfirmation;

            //validate order
            var order = manager.FindOrder(orderID);
            if (order == null)
                throw new TicketingException(StringsResource.NullOrder);

            if ((order.State == OrderState.Approved) && (amount > 0))
                throw new TicketingException(StringsResource.OrderAlreadyApproved);


            if (currency != null)
            {
                exchangeProx = new CurrencyExchangeWcfProxy.CurrencyExchangeProxy();
                amount = Math.Round(exchangeProx.Buy(currency.Value, amount), 1);
            }

            #region call the payment service
            //call the payment service
            lock (this)
            {
                if (paymentChf == null)
                    paymentChf = new ChannelFactory<IPaymentService>("CertPaymentEP");
            }

            try
            {
                paymentProx = paymentChf.CreateChannel();
                paymentConfirmation = paymentProx.PayForOrder(orderID, payingCustomerID, amount, methodOfPayment, creditCard);

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new TicketingException(StringsResource.FailedToContactPayment + " " + ex.Message, ex);
            }
            finally
            {
                var channel = paymentProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
            #endregion

            //save the payment in the order;
            order.PaymentsInfo.Add(paymentConfirmation);
            double totalpayments = 0;
            foreach (var item in order.PaymentsInfo)
                totalpayments += item.Amount;

            if ((totalpayments >= order.TotalPrice) && (order.State != OrderState.Approved))
            {
                order.State = OrderState.Approved;
                manager.UpdateOrder(order);
            }

            if ((amount < 0) && (order.State == OrderState.Approved))
            {
                order.State = OrderState.Created;
                manager.UpdateOrder(order);
            }

            return paymentConfirmation;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public Payment CancelTicket(Guid orderID, Guid payingCustomerID, string creditCard)
        {
            IReservationService resrevationsProx = null;
            Payment refund = null;

            //validate order
            var order = manager.FindOrder(orderID);
            if (order == null)
                throw new TicketingException(StringsResource.NullOrder);

            refund = PayForTicket(orderID, payingCustomerID, 0 - order.TotalPrice, PaymentType.Voucher, null, creditCard);
            order.State = OrderState.Canceled;
            manager.UpdateOrder(order);

            #region Call the resrevation service to unreserve seats
            //Contact the HallState service to perform the reservation.
            lock (this)
            {
                if (reservationsChf == null)
                    reservationsChf = new ChannelFactory<IReservationService>("ReservationsEP");
            }

            try
            {
                resrevationsProx = reservationsChf.CreateChannel();
                resrevationsProx.DeleteResrevation(orderID);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new TicketingException(StringsResource.FailedToContactHallState + " " + ex.Message, ex);
            }
            finally
            {
                var channel = resrevationsProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }

            #endregion

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.TicketingNotification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications(new OrderMessage()
                {
                    Content = "Order Canceled, Check for refund",
                    OrderID = orderID
                }, clientRegistrations);

            #endregion

            return refund;
        }


        public Order FindOrder(Guid orderID)
        {

            Order order = manager.FindOrder(orderID);
            FillPaymentInfo(order);
            FillEventInfo(order);
            FillCustomerInfo(order);

            return order;

        }

        public Order[] FindOrders(OrderCriteria criteria)
        {
            Order[] orders = manager.FindOrdersByCriteria(criteria);

            foreach (var item in orders)
            {
                FillPaymentInfo(item);
                FillEventInfo(item);
                FillCustomerInfo(item);
            }

            return orders;
        }

        public Guid[] FindBestCustomersIds(int numberOfCustomers)
        {
            return manager.GetBestCustomersID(numberOfCustomers);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public string PrintTicket(Guid orderID)
        {
            if (PrintingManager == null)
                PrintingManager = new ConsolePrintingManager();

            Order order = manager.FindOrder(orderID);

            return PrintingManager.Print(order);
        }

        #endregion

        /// <summary>
        /// Call the crm service to fill the customer info. In the orders DB only the ID is saved. 
        /// The order data contract includes a full customer record so the crm service is called to get that info.
        /// </summary>
        /// <param name="ord"></param>
        private void FillCustomerInfo(Order ord)
        {
            ICrmBase crmProx = null;
            lock (this)
            {
                if (crmChf == null)
                    crmChf = new ChannelFactory<ICrmBase>("CrmCertEP");
            }

            try
            {
                crmProx = crmChf.CreateChannel();
                var crm_cust = crmProx.GetCustomerByID(ord.CustomerInfo.ID);
                if (crm_cust == null)
                    throw new TicketingException(string.Format(StringsResource.CustomerNotFound, ord.CustomerInfo.ID));
                ord.CustomerInfo = CustomerMapper.MapToTicketingCustomer(crm_cust);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new TicketingException(StringsResource.FailedToContactCrm + " " + ex.Message, ex);
            }
            finally
            {
                var channel = crmProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
        }

        /// <summary>
        /// call the shows service to fill in the Event info
        /// </summary>
        /// <param name="ord"></param>
        private void FillEventInfo(Order ord)
        {
            IShowsService showsProx = null;
            lock (this)
            {
                if (showsChf == null)
                    showsChf = new ChannelFactory<IShowsService>("ShowEP");
            }

            try
            {
                showsProx = showsChf.CreateChannel();
                ord.EventInfo = showsProx.FindEventByID(ord.EventInfo.EventID);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new TicketingException(StringsResource.FailedToContactPayment + " " + ex.Message, ex);
            }
            finally
            {
                var channel = showsProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
        }

        /// <summary>
        /// call the payment service to fill in the payments info
        /// </summary>
        /// <param name="order"></param>   
        private void FillPaymentInfo(Order order)
        {
            IPaymentService paymentProx = null;
            lock (this)
            {
                if (paymentChf == null)
                    paymentChf = new ChannelFactory<IPaymentService>("CertPaymentEP");
            }

            try
            {
                paymentProx = paymentChf.CreateChannel();
                var payments = paymentProx.FindPayments(new PaymentCriteria() { OrderID = order.ID });
                order.PaymentsInfo = payments.ToList();
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new TicketingException(StringsResource.FailedToContactPayment + " " + ex.Message, ex);
            }
            finally
            {
                var channel = paymentProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
        }

    }



}
