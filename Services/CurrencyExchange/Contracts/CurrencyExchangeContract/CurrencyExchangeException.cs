using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketingOffice.CurrencyExchange.Contract
{
    public class CurrencyExchangeException : Exception
    {
         public CurrencyExchangeException() : base() { }

        public CurrencyExchangeException(string message) : base(message) { }

        public CurrencyExchangeException(string message, Exception inner) : base(message, inner) { }
    }
}
