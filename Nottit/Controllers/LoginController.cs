﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nottit.Models;

namespace Nottit.Controllers {
    public class LoginController : BaseController {
        [AllowAnonymous]
        public object PostLogin(LoginModel loginModel) {
            string errorMessage;
            if (LoginManager.Login(loginModel.username, loginModel.password, out errorMessage)) {
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
