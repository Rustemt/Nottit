using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nottit.Models {
    public class Comment {
        public virtual int Id { get; set; }

        public virtual int LinkId { get; set; }
        public virtual Link Link { get; set; }

        public virtual int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public virtual string Text { get; set; }
    }
}