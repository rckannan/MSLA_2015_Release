(function() {
    'use strict';

    angular
        .module('Arror.UI.authentication')
        .controller('LoginController', LoginController);

    /* @ngInject */
    function LoginController($scope, $state, $exceptionHandler, arrSettings, $stateParams, arrAuthService, $dashboardState, triMenu, arrHTTPDBQueryService) {
        var vm = this;
        vm.loginClick = loginClick;
        vm.socialLogins = [{
            icon: 'fa fa-twitter',
            color: '#5bc0de',
            url: '#'
        },{
            icon: 'fa fa-facebook',
            color: '#337ab7',
            url: '#'
        },{
            icon: 'fa fa-google-plus',
            color: '#e05d6f',
            url: '#'
        },{
            icon: 'fa fa-linkedin',
            color: '#337ab7',
            url: '#'
        }];
        vm.triSettings = arrSettings;
        // create blank user variable for login form
        vm.user = {
            userName: '' 
        };
        vm.showLogin = true;
        vm.message = $stateParams.msg;
        vm.message1 = '';
        vm.errmessage = '';
        ////////////////
        function States(resp1) {
            if (resp1.fldMenuType === 'dropdown') {
                angular.forEach(resp1.fldChildren, function (child) {
                    States(child);
                });
            } else {
                var isExist = $dashboardState.getState(resp1.fldstateName);

                if (isExist == null) {
                    $dashboardState.addState(resp1.fldstateName, resp1.fldisabstract,
                        resp1.fldtemplateUrl, resp1.fldurl, resp1.fldcontrollerName, resp1.fldcontrollerNameAs);
                } 
            }
        }

        function loginClick() {
            vm.showLogin = false;
            vm.message1 = "authenticating.....";
            arrAuthService.login(vm.user).then(function (response) {
                //$state.go('User.Auth.Profile');
                
                vm.message1 = "Login Success";
                
                    //arrException('err','err msg');
                //$dashboardState.addState('triangular.admin-default.profile', false, 'app/authentication/profile/profile.tmpl.html', '/profile', 'ProfileController', 'vm');
                //$dashboardState.addState('triangular.admin-default.forms-binding', false, 'app/forms/binding.tmpl.html', '/forms/binding', '', '');

                
                    arrHTTPDBQueryService.GetQuery('api/Authorization/GetMenus').then(function (response) {

                        angular.forEach(response.data.menus, function (resp) {
                            States(resp);
                        });
                        var dsd = JSON.parse(response.data.menuStates);
                        triMenu.removeAllMenu();
                        angular.forEach(JSON.parse(response.data.menuStates).Menudatas, function (resp) {
                            triMenu.addMenu(resp);
                        });

                        $state.go('triangular.admin-default.profile');
                    } );  
                vm.message = "Login Success";
 
            },
             function (err) { 
                  vm.message = err.error_description;
                  vm.errmessage = err;
                  $exceptionHandler('An error has while login. HTTP error: ' + response.status + ' (' + response.statusText + ')');
             }); 
        }; 
    } 
})();