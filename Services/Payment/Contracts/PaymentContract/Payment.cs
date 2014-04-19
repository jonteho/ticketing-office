using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.PaymentService.Contracts
{
    /// <summary>
    /// Payment information
    /// </summary>
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public class Payment
    {
        [DataMember]
        public long ID { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public Guid CustomerID { get; set; }
        [DataMember]
        public PaymentType MethodOfPayment 
        {
            get { return (PaymentType)Enum.ToObject(typeof(PaymentType), MethodOfPaymentID); }
            set { MethodOfPaymentID = (int)value;}
        }
        [DataMember]
        public Guid OrderID { get; set; }

        //reflect the MethodOfPaymentn as an integer
        public int MethodOfPaymentID { get; set; }

        public Payment() { }     
        
    }

    /// <summary>
    /// Different kind of payment methods
    /// </summary>
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public enum PaymentType
    {
        [EnumMember]
        CreditCard = 0,
        [EnumMember]
        Cash = 1,
        [EnumMember]
        Cheque = 2,
        [EnumMember]
        Voucher = 3
    }
}