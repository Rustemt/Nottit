using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Nottit.Models;

namespace Nottit.Controllers {
    public class CommentController : BaseController {
        private object Transform(Comment comment) {
            return new {
                id = comment.Id,
                author = new {
                    userName = comment.Author.UserName,
                    id = comment.AuthorId
                },
                link = new {
                    title = comment.Link.Title,
                    url = comment.Link.Url,
                    id = comment.LinkId
                },
                text = comment.Text
            };
        }

        // GET api/comment
        [AllowAnonymous]
        public IEnumerable Get() {
            return Db.Comments.OrderBy(c => c.Id).Select(c => Transform(c));
        }

        // GET api/comment/5
        [AllowAnonymous]
        public object Get(int id) {
            var comment = Db.Comments.FirstOrDefault(c => c.Id == id);

            if (comment == null) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return Transform(comment);
        }

        // POST api/comment
        [Authorize]
        public object Post(Comment comment) {
            comment.AuthorId = LoginManager.CurrentUser.Id;

            Db.Comments.Add(comment);
            Db.SaveChanges();

            return Transform(comment);
        }
    }
}
