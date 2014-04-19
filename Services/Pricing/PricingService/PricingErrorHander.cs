﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using TicketingOffice.Pricing.Contracts;

namespace TicketingOffice.PricingService
{
    /// <summary>
    /// Custom error handler that writes a specific fault message to the channel 
    /// so the client can catch the specific exception.
    /// </summary>
    public class PricingErrorHander
    {

        public PricingErrorHander() { }

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
            FaultException<PricingException> faultException =
                new FaultException<PricingException>(new PricingException(error.Message,error.InnerException));
            MessageFault messageFault = faultException.CreateMessageFault();

            fault = Message.CreateMessage(version, messageFault, faultException.Action);
        }

        #endregion

    }

}
