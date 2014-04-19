using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Threading;
using TicketingOffice.PaymentService.Contracts;
using TicketingOffice.Common.Properties;
using TicketingOffice.PaymentService.DataAccess;

namespace TicketingOffice.PaymentService.BusinessLogic
{
    /// <summary>
    /// Execute the payment logic
    /// </summary>
    public class PaymentManager :IPaymentManager
    {
      
        #region IPaymentManager Members  


        /// <summary>
        /// Create a payment for a ticketing order. An Order can hold a collection of payment items.
        /// </summary>
        /// <param name="orderID">Order to pay for</param>
        /// <param name="customerID">Paying customer</param>
        /// <param name="amount"></param>
        /// <param name="methodOfPayment"></param>
        /// <param name="creditCardNumber">CreditCard information. Relevant only when the payment method is credit card. </param>
        /// <returns></returns>
        public Contracts.Payment PayForOrder(Guid orderID, Guid customerID, double amount, PaymentType methodOfPayment, string creditCardNumber)
        {
            PaymentDal p_dal = new PaymentDal();
            Contracts.Payment newPayment;

            if (methodOfPayment == PaymentType.CreditCard)
                ApproveCreditCard(creditCardNumber);
            
            newPayment = new Contracts.Payment()
            {
                    Amount = amount, 
                    Date = DateTime.Now,
                    MethodOfPayment = methodOfPayment,
                    CustomerID = customerID,
                    OrderID = orderID               
            };                

            var id = p_dal.CreateEntity(newPayment);                
           
            // to make sure the payment is saved we return the payment written to the database.
            return p_dal.GetEntity(id);
        }

        /// <summary>
        /// Simulate credit card approval.
        /// </summary>
        /// <param name="creditCardNumber"></param>
        private void ApproveCreditCard(string creditCardNumber)
        {
            //This is only a simulation. 90% success.
            Random rnd = new Random(DateTime.Now.Second);
            if (rnd.Next(10) == 5)
                throw new PaymentException(StringsResource.CreditCardNotApproved);
        }

        /// <summary>
        /// Simulate refund by creating a negative payment item.
        /// </summary>
        /// <param name="paymentID">The payment item to refund</param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Contracts.Payment Refund(long paymentID, Guid customerID)
        {
            PaymentDal p_dal = new PaymentDal();
            Contracts.Payment refund;
                                   
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                //validate the payment.
                var payment = p_dal.GetEntity(paymentID);
                if (payment == null)
                    throw new PaymentException(StringsResource.RefundFailedPaymentNotFound);
                if (payment.Amount < 0)
                    throw new PaymentException(StringsResource.RefundFailedNegativeAmount);

                //Refund is a payment record with a negative amount
                refund = new Contracts.Payment()
                {
                    ID = Interlocked.Increment(ref paymentID),
                    Amount = 0 - payment.Amount,
                    Date = DateTime.Now,
                    MethodOfPayment = payment.MethodOfPayment,
                    CustomerID = customerID,
                    OrderID = payment.OrderID  
                };

                p_dal.CreateEntity(refund);

                scope.Complete();                
            }

            // to make sure the payment is saved we return the payment written to the database.
            return p_dal.GetEntity(refund.ID);            
        }

      
        /// <summary>
        /// Find a particular payment record.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Contracts.Payment[] FindPayments(PaymentCriteria criteria)
        {
            PaymentDal p_dal = new PaymentDal();
            return p_dal.FindPaymentByCriteria(criteria);                                    
        }


        /// <summary>
        /// Find a payment record accoring to its ID
        /// </summary>
        /// <param name="paymentID"></param>
        /// <returns></returns>
        public Contracts.Payment FindPayment(long paymentID)
        {
            PaymentDal p_dal = new PaymentDal();
            return p_dal.GetEntity(paymentID);                                   
        }


        /// <summary>
        /// Determine if a certain payment type can be used
        /// </summary>
        /// <param name="paymentType"></param>
        /// <returns></returns>
        public bool IsPaymentTypeEnabled(Contracts.PaymentType paymentType)
        {
            // This is only a simulation. 
            Random rnd = new Random((int)(DateTime.Now.Ticks % 100));
            if (rnd.Next(10) <= 7)
                return true;

            return false;
        }
        #endregion
    }
}
