using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.ShowsService.Contracts;

namespace TicketingOffice.ShowsService.DataAccess
{
    /// <summary>
    /// Helper class to create an object to object mapping between the ORM (EF) entities 
    /// and the business entities defined in the contracts
    /// </summary>
    internal static class Mapper
    {
        /// <summary>
        /// map an ORM Event to a contract Event
        /// </summary>
        /// <param name="_event"></param>
        /// <returns></returns>
        internal static Contracts.Event MapEvent(DataAccess.Event _event)
        {
            return new Contracts.Event()
            {
                Date = _event.Date,
                EventID = _event.ID,
                ListPrice = _event.ListPrice,
                ShowDetails = MapShow(_event.Show),
                StartTime = _event.StartTime,
                State = (EventState)Enum.Parse(typeof(EventState), _event.State),
                TheaterID = _event.TheaterID
            };
        }


        /// <summary>
        /// Map an ORM Show to a contract Show
        /// </summary>
        /// <param name="_show"></param>
        /// <returns></returns>
        internal static Contracts.Show MapShow(DataAccess.Show _show)
        {
            var result = new Contracts.Show()
            {
                ShowID = _show.ID,
                Title = _show.Title,
                Category = _show.Category,
                Description = _show.Description,
                Cast = _show.Cast,
                Duration = new TimeSpan(0, (_show.Duration ?? 0), 0), 
                DetailsLink =  new Uri("http://empty.com"),
                Preview = new Uri("http://empty.com"),    
                Logo = _show.Logo
            };

            if (!string.IsNullOrEmpty(_show.Url))
                result.DetailsLink = new Uri(_show.Url);

            if (!string.IsNullOrEmpty(_show.Preview))
              result.Preview =  new Uri(_show.Preview);                   
          

            result.Events = _show.Events.Select(e => CreateEventForShow(e, result)).ToArray();

            return result;

        }


        /// <summary>
        /// Helper to implement the relation between Event and Show in the contract layer.
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="_show"></param>
        /// <returns></returns>
        private static Contracts.Event CreateEventForShow(DataAccess.Event _event, Contracts.Show _show)
        {
            var result = new Contracts.Event()
            {
                Date = _event.Date,
                EventID = _event.ID,
                ListPrice = _event.ListPrice,
                ShowDetails = _show,
                StartTime = _event.StartTime,
                State = (EventState)Enum.Parse(typeof(EventState), _event.State),
                TheaterID = _event.TheaterID
            };

            return result;
        }


        

    }
}
