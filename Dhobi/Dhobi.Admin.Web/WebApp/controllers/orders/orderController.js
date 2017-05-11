define(['app', 'underscore', 'dir-pagination'], function (app, _) {
    app.controller('orderController', ['$scope', 'apiConstant', 'httpService', '$timeout',
        function ($scope, apiConstant, httpService, $timeout) {
            "use strict";

            $scope.currentPage = 1;
            $scope.pageSize = 10;

            $scope.Data = {
                Orders: [],
                TotalOrders: null
            };

            $scope.Methods = {
                Init: function () {
                    //$scope.Methods.GetAllOrders($scope.currentPage);
                },
                GetAllOrders: function (pageNumber) {
                    var skip = (pageNumber - 1) * $scope.pageSize;
                    httpService.get(apiConstant.getAllOrders + "?skip=" + skip, function (orders) {
                        $timeout(function () {
                            $scope.Data.Orders = orders.Data.OrderList;
                            $scope.Data.TotalOrders = orders.Data.TotalOrders;
                        });
                    }, true);
                },
                PageChangeHandler: function (pageNum) {
                    $scope.currentPage = pageNum;
                    $scope.Methods.GetAllOrders(pageNum);
                }
            };

            console.log("Order Controller");
            $scope.Methods.Init();
        }]);

});