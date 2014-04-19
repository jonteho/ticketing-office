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
    public class CustomerController : ApiController
    {
        private ICrmManager _repository;

        public CustomerController()
        {
        }

        public CustomerController(ICrmManager repository)
        {
            _repository = repository;
        }

        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        // GET api/<controller>
        [ActionName("GetCustomers")]
        public IEnumerable<Customer> GetCustomers()
        {
                _repository = new CrmManager();
                var criteria = new CustomerCriteria();
                return _repository.FindCustomers(criteria);
        }

        // GET api/<controller>/5
        [ActionName("GetCustomer")]
        public Customer GetCustomer(string email)
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

        // POST api/<controller>/getcustomer/
        [ActionName("AddCustomer")]
        public void AddCustomer(Customer customer)
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

        // PUT api/<controller>/5
        [ActionName("UpdateCustomer")]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpPost]
        public void UpdateCustomer(Customer customer)
        {
            _repository = new CrmManager();
            _repository.UpdateCustomer(customer);
        }

        [ActionName("DeleteCustomer")]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public void DeleteCustomer(string email)
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

        public void Put()
        {
        }
        public void Post()
        {
        }
        public void Get()
        {
        }

        // DELETE api/<controller>/5
        public string Delete()
        {
            return "Delete";
        }



    }
}