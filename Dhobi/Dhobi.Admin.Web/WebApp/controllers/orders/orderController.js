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
                SelectedDate: moment(),
                SelectedOrders: []
            };

            $scope.Data.UpdateOrderStatus = {
                New: 1,
                Acknowledged: 2,
                Confirmed: 3,
                Cancelled: 4,
                PickedUp: 5,
                InProgress: 6,
                Processed: 7,
                Deliverable: 8,
                OnTheWay: 9,
                Delivered: 10,
                Paid: 11
            };

            $scope.Methods = {
                Init: function () {
                    $scope.Methods.GetAllOrders($scope.currentPage, $scope.Data.SelectedDate);
                },
                GetAllOrders: function (pageNumber, selectedDate) {
                    var skip = (pageNumber - 1) * $scope.pageSize;
                    //var filterDate = moment(selectedDate).format('L');
                    var filterDate = "7/21/2017";

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
                OnDateSelected: function () {
                    $timeout(function () {
                        $scope.Methods.GetAllOrders($scope.currentPage, $scope.Data.SelectedDate);
                    });
                },
                OnCheckboxClicked: function (serviceId, index) {
                    console.log(index);
                    _.each($scope.Data.SelectedOrders, function (order) {
                        if (index && order.ServiceId !== serviceId) {
                            $scope.Data.SelectedOrders.push(serviceId);
                        }
                    });
                    console.log($scope.Data.SelectedOrders);
                },
                UpdateOrderStatus: function (status) {
                    var updatedOrderStatus = "";

                    //httpService.post(apiConstant.updateOrderStatus, listPromo, message, function (response) {
                    //    $rootScope.httpLoading = false;
                    //});
                }
            };

            console.log("Order Controller");
            $scope.Methods.Init();
        }]);

});