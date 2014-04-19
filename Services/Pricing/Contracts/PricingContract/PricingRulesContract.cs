using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TicketingOffice.PricingRules.Contracts
{
    /// <summary>
    /// Exposes CRUD operations for the pricing rules
    /// </summary>
    [ServiceContract(Namespace = @"http://Fabrikam.com")]  
    public interface IPricingRulesService
    {
        [OperationContract]       
        PricingRule[] GetPricingRulesByCriteria(RulesCriteria criteria);

        [OperationContract]    
        PricingRule GetPricingruleByID(int RuleID);

        [OperationContract]
        int CreatePricingRule(PricingRule newRule);

        [OperationContract]
        void UpdatePricingRule(PricingRule newRule);

        [OperationContract]      
        void DeletePricingRule(int RuleID);
    }




}
