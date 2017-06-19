define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('managePromoController', ['$scope', 'apiConstant', 'httpService', '$state', 'toastr', 'moment', '$rootScope',
        function ($scope, apiConstant, httpService, $state, toastr, moment, $rootScope) {
            "use strict";


            $rootScope.httpLoading = false;

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
                    Navigations: []
                },
                MinStartDate: moment(),
                MinEndDate: moment(),
                Offers: [],
                ClickedIndex: null
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
                AssignIndex: function (index) {
                    $scope.Data.ClickedIndex = index;
                },
                OnStartDateSelected: function () {
                    //@todo NEED TO FIX.
                    //$scope.Data.Offer.Navigations[$scope.Data.ClickedIndex].MinEndDate = $scope.Data.Offer.Navigations[$scope.Data.ClickedIndex].StartDate;
                    $scope.Data.Offer.Navigations[$scope.Data.ClickedIndex].EndDate = $scope.Data.Offer.Navigations[$scope.Data.ClickedIndex].StartDate;
                },
                AddOrUpdatePromo: function () {
                    var api = apiConstant.promo;
                    var message = "Promo is Created";

                    _.each($scope.Data.Offer.Navigations, function (navigation) {
                        navigation.StartDate = moment(navigation.StartDate).valueOf();
                        navigation.EndDate = moment(navigation.EndDate).valueOf();
                    });

                    $rootScope.httpLoading = true;
                    var listPromo = $scope.Data.Offer.Navigations;
                    httpService.post(api, listPromo, message, function (response) {
                        $rootScope.httpLoading = false;
                    });
                },
                GetPromoOffers: function () {
                    httpService.get(apiConstant.promo, function (offer) {
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