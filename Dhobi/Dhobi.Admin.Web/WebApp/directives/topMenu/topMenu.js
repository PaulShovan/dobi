define(['app', 'jquery', 'bootstrap'], function (app, $) {
    app = app || angular.module("topMenuModule", []);
    app.controller("topMenuController", function ($scope, $localStorage, roleConstant) {
        "use strict";
        $scope.TopMenuData = {
            TopMenuUrl: '',
            User: $localStorage.UserInfo,
            ViewName: $localStorage.ViewName,
            RoleName: ""
        };

        $scope.TopMenuMethods = {
            SelectTopMenu: function () {
                var role = $localStorage.UserInfo.Role;
                if (role === roleConstant.Superadmin) {
                    $scope.TopMenuData.TopMenuUrl = '/WebApp/directives/topMenu/superAdminMenu.html';
                } else if (role === roleConstant.Admin) {
                    $scope.TopMenuData.TopMenuUrl = '/WebApp/directives/topMenu/adminMenu.html';
                } else if (role === roleConstant.Manager) {
                    $scope.TopMenuData.TopMenuUrl = '/WebApp/directives/topMenu/managerMenu.html';
                }
            },
            SelectedRole: function () {
                switch ($scope.TopMenuData.User.Role) {
                    case roleConstant.Superadmin:
                        $scope.TopMenuData.RoleName = "Super Admin"; break;
                    case roleConstant.Admin:
                        $scope.TopMenuData.RoleName = "Admin"; break;
                    case roleConstant.Manager:
                        $scope.TopMenuData.RoleName = "Manager"; break;
                }
            },
            Logout: function () {
                $localStorage.$reset();
                window.location.href = '/Login';
            }
        };
        $scope.TopMenuMethods.SelectTopMenu();
        $scope.TopMenuMethods.SelectedRole();
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
