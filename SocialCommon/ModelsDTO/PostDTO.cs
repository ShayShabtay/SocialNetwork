﻿using SocialCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialCommon.ModelsDTO
{
    public class PostDTO
    {
        public Post Post { get; set; }
        public List<string> Tags { get; set; }
    }
}
