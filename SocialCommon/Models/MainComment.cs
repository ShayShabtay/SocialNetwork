using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialCommon.Models
{
    public class MainComment
    {
        public Comment Comment { get; set; }
        public User CommentOwner { get; set; }
        public bool IsLike { get; set; }
    }
}
