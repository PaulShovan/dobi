define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('userController', ['$scope', 'apiConstant', 'httpService', '$state', 'toastr', 'appUtility', '$stateParams', '$rootScope',
        function ($scope, apiConstant, httpService, $state, toastr, appUtility, $stateParams, $rootScope) {
            "use strict";

            $rootScope.files = null;
            $scope.httpProcessing = false;
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
                TemporaryPhoneNumber: "",
                TemporaryEmergencyContactNumber: "",
                IsEditView: false
            };

            $scope.Methods = {
                Init: function () {
                    if ($stateParams.id) {
                        $scope.Data.SaveOrUpdateBtn = "Update";
                        $scope.Data.IsEditView = true;
                        $scope.Methods.GetUserById($stateParams.id);
                    }
                },
                GetUserById: function (id) {
                    httpService.get(apiConstant.user + "?userId=" + id, function (user) {
                        $scope.Data.User = user.Data;
                        $scope.Data.User.Roles = user.Data.Roles[0];
                        $scope.Data.User.Phone = appUtility.RemoveMalaysiaCC(user.Data.Phone);
                        $scope.Data.User.EmergencyContactNumber = appUtility.RemoveMalaysiaCC(user.Data.EmergencyContactNumber);
                        $scope.Data.TemporaryPhoneNumber = $scope.Data.User.Phone;
                        $scope.Data.TemporaryEmergencyContactNumber = $scope.Data.User.EmergencyContactNumber;
                    }, true);
                },
                AddOrUpdateUser: function () {
                    if (($scope.Data.User.Photo == null || $scope.Data.User.Photo.length <= 0) && ($scope.files === null || $scope.files === "")) {
                        $scope.Data.FileErrorMsg = "Please Upload a photo";
                        return;
                    }
                    if ($scope.files && $scope.files.length > 0) {
                        $scope.Data.User.Photo = $scope.files;
                    }
                    $scope.Data.User.Password = $scope.Data.IsEditView ? "" : $scope.Data.User.Password;
                    $scope.Data.User.Phone = appUtility.AddMalaysiaCC($scope.Data.TemporaryPhoneNumber);
                    $scope.Data.User.EmergencyContactNumber = appUtility.AddMalaysiaCC($scope.Data.TemporaryEmergencyContactNumber);
                    $rootScope.httpLoading = true;
                    $scope.httpProcessing = true;
                    
                    var api = $scope.Data.User.UserId ? apiConstant.updateUser : apiConstant.user;
                    var message = $scope.Data.User.UserId ? "User Updated Successfully." : "New User Added Successfully";
                    httpService.postMultipart(api, { Files: $scope.files }, $scope.Data.User, message, function (response) {
                        $scope.Data.User.Phone = appUtility.RemoveMalaysiaCC($scope.Data.User.Phone);
                        if (response.status === 200) {
                            toastr.success(response.Message, "Success!");
                            $state.go('usermanage');
                        }
                        $rootScope.httpLoading = false;
                    }, false);
                }
            };

            $scope.Methods.Init();
        }]);
});