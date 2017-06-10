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
                    $scope.Methods.AddNewOffer($scope.Data.Offer.Navigations);
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

                    _.each($scope.Data.Offer.Navigations, function(navigation) {
                        navigation.StartDate = moment().valueOf(navigation.StartDate);
                        navigation.EndDate = moment().valueOf(navigation.EndDate);
                    });

                    var listPromo = $scope.Data.Offer.Navigations;
                    httpService.post(api, listPromo, message, function (response) {
                        console.log(response);
                    });
                },
                GetPromoOffers: function() {
                    httpService.get(apiConstant.getAllPromo, function (offer) {
                        $timeout(function () {
                            $scope.Data.Offers = offer.Data.OfferList;
                            $scope.Data.TotalUsers = offer.Data.TotalOffer;
                        });
                    }, true);
                }
            };

            console.log("Manage Promo Controller");
            $scope.Methods.Init();
        }]);
});