﻿//define(['app'], function (app) {
//    app.controller('viewSwichController', ['$scope', '$state', '$localStorage', 'roleConstant',
//        function ($scope, $state, $localStorage, roleConstant) {
//            "use strict";

//            $scope.ViewSwichMethods = {
//                Init: function () {
//                    $scope.ViewSwichMethods.SelectRoute();
//                },
//                SelectRoute: function () {
//                    var role = $localStorage.UserInfo.Role;
//                    var viewName = $localStorage.ViewName;
                   
//                    if (viewName === roleConstant.SuperAdmin && role === roleConstant.SuperAdmin) {
//                        // redirect to the SuperAdmin page
//                        $state.go('dashboard');
//                    } else if (viewName === roleConstant.Admin && role === roleConstant.Admin) {
//                        // redirect to the Admin page
//                    } else if (viewName === roleConstant.Manager && role === roleConstant.Manager) {
//                        // redirect to the Manager page
//                        $state.go('dashboard');
//                    }
//                }
//            };

//            $scope.ViewSwichMethods.Init();
//        }]);
//});