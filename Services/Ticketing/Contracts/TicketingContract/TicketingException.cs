using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketingOffice.TicketingService.Contracts
{
    /// <summary>
    /// Exception for the ticketing service
    /// </summary>
    public class TicketingException : Exception
    {
        public TicketingException() : base() { }

        public TicketingException(string message) : base(message) { }

        public TicketingException(string message, Exception inner) : base(message, inner) { }
    }
}
