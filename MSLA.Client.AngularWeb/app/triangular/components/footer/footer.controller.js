(function() {
    'use strict';

    angular
        .module('Arror.UI.components')
        .controller('FooterController', FooterController);

    /* @ngInject */
    function FooterController(arrSettings, arrLayout) {
        var vm = this;
        vm.name = arrSettings.name;
        vm.copyright = arrSettings.copyright;
        vm.layout = arrLayout.layout;
        vm.version = arrSettings.version;
    }
})();