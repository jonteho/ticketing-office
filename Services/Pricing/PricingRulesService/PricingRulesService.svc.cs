using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.PricingRules.Contracts;
using TicketingOffice.Common.Properties;


namespace TicketingOffice.PricingRulesService
{

    /// <summary>
    /// A simple service that exposes CRUD operations for the pricing rules.
    /// </summary>
    public class TicketingPricingRulesService : IPricingRulesService
    {

        PricingRules.DataAccess.PricingRulesDal dal = new PricingRules.DataAccess.PricingRulesDal();

        #region IPricingRulesContract Members

        public PricingRule[] GetPricingRulesByCriteria(RulesCriteria criteria)
        {
            return dal.GetRulesByCriteria(criteria);
        }

        public PricingRule GetPricingruleByID(int RuleID)
        {
            return dal.GetEntity(RuleID);
        }

        public int CreatePricingRule(PricingRule newRule)
        {
            if ((newRule.ToDate < DateTime.Now) || (newRule.Reduction > 100))
                throw new PricingRuleException(StringsResource.InvalidPricingRule);

            return dal.CreateEntity(newRule);
        }

        public void UpdatePricingRule(PricingRule newRule)
        {
            if ((newRule.ToDate < DateTime.Now) || (newRule.Reduction > 100))
                throw new PricingRuleException(StringsResource.InvalidPricingRule);

            dal.UpdateEntity(newRule);

        }

        public void DeletePricingRule(int ruleID)
        {
            dal.DeleteEntity(ruleID);
        }

        #endregion
    }
}
