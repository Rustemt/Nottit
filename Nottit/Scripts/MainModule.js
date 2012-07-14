angular.module('main', ['currentuser', 'challenge', 'challengeVote', 'challengeAssignment', 'challengeComment']).
config(function ($routeProvider) {
    $routeProvider.
        when('/', { controller: LandingPageController, templateUrl: '/App/LandingPage' }).
        when('/Profile', { controller: ProfileController, templateUrl: '/App/Profile' }).
        when('/Challenges', { controller: ChallengeController, templateUrl: '/App/Challenges' }).
        when('/Challenge/:Id', { controller: ChallengeDetailController, templateUrl: '/App/ChallengeDetail' }).
        when('/Join', { controller: JoinController, templateUrl: '/App/Join' }).
        otherwise({ redirectTo: '/' });
}).
config(function ($httpProvider) {
    var interceptor = ['$rootScope', '$q', function (scope, $q) {

        function success(response) {
            return response;
        }

        function error(response) {
            var status = response.status;

            if (status === 401) {
                var deferred = $q.defer();
                var req = {
                    config: response.config,
                    deferred: deferred
                }
                scope.requests401.push(req);
                $('#loginModal').modal()
                return deferred.promise;
            }
            // otherwise
            return $q.reject(response);

        }

        return function (promise) {
            return promise.then(success, error);
        }

    }];
    $httpProvider.responseInterceptors.push(interceptor);
}).
run(['$rootScope', '$http', function (scope, $http) {
    scope.DisplayName = "Not Logged In";
    scope.requests401 = [];

    scope.$on('event:loginConfirmed', function () {
        var i, requests = scope.requests401;
        for (i = 0; i < requests.length; i++) {
            retry(requests[i]);
        }
        scope.requests401 = [];

        function retry(req) {
            $http(req.config).then(function (response) {
                req.deferred.resolve(response);
            });
        }
    });
}]);

function LandingPageController($scope) {
}

function ProfileController($rootScope, $scope, CurrentUser, ChallengeAssignment) {
    $rootScope.user = CurrentUser.get();

    $scope.markComplete = function (challenge) {
        ChallengeAssignment.update({
            ChallengeId: challenge.Id,
            Completed: true
        }, function () {
            $rootScope.user.CurrentChallenges.splice($rootScope.user.CurrentChallenges.indexOf(challenge), 1);
            $rootScope.user.CompleteChallenges.push(challenge);
        });
    };
}

function ChallengeDetailController($scope, $routeParams, Challenge, ChallengeComment) {
    $scope.challenge = Challenge.get({ Id: $routeParams.Id });

    $scope.createComment = function () {
        var comment = new ChallengeComment($scope.newComment);
        comment.ChallengeId = $scope.challenge.Id;
        comment.$save(function (newComment) {
            $scope.challenge.Comments.splice(0, 0, newComment);
        });
    };
}

function ChallengeController($rootScope, $scope, Challenge, ChallengeVote, ChallengeAssignment) {
    $scope.challenges = Challenge.query();

    $scope.voteUp = function(challenge) {
        ChallengeVote.voteUp(challenge);
    };

    $scope.voteDown = function (challenge) {
        ChallengeVote.voteDown(challenge);
    };

    $scope.takeChallenge = function (challenge) {
        var assignment = new ChallengeAssignment({
            ChallengeId: challenge.Id
        });

        assignment.$save(function () {
            challenge.StateCurrentUser = 'assigned';
        });
    };

    $scope.showChallengeModal = function () {
        $('#createChallengeModal').modal();
    };

    $scope.create = function() {
        Challenge.save($scope.challenge, function (createdChallenge) {
            $scope.challenges.push(createdChallenge);
            $('#createChallengeModal').modal('hide');
        });
    };
}

function LoginController($rootScope, $scope, $http, $location) {
    $scope.login = function () {
        var payload = { EmailAddress: $scope.email, Password: $scope.password };
        $http.post('/api/Login', payload).success(function (data) {
            if (data.result === true) {
                delete $scope.loginErrorMessage;
                $scope.$emit('event:loginConfirmed');
                $('#loginModal').modal('hide')
            } else {
                $scope.loginErrorMessage = data.errorMessage;
            }
        });
    };
    $scope.logout = function () {
        $http.delete ('/api/Login').success(function () {
            delete $rootScope.user;
            $location.path('/');
        });
    };
}

function JoinController($rootScope, $scope, $http, $location) {
    $scope.join = function () {
        var payload = {
            UserName: $scope.username,
            EmailAddress: $scope.email,
            Password: $scope.password,
            PasswordConfirm: $scope.passwordConfirm
        };

        $http.post('/api/Join', payload).success(function (data) {
            if (data.result === true) {
                delete $scope.joinErrorMessage;
                $scope.emit('event:loginConfirmed');
                $location.path('/Profile');
            } else {
                $scope.joinErrorMessage = data.errorMessage;
            }
        }).
        error(function (data) {
            $scope.joinErrorMessage = data[0].Value;
        });
    };
}
