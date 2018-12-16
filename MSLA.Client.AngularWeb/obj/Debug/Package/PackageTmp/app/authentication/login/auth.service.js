(function () {
    'use strict';

    angular
        .module('Arror.UI.authentication')
        .service('arrAuthService', arrAuthService);

    /* @ngInject */
    function arrAuthService($http, $q, localStorageService, loginInfoService,API_CONFIG) {

        this.login = login;

        function logOut() {

            localStorageService.remove('authorizationData');

            authentication.isAuth = false;
            authentication.userName = "";
            authentication.useRefreshTokens = false;
            authentication.eMail = "";
            authentication.user_ID = "";

        }

        this.logOut = logOut;
        this.fillAuthData = fillAuthData;
        this.refreshToken = refreshToken;
        this.authDetail = authentication;
        
        var authentication = {
            isAuth: false,
            userName: "",
            useRefreshTokens: true,
            eMail: "",
            user_ID: "",
            sessionID :""
        };

        function login(loginData) {

            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.userName;

            
                data = data + "&client_id=" + API_CONFIG.clientID; 

            var deferred = $q.defer();

            $http.post(API_CONFIG.url + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
 
                localStorageService.set('authorizationData',
                {
                    token: response.access_token,
                    userName: loginData.userName,
                    refreshToken: response.refresh_token,
                    useRefreshTokens: true,
                    eMail: response.email,
                    userID: response.UserID,
                    sessionID: response.sessionID
                });
                
                authentication.isAuth = true;
                authentication.userName = loginData.userName;
                authentication.useRefreshTokens = true;
                authentication.eMail = response.email;
                authentication.user_ID = response.UserID;
                authentication.sessionID = response.sessionID;

                loginInfoService.userName = loginData.userName;
                loginInfoService.eMail = response.email;
                loginInfoService.user_ID = response.UserID;
                loginInfoService.sessionID = response.sessionID;

                deferred.resolve(response);

            }).error(function (err, status) {
                logOut();
                deferred.reject(err);
            });

            return deferred.promise;
        };

        function fillAuthData() {

            var authData = localStorageService.get('authorizationData');
            if (authData) {
                authentication.isAuth = true;
                authentication.userName = authData.userName;
                authentication.useRefreshTokens = authData.useRefreshTokens;
                authentication.eMail = authData.eMail;
                authentication.user_ID = authData.userID;
                authentication.sessionID = authData.sessionID;
            }

        };

        function refreshToken() {
            var deferred = $q.defer();

            var authData = localStorageService.get('authorizationData');

            if (authData) {

                if (authData.useRefreshTokens) {

                    var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + API_CONFIG.clientId;

                    localStorageService.remove('authorizationData');

                    $http.post(API_CONFIG.url + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                        localStorageService.set('authorizationData', {
                            token: response.access_token,
                            userName: response.userName,
                            refreshToken: response.refresh_token,
                            useRefreshTokens: true,
                            eMail: response.email,
                            userID: response.UserID,
                            sessionID: response.sessionID 
                        });

                        deferred.resolve(response);

                    }).error(function (err, status) {
                        logOut();
                        deferred.reject(err);
                    });
                }
            }

            return deferred.promise;
        }; 
    }
})();