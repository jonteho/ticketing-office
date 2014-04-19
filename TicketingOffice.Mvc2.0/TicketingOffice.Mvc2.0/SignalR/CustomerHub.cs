using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using TicketingOffice.CrmService.Contracts;

namespace TicketingOffice.Mvc2._0.SignalR
{
    public class CustomerHub : Hub
    {

        public static Timer myTimer = new Timer();
        static CustomerHub()
        {
            myTimer.Interval = 2000;
            myTimer.Elapsed += myTimer_Elapsed;
            myTimer.Start();
        }

        static void myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext("CustomerHub");
            hub.Clients.All.BroadcastMessage(string.Format("{0} - Still running", DateTime.UtcNow));
        }

        public void Send(object values, DateTime time)
        {
            var customer = JsonConvert.DeserializeObject<Customer>(values.ToString());

            Clients.All.BroadcastMessage(customer, string.Format("{0}", DateTime.UtcNow));
        }
    }
}