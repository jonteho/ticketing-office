using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using TicketingOffice.Common.Properties;

namespace TicketingOffice.Common.Behaviors
{
      
    public sealed class ErrorBehaviorAttribute  : Attribute, IServiceBehavior
    {
        Type errorHandler;
        public Type ErrorHandlerType
        {
            get { return this.errorHandler; }
        }

        public ErrorBehaviorAttribute(Type errorHandler)
        {
            this.errorHandler = errorHandler;
        }

        void IServiceBehavior.Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        { }
        void IServiceBehavior.AddBindingParameters(ServiceDescription description, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
        { }
        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler;

            try
            {
                errorHandler = (IErrorHandler)Activator.CreateInstance(ErrorHandlerType);
            }
            catch (MissingMethodException ex)
            {
                throw new ArgumentException(StringsResource.ErrorHandlerMustIncludeEmptyConstructor, ex);
            }
            catch (InvalidCastException ex)
            {
                throw new ArgumentException(StringsResource.ErrorHandlerMustImplementIErrorHandler, ex);
            }

            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                channelDispatcher.ErrorHandlers.Add(errorHandler);
            }
        }
    }
}



