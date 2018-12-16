(function() {
    'use strict';

    angular
        .module('Arror.UI')
        .run(runFunction);

    /* @ngInject */
    function runFunction($rootScope, $window, $state, $filter, $translate, $timeout, arrRoute, arrBreadcrumbsService) {
        var breadcrumbs = arrBreadcrumbsService.breadcrumbs;
        init();

        var destroyOn = $rootScope.$on('$stateChangeSuccess', function(){
            setFullTitle();
        });

        $rootScope.$on('$destroy', function(){
            destroyOn();
        });

        function setFullTitle() {
            $timeout(function(){
                var title = arrRoute.title;
                angular.forEach(breadcrumbs.crumbs, function(crumb){
                    title += ' ' + arrRoute.separator + ' ' + $filter('translate')(crumb.name);
                });
                $window.document.title = title;
            });
        }

        function init() {
            setFullTitle();
        }
    }
})();
