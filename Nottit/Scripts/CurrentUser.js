angular.module('currentuser', ['ngResource']).
    factory('CurrentUser', function ($resource) {
        return $resource('/api/CurrentUser');
    });