using Microsoft.Owin;
using Owin;
using TicketingOffice.Mvc2._0.SignalR;

[assembly: OwinStartup(typeof(Startup))]

namespace TicketingOffice.Mvc2._0.SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
