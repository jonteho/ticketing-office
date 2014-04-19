using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.PricingRules.Contracts
{
    /// <summary>
    /// Criteria information. The criteria is used to build the where statement for an EF query
    /// </summary>
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public class RulesCriteria
    {
        [DataMember]
        public DateTime? FromDate { get; set; }
        [DataMember]
        public DateTime? ToDate { get; set; }
        [DataMember]
        public string Scope { get; set; }
        [DataMember]
        public int? MinNumOfOrdes { get; set; }
    }
}
