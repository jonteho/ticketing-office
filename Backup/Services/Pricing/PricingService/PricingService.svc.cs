using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.Pricing.Contracts;
using TicketingOffice.PricingRules.Contracts;
using TicketingOffice.Common.Properties;
using TicketingOffice.Common.Helpers;
using TicketingOffice.PricingService.BusinessLogic;
using TicketingOffice.CurrencyExchange.Contract;
using TicketingOffice.CurrencyExchangeWcfProxy;
using TicketingOffice.Common.HostExtentions;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;
using System.Configuration;

namespace TicketingOffice.PricingService
{
    /// <summary>
    /// A service for calculating the final price for a particular event.
    /// The service will call the pricing rule service to get the pricing rules required for the calculation
    /// </summary>
    public class TicketsPricingService : IPricingService
    {
        IPricingManager manager = new PricingManager();

        ChannelFactory<IPricingRulesService> chf;
        double localResult;

        #region IPricingService Members

        /// <summary>
        /// Calculate a final price
        /// </summary>
        /// <param name="reductionCode">This code is used to classify customes and to find pricing rules appropriate for different kind of customers</param>
        /// <param name="policyName">The relevant group of rules</param>
        /// <param name="listPrice">The base price if the event</param>
        /// <param name="numberOfOrders">Numbers of tickes ordered</param>
        /// <param name="currency">Currency in which the price us shown to the client</param>
        /// <returns>The calculated price</returns>
        public virtual double CalculatePrice(int reductionCode, string policyName, int listPrice, int numberOfTickets, Currencies? currency)
        {

            IExchangeService exchangeProx;
            PricingRule[] rules = null;

            #region Call Pricing Rules service and calculate

              

            rules = GetRulesFromService(policyName);            

            //If there are rules use them
            if ((rules != null) && (rules.Count() > 0))
            {

                var effectiveRules = rules.Where(rl => ((rl.ReductionCode == reductionCode) ||
                                                             (rl.MinNumOfOrders <= numberOfTickets))).ToArray();

                localResult = manager.CalculatePrice(effectiveRules, listPrice);
            }
            else
                localResult = listPrice;

            #endregion

            if (currency != null)
            {
                // wrap the channel creation 
                exchangeProx = new CurrencyExchangeProxy();
                //Use the currency exhange service to tranform the price to the required currency
                localResult = exchangeProx.Buy(currency.Value, localResult);
            }

            return localResult;
        }


        /// <summary>
        /// Call the discovery proxy and get a list of pricing rules services endpoints.
        /// Look for the relevant rules in each of the service. When found return the rules.
        /// </summary>
        /// <param name="policyName"></param>
        /// <returns></returns>
        private PricingRule[] GetRulesFromService(string policyName)
        {
            IPricingRulesService prox = null;
            // Create a DiscoveryEndpoint that points to the DiscoveryProxy
            Uri probeEndpointAddress = new Uri(ConfigurationManager.AppSettings["ProbUri"]); //i.e. "net.tcp://localhost:5050/Probe"
            DiscoveryEndpoint discoveryEndpoint = new DiscoveryEndpoint(new NetTcpBinding(),
                new EndpointAddress(probeEndpointAddress));

            //Use Ad-Hoc discovery to find pricing rules services in the enterprise.
            DiscoveryClient discoveryClient = new DiscoveryClient(discoveryEndpoint);

            PricingRule[] rules = null;
            EndpointAddress[] addresses;
            FindResponse findResponse = null;
            try
            {
                // TODO: Ex6 - Find a service that exposes the IPricingRulesService service contract                
                lock (this)
                {
                    if (chf == null)
                        chf = new ChannelFactory<IPricingRulesService>("PricingRulesEP");
                }

                // Find IPricingRulesService endpoints                
                try
                {
                    findResponse = discoveryClient.Find(
                      new FindCriteria(typeof(IPricingRulesService)));
                }
                catch (Exception) { }

                if (findResponse != null)
                {
                    LoggingManager.Logger.Log(LoggingCategory.Info,
                        string.Format("Discovery proxy found {0} endpoints", 
                        findResponse.Endpoints.Count));

                    addresses = (from ep in findResponse.Endpoints
                                 select ep.Address).ToArray();
                }
                else
                    //Set a default address for the pricing rules service
                    addresses = new EndpointAddress[] { chf.Endpoint.Address };

                // look for the rules in all the endpoints found. 
                // When rules are found stop searching.
                foreach (var address in addresses)
                {
                    prox = chf.CreateChannel(address);
                    rules = prox.GetPricingRulesByCriteria(new RulesCriteria()
                    {
                        Scope = policyName,
                        FromDate = DateTime.Today,
                        ToDate = DateTime.Today.AddDays(1)
                    });
                    if ((rules != null) && (rules.Count() > 0))
                        break;
                } 

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, StringsResource.FailedToContactPriceRules + " " + ex.Message);
                throw new PricingException(StringsResource.FailedToContactPriceRules + " " + ex.Message, ex);
            }
            finally
            {
                var channel = prox as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }

            return rules;
        }

        #endregion
    }
}
