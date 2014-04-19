using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketingOffice.HallState.Contracts
{
    public class HallStateException : Exception
    {
        public HallStateException() : base() { }

        public HallStateException(string message) : base(message) { }

        public HallStateException(string message, Exception inner) : base(message, inner) { }
    }
}
