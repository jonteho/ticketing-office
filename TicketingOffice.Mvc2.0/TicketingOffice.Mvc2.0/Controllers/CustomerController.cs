using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json;
using TicketingOffice.CrmService.Contracts;

namespace TicketingOffice.Mvc2._0.Controllers
{
    public class CustomerController : Controller
    { 
        //
        // GET: /Customer/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Customer list)
        {
            return View(list);
        }

        [HttpPost]
        public ActionResult AddCustomer(Customer customer)
        {
            var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"]) };
            client.PostAsJsonAsync(ConfigurationManager.AppSettings["ApiAddress"], customer);
            return View("Index");
        }

        [HttpPost]
        public ActionResult DeleteCustomer(string email)
        {
            var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"]) };
            //Task.Factory.StartNew(async delegate
            //{
            //    return await client.DeleteAsync(ConfigurationManager.AppSettings["ApiAddress"] + "?email=" + email);
            //});

            client.DeleteAsync(ConfigurationManager.AppSettings["ApiAddress"] + "?email=" + email);
            return View("Index");
        }

        [HttpPost]
        public ViewResult GetCustomers()
        {
            //var client = new HttpClient();
            //client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"]);
            //var response = client.GetStringAsync(ConfigurationManager.AppSettings["ApiAddress"] + "GetCustomers").Result;
            //_customers = JsonConvert.DeserializeObject<List<Customer>>(response);
            //return View("Index", _customers);
            return View("Index");
        }

        public ActionResult ManageCustomer()
        {
            return View(new Customer());
        }

        [HttpPost]
        public ActionResult GetCustomer(string email)
        {
            var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"]) };
            var response = client.GetStringAsync(ConfigurationManager.AppSettings["ApiAddress"] + "?email=" + email).Result;
            var customer = JsonConvert.DeserializeObject<Customer>(response);
            return View("ManageCustomer", customer);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(Customer customer)
        {
            var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"]) };
            //Task.Factory.StartNew(async delegate
            //{
            //    return await client.PutAsJsonAsync(ConfigurationManager.AppSettings["ApiAddress"], customer);
            //});

            client.PutAsJsonAsync(ConfigurationManager.AppSettings["ApiAddress"], customer);
            return View("Index");
        }

    }
}
