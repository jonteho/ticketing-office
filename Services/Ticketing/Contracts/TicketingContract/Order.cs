using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TicketingOffice.ShowsService.Contracts;
using TicketingOffice.PaymentService.Contracts;


namespace TicketingOffice.TicketingService.Contracts
{
    /// <summary>
    /// The Order entity. 
    /// </summary>
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public class Order
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public Customer CustomerInfo { get; set; }
        [DataMember]
        public DateTime? DateOfPurchase { get; set; }
        [DataMember]
        public Event EventInfo { get; set; }
        [DataMember]
        public List<Payment> PaymentsInfo { get; set; }
        [DataMember]
        public Guid ReservationID { get; set; }
        [DataMember]
        public OrderState State { get; set; }
        [DataMember]
        public double TotalPrice { get; set; }
        [DataMember]
        public string Remarks { get; set; }

        public Order()
        {
            PaymentsInfo = new List<Payment>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("OrderID: {0}",ID.ToString()));
            sb.AppendLine(EventInfo.ShowDetails.Title);
            sb.AppendLine(string.Format("Event date: {0}, {1}", EventInfo.Date.ToShortDateString(), EventInfo.Date.ToShortTimeString()));
            sb.AppendLine(string.Format("Customer Name: {0} {1}", CustomerInfo.LastName, CustomerInfo.FirstName));
            sb.AppendLine(string.Format("Date of purchase: {0}",DateOfPurchase.Value.ToShortDateString()));
            foreach (Payment item in PaymentsInfo)
            {
                sb.AppendLine(string.Format("Payment {0} at {1}", item.Amount, item.Date));
            }
            sb.AppendLine(string.Format("Total Price: {0}", TotalPrice.ToString()));
            if (!string.IsNullOrEmpty(Remarks))
                sb.AppendLine(string.Format("Remarks: {0}:", Remarks));

            return sb.ToString();
        }
       
    }



    /// <summary>
    /// The state in which an order can be
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// The Order was only created
        /// </summary>
        Created,
        /// <summary>
        /// The order is approved. Usually as payment demands where satisfied
        /// </summary>
        Approved,
        /// <summary>
        /// The order is canceled
        /// </summary>
        Canceled
    }
}
