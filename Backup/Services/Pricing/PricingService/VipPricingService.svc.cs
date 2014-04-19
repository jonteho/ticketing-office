using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.Pricing.Contracts;
using TicketingOffice.Common.Helpers;

namespace TicketingOffice.PricingService
{
   
    /// <summary>
    /// Another instance of the pricing service. 
    /// This service will be used to demonstrate routing.
    /// </summary>
    public class VipPricingService : TicketsPricingService , IPricingService
    {
        public override double CalculatePrice(int reductionCode, string policyName, int listPrice, int numberOfTickets, CurrencyExchange.Contract.Currencies? currency)
        {
            LoggingManager.Logger.Log(LoggingCategory.Info, "VIP Pricing");

            // Just call the base and give another 10% discount
            return 0.9 * base.CalculatePrice(reductionCode,policyName,listPrice,numberOfTickets,currency);
        }
    }
}
