(function () {
    'use strict';

    angular
        .module('Arror.UI.Shared')
        .controller('arrExceptionController', arrExceptionController);

    /* @ngInject */
    function arrExceptionController( arrHttpMenuService) {
        var vm = this;
        vm.loginClick = loginClick; 

    }




})();