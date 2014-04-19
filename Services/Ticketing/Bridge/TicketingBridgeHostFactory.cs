using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using TicketingOffice.Common.HostExtentions;
using TicketingOffice.Bridge;

namespace TicketingOffice.Host.Factories
{
    public class TicketingBridgeHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(typeof(TicketingBridge),baseAddresses[0]);
            host.Extensions.Add(new ServerStateHostExtension());

            //More host configuration can come here.

            return host;
        }
    }
}