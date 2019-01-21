﻿using Microsoft.AspNet.SignalR.Client;
using NotificationService.signalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.SignalR
{
    public class SignalRClient
    {
        public string Url { get; set; }
        public HubConnection Connection { get; set; }
        public IHubProxy Hub { get; set; }
        //public MainModel model { get; set; }
        public SignalRClient()//Chat Ctor
        {
            //this.model = model;
            Connection = new HubConnection("http://localhost:51446/signalr");
            Hub = Connection.CreateHubProxy("NotificationHub");

            Hub.On("GetNotification", (string userId) =>
            {
                Hub.Invoke("GetNotificationsFromServer",userId);
            });
            Hub.On("GotNotificationsFromServer", (List<NotificationModel> notifications) =>
            {
                foreach (var item in notifications)
                {
                   // model.Notifications.Clear();
                   // model.Notifications.Add(item);
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