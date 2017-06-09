﻿define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('userAddController', ['$scope', 'apiConstant', 'httpService', '$state', 'toastr',
        function ($scope, apiConstant, httpService, $state, toastr) {
            "use strict";

            $scope.Data = {
                User: {
                    Name: "",
                    UserName: "",
                    Password: "",
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
                    Roles: "admin"
                },
                FileErrorMsg: null
            };

            $scope.Methods = {
                Init: function () {

                },

                AddNewUser: function (files) {
                    if (!files || files.length <= 0) {
                        $scope.Data.FileErrorMsg = "Please Upload a file";
                        return;
                    } else {
                        httpService.postMultipart(apiConstant.addNewUser, { Files: files }, $scope.Data.User, "New User Added Successfully", function (response) {
                            if (response.status === 200) {
                                toastr.success(response.Message, "Success!");
                                //$state.go('usermanage');
                            }
                        });
                    }
                }
            };

            console.log("Add User Controller");
            $scope.Methods.Init();
        }]);
});