using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationService.signalR
{
    public class NotificationHub:Hub
    {

        NotificationsService NotificationsService { get; set; }
        public NotificationHub()
        {
            NotificationsService = NotificationsService.GetInstance;
        }

        public void SignIn(string userName)
        {
            NotificationsService.AddConnectedUser(Context.ConnectionId, userName);
            List<NotificationModel> notifications= NotificationsService.CheckNotification(userName);
            if (notifications!=null)
            {
                //send to user--invoke func in client
            }
            
        }
        public void GetNotificationsFromServer(string userName)
        {
            Clients.Client(NotificationsService.ConnectedUsers[userName]).GotNotificationsFromServer(NotificationsService.NotificationsList[userName]);
        }

        public void GotAllNotifications(string userName)
        {
            NotificationsService.clearNotifications(userName);
        }



    }
}