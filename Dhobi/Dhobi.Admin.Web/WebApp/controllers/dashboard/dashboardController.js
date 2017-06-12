define(['app', 'underscore'], function (app, _) {
    app.controller('dashboardController', ['$scope', '$localStorage', function ($scope, $localStorage) {
        "use strict";

        $scope.Data = {
            UserInfo: {
                Name: "",
                Role: ""
            },
            ViewName: ""
        };

        $scope.Methods = {
            Init: function () {
                $scope.Data.UserInfo = $localStorage.UserInfo;
                $scope.Data.ViewName = $localStorage.ViewName;
            }
        };

        console.log("Dashboard Controller");
        $scope.Methods.Init();
    }]);

});