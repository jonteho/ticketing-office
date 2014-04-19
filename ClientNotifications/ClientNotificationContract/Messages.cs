using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TicketingOffice.ClientNotification.Contract
{
    /// <summary>
    /// The base message sent to the customer in the client notification channel.
    /// To enable serialization of types derrived from this base class KnownTypes where used.
    /// </summary>
    [KnownType(typeof(ShowMessage))]
    [KnownType(typeof(EventMessage))]
    [KnownType(typeof(OrderMessage))]
    [KnownType(typeof(PaymentMessage))]
    [KnownType(typeof(PricingRuleMessage))]
    [DataContract(Namespace = "http://Fabrikam.com")]
    public class TicketingMessage
    {
        [DataMember]
        public string From { get; set; }       
        [DataMember]
        public string Content { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public Type MessageType { get; set; }

        public TicketingMessage()
        {
            Date = DateTime.Now;
            if (OperationContext.Current != null)
                From = string.Format("ServiceName:{0} at {1}",
                                        OperationContext.Current.Host.Description.Name,
                                        OperationContext.Current.Host.ChannelDispatchers[0].Listener.Uri.AbsoluteUri);
        }
    }

    [DataContract(Namespace = "http://Fabrikam.com")]
    public class ShowMessage : TicketingMessage
    {
        [DataMember]
        public int ShowID { get; set; }
    }

    [DataContract(Namespace = "http://Fabrikam.com")]
    public class EventMessage : TicketingMessage
    {
        [DataMember]
        public Guid EventID { get; set; }       
    }
  
    [DataContract(Namespace = "http://Fabrikam.com")]
    public class OrderMessage : TicketingMessage
    {
        [DataMember]
        public Guid OrderID { get; set; }       
    }

    [DataContract(Namespace = "http://Fabrikam.com")]
    public class PaymentMessage : TicketingMessage
    {
        [DataMember]
        public long PaymentID { get; set; }
        [DataMember]
        public bool PaymentRecieved { get; set; }
        [DataMember]
        public bool Refund { get; set; }
    }

    [DataContract(Namespace = "http://Fabrikam.com")]
    public class PricingRuleMessage : TicketingMessage
    {
        [DataMember]
        public int RuleID { get; set; }
        [DataMember]
        public string  Scope { get; set; }
       
    }

    [DataContract(Namespace = "http://Fabrikam.com")]
    public class CrmMessage : TicketingMessage
    {
        public Guid ClientID { get; set; }
    }


    /// <summary>
    /// The type of notification the client wants to register to.
    /// </summary>
    [DataContract(Namespace = "http://Fabrikam.com")]
    public enum NotificationTypes
    {
        [EnumMember]
        TicketingNotification,
        [EnumMember]
        ShowNotification,
        [EnumMember]
        EventsNoification,
        [EnumMember]       
        PaymentNoification,        
        [EnumMember]
        CustomerNoification
    }

}
