using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialCommon.Models
{
    public class Post
    {
        public string PostID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        //public List<string> referebce { get; set; }
      

        public Post(string content)
        {
            PostID = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
            this.Content = content;
        }

        

    }
}
