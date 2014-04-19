using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.TicketingService.Contracts;

namespace TicketingOffice.TicketingService
{
    /// <summary>
    /// Object to object mapping between the customer defined by the CRM service and the customer defined by the Ticketing service.
    /// </summary>
    public static class CustomerMapper
    {
        public static CrmService.Contracts.Customer MapToCrmCustomer(Customer tck_ust)
        {
            return new CrmService.Contracts.Customer()
            {
                Address = tck_ust.Address,
                BirthDate = tck_ust.BirthDate,
                CellNumber = tck_ust.CellNumber,
                City = tck_ust.City,
                Country = tck_ust.Country,
                Email = tck_ust.Email,
                FirstName = tck_ust.FirstName,
                ID = tck_ust.ID,
                LastName = tck_ust.LastName,
                PhoneNumber = tck_ust.PhoneNumber,
                ReductionCode = tck_ust.ReductionCode
            };
        }


        public static Customer MapToTicketingCustomer(CrmService.Contracts.Customer crm_cust)
        {
            return new TicketingOffice.TicketingService.Contracts.Customer()
            {
                Address = crm_cust.Address,
                BirthDate = crm_cust.BirthDate,
                CellNumber = crm_cust.CellNumber,
                City = crm_cust.City,
                Country = crm_cust.Country,
                Email = crm_cust.Email,
                FirstName = crm_cust.FirstName,
                ID = crm_cust.ID,
                LastName = crm_cust.LastName,
                PhoneNumber = crm_cust.PhoneNumber,
                ReductionCode = crm_cust.ReductionCode
            };
        }
    }
}
