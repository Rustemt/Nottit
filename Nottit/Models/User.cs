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
    }
}