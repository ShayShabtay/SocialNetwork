﻿using SocialCommon.Models;

namespace SocialRepository.GraphDB
{
    public interface IGraphDB
    {

        void addUser(User user);
        void addPost(Post post);
        void creatConection(string source, string target, string type);
        void Follow(string SourceUserId, string targetUserId);
    }
}