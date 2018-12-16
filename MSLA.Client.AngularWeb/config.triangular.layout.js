(function() {
    'use strict';

    angular
        .module('Arror')
        .config(config);

    /* @ngInject */
    function config(arrLayoutProvider) {
        arrLayoutProvider.setDefaultOption('toolbarSize', 'default');

        arrLayoutProvider.setDefaultOption('toolbarShrink', false);

        arrLayoutProvider.setDefaultOption('toolbarClass', '');

        arrLayoutProvider.setDefaultOption('contentClass', '');

        arrLayoutProvider.setDefaultOption('sideMenuSize', 'full');

        arrLayoutProvider.setDefaultOption('showToolbar', true);

        arrLayoutProvider.setDefaultOption('footer', true);
    }
})();