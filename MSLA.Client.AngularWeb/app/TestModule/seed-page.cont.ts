 
//import Angular = require("../../scripts/typings/angularjs/angular");
"use strict";

interface ISeed {
    textdata: string[];  
    values: any;
}

interface ISeedModuleController extends ISeed { 
};

class seedModuleController implements ISeedModuleController {
   
    textdata: any[];
    constructor($stateParams, arrHTTPService, triMenu) {
        var vm = this;
        vm.textdata = ['triangular', 'is', 'great', 'AEIOU']; 

        arrHTTPService.getData('/api/refreshtokens').then(response => {
            vm.values =  response.data;

        }).catch(reason => {
            vm.values =  reason.message;
        });
    }
    values: any[];

   
   
}

Angular.angular.angular.module('seed-module').component('seedModuleController', seedModuleController);