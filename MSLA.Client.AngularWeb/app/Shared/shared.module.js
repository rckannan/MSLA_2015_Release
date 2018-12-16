(function () {
    'use strict';

   

    function f(f) {
        f.decorator("$exceptionHandler", [ 
            '$delegate',   "arrException", 'loginInfoService', function ($delegate, arrException, loginInfoService) {
                return function (exception, cause) {
                    //var rootScope = $injector.get('$rootScope');

                   
                    var errMessage = {
                        Ex: exception.message === undefined ? exception.data === undefined ? '' : exception.status + ' - ' + exception.statusText : exception.message,
                        status: exception.data === undefined ? '' : exception.status,
                        statusText: exception.data === undefined ? '' : exception.statusText,
                        stack: exception.stack === undefined ? '' : exception.stack,
                        stackArg: exception.data === undefined ? '' : typeof (exception.data) === 'object' ? exception.data.messageDetail === undefined ? exception.data.statusText : exception.data.messageDetail : exception.data,
                        timestamp: new Date(),//.toLocaleString('en-us', options),
                        icon: 'zmdi zmdi-alert-circle',
                        color: 'rgb(255, 152, 0)',
                        user_ID: loginInfoService.user_ID,
                        webClient_ID: loginInfoService.webClient_ID,
                        menu: ''
                };
                    arrException.addException(errMessage);
                    $delegate(exception, cause);
                    //rootScope.$broadcast('arrExceptionEvent');
                    //alert(exception.message === undefined ? exception === Object ? exception.data : exception : exception.message);
                     
                };
            }
        ]);
    }

    angular.module('Arror.UI.Shared', []).config(f), f.$inject = ["$provide", "arrExceptionProvider"];
})();