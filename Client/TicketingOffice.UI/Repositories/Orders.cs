using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.TicketingService.Contracts;
using System.ServiceModel;
using System.Configuration;

namespace TicketingOffice.UI.Repositories
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetCustomersOrders(CrmService.Contracts.Customer customer);
    }
   
    class OrdersServiceRepository : IOrdersRepository
    {
        public IEnumerable<Order> GetCustomersOrders(CrmService.Contracts.Customer customer)
        {
            var channelFactory = new ChannelFactory<CrmService.Contracts.ICrmService>("CrmEP");

            CrmService.Contracts.ICrmService proxy = channelFactory.CreateChannel();
            return proxy.GetCustomerOrders(customer.ID, null, null);
        }
    }
}
