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

        private object Transform(Link link, bool includeComments) {
            return new {
                Id = link.Id,
                Title = link.Title,
                Url = link.Url,
                Submitter = new {
                    UserName = link.Author.UserName,
                    Id = link.AuthorId
                },
                CommentCount = link.Comments.Count,
                CommentsIncluded = includeComments,
                Comments = !includeComments ? null : link.Comments.Select(c => new {
                    Id = c.Id,
                    Author = c.Author.UserName,
                    AuthorId = c.AuthorId,
                    Text = c.Text
                })
            };

        }

        // GET api/link
        [AllowAnonymous]
        public IEnumerable GetSummary() {
            return Db.Links
                .Include(l => l.Comments)
                .Include(l => l.Author)
                .OrderBy(l => l.Id).AsEnumerable().Select(l => Transform(l, false));
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

            return Transform(link, true);
        }

        // POST api/link
        [Authorize]
        public object Post(Link link) {
            link.Author = LoginManager.CurrentUser;

            Db.Links.Add(link);
            Db.SaveChanges();

            link.Comments = new List<Comment>();

            return Transform(link, true);
        }
    }
}
