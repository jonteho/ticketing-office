using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.Common;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Exceptions;
using TicketingOffice.Common.Properties;
using TicketingOffice.HallState.Contracts;

namespace TicketingOffice.HallState.DataAccess
{
    /// <summary>
    /// The data access layer for managing reservations. 
    /// The ReservationDal exposes reservations as defined in the contracts assembly.
    /// The dal is based on Entity Framework
    /// </summary>
    public class ReservationDal : IEntityCRUD<Contracts.Reservation,Guid>
    {       
       #region Helpers
        /// <summary>
        /// Object to object mapping between EF (ORM) entities and contract entities
        /// </summary>
        /// <param name="_theater"></param>
        /// <returns></returns>
        private Theater MapToOrmTheater(Contracts.Theater _theater)
        {
            return new Theater()
            {
                Capacity = _theater.Capacity,
                City = _theater.City,
                Country = _theater.Country,
                ID = _theater.ID,
                Map = _theater.Map,
                Name = _theater.Name
            };

        }

        /// <summary>
        /// Object to object mapping between EF (ORM) entities and contract entities
        /// </summary>
        /// <param name="_theater"></param>
        /// <returns></returns>
        private Contracts.Theater MapToContractsTheater(Theater _theater)
        {
            return new Contracts.Theater()
            {
                Capacity = _theater.Capacity,
                City = _theater.City,
                Country = _theater.Country,
                ID = _theater.ID,
                Map = _theater.Map,
                Name = _theater.Name,
            };
        }


        /// <summary>
        /// Object to object mapping between EF (ORM) entities and contract entities
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Contracts.Reservation MapToCintractsReservation(Reservation entity)
        {
            return new Contracts.Reservation()
            {
                ID = entity.ID,
                Seats = SeatIndex.ParseSeats(entity.Seats),
                EventID = entity.EventID,
                Remarks = entity.Remark,
                CustomerID = entity.CustomerID,
                TheaterInfo = MapToContractsTheater(entity.Theater)
            };

        }


        #endregion

        /// <summary>
        /// Retrieve a collection of reservations according to criteria parameter.
        /// The criteria is used to create the where statement fot the internal EF query. 
        /// </summary>
        /// <param name="criteria">The "where" criteria to retrieve entities</param>
        /// <returns></returns>
        public Contracts.Reservation[] GetReservationsByCriteria(ReservationCriteria criteria)
        {
            try
            {
                using (TicketingOfficeHallsEntities ctx = new TicketingOfficeHallsEntities())
                {
                    IQueryable<Reservation> query = ctx.Reservations.Include("Theater");
                    if (criteria.EventID != null)
                        query = query.Where(res => res.EventID == criteria.EventID);
                    if (criteria.HallID != null)
                        query = query.Where(res => res.Theater.ID == criteria.HallID);                   
                    if (criteria.CustomerID != null)
                        query = query.Where(res => res.CustomerID == criteria.CustomerID);

                    var tmp = query.Select(res => res).ToArray();

                    if (criteria.Seats != null)
                    {
                        return (from Reservation res in tmp
                                where SeatIndex.CompareSeates(SeatIndex.ParseSeats(res.Seats).ToArray(), criteria.Seats)
                                select MapToCintractsReservation(res)).ToArray();
                    }

                    return tmp.Select(res => MapToCintractsReservation(res)).ToArray();                                   
                }

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Reservation", ex.Message), ex) { EntityType = typeof(Reservation) };
            }
        }


       #region IEntityCRUD<Reservation,Guid> Members

        /// <summary>
        /// Get a reservation accoring to its id
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public Contracts.Reservation GetEntity(Guid entityID)
        {
            Contracts.Reservation res = null;
            try
            {
                using (TicketingOfficeHallsEntities ctx = new TicketingOfficeHallsEntities())
                {
                    var tmp = (from reserve in ctx.Reservations.Include("Theater")
                                where reserve.ID == entityID
                                select reserve).FirstOrDefault();
                    if (tmp != null) 
                        res = MapToCintractsReservation(tmp);
                }

            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.RetrieveError, "Reservation", ex.Message), ex) { EntityType = typeof(Reservation) };
            }

            return res;
        }
  
        /// <summary>
        /// Insert a new reservation
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Guid CreateEntity(Contracts.Reservation entity)
        {
            try
            {
                using (TicketingOfficeHallsEntities ctx = new TicketingOfficeHallsEntities())
                {

                    var theater = ctx.Theaters.Where(th => th.ID == entity.TheaterInfo.ID).FirstOrDefault();
                    if (theater == null)
                        throw new DalException(StringsResource.ReservationFailedTheaterNotFound);

                    Reservation newReservation = new Reservation()
                    {
                        ID = entity.ID,
                        Seats = SeatIndex.CreateSeatsString(entity.Seats),
                        Remark = entity.Remarks, 
                        CustomerID = entity.CustomerID,
                        EventID = entity.EventID,
                        Theater = theater
                    };

                    ctx.Reservations.AddObject(newReservation);
                    ctx.SaveChanges();
                    return newReservation.ID;
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.InsertError, "Reservation", ex.Message), ex) { EntityType = typeof(Reservation) };
            }
        }

        /// <summary>
        /// Update an existing reservation
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(Contracts.Reservation entity)
        {
            try
            {
                using (TicketingOfficeHallsEntities ctx = new TicketingOfficeHallsEntities())
                {
                    var ReservationToUpdate = ctx.Reservations.Where(reserv => reserv.ID == entity.ID).FirstOrDefault();
                    if (ReservationToUpdate == null)
                    {
                        CreateEntity(entity);
                        return;
                    }

                    ReservationToUpdate.Seats = SeatIndex.CreateSeatsString(entity.Seats);                   
                    ReservationToUpdate.CustomerID = entity.CustomerID;
                    ReservationToUpdate.EventID = entity.EventID;
                    
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.UpdateError, "Reservation", ex.Message), ex) { EntityType = typeof(Reservation) };
            }
        }

        /// <summary>
        /// Delete an existing resrevation
        /// </summary>
        /// <param name="entityID"></param>
        public void DeleteEntity(Guid entityID)
        {
            try
            {
                using (TicketingOfficeHallsEntities ctx = new TicketingOfficeHallsEntities())
                {
                    var reservationToDelete = ctx.Reservations.Where(reserv => reserv.ID == entityID).FirstOrDefault();
                    if (reservationToDelete == null)
                        return;

                    ctx.Reservations.DeleteObject(reservationToDelete);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new DalException(string.Format(StringsResource.DeleteError, "Reservation", ex.Message), ex) { EntityType = typeof(Reservation) };
            }
        }

       #endregion
    }
}
