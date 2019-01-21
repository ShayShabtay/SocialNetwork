namespace NotificationService.signalR
{
    public  class NotificationModel
    {
        public NotificationModel()
        {

        }

        public NotificationModel(string sourceName,string targetName,string postId)
        {
            this.SourceClient = sourceName;
            this.TargetClient = targetName;
            this.PostId = postId;
        }

        public string SourceClient { get; set; }
        public string SourceClientFullName { get; set; }
        public string TargetClient { get; set; }
        public string PostId { get; set; }
        public string type { get; set; }


    }
}