(function() {
    'use strict';
    app.factory('authService', [
        '$http', '$q', 'localStorageService', 'ngAuthSettings', function($http, $q, localStorageService, ngAuthSettings) {

            var serviceBase = ngAuthSettings.apiServiceBaseUri;
            var authServiceFactory = {};

            //authServiceFactory.saveRegistration = _saveRegistration;


            //authServiceFactory.obtainAccessToken = _obtainAccessToken;
            //authServiceFactory.externalAuthData = _externalAuthData;
            //authServiceFactory.registerExternal = _registerExternal;


            var authentication = {
                isAuth: false,
                userName: "",
                useRefreshTokens: false,
                eMail: "",
                user_ID: ""
            };
            var logOut = {};
            var login = function(loginData) {

                var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

                if (loginData.useRefreshTokens) {
                    data = data + "&client_id=" + ngAuthSettings.clientId;
                }

                var deferred = $q.defer();

                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function(response) {

                    if (loginData.useRefreshTokens) {
                        localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: response.refresh_token, useRefreshTokens: true, eMail: response.email, userID: response.UserID });
                    } else {
                        localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false, eMail: response.email, userID: response.UserID });
                    }
                    authentication.isAuth = true;
                    authentication.userName = loginData.userName;
                    authentication.useRefreshTokens = loginData.useRefreshTokens;
                    authentication.eMail = response.email;
                    authentication.user_ID = response.UserID;

                    deferred.resolve(response);

                }).error(function(err, status) {
                    logOut();
                    deferred.reject(err);
                });

                return deferred.promise;

            };
            logOut = function() {

                localStorageService.remove('authorizationData');

                authentication.isAuth = false;
                authentication.userName = "";
                authentication.useRefreshTokens = false;
                authentication.eMail = "";
                authentication.user_ID = "";

            };
            var fillAuthData = function() {

                var authData = localStorageService.get('authorizationData');
                if (authData) {
                    authentication.isAuth = true;
                    authentication.userName = authData.userName;
                    authentication.useRefreshTokens = authData.useRefreshTokens;
                    authentication.eMail = authData.eMail;
                    authentication.user_ID = authData.userID;
                }

            };

            var refreshToken = function() {
                var deferred = $q.defer();

                var authData = localStorageService.get('authorizationData');

                if (authData) {

                    if (authData.useRefreshTokens) {

                        var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                        localStorageService.remove('authorizationData');

                        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function(response) {

                            localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });

                            deferred.resolve(response);

                        }).error(function(err, status) {
                            logOut();
                            deferred.reject(err);
                        });
                    }
                }

                return deferred.promise;
            };

            authServiceFactory.login = login;
            authServiceFactory.logOut = logOut;
            authServiceFactory.fillAuthData = fillAuthData;
            authServiceFactory.authentication = authentication;
            authServiceFactory.refreshToken = refreshToken;
            return authServiceFactory;
        }
    ]);

})();