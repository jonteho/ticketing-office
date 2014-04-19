using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketingOffice.ShowsService.BusinessLogic
{
    public interface IEventManager
    {
        Contracts.Event[] FindEvents(Contracts.EventCriteria criteria);
        Contracts.Event FindEvent(Guid eventID);
        Guid CreateEvent(Contracts.Event eventTocreate);
        void DeleteEvent(Guid eventID);
        void UpdateEvent(Contracts.Event newEvent);
       

    }
}
