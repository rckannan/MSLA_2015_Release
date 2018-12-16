(function() {
    'use strict';

    angular
       .module('security')
       .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($translatePartialLoaderProvider) {
        $translatePartialLoaderProvider.addPart('app/Security'); 
    }
})(); 