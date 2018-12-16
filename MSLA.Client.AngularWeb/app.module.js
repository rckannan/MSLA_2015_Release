(function() {
    'use strict';

    angular
        .module('Arror', [
            'Arror.UI',
            'ngAnimate', 'ngCookies', 'ngSanitize', 'ngMessages', 'ngMaterial',
            'ui.router', 'pascalprecht.translate', 'LocalStorageModule', 'googlechart', 'chart.js', 'linkify', 'ui.calendar', 'angularMoment', 'textAngular', 'uiGmapgoogle-maps', 'hljs', 'md.data.table', angularDragula(angular), 'ngFileUpload' 
            , 'seed-module', 'Arror.UI.App.forms', 'Arror.UI.Shared', 'Arror.UI.authentication', 'security'
            // uncomment above to activate the example seed module
           // 'app.examples'
        ])
        // create a constant for languages so they can be added to both triangular & translate
        .constant('APP_LANGUAGES', [{
            name: 'LANGUAGES.CHINESE',
            key: 'zh'
        },{
            name: 'LANGUAGES.ENGLISH',
            key: 'en'
        },{
            name: 'LANGUAGES.FRENCH',
            key: 'fr'
        },{
            name: 'LANGUAGES.PORTUGUESE',
            key: 'pt'
        }])
        // set a constant for the API we are connecting to
        .constant('API_CONFIG', {
            'url': 'http://localhost/MSLA.WebAPI/',
            'clientID': 'MSLAApp'

        });
})();
 
//.config(function ($httpProvider) {
//    $httpProvider.interceptors.push('arrauthInterceptorService');
//})


(function() {
    "use strict";

    function e(e) {
        e.interceptors.push('arrauthInterceptorService');
        e.defaults.xsrfCookieName = 'csrftoken';
        e.defaults.xsrfHeaderName = 'X-CSRFToken';
        e.defaults.headers.post['Content-Type'] = 'application/json; charset=utf-8';
        e.defaults.headers.post['Content-Encoding'] = 'gzip';
    }

     
    //angular.module("Arror").config(e).config(f), e.$inject = ["$httpProvider"], f.$inject = ["$provide", "arrExceptionProvider"  ];
    angular.module("Arror").config(e) , e.$inject = ["$httpProvider"] ;
})

();



(function() {
    "use strict";

    function e(e, t,h) {
        e.state("404", {
            url: "/404",
            templateUrl: "404.tmpl.html",
            controllerAs: "vm",
            controller: [
                "$state", function(e) {
                    var t = this;
                    t.goHome = function() {
                        e.go("authentication.login");
                    }
                }
            ]
        }).state("500", {
            url: "/500",
            templateUrl: "500.tmpl.html",
            controllerAs: "vm",
            controller: [
                "$state", function(e) {
                    var t = this;
                    t.goHome = function() {
                        e.go("authentication.login");
                    }
                }
            ]
        }), t.when("", "/login/"), t.when("/", "/login/"), t.otherwise("/404");
    }

    angular.module("Arror").config(e), e.$inject = ["$stateProvider", "$urlRouterProvider"];
})

();