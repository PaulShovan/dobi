define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('managePromoController', ['$scope', 'apiConstant', 'httpService', '$state', 'toastr', 'moment',
        function ($scope, apiConstant, httpService, $state, toastr, moment) {
            "use strict";

            var newOffer = function () {
                return {
                    Navigations: []
                };
            };

            $scope.Data = {
                Offer: {
                    Text: "",
                    StartDate: moment(),
                    EndDate: moment(),
                    Amount: 0
                },
                MinDate: moment(),
                Navigations: []
            };

            $scope.Methods = {
                Init: function () {
                    $scope.Methods.AddNewOffer($scope.Data.Navigations);
                },
                AddNewOffer: function (offers) {
                    offers.push(new newOffer());
                },
                RemoveOffer: function (offers, index) {
                    offers.splice(index, 1);
                },
            };

            console.log("Manage Promo Controller");
            $scope.Methods.Init();
        }]);
});