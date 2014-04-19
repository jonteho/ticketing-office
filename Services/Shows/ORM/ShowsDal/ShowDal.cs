using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Properties;
using TicketingOffice.Common;
using TicketingOffice.Common.Exceptions;

namespace TicketingOffice.ShowsService.DataAccess
{
    /// <summary>
    /// The data access layer for managing Shows. 
    /// The ShowDal exposes orders as defined in the contracts assembly.
    /// The dal is based on Entity Framework
    /// </summary>
    public class ShowDal : IEntityCRUD<Contracts.Show,int>
    {
        /// <summary>
        /// Retrieve a collection of shows according to criteria parameter.
        /// The criteria is used to create the where statement fot the internal EF query.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Contracts.Show[] GetShowsByCriteria(Contracts.ShowCriteria criteria)
        {
            Contracts.Show[] result = null;
            try
            {
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    IQueryable<Show> query;
                    query = ctx.Shows.Include("Events");

                    if (!string.IsNullOrEmpty(criteria.Title))
                        query = query.Where(sh => sh.Title == criteria.Title);                    
                    if (criteria.Duration != null)
                        query = query.Where(sh => sh.Duration == criteria.Duration);
                    if (criteria.Category != null)
                        query = query.Where(sh => sh.Category == criteria.Category);

                    var tmp = query.ToArray();

                    if (!string.IsNullOrEmpty(criteria.Cast))
                        result = (from sh in tmp
                                  where FindMatch(sh.Cast, criteria.Cast)
                                  select Mapper.MapShow(sh)).ToArray();
                    else
                        result = tmp.Select(sh => Mapper.MapShow(sh)).ToArray();                    
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Show", ex.Message), ex) { EntityType = typeof(Show) };
            }
            return result;
        }

        /// <summary>
        /// Delete all the shows that match the criteria
        /// </summary>
        /// <param name="criteria"></param>
        public void DeleteShowsByCriteria(Contracts.ShowCriteria criteria)
        {
           
            try
            {
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    IQueryable<Show> query;
                    query = ctx.Shows.Include("Events");

                    if (!string.IsNullOrEmpty(criteria.Title))
                        query = query.Where(sh => sh.Title == criteria.Title);
                    if (criteria.Duration != null)
                        query = query.Where(sh => sh.Duration == criteria.Duration);
                    if (criteria.Category != null)
                        query = query.Where(sh => sh.Category == criteria.Category);
                    
                    foreach (var item in query)
                    {
                        ctx.Shows.DeleteObject(item); 
                    }

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Show", ex.Message), ex) { EntityType = typeof(Show) };
            }
        }

        /// <summary>
        /// Helper method to find match between shows cast. 
        /// If there is one actor that plays in both shows there is a match.
        /// </summary>
        /// <param name="castStr1">Show1 actor names seperated by ',' </param>
        /// <param name="castStr2">Show2 actor names seperated by ','</param>
        /// <returns></returns>
        private bool FindMatch(string castStr1, string castStr2)
        {
            if ((string.IsNullOrEmpty(castStr1)) || (string.IsNullOrEmpty(castStr2)))
                return false;
            if (castStr1 == castStr2)
                return true;
            string[] words = castStr1.Split(',');
             
            foreach (var item in words)
        	{
        		 if (castStr2.Contains(item))
                     return true;
        	}           

            return false; 
        }


        #region IEntityCRUD<Show,Guid> Members

        /// <summary>
        /// Retrieve a show using its ID
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public Contracts.Show GetEntity(int entityID)
        {
            Contracts.Show res = null; 
            try
            {                
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    var tmp = (from sh in ctx.Shows.Include("Events")
                                where sh.ID == entityID
                                select sh).FirstOrDefault();

                    if (tmp != null)
                        res = Mapper.MapShow(tmp);
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Show", ex.Message), ex) { EntityType = typeof(Show) };
             }
            return res;
        }

        /// <summary>
        /// Insert a new show
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int CreateEntity(Contracts.Show entity)
        {
            try
            {
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    Show newShow = new Show()
                    {
                        Cast = entity.Cast,
                        Description = entity.Description,               
                        Duration = (int?)entity.Duration.TotalMinutes,                       
                        Title = entity.Title,
                        Category = entity.Category, 
                        Url = @"http://empty.com",
                        Preview = @"http://empty.com"                        
                    };

                    if (entity.Preview != null)
                        newShow.Preview = entity.Preview.AbsoluteUri;                  
                    if ((entity.Logo != null) && (entity.Logo.Length > 0))
                        newShow.Logo = entity.Logo;
                    if (entity.DetailsLink != null)
                        newShow.Url = entity.DetailsLink.AbsoluteUri;

                    ctx.Shows.AddObject(newShow);
                    ctx.SaveChanges();

                    return newShow.ID;
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.InsertError, "Show", ex.Message), ex) { EntityType = typeof(Show) };
 
            }
        }

        /// <summary>
        /// Update an existing show. If the show does not exist it will be created
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(Contracts.Show entity)
        {
            try
            {
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    var showToUpdate = ctx.Shows.Where(sh => sh.ID == entity.ShowID).FirstOrDefault();
                    if (showToUpdate == null)
                    {
                        CreateEntity(entity);
                        return;
                    }
                    showToUpdate.Cast = entity.Cast;
                    showToUpdate.Description = entity.Description;
                    showToUpdate.Duration = (int?)entity.Duration.TotalMinutes;
                    showToUpdate.Logo = entity.Logo;
                    showToUpdate.Title = entity.Title;
                    showToUpdate.Category = entity.Category;
                    showToUpdate.Preview = (entity.Preview != null) ? entity.Preview.AbsoluteUri : @"http://empty.com";
                    showToUpdate.Url = (entity.DetailsLink != null) ? entity.DetailsLink.AbsoluteUri : @"http://empty.com";

                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.UpdateError, "Show", ex.Message), ex) { EntityType = typeof(Show) };
             }
        }

        /// <summary>
        /// Delete an existing show
        /// </summary>
        /// <param name="entityID"></param>
        public void DeleteEntity(int entityID)
        {
            try
            {
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    var showToDelete = ctx.Shows.Where(sh => sh.ID == entityID).FirstOrDefault();
                    if (showToDelete == null)
                        return;

                    //First delete all the events
                    if (showToDelete.Events.Count > 0)
                    {
                        EventDal evDal = new EventDal();
                        var ids = showToDelete.Events.Select(ev => ev.ID);
                        foreach (var id in ids)
                        {                           
                            evDal.DeleteEntity(id);                            
                        }
                        ctx.SaveChanges();                    
                    }
                }
                    
                using (TicketingOfficeShowEntities ctx = new TicketingOfficeShowEntities())
                {
                    var showToDelete = ctx.Shows.Where(sh => sh.ID == entityID).FirstOrDefault();
                    ctx.Shows.DeleteObject(showToDelete);
                    ctx.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.DeleteError, "Show", ex.Message), ex) { EntityType = typeof(Show) };
            }
        }

        #endregion
    }
}
