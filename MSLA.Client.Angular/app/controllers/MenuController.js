'use strict';
app.controller('MenuController', ['$scope', 'tokensManagerService', function ($scope, tokensManagerService) {

    $scope.menus = [];

    tokensManagerService.getMenus().then(function (results) {

        $scope.Menus = results.data;

    }, function (error) {
        alert(error.data.message);
    });
     

}]);