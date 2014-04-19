using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.HallState.Contracts
{
    /// <summary>
    /// Reserevation of a collection of seats in a certain event (taking place in a specific theater) to a particular client
    /// </summary>
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public class Reservation
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public Guid EventID { get; set; }
        [DataMember]
        public List<SeatIndex> Seats { get; set; }
        [DataMember]
        public Theater  TheaterInfo { get; set; }
        [DataMember]
        public Guid? CustomerID { get; set; }
        [DataMember]
        public string Remarks { get; set; }      
       
    }
}
