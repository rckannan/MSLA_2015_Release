'use strict';
app.controller('successController', ['$scope','authService', function ($scope, authService) {
    //var authobjts = this;
    $scope.authobjts = authService.authentication;
}]);