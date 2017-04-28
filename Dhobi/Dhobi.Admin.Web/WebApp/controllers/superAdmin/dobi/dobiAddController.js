define(['app', 'underscore'], function (app, _) {
    app.controller('dobiAddController', ['$scope', function ($scope) {
        "use strict";

        $scope.Data = {
            Dobi: {
                FullName: "",
                PhoneNumber: "",
                Email: "",
                Address: "",
                EmergencyContactNo: "",
                PassportNo: "",
                ICNo: "",
                DrivingLicense: "",
                Age: "",
                Sex: "",
                Salary: ""
            }
        };

        $scope.Methods = {
            Init: function () {
                
            }
        };

        console.log("Add Dobi Controller");
        $scope.Methods.Init();
    }]);

});