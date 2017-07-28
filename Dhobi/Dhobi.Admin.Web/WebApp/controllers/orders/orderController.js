define(['app', 'underscore', 'dir-pagination', 'i-check'], function (app, _) {
    app.controller('orderController', ['$scope', 'apiConstant', 'httpService', '$timeout', 'moment', '$rootScope', 'toastr',
        function ($scope, apiConstant, httpService, $timeout, moment, $rootScope, toastr) {
            "use strict";

            $scope.currentPage = 1;
            $scope.pageSize = 10;

            $scope.Data = {
                Orders: [],
                TotalOrders: 0,
                ShowingFrom: 0,
                ShowingTo: 0,
                SelectedDate: moment(),
                SelectAll: false,
                SelectedOrders: null,
                OrderStatusName: ""
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
                    var filterDate = moment(selectedDate).format('L');
                    //var filterDate = "7/21/2017";

                    httpService.get(apiConstant.orders + "?date=" + filterDate + "&&skip=" + skip + "&&limit=10", function (orders) {
                        $timeout(function () {
                            $scope.Methods.OrderStatusNameFromEnum(orders.Data);
                            $scope.Data.Orders = orders.Data;
                            $scope.Methods.OrderStatusNameFromEnum($scope.Data.Orders);
                            
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
                
                UpdateOrderStatus: function (status, statusName) {
                    var selectedOrderModel = [];
                    if (!$scope.Data.SelectedOrders || $scope.Data.SelectedOrders == null) {
                        toastr.error('Please Select at least one order.', 'Error!');
                        return;
                    }
                    _.each($scope.Data.SelectedOrders, function (key, value) {
                        if (key === true) {
                            selectedOrderModel.push(value);
                        }
                    });
                    var orderStatusUpdateModel = {
                        Orders: selectedOrderModel,
                        UpdatedStatus: status
                    }
                    var message = "Order Move to " + statusName;
                    $rootScope.httpLoading = true;
                    httpService.post(apiConstant.updateOrderStatus, orderStatusUpdateModel, message, function (response) {
                        $rootScope.httpLoading = false;
                        $scope.Methods.GetAllOrders($scope.currentPage, $scope.Data.SelectedDate);
                        $scope.Data.SelectedOrders = null;
                    });
                },

                // Private
                OrderStatusNameFromEnum: function(orders) {
                    _.each($scope.Data.Orders, function (order) {
                        _.each($scope.Data.UpdateOrderStatus, function (key, status) {
                            if (order.OrderStatus === key)
                                order.OrderStatus = status;
                        });
                    });
                }

            };

            console.log("Order Controller");
            $scope.Methods.Init();
        }]);

});