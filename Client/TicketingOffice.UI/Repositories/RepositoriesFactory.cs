using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TicketingOffice.UI.Repositories
{
    public interface IRepositoriesFactory
    {
        ICustomersRepository CreateCustomerRepository();
        IOrdersRepository CreateOrdersRepository();
    }

    public class RepositoryFactory
    {
        private static IRepositoriesFactory _repositoryFactory;

        static public IRepositoriesFactory Default
        {
            get
            {
                return _repositoryFactory;
            }
        }

        static RepositoryFactory()
        {
            switch (ConfigurationManager.AppSettings["RepositoryType"])
            {              
                case "Service":
                    _repositoryFactory = new ServiceRepositoriesFactory();
                    break;
                default:
                    throw new ArgumentNullException(
@"Must specify repository type in App.Config: 
Add the following Key to your App.Config file:
<Add Key=""RepositoryType"" Value=""Mock/Service""");
            }
        }       
    }

    class ServiceRepositoriesFactory : IRepositoriesFactory
    {
        public ICustomersRepository CreateCustomerRepository()
        {
            return new CustomersServiceRepository();
        }
        public IOrdersRepository CreateOrdersRepository()
        {
            return new OrdersServiceRepository();
        }
    }
}
