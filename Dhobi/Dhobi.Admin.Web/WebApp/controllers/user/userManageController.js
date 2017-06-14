define(['app', 'underscore', 'dir-pagination'], function (app, _) {
    app.controller('userManageController', ['$scope', 'apiConstant', 'httpService', '$timeout', function ($scope, apiConstant, httpService, $timeout) {
        "use strict";

        $scope.currentPage = 1;
        $scope.pageSize = 10;

        $scope.Data = {
            Users: [],
            TotalUsers: 0,
            ShowingFrom: 0,
            ShowingTo: 0
        };

        $scope.Methods = {
            Init: function () {
                $scope.Methods.GetAllUser($scope.currentPage);
            },
            GetAllUser: function (pageNumber) {
                var skip = (pageNumber - 1) * $scope.pageSize;
                httpService.get(apiConstant.user + "?skip=" + skip, function (user) {
                    $timeout(function () {
                        $scope.Data.Users = user.Data.ManagerList;
                        $scope.Data.TotalUsers = user.Data.TotalManager;
                        $scope.Data.ShowingFrom = skip + 1;
                        $scope.Data.ShowingTo = skip + user.Data.ManagerList.length;
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