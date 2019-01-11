﻿using SocialCommon.Models;
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
        IGraphDB graphDB;
        public SocialManager()
        {
            graphDB = new neo4jDB();
        }

        public void addPost(Post post,User user)
        {
            graphDB.addPost(post,user);
          //  graphDB.creatConection(user,post,"publish");
        }

        public void addUser(User user)
        {
            graphDB.addUser(user);
        }


    }
}
