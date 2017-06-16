define(['app', 'underscore', 'dir-pagination'], function (app, _) {
    app.controller('dobiManageController', ['$scope', 'apiConstant', 'httpService', '$timeout', 'appUtility', function ($scope, apiConstant, httpService, $timeout, appUtility) {
        "use strict";

        $scope.currentPage = 1;
        $scope.pageSize = 10;

        $scope.Data = {
            Dobies: [],
            TotalDobies: 0,
            ShowingFrom: 0,
            ShowingTo: 0
        };

        $scope.Methods = {
            Init: function () {
                $scope.Methods.GetAllDobi($scope.currentPage);
            },
            GetAllDobi: function (pageNumber) {
                var skip = (pageNumber-1) * $scope.pageSize;
                httpService.get(apiConstant.dobi + "?skip=" + skip , function (dobi) {
                    $timeout(function() {
                        $scope.Data.Dobies = dobi.Data.DobiList;
                        $scope.Data.TotalDobies = dobi.Data.TotalDobi;
                        appUtility.AddParamWithPhotoUrl($scope.Data.Dobies);
                        $scope.Data.ShowingFrom = skip + 1;
                        $scope.Data.ShowingTo = skip + dobi.Data.DobiList.length;
                    });
                }, true);
            },
            PageChangeHandler: function (pageNum) {
                $scope.currentPage = pageNum;
                $scope.Methods.GetAllDobi(pageNum);
            }
        };

        console.log("Manage Dobi Controller");
        $scope.Methods.Init();
    }]);

});