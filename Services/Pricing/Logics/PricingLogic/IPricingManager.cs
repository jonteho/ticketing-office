using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.PricingRules.Contracts;

namespace TicketingOffice.PricingService.BusinessLogic
{
    public interface IPricingManager
    {
        double CalculatePrice(PricingRule[] rules, double listPrice);        
    }
}
