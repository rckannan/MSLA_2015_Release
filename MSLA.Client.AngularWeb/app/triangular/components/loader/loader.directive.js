(function() {
    'use strict';

    angular
        .module('Arror.UI.components')
        .directive('arrLoader', arrLoader);

    /* @ngInject */
    function arrLoader($rootScope) {
        var directive = {
            bindToController: true,
            controller: arrLoaderController,
            controllerAs: 'vm',
            template: '<div flex class="loader" ng-show="vm.status.active" layout="column" layout-fill layout-align="center center"><div class="loader-inner"><md-progress-circular md-mode="indeterminate"></md-progress-circular></div><h3 class="md-headline">{{vm.appName}}</h3></div>',
            link: link,
            restrict: 'E',
            replace: true,
            scope: {
            }
        };
        return directive;

        function link($scope) {
            var loadingListener = $rootScope.$on('$viewContentLoading', function() {
                $scope.vm.setLoaderActive(true);
            });

            var loadedListener = $rootScope.$on('$viewContentLoaded', function() {
                $scope.vm.setLoaderActive(false);
            });

            $scope.$on('$destroy', removeListeners);

            function removeListeners() {
                loadingListener();
                loadedListener();
            }
        }
    }

    /* @ngInject */
    function arrLoaderController($rootScope, arrLoaderService, arrSettings) {
        var vm = this;
        vm.appName = arrSettings.name;
        vm.status = arrLoaderService.status;
        vm.setLoaderActive = arrLoaderService.setLoaderActive;
    }
})();