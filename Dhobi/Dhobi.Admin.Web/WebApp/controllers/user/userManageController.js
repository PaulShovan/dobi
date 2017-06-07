define(['app', 'underscore', 'dir-pagination'], function (app, _) {
    app.controller('userManageController', ['$scope', 'apiConstant', 'httpService', '$timeout', function ($scope, apiConstant, httpService, $timeout) {
        "use strict";

        $scope.currentPage = 1;
        $scope.pageSize = 10;

        $scope.Data = {
            Users: [],
            TotalUsers: 0
        };

        $scope.Methods = {
            Init: function () {
                $scope.Methods.GetAllUser($scope.currentPage);
            },
            GetAllUser: function (pageNumber) {
                var skip = (pageNumber - 1) * $scope.pageSize;
                httpService.get(apiConstant.getAllUser + "?skip=" + skip, function (user) {
                    $timeout(function () {
                        $scope.Data.Users = user.Data.DobiList;
                        $scope.Data.TotalUsers = user.Data.TotalDobi;
                    });
                }, true);
            },
            PageChangeHandler: function (pageNum) {
                $scope.currentPage = pageNum;
                $scope.Methods.GetAllUser(pageNum);
            }
        };

        console.log("Manage User Controller");
        $scope.Methods.Init();
    }]);

});