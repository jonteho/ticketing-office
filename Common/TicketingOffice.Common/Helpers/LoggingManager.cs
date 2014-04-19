using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketingOffice.Common.Helpers
{
    public class LoggingManager
    {
        public static ILogger Logger { get; set; }

        static LoggingManager()
        {
            //Default logger.
            Logger = new ConsoleLogger();
        }
    }
}
