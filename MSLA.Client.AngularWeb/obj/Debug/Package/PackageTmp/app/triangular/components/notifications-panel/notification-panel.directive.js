(function() {
    'use strict';

    angular
        .module('Arror.UI.components')
        .directive('triError', triErrorNotification);

    /* @ngInject */
    function triErrorNotification($location, $mdTheming, arrTheming) {
        // Usage:
        //
        // Creates:
        //
        var directive = {
            restrict: 'E',
            //template: '<md-content><tri-menu-item ng-repeat="item in triExceptionController.exceptions | orderBy:\'timestamp\'" item="::item"></tri-menu-item></md-content>',
            //template: '<md-list-item class="md-3-line" ng-repeat="email in ::triExceptionController.exceptions" ng-click="vm.openMail(email)" layout-align="space-between center"> <md-icon md-font-icon="{{::email.icon}}" ng-style="{ color: email.color }"></md-icon><div class="md-list-item-text"><div class="email-inbox-list-item-name-time" layout="row" layout-align="space-between start"><h6 class="md-body-1">{{::email.Ex}}</h6><p class="md-body-2" am-time-ago="::email.timestamp"></p></div> </div><md-divider ng-hide="$last"></md-divider></md-list-item>',
            template: '<div ng-include="::triExceptionController.template"></div>',
            scope: { item: '=' },
            controller: triExceptionController,
            controllerAs: 'triExceptionController', 
            bindToController: true
        };
        return directive;

        //function link($scope, $element) {
        //    $mdTheming($element);
        //    var $mdTheme = $element.controller('mdTheme'); //eslint-disable-line

        //    var menuColor = arrTheming.getThemeHue($mdTheme.$mdTheme, 'primary', 'default');
        //    var menuColorRGBA = arrTheming.rgba(menuColor.value);
        //    $element.css({ 'background-color': menuColorRGBA });
        //    $element.children('md-content').css({ 'background-color': menuColorRGBA });
        //}
    }

    /* @ngInject */
    function triExceptionController($scope,arrException) {
        var triExceptionController = this;
        // get the menu and order  t 
        triExceptionController.template = 'app/triangular/components/notifications-panel/notifications-panel.Error.tmpl.html';
        triExceptionController.exceptions = arrException.exceptions;
 
    }
})();
