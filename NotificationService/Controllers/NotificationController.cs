using Microsoft.AspNet.SignalR;
using NotificationService.signalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NotificationService.Controllers
{
    public class NotificationController : ApiController
    {
        IHubContext context { get; set; }
        public NotificationController()
        {
            context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        }
        [HttpPost]
        [Route("api/Notification/AddNotification")]
        public IHttpActionResult AddNotification([FromBody]List<string> param)
        {
            NotificationModel notificationModel = new NotificationModel(param[0], param[1], param[2],param[3]);
            NotificationsService notificationsService = NotificationsService.GetInstance;
            notificationsService.AddNotification(notificationModel);
            var userNameKey = notificationsService.ConnectedUsers.FirstOrDefault(x => x.Value == param[1]).Key;
            if (userNameKey != null)
            {
                context.Clients.Client(notificationsService.ConnectedUsers[userNameKey]).GetNotification(param[1]);

            }

            return Ok() ;
        }
    }
}
