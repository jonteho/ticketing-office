using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.ClientNotification.Contract;

namespace TicketingOffice.ClientNotification
{
    /// <summary>
    /// A base class for services that enrich the service with client notification capabilities
    /// </summary>
    public abstract class ServiceBase
    {
        ClientNotificationsManager registrationManager = new ClientNotificationsManager();

        #region INotificationManager Members

        public void Register(string clientUri, NotificationTypes notificationType)
        {
            registrationManager.Register(clientUri, notificationType);
        }

        #endregion

        #region IRegisterForDuplexNotification Members

        public void RegisterCallback(NotificationTypes notificationType)
        {
            registrationManager.RegisterCallback(notificationType);
        }

        #endregion
    }
}
