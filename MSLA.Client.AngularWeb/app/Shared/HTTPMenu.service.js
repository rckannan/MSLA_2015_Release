(function () {
    'use strict';

    angular
        .module('Arror.UI.Shared')
        .service('arrHttpMenuService', arrHttpMenuService);

    /* @ngInject */
    function arrHttpMenuService($http, $q, $exceptionHandler, API_CONFIG) {

        var serviceBOBase = API_CONFIG.url; 

        this.getMenus = getMenus;
        this.PostErrors = postErrorLogs;
        this.postData = postData;

        function getMenus(reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'GET',
                url: serviceBOBase + reqObject
            }).then(function successCallback(response) {
                // Use “response” within the application
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged)
                $exceptionHandler('An error has occurred. HTTP error: ' + response.status + ' (' + response.statusText + ')');
                deferred.reject(response);
            }); 
            return deferred.promise;
        };

        function postErrorLogs(reqpath, reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'POST',
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
 