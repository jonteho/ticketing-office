using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.HallState.Contracts
{
    public class ReservationException : Exception
    {
        public ReservationException() : base() { }

        public ReservationException(string message) : base(message) { }

        public ReservationException(string message, Exception inner) : base(message, inner) { }

        public ReservationException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        {
            ReservationInfo = (Reservation)info.GetValue("EntityType", typeof(Type));
        }

        public Reservation ReservationInfo { get; set; }           

        override public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ReservationInfo", ReservationInfo);            
        }
    }
}
