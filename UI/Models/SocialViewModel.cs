﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace UI.Models
{
    public class SocialViewModel
    {
        public List<NotificationModel> Notifications { get; set; }
        public Comment Comment { get; set; }
        public CommentDTO CommentDTO { get; set; }
        public Post Post { get; set; }
        public PostDTO PostDTO { get; set; }
        public User User { get; set; }
        public UserIdentityModel UserIdentityModel { get; set; }
        public UserIdentityModel OtherUserIdentityModel { get; set; }
        public IEnumerable<UserDTO> UserDTO { get; set; }
        public IEnumerable<ClientPost> ClientPostFeed { get; set; }

        public SocialViewModel()
        {
            UserDTO = new List<UserDTO>();
            UserIdentityModel = new UserIdentityModel();
            OtherUserIdentityModel = new UserIdentityModel();
            User = new User();
            Post = new Post("");
            Comment = new Comment("");
            CommentDTO = new CommentDTO();
            PostDTO = new PostDTO();
            ClientPostFeed = new List<ClientPost>();

            Notifications = new List<NotificationModel>();
        }

        public void AddAndNotify(NotificationModel notificationModel)
        {
            Notifications.Add(notificationModel);
        }
    }


    public class Comment
    {
        public string CommentID { get; set; }
        public string PostID { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public HttpPostedFileBase Picture1 { get; set; }

        public Comment(string content)
        {
            CommentID = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
            Content = content;
        }
    }

    public class CommentDTO
    {
        public Comment Comment { get; set; }
        public List<string> Tags { get; set; }

        public CommentDTO()
        {
            Tags = new List<string>();
            Comment = new Comment("");
        }
    }


    public class Post
    {
        public string Content { get; set; }
        public string PostID { get; set; }
        public DateTime CreationDate { get; set; }
        public string ImageUrl { get; set; }
        public string Permission { get; set; }
        public HttpPostedFileBase Picture1 { get; set; }


        public Post(string content)
        {
            PostID = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
            this.Content = content;
        }
    }

    public class PostDTO
    {
        public Post Post { get; set; }
        public List<string> Tags { get; set; }

        public PostDTO()
        {
            Tags = new List<string>();
            Post = new Post("");
        }
    }

    public class User
    {
        public string UserId { get; set; }
        public string Name { get; set; }

    }

    public class ClientPost
    {
        public string postID { get; set; }
        public User PostOwner { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public string imageUrl { get; set; }
        public List<MainComment> Comments { get; set; }
        public List<User> UsersLikes { get; set; }
        public int LikeCount { get; set; }
        public bool IsLike { get; set; }

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

    public class NotificationModel
    {

        public string SourceClient { get; set; }
        public string SourceClientFullName { get; set; }
        public string TargetClient { get; set; }
        public string PostId { get; set; }
        public string type { get; set; }
        public DateTime createdDate;

        public NotificationModel()
        {

        }

        public NotificationModel(string sourceName, string targetName, string postId, string type)
        {
            this.SourceClient = sourceName;
            this.TargetClient = targetName;
            this.PostId = postId;
            this.type = type;
            this.createdDate = DateTime.UtcNow;
        }

        public override string ToString()
        {
            if (type == "Follow")
            {

                return SourceClient + " " + type + " you";
            }
            return SourceClient + " " + type + " your" + PostId;
        }
    }

    public class MainComment
    {
        public Comment Comment { get; set; }
        public User CommentOwner { get; set; }
        public bool IsLike { get; set; }
    }
}