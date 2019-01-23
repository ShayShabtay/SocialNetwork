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
            this.createdDate = DateTime.UtcNow;
        }


    }
}