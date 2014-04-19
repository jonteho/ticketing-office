using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketingOffice.ClientNotification.Contract;

namespace TicketingOffice.ClientNotification
{
    /// <summary>
    /// Holds registration information for duplex and one-way registration.
    /// </summary>
    public class Registration
    {
        public Uri RegistrationUri { get; set; }
        public string EventType { get; set; }
        public string UserName { get; set; }

        //For duplex channel
        public IClientNotification clientProxy { get; set; }

        /// <summary>
        /// Create a key for this registration to be used by the repositories that saves the registrations. 
        /// The key is a combination of the user and eventType.
        /// </summary>
        /// <returns></returns>
        public string CreateKey()
        {
            return string.Format("{0}_{1}",EventType,UserName);
        }
            
    }


    /// <summary>
    /// Repository of client notification registrations.
    /// This repository is NOT persisted to disk.
    /// </summary>
    public class MemoryRepository 
    {
     
        /// <summary>
        /// A collection of (Uri , eventType) pairs
        /// </summary>
        private Dictionary<string, List<Registration>> Registrations = new Dictionary<string, List<Registration>>();

        //singleton pattern
        private static MemoryRepository _current;
        public static MemoryRepository Current 
        {  
            get
            {
                if (_current == null)
                {
                    lock (typeof(MemoryRepository))
                    {
                        if (_current == null)
                            _current = new MemoryRepository();
                    }
                }
                return _current;
            }

        }

        /// <summary>
        /// Create a client registration for a certain event type.
        /// </summary>
        /// <param name="clientUri"></param>
        /// <param name="eventType"></param>
        /// <param name="userName"></param>
        /// <param name="prox"></param>
        public void RegisterClient(Uri clientUri, NotificationTypes eventType, string userName, IClientNotification prox)
        {           
            var reg = new Registration() { RegistrationUri = clientUri, EventType = eventType.ToString(), UserName = userName, clientProxy=prox  };
            var key = reg.CreateKey();
            if (!Registrations.ContainsKey(key))
                Registrations.Add(key, new List<Registration> { reg });
            else if (Registrations[key].Find(rg => (rg.RegistrationUri.AbsolutePath == reg.RegistrationUri.AbsolutePath)) == null)
                   Registrations[key].Add(reg);            
        }

        /// <summary>
        /// Create a client registration for a certain event type.
        /// </summary>
        /// <param name="clientUri"></param>
        /// <param name="eventType"></param>
        public void UnRegisterClient(Uri clientUri, NotificationTypes eventType)
        {
            var reg = new Registration() { RegistrationUri = clientUri, EventType = eventType.ToString() };
             var key = reg.CreateKey();
             if (Registrations.ContainsKey(key))
                 Registrations.Remove(key);
        }

        /// <summary>
        /// Get all the client registrations
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public Registration[] GetAllRegistrations(NotificationTypes eventType)
        {
            var q = from reglst in Registrations.Values
                     from reg in reglst
                     where reg.EventType == eventType.ToString()
                     select reg;

            return q.ToArray();
        }

        /// <summary>
        /// Get a particular registration
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Registration[] GetRegistrations(NotificationTypes eventType, string userName)
        {
            if (string.IsNullOrEmpty(userName))
                userName = "Anonymous";

            var q = from reglst in Registrations.Values
                    from reg in reglst
                    where ((reg.EventType == eventType.ToString()) && (reg.UserName == userName))
                    select reg;

            return q.ToArray();

        }

        /// <summary>
        /// Clear all registrations for an event type
        /// </summary>
        /// <param name="eventType"></param>
        public void ClearRegistrations(NotificationTypes? eventType)
        {
            if (eventType == null)
                Registrations = new Dictionary<string, List<Registration>>();
            else if (Registrations.ContainsKey(eventType.ToString()))
            {
                Registrations[eventType.ToString()] = new List<Registration>();
            }
        }

      
    }
}
