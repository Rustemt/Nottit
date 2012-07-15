using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Nottit.Models;

namespace Nottit.Controllers {
    public class LinkVoteController : BaseController {

        private void NormalizeValue(LinkVote vote) {
            if (vote.Value < 0) {
                vote.Value = -1;
            } else if (vote.Value > 1) {
                vote.Value = 1;
            }
        }

        private object SetVote(LinkVote vote) {
            NormalizeValue(vote);

            var existingVote = Db.LinkVotes.FirstOrDefault(lv => lv.VoterId == LoginManager.CurrentUser.Id && lv.LinkId == vote.LinkId);

            if (existingVote != null) {
                if (vote.Value == 0) {
                    Db.LinkVotes.Remove(existingVote);
                } else {
                    existingVote.Value = vote.Value;
                }
            } else {
                vote.Voter = LoginManager.CurrentUser;
                Db.LinkVotes.Add(vote);
            }

            int voteTally = Db
                .Links
                .Include(l => l.Votes)
                .FirstOrDefault(l => l.Id == vote.LinkId).VoteTally;

            return new {
                VoteTally = voteTally,
                UpvoteCurrentUser = vote.Value == 1,
                DownvoteCurrentUser = vote.Value == -1
            };
        }

        // POST api/linkvote
        [Authorize]
        public object Post(LinkVote vote) {
            // TODO: if no LinkId, return an error
            var result = SetVote(vote);
            Db.SaveChanges();

            return result;
        }
    }
}
