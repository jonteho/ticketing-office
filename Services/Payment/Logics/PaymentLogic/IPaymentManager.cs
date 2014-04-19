using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.PaymentService.Contracts;
using System.ServiceModel;


namespace TicketingOffice.PaymentService.BusinessLogic
{
   
    public interface IPaymentManager
    {
        Contracts.Payment PayForOrder(Guid orderID, Guid customerID, double amount, PaymentType methodOfPayment, string CreditCardNumber);

        Contracts.Payment Refund(long paymentID, Guid customerID);

        Contracts.Payment[] FindPayments(PaymentCriteria criteria);

        Contracts.Payment FindPayment(long paymentID);

        bool IsPaymentTypeEnabled(Contracts.PaymentType paymentType);
    }
}
