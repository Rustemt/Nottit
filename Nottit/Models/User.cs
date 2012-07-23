using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nottit.Models {
    public class User {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Link> Links { get; set; }

        public object Transform() {
            return new {
                Id = Id,
                UserName = UserName,
                Comments = (Comments ?? new List<Comment>()).Select(c => new {
                    Id = c.Id,
                    Link = new {
                        Id = c.Link.Id,
                        Url = c.Link.Url,
                        Title = c.Link.Title,
                        Submitter = new {
                            UserName = c.Link.Author.UserName,
                            Id = c.Link.AuthorId
                        },
                    },
                    Text = c.Text,
                    Author = new {
                        UserName = UserName,
                        Id = Id
                    }
                }),
                Links = (Links ?? new List<Link>()).Select(l => l.Transform(false, this))
            };
        }
    }
}