using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using TicketingOffice.HallState.Contracts;
using TicketingOffice.HallState.DataAccess;
using TicketingOffice.Common.Properties;
using System.Threading;
using System.Web;
using TicketingOffice.Common.Helpers;


namespace TicketingOffice.HallState.BusinessLogic
{
    /// <summary>
    /// Execute hall state and reservation logic
    /// </summary>
    public class HallStateManager : IHallStateManager
    {
        // The cache holds list of resrevations per eventID.
        // The cache also holds theater info per theaterID.
        public static Cache HallCache { get; set; }        
        public static ReaderWriterLock rwl;
        Func<Guid, SeatIndex[]> getHallState_delegate;
        Func<string, Contracts.Theater> findTheater_delegate;


        public HallStateManager()
        {
            HallCache = HttpRuntime.Cache;
            rwl = new ReaderWriterLock();
        }
        

       #region IHallStateManager Members
        /// <summary>
        /// Get the hall state which is a collection of occupied seats. 
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
        public SeatIndex[] GetHallState(Guid eventID)
        {

            LoggingManager.Logger.Log(LoggingCategory.Info,
                 string.Format(StringsResource.AsyncExecution, "GetHallState", Thread.CurrentThread.ManagedThreadId));
 
           var result = new List<SeatIndex>();
           List<Contracts.Reservation> reservations;
           
            //If the hall state is in the cache read it from the cache.   
           rwl.AcquireReaderLock(new TimeSpan(1, 0, 0));
                reservations = (HallCache.Get(eventID.ToString()) as List<Contracts.Reservation>);
            rwl.ReleaseReaderLock();
           
           // if the info is not in the cache read it from the database and put it in the cache.
           if (reservations == null)
           {
              
               ReservationDal Dal = new ReservationDal();
               reservations = Dal.GetReservationsByCriteria(
                   new ReservationCriteria() { EventID = eventID }).ToList();

               rwl.AcquireWriterLock(new TimeSpan(1,0,0));
            		HallCache[eventID.ToString()] = reservations;
               rwl.ReleaseWriterLock();
           }

           foreach (var rs in reservations)
           {
               foreach (var si in rs.Seats)
               {
                   result.Add(si);
               } 
           }
                  
       
           return result.ToArray();
        }

        /// <summary>
        /// Call the GetHallState asynchronously
        /// Implementation of the async pattern.
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="cb"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IAsyncResult BeginGetHallState(Guid eventID, AsyncCallback cb, object state)
        {
            getHallState_delegate = new Func<Guid, SeatIndex[]>(GetHallState);
            return getHallState_delegate.BeginInvoke(eventID, cb, state);
        }


        /// <summary>
        /// Get the GetHallState results 
        /// Implementation of the async pattern
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        public SeatIndex[] EndGetHallState(IAsyncResult ar)
        {
            return getHallState_delegate.EndInvoke(ar);
        }


        /// <summary>
        /// Find Theater by name asynchronously
        /// Implementation of the async pattern (Begin invoke)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cb"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IAsyncResult BeginFindTheater(string name, AsyncCallback cb, object state)
        {
            findTheater_delegate = new Func<string, Contracts.Theater>(FindTheater);
            return findTheater_delegate.BeginInvoke(name, cb, state);
        }


        /// <summary>
        /// Find Theater by name asynchronously
        /// Implementation of the async pattern (End Invoke)
        /// </summary>
        /// <param name="ar"></param>
        /// <returns></returns>
        public Contracts.Theater EndFindTheater(IAsyncResult ar)
        {
            return findTheater_delegate.EndInvoke(ar);
        }
        
        /// <summary>
        /// Submit a new reservation
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public Guid CreateResrvation(Contracts.Reservation reservation)
        {
            // Create the event in the database
            ReservationDal reservationDal = new ReservationDal();
            TheaterDal theaterDal = new TheaterDal();

            //1. validate
            foreach (var si in reservation.Seats)
            {
                if ((si.Row < 0) || (si.Seat < 0))
                    throw new ArgumentException(StringsResource.InvalidSeatOrRow);

                //Check that there is no other reservation for the same seats               
                var takenSeats = GetHallState(reservation.EventID);
                foreach (var item in reservation.Seats)
            	{
            		 if (takenSeats.Contains(item))
                         throw new ReservationException(StringsResource.SeatAlreadyTaken);
            	}
               
            }

            //2. Create an ID if needed. 
            if (reservation.ID == Guid.Empty)
                reservation.ID = Guid.NewGuid(); 
            
            //3. Create the reservation
            Guid id = reservationDal.CreateEntity(reservation);

            //4. Add it to the cache.
           rwl.AcquireWriterLock(new TimeSpan(1,0,0));

                if (HallCache[reservation.EventID.ToString()] == null)
                    HallCache.Add(reservation.EventID.ToString(), new List<Contracts.Reservation>() { reservation },
                         null,DateTime.Now.AddDays(30),TimeSpan.FromHours(5),CacheItemPriority.Normal,null);
                 else
                    (HallCache[reservation.EventID.ToString()] as List<Contracts.Reservation>).Add(reservation);
           
            rwl.ReleaseWriterLock();

            return id;
        }


        public void UpdateResrvation(Contracts.Reservation newReservation)
        {
            ReservationDal reservationDal = new ReservationDal();
            
            // validate
            Contracts.Reservation OldRersevation = reservationDal.GetEntity(newReservation.ID);
            if (OldRersevation == null)
                throw new HallStateException(string.Format(StringsResource.ReservationNotFound, newReservation.ID));
            
            var oldseats = OldRersevation.Seats;
           
            foreach (var si in newReservation.Seats)
            {
                if ((si.Row < 0) || (si.Seat < 0))
                    throw new ArgumentException(StringsResource.InvalidSeatOrRow);

                //Check that there is no other reservation for the same seats               
                var takenSeats = GetHallState(newReservation.EventID);
                foreach (var item in newReservation.Seats)
                {
                    if (takenSeats.Contains(item) && (!oldseats.Contains(item)))
                        throw new ReservationException(StringsResource.SeatAlreadyTaken);
                }

            }

            rwl.AcquireWriterLock(new TimeSpan(1, 0, 0));

            //Update the database          
            reservationDal.UpdateEntity(newReservation);

            //2. Upfdate the cache
            var cache = HallCache[newReservation.EventID.ToString()] as List<Contracts.Reservation>;
            var reservationToUpdate = cache.Where(res => res.ID == newReservation.ID).FirstOrDefault();
            reservationToUpdate = newReservation;

            rwl.ReleaseWriterLock();
        }

                
        public void DeleteResrvation(Guid reservationID)
        {
            ReservationDal reservationDal = new ReservationDal();
            var entity = reservationDal.GetEntity(reservationID);
            if (entity != null)
            {

                rwl.AcquireWriterLock(new TimeSpan(1,0,0));
                var reservationsInCache = (HallCache[entity.EventID.ToString()] as List<Contracts.Reservation>);
                if (reservationsInCache != null)
                    reservationsInCache.RemoveAll(rv => (SeatIndex.CreateSeatsString(rv.Seats) == SeatIndex.CreateSeatsString(entity.Seats)) || 
                                                             ((rv.ID == entity.ID)));                

                reservationDal.DeleteEntity(reservationID);
                rwl.ReleaseWriterLock();
            }
        }

        
        public Contracts.Reservation[] FindReservations(ReservationCriteria criteria)
        {
            ReservationDal reservationDal = new ReservationDal();

            return reservationDal.GetReservationsByCriteria(criteria);
        }

        /// <summary>
        /// Find a theater according to name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>      
        public Contracts.Theater FindTheater(string name)
        {
            return InternalFindTheater(name);
        }

        
        /// <summary>
        /// Find a theater according to ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Contracts.Theater FindTheater(int ID)
        {
            return InternalFindTheater(ID.ToString());
        }

        /// <summary>
        /// Internal implementation of Find theater
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Contracts.Theater InternalFindTheater(string key)
        {
            Contracts.Theater result = null;
            
            // If the theater is in the cache return it from there
            rwl.AcquireReaderLock(new TimeSpan(1, 0, 0));
            if (HallCache[key] != null)
            {
                result = (HallCache[key] as Contracts.Theater);
                rwl.ReleaseReaderLock();
                return result;
            }
            rwl.ReleaseReaderLock();
            //Read the theater info from the DB
            TheaterDal dal = new TheaterDal();
            int id;
           
            // look in the data access according to the id.
            if (int.TryParse(key,out id))
                result = dal.GetEntity(id);

             // look in the data access according to the name
            if (result == null)
                result = dal.GetEntity(key);

            //Write it to the cache
            if (result != null)
            {
                rwl.AcquireWriterLock(new TimeSpan(1, 0, 0));
                HallCache.Add(result.ID.ToString(), result, null, DateTime.Now.AddYears(1),
                   Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                rwl.ReleaseWriterLock();
            }
           
            return result;
        }
           
  
        public int CreateTheater(Contracts.Theater newTheater)
        {
            TheaterDal dal = new TheaterDal();
           
            // Add the theater to the cache
            rwl.AcquireWriterLock(new TimeSpan(1, 0, 0));
            if (HallCache[newTheater.ID.ToString()] != null)
                HallCache.Remove(newTheater.ID.ToString());
            HallCache.Add(newTheater.ID.ToString(), newTheater, null, DateTime.Now.AddYears(1),
              Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            rwl.ReleaseWriterLock();
            
            return dal.CreateEntity(newTheater);
        }


        public void UpdateTheater(Contracts.Theater newTheater)
        {
            TheaterDal dal = new TheaterDal();
            
            // Add the theater to the cache
            rwl.AcquireWriterLock(new TimeSpan(1, 0, 0));
            if (HallCache[newTheater.ID.ToString()] != null)
                HallCache.Remove(newTheater.ID.ToString());
            HallCache.Add(newTheater.ID.ToString(), newTheater, null, DateTime.Now.AddYears(1),
             Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            rwl.ReleaseWriterLock();
            
            dal.UpdateEntity(newTheater);
        }


        public void DeleteTheater(int TheaterID)
        {
            TheaterDal dal = new TheaterDal();
            dal.DeleteEntity(TheaterID);

            // Remove the theater from the cache
            rwl.AcquireWriterLock(new TimeSpan(1, 0, 0));
            if (HallCache[TheaterID.ToString()] != null)
                HallCache.Remove(TheaterID.ToString());
            rwl.ReleaseWriterLock();
        }

        #endregion
    }
}
