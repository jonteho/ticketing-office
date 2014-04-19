using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.ShowsService.Contracts
{
    public class ShowException: Exception
    {
        public ShowException() : base() { }

        public ShowException(string message) : base(message) { }

        public ShowException(string message, Exception inner) : base(message, inner) { }
    }

    [DataContract(Namespace = "http://Fabrikam.com")]
    public class ShowExceptionInfo
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string ExceptionInfo { get; set; }
    }
}
