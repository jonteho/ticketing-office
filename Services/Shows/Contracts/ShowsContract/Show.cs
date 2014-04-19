using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.ShowsService.Contracts
{
    /// <summary>
    /// Data contract for show
    /// </summary>
    [DataContract(Namespace = @"http://Fabrikam.com")]
    public class Show
    {
        [DataMember]
        public int ShowID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public Uri DetailsLink { get; set; }
        [DataMember]
        public Uri Preview { get; set; }
        [DataMember]
        public string Cast { get; set; }
        [DataMember]
        public TimeSpan Duration { get; set; }
        [DataMember]
        public Event[] Events { get; set; }
        [DataMember]
        public byte[] Logo { get; set; }   
       
    }
}
