using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.PricingRules.Contracts
{
    /// <summary>
    /// A pricing rules store information required to calculate the final price of a certain event.
    /// Each event has a list-price to begin with but different kinds of customers are entitled to get some reductions.
    /// The price rules holds the information required to calculate that reduction. 
    /// </summary>
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public class PricingRule
    {
        [DataMember]
        public int RuleID { get; set; }
        /// <summary>
        /// Pricing rules are grouped in policies
        /// </summary>
        [DataMember]
        public string PolicyName { get; set; }
        /// <summary>
        /// The actual reduction
        /// </summary>
        [DataMember]
        public byte Reduction { get; set; }
        /// <summary>
        /// Code given by the customer profile. This code is used to classify customers to get reductions. 
        /// </summary>
        [DataMember]
        public int? ReductionCode { get; set; }
        /// <summary>
        /// Date the rule is valid
        /// </summary>
        [DataMember]
        public DateTime? FromDate { get; set; }
        /// <summary>
        /// Date the rule is valid
        /// </summary>
        [DataMember]
        public DateTime? ToDate { get; set; }
        /// <summary>
        /// Minimum of tickets to make the rule valid.
        /// </summary>
        [DataMember]
        public int? MinNumOfOrders { get; set; }
    }
}
