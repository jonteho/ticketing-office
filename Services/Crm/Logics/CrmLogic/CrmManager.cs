using System;
using TicketingOffice.CrmService.Contracts;
using TicketingOffice.CrmService.CrmLogic;
using TicketingOffice.CrmService.DataAccess;

namespace CrmLogic
{
    /// <summary>
    /// Execute customer related logic. 
    /// In this simple example there is only call to the data access.
    /// </summary>
    public class CrmManager : ICrmManager
    {
        #region ICrmManager Members

        public TicketingOffice.CrmService.Contracts.Customer[] FindCustomers(CustomerCriteria criteria)
        {
            CustomerDal dal = new CustomerDal();
            return dal.GetCustomersByCriteria(criteria);
        }

        public Guid CreateCustomer(TicketingOffice.CrmService.Contracts.Customer newCustomer)
        {
            CustomerDal dal = new CustomerDal();
            return dal.CreateEntity(newCustomer);
        }

        public void UpdateCustomer(TicketingOffice.CrmService.Contracts.Customer newCustomer)
        {
            CustomerDal dal = new CustomerDal();
            dal.UpdateEntity(newCustomer);
        }

        public void DeleteCustomer(Guid customerID)
        {
            CustomerDal dal = new CustomerDal();
            dal.DeleteEntity(customerID);
        }
                
        public TicketingOffice.CrmService.Contracts.Customer FindCustomer(Guid customerID)
        {
            CustomerDal dal = new CustomerDal();
            return dal.GetEntity(customerID);
        }

        #endregion
    }
}
