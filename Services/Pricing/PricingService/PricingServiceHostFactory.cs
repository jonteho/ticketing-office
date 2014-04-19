using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.Common.HostExtentions;
using TicketingOffice.PricingService;
using System.ServiceModel.Activation;

namespace TicketingOffice.Host.Factories
{
    public class PricingServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(typeof(TicketsPricingService), baseAddresses);
            
            host.Extensions.Add(new ServerStateHostExtension());   

            //More host configuration can come here.

            return host;
        }
    }
}
