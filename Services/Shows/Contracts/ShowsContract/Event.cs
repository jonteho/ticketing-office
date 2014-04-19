using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.ShowsService.Contracts
{
    /// <summary>
    /// Ticketing Event is a particular presentation of a certain show.
    /// </summary>
    [DataContract(Namespace = "http://Fabrikam.com")]
    public class Event
    {
        [DataMember]
        public Guid EventID { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public double ListPrice { get; set; }
        [DataMember]
        public Show ShowDetails;
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public EventState State { get; set; }
        [DataMember]
        public int TheaterID { get; set; }   
        [DataMember]
        public string PricingPolicy { get; set; }
    }


    /// <summary>
    /// The state in which a ticketing event can be in
    /// </summary>
    [DataContract(Namespace = "http://Fabrikam.com")]
    public enum EventState
    {
        /// <summary>
        /// The event was created not opened yet for sale.
        /// </summary>
         [EnumMember]
        NotOpened,
        /// <summary>
        /// The event is open for sale
        /// </summary>
         [EnumMember]
        Opened,
        /// <summary>
        /// The event is closed for sale
        /// </summary>
         [EnumMember]
        Closed,
        /// <summary>
        /// The event was canceled
        /// </summary>
         [EnumMember]
        Canceled 
    }
}
