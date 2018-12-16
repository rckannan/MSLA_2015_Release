(function() {
    'use strict';

    angular
        .module('Arror.UI.components')
        .directive('arrMenu', arrMenuDirective);

    /* @ngInject */
    function arrMenuDirective($location, $mdTheming, arrTheming) {
        // Usage:
        //
        // Creates:
        //
        var directive = {
            restrict: 'E',
            template: '<md-content><arr-menu-item ng-repeat="item in triMenuController.menu | orderBy:\'priority\'" item="::item"></arr-menu-item></md-content>',
            scope: {},
            controller: arrMenuController,
            controllerAs: 'triMenuController',
            link: link
        };
        return directive;

        function link($scope, $element) {
            $mdTheming($element);
            var $mdTheme = $element.controller('mdTheme'); //eslint-disable-line

            var menuColor = arrTheming.getThemeHue($mdTheme.$mdTheme, 'primary', 'default');
            var menuColorRGBA = arrTheming.rgba(menuColor.value);
            $element.css({ 'background-color': menuColorRGBA });
            $element.children('md-content').css({ 'background-color': menuColorRGBA });
        }
    }

    /* @ngInject */
    function arrMenuController(arrMenu) {
        var triMenuController = this;
        // get the menu and order it
        triMenuController.menu = arrMenu.menu;
    }
})();
