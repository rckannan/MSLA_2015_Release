(function () {
    'use strict';
app.controller('GetDTController', ['$scope', 'tokensManagerService', function ($scope, tokensManagerService) {

    $scope.Menus = [];
    $scope.EiborRes = [];

    tokensManagerService.getDT().then(function (results) {

        $scope.Menus = results.data;

    }, function (error) {
        alert(error.data.message);
    });

    $scope.Reqobj = new Object(); 
    $scope.Reqobj.RequestObject = "EiborFetch";
    $scope.Reqobj.Params = [];
    $scope.Reqobj.TimeOut = 30;

    $scope.GetData = function () {

        tokensManagerService.getDTParam($scope.Reqobj).then(function (results) {
           
            $scope.EiborRes = results.data;

        }, function (error) {
            alert(error.data.message);
        });
    };

    $scope.authCompletedCB = function (fragment) { alert('Alertsds'); };
     

}])

})();