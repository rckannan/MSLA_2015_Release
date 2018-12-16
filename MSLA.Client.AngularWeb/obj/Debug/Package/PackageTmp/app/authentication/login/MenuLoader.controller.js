(function() {
    'use strict';

    angular
        .module('Arror.UI.Shared')
        .controller('arrExceptionLoader', MenuLoaderController);

    /* @ngInject */
    function MenuLoaderController($state,$stateProvider, arrSettings, $stateParams, arrHttpMenuService) {
        var vm = this;
        vm.loadMenus = loginClick;
        //loginClick();
        vm.triSettings = arrSettings;
        

        vm.message = $stateParams.msg;
        vm.errmessage = '';
        ////////////////

        function loginClick() {

            arrHttpMenuService.getMenus('api/Menu').then(function (response) {
                //vm.value = response.data;

                angular.forEach(response.data, function (resp) { 
                    LoadState(resp.fldChildren);
                    $state.go('triangular.admin-default.seed-page');
                });
            },
              function (err, status) {
                  vm.message = err.message + " -- " + status;
              });

            //Load the state in the menus
            function LoadState(resp) {
                if (resp.fldMenuType === 'dropdown') {
                    LoadState(resp.fldChildren);
                } else {
                    $stateProvider.state(resp.fldstateName, {
                        abstract: resp.fldisabstract,
                        templateUrl: resp.fldtemplateUrl,
                        url: resp.fldurl,
                        controller: resp.fldcontrollerName,
                        controllerAs: resp.fldcontrollerNameAs
                    });


                }
            };
        }
 

        //$scope.$watch('region', function(newValue, oldValue) {
        //    console.log(newValue, oldValue);
        //});

        //$scope.$on('toggleDropdownMenu', function (event, item, open) {
        //    // if this is the item we are looking for
        //    if (triMenuItem.item === item) {
        //        triMenuItem.item.open = open;
        //    }
        //    else {
        //        triMenuItem.item.open = false;
        //    }
        //});
    }
})();