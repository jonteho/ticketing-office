using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.ShowsService.Contracts
{
    /// <summary>
    /// Criteria information. The criteria is used to build the where statement for an EF query
    /// </summary>
    [DataContract(Namespace = "http://Fabrikam.com")]
    public class EventCriteria
    {
        [DataMember]
        public DateTime? Date { get; set; }
        [DataMember]
        public int? TheaterID { get; set; }
        [DataMember]
        public EventState? State { get; set; }
        [DataMember]
        public int? ShowID { get; set; }
        [DataMember]
        public DateTime? FromDate { get; set; }
        [DataMember]
        public DateTime? ToDate { get; set; }
    }
}
