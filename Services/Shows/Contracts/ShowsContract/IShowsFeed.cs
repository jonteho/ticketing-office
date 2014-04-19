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
    /// The contract for the shows RSS/ATOM feed
    /// </summary>
    [ServiceContract(Namespace = "http://Fabrikam.com")]
    [ServiceKnownType(typeof(Atom10FeedFormatter))]
    [ServiceKnownType(typeof(Rss20FeedFormatter))]
    public interface IShowsFeed
    {
        
        [OperationContract]
        [WebGet(UriTemplate = "*", BodyStyle = WebMessageBodyStyle.Bare)]
        SyndicationFeedFormatter GetShows();

        
    }
}
