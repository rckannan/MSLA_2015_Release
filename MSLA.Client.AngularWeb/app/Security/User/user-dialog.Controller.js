(function () {
    'use strict';

    angular
        .module('security')
        .controller('userDialogController', userDialogController);

    /* @ngInject */
    function userDialogController($mdToast,$mdDialog, arrHTTPDBQueryService, businessObject) {
        var vm = this;
        vm.BusinessObject = businessObject; 
        vm.cancelClick = cancelClick;
        vm.okClick = saveDialog;
        vm.statusText = '';

        function cancelClick() {
            $mdDialog.cancel();
        }

        function saveDialog() {  
            arrHTTPDBQueryService.PostBOData(vm.BusinessObject).then(function (response) {
                vm.BusinessObject = response.data.data;
                vm.statusText = response.data.statusText;
            });
        }
    }
})();