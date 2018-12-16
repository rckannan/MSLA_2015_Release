(function () {
    'use strict';

    angular
        .module('Arror.UI.Shared')
        .service('arrHTTPDBQueryService', arrHTTPDBQueryService);

    /* @ngInject */
    function arrHTTPDBQueryService($http, $q, $exceptionHandler, $mdDialog, $mdToast,API_CONFIG, loginInfoService) {

        var serviceBOBase = API_CONFIG.url; 

        this.GetQuery = getQuery;
        this.PostFillDT = postFillDt;
        this.PostExecNonQry = postExecNonQry;
        this.PostExecSclr = postExecSclr;
        this.PostError = postError;
        this.PostFeedback = postFeedback;

        this.GetBOData = getBOData;
        this.PostBOData = postBOData;

        function getQuery(reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'GET', 
                url: serviceBOBase + reqObject
            }).then(function successCallback(response) { 
                deferred.resolve(response);
            }, function errorCallback(response) {
                $exceptionHandler(response);
                deferred.reject(response);
            }); 
            return deferred.promise;
        };

        function postFillDt(reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'POST', 
                url: serviceBOBase + 'api/QueryDB/FillDT',
                data: JSON.stringify(reqObject) 
            }).then(function successCallback(response) {
                // Use “response” within the application
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged) 
                $mdDialog.show(
                      $mdDialog.confirm()
                      .title('Error')
                      .textContent('OOPs.... Something went wrong... Please contact Administrator.')
                      .ok('Close') 
                  );
                deferred.reject(response);
            });
            return deferred.promise;
        };

        function postExecNonQry( reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'POST', 
                url: serviceBOBase + 'api/QueryDB/ExecNonQry',
                data: JSON.stringify(reqObject)
            }).then(function successCallback(response) {
                // Use “response” within the application
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged) 
                $exceptionHandler(response);
                $mdDialog.show(
                     $mdDialog.confirm()
                     .title('Error')
                     .textContent('OOPs.... Something went wrong... Please contact Administrator.')
                     .ok('Close')
                 );
                deferred.reject(response);
            });
            return deferred.promise;
        };

        function postExecSclr(reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'POST', 
                url: serviceBOBase + 'api/QueryDB/ExecNonQry',
                data: JSON.stringify(reqObject)
            }).then(function successCallback(response) {
                // Use “response” within the application
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged) 
                $exceptionHandler(response);
                $mdDialog.show(
                     $mdDialog.confirm()
                     .title('Error')
                     .textContent('OOPs.... Something went wrong... Please contact Administrator.')
                     .ok('Close')
                 );
                deferred.reject(response);
            });
            return deferred.promise;
        };

        function postError(reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'POST',
                url: serviceBOBase + 'api/QueryDB/PostException',
                data: JSON.stringify(reqObject)
            }).then(function successCallback(response) {
                // Use “response” within the application
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged) 
                //$exceptionHandler( 'An error has occurred. HTTP error: ' + response.status + ' (' + response.statusText + ')');
                deferred.reject(response);
            });
            return deferred.promise;
        };

        function postFeedback(reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'POST',
                url: serviceBOBase + 'api/QueryDB/PostFeedback',
                data: JSON.stringify(reqObject)
            }).then(function successCallback(response) {
                // Use “response” within the application
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged) 
                //$exceptionHandler( 'An error has occurred. HTTP error: ' + response.status + ' (' + response.statusText + ')');
                deferred.reject(response);
            });
            return deferred.promise;
        };

        function getBOData(DocType,Doc_ID) {
            var deferred = $q.defer();

            $http({
                method: 'GET',
                url: serviceBOBase + 'api/QueryBO/FetchBO/' + DocType + '/' + Doc_ID  
            }).then(function successCallback(response) {
                // Use “response” within the application
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged) 
                $mdDialog.show(
                      $mdDialog.confirm()
                      .title('Error')
                      .textContent('OOPs.... Something went wrong... Please contact Administrator.')
                      .ok('Close').skipHide(true)
                  );
                deferred.reject(response);
            });
            return deferred.promise;
        };

        function postBOData(reqObject) {
            var deferred = $q.defer();

            $http({
                method: 'POST',
                url: serviceBOBase + 'api/QueryBO/SaveBO',
                data: JSON.stringify(reqObject)
            }).then(function successCallback(response) {
                // Use “response” within the application

                //Validate for Broken Rules
                if (response.data.status === 412) {
                    //Broken rules hit
                    $mdDialog.show({
                        controller: 'feedbackController',
                        controllerAs: 'vm',
                        templateUrl: 'app/triangular/components/toolbars/Feedback-dialog.tmpl.html' 
                    });

                } else if (response.data.status === 201) {
                    //Success
                    
                }
                 
                $mdToast.show(
                       $mdToast.simple()
                       .content(response.data.statusText)
                       .position('bottom right')
                       .hideDelay(5000)
                   );
                deferred.resolve(response);
            }, function errorCallback(response) {
                // Well-handled error (details are logged) 
                $mdDialog.show(
                     $mdDialog.confirm()
                     .title('Error')
                     .textContent('OOPs.... Something went wrong... Please contact Administrator.')
                     .ok('Close')
                 );
                deferred.reject(response);
            });
            return deferred.promise;
        };
    }
})();
 