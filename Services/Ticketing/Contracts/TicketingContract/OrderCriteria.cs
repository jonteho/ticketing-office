using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TicketingOffice.ShowsService.Contracts;
using TicketingOffice.PaymentService.Contracts;


namespace TicketingOffice.TicketingService.Contracts
{
    /// <summary>
    /// Criteria information. The criteria is used to build the where statement for an EF query.
    /// </summary>
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public class OrderCriteria
    {
        [DataMember]
        public DateTime? Date { get; set; }
        [DataMember]
        public Guid? CustomerID { get; set; }
        [DataMember]
        public DateTime? FromDate { get; set; }
        [DataMember]
        public DateTime? ToDate { get; set; }
        [DataMember]
        public Guid? EventID { get; set; }
    }
}