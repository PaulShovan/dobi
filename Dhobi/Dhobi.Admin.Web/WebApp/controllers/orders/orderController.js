define(['app', 'underscore', 'dir-pagination', 'i-check'], function (app, _) {
    app.controller('orderController', ['$scope', 'apiConstant', 'httpService', '$timeout', 'moment',
        function ($scope, apiConstant, httpService, $timeout, moment) {
            "use strict";

            $scope.currentPage = 1;
            $scope.pageSize = 10;

            $scope.Data = {
                Orders: [],
                TotalOrders: 0,
                ShowingFrom: 0,
                ShowingTo: 0,
                SelectedOrders: [],
                IsSelectedOrder: false,

                SelectedDate: moment(),
                MinStartDate: moment(),
                MinEndDate: moment(),
            };

            $scope.Methods = {
                Init: function () {
                    $scope.Methods.GetAllOrders($scope.currentPage, $scope.Data.SelectedDate);
                },
                GetAllOrders: function (pageNumber, selectedDate) {
                    var skip = (pageNumber - 1) * $scope.pageSize;
                    var filterDate = moment(selectedDate).format('L');

                    httpService.get(apiConstant.orders + "?date=" + filterDate + "&&skip=" + skip + "&&limit=10", function (orders) {
                        $timeout(function () {
                            $scope.Data.Orders = orders.Data;
                            $scope.Data.TotalOrders = orders.Data.length; //@todo: TotalOrders should be come from api not the length
                            $scope.Data.ShowingFrom = skip + 1;
                            $scope.Data.ShowingTo = skip + orders.Data.length;
                        });
                    }, true);
                },
                PageChangeHandler: function (pageNum) {
                    $scope.currentPage = pageNum;
                    $scope.Methods.GetAllOrders(pageNum);
                },
                OnDateSelected: function() {
                    $scope.Methods.GetAllOrders($scope.currentPage, $scope.Data.SelectedDate);
                }
            };

            console.log("Order Controller");
            $scope.Methods.Init();
        }]);

});