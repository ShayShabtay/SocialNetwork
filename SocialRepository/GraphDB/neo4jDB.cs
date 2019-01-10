using Neo4j.Driver.V1;
using SocialCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialRepository.GraphDB
{
   public class neo4jDB: IGraphDB
    {
        ISession session;

        public neo4jDB()
        {
              session=DbHelper.getSession();  
        }

        public void getUser (){

        }

        public void addUser(User user)
        {
            User u = new User("12345","nitzan");
            var jsonObj = DbHelper.ObjectToJson(u);
            string query = $"Create (u:User{jsonObj})";
            session.Run(query);
        }

    }
}
