using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.TicketingService.Contracts;
using System.ServiceModel.Discovery;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace TicketingOffice.TicketingService
{
    /// <summary>
    /// Helper class that returnes a proxy to the ticketing bridge to return the callback
    /// </summary>
    internal class CallBackChannelFactory
    {
        private static ChannelFactory<ITicketingCallBack> _chf;

        internal static ITicketingCallBack GetProxy(bool discovery)
        {
            if (_chf == null)
            {
                lock (typeof(CallBackChannelFactory))
                {
                    if (_chf == null)
                        _chf = new ChannelFactory<Contracts.ITicketingCallBack>("BridgeCallBackEP");
                }
            }

            if (discovery)
            {
                // Create a DynamicEndpoint which will discover endpoints when the client is opened. 
                // By default, the contract specified in DynamicEndpoint will be used as the FindCriteria
                // and UdpDiscoveryEndpoint will be used to send Probe message
                DynamicEndpoint dynamicEndpoint = new DynamicEndpoint(
                                                            ContractDescription.GetContract(typeof(ITicketingCallBack)),
                                                            new NetMsmqBinding());
                return _chf.CreateChannel(dynamicEndpoint.Address);
            }
            else
            {
                return _chf.CreateChannel();
            }
        }

    }


   
    /// <summary>
    /// Create a general proxy to the ticketing service
    /// </summary>
    internal class TicketingRequestChannelFactory
    {
        private static ChannelFactory<ITicketingService> _chf;


        public static IRequestChannel GetProxy()
        {

            if (_chf == null)
                _chf = new ChannelFactory<ITicketingService>("TicketingEP");

            var binding = new WS2007HttpBinding(); 
            binding.Security.Mode = SecurityMode.None;           
            binding.OpenTimeout = TimeSpan.FromSeconds(600);
            EndpointAddress address = _chf.Endpoint.Address;
            ChannelFactory<IRequestChannel> factory = new ChannelFactory<IRequestChannel>(binding, address);
            return factory.CreateChannel();
        }

    }
}
