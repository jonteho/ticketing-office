using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TicketingOffice.CrmService.Contracts
{
    /// <summary>
    /// Criteria information. The criteria is used to build the where statement for an EF query
    /// </summary>
    [DataContract(Namespace = "http://Fabrikam.com")]
    public class CustomerCriteria
    {
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public DateTime? BirthDate { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string CellNumber { get; set; }
        [DataMember]
        public int? ReductionCode { get; set; }



    }
}
