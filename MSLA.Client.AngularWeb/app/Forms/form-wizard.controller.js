(function() {
    'use strict';

    angular
        .module('Arror.UI.App.forms')
        .controller('FormWizardController', FormWizardController);

    /* @ngInject */
    function FormWizardController() {
        var vm = this;
        vm.data = {};
    }
})();