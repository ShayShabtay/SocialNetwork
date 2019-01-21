using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly:OwinStartup(typeof (NotificationService.Startup))]
namespace NotificationService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR("/signalr",new Microsoft.AspNet.SignalR.HubConfiguration());
        }
    }
}