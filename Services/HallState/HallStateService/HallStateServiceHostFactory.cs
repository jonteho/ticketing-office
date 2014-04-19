using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.Common.HostExtentions;
using TicketingOffice.HallStateService;
using System.ServiceModel.Activation;

namespace TicketingOffice.Host.Factories
{
    public class HallStateServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(typeof(ReservationsService), baseAddresses);
            host.Extensions.Add(new ServerStateHostExtension());

            //More host configuration can come here.

            return host;
        }
    }
}
