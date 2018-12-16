(function () {
    'use strict';

    angular
        .module('Arror.UI.Shared')
        .service('arrHttpBaseService', arrHttpBaseService);

    /* @ngInject */
    function arrHttpBaseService($http, $q, $exceptionHandler, API_CONFIG) {

        var serviceBOBase = API_CONFIG.url; 

        this.getBase = getBases;
        this.PostBase = postBase;

        function getBases(reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'GET',
                header: { 'Content-Type': 'application/json; charset=utf-8' },
                url: serviceBOBase + reqObject
            }).then(function successCallback(response) { 
                deferred.resolve(response);
            }, function errorCallback(response) {
                $exceptionHandler(response);
                deferred.reject(response);
            }); 
            return deferred.promise;
        };

        function postBase(reqpath, reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'POST',
                header: { 'Content-Type': 'application/json; charset=utf-8' },
                url: serviceBOBase + reqpath,
                data: JSON.stringify(reqObject) 
            }).then(function successCallback(response) {
                // Use “response” within the application
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged) 
                //$exceptionHandler( 'An error has occurred. HTTP error: ' + response.status + ' (' + response.statusText + ')');
                //deferred.reject(response);
            });
            return deferred.promise;
        };

        function postData(reqpath, reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'POST',
                header: { 'Content-Type': 'application/json; charset=utf-8' },
                url: serviceBOBase + reqpath,
                data: JSON.stringify(reqObject)
            }).then(function successCallback(response) {
                // Use “response” within the application
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged) 
                $exceptionHandler(response);
                deferred.reject(response);
            });
            return deferred.promise;
        };

       
    }
})();
 