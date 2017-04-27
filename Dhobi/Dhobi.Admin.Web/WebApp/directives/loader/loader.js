define(['app'], function (app) {
    app = app || angular.module("loadingModule", []);
    app.directive('loading', ['$http', function ($http) {
        "use strict";
        return {
            restrict: 'E',
            templateUrl: '/WebApp/directives/loader/loader.html',
            link: function (scope, elm, attrs) {
                scope.isLoading = function () {
                    return $http.pendingRequests.length > 0;
                };

                scope.$watch(scope.isLoading, function (v) {
                    if (v) {
                        elm.show();
                    } else {
                        elm.hide();
                    }
                });
            }
        };
    }]);
});