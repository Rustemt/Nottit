using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nottit.Controllers {
    public class CurrentUserController : BaseController {

        public object Get() {
            var currentUser = LoginManager.CurrentUser;

            if (currentUser == null) {
                return null;
            }
            return currentUser.Transform();
        }
    }
}
