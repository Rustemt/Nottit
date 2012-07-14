angular.module('challengeComment', ['ngResource']).
    factory('ChallengeComment', function ($resource) {
        return $resource('/api/ChallengeComment');
    });