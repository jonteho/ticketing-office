using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TicketingOffice.CrmService.Contracts;
using TicketingOffice.WebAPI.Controllers;
using System.Net.Http.Formatting;

namespace TicketingOffice.MVC.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [HttpGet]
        public ActionResult Index()
        {
            return View(new List<Customer>());
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<Customer> list)
        {
            return View(list);
        }

        [HttpPost]
        public void AddCustomer(Customer customer)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"]);
            client.PostAsJsonAsync(ConfigurationManager.AppSettings["ApiAddress"] + "AddCustomer", customer);
        }

        [HttpPost]
        public void UpdateCustomer(Customer customer)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:17016");
            client.PutAsJsonAsync("http://localhost:17016/api/customer", customer);
        }

        [HttpPost]
        public void DeleteCustomer(Customer customer)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:17016");
        }

        [HttpPost]
        public ViewResult GetCustomers()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"]);
            var response = client.GetStringAsync(ConfigurationManager.AppSettings["ApiAddress"] + "GetCustomers").Result;
            var customers = JsonConvert.DeserializeObject<List<Customer>>(response);
            return View("Index", customers);
        }

    }
}