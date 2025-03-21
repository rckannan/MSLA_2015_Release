﻿
var app = angular.module('AngularAuthApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar']);

 
    app.config(function ($routeProvider) {
         

        $routeProvider.when("/home", {
            controller: "homeController",
            templateUrl: "app/views/home.html"
        });

        $routeProvider.when("/login", {
            controller: "loginController",
            templateUrl: "app/views/login.html"

        });

        //$routeProvider.when("/login", {
        //    controller: "loginController",
        //    templateUrl: "app/views/login.html" 

        //});


$routeProvider.when("/signup", {
            controller: "signupController",
            templateUrl: "/WebAngular/app/views/signup.html"
        });

        $routeProvider.when("/success", {
            controller: "successController",
            templateUrl: "app/views/success.html",
                controllerAs: 'vm'
        });


        $routeProvider.when("/refresh", {
            controller: "refreshController",
            templateUrl: "app/views/refresh.html"
        });

        $routeProvider.when("/tokens", {
            controller: "tokensManagerController",
            templateUrl: "app/views/tokens.html"
        });

        $routeProvider.when("/menus", {
            controller: "MenuController",
            templateUrl: "app/views/Menu.html"
        });

        $routeProvider.when("/GetDT", {
            controller: "GetDTController",
            templateUrl: "app/views/GetDT.html"
        });

        //$routeProvider.when("/associate", {
        //    controller: "associateController",
        //    templateUrl: "/WebAngular/app/views/associate.html"
        //});

        $routeProvider.otherwise({ redirectTo: "/home" });

    });
 

//var serviceBase = 'http://localhost:55041/';
    var serviceBase = 'http://fiddev1vmsrv/MSLA.WebAPI/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);

 