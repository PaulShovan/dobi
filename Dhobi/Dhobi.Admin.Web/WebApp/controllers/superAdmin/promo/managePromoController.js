define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('managePromoController', ['$scope', 'apiConstant', 'httpService', '$state', 'toastr',
        function ($scope, apiConstant, httpService, $state, toastr) {
            "use strict";

            $scope.Data = {
                
            };

            $scope.Methods = {
                Init: function () {

                }
            };

            console.log("Manage Promo Controller");
            $scope.Methods.Init();
        }]);
});