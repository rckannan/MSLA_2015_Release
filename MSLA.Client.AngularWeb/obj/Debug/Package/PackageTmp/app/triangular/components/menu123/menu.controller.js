(function() {
    'use strict';

    angular
        .module('Arror.UI.components')
        .controller('MenuController', MenuController);

    /* @ngInject */
    function MenuController(arrSettings, arrLayout) {
        var vm = this;
        vm.layout = arrLayout.layout;
        vm.sidebarInfo = {
            appName: arrSettings.name,
            appLogo: arrSettings.logo
        };
        vm.toggleIconMenu = toggleIconMenu;

        ////////////
        function toggleIconMenu() {
            var menu = vm.layout.sideMenuSize === 'icon' ? 'full' : 'icon';
            arrLayout.setOption('sideMenuSize', menu);
        }
    }
})();
