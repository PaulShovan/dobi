define(['app', 'underscore', 'dir-pagination'], function (app, _) {
    app.controller('dobiManageController', ['$scope', 'apiConstant', 'httpService', '$timeout', function ($scope, apiConstant, httpService, $timeout) {
        "use strict";

        $scope.currentPage = 1;
        $scope.pageSize = 10;

        $scope.Data = {
            Dobies: [],
            TotalDobies: 0
        };

        $scope.Methods = {
            Init: function () {
                $scope.Methods.GetAllDobi($scope.currentPage);
            },
            GetAllDobi: function(pageNumber) {
                httpService.get(apiConstant.getAllDobi, function (dobi) {
                    $timeout(function() {
                        $scope.Data.Dobies = dobi.Data.DobiList;
                        //$scope.Data.TotalDobies = dobi.Total;
                    })
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