using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TicketingOffice.Common.Helpers;

namespace TicketingOffice.Common.HostExtentions
{
    /// <summary>
    /// Host Extension for holding server state
    /// </summary>
    public class ServerStateHostExtension : IExtension<ServiceHostBase>
    {

        public ThreadSafeDictionary<string, object> StateData { get; set; }


      #region IExtension<ServiceHostBase> Members

        public void Attach(ServiceHostBase owner)
        {
            StateData = new ThreadSafeDictionary<string, object>();
            owner.Closed += new EventHandler(owner_Closed);
            owner.Faulted += new EventHandler(owner_Faulted);
        }       

        public void Detach(ServiceHostBase owner)
        {
            StateData = null;
            owner.Closed -= owner_Closed;
            owner.Faulted -= owner_Faulted;
        }


        void owner_Faulted(object sender, EventArgs e)
        {
            StateData = null;
        }

        void owner_Closed(object sender, EventArgs e)
        {
            StateData = null;
        }

        #endregion
    }
}
