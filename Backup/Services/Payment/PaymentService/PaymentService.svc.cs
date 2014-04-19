using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.PaymentService.Contracts;
using TicketingOffice.PaymentService.BusinessLogic;
using TicketingOffice.TicketingService.Contracts;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Properties;
using TicketingOffice.CrmService.Contracts;
using TicketingOffice.ClientNotification;
using TicketingOffice.ClientNotification.Contract;

namespace TicketingOffice.PaymentService
{
    /// <summary>
    /// Implement payment.
    /// The payment service simulate a dredit card clearing system.
    /// 
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TicketsPaymentService : ServiceBase, IPaymentService, INotificationManager, IRegisterForDuplexNotification
    {
        IPaymentManager manager = new PaymentManager();

        ChannelFactory<ITicketingService> ticketingChf;
        ChannelFactory<ICrmBase> crmChf;

        #region IPaymentService Members

        /// <summary>
        /// Pay for a specific order.
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="customerID"></param>
        /// <param name="amount"></param>
        /// <param name="methodOfPayment"></param>
        /// <param name="CreditCardNumber"></param>
        /// <returns></returns>
        public Payment PayForOrder(Guid orderID, Guid customerID, double amount, PaymentType methodOfPayment, string CreditCardNumber)
        {
            ITicketingService orderProx = null;
            ICrmBase crmProx = null;

            #region Validate Order
            //Contact the ticketing service to ensure the the order exist.
            lock (this)
            {
                if (ticketingChf == null)
                    ticketingChf = new ChannelFactory<ITicketingService>("TicketingEP");
            }

            try
            {
                orderProx = ticketingChf.CreateChannel();
                var order = orderProx.FindOrder(orderID);
                if (order == null)
                    throw new NullReferenceException(StringsResource.NullOrder);

                //It is always possible to pay less (the order will not be approved) but it should not be possible to pay more then required.
                if (order.TotalPrice < amount)
                    throw new PaymentException(StringsResource.AmountTooLarge);

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new PaymentException(StringsResource.FailedToContactTicketing + " " + ex.Message, ex);
            }
            finally
            {
                var channel = orderProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
            #endregion

            #region Validate Customer
            //Contact the Crm service to ensure the customer exist.
            //The payment service is using a certificate to identify against the crm service.
            lock (this)
            {
                if (crmChf == null)
                    crmChf = new ChannelFactory<ICrmBase>("CrmCertEP");
            }

            try
            {
                crmProx = crmChf.CreateChannel();
                var customer = crmProx.GetCustomerByID(customerID);
                if (customer == null) 
                    throw new NullReferenceException(StringsResource.NullCustomer);
               

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new PaymentException(string.Format(StringsResource.CustomerNotFound, customerID), ex);
            }
            finally
            {
                var channel = crmProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }

            #endregion

            var res = manager.PayForOrder(orderID, customerID, amount, methodOfPayment, CreditCardNumber);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.PaymentNoification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications(new PaymentMessage()
                {
                    Content = "PaymentApproved",
                    PaymentID = res.ID,
                    PaymentRecieved = (res.Amount > 0)
                }, clientRegistrations);
            #endregion

            return res;

        }


        /// <summary>
        /// Refund by creating another payment item with negative amount.
        /// </summary>
        /// <param name="paymentID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Payment Refund(long paymentID, Guid customerID)
        {
            ICrmBase crmProx = null;

            #region Validate Customer
            //Contact the Crm service to ensure the customer exist.
            lock (this)
            {
                if (crmChf == null)
                    crmChf = new ChannelFactory<ICrmBase>("CrmEP");
            }

            try
            {
                crmProx = crmChf.CreateChannel();
                var customer = crmProx.GetCustomerByID(customerID);
                if (customer == null)
                {
                    throw new NullReferenceException(StringsResource.NullCustomer);
                }

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new PaymentException(string.Format(StringsResource.CustomerNotFound, customerID), ex);
            }
            finally
            {
                var channel = crmProx as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }

            #endregion

            var res = manager.Refund(paymentID, customerID);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.PaymentNoification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications(
                    new PaymentMessage()
                    {
                        Content = "Refund was given",
                        PaymentID = res.ID,
                        Refund = true
                    }, clientRegistrations);

            #endregion

            return res;
        }

        /// <summary>
        /// Find payments items according to PaymentCriteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>

        public Payment[] FindPayments(PaymentCriteria criteria)
        {
            return manager.FindPayments(criteria);
        }

        /// <summary>
        /// Find a payment item according to its ID
        /// </summary>
        /// <param name="paymentID"></param>
        /// <returns></returns>
        public Payment FindPayment(long paymentID)
        {
            return manager.FindPayment(paymentID);
        }

        #endregion
    }
}
