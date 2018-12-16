(function () {
    'use strict';

    angular
        .module('Arror.UI.Shared')
        .service('arrHTTPService', arrHTTPService);

    /* @ngInject */
    function arrHTTPService($http, $q, API_CONFIG) {

        var serviceBOBase = API_CONFIG.url + '/api/QueryBO', serviceQueryBase = API_CONFIG.url + '/api/QueryDBFetch',
                   serviceBase = API_CONFIG.url;

        this.getData = getData;
        this.postData = postData;
        //this.getBOObject = getBOObject;
        //this.postBOObject = postBOObject;

        function getData(reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'GET',
                url: serviceBase + reqObject
            }).then(function successCallback(response) {
                // Use “response” within the application
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged)
                //$exceptionHandler(“An error has occurred.\nHTTP error: ” + response.status + “ (“ + response.statusText + “)”);
                deferred.reject(response);
            });

            //$http.get(serviceBase + reqObject).success(function (response) { 
 
               

            //}).error(function (err, status) {
            //   //do a logging here;
            //    deferred.reject(err);
            //});

            return deferred.promise;
        };

        function postData(path, reqObject) {
            var deferred = $q.defer();

            $http.post(serviceBase + path, reqObject).success(function (response) {

                deferred.resolve(response);

            }).error(function (err, status) {
                //do a logging here;
                deferred.reject(err);
            });

            return deferred.promise;
        };
         
         
    }
})();
 