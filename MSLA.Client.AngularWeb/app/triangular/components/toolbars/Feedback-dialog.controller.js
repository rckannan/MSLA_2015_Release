(function () {
    'use strict';

    angular
        .module('Arror.UI.components')
        .controller('feedbackController', feedbackController);

    /* @ngInject */
    function feedbackController($state, $mdDialog, loginInfoService, arrHTTPDBQueryService) {
        var vm = this;

        vm.feedBackDetail = {
            menu: $state.current.name,
            description: '',
            user_ID: loginInfoService.user_ID,
            webClientID : loginInfoService.webClient_ID,
            updatedOn : new Date()
        };

        vm.cancelClick = cancelClick;
        vm.okClick = saveFeedback;   

        ////////////////
 
        function cancelClick() {
            $mdDialog.cancel();
        } 

        function saveFeedback() {
            //save here
            arrHTTPDBQueryService.PostFeedback(vm.feedBackDetail);
            $mdDialog.hide();
        }
    }
})();