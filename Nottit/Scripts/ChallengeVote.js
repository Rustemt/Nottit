angular.module('challengeVote', []).
    factory('ChallengeVote', function ($http) {
        function sendVote(challenge, value) {
            $http.post('/api/ChallengeVote', {
                ChallengeId: challenge.Id,
                Value: value
            }).success(function (data) {
                challenge.VoteTally = data.VoteTally;
                challenge.UpvoteCurrentUser = data.UpvoteCurrentUser;
                challenge.DownvoteCurrentUser = data.DownvoteCurrentUser;
            });
        }

        return {
            voteUp: function (challenge) {
                sendVote(challenge, challenge.UpvoteCurrentUser ? 0 : 1);
            },
            voteDown: function (challenge) {
                sendVote(challenge, challenge.DownvoteCurrentUser ? 0 : -1);
            }
        };
    });