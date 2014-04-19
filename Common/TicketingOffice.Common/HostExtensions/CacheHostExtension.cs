using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Web;

namespace TicketingOffice.Common.HostExtentions
{
    /// <summary>
    /// Host Extension for managing a cache
    /// </summary>
    public class CacheHostExtension : IExtension<ServiceHostBase>
    {
        public System.Web.Caching.Cache HostCache { get; set; }

        public object GetItem(string key)
        {
            return HostCache.Get(key);
        }

        public object SetItem(string key, object value)
        {
            return HostCache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }

        public object SetItem(string key, object value, DateTime absoluteExpiration)
        {
            return HostCache.Add(key, value, null, absoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }

        public object SetItem(string key, object value, TimeSpan slidingExpiration)
        {
            return HostCache.Add(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
        }

       #region IExtension<ServiceHostBase> Members

        public void Attach(ServiceHostBase owner)
        {
             
        }

        public void Detach(ServiceHostBase owner)
        {
              
        }


        void owner_Faulted(object sender, EventArgs e)
        {
            HostCache = null;
        }

        void owner_Closed(object sender, EventArgs e)
        {
            HostCache = null;
        }

        #endregion
    }
}
