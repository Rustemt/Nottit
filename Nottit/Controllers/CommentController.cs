using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
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
                Id = comment.Id,
                Author = new {
                    UserName = comment.Author.UserName,
                    Id = comment.AuthorId
                },
                Link = new {
                    Title = comment.Link.Title,
                    Url = comment.Link.Url,
                    Id = comment.LinkId
                },
                Text = comment.Text
            };
        }

        // GET api/comment
        [AllowAnonymous]
        public IEnumerable Get() {
            return Db.Comments
                .Include(c => c.Author)
                .Include(c => c.Link)
                .OrderBy(c => c.Id).AsEnumerable().Select(c => Transform(c));
        }

        // GET api/comment/5
        [AllowAnonymous]
        public object Get(int id) {
            var comment = Db.Comments
                .Include(c => c.Author)
                .Include(c => c.Link)                
                .FirstOrDefault(c => c.Id == id);

            if (comment == null) {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return Transform(comment);
        }

        // POST api/comment
        [Authorize]
        public object Post(Comment comment) {
            comment.Author = LoginManager.CurrentUser;

            Db.Comments.Add(comment);
            Db.SaveChanges();

            return Transform(comment);
        }
    }
}
