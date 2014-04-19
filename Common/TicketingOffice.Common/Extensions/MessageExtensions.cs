using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;
using System.IO;

namespace TicketingOffice.Common.Extensions
{
    public static class MessageExtensions
    {
        public static Message Clone(this Message original)
        {
            var buffer = original.CreateBufferedCopy(int.MaxValue);
            return buffer.CreateMessage();           
        }
    }
}
