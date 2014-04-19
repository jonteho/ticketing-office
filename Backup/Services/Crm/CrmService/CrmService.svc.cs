using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.CrmService.Contracts;
using TicketingOffice.CrmService.CrmLogic;
using TicketingOffice.Common.Properties;
using System.Security;
using System.Security.Principal;
using System.Security.Permissions;
using TicketingOffice.TicketingService.Contracts;
using TicketingOffice.Common.Helpers;
using TicketingOffice.ClientNotification.Contract;
using TicketingOffice.ClientNotification;
using System.Diagnostics;

namespace TicketingOffice.CrmService
{
    /// <summary>
    /// A service for all the customer related operations
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class CustomerRelationsService : ServiceBase, 
                                            ICrmService, IPrivateCrm, 
                                            INotificationManager, IRegisterForDuplexNotification
    {
        ICrmManager Manager;
        ChannelFactory<ITicketingService> chf;


        public CustomerRelationsService()
        {
            Manager = new CrmManager();
        }

        private void LogUser()
        {
              
        }

         //Thses operations can be execited only by an employee or a manager of the ticketing office
        #region ICrmService Members

          
        public Contracts.Customer[] FindCustomersByCriteria(CustomerCriteria criteria)
        {
            LogUser();
            return Manager.FindCustomers(criteria);
        }

        /// <summary>
        /// Search customer by the Guid ID. This method will be called by other services thus there is no pricipal permission demand
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Contracts.Customer GetCustomerByID(Guid customerID)
        {
            LogUser();
            return Manager.FindCustomer(customerID);
        }

          
        public void UpdateCustomer(Contracts.Customer newCustomer)
        {
            if (newCustomer == null)
                throw new ArgumentNullException(StringsResource.InvalidCustomer);
            LogUser();
            Manager.UpdateCustomer(newCustomer);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.CustomerNoification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications
                    (new CrmMessage() { Content = "User Updated", ClientID = newCustomer.ID }, clientRegistrations);
            #endregion
        }

          
        public Guid CreateCustomer(Contracts.Customer newCustomer)
        {
            if (newCustomer == null)
                throw new ArgumentNullException(StringsResource.InvalidCustomer);

            LogUser();

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.CustomerNoification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications
                    (new CrmMessage() { Content = "User Created", ClientID = newCustomer.ID }, clientRegistrations);
            #endregion
           
          
            return Manager.CreateCustomer(newCustomer);
        }

          
        public void DeleteCustomer(Guid customerID)
        {
            LogUser();

              

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.CustomerNoification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications
                    (new CrmMessage() { Content = "User Deleted", ClientID = customerID }, clientRegistrations);
            #endregion
        }

          
        public Order[] GetCustomerOrders(Guid customerID, DateTime? fromDate, DateTime? toDate)
        {
            LogUser();
            return InnerGetCustomerOrders(customerID, fromDate, toDate);
        }

          
        public Contracts.Customer FindCustomerByOrder(Guid orderID)
        {

            Order order;
            ITicketingService prox = null;

            LogUser();
            lock (this)
            {
                if (chf == null)
                    chf = new ChannelFactory<ITicketingService>("TicketingEP");
            }
            try
            {
                prox = chf.CreateChannel();
                order = prox.FindOrder(orderID);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new CrmException(StringsResource.FailedToContactTicketing + " " + ex.Message, ex);
            }
            finally
            {
                var channel = prox as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }

            if (order == null)
                throw new FaultException<CrmException>(
                    new CrmException(string.Format(StringsResource.OrderNotFound, orderID)));

            return Manager.FindCustomer(order.CustomerInfo.ID);
        }

          
        public Contracts.Customer[] FindBestCustomers(int numberOfCustomers)
        {
            Guid[] ids;
            ITicketingService prox = null;

            LogUser();
            lock (this)
            {
                if (chf == null)
                    chf = new ChannelFactory<ITicketingService>("TicketingEP");
            }

            try
            {
                prox = chf.CreateChannel();
                ids = prox.FindBestCustomersIds(numberOfCustomers);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new CrmException(StringsResource.FailedToContactTicketing + " " + ex.Message, ex);
            }
            finally
            {
                var channel = prox as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
            return ids.Select(id => Manager.FindCustomer(id)).ToArray();
        }

        #endregion

         //Operations for a registered user
        #region IPrivateCrm Members

        public Order[] GetMyOrders(string email, string cellNumber, DateTime? fromDate, DateTime? toDate)
        {
            Guid customerID = GetCustomerID(email, cellNumber);

            return InnerGetCustomerOrders(customerID, fromDate, toDate);
        }

        public Contracts.Customer GetMyDetails(string email, string cellNumber)
        {
            Guid customerID = GetCustomerID(email, cellNumber);
            return Manager.FindCustomer(customerID);
        }

        public void ChangeMyDetails(string email, string cellNumber, Contracts.Customer newCustomer)
        {
            Guid customerID = GetCustomerID(email, cellNumber);
            newCustomer.ID = customerID;

            Manager.UpdateCustomer(newCustomer);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.CustomerNoification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications
                     (new CrmMessage() { Content = "User Updated", ClientID = newCustomer.ID }, clientRegistrations);
            #endregion
        }

        #endregion

        #region Inner Methods

        /// <summary>
        /// Helper: Fetch the customer ID
        /// </summary>
        /// <param name="email"></param>
        /// <param name="cellNumber"></param>
        /// <returns></returns>
        private Guid GetCustomerID(string email, string cellNumber)
        {
            try
            {
                var username = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                var customers = Manager.FindCustomers(new CustomerCriteria() { Email = email, CellNumber = cellNumber });
                if (customers.Count() == 0)
                    throw new CrmException(string.Format(StringsResource.CustomerNotFound, email));
                if (customers.Count() > 1)
                    throw new CrmException(StringsResource.MoreThanOneCustomerWasFound);
               
                return  customers[0].ID;
        	}
        	catch (Exception ex)
        	{
        		LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw  new CrmException(StringsResource.CustomerIDNotFound, ex);
          	}  

        }

        /// <summary>
        /// Helper: Call the ticketing service to get order information for the particular customer
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private Order[] InnerGetCustomerOrders(Guid customerID, DateTime? fromDate, DateTime? toDate)
        {
            Order[] result;
            ITicketingService prox = null;
            lock (this)
            {
                if (chf == null)
                    chf = new ChannelFactory<ITicketingService>("TicketingEP");
            }

            try
            {
                prox = chf.CreateChannel();
                result = prox.FindOrders(new OrderCriteria()
                {
                    CustomerID = customerID,
                    FromDate = fromDate,
                    ToDate = toDate
                });

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new CrmException(StringsResource.FailedToContactTicketing + " " + ex.Message, ex);
            }
            finally
            {
                var channel = prox as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }


            return result;
        }

        #endregion
    }
}
