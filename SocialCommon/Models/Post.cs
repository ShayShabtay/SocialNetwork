using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialCommon.Models
{
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
}
