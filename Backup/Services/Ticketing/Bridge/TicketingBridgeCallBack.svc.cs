using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TicketingOffice.TicketingService.Contracts;
using TicketingOffice.HallState.Contracts;
using TicketingOffice.PaymentService.Contracts;
using TicketingOffice.Common.Helpers;
using System.Threading;
using TicketingOffice.Common.Properties;
using TicketingOffice.CurrencyExchange.Contract;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;

namespace TicketingOffice.Bridge
{
    // TODO: Ex5 - Change the concurrency and instancing mode of the TicketingBridgeCallBack service
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class TicketingBridgeCallBack : ITicketingCallBack
    {
        //The callback interface exposed to the service to return the result.
        #region ITicketingCallBack Members

        public void PaymentArrived(Payment result, Guid callID)
        {
            HandleResult(result, callID);
        }

        public void OrderArrived(Order result, Guid callID)
        {
            HandleResult(result, callID);
        }

        public void OrdersArrived(Order[] result, Guid callID)
        {
            HandleResult(result, callID);
        }

        public void IDArrived(Guid result, Guid callID)
        {
            HandleResult(result, callID);
        }

        public void IDsArrived(Guid[] result, Guid callID)
        {
            HandleResult(result, callID);
        }

        public void MessageArrived(string result, Guid callID)
        {
            HandleResult(result, callID);
        }

        #endregion


        //When the result arrives the bridge sets the waitHandle to resume processing and return the result to the client 
        private void HandleResult(object result, Guid callID)
        {
            // TODO: Ex2 - Add code that sets the auto-reset event
            var resultPack = ResultsCache.Current.GetResultPack(callID);
            if (resultPack != null)
            {
                resultPack.Result = result;
                resultPack.ResultArrived.Set();
            }

        }
    }
}
