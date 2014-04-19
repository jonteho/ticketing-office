using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.PaymentService;

namespace TicketingOffice.SimpleServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            string state;
            HostingManager.CreateHosts();

            HostingManager.StartHosts();

            
            while (exit == false)
            {
                int i = 0;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine();
                
                //print all the services
                foreach (var item in HostingManager.ServicesTypes)
                {
                    if (!HostingManager.IsAlive(HostingManager.ServicesTypes[i]))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        state = "Closed";
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        state = "Opened";
                    }

                    Console.WriteLine("{0} : {1} : {2}", i, HostingManager.ServicesTypes[i++], state);
                }

                Console.WriteLine("Press the number of the service to start/stop");
                Console.WriteLine("Press ENTER to exit");

                var str = Console.ReadLine();
              
                // Start / Stop a service
                int serviceIndex;
                if ((int.TryParse(str, out serviceIndex)) && (serviceIndex < HostingManager.ServicesTypes.Count()))
                {
                    if (HostingManager.IsAlive(HostingManager.ServicesTypes[serviceIndex]))
                        HostingManager.StopHost(HostingManager.ServicesTypes[serviceIndex]);
                    else
                        HostingManager.StartHost(HostingManager.ServicesTypes[serviceIndex]);
                }
                else
                    exit = true;                   
            }

            //stop all hosts
            HostingManager.StopHost(null);
           
        }
    }
}
