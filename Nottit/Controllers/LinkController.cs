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

        // GET api/link
        [AllowAnonymous]
        public IEnumerable GetSummary() {
            return Db.Links
                .Include(l => l.Comments)
                .Include(l => l.Author)
                .OrderBy(l => l.Id).AsEnumerable().Select(l => l.Transform(false));
        }

        // GET api/link/5
        [AllowAnonymous]
        public object Get(int id) {
            var link = Db.Links
                .Include(l => l.Comments)
                .Include(l => l.Author)                
                .FirstOrDefault(l => l.Id == id);

            if (link == null) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return link.Transform(true);
        }

        // POST api/link
        [Authorize]
        public object Post(Link link) {
            var existing = Db.Links.FirstOrDefault(l => l.Url == link.Url);

            if (existing == null) {
                link.Author = LoginManager.CurrentUser;

                Db.Links.Add(link);
                Db.SaveChanges();

                link.Comments = new List<Comment>();

                return link.Transform(true);
            } else {
                return null;
            }
        }
    }
}
