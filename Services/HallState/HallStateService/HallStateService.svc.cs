using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.HallState.Contracts;
using TicketingOffice.HallState.BusinessLogic;
using TicketingOffice.ShowsService.Contracts;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Properties;
using System.Threading;

namespace TicketingOffice.HallStateService
{
    /// <summary>
    /// The hall service implement both reservation and hall state and management logic. 
    /// The InstanceContextMode is single because the service holds state. (Cache inside the HallStateManager).
    /// In reality try to avoid stateful services. This is done for demonstration only.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, Namespace = "http://Fabrikam.com")]      
    public class ReservationsService : IHallStateService, IReservationService, IHallManagementService
    {
        IHallStateManager manager = new HallStateManager();
        ChannelFactory<IShowsService> chf;

        #region IHallStateService Members

        public SeatIndex[] GetHallState(Guid eventID)
        {
            return manager.GetHallState(eventID);
        }

        public Theater FindTheater(string name, int? theaterID)
        {
            if (theaterID != null)
            {
                if (theaterID < 0)
                    throw new ArgumentException(StringsResource.InvalidTheaterID);
                return manager.FindTheater(theaterID.Value);
            }

            return manager.FindTheater(name);
        }

        #endregion

        #region IHallManagementService Members

        /// <summary>
        /// Create a new hall
        /// </summary>
        /// <param name="theater"></param>
        /// <returns></returns>
        public int CreateTheater(Theater theater)
        {
            return manager.CreateTheater(theater);
        }

        /// <summary>
        /// delete an existing hall
        /// </summary>
        /// <param name="theaterID"></param>
        public void DeleteTheater(int theaterID)
        {
            //First delete the reservations and then delete the hall.
            var reservations = FindReservations(new ReservationCriteria() { HallID = theaterID });
            foreach (var item in reservations)
                manager.DeleteResrvation(item.ID);

            manager.DeleteTheater(theaterID);
        }

        /// <summary>
        /// Update the hall details
        /// </summary>
        /// <param name="newTheater"></param>
        public void UpdateTheater(Theater newTheater)
        {
            manager.UpdateTheater(newTheater);
        }

        #endregion

        #region IReservationService Members

        /// <summary>
        /// Create a new reservation
        /// Reservations must live inside a transaction
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        [OperationBehavior(TransactionScopeRequired = true)]
        public Guid CreateResevation(Reservation reservation)
        {
            int hallID;
            #region Validate Event
            IShowsService prox = null;
            Event _event;

            //1. contact the shows service to fetch information about the event.
            lock (this)
            {
                if (chf == null)
                    chf = new ChannelFactory<IShowsService>("ShowEP");
            }

            try
            {
                prox = chf.CreateChannel();
                _event = prox.FindEventByID(reservation.EventID);
                hallID = _event.TheaterID;
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new HallStateException(StringsResource.FailedToContactShows, ex);
            }
            finally
            {
                var channel = prox as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }

            //2. Validate the reservation using the event data.
            if ((_event == null) || (_event.State != EventState.Opened))
                throw new ReservationException(StringsResource.InvalidOrClosedEvent);
            #endregion

            //Fill in theater details.
            if (reservation.TheaterInfo == null)
                reservation.TheaterInfo = manager.FindTheater(hallID);

            //3. Create the reservation
            return manager.CreateResrvation(reservation);
        }

        /// <summary>
        /// Select a single reservation
        /// </summary>
        /// <param name="showName"></param>
        /// <param name="date"></param>
        /// <param name="HallID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Reservation FindReservation(string showName, DateTime date, int? HallID, Guid customerID)
        {
            IShowsService prox = null;


            //1. contact the shows service to fetch information about the event.
            lock (this)
            {
                if (chf == null)
                    chf = new ChannelFactory<IShowsService>("ShowEP");
            }

            try
            {
                prox = chf.CreateChannel();
                //1. Find the show and the event
                var _show = prox.FindShowsByCriteria(new ShowCriteria() { Title = showName });

                // If there is no show there is no reservation.
                if ((_show == null) || (_show.Count() == 0))
                    return null;

                var eventQuery = _show.First().Events.Where(ev => ev.Date == date);


                if (HallID != null)
                    eventQuery = eventQuery.Where(ev => ev.TheaterID == HallID);

                if (eventQuery.Count() != 1)
                    throw new HallStateException(StringsResource.EventInfoNotFound);

                var eventID = eventQuery.FirstOrDefault().EventID;

                //2. Find the resrevation according to the eventID
                var reservations = manager.FindReservations(new ReservationCriteria() { EventID = eventID });

                //3. Find the reservation of the client
                return reservations.Where(res => res.CustomerID == customerID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new HallStateException(StringsResource.FailedToContactShows + " " + ex.Message, ex);
            }
            finally
            {
                var channel = prox as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
        }


        /// <summary>
        /// Select a collection of reservations according to a criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Reservation[] FindReservations(ReservationCriteria criteria)
        {
            return manager.FindReservations(criteria);
        }

        /// <summary>
        /// Delete an existing reservation
        /// Reservations must live inside a transaction
        /// </summary>
        /// <param name="ID"></param>
        [OperationBehavior(TransactionScopeRequired = true)]
        public void DeleteResrevation(Guid ID)
        {
            manager.DeleteResrvation(ID);
        }

        /// <summary>
        /// Update an existing reservation
        /// Reservations must live inside a transaction
        /// </summary>
        /// <param name="newReservation"></param>
        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateReservation(Reservation newReservation)
        {
            int hallID;
            #region Validate Event
            IShowsService prox = null;
            Event _event;

            //1. contact the shows service to fetch information about the event.
            lock (this)
            {
                if (chf == null)
                    chf = new ChannelFactory<IShowsService>("ShowsEP");
            }

            try
            {
                prox = chf.CreateChannel();
                _event = prox.FindEventByID(newReservation.EventID);
                hallID = _event.TheaterID;
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new HallStateException(StringsResource.FailedToContactShows + " " + ex.Message, ex);
            }
            finally
            {
                var channel = prox as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }

            //2. Validate the reservation using the event data.
            if ((_event == null) || (_event.State != EventState.Opened))
                throw new ReservationException(StringsResource.InvalidOrClosedEvent);
            #endregion

            //Fill in theater details.
            if (newReservation.TheaterInfo == null)
                newReservation.TheaterInfo = manager.FindTheater(hallID);

            manager.UpdateResrvation(newReservation);
        }


        #endregion    
       
    }



}
