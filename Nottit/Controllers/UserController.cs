using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nottit.Models;
using nUser = Nottit.Models.User;

namespace Nottit.Controllers {
    public class UserController : BaseController {
  
        // Get api/user
        [AllowAnonymous]
        public IEnumerable Get() {
            return Db.Users
                .Include(u => u.Comments)
                .Include(u => u.Links)
                .AsEnumerable().Select(u => u.Transform());
        }

        // GET api/user/5
        [AllowAnonymous]
        public object Get(int id) {
            var user = Db.Users
                .Include(u => u.Comments)
                .Include("Links.Author")              
                .FirstOrDefault(u => u.Id == id);

            if (user == null) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return user.Transform();
        }
    }
}
