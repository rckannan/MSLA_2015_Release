(function() {
    'use strict';

    angular
        .module('Arror.UI.authentication', [  ]).provider('$dashboardState', function ($stateProvider) {
            this.$get = function($state) {
                return {
                    /**
                 * @function app.dashboard.dashboardStateProvider.addState
                 * @memberof app.dashboard
                 * @param {string} title - the title used to build state, url & find template
                 * @param {string} controllerAs - the controller to be used, if false, we don't add a controller (ie. 'UserController as user')
                 * @param {string} templatePrefix - either 'content', 'presentation' or null
                 * @author Alex Boisselle
                 * @description adds states to the dashboards state provider dynamically
                 * @returns {object} user - token and id of user
                 */
                    addState: function (fldstateName, fldisabstract, fldtemplateUrl, fldurl, fldcontrollerName, fldcontrollerNameAs) {

                      //var stt =  $stateProvider.state.get();
                       
                        $stateProvider.state(fldstateName, {
                            abstract: fldisabstract,
                            templateUrl: fldtemplateUrl,
                            url: fldurl,
                            controller: fldcontrollerName,
                            controllerAs: fldcontrollerNameAs
                        });
                    },
                    getState: function (fldstateName) {
                        return $state.get(fldstateName); // returns true
                    }
                }
            }
        });
})();