(function() {
    'use strict';

    angular
        .module('Arror.UI.authentication')
        .config(moduleConfig) 
       ;
    //.run(moduleRun), moduleRun.$inject = ["$rootScope", "$stateProvider", "arrHttpMenuService"]; 
   

    /* @ngInject */
    function moduleConfig($translatePartialLoaderProvider, $stateProvider) { 

        $translatePartialLoaderProvider.addPart('app/authentication');
        //var AuthState = $injector.get('API_CONFIG');
        $stateProvider
            .state('authentication', {
                abstract: true,
                templateUrl: 'app/authentication/layouts/authentication.tmpl.html'
            })
            .state('authentication.login', {
                url: '/login/:msg',
                templateUrl: 'app/authentication/login/login.tmpl.html',
                controller: 'LoginController',
                controllerAs: 'vm'
            }); 
    }
     
})();