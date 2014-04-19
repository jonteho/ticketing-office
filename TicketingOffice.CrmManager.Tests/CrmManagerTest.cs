using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TicketingOffice.CrmManager.Tests.Annotations;
using TicketingOffice.CrmService.CrmLogic;

namespace TicketingOffice.CrmManager.Tests
{
    [TestClass]
    public class CrmManagerTest
    {
        private Mock<ICrmManager> _mockRepository;
        private ICrmManager _mockCrmManager;
        private CrmService.Contracts.Customer _customer;
        private Guid _id;

        [TestInitialize]
        public void Init()
        {
            _id = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c");
            _customer = new CrmService.Contracts.Customer()
            {
                ID = _id,
                FirstName = "Jonathan",
                LastName = "Holm",
                Address = "Stockholmsvägen",
                CellNumber = "123",
                BirthDate = DateTime.Now.AddYears(-23),
                Country = "Sweden",
                City = "Stockholm",
                Email = "123@abc.com",
                PhoneNumber = "555-999",
                ReductionCode = 3
            };
            _mockRepository = new Mock<ICrmManager>();

            _mockRepository.Setup(m => m.FindCustomer(_customer.ID)).Returns(_customer);
            _mockRepository.Setup(m => m.CreateCustomer(_customer)).Returns(_customer.ID);
            _mockRepository.Setup(m => m.UpdateCustomer(_customer));
            _mockRepository.Setup(m => m.DeleteCustomer(_id));

            _mockCrmManager = _mockRepository.Object;
        }

        [TestMethod]
        public void TestCreateCustomer()
        {
            // Arrange in init()

            // Act
            var expectedId = _mockCrmManager.CreateCustomer(_customer);

            // Assert
            Assert.AreEqual(expectedId, _id);
        }

        [TestMethod]
        public void TestUpdateCustomer()
        {
            // Arrange
            var name = _mockCrmManager.FindCustomer(_customer.ID).FirstName;

            // Act
            _customer.FirstName = "Henrik";
            _mockCrmManager.UpdateCustomer(_customer);

            // Assert
            Assert.AreNotEqual(_mockCrmManager.FindCustomer(_customer.ID).FirstName, name);
            Assert.AreEqual(_mockCrmManager.FindCustomer(_customer.ID).FirstName, "Henrik");
        }

        [TestMethod]
        public void TestDeleteCustomer()
        {
            // Kolla att den anropas

            // Arrange

            // Act

            // Assert

        }
    }
}
