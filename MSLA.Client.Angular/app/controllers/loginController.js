'use strict';
app.controller('loginController', ['$routeParams',
      '$scope', '$location', 'authService' , function ($routeParams, $scope, $location, authService) {

          var param1 = $routeParams.param1;
          $scope.loginData = {
              userName: param1,
              password: "",
              useRefreshTokens: false
          };

          $scope.message = "";

          $scope.login = function () {

              authService.login($scope.loginData).then(function (response) {

                  $location.path('/success');
                  $scope.message = 'succ';
              },
               function (err) {
                   $scope.message = err.error_description;
                   $scope.errmessage = err;
               });

              $scope.authCompletedCB = function (fragment) {

                  $scope.$apply(function () {

                      if (fragment.haslocalaccount == 'False') {

                          authService.logOut();

                          authService.externalAuthData = {
                              provider: fragment.provider,
                              userName: fragment.external_user_name,
                              externalAccessToken: fragment.external_access_token
                          };

                          $location.path('/associate');

                      }
                      else {
                          //Obtain access token and redirect to orders
                          var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                          authService.obtainAccessToken(externalData).then(function (response) {

                              $location.path('/success');

                          },
                       function (err) {
                           $scope.message = err.error_description;
                       });
                      }

                  });
              }
          };

          $scope.$apply(function () {

              if (fragment.haslocalaccount == 'False') {

                  authService.logOut();

                  authService.externalAuthData = {
                      provider: fragment.provider,
                      userName: fragment.external_user_name,
                      externalAccessToken: fragment.external_access_token
                  };

                  $location.path('/associate');

              }
              else {
                  //Obtain access token and redirect to orders
                  var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                  authService.obtainAccessToken(externalData).then(function (response) {

                      $location.path('/success');

                  },
               function (err) {
                   $scope.message = err.error_description;
               });
              }

          });
      }




]);