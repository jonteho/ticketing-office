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
    public class ShowCriteria
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Cast { get; set; }
        [DataMember]
        public int? Duration { get; set; }   
        [DataMember]
        public string Category { get; set; }
    }
}
