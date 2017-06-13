define(['app', 'jquery', 'bootstrap'], function (app, $) {
    app = app || angular.module("topMenuModule", []);
    app.controller("topMenuController", function ($scope, $localStorage, roleConstant) {
        "use strict";
        $scope.TopMenuData = {
            TopMenuUrl: '',
            User: $localStorage.UserInfo,
            ViewName: $localStorage.ViewName,

            SuperAdminMenues: [
                { Title: 'ADD NEW DOBI', Link: 'dobiadd', Cls: 'fa-chevron-right', Submenu: [] },
                { Title: 'MANAGE DOBI', Link: 'dobimanage', Cls: 'fa-chevron-right', Submenu: [] },
                { Title: 'ADD MANAGER', Link: 'manageradd', Cls: 'fa-chevron-right', Submenu: [] },
                { Title: 'PAYMENTS', Link: 'payments', Cls: 'fa-chevron-right', Submenu: [] },
                { Title: 'MANAGE PROMO OFFERS', Link: 'managepromo', Cls: 'fa-chevron-right', Submenu: [] },
                { Title: 'ORDERS', Link: 'orders', Cls: 'fa-chevron-right', Submenu: [] }
            ],

            AdminMenues: [
                { Title: 'Admin Menu 1', Link: 'admin.Link1', Cls: 'fa-bars', Submenu: [] },
                { Title: 'Admin Menu 2', Link: 'admin.Link2', Cls: 'fa-picture-o', Submenu: [] },
                { Title: 'Admin Menu 3', Link: 'admin.Link3', Cls: 'fa-music', Submenu: [] },
                //{
                //    Title: 'settings', Link: 'settings', Cls: 'fa-music', Submenu: [
                //      { Title: 'Account Settings', Link: 'settings.accountDetails' },
                //      { Title: 'Users', Link: 'settings.users' },
                //      { Title: 'Billing', Link: 'settings.billings' }]
                //}
            ],

            ManagerMenues: [
                { Title: 'Manager Menu 1', Link: 'manager.link1', Cls: 'fa-music', Submenu: [] },
                { Title: 'Manager Menu 1', Link: 'manager.link2', Cls: 'fa-bars', Submenu: [] },
                { Title: 'Manager Menu 1', Link: 'manager.link3', Cls: 'fa-picture-o', Submenu: [] },
                //{
                //    Title: 'settings', Link: 'settings', Cls: 'fa-cog', Submenu: [
                //      { Title: 'Account Details', Link: 'settings.accountDetails' },
                //      { Title: 'Users', Link: 'settings.users' }, { Title: 'Venues', Link: 'settings.venues' }]
                //}
            ]
        };

        $scope.TopMenuMethods = {
            SelectTopMenu: function () {
                var role = $localStorage.UserInfo.Role;
                if (role === roleConstant.SuperAdmin) {
                    $scope.TopMenuData.TopMenuUrl = '/WebApp/directives/topMenu/superAdminMenu.html';
                } else if (role === roleConstant.Admin) {
                    $scope.TopMenuData.TopMenuUrl = '/WebApp/directives/topMenu/adminMenu.html';
                } else if (role === roleConstant.Manager) {
                    $scope.TopMenuData.TopMenuUrl = '/WebApp/directives/topMenu/managerMenu.html';
                }
            },
            Logout: function () {
                $localStorage.$reset();
                window.location.href = '/Login';
            }
        };
        $scope.TopMenuMethods.SelectTopMenu();
    });

    app.directive('topmenu', function () {
        "use strict";
        return {
            restrict: 'E',
            controller: 'topMenuController',
            templateUrl: '/WebApp/directives/topMenu/topMenu.html'
        };
    });
});
