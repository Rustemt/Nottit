using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nottit.Models {
    public class Link {
        private bool HasUpvoteFromUser(User user) {
            return user != null
                && Votes.Any(v => v.VoterId == user.Id && v.Value == 1);
        }

        private bool HasDownvoteFromUser(User user) {
            return user != null
                && Votes.Any(v => v.VoterId == user.Id && v.Value == -1);
        }

        public virtual int Id { get; set; }

        public virtual int AuthorId { get; set; }
        public virtual User Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual string Title { get; set; }
        public virtual string Url { get; set; }

        public virtual ICollection<LinkVote> Votes { get; set; }

        public int VoteTally {
            get {
                if (Votes == null) {
                    return 0;
                }
                return Votes.Aggregate(0, (accum, vote) => accum + vote.Value);
            }
        }

        public object Transform(bool includeComments, User currentUser) {
            var comments = this.Comments ?? new List<Comment>();

            return new {
                Id = Id,
                Title = Title,
                Url = Url,
                Submitter = new {
                    UserName = Author.UserName,
                    Id = AuthorId
                },
                VoteTally = VoteTally,
                CommentCount = comments.Count,
                CommentsIncluded = includeComments,
                Comments = !includeComments ? null : comments.Select(c => new {
                    Id = c.Id,
                    Author = new {
                        Id = c.AuthorId,
                        UserName = c.Author.UserName,
                    },
                    Text = c.Text
                }),
                UpvoteCurrentUser = HasUpvoteFromUser(currentUser),
                DownvoteCurrentUser = HasDownvoteFromUser(currentUser)
            };

        }
    }
}