using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Runtime.Serialization;
using TicketingOffice.PricingRules.Contracts;
using TicketingOffice.Common.Properties;
using TicketingOffice.Pricing.Contracts;

namespace TicketingOffice.PricingService.BusinessLogic
{
    /// <summary>
    /// Execute the pricing logic
    /// </summary>
    public class PricingManager : IPricingManager
    {

        #region IPricingManager Members
        /// <summary>
        /// Calculate the final price
        /// </summary>
        /// <param name="rules">Pricing rules to use</param>
        /// <param name="listPrice">The list price to start with</param>
        /// <returns></returns>
        public double CalculatePrice(PricingRule[] rules, double listPrice)
        {
            //1. Find the best pricing rule. Only one rule can apply
            PricingRule effectiveRule = (from rl in rules
                                             orderby rl.Reduction descending
                                             select rl).FirstOrDefault();
            if (effectiveRule == null)
                return listPrice;

            //simple validation on the reduction
            if ((effectiveRule.Reduction > 99) || (effectiveRule.ToDate < DateTime.Today))
                throw new PricingException(StringsResource.InvalidPricingRule);

            //The final calculation
            return (listPrice * (100 - effectiveRule.Reduction)) / 100;

        }

        #endregion
    }
}
