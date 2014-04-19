//using System;
//using System.Net.Http;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using TicketingOffice.CrmService.Contracts;
//using TicketingOffice.CrmService.CrmLogic;
//using TicketingOffice.WebAPI.Controllers;

//namespace TicketingOffice.WebAPITest
//{
//    [TestClass]
//    public class CustomerControllerTest
//    {
//        private CustomerController _controller;
//        private Guid _id;
//        private Customer _customer;

//        private Mock<ICrmManager> _mockRepository;

//        [TestInitialize]
//        public void Init()
//        {
//            _id = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c");
//            _customer = new Customer()
//            {
//                ID = _id,
//                FirstName = "Jonathan",
//                LastName = "Holm",
//                Address = "Stockholmsvägen",
//                CellNumber = "123",
//                BirthDate = DateTime.Now.AddYears(-23),
//                Country = "Sweden",
//                City = "Stockholm",
//                Email = "1234@abc.com",
//                PhoneNumber = "555-999",
//                ReductionCode = 3
//            };

//            _mockRepository = new Mock<ICrmManager>();
//            _mockRepository.Setup(m => m.CreateCustomer(_customer));
//            _mockRepository.Setup(m => m.FindCustomer(_customer.ID));
//            _mockRepository.Setup(m => m.UpdateCustomer(_customer));
//            _mockRepository.Setup(m => m.DeleteCustomer(_customer.ID));

//        }

//        [TestMethod]
//        public void TestCreateCustomer()
//        {
//            // Arrange
//            _controller = new CustomerController();

//            // Act
//            _controller.AddCustomer(_customer);
//            var customer = _controller.GetCustomer(_customer.Email);

//            // Assert
//            Assert.IsNotNull(customer, "Användaren finns redan i databasen");
//        }

//        [TestMethod]
//        public void TestUpdateCustomer()
//        {
//            // Arrange
//            _controller = new CustomerController();
//            var customer = _controller.GetCustomer(_customer.Email);
//            var customerName = customer.FirstName;
//            customer.FirstName = "Henrik";

//            // Act
//            _controller.PutCustomer(customer);
//            var updatedCustomer = _controller.GetCustomer(customer.Email);

//            // Assert
//            Assert.AreNotEqual(updatedCustomer.FirstName, customerName, "Användaren finns inte i databasen");
//        }

//        [TestMethod]
//        public void TestDeleteCustomer()
//        {
//            // Arrange
//            _controller = new CustomerController();

//            // Act
//            _controller.DeleteCustomer(_customer.Email);
//            var customer = _controller.GetCustomer(_customer.Email);

//            // Assert
//            Assert.IsNull(customer, "Användarens ID finns inte i databasen");
//        }

//        [TestMethod]
//        public void TestGetCustomers()
//        {
//            // Arrange
//            _controller = new CustomerController();

//            // Act
//            var customers = _controller.GetCustomers();

//            // Assert
//            Assert.IsNotNull(customers);
//        }

//    }
//}
