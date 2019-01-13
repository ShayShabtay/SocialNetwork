using Neo4j.Driver.V1;
using SocialCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public string getUser (User user){
            string query = $"Match (u:User{{Name:{user.Name}}}) return u.Name";
            var res=session.Run(query);
            string uName = res.ToString();
            return uName;
        }

        public void addUser(User user)
        {
            var jsonObj = DbHelper.ObjectToJson(user);
            string query = $"Create (u:User{jsonObj})";
            session.Run(query);
        }

       

        public void addPost(Post post ,User user)
        {
            Post p = new Post("test post2");
            User u = new User("omer","carmeli");
            var jsonPost = DbHelper.ObjectToJson(p);
            var jsonUser = DbHelper.ObjectToJson(u);
            string query = $"Create (u:User{jsonUser}-[:Publish]->p:Post{jsonPost})";
            string q1 = $"Create (p:Post{jsonPost})";
            string q2 = $"Match (u:User),(p:Post) Where ((User u)=> u.Name=={u.Name}) AndWhere (Post p)=> p.postID=={p.postID} Create (u-[:publish]->p Return u)";
            session.Run(query);
          //  Thread.Sleep(2000);
         //   session.Run(q2);
            ////////////////////////////////////////
            //string uName = getUser(user);
           // creatConection(user.Name,post.postID,"publish");
        }

        public void creatConection(string source, string target, string type)
        {

            string query = $"Create {source}-[:{type}]->{target}";
            session.Run(query);

        }

        public void Follow(string SourceUserId, string targetUserId)
        {
            string query = $"Merge (:User{{userID:\"{SourceUserId}\"}})-[:Follow]->(:User{{userID:\"{targetUserId}\"}})";
            session.Run(query);
        }
    }
}
