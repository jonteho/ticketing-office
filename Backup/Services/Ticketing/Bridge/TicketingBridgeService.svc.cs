using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.TicketingService.Contracts;
using TicketingOffice.HallState.Contracts;
using TicketingOffice.PaymentService.Contracts;
using TicketingOffice.Common.Helpers;
using System.Threading;
using TicketingOffice.Common.Properties;
using TicketingOffice.CurrencyExchange.Contract;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;

namespace TicketingOffice.Bridge
{
    /// <summary>
    /// First Option for an http bridge implementation.
    /// This bridge is simple. It exposes the request response ticketing contract.
    /// The bridge forwards the call by calling the one way ticketing endpoint.
    /// The bridge exposes a callback endpoint with which it will get the ticketing results from the service.
    /// When the result is retrieved the bridge will return it to the original client.
    /// </summary>

    // TODO: Ex5 - Change the concurrency and instancing mode of the TicketingBridge service
    [ServiceBehavior(ReleaseServiceInstanceOnTransactionComplete = false,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class TicketingBridge : ITicketingService
    {
        // Wait for results Timeout
        TimeSpan timeout = TimeSpan.FromMinutes(10);

        //Call the ticketing service using a one way channel
        #region ITicketingService Members

        [OperationBehavior(TransactionScopeRequired = true)]
        public Guid OrderTicket(Order newOrder, SeatIndex[] seats)
        {
            ITicketingServiceOneWay prox = null;
            Guid callId = Guid.NewGuid();
            try
            {
                // TODO: Ex2 - Add code to call the one-way service
                // Find a proxy to the ticketing service (one-way)
                prox = TicketingServiceOneWayProxyFactory.GetProxy(false);
                AutoResetEvent arrived = new AutoResetEvent(false);

                // Create a ResultPackage with a wait handle to wait on until a response arrives (on another channel)
                ResultPackage pack = new ResultPackage() { ResultArrived = arrived };
                ResultsCache.Current.SetPackage(callId, pack);

                // Call the pricing service on MSMQ channel on another thread.
                Action<ITicketingServiceOneWay> del =
                    (p => p.OrderTicket(newOrder, seats, callId));
                del.BeginInvoke(prox, null, null);
                //Wait until result arrives
                arrived.WaitOne(timeout);
                Guid result = (Guid)ResultsCache.Current.GetResult(callId);
                ResultsCache.Current.ClearResult(callId);
                return result;

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketing + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((prox != null) && ((prox as ICommunicationObject).State == CommunicationState.Opened))
                    (prox as ICommunicationObject).Close();
            }

        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Payment PayForTicket(Guid orderID, Guid payingCustomerID, double amount, PaymentType methodOfPayment, Currencies? currency, string creditCard)
        {
            ITicketingServiceOneWay prox = null;
            Guid callId = Guid.NewGuid();
            try
            {
                // Find a proxy to the ticketing service (one way)
                prox = TicketingServiceOneWayProxyFactory.GetProxy(false);
                AutoResetEvent arrived = new AutoResetEvent(false);
                // Create a ResultPackage with a wait handle to wait on until a response arrives (on another channel)
                ResultPackage pack = new ResultPackage() { ResultArrived = arrived };
                ResultsCache.Current.SetPackage(callId, pack);

                // Call the ticketing service via MSMQ channel on another thread.
                Action<ITicketingServiceOneWay> del = (p => p.PayForTicket(orderID, payingCustomerID, amount, methodOfPayment, currency, callId, creditCard));
                del.BeginInvoke(prox, null, null);

                //Wait until result arrives
                arrived.WaitOne(timeout);
                Payment result = (Payment)ResultsCache.Current.GetResult(callId);
                ResultsCache.Current.ClearResult(callId);
                return result;
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketing + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((prox != null) && ((prox as ICommunicationObject).State == CommunicationState.Opened))
                    (prox as ICommunicationObject).Close();
            }
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Payment CancelTicket(Guid orderID, Guid payingCustomerID, string creditCard)
        {
            ITicketingServiceOneWay prox = null;
            Guid callId = Guid.NewGuid();
            try
            {
                // Find a proxy to the ticketing service (one way)
                prox = TicketingServiceOneWayProxyFactory.GetProxy(false);
                AutoResetEvent arrived = new AutoResetEvent(false);
                // Create a ResultPackage with a wait handle to wait on until a response arrives (on another channel)
                ResultPackage pack = new ResultPackage() { ResultArrived = arrived };
                ResultsCache.Current.SetPackage(callId, pack);

                // Call the ticketing service via MSMQ channel on another thread.
                Action<ITicketingServiceOneWay> del = (p => p.CancelTicket(orderID, payingCustomerID, callId, creditCard));
                del.BeginInvoke(prox, null, null);


                //Wait until result arrives
                arrived.WaitOne(timeout);
                Payment result = (Payment)ResultsCache.Current.GetResult(callId);
                ResultsCache.Current.ClearResult(callId);
                return result;
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketing + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((prox != null) && ((prox as ICommunicationObject).State == CommunicationState.Opened))
                    (prox as ICommunicationObject).Close();
            }
        }


        public Order FindOrder(Guid orderID)
        {
            ITicketingServiceOneWay prox = null;
            Guid callId = Guid.NewGuid();
            try
            {
                // Find a proxy to the ticketing service (one way)
                prox = TicketingServiceOneWayProxyFactory.GetProxy(false);
                AutoResetEvent arrived = new AutoResetEvent(false);
                // Create a ResultPackage with a wait handle to wait on until a response arrives (on another channel)
                ResultPackage pack = new ResultPackage() { ResultArrived = arrived };
                ResultsCache.Current.SetPackage(callId, pack);

                // Call the ticketing service via MSMQ channel on another thread.
                Action<ITicketingServiceOneWay> del = (p => p.FindOrder(orderID, callId));
                del.BeginInvoke(prox, null, null);

                //Wait until result arrives
                arrived.WaitOne();
                Order result = (Order)ResultsCache.Current.GetResult(callId);
                ResultsCache.Current.ClearResult(callId);
                return result;
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketing + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((prox != null) && ((prox as ICommunicationObject).State == CommunicationState.Opened))
                    (prox as ICommunicationObject).Close();
            }
        }

        public Order[] FindOrders(OrderCriteria criteria)
        {
            ITicketingServiceOneWay prox = null;
            Guid callId = Guid.NewGuid();
            try
            {
                // Find a proxy to the ticketing service (one way)
                prox = TicketingServiceOneWayProxyFactory.GetProxy(false);
                AutoResetEvent arrived = new AutoResetEvent(false);
                // Create a ResultPackage with a wait handle to wait on until a response arrives (on another channel)
                ResultPackage pack = new ResultPackage() { ResultArrived = arrived };
                ResultsCache.Current.SetPackage(callId, pack);

                // Call the ticketing service via MSMQ channel on another thread.
                Action<ITicketingServiceOneWay> del = (p => p.FindOrders(criteria, callId));
                del.BeginInvoke(prox, null, null);

                //Wait until result arrives
                arrived.WaitOne(timeout);
                Order[] result = ResultsCache.Current.GetResult(callId) as Order[];
                ResultsCache.Current.ClearResult(callId);
                return result;
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketing + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((prox != null) && ((prox as ICommunicationObject).State == CommunicationState.Opened))
                    (prox as ICommunicationObject).Close();
            }
        }

        public Guid[] FindBestCustomersIds(int numberOfCustomers)
        {
            ITicketingServiceOneWay prox = null;
            Guid callId = Guid.NewGuid();
            try
            {
                // Find a proxy to the ticketing service (one way)
                prox = TicketingServiceOneWayProxyFactory.GetProxy(false);
                AutoResetEvent arrived = new AutoResetEvent(false);
                // Create a ResultPackage with a wait handle to wait on until a response arrives (on another channel)
                ResultPackage pack = new ResultPackage() { ResultArrived = arrived };
                ResultsCache.Current.SetPackage(callId, pack);

                // Call the ticketing service via MSMQ channel on another thread.
                Action<ITicketingServiceOneWay> del = (p => p.FindBestCustomersIds(numberOfCustomers, callId));
                del.BeginInvoke(prox, null, null);

                //Wait until result arrives
                arrived.WaitOne(timeout);
                Guid[] result = ResultsCache.Current.GetResult(callId) as Guid[];
                ResultsCache.Current.ClearResult(callId);
                return result;
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketing + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((prox != null) && ((prox as ICommunicationObject).State == CommunicationState.Opened))
                    (prox as ICommunicationObject).Close();
            }
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public string PrintTicket(Guid orderID)
        {
            ITicketingServiceOneWay prox = null;
            Guid callId = Guid.NewGuid();
            try
            {
                // Find a proxy to the ticketing service (one way)
                prox = TicketingServiceOneWayProxyFactory.GetProxy(false);
                AutoResetEvent arrived = new AutoResetEvent(false);
                // Create a ResultPackage with a wait handle to wait on until a response arrives (on another channel)
                ResultPackage pack = new ResultPackage() { ResultArrived = arrived };
                ResultsCache.Current.SetPackage(callId, pack);

                // Call the ticketing service via MSMQ channel on another thread.
                Action<ITicketingServiceOneWay> del = (p => p.PrintTicket(orderID, callId));
                del.BeginInvoke(prox, null, null);

                //Wait until result arrives
                arrived.WaitOne(timeout);
                string result = ResultsCache.Current.GetResult(callId).ToString();
                ResultsCache.Current.ClearResult(callId);
                return result;
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactTicketing + " " + ex.Message);
                throw new TicketingException(StringsResource.TicketingFailed + " " + ex.Message, ex);
            }
            finally
            {
                if ((prox != null) && ((prox as ICommunicationObject).State == CommunicationState.Opened))
                    (prox as ICommunicationObject).Close();
            }
        }

        #endregion
    }




    /// <summary>
    /// Helper class that returns a proxy to the ticketing service (one way)
    /// </summary>
    internal class TicketingServiceOneWayProxyFactory
    {
        private static ChannelFactory<ITicketingServiceOneWay> _chf;

        internal static ITicketingServiceOneWay GetProxy(bool discovery)
        {
            if (_chf == null)
            {
                lock (typeof(TicketingServiceOneWayProxyFactory))
                {
                    if (_chf == null)
                        _chf = new ChannelFactory<ITicketingServiceOneWay>("QueuedTicketingEP");
                }
            }

            if (discovery)
            {
                // TODO: Ex6 - Use a dynamic endpoint to find the ticketing service
                DynamicEndpoint dynamicEndpoint = new
                   DynamicEndpoint(ContractDescription.GetContract(
                    typeof(ITicketingServiceOneWay)),
                      new NetMsmqBinding());
                return _chf.CreateChannel(dynamicEndpoint.Address);
            }
            else
            {
                return _chf.CreateChannel();
            }
        }

    }
}
