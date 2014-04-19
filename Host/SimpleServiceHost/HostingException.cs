using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketingOffice.SimpleServiceHost
{
    public class HostingException : Exception
    {
        public HostingException() : base() { }

        public HostingException(string message) : base(message) { }

        public HostingException(string message, Exception inner) : base(message, inner) { }
    }
}
