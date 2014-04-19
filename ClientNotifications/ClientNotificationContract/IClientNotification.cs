using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace TicketingOffice.ClientNotification.Contract
{
    /// <summary>
    /// The contract implemented by the client to enable notifications reception
    /// </summary>
    [ServiceContract(Namespace = @"http://Fabrikam.com")]
    public interface IClientNotification
    {
        [OperationContract(IsOneWay=true)]
        void MessageArrived(TicketingMessage message);
    }

    /// <summary>
    /// Register a client for notifications
    /// </summary>
    [ServiceContract(Namespace = @"http://Fabrikam.com")]        
    public interface INotificationManager
    {       
        [OperationContract(IsOneWay=true)]
        void Register(string clientUri, NotificationTypes notificationType);       
    }

    /// <summary>
    /// Register a client for notifications using a duplex channel
    /// </summary>
    [ServiceContract(Namespace = @"http://Fabrikam.com",
        CallbackContract=typeof(IClientNotification))]
    public interface IRegisterForDuplexNotification
    {
        [OperationContract(IsOneWay = true)]
        void RegisterCallback(NotificationTypes notificationType);
    }


    
}
