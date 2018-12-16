(function () {
    'use strict';

    angular
        .module('Arror.UI.authentication')
        .service('loginInfoService', loginInfoService);

    /* @ngInject */
    function loginInfoService(API_CONFIG) {
 
        this.userName = "";
        this.eMail = "";
        this.user_ID = "";
        this.sessionID = "";
        this.webClient_ID = API_CONFIG.clientID;
    };
           
})();