define(['app', 'underscore', 'dir-pagination'], function (app, _) {
    app.controller('orderController', ['$scope', 'apiConstant', 'httpService', '$timeout',
        function ($scope, apiConstant, httpService, $timeout) {
            "use strict";

            $scope.currentPage = 1;
            $scope.pageSize = 10;

            $scope.Data = {
                Orders: [],
                TotalOrders: 0,
                ShowingFrom: 0,
                ShowingTo: 0
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
                            $scope.Data.ShowingFrom = skip + 1;
                            $scope.Data.ShowingTo = skip + orders.Data.OrderList.length;
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