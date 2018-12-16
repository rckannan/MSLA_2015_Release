(function () {
    'use strict'; 

    /* @ngInject */
    function arrExceptionHandler() {
        // Provider
        var exceptionArray = [];
        this.exceptions = exceptionArray; 

        // Service
        this.$get = function ($injector) {
            return {
                exceptions: exceptionArray,
                addException: function (item) {
                    var states = $injector.get('$state');
                    item.menu = states.current.name;

                    exceptionArray.push(item);
                    var rootScope = $injector.get('$rootScope'); 
                    rootScope.$broadcast('arrExceptionEvent', item); 
                }  
            };
        };
    }

    angular
        .module('Arror.UI.Shared')
        .provider('arrException', arrExceptionHandler);
})();

