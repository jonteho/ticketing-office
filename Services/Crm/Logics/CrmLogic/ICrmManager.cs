using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.CrmService;
using TicketingOffice.CrmService.Contracts;

namespace TicketingOffice.CrmService.CrmLogic
{
    public interface ICrmManager
    {
        Customer[] FindCustomers(CustomerCriteria criteria);
        Customer FindCustomer(Guid customerID);
        Guid CreateCustomer(Customer newCustomer);
        void UpdateCustomer(Customer newCustomer);
        void DeleteCustomer(Guid customerID);       
    }
}
