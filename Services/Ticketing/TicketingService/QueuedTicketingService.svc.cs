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
    /// One way facade for the ticketing service
    /// </summary>
    // TODO: Ex3 - Decorate the QueuedTicketingService service with a ServiceBehavior attribute
    [ServiceBehavior(TransactionAutoCompleteOnSessionClose = true)]
    public class QueuedTicketingService : ITicketingServiceOneWay
    {
        GeneralTicketingService generalTicketing = new GeneralTicketingService();
        ITicketingCallBack callback_prox;

        #region ITicketingServiceOneWay Members

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void OrderTicket(Order newOrder, SeatIndex[] seats, Guid callID)
        {
            try
            {
                callback_prox = CallBackChannelFactory.GetProxy(false);
                Guid result;

                // Do the job and call back the ticketing bridge
                result = generalTicketing.OrderTicket(newOrder, seats);

                //Call back the bridge
                callback_prox.IDArrived(result, callID);
            }
            catch (TicketingException tex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.TicketingFailed + " " + tex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + tex.Message, tex);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketingBridge + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((callback_prox != null) && ((callback_prox as ICommunicationObject).State == CommunicationState.Opened))
                    (callback_prox as ICommunicationObject).Close();
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void PayForTicket(Guid orderID, Guid payingCustomerID, double amount, PaymentType methodOfPayment, Currencies? currency, Guid callID, string creditCard)
        {
            try
            {
                callback_prox = CallBackChannelFactory.GetProxy(false);
                Payment payment = null;

                // Do the job and call back the ticketing bridge
                payment = generalTicketing.PayForTicket(orderID, payingCustomerID, amount, methodOfPayment, currency, creditCard);


                callback_prox.PaymentArrived(payment, callID);
            }
            catch (TicketingException tex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.TicketingFailed + " " + tex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + tex.Message, tex);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketingBridge + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((callback_prox != null) && ((callback_prox as ICommunicationObject).State == CommunicationState.Opened))
                    (callback_prox as ICommunicationObject).Close();
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void CancelTicket(Guid orderID, Guid payingCustomerID, Guid callID, string creditCard)
        {
            try
            {
                callback_prox = CallBackChannelFactory.GetProxy(false);
                Payment payment = null;

                // Do the job and call back the ticketing bridge
                generalTicketing.CancelTicket(orderID, payingCustomerID, creditCard);

                callback_prox.PaymentArrived(payment, callID);
            }
            catch (TicketingException tex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.TicketingFailed + " " + tex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + tex.Message, tex);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketingBridge + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((callback_prox != null) && ((callback_prox as ICommunicationObject).State == CommunicationState.Opened))
                    (callback_prox as ICommunicationObject).Close();
            }

        }

        public void FindOrder(Guid orderID, Guid callID)
        {
            try
            {
                callback_prox = CallBackChannelFactory.GetProxy(false);
                // Do the job and call back the ticketing bridge
                var order = generalTicketing.FindOrder(orderID);
                callback_prox.OrderArrived(order, callID);
            }
            catch (TicketingException tex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.TicketingFailed + " " + tex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + tex.Message, tex);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketingBridge + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((callback_prox != null) && ((callback_prox as ICommunicationObject).State == CommunicationState.Opened))
                    (callback_prox as ICommunicationObject).Close();
            }
        }

        public void FindOrders(OrderCriteria criteria, Guid callID)
        {

            try
            {
                callback_prox = CallBackChannelFactory.GetProxy(false);
                // Do the job and call back the ticketing bridge
                callback_prox.OrdersArrived(generalTicketing.FindOrders(criteria), callID);
            }
            catch (TicketingException tex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.TicketingFailed + " " + tex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + tex.Message, tex);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketingBridge + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((callback_prox != null) && ((callback_prox as ICommunicationObject).State == CommunicationState.Opened))
                    (callback_prox as ICommunicationObject).Close();
            }
        }

        public void FindBestCustomersIds(int numberOfCustomers, Guid callID)
        {
            try
            {
                callback_prox = CallBackChannelFactory.GetProxy(false);
                // Do the job and call back the ticketing bridge
                callback_prox.IDsArrived(generalTicketing.FindBestCustomersIds(numberOfCustomers), callID);
            }
            catch (TicketingException tex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.TicketingFailed + " " + tex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + tex.Message, tex);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketingBridge + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((callback_prox != null) && ((callback_prox as ICommunicationObject).State == CommunicationState.Opened))
                    (callback_prox as ICommunicationObject).Close();
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public void PrintTicket(Guid orderID, Guid callID)
        {
            try
            {
                callback_prox = CallBackChannelFactory.GetProxy(false);
                // Do the job and call back the ticketing bridge
                callback_prox.MessageArrived(generalTicketing.PrintTicket(orderID), callID);
            }
            catch (TicketingException tex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.TicketingFailed + " " + tex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + tex.Message, tex);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketingBridge + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((callback_prox != null) && ((callback_prox as ICommunicationObject).State == CommunicationState.Opened))
                    (callback_prox as ICommunicationObject).Close();
            }
        }

        #endregion


    }

}
