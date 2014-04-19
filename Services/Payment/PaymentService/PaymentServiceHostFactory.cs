using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;
using TicketingOffice.CrmService;
using TicketingOffice.Common.HostExtentions;

namespace TicketingOffice.PaymentService
{
    public class PaymentServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            ServiceHost host = new ServiceHost(typeof(TicketsPaymentService), baseAddresses);
            host.Extensions.Add(new ServerStateHostExtension());       
          
            //More host configuration can come here.

            return host;
        }
    }   
}
