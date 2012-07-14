angular.module('challenge', ['ngResource']).
    factory('Challenge', function ($resource) {
        return $resource('/api/Challenge');
    });