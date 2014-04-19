using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Messaging;

namespace TicketingOffice.Common.Helpers
{
    public static class MsmqHelper
    {
        public static void CreateMsmq()
        {
            string req_queue =
                    ConfigurationManager.AppSettings["BridgeRequestQueue"];
            string res_queue =
                 ConfigurationManager.AppSettings["BridgeResponseQueue"];
            if (!MessageQueue.Exists(req_queue))
                MessageQueue.Create(req_queue, true);

            if (!MessageQueue.Exists(res_queue))
                MessageQueue.Create(res_queue, true);
        }



        /// <summary>
        /// Clean the queues
        /// </summary>
        public static void CleanMsmq()
        {
            string req_queue = ConfigurationManager.AppSettings["BridgeRequestQueue"];
            string res_queue = ConfigurationManager.AppSettings["BridgeResponseQueue"];

            if (MessageQueue.Exists(req_queue))
                MessageQueue.Delete(req_queue);

            if (MessageQueue.Exists(res_queue))
                MessageQueue.Delete(res_queue);
        }

    }
}
