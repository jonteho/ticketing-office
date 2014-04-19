using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.TicketingService.Contracts;

namespace TicketingOffice.CrmService.Contracts
{
    // TODO: Ex1 - Examine the service contracts

    [ServiceContract(Namespace = @"http://Fabrikam.com")]
    public interface ICrmBase
    {
        [OperationContract]
        Customer GetCustomerByID(Guid customerID);
    }
    
    /// <summary>
    /// Contract of the CRM service.
    /// These operations should be executed only by managers or employees of the ticketing office
    /// The CRM contract was split to enable different security policies for different endpoints.
    /// </summary>
    [ServiceContract(Namespace = @"http://Fabrikam.com")] 
    public interface ICrmService : ICrmBase
    {
        [OperationContract]
        Customer[] FindCustomersByCriteria(CustomerCriteria criteria);       
        [OperationContract]
        void UpdateCustomer(Customer newCustomer);
        [OperationContract]
        Guid CreateCustomer(Customer newCustomer);
        [OperationContract]
        void DeleteCustomer(Guid customerID);        
        [OperationContract]
        Order[] GetCustomerOrders(Guid customerID, DateTime? fromDate, DateTime? toDate);
        [OperationContract]
        Customer FindCustomerByOrder(Guid OrderID);
        [OperationContract]
        Customer[] FindBestCustomers(int numberOfCustomers);
    }

    /// <summary>
    /// Contract of the CRM service.
    /// These operations should be executed by registered users.
    /// The CRM contract was split to enable different security policies for different endpoints.
    /// </summary>
    [ServiceContract(Namespace = @"http://Fabrikam.com")]
    public interface IPrivateCrm
    {
       [OperationContract]
       Order[] GetMyOrders(string email,string cellNumber,DateTime? fromDate, DateTime? toDate);
       [OperationContract]
       Customer GetMyDetails(string email, string cellNumber);
       [OperationContract]
       void ChangeMyDetails(string email, string cellNumber, Customer newCustomer);
    }
}
