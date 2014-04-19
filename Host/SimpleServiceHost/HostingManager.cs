using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;
using System.Configuration;
using TicketingOffice.Common.Properties;
using System.ServiceModel;
using TicketingOffice.Common.Helpers;
using TicketingOffice.CrmService;
using TicketingOffice.CurrencyExchange.Wcf;
using TicketingOffice.HallStateService;
using TicketingOffice.PricingService;
using TicketingOffice.PricingRulesService;
using TicketingOffice.ShowsService;
using TicketingOffice.TicketingService;
using TicketingOffice.Bridge;
using TicketingOffice.PaymentService;
using TicketingOffice.Common.HostExtentions;
using System.Messaging;
using TicketingOffice.PricingBrokerService;

namespace TicketingOffice.SimpleServiceHost
{
    public class HostingManager
    {
        public static Dictionary<string, ServiceHostBase> Hosts { get; set; }

        static HostingManager()
        {
            Hosts = new Dictionary<string, ServiceHostBase>();
        }

        /// <summary>
        /// A helper dictionary with all the service implementation types. 
        /// </summary>
        static Dictionary<string, Type> ServiceTypeResolver = new Dictionary<string, Type>()
        {           
            {"TicketingOffice.CrmService.CustomerRelationsService", typeof(CustomerRelationsService)},
            {"TicketingOffice.CurrencyExchange.Wcf.CurrencyExchangeService", typeof(CurrencyExchangeService)},
            {"TicketingOffice.HallStateService.ReservationsService", typeof(ReservationsService)},      
            {"TicketingOffice.PaymentService.TicketsPaymentService", typeof(TicketsPaymentService)},
            {"TicketingOffice.PricingService.TicketsPricingService", typeof(TicketsPricingService)},          
            {"TicketingOffice.PricingRulesService.TicketingPricingRulesService", typeof(TicketingPricingRulesService)},           
            {"TicketingOffice.ShowsService.ShowsAndEventsService", typeof(ShowsAndEventsService)},
            {"TicketingOffice.TicketingService.GeneralTicketingService", typeof(GeneralTicketingService)},
            {"TicketingOffice.TicketingService.QueuedTicketingService", typeof(QueuedTicketingService)},           
            {"TicketingOffice.Bridge.TicketingBridge", typeof(TicketingBridge)},
            {"TicketingOffice.Bridge.TicketingBridgeCallBack", typeof(TicketingBridgeCallBack)},  
            {"TicketingOffice.PricingBrokerService.DiscoveryProxyService", typeof(DiscoveryProxyService)}
        };


        public static string[] ServicesTypes
        {
            get { return ServiceTypeResolver.Keys.ToArray(); }
        }

        /// <summary>
        /// Create MSMQ queues to be used by queued services
        /// </summary>
        public static void CreateMsmq()
        {
            // TODO: Ex2 - Add code to create MSMQ queues         
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


        /// <summary>
        /// Create the ServiceHost objects that will host the wcf services. 
        /// </summary>
        public static void CreateHosts()
        {
            LoggingManager.Logger.Log(LoggingCategory.Info, StringsResource.CreateHosts);

            var cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSectionGroup("system.serviceModel");

            var servicesConfig = cfg.Sections["services"] as ServicesSection;
            if (servicesConfig == null)
                throw new HostingException(StringsResource.SevicesToHostNotFound);

            // read the service configuration and use it to create a service host
            foreach (ServiceElement item in servicesConfig.Services)
            {
                try
                {
                    Hosts[item.Name] = new ServiceHost(ServiceTypeResolver[item.Name]);
                    // Add IExtentsion<ServicaHostBase> extentions to each host.
                    Hosts[item.Name].Extensions.Add(new ServerStateHostExtension()); // attach a memory state collection

                    LoggingManager.Logger.Log(LoggingCategory.Info, string.Format(StringsResource.HostCreated, item.Name));
                    Hosts[item.Name].Faulted += new EventHandler(HostingManager_Faulted);
                }
                catch (Exception ex)
                {
                    LoggingManager.Logger.Log(LoggingCategory.Warning,
                        string.Format(StringsResource.HostCreationFailed, item.Name, ex.ToString()));
                }

            }

            try
            {
                CreateMsmq();
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Warning,
                        string.Format(StringsResource.MSMQCreationFailed, ex.Message));
            }

            Console.WriteLine();

        }

        /// <summary>
        /// Start the service hosts and begin listening on the channels
        /// </summary>
        public static void CreateHost(string name)
        {
            if (!Hosts.ContainsKey(name))
                return;
            try
            {
                CreateMsmq();

                Hosts[name] = new ServiceHost(ServiceTypeResolver[name]);
                Hosts[name].Extensions.Add(new ServerStateHostExtension());
                Hosts[name].Extensions.Add(new CacheHostExtension());
                LoggingManager.Logger.Log(LoggingCategory.Info, string.Format(StringsResource.HostCreated, name));
                Hosts[name].Faulted += new EventHandler(HostingManager_Faulted);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Warning,
                    string.Format(StringsResource.HostCreationFailed, name, ex.ToString()));
            }
        }


        public static void StartHosts()
        {
            ServiceHost err = null;
            StringBuilder sb = new StringBuilder();
            LoggingManager.Logger.Log(LoggingCategory.Info, StringsResource.StatrtHosts);
            foreach (ServiceHost host in Hosts.Values)
            {
                try
                {

                    err = host;
                    host.Open();

                    if (host.State == CommunicationState.Opened)
                    {
                        LoggingManager.Logger.Log(LoggingCategory.Info,
                            string.Format(StringsResource.HostStarted,
                            host.Description.Name));

                        foreach (var dispatcher in host.ChannelDispatchers)
                        {
                            if ((dispatcher.Listener != null) && (dispatcher.Listener.Uri != null))
                                sb.AppendLine(string.Format(StringsResource.ListentingAt, dispatcher.Listener.Uri.AbsoluteUri));
                        }
                        LoggingManager.Logger.Log(LoggingCategory.Info, sb.ToString());
                        sb.Clear();
                    }
                }
                catch (Exception ex)
                {
                    LoggingManager.Logger.Log(LoggingCategory.Warning,
                            string.Format(StringsResource.HostStartFailed, err.Description.Name, ex.ToString()));
                }
            }
        }


        public static void StopHost(string name)
        {
            LoggingManager.Logger.Log(LoggingCategory.Info, StringsResource.StoppingHosts);

            if (name == null)
            {
                foreach (var host in Hosts.Values)
                {
                    if ((host != null) && (host.State == CommunicationState.Opened))
                    {
                        host.Close();
                        LoggingManager.Logger.Log(LoggingCategory.Info,
                            string.Format(StringsResource.HostClosed, host.Description.Name));
                    }
                }
                CleanMsmq();
            }
            else
            {
                if (Hosts.ContainsKey(name) && Hosts[name] != null && Hosts[name].State == CommunicationState.Opened)
                {
                    Hosts[name].Close();
                    LoggingManager.Logger.Log(LoggingCategory.Info, string.Format(StringsResource.HostClosed, name));
                }
            }


        }


        public static void StartHost(string name)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (Hosts.ContainsKey(name))
                {
                    CreateHost(name);
                    Hosts[name].Open();
                }

                if (Hosts[name].State == CommunicationState.Opened)
                {
                    LoggingManager.Logger.Log(LoggingCategory.Info,
                        string.Format(StringsResource.HostStarted,
                        Hosts[name].Description.Name));

                    foreach (var dispatcher in Hosts[name].ChannelDispatchers)
                    {
                        if ((dispatcher.Listener != null) && (dispatcher.Listener.Uri != null))
                            sb.AppendLine(string.Format(StringsResource.ListentingAt, dispatcher.Listener.Uri.AbsoluteUri));
                    }
                    LoggingManager.Logger.Log(LoggingCategory.Info, sb.ToString());
                    sb.Clear();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Warning,
                   string.Format(StringsResource.HostStartFailed, name, ex.ToString()));
            }

        }


        public static bool IsAlive(string name)
        {
            if (!Hosts.ContainsKey(name))
                return false;

            return ((Hosts[name].State == CommunicationState.Opened));
        }

        static void HostingManager_Faulted(object sender, EventArgs e)
        {
            LoggingManager.Logger.Log(LoggingCategory.Error, string.Format(StringsResource.HostFalted, sender.ToString()));
        }

    }
}
