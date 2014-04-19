using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.TicketingService.Contracts
{
    /// <summary>
    /// The customer entity. It is a replica of the customer defined in the CRM service
    /// </summary>
    [DataContract(Namespace = "http://Fabrikam.com")]
    public class Customer
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string CellNumber { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public DateTime? BirthDate { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public int? ReductionCode { get; set; }
       

        public Customer() { }
      
    }
}
