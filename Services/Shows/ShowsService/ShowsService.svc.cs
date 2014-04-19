using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.ShowsService.Contracts;
using TicketingOffice.ShowsService.BusinessLogic;
using TicketingOffice.HallState.Contracts;
using TicketingOffice.Common.Helpers;
using TicketingOffice.Common.Properties;
using TicketingOffice.ClientNotification;
using TicketingOffice.ClientNotification.Contract;
using TicketingOffice.Common.Behaviors;

namespace TicketingOffice.ShowsService
{

    public class ShowsAndEventsService : ServiceBase, IShowsService, INotificationManager, IRegisterForDuplexNotification
    {
        IEventManager eventManager = new EventManager();
        IShowsManager showsManager = new ShowManager();

        ChannelFactory<IHallStateService> chf;

        #region IShowsService Members

        public Show[] FindShowsByCriteria(ShowCriteria criteria)
        {
            //Clear the loop reference between show and event to solve serialization issues.
            //DataContract serialization does support loop reference but there is no business need to use it here.
            var result = showsManager.FindShows(criteria);

            foreach (Show sh in result)
            {
                foreach (Event ev in sh.Events)
                    ev.ShowDetails = null;
            }
            return result;
        }

        public Show FindShowByID(int showID)
        {
            //Clear the loop reference between show and event to solve serialization issues.
            //DataContract serialization does support loop reference but there is no business need to use it here.
            var result = showsManager.FindShow(showID);
            if (result != null)
                foreach (Event ev in result.Events)
                    ev.ShowDetails = null;

            return result;
        }


        public int CreateShow(Show newShow)
        {
            ValidateShow(newShow);

            int res = showsManager.CreateShow(newShow);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.ShowNotification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications(new ShowMessage()
                {
                    Content = "Show Created",
                    ShowID = newShow.ShowID
                }, clientRegistrations);

            #endregion

            return res;
        }

        private void ValidateShow(Show newShow)
        {
            if (newShow == null)
                throw new NullReferenceException(StringsResource.NullShow);

            if (string.IsNullOrEmpty(newShow.Title))
                throw new ArgumentException(StringsResource.ShowMustHaveAName);
        }


        public void UpdateShow(Show newShow)
        {
            ValidateShow(newShow);

            showsManager.UpdateShow(newShow);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.ShowNotification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications(new ShowMessage()
                {
                    Content = "Show Updated",
                    ShowID = newShow.ShowID
                }, clientRegistrations);

            #endregion
        }


        public void DeleteShow(int showID)
        {
            showsManager.DeleteShow(showID);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.ShowNotification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications(new ShowMessage()
                {
                    Content = "Show Deleted",
                    ShowID = showID
                }, clientRegistrations);
            #endregion
        }

        public Event[] FindEventsByCrireria(EventCriteria criteria)
        {
            var result = eventManager.FindEvents(criteria);

            //Clear the loop reference between show and event to solve serialization issues.
            //DataContract serialization does support loop reference but there is no business need to use it here.
            foreach (Event item in result)
            {
                item.ShowDetails.Events = null;
            }
            return result;
        }

        public Event FindEventByID(Guid eventID)
        {
            var result = eventManager.FindEvent(eventID);

            //Clear the loop reference between show and event to solve serialization issues.
            //DataContract serialization does support loop reference but there is no business need to use it here.
            if (result != null)
                result.ShowDetails.Events = null;
            return result;
        }

        public Guid CreateEvent(Event newEvent)
        {
            ValidateEvent(newEvent);

            #region Get Theater Details
            IHallStateService prox = null;
            lock (this)
            {
                if (chf == null)
                    chf = new ChannelFactory<IHallStateService>("HallStateEP");
            }

            try
            {
                prox = chf.CreateChannel();
                var theater = prox.FindTheater(string.Empty, newEvent.TheaterID);
                if (theater == null)
                    throw new NullReferenceException(string.Format(StringsResource.HallNotFound, newEvent.TheaterID));
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new HallStateException(string.Format(StringsResource.HallNotFound, newEvent.TheaterID), ex);
            }
            finally
            {
                var channel = prox as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
            #endregion

            var res = eventManager.CreateEvent(newEvent);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;


            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.EventsNoification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications(new EventMessage()
                {
                    Content = "Event created",
                    EventID = newEvent.EventID
                }, clientRegistrations);

            #endregion

            return res;
        }

        public void UpdateEvent(Event newEvent)
        {
            ValidateEvent(newEvent);

            eventManager.UpdateEvent(newEvent);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.EventsNoification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications(new EventMessage()
                {
                    Content = "Event Changed",
                    EventID = newEvent.EventID
                }, clientRegistrations);

            #endregion
        }

        public void DeleteEvent(Guid eventID)
        {
            eventManager.DeleteEvent(eventID);

            #region ClientNotifications
            //ClientNotifications
            string userName = string.Empty;
            if ((OperationContext.Current != null) && (OperationContext.Current.ServiceSecurityContext != null) && (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null))
                userName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            var clientRegistrations = MemoryRepository.Current.GetRegistrations(NotificationTypes.EventsNoification, userName);
            if (clientRegistrations.Length > 0)
                ClientNotificationsManager.SendNotifications(new EventMessage()
                {
                    Content = "Event deleted",
                    EventID = eventID
                }, clientRegistrations);

            #endregion
        }

        public ListPriceRecord[] ShowPrices(EventCriteria criteria)
        {
            var events = this.FindEventsByCrireria(criteria);
            return events.Select(ev => CreateListPriceRecord(ev)).ToArray();
        }

        private ListPriceRecord CreateListPriceRecord(Event ev)
        {
            IHallStateService prox = null;

            ListPriceRecord result = new ListPriceRecord()
            {
                ListPrice = (int)ev.ListPrice,
                EventDate = ev.Date,
                ShowName = ev.ShowDetails.Title

            };


            #region Get Theater Details
            lock (this)
            {
                if (chf == null)
                    chf = new ChannelFactory<IHallStateService>("HallStateEP");
            }

            try
            {
                prox = chf.CreateChannel();
                result.TheaterDetails = prox.FindTheater(string.Empty, ev.TheaterID);
            }
            catch (Exception ex)
            {
                LoggingManager.Logger.Log(LoggingCategory.Error, ex.Message);
                throw new FaultException<HallStateException>
                    (new HallStateException(string.Format(StringsResource.HallNotFound, ev.TheaterID), ex));
            }
            finally
            {
                var channel = prox as ICommunicationObject;
                if ((channel != null) && (channel.State == CommunicationState.Opened))
                    channel.Close();
            }
            #endregion

            return result;
        }

        #endregion

      
        /// <summary>
        /// Validate that the event is related to a vaild show. 
        /// </summary>
        /// <param name="eventToValidate"></param>
        private void ValidateEvent(Event eventToValidate)
        {
            if (eventToValidate.ShowDetails == null)
            {
            }

            int id = eventToValidate.ShowDetails.ShowID;

            if (FindShowByID(id) == null)
            {
            }
        }
    }
}
