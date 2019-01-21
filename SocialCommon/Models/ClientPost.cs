using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialCommon.Models
{
    public class ClientPost
    {
        public string postID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public string imageUrl { get; set; }
        public List<Comment> Comments { get; set; }
        public List<User> UsersLikes { get; set; }
        public int LikeCount { get; set; }


        public ClientPost(Post post)
        {
            this.postID = post.PostID;
            this.CreationDate = post.CreationDate;
            this.Content = post.Content;
            this.imageUrl = post.ImageUrl;
        }

        public ClientPost()
        {
        }


    }
}
