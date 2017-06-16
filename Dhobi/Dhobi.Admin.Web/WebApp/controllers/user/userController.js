define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('userController', ['$scope', 'apiConstant', 'httpService', '$state', 'toastr', 'appUtility', '$stateParams',
        function ($scope, apiConstant, httpService, $state, toastr, appUtility, $stateParams) {
            "use strict";

            $scope.files = null;
            $scope.Data = {
                User: {
                    UserId: "",
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
                FileErrorMsg: null,
                SaveOrUpdateBtn: "Save",
                TemporaryPhoneNumber: ""
            };

            $scope.Methods = {
                Init: function () {
                    if ($stateParams.id) {
                        $scope.Data.SaveOrUpdateBtn = "Update";
                        $scope.Methods.GetUserById($stateParams.id);
                    }
                },
                GetUserById: function (id) {
                    httpService.get(apiConstant.user + "?userId=" + id, function (user) {
                        $scope.Data.User = user.Data;
                        $scope.Data.User.Phone = appUtility.RemoveMalaysiaCC(user.Data.Phone);
                        $scope.Data.TemporaryPhoneNumber = $scope.Data.User.Phone;
                    }, true);
                },
                AddOrUpdateUser: function () {
                    if ($scope.Data.User.Photo == null && ($scope.files === null || $scope.files === "")) {
                        $scope.Data.FileErrorMsg = "Please Upload a file";
                        return;
                    }
                    if ($scope.files && $scope.files.length > 0) {
                        $scope.Data.User.Photo = $scope.files;
                    }
                    $scope.Data.User.Phone = appUtility.AddMalaysiaCC($scope.Data.TemporaryPhoneNumber);
                    $scope.httpLoading = true;

                    var api = $scope.Data.User.UserId ? apiConstant.updateUser : apiConstant.user;
                    var message = $scope.Data.User.UserId ? "User Updated Successfully." : "New User Added Successfully";
                    httpService.postMultipart(api, { Files: $scope.files }, $scope.Data.User, message, function (response) {
                        $scope.Data.User.Phone = appUtility.RemoveMalaysiaCC($scope.Data.User.Phone);
                        if (response.status === 200) {
                            toastr.success(response.Message, "Success!");
                            $scope.httpLoading = false;
                            $state.go('usermanage');
                        }
                    }, false);
                }
            };

            $scope.Methods.Init();
        }]);
});