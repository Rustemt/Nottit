using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Nottit.Models {
    public class LinkVote {
        public virtual int Id { get; set; }

        public virtual int Value { get; set; }

        public virtual int VoterId { get; set; }
        public virtual User Voter { get; set; }

        public virtual int LinkId { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public virtual Link Link { get; set; }
    }
}