using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nottit.Models;

namespace Nottit.Controllers {
    public class LinkController : BaseController {

        private IQueryable<Link> Links() {
            return Db.Links
                .Include("Comments.Author")
                .Include(l => l.Votes)
                .Include(l => l.Author)
                .OrderBy(l => l.Id);
        }

        // GET api/link
        [AllowAnonymous]
        public IEnumerable GetSummary() {
            return Links().AsEnumerable().Select(l => l.Transform(false, LoginManager.CurrentUser));
        }

        // GET api/link/5
        [AllowAnonymous]
        public object Get(int id) {
            var link = Links().FirstOrDefault(l => l.Id == id);

            if (link == null) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return link.Transform(true, LoginManager.CurrentUser);
        }

        // POST api/link
        [Authorize]
        public object Post(Link link) {
            var existing = Db.Links.FirstOrDefault(l => l.Url == link.Url);

            if (existing == null) {
                link.Author = LoginManager.CurrentUser;

                Db.Links.Add(link);
                Db.SaveChanges();

                link = Links().FirstOrDefault(l => l.Id == link.Id);

                return link.Transform(true, LoginManager.CurrentUser);
            } else {
                return null;
            }
        }
    }
}
