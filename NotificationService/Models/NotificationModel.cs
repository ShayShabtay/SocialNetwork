using System;

namespace NotificationService.signalR
{
    public  class NotificationModel
    {

        public string SourceClient { get; set; }
        public string SourceClientFullName { get; set; }
        public string TargetClient { get; set; }
        public string PostId { get; set; }
        public string type { get; set; }
        public DateTime createdDate;

        public NotificationModel()
        {

        }

        public NotificationModel(string sourceName,string targetName,string postId,string type)
        {
            this.SourceClient = sourceName;
            this.TargetClient = targetName;
            this.PostId = postId;
            this.type = type;
            this.createdDate = DateTime.UtcNow;
        }

        public override string ToString()
        {
            if (type == "Follow")
            {

            return SourceClient + " " + type + " you";
            }
            return SourceClient + " " + type + " your"+ PostId;
        }
    }
}