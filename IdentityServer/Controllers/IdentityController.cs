using IdentityBL.Managers;
using IdentityServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IdentityServer.Controllers
{
    public class IdentityController : ApiController, IIdentityApi
    {
        IdentityManager _identityManager;

        public IdentityController()
        {
            _identityManager = new IdentityManager();
        }

        public IHttpActionResult GetUserProfile()
        {
            throw new NotImplementedException();
        }

        public IHttpActionResult UpdateUserProfile()
        {
            throw new NotImplementedException();
        }
    }
}
