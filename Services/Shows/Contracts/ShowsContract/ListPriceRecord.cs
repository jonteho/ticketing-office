using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TicketingOffice.HallState.Contracts;

namespace TicketingOffice.ShowsService.Contracts
{
    /// <summary>
    /// Data contract for presenting list-price of a ticketing event 
    /// The final price is calculated based on the list-price.
    /// </summary>
    [DataContract(Namespace = "http://Fabrikam.com")]
    public class ListPriceRecord
    {
        [DataMember]
        public string ShowName { get; set; }
        [DataMember]
        public DateTime EventDate { get; set; }
        [DataMember]
        public Theater TheaterDetails { get; set; }
        [DataMember]
        public int ListPrice { get; set; }
    }
}
