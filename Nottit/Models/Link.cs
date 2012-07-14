using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nottit.Models {
    public class Link {
        public virtual int Id { get; set; }

        public virtual int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual string Title { get; set; }
        public virtual string Url { get; set; }

        public object Transform(bool includeComments) {
            var comments = this.Comments ?? new List<Comment>();

            return new {
                Id = Id,
                Title = Title,
                Url = Url,
                Submitter = new {
                    UserName = Author.UserName,
                    Id = AuthorId
                },
                CommentCount = comments.Count,
                CommentsIncluded = includeComments,
                Comments = !includeComments ? null : comments.Select(c => new {
                    Id = c.Id,
                    Author = c.Author.UserName,
                    AuthorId = c.AuthorId,
                    Text = c.Text
                })
            };

        }
    }
}