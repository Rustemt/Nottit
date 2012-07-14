using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nottit.Controllers {
    public class LoginController : BaseController {
        [AllowAnonymous]
        public object PostLogin(string username, string password) {
            string errorMessage;
            if (LoginManager.Login(username, password, out errorMessage)) {
                return new { result = true };
            } else {
                return new { result = false, errorMessage = errorMessage };
            }
        }

        [AllowAnonymous]
        public void DeleteLogin() {
            LoginManager.Logout();
        }
    }
}
