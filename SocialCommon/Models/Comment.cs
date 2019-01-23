
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialCommon.Models
{
    public class Comment
    {
        public string CommentID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        //public List<User> UsersLike { get; set; }


        public Comment(string content)
        {
            CommentID = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
            this.Content = content;
        }
    }
}
