using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UI.Models;

namespace UI.SignalR
{
    public class SignalRClient
    {
        public string Url { get; set; }
        public HubConnection Connection { get; set; }
        public IHubProxy Hub { get; set; }
        public SocialViewModel Model { get; set; }


        public SignalRClient(SocialViewModel model)//Chat Ctor
        {
           this.Model = model;
            Connection = new HubConnection("http://localhost:51446/signalr");
            Hub = Connection.CreateHubProxy("NotificationHub");

            Hub.On("GetNotification", (string userId) =>
            {
                Hub.Invoke("GetNotificationsFromServer",userId);
            });
            Hub.On("GotNotificationsFromServer", (List<NotificationModel> notifications) =>
            {
                Model.Notifications.Clear();
                foreach (var item in notifications)
                {
                    
                    Model.Notifications.Add(item);

                    //contorller . addNotification{add to notificatin list and notify the view to render}
                }

            });


            Connection.Start().Wait();

        }
        public void Login(string username)
        {
            //notifies the server on log in
            Hub.Invoke("SignIn", username);
            //Hub.Invoke("CheckForNotificationsOnLogin", username);
        }

        public void SendOk(string username)
        {
            Hub.Invoke("GotAllNotifications",username);
        }

    }
}