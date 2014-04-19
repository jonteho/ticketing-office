using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.CurrencyExchange.BusinessLogic
{
    
    public class HallStateException : Exception
    {
        public HallStateException() : base() { }

        public HallStateException(string message) : base(message) { }

        public HallStateException(string message, Exception inner) : base(message, inner) { }

        public HallStateException(SerializationInfo info, StreamingContext context) : base(info, context) { }        
    }
}
