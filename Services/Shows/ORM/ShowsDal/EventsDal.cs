using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using TicketingOffice.Common.Properties;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common;
using TicketingOffice.Common.Exceptions;

namespace TicketingOffice.ShowsService.DataAccess
{
    /// <summary>
    /// The data access layer for managing Ticketing Events. 
    /// The EventsDal exposes orders as defined in the contracts assembly.
    /// The dal is based on Entity Framework
    /// </summary>
    public class EventDal : IEntityCRUD<Contracts.Event,Guid>
    {

        /// <summary>
        /// Retrieve a collection of Ticketing Events according to criteria parameter.
        /// The criteria is used to create the where statement fot the internal EF query.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Contracts.Event[] GetEventsByCriteria(Contracts.EventCriteria criteria)
        {
            Contracts.Event[] result = null;
            try
            {
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    IQueryable<Event> query;
                    query = ctx.Events.Include("Show");

                    if (criteria.Date != null)
                        query = query.Where(ev => ev.Date == criteria.Date);
                    if (criteria.ShowID != null)
                        query = query.Where(ev => ev.Show.ID == criteria.ShowID);
                    if (criteria.State != null)
                    {
                        var stateStr = criteria.State.ToString(); // Solves linq to EF issue.Linq to EF cannot parse the ToString into the expression tree.
                        query = query.Where(ev => ev.State == stateStr);
                    }
                    if (criteria.TheaterID != null)
                        query = query.Where(ev => ev.TheaterID == criteria.TheaterID);
                    if ((criteria.FromDate != null) && (criteria.ToDate != null))
                        query = query.Where(ev => ((ev.Date >= criteria.FromDate) && (ev.Date <= criteria.ToDate)));

                    var tmp = query.ToArray();

                    result = tmp.Select(ev => Mapper.MapEvent(ev)).ToArray();                    

                }

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Event", ex.Message), ex) { EntityType = typeof(Event) };
            }

            return result;

        }


                

       #region IEntityCRUD<Event> Members

        /// <summary>
        /// Retrieve an event using its ID
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public Contracts.Event GetEntity(Guid entityID)
        {
            Contracts.Event res = null;
            try
            {
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {                    
                    var tmp = (from ev in ctx.Events.Include("Show")
                                where ev.ID == entityID
                                select ev).FirstOrDefault();

                    if (tmp != null)
                        res = Mapper.MapEvent(tmp);
                }

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Event", ex.Message), ex) { EntityType = typeof(Event) }; 
            }

            return res;
            
        }

        /// <summary>
        /// Insert a new Ticketing event
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Guid CreateEntity(Contracts.Event entity)
        {
            try
            {
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    Show show = ctx.Shows.Where(sh => sh.ID == entity.ShowDetails.ShowID).FirstOrDefault();
                    if (show == null)
                        throw new DalException(string.Format(StringsResource.ShowNotFound,entity.ShowDetails.ShowID));

                    Event newEvent = new Event()
                    {
                        Date = entity.Date,
                        ID = entity.EventID,
                        ListPrice = (int)entity.ListPrice,
                        StartTime = entity.StartTime,
                        State = entity.State.ToString(),
                        PricingPolicy = entity.PricingPolicy,
                        Show = show,
                        TheaterID = entity.TheaterID
                    };

                    ctx.Events.AddObject(newEvent);
                    ctx.SaveChanges();

                    return newEvent.ID;
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.InsertError, "Event", ex.Message), ex) { EntityType = typeof(Event) };
            }
           
        }

        /// <summary>
        /// Update an existing event. If the event does not exist it will be created
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(Contracts.Event entity)
        {
            try
            {
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    var eventToUpdate = ctx.Events.Where(ev => ev.ID == entity.EventID).FirstOrDefault();
                    if (eventToUpdate == null)
                    {
                        CreateEntity(entity);
                        return; 
                    }
                    eventToUpdate.Date = entity.Date;
                    eventToUpdate.ListPrice = (int)entity.ListPrice;
                    eventToUpdate.StartTime = entity.StartTime;
                    eventToUpdate.State = entity.State.ToString();
                    eventToUpdate.PricingPolicy = entity.PricingPolicy;

                    ctx.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.UpdateError, "Event", ex.Message), ex) { EntityType = typeof(Event) };
            }
        }

        /// <summary>
        /// Delete an existing ticketing event
        /// </summary>
        /// <param name="entityID"></param>
        public void DeleteEntity(Guid entityID)
        {
            try
            {
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    var eventToDelete = ctx.Events.Where(ev => ev.ID == entityID).FirstOrDefault();
                    if (eventToDelete == null)
                        return;

                    ctx.Events.DeleteObject(eventToDelete);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.DeleteError, "Event", ex.Message), ex) { EntityType = typeof(Event) };
            }
        }

        #endregion
    }

    



}
