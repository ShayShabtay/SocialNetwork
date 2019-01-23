using IdentityCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace IdentityServer.Interfaces
{
    public interface IIdentityApi
    {
        IHttpActionResult GetUserProfile(string OtherUserId);
        IHttpActionResult UpdateUserProfile(UserIdentity userIdentity);
    }
}
