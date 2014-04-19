using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CrmLogic;
using TicketingOffice.CrmService.Contracts;
using TicketingOffice.CrmService.CrmLogic;

namespace TicketingOffice.WebAPI.Controllers
{
    public class TestController : ApiController
    {

        private ICrmManager _repository;

        //public HttpResponseMessage Options()
        //{
        //    var response = new HttpResponseMessage();
        //    response.StatusCode = HttpStatusCode.OK;
        //    return response;
        //}

        // GET api/test
        public IEnumerable<Customer> Get()
        {
            _repository = new CrmManager();
            var criteria = new CustomerCriteria();
            return _repository.FindCustomers(criteria);
        }

        // GET api/test/5
        public Customer Get(string email)
        {
            _repository = new CrmManager();
            var customer = new Customer();
            var customers = _repository.FindCustomers(new CustomerCriteria());
            foreach (var c in customers)
            {
                if (c.Email == email)
                {
                    customer = new Customer()
                    {
                        ID = c.ID
                    };
                }
            }
            return _repository.FindCustomer(customer.ID);
        }

        // POST api/test
        public void Post(Customer customer)
        {
            _repository = new CrmManager();
            var newCustomer = new Customer()
            {
                ID = Guid.NewGuid(),
                Address = customer.Address,
                BirthDate = customer.BirthDate,
                CellNumber = customer.CellNumber,
                Country = customer.Country,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                City = customer.City,
                ReductionCode = customer.ReductionCode
            };
            _repository.CreateCustomer(newCustomer);
        }

        // PUT api/test/5
        public void PutCustomer(Customer customer)
        {
            _repository = new CrmManager();
            _repository.UpdateCustomer(customer);

        }

        // DELETE api/test/5
        public void Delete(string email)
        {
            _repository = new CrmManager();
            var customer = new Customer();
            var customers = _repository.FindCustomers(new CustomerCriteria());
            foreach (var c in customers)
            {
                if (c.Email == email)
                {
                    customer = new Customer()
                    {
                        ID = c.ID
                    };
                }
            }
            _repository.DeleteCustomer(customer.ID);
           
        }
    }
}
