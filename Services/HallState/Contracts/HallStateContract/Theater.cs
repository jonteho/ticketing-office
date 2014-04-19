using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.HallState.Contracts
{
    /// <summary>
    /// Theater information
    /// </summary>
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public class Theater
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int? Capacity { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public byte[] Map { get; set; } //Road map to guide customers to the theater.
     
        public Theater(){}       
    }
}
