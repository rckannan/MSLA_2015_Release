(function() {
    'use strict';

    angular
        .module('seed-module')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($translatePartialLoaderProvider, $stateProvider, triMenuProvider) {
        $translatePartialLoaderProvider.addPart('app/seed-module');

        //$stateProvider
        //.state('User.Test.Page', {
        //    url: '/seed-module/seed-page/:id',
        //    templateUrl: 'app/TestModule/seed-page.tmpl.html',
        //    // set the controller to load for this page
        //    controller: 'SeedPageController',
        //    controllerAs: 'vm'
        //});

        //triMenuProvider.addMenu({
        //    name: 'MENU.SEED.SEED-MODULE',
        //    icon: 'zmdi zmdi-grade',
        //    type: 'dropdown',
        //    priority: 1.2,
        //    children: [
        //        {
        //            name: 'MENU.SEED.SEED-PAGE',
        //            state: 'triangular.admin-default.seed-page',
        //            type: 'link'
        //        }, {
        //            name: 'MENU.SEED.SEED-PAGE1',
        //            state: 'triangular.admin-default.seed-page',
        //            type: 'link'
        //        }
        //    ]
        //});

       // triMenuProvider.addMenu({
       //    type: "divider",
       //    priority: 2.4
       //});
    }
})();