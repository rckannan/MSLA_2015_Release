/// <reference path="../../TestModule/seed-page.tmpl.html" />
'use strict';

(function () {
    'use strict';

    angular.module('security').controller('userController', userController);

    /* @ngInject */
    function userController($mdDialog, $mdToast, $filter, arrHTTPDBQueryService) {
        var vm = this;
        vm.user = {
            fldUser_ID: -1,
            fldUserName: '',
            fldPassword: '',
            fldFullUserName: '',

            fldActiveUser: true,
            fldForceChangePassword: false,
            fldEmailAddress: '',
            fldPasswordLastUpdated: new Date(),
            fldGroupName: '',
            fldDomainName: ''
        };

        vm.Menus = {};
        vm.businessObject = {};
        vm.cancelClick = cancelClick;
        vm.okClick = saveFeedback;
        vm.gridBtnClick = GridBtnClick;

        vm.Reqobj = new Object();
        vm.Reqobj.RequestObject = "Security.User.Select";
        vm.Reqobj.Params = [];
        vm.Reqobj.TimeOut = 30;

        vm.mainGridOptions = {
            dataSource: {
                transport: {
                    read: function read(e) {
                        //var uris = '/api/User' + '?$skip=1&$top=1';
                        arrHTTPDBQueryService.PostFillDT(vm.Reqobj).then(function (response) {
                            e.success(response.data.data.rows);
                        });
                    }
                },

                serverPaging: false,
                serverSorting: true
            },
            sortable: true,
            pageable: false,
            filterable: true,
            dataBound: function dataBound() {
                this.expandRow(this.tbody.find("tr.k-master-row").first());
            },
            columns: [{
                name: "upload",
                text: "Upload",
                template: '<a ng-click="vm.gridBtnClick(dataItem)" class="k-button k-button-icontext k-grid-upload">Edit</a>',
                width: "80px"
            }, {
                field: "fldUserName",
                title: "UserName"
            }, {
                field: "fldFullUserName",
                title: "FullUserName"
            }, {
                field: "fldEmailAddress",
                title: "EmailAddress"
            }, {
                field: "fldLastUpdated",
                title: "LastUpdated"
            }]
        };

        //vm.detailGridOptions = function (dataItem) {
        //    return {
        //        dataSource: {
        //            type: "odata",
        //            transport: {
        //                read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Orders"
        //            },
        //            serverPaging: true,
        //            serverSorting: true,
        //            serverFiltering: true,
        //            pageSize: 5,
        //            filter: { field: "EmployeeID", operator: "eq", value: dataItem.EmployeeID }
        //        },
        //        scrollable: false,
        //        sortable: true,
        //        pageable: true,
        //        columns: [
        //        { field: "OrderID", title: "ID", width: "56px" },
        //        { field: "ShipCountry", title: "Ship Country", width: "110px" },
        //        { field: "ShipAddress", title: "Ship Address" },
        //        { field: "ShipName", title: "Ship Name", width: "190px" }
        //        ]
        //    };
        //};

        ////////////////

        function GridBtnClick(dataItem) {

            arrHTTPDBQueryService.GetBOData('Users', dataItem.fldUser_ID).then(function (response) {
                vm.businessObject = response.data.data;

                vm.user = {
                    fldUser_ID: response.data.data.propertyBag.fldUser_ID,
                    fldUserName: response.data.data.propertyBag.fldUserName,
                    fldPassword: response.data.data.propertyBag.fldPassword,
                    fldFullUserName: response.data.data.propertyBag.fldFullUserName,

                    fldActiveUser: response.data.data.propertyBag.fldActiveUser,
                    fldForceChangePassword: response.data.data.propertyBag.fldForceChangePassword,
                    fldEmailAddress: response.data.data.propertyBag.fldEmailAddress,
                    fldPasswordLastUpdated: response.data.data.propertyBag.fldPasswordLastUpdated,
                    fldGroupName: response.data.data.propertyBag.fldGroupName,
                    fldDomainName: response.data.data.propertyBag.fldDomainName
                };

                $mdDialog.show({
                    controller: 'userController',
                    controllerAs: 'vm',
                    templateUrl: 'app/Security/User/userDial.tmpl.html'
                }).then(function () {

                    $mdToast.show($mdToast.simple().content($filter('translate')('Successfully saved.')).position('bottom right').hideDelay(2000));
                });
            });
        }

        function cancelClick() {
            $mdDialog.cancel();
        }

        function saveFeedback() {
            //save here
            //arrHttpMenuService.postData('api/Feedback', vm.user);
            //try {
            //    arrHttpMenuService.postData('/api/user', vm.user).then(function (response) {

            //            var resp = response;
            //        },
            // function (err, status) {
            //     $exceptionHandler('Error Message getMenus: ' + response.status + ' (' + response.statusText + ')' + err.error_description);
            //     vm.message = "Error Message getMenus : " + err.error + err.error_description + " -- " + status;
            // });
            //} catch (applyError) {
            //    $exceptionHandler(applyError);
            //}

            arrHTTPDBQueryService.PostFillDT(vm.Reqobj).then(function (response) {
                e.success(response.data.data.rows);
            });
        }
    }
})();

