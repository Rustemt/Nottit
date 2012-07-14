using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nottit.Models;

namespace Nottit.Controllers {
    public class LinkController : BaseController {

        private object Transform(Link link, bool includeComments) {
            return new {
                id = link.Id,
                title = link.Title,
                url = link.Url,
                author = new {
                    userName = link.Author.UserName,
                    id = link.AuthorId
                },
                commentCount = link.Comments.Count,
                commentsIncluded = includeComments,
                comments = !includeComments ? null : link.Comments.Select(c => new {
                    id = c.Id,
                    author = c.Author.UserName,
                    authorId = c.AuthorId,
                    text = c.Text
                })
            };

        }

        // GET api/link
        [AllowAnonymous]
        public IEnumerable GetSummary() {
            return Db.Links.OrderBy(l => l.Id).Select(l => Transform(l, false));
        }

        // GET api/link/5
        [AllowAnonymous]
        public object Get(int id) {
            var link = Db.Links.FirstOrDefault(l => l.Id == id);

            if (link == null) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return Transform(link, true);
        }

        // POST api/link
        public object Post(Link link) {
            link.AuthorId = LoginManager.CurrentUser.Id;

            Db.Links.Add(link);
            Db.SaveChanges();

            return Transform(link, true);
        }
    }
}
