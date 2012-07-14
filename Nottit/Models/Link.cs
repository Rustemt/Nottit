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
    }
}