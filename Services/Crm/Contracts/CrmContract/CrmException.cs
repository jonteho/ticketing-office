using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketingOffice.CrmService.Contracts
{
    public class CrmException : Exception
    {
        public CrmException() : base() { }

        public CrmException(string message) : base(message) { }

        public CrmException(string message, Exception inner) : base(message, inner) { }
    }
}
