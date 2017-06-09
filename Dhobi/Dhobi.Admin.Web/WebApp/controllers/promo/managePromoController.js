define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('managePromoController', ['$scope', 'apiConstant', 'httpService', '$state', 'toastr', 'moment',
        function ($scope, apiConstant, httpService, $state, toastr, moment) {
            "use strict";

            var newoffer = function () {
                return {
                    Text: "",
                    StartDate: moment(),
                    EndDate: moment(),
                    Amount: 0,
                    Navigations: []
                };
            };

            $scope.Data = {
                Offer: {
                    Navigations:[]
                },
                MinDate: moment(),
                Offers: []
            };

            $scope.Methods = {
                Init: function () {
                    //$scope.Methods.AddNewOffer(new newoffer());
                },
                AddNewOffer: function (offers) {
                    offers.push(new newoffer());
                },
                RemoveOffer: function (offers, index) {
                    offers.splice(index, 1);
                },
                AddOrUpdatePromo: function () {
                    var api = apiConstant.addPromo;
                    var message = "Promo is Created";

                    var listPromo = $scope.Data.Offer.Navigations;
                    //var obj = { Text: $scope.Data.ConfigId, name: $scope.Data.WebCrawling.Name, config: $scope.Data.WebCrawling };
                    httpService.post(api, listPromo, message, function (response) {
                        console.log(response);
                    });
                },
            };

            console.log("Manage Promo Controller");
            $scope.Methods.Init();
        }]);
});