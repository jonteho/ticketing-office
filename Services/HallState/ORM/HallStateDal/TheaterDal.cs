using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.Common.Properties;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common;
using TicketingOffice.Common.Exceptions;

namespace TicketingOffice.HallState.DataAccess
{
    /// The data access layer for managing theater information. 
    /// The TheaterDal exposes theater (hall) as defined in the contracts assembly.
    /// The dal is based on Entity Framework
    public class TheaterDal : IEntityCRUD<Contracts.Theater, int>
    {
        #region IEntityCRUD<Theater,int> Members

        /// <summary>
        /// Get the theater info according to its id
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public Contracts.Theater GetEntity(int entityID)
        {
            Contracts.Theater res;
            try
            {
                using (TicketingOfficeHallsEntities ctx = new TicketingOfficeHallsEntities())
                {
                    res = (from th in ctx.Theaters
                           where th.ID == entityID
                           select new Contracts.Theater()
                           {
                             Capacity = th.Capacity,
                             City = th.City,
                             Country = th.Country,
                             ID = th.ID,
                             Map = th.Map,
                             Name = th.Name
                           }).FirstOrDefault();
                }
                                
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Theater",ex.Message), ex) { EntityType = typeof(Theater) };
  
            }
            return res;
        }


        /// <summary>
        /// Get the theater info according to its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Contracts.Theater GetEntity(string name)
        {
            Contracts.Theater res;
            try
            {
                using (TicketingOfficeHallsEntities ctx = new TicketingOfficeHallsEntities())
                {
                    res = (from th in ctx.Theaters
                           where th.Name == name
                           select new Contracts.Theater()
                           {
                               Capacity = th.Capacity,
                               City = th.City,
                               Country = th.Country,
                               ID = th.ID,
                               Map = th.Map,
                               Name = th.Name
                           }).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Theater", ex.Message), ex) { EntityType = typeof(Theater) };

            }
            return res;
        }

        /// <summary>
        /// Insert a newn theater
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int CreateEntity(Contracts.Theater entity)
        {
            try
            {
                using (TicketingOfficeHallsEntities ctx = new TicketingOfficeHallsEntities())
                {
                    Theater newTheater = new Theater()
                    {
                        Capacity = entity.Capacity,
                        City = entity.City,
                        Country = entity.Country,            
                        Map = entity.Map,
                        Name = entity.Name
                    };
                    ctx.Theaters.AddObject(newTheater);
                    ctx.SaveChanges();

                    return newTheater.ID;
                }               
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.InsertError, "Theater", ex.Message), ex) { EntityType = typeof(Theater) };
  
            }
        }


        /// <summary>
        /// Update an existing theater
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(Contracts.Theater entity)
        {
            try
            {
                using (TicketingOfficeHallsEntities ctx = new TicketingOfficeHallsEntities())
                {
                    var TheaterToUpdate = ctx.Theaters.Where(th => th.ID == entity.ID).FirstOrDefault();
                    if (TheaterToUpdate == null)
                    {
                        CreateEntity(entity);
                        return;
                    }

                    TheaterToUpdate.Capacity = entity.Capacity;
                    TheaterToUpdate.City = entity.City;
                    TheaterToUpdate.Country = entity.Country;
                    TheaterToUpdate.Map = entity.Map;
                    TheaterToUpdate.Name = entity.Name;                     

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.UpdateError, "Theater", ex.Message), ex) { EntityType = typeof(Theater) };
  
            }
        }

        /// <summary>
        /// Delete an existing theater
        /// </summary>
        /// <param name="entityID"></param>
        public void DeleteEntity(int entityID)
        {
            try
            {
                using (TicketingOfficeHallsEntities ctx = new TicketingOfficeHallsEntities())
                {
                    var theaterToDelete = ctx.Theaters.Where(th => th.ID == entityID).FirstOrDefault();
                    if (theaterToDelete == null)
                        return;

                    ctx.Theaters.DeleteObject(theaterToDelete);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.DeleteError, "Theater", ex.Message), ex) { EntityType = typeof(Theater) };
            }
        }

        #endregion
    }
}
