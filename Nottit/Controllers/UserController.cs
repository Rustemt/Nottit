using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nottit.Models;

namespace Nottit.Controllers {
    public class UserController : BaseController {

        private object Transform(User user) {
            return new {
                id = user.Id,
                userName = user.UserName,
                comments = user.Comments.Select(c => new {
                    Id = c.Id,
                    LinkUrl = c.Link.Url,
                    Text = c.Text
                }),
                links = user.Links.Select(l => new {
                    Id = l.Id,
                    Title = l.Title,
                    Url = l.Url
                })
            };
        }
  
        // Get api/user
        [AllowAnonymous]
        public IEnumerable Get() {
            return Db.Users.AsEnumerable().Select(u => Transform(u));
        }

        // GET api/user/5
        [AllowAnonymous]
        public object Get(int id) {
            var user = Db.Users.FirstOrDefault(u => u.Id == id);

            if (user == null) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return Transform(user);
        }
    }
}
