define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('managerAddController', ['$scope', 'apiConstant', 'httpService', '$state', 'toastr',
        function ($scope, apiConstant, httpService, $state, toastr) {
            "use strict";

            $scope.Data = {
                Manager: {
                    Name: "",
                    Phone: "",
                    Email: "",
                    Address: "",
                    EmergencyContactNumber: "",
                    PassportNumber: "",
                    IcNumber: "",
                    DrivingLicense: "",
                    Age: "",
                    Sex: "male",
                    Salary: "",
                    Photo: [],
                    SelectedRole: "admin"
                },
                FileErrorMsg: null
            };

            $scope.Methods = {
                Init: function () {

                },
                AddNewManager: function (files) {
                    $scope.Data.Manager.Photo = files;
                    httpService.postMultipart(apiConstant.addNewManager, { Files: files }, $scope.Data.Manager, "New Manager Added Successfully", function (response) {
                        if (response.status === 200) {
                            toastr.success(response.Message, "Success!");
                            //$state.go('dobimanage');
                        }
                    });
                }
            };

            console.log("Add Manager Controller");
            $scope.Methods.Init();
        }]);
});