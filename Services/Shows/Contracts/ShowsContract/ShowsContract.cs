using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace TicketingOffice.ShowsService.Contracts
{
    /// <summary>
    /// The service contract for the shows service.
    /// This contract encapsulate all the shows and ticketing events operations.
    /// </summary>
    [ServiceContract(Namespace = "http://Fabrikam.com")]   
    public interface IShowsService
    {
        [OperationContract]     
        Show[] FindShowsByCriteria(ShowCriteria criteria);
        [OperationContract]
        Show FindShowByID(int showID);
        [OperationContract]      
        int CreateShow(Show newShow);
        [OperationContract]       
        void UpdateShow(Show newShow);
        [OperationContract]      
        void DeleteShow(int showID);
        [OperationContract]
        Event[] FindEventsByCrireria(EventCriteria criteria);
        [OperationContract]
        Event FindEventByID(Guid eventID);

        [OperationContract]
        Guid CreateEvent(Event newEvent);
        [OperationContract]
        void UpdateEvent(Event newEvent);
        [OperationContract]
        void DeleteEvent(Guid EventID);

        [OperationContract]
        ListPriceRecord[] ShowPrices(EventCriteria criteria);
        
    }
}
