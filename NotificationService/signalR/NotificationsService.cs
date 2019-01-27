using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationService.signalR
{
    public class NotificationsService
    {

        private static NotificationsService Instance;

        //51446 port
         public Dictionary<string, List<NotificationModel>> NotificationsList { get; set; }
         public Dictionary<string, string> ConnectedUsers { get; set; }


      
        private NotificationsService()
        {
            NotificationsList = new Dictionary<string, List<NotificationModel>>();
            ConnectedUsers = new Dictionary<string, string>();
            seed();
        }

        private void seed()
        {
            List<NotificationModel> seedList = new List<NotificationModel>();
            seedList.Add(new NotificationModel("someOne","testUser","","Follow"));
            seedList.Add(new NotificationModel("someOne2", "testUser", "1234", "Like"));

            NotificationsList.Add("testUser", seedList);
        }

        public static NotificationsService GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new NotificationsService();
                }
                return Instance;
            }
        }


        public void AddConnectedUser(string ConnectionId, string userName)
        {
            ConnectedUsers.Add(ConnectionId, userName);
        }

        public void AddNotification(NotificationModel notificationModel)
        {
            if (NotificationsList.ContainsKey(notificationModel.TargetClient))
            {
                NotificationsList[notificationModel.TargetClient].Add(notificationModel); ;
            }
            else
            {
                List<NotificationModel> list = new List<NotificationModel>();
                list.Add(notificationModel);
                NotificationsList.Add(notificationModel.TargetClient, list);
            }
        }

        public  void clearNotifications(string userName)
        {
            NotificationsList.Remove(userName);
        }

        public bool CheckNotification(string connectionId)
        {
            if (NotificationsList.ContainsKey(connectionId))
            {
                return true;
            }

            return false;
        }





    }
}