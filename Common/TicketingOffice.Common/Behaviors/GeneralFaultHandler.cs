using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace TicketingOffice.Common.Behaviors
{
    public class GeneralFaultHandler<T> : IErrorHandler where T : Exception, new()
    {
        #region IErrorHandler Members

        /// <summary>
        ///  Allows error processing to take place in the event of an error and controls whether additional error handling can run.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HandleError(Exception error)
        {
            
            return true;
        }

        /// <summary>
        ///  Allows to add, modify, or suppress a fault message that is generated in response to an exception.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="version"></param>
        /// <param name="fault"></param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            T errorToSend = error as T;
            if (error is FaultException<T>)
                errorToSend = ((FaultException<T>)error).InnerException as T;

            if (errorToSend ==  null)
                errorToSend = new T();

            FaultException<T> faultException =
                new FaultException<T>(errorToSend);
            MessageFault messageFault = faultException.CreateMessageFault();

            fault = Message.CreateMessage(version, messageFault, faultException.Action); 
        }

        #endregion
    }
}
