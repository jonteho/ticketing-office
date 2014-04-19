using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.HallState.Contracts
{
    /// <summary>
    /// Criteria information. The criteria is used to build the where statement for an EF query
    /// </summary>
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public class ReservationCriteria
    {
        [DataMember]
        public SeatIndex[] Seats { get; set; }
        [DataMember]
        public Guid? EventID { get; set; }
        [DataMember]
        public int? HallID { get; set; }
        [DataMember]
        public Guid? CustomerID { get; set; }
    }
}
