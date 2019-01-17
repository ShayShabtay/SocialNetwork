using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Models
{
    public class SocialViewModel
    {
        public Comment Comment { get; set; }
        public Post Post { get; set; }
        public User User { get; set; }
        public UserIdentityModel UserIdentityModel { get; set; }
        
    }

    public class Comment
    {
        public string CommentID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public string imageUrl { get; set; }


        public Comment(string content)
        {
            CommentID = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
            this.Content = content;
        }
    }

    public class Post
    {
        public string postID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public string imageUrl { get; set; }
        //public List<string> referebce { get; set; }


        public Post(string content)
        {
            postID = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
            this.Content = content;
        }
    }

    public static class RelationsMap
    {
        public static Dictionary<string, Tuple<string, string, string, string>> map { get; set; }

    }

    public class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }

    }
}