using Neo4j.Driver.V1;
using Newtonsoft.Json;
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

       

        public void addPost(Post post)
        {
            //Post p = new Post("test post2");
            //User u = new User("omer","carmeli");
            var jsonPost = DbHelper.ObjectToJson(post);
            //var jsonUser = DbHelper.ObjectToJson(u);
            //string query = $"Create (u:User{jsonUser}-[:Publish]->p:Post{jsonPost})";
            string query = $"Create (p:Post{jsonPost})";
            //string q2 = $"Match (u:User),(p:Post) Where ((User u)=> u.Name=={u.Name}) AndWhere (Post p)=> p.postID=={p.postID} Create (u-[:publish]->p Return u)";
            session.Run(query);
          //  Thread.Sleep(2000);
         //   session.Run(q2);
            ////////////////////////////////////////
            //string uName = getUser(user);
           // creatConection(user.Name,post.postID,"publish");
        }

        public void creatConection(string source, string target, string relation)
        {
                        if (RelationsMap.map.ContainsKey(relation))
            {
                Tuple<string,string,string,string> values=RelationsMap.map[relation];
                string q = $"Match (x:{values.Item1}{{{values.Item2}:\"{source}\"}})" +
                           $"Match (y:{values.Item3}{{{values.Item4}:\"{target}\"}})" +
                           $"Merge ((x)-[:{relation}]->(y))";

                string query = $"Merge (x:{values.Item1}{{{values.Item2}:\"{source}\"}})-[:{relation}]->(y:{values.Item3}{{{values.Item4}:\"{target}\"}})";
                session.Run(q);

            }
            else
            {
                throw new KeyNotFoundException();
            }


        }

        public void Follow(string SourceUserId, string targetUserId)
        {
            string query = $"Merge (x:User{{userID:\"{SourceUserId}\"}})-[:Follow]->(y:User{{userID:\"{targetUserId}\"}})";
            session.Run(query);
        }

        public List<Post> getAllPosts(string userId)
        {

            //whos im follow=> what post they published =>limit 20
            //change the first line in query for better tuntimr

            string query = $"Match (u:User)-[:Follow]->(u2:User) " +
                           $"Where u.UserID=\"{userId}\" " +
                           $"Match (u2)-[:Publish]->(p:Post)" +
                           $"Return p";

            IStatementResult posts=session.Run(query);
            List<Post> postList = new List<Post>();
            foreach (var item in posts)
            {
                var props = JsonConvert.SerializeObject(item[0].As<INode>().Properties);
                postList.Add(JsonConvert.DeserializeObject<Post>(props));    
            }
            return postList;
        }

        public  async Task<bool> addComment(Comment comment)
        {
            var jsonObj = DbHelper.ObjectToJson(comment);
            string query = $"Create (c:Comment{jsonObj})";
            var res= session.Run(query);
            if (res != null) {
                return true;
            }
            return false;
        }

        public List<Post> getMyPosts(string userId)
        {
            List<Post> postList = new List<Post>();

            string query = $"Match (u:User)" +
                           $"Where u.UserID=\"{userId}\"" +
                           $"Match (u)-[:publish]->(p:Post)" +
                           $"return p";

            IStatementResult posts = session.Run(query);
            foreach (var item in posts)
            {
                var props = JsonConvert.SerializeObject(item[0].As<INode>().Properties);
                postList.Add(JsonConvert.DeserializeObject<Post>(props));
            }
            return postList;
        }

    }
}
