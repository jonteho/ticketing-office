using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.PaymentService.Logic
{
    public class PaymentException : Exception
    {
        public PaymentException() : base() { }

        public PaymentException(string message) : base(message) { }

        public PaymentException(string message, Exception inner) : base(message, inner) { }

        public PaymentException(SerializationInfo info, StreamingContext context) : base(info, context) { }  
    }
}
