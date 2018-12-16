(function() {
    'use strict';

    angular
        .module('Arror.UI.components')
        .controller('DefaultToolbarController', DefaultToolbarController);

    /* @ngInject */
    function DefaultToolbarController($scope, $rootScope, $mdMedia, $translate, $mdDialog, $state, $element, $filter, $mdUtil, $mdSidenav, $mdToast, $timeout, $document, arrBreadcrumbsService, arrSettings, arrLayout, arrException, arrHTTPDBQueryService) {
        var vm = this;
        vm.breadcrumbs = arrBreadcrumbsService.breadcrumbs;
        vm.emailNew = false;
        vm.languages = arrSettings.languages;
        vm.openSideNav = openSideNav;
        vm.hideMenuButton = hideMenuButton;
        vm.switchLanguage = switchLanguage;
        vm.toggleNotificationsTab = toggleNotificationsTab;
        vm.isFullScreen = false;
        vm.fullScreenIcon = 'zmdi zmdi-fullscreen';
        vm.toggleFullScreen = toggleFullScreen;
        vm.exceptionCount = arrException.exceptions.length;
        vm.exceptions = arrException.exceptions;
        // initToolbar();

        ////////////////

        function openSideNav(navID) {
            $mdUtil.debounce(function(){
                $mdSidenav(navID).toggle();
            }, 300)();
        }

        function switchLanguage(languageCode) {
            $translate.use(languageCode)
            .then(function() {
                $mdToast.show(
                    $mdToast.simple()
                    .content($filter('translate')('MESSAGES.LANGUAGE_CHANGED'))
                    .position('bottom right')
                    .hideDelay(500)
                );
            });
        }

        function hideMenuButton() {
            return arrLayout.layout.sideMenuSize !== 'hidden' && $mdMedia('gt-sm');
        }

        function toggleNotificationsTab(tab) {
            $rootScope.$broadcast('triSwitchNotificationTab', tab);
            vm.openSideNav('notifications'); 
        }

        function toggleFullScreen() {
            vm.isFullScreen = !vm.isFullScreen;
            vm.fullScreenIcon = vm.isFullScreen ? 'zmdi zmdi-fullscreen-exit':'zmdi zmdi-fullscreen';
            // more info here: https://developer.mozilla.org/en-US/docs/Web/API/Fullscreen_API
            var doc = $document[0];
            if (!doc.fullscreenElement && !doc.mozFullScreenElement && !doc.webkitFullscreenElement && !doc.msFullscreenElement ) {
                if (doc.documentElement.requestFullscreen) {
                    doc.documentElement.requestFullscreen();
                } else if (doc.documentElement.msRequestFullscreen) {
                    doc.documentElement.msRequestFullscreen();
                } else if (doc.documentElement.mozRequestFullScreen) {
                    doc.documentElement.mozRequestFullScreen();
                } else if (doc.documentElement.webkitRequestFullscreen) {
                    doc.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
                }
            } else {
                if (doc.exitFullscreen) {
                    doc.exitFullscreen();
                } else if (doc.msExitFullscreen) {
                    doc.msExitFullscreen();
                } else if (doc.mozCancelFullScreen) {
                    doc.mozCancelFullScreen();
                } else if (doc.webkitExitFullscreen) {
                    doc.webkitExitFullscreen();
                }
            }
        }

        $scope.$on('newMailNotification', function(){
            vm.emailNew = true;
        });

        $scope.$on('arrExceptionEvent', function ($event, item) {
            vm.emailNew = true;
            vm.exceptionCount = arrException.exceptions.length;
 
            arrHTTPDBQueryService.PostError(item);
        });

        vm.openFeedback = openFeedback;

        /////////////////////////////////

        function openFeedback($event) {
            $mdDialog.show({
                controller: 'feedbackController',
                controllerAs: 'vm',
                templateUrl: 'app/triangular/components/toolbars/Feedback-dialog.tmpl.html', 
                targetEvent: $event
            })
            .then(function () {  
                // pop a toast
                $mdToast.show(
                    $mdToast.simple()
                    .content($filter('translate')('Feedback has been sent to administrator.'))
                    .position('bottom right')
                    .hideDelay(2000)
                );
            });
        }
    }
})();