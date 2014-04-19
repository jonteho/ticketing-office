using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.Common;
using TicketingOffice.ShowsService.Contracts;
using TicketingOffice.ShowsService.DataAccess;


namespace TicketingOffice.ShowsService.BusinessLogic
{
    /// <summary>
    /// Execute ticketing event related logic. 
    /// In this simple example there is only call to data access logic.
    /// </summary>
    public class EventManager : IEventManager
    {

        #region IEventManager Members

        public Contracts.Event[] FindEvents(Contracts.EventCriteria criteria)
        {
            EventDal dal = new EventDal();
            return dal.GetEventsByCriteria(criteria);
        }


        public Contracts.Event FindEvent(Guid eventID)
        {
            EventDal dal = new EventDal();
            return dal.GetEntity(eventID);
        }

        public Guid CreateEvent(Contracts.Event eventTocreate)
        {
            EventDal dal = new EventDal();
            return dal.CreateEntity(eventTocreate);
        }

        public void DeleteEvent(Guid eventID)
        {
            EventDal dal = new EventDal();
            dal.DeleteEntity(eventID);
        }

        public void UpdateEvent(Contracts.Event newEvent)
        {
            EventDal dal = new EventDal();
            dal.UpdateEntity(newEvent);
        }

        #endregion
    }
}
