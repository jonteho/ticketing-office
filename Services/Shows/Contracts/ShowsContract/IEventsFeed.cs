using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Text;

namespace TicketingOffice.ShowsService.Contracts
{
    /// <summary>
    /// The contract for the Ticketing events RSS/ATOM feed 
    /// </summary>
    [ServiceContract(Namespace = "http://Fabrikam.com")]
    [ServiceKnownType(typeof(Atom10FeedFormatter))]
    [ServiceKnownType(typeof(Rss20FeedFormatter))]
    public interface IEventsFeed
    {

        [OperationContract]      
        [WebGet(UriTemplate = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        SyndicationFeedFormatter GetAllEvents();

        [OperationContract]
        [WebGet(UriTemplate = "{showname}", BodyStyle = WebMessageBodyStyle.Bare)]       
        SyndicationFeedFormatter GetEvents(string showName);

        [OperationContract]
        [WebGet(UriTemplate = "{showname}/Date/{from}/{to}", BodyStyle = WebMessageBodyStyle.Bare)]
        SyndicationFeedFormatter GetEventsPerDate(string showName, string from, string to);    
               
    }
}
