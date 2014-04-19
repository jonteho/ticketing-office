using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.CrmService.Contracts;
using System.ServiceModel;
using System.Configuration;
using System.Windows;

namespace TicketingOffice.UI.Repositories
{
    public interface ICustomersRepository
    {
        IEnumerable<Customer> GetCustomers();
    }   

    class CustomersServiceRepository : ICustomersRepository
    {
        public IEnumerable<Customer> GetCustomers()
        {
            ChannelFactory<ICrmService> channelFactory = new ChannelFactory<ICrmService>("CrmEP");
            
            ICrmService proxy = channelFactory.CreateChannel();            

            return proxy.FindCustomersByCriteria(new CustomerCriteria());
        }
    }
}
