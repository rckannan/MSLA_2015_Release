(function () {
    'use strict';

    angular
        .module('Arror.UI.components')
        .controller('ErrorNotiDialogController', ErrorNotiDialogController);

    /* @ngInject */
    function ErrorNotiDialogController($window, $mdDialog, email) {
        var vm = this;
        vm.cancelClick = cancelClick;
        vm.okClick = okClick;
        vm.email = email;
        vm.printClick = printClick;


        ////////////////

        function okClick() {
            $mdDialog.hide();
        }

        function cancelClick() {
            $mdDialog.cancel();
        }

        function printClick() {
            $window.print();
        }

    }
})();