define(['app', 'underscore'], function (app, _) {
    app.controller('dashboardController', ['$scope', function ($scope) {
        "use strict";

        $scope.Data = {

        };

        $scope.Methods = {
            Init: function () {

            }
        };

        console.log("Dashboard Controller");
        $scope.Methods.Init();
    }]);

});