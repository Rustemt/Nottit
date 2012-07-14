angular.module('challengeAssignment', ['ngResource']).
    factory('ChallengeAssignment', function ($resource) {
        return $resource('/api/ChallengeAssignment',
            {},
            {
                update: { method: 'PUT' }
            });
    });