/// <reference path="../../TestModule/seed-page.tmpl.html" />
(function () {
    'use strict';

    angular
        .module('security')
        .controller('userController', userController);

    /* @ngInject */
    function userController($mdDialog, $mdToast, $filter, arrHTTPDBQueryService ) {
        var vm = this; 
        vm.Menus = {};
        vm.businessObject = {}; 
        vm.gridBtnClick = GridBtnClick;
        //vm.optionCallback = {};
        vm.Reqobj = new Object();
        vm.Reqobj.RequestObject = "Security.User.Select";
        vm.Reqobj.Params = [];
        vm.Reqobj.TimeOut = 30;

       
        vm.mainGridOptions = {
            dataSource: {
                transport: {
                    read: function(es) { 
                        arrHTTPDBQueryService.PostFillDT(vm.Reqobj).then(function (response) {
                            es.success(response.data.data.rows); 
                        });
                    }
                },
               
                serverPaging: false,
                serverSorting: true
            },
            sortable: true,
            reorderable: true,
            groupable: true,
            resizable: true,
            filterable: true,
            columnMenu: true,
            dataBound: function () {
                this.expandRow(this.tbody.find("tr.k-master-row").first());
            },
            columns: [{
                name: "upload",
                text: "Upload",
                template: '<a ng-click="vm.gridBtnClick(dataItem)" class="k-button k-button-icontext k-grid-upload">Edit</a>',
                width: "80px"
            },{
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
            } ]
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

                $mdDialog.show({
                    multiple: true,
                    controller: 'userDialogController',
                    controllerAs: 'vm',
                    templateUrl: 'app/Security/User/userDial.tmpl.html',
                    locals: {
                        businessObject: vm.businessObject
                    } 
                }).finally(function() {
                    //if (vm.optionCallback !== undefined) {
                    //    vm.mainGridOptions.dataSource.transport.read(vm.optionCallback);
                    //}
                }); 
            });  
        }

         
    }
})();