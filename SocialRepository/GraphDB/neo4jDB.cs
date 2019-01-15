﻿using Neo4j.Driver.V1;
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
    public class Neo4jDB : IGraphDB
    {
        ISession session;

        //Ctor
        public Neo4jDB()
        {
            session = DbHelper.getSession();
        }

        public void CreateRelationship(string source, string target, string relation)
        {
            if (RelationsMap.map.ContainsKey(relation))
            {
                Tuple<string, string, string, string> values = RelationsMap.map[relation];
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

        public void DeleteRelationship(string source, string target, string relation)
        {
            if (RelationsMap.map.ContainsKey(relation))
            {
                Tuple<string, string, string, string> values = RelationsMap.map[relation];
                string q = $"MATCH (:{values.Item1}{{{values.Item2}:\"{source}\"}})-[r:{relation}]->(:{values.Item3}{{{values.Item4}:\"{target}\"}})" +
                           $"DELETE r";
                session.Run(q);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }


        //User Methods
        public void AddUser(User user)
        {
            var jsonObj = DbHelper.ObjectToJson(user);
            string query = $"Create (u:User{jsonObj})";
            session.Run(query);
        }

        public List<User> GetAllUsers(string userId)
        {
            List<User> allUsers = new List<User>();
            string query = $"Match(u: User)" +
                           $"Where Not EXISTS((u) -[:Block] - (:User{{UserId:\"{userId}\"}}))" +
                           $"AND u.UserId <> \"{userId}\"" +
                           $"return u";
            IStatementResult res = session.Run(query);

            foreach (var item in res)
            {
                var props = JsonConvert.SerializeObject(item[0].As<INode>().Properties);
                allUsers.Add(JsonConvert.DeserializeObject<User>(props));
            }

            return allUsers;
        }

        public List<User> GetFollowers(string userId)
        {
            List<User> followers = new List<User>();
            string query = $"Match(u: User)" +
                           $"Where((u) -[:Follow] -> (:User{{UserId:\"{userId}\"}}))" +
                           $"return u";
            IStatementResult res = session.Run(query);

            foreach (var item in res)
            {
                var props = JsonConvert.SerializeObject(item[0].As<INode>().Properties);
                followers.Add(JsonConvert.DeserializeObject<User>(props));
            }

            return followers;
        }

        public List<User> GetFollowing(string userId)
        {
            List<User> following = new List<User>();
            string query = $"Match(u: User)" +
                           $"Where( (:User{{UserId:\"{userId}\"}}) -[:Follow] ->(u))" +
                           $"return u";
            IStatementResult res = session.Run(query);

            foreach (var item in res)
            {
                var props = JsonConvert.SerializeObject(item[0].As<INode>().Properties);
                following.Add(JsonConvert.DeserializeObject<User>(props));
            }

            return following;
        }

        public List<User> GetBlockedUsers(string userId)
        {
            List<User> blockedUsers = new List<User>();
            string query = $"Match(u: User)" +
                           $"Where( (:User{{UserId:\"{userId}\"}}) -[:Block] ->(u))" +
                           $"return u";
            IStatementResult res = session.Run(query);

            foreach (var item in res)
            {
                var props = JsonConvert.SerializeObject(item[0].As<INode>().Properties);
                blockedUsers.Add(JsonConvert.DeserializeObject<User>(props));
            }

            return blockedUsers;
        }


        //Post Methods
        public void AddPost(Post post)
        {
            var jsonPost = DbHelper.ObjectToJson(post);
            string query = $"Create (p:Post{jsonPost})";
            session.Run(query);
        }
       
        public  void AddComment(Comment comment)
        {
            var jsonObj = DbHelper.ObjectToJson(comment);
            string query = $"Create (c:Comment{jsonObj})";
            var res= session.Run(query);
        }

        public List<Post> GetAllPosts(string userId)
        {

            //whos im follow=> what post they published =>limit 20
            //change the first line in query for better tuntimr

            string query = $"Match (u:User)-[:Follow]->(u2:User) " +
                           $"Where u.UserID=\"{userId}\" " +
                           $"Match (u2)-[:Publish]->(p:Post)" +
                           $"Return p";

            IStatementResult posts = session.Run(query);
            List<Post> postList = new List<Post>();
            foreach (var item in posts)
            {
                var props = JsonConvert.SerializeObject(item[0].As<INode>().Properties);
                postList.Add(JsonConvert.DeserializeObject<Post>(props));
            }
            return postList;
        }

        public List<Post> GetMyPosts(string userId)
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

        public List<Comment> GetCommentsForPost(string postID)
        {
            List<Comment> commentsList = new List<Comment>();
            string query = $"Match (p:Post)" +
                           $"Wehre p.PostID=\"{postID}\"" +
                           $"Match (p)-[:PostComment]->(c:Comment)" +
                           $"return c";
            IStatementResult res = session.Run(query);
            foreach (var item in res)
            {
                var props = JsonConvert.SerializeObject(item[0].As<INode>().Properties);
                commentsList.Add(JsonConvert.DeserializeObject<Comment>(props));
            }

            return commentsList;
        }

        public List<User> GetLikesForPost(string postID)
        {
            List<User> likesList = new List<User>();
            string query = $"Match (p:Post)" +
                           $"Wehre p.PostID=\"{postID}\"" +
                           $"Match (p)-[:Like]->(u:User)" +
                           $"return c";
            IStatementResult res = session.Run(query);
            foreach (var item in res)
            {
                var props = JsonConvert.SerializeObject(item[0].As<INode>().Properties);
                likesList.Add(JsonConvert.DeserializeObject<User>(props));
            }

            return likesList;
        }
    }
}
