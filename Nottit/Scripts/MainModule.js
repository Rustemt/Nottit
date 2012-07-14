angular.module('main', ['ngResource']).
factory('Link', function ($resource) {
    return $resource('/api/Link');
}).
factory('Comment', function ($resource) {
    return $resource('/api/Comment');
}).
factory('User', function ($resource) {
    return $resource('/api/User');
}).
config(function ($routeProvider) {
    $routeProvider.
        when('/', { controller: LinksController, templateUrl: '/App/Links' }).
        when('/User/:Id', { controller: UserDetailController, templateUrl: '/App/UserDetail' }).
        when('/Link/:Id', { controller: LinkDetailController, templateUrl: '/App/LinkDetail' }).
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
    scope.appTitle = 'Nottit';
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

function LinksController($scope, Link) {
    $scope.links = Link.query();
}

function UserDetailController($rootScope, $scope, User) {
}

function LinkDetailController($scope, $routeParams, Link) {
    //$scope.challenge = Challenge.get({ Id: $routeParams.Id });

    //$scope.createComment = function () {
    //    var comment = new ChallengeComment($scope.newComment);
    //    comment.ChallengeId = $scope.challenge.Id;
    //    comment.$save(function (newComment) {
    //        $scope.challenge.Comments.splice(0, 0, newComment);
    //    });
    //};
}

function LoginController($rootScope, $scope, $http, $location) {
    $scope.login = function () {
        var payload = { username: $scope.username, password: $scope.password };
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

