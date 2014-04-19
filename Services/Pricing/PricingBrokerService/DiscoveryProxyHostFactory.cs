using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;
using TicketingOffice.Common.HostExtentions;
using TicketingOffice.PricingBrokerService;

namespace TicketingOffice.Host.Factories
{
    class DiscoveryProxyHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(typeof(DiscoveryProxyService), baseAddresses);
            host.Extensions.Add(new ServerStateHostExtension());

            //More host configuration can come here.

            return host;
        }
    }
}
