using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TicketingOffice.Common.Exceptions
{
    public class DalException : Exception
    {
        public DalException() : base() { }

        public DalException(string message) : base(message) { }

        public DalException(string message, Exception inner) : base(message, inner) { }

        public DalException(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
            EntityType = (Type)info.GetValue("EntityType", typeof(Type));
        }

        public Type EntityType { get; set; }

        override public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
             info.AddValue("EntityType", EntityType);            
        }

    }
}
