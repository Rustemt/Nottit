angular.module('main', ['ngResource']).
factory('CurrentUser', function ($resource) {
    return $resource('/api/CurrentUser');
}).
factory('Link', function ($resource) {
    return $resource('/api/Link');
}).
factory('Comment', function ($resource) {
    return $resource('/api/Comment');
}).
factory('User', function ($resource) {
    return $resource('/api/User');
}).
directive('nottitLink', function () {
    return {
        restrict: 'E',
        replace: true,
        templateUrl: '/app/LinkTemplate'
    }
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

            if (response.config.url !== '/api/CurrentUser' && status === 401) {
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
run(['$rootScope', '$http', 'CurrentUser', function (scope, $http, CurrentUser) {
    scope.appTitle = 'Nottit';
    scope.requests401 = [];
    scope.user = CurrentUser.get();

    scope.doLogin = function () {
        $('#loginModal').modal();
    };

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

function LinksController($rootScope, $scope, Link) {
    var dialog = '#createLinkModal';

    $scope.links = Link.query();

    $scope.showCreate = function () {
        $(dialog).modal();
    };

    $scope.create = function () {
        var link = new Link($scope.newLink);
        link.$save(function (result) {
            $scope.links = Link.query();
            $(dialog).modal('hide');
        });
    };

    $rootScope.$on('event:loginConfirmed', function () {
        $scope.links = Link.query();
    });
}

function UserDetailController($scope, $routeParams, User) {
    $scope.user = User.get({ Id: $routeParams.Id });
}

function LinkDetailController($scope, $routeParams, Link, Comment) {
    $scope.link = Link.get({ Id: $routeParams.Id });

    $scope.createComment = function () {
        var comment = new Comment($scope.newComment);
        comment.LinkId = $scope.link.Id;
        comment.$save(function (newComment) {
            $scope.link.Comments.splice(0, 0, newComment);
        });
    };
}

function LoginController($rootScope, $scope, $http, $location) {
    $scope.login = function () {
        var payload = { username: $scope.username, password: $scope.password };
        $http.post('/api/Login', payload).success(function (data) {
            if (data.Result === true) {
                $rootScope.user = data.User;
                delete $scope.loginErrorMessage;
                $scope.$emit('event:loginConfirmed');
                $('#loginModal').modal('hide');
            } else {
                $scope.loginErrorMessage = data.ErrorMessage;
            }
        });
    };
    $scope.logout = function () {
        $http.delete('/api/Login').success(function () {
            delete $rootScope.user;
            $location.path('/');
        });
    };
}

