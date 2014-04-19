using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.Common.HostExtentions;
using TicketingOffice.PaymentService;
using System.ServiceModel.Activation;
using TicketingOffice.TicketingService;

namespace TicketingOffice.Host.Factories
{
    public class TicketingServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(typeof(GeneralTicketingService), baseAddresses);
            host.Extensions.Add(new ServerStateHostExtension());

            //More host configuration can come here.

            return host;
        }
    }


    public class QueuedTicketingServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(typeof(QueuedTicketingService), baseAddresses);
            host.Extensions.Add(new ServerStateHostExtension());

            //More host configuration can come here.

            return host;
        }
    }
}



   
