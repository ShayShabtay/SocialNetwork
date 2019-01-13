using SocialCommon.Models;
using SocialRepository.GraphDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialBL.Managers
{
    public class SocialManager:ISocialManager
    {
        IGraphDB _graphDB;
        public SocialManager()
        {
            _graphDB = new neo4jDB();
        }

        public void AddPost(Post post,User user)
        {
            _graphDB.addPost(post,user);
          //  graphDB.creatConection(user,post,"publish");
        }

        public void AddUser(User user)
        {
            _graphDB.addUser(user);
        }

        public void Follow()
        {
            _graphDB.Follow();
        }
    }
}
