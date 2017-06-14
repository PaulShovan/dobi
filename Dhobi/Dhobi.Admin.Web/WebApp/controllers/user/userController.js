define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('userController', ['$scope', 'apiConstant', 'httpService', '$state', 'toastr',
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
                    Roles: "Admin"
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
                        $scope.Data.User.Phone = "006" + $scope.Data.User.Phone;
                        httpService.postMultipart(apiConstant.user, { Files: files }, $scope.Data.User, "New User Added Successfully", function (response) {
                            if (response.status === 200) {
                                toastr.success(response.Message, "Success!");
                                $state.go('usermanage');
                            }
                        });
                    }
                }
            };

            console.log("Add User Controller");
            $scope.Methods.Init();
        }]);
});