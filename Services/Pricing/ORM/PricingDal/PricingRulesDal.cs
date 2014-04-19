using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.Common;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Exceptions;
using TicketingOffice.Common.Properties;

namespace TicketingOffice.PricingRules.DataAccess
{
    /// <summary>
    /// The data access layer for managing Pricing rules. 
    /// The PricingRulesDal exposes pricing rules as defined in the contracts assembly.
    /// The dal is based on Entity Framework
    /// </summary>
    public class PricingRulesDal : IEntityCRUD<Contracts.PricingRule,int>
    {
        /// <summary>
        /// Retrieve a collection of pricing rules according to criteria parameter.
        /// The criteria is used to create the where statement fot the internal EF query. 
        /// </summary>
        /// <param name="criteria">The "where" criteria to retrieve entities</param>
        /// <returns></returns>
        public Contracts.PricingRule[] GetRulesByCriteria(Contracts.RulesCriteria criteria)
        {
            Contracts.PricingRule[] res;          

            try
            {
                using (TicketingOfficePricingEntities ctx = new TicketingOfficePricingEntities())
                {
                    IQueryable<PricingRule> query = ctx.PricingRules;
                    if (criteria.Scope != null)
                        query = query.Where(rule => rule.PolicyName == criteria.Scope);
                    if (criteria.MinNumOfOrdes != null)
                        query = query.Where(rule => rule.MinNumOfOrders <= criteria.MinNumOfOrdes);
                    if ((criteria.FromDate != null) && (criteria.ToDate != null))
                        query = query.Where(rule => ((rule.FromDate <= criteria.FromDate) &&
                                                        (rule.ToDate >= criteria.ToDate)));

                    res = query.Select(rule => new Contracts.PricingRule()
                                                    {
                                                        FromDate = rule.FromDate,
                                                        ToDate = rule.ToDate,
                                                        PolicyName = rule.PolicyName,
                                                        Reduction = rule.Reduction,
                                                        ReductionCode = rule.ReductionCode,
                                                        MinNumOfOrders = rule.MinNumOfOrders,
                                                        RuleID = rule.RuleID
                                                    }).ToArray();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "PricingRule", ex.Message), ex) { EntityType = typeof(PricingRule) };
            }
            return res;

        }


       #region IEntityCRUD<PricingRule,int> Members

        /// <summary>
        /// Get a prcing rule according to its ID
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public Contracts.PricingRule GetEntity(int entityID)
        {
            Contracts.PricingRule res;
            try 
        	{	        
        		using (TicketingOfficePricingEntities ctx = new TicketingOfficePricingEntities())
                {
                    var q = from rule in ctx.PricingRules
                            where rule.RuleID == entityID
                            select rule;
                    res = q.Select(rule => new Contracts.PricingRule()
                                                {
                                                    FromDate = rule.FromDate,
                                                    ToDate = rule.ToDate,
                                                    PolicyName = rule.PolicyName,
                                                    Reduction = rule.Reduction,
                                                    ReductionCode = rule.ReductionCode,
                                                    MinNumOfOrders = rule.MinNumOfOrders,
                                                    RuleID = rule.RuleID
                                                }).FirstOrDefault();
                }
        	}
	        catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "PricingRule", ex.Message), ex) { EntityType = typeof(PricingRule) };
            }
            return res;
            
        }

        /// <summary>
        /// Insert a new pricing rule
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int CreateEntity(Contracts.PricingRule entity)
        {
            try
            {
                using (TicketingOfficePricingEntities ctx = new TicketingOfficePricingEntities())
                {
                    DataAccess.PricingRule newRule = new PricingRule()
                    {
                        FromDate = entity.FromDate,
                        ToDate = entity.ToDate,
                        PolicyName = entity.PolicyName,
                        Reduction = entity.Reduction,
                        ReductionCode = entity.ReductionCode,
                        MinNumOfOrders = entity.MinNumOfOrders,
                    };

                    ctx.PricingRules.AddObject(newRule);
                    ctx.SaveChanges();
                    return newRule.RuleID;
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.InsertError, "PricingRule", ex.Message), ex) { EntityType = typeof(PricingRule) };
            }
        }

        /// <summary>
        /// Update an existing pricing rule
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(Contracts.PricingRule entity)
        {
            try
            {
                using (TicketingOfficePricingEntities ctx = new TicketingOfficePricingEntities())
                {
                    var RuleToUpdate = ctx.PricingRules.Where(rule => rule.RuleID == entity.RuleID).FirstOrDefault();
                    if (RuleToUpdate == null)
                    {
                        CreateEntity(entity);
                        return;
                    }
                    RuleToUpdate.FromDate = entity.FromDate;
                    RuleToUpdate.PolicyName = entity.PolicyName;
                    RuleToUpdate.Reduction = entity.Reduction;
                    RuleToUpdate.ReductionCode = entity.ReductionCode;
                    RuleToUpdate.MinNumOfOrders = entity.MinNumOfOrders;
                    RuleToUpdate.ToDate = entity.ToDate;

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.UpdateError, "PricingRule", ex.Message), ex) { EntityType = typeof(PricingRule) };
            }
        }

        /// <summary>
        /// Delete a pricing rule
        /// </summary>
        /// <param name="entityID"></param>
        public void DeleteEntity(int entityID)
        {
            try
            {
                using (TicketingOfficePricingEntities ctx = new TicketingOfficePricingEntities())
                {
                    var ruleToDelete = ctx.PricingRules.Where(rule => rule.RuleID == entityID).FirstOrDefault();
                    if (ruleToDelete == null)
                        return;

                    ctx.PricingRules.DeleteObject(ruleToDelete);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.DeleteError, "PricingRule", ex.Message), ex) { EntityType = typeof(PricingRule) };
            }
        }

        #endregion
    }
}
