(function () {
    'use strict';

    angular
        .module('Arror.UI.components')
        .service('arrauthInterceptorService', arrauthInterceptorService);

    /* @ngInject */
    function arrauthInterceptorService($q, $injector, $location, $exceptionHandler, localStorageService) {
        this.request = _request;
        this.responseError = _responseError;

       function _request(config) {

            config.headers = config.headers || {};
            var authData = localStorageService.get('authorizationData');
            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
                config.headers.sessionID = authData.sessionID;
            } else {
                //AuthState.go('authentication.login');
                $location.path('/login/Current session is missing. Please relogin');
            }
            return config;
        }

       function _responseError(rejection) {
            if (rejection.status === 401) {
                var authService = $injector.get('arrAuthService');
                var authData = localStorageService.get('authorizationData');

                if (authData) {
                    //if (authData.useRefreshTokens) {
                    //    //$location.path('/refresh');
                        
                        return $q.reject(rejection);
                    //}
                }
                authService.logOut();
                //$location.path('/logout');
                $location.path('/login/Current session is missing. Please relogin');
            } else if (rejection.status === 404) {
                //$location.path('/404');
                //throw  'HTTP response error: ' + rejection.status + ' (' + rejection.statusText + ')'  ;
                $exceptionHandler(rejection);
            } else if (rejection.status === 500) {
                //$location.path('/500');
                $exceptionHandler(rejection);
                //$exceptionHandler('An error has occurred. HTTP error: ' + rejection.status + ' (' + rejection.statusText + ')');
                //throw  'HTTP response error: ' + rejection.status + ' (' + rejection.statusText + ')'   ;
            }
            else  {
                //$location.path('/500');
                $exceptionHandler(rejection);
                //$exceptionHandler('An error has occurred. HTTP error: ' + rejection.status + ' (' + rejection.statusText + ')');
                //throw  'HTTP response error: ' + rejection.status + ' (' + rejection.statusText + ')'   ;
            }
            return $q.reject(rejection);
        }

    }
})();