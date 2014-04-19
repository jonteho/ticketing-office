using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.PaymentService.Contracts;

namespace TicketingOffice.PaymentService.Contracts
{
    [ServiceContract(Namespace="http://Fabrikam.com")]
    public interface IPaymentService
    {
        [OperationContract]
        Payment PayForOrder(Guid orderID, Guid payingCustomerID, double amount, PaymentType methodOfPayment, string CreditCardNumber);
        [OperationContract]
        Payment Refund(long paymentID, Guid customerID);
        [OperationContract]
        Payment[] FindPayments(PaymentCriteria criteria);
        [OperationContract]
        Payment FindPayment(long paymentID);

    }
}
