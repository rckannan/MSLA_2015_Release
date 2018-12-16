//import Angular = require("../../scripts/typings/angularjs/angular");
"use strict";
;
var seedModuleController = (function () {
    function seedModuleController($stateParams, arrHTTPService, triMenu) {
        var vm = this;
        vm.textdata = ['triangular', 'is', 'great', ''];
        arrHTTPService.getData('/api/refreshtokens').then(function (response) {
            vm.values = response.data;
        }).catch(function (reason) {
            vm.values = reason.message;
        });
    }
    return seedModuleController;
}());
Angular.angular.angular.module('seed-module').component('seedModuleController', seedModuleController);
