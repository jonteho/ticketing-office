using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.Common.HostExtentions;
using System.ServiceModel;
using TicketingOffice.CurrencyExchange.Wcf;
using System.ServiceModel.Activation;

namespace TicketingOffice.Host.Factories
{
    public class CurrencyExchangeServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(typeof(CurrencyExchangeService),baseAddresses);
            host.Extensions.Add(new ServerStateHostExtension());

            //More host configuration can come here.

            return host;
        }
    }
}
