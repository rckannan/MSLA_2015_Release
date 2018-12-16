(function() {
    'use strict';
app.factory('tokensManagerService', ['$http', '$q', 'ngAuthSettings', function ($http, $q, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    
    var tokenManagerServiceFactory = {};

    var _getRefreshTokens = function () {

        return $http.get(serviceBase + 'api/refreshtokens').then(function (results) {
            return results;
        });
    };

    var _getMenus = function () {

        return $http.get(serviceBase + 'api/Menu').then(function (results) {
            return results;
        });
    };

    var _getDT = function () {

        return $http.get(serviceBase + 'api/QueryDB/FillDT').then(function (results) {
            return results;
        });
    };

    var _getDTParam = function (ReqObject) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/QueryDB/FillDT', ReqObject, { headers: { 'Content-Type': 'application/json; charset=utf-8' } }).success(function (response) {
             
            deferred.resolve(response);

        }).error(function (err, status) { 
            deferred.reject(err);
        });

        return deferred.promise; 
       
    };

    var _deleteRefreshTokens = function (tokenid) {

        return $http.delete(serviceBase + 'api/refreshtokens/?tokenid=' + tokenid).then(function (results) {
            return results;
        });
    };

    tokenManagerServiceFactory.deleteRefreshTokens = _deleteRefreshTokens;
    tokenManagerServiceFactory.getRefreshTokens = _getRefreshTokens;
    tokenManagerServiceFactory.getMenus = _getMenus;
    tokenManagerServiceFactory.getDT = _getDT;
    tokenManagerServiceFactory.getDTParam = _getDTParam;

    return tokenManagerServiceFactory;

}]) 

})();