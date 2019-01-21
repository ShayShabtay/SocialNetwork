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
           bool hasNotification= NotificationsService.CheckNotification(userName);
            if (hasNotification)
            {
                GetNotificationsFromServer(userName);
                //return notifications;
            }
            //return null;
        }
        public void GetNotificationsFromServer(string userName)
        {
           // Clients.Client(NotificationsService.ConnectedUsers[userName]).GotNotificationsFromServer(NotificationsService.NotificationsList[userName]);
            var userNameKey = NotificationsService.ConnectedUsers.FirstOrDefault(x => x.Value == userName).Key;

            Clients.Client(userNameKey).GotNotificationsFromServer(NotificationsService.NotificationsList[userName]);
        }

        public void GotAllNotifications(string userName)
        {
            NotificationsService.clearNotifications(userName);
        }



    }
}