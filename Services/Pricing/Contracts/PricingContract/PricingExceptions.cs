using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketingOffice.PricingRules.Contracts
{

    public class PricingRuleException : Exception
    {
        public PricingRuleException() : base() { }

        public PricingRuleException(string message) : base(message) { }

        public PricingRuleException(string message, Exception inner) : base(message, inner) { }
    }
}

namespace TicketingOffice.Pricing.Contracts
{

    public class PricingException : Exception
    {
        public PricingException() : base() { }

        public PricingException(string message) : base(message) { }

        public PricingException(string message, Exception inner) : base(message, inner) { }
    }
    
}
