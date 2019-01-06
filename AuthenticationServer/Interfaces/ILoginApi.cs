using AuthenticationCommon.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuthenticationServer.Interfaces
{
    public interface ILoginApi
    {
        IHttpActionResult Register(UserLoginDTO UserLoginDTO);
        IHttpActionResult LoginManual();
        IHttpActionResult LoginFacebook();
        IHttpActionResult Logout();
        IHttpActionResult ResetPassword();
    }
}
