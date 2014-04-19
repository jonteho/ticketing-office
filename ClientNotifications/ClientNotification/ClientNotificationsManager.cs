using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Properties;
using TicketingOffice.ClientNotification.Contract;

namespace TicketingOffice.ClientNotification
{
    /// <summary>
    /// Manges registration and sends callback to the registered clients.
    /// </summary>
    public class ClientNotificationsManager : INotificationManager, IRegisterForDuplexNotification
    {
       #region IRegisterForNotification Members
        static ChannelFactory<IClientNotification> chf;

        /// <summary>
        /// Register a one-way channel information
        /// </summary>
        /// <param name="clientUri"></param>
        /// <param name="notificationType"></param>
        public void Register(string clientUri, NotificationTypes notificationType)
        {
            string userName = "Anonymous";

            if ((OperationContext.Current != null) &&
                (OperationContext.Current.ServiceSecurityContext != null) &&
                (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
            {
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;               
            }
            MemoryRepository.Current.RegisterClient(new Uri(clientUri), notificationType, userName, null);
         }

       #endregion


        #region IRegisterForDuplexNotification Members

        /// <summary>
        /// Register a duplex callback channel
        /// </summary>
        /// <param name="notificationType"></param>
        public void RegisterCallback(NotificationTypes notificationType)
        {
            string userName = "Anonymous";

            if ((OperationContext.Current != null) &&
                (OperationContext.Current.ServiceSecurityContext != null) &&
                (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
            {
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            }

          //  var dchf = new DuplexChannelFactory<IClientNotification>(OperationContext.Current.InstanceContext);
           // IClientNotification prox = dchf.CreateChannel();
            IClientNotification prox = OperationContext.Current.GetCallbackChannel<IClientNotification>();
            MemoryRepository.Current.RegisterClient(null , notificationType, userName, prox);
        }

        #endregion


        /// <summary>
        /// Send notifications to registered clients.
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="address">The client's address </param>
        public static void SendNotifications(TicketingMessage message, Registration[] address)
        {
            //Notifications are sent on another thread.
            foreach (var add in address)
            {
                Action<TicketingMessage, Registration> del = SendMessage;
                del.BeginInvoke(message, add, null, null);
            }

        }

        /// <summary>
        /// Send a message back to the client. 
        /// First try a duplex callcack channel, then try a one-way channel back to the client.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clientRegistration"></param>
        private static void SendMessage(TicketingMessage message,Registration clientRegistration)
        {
            IClientNotification prox = null;
            try
            {
                // Create a proxy for duplex channel
                if ((clientRegistration.clientProxy != null) &&
                    ((clientRegistration.clientProxy as IClientChannel).State == CommunicationState.Opened))
                    prox = clientRegistration.clientProxy;
                else
                {
                    //Create a proxy for oneway channel
                    if (chf == null)
                        chf = new ChannelFactory<IClientNotification>(new BasicHttpBinding());

                    prox = chf.CreateChannel(new EndpointAddress(clientRegistration.RegistrationUri));
                }

                //send the message to the client
                prox.MessageArrived(message);                
            }
            catch (Exception ex)
            {
                 LoggingManager.Logger.Log(LoggingCategory.Error, string.Format(StringsResource.FailedToContactClient,ex.Message));                
            }
            finally
            {
               var channel = prox as ICommunicationObject;
               if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
             }
           }
     

      
    }
}
