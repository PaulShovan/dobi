define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('dobiController', ['$scope', 'apiConstant', 'httpService', '$state', '$stateParams', 'toastr', 'appUtility',
        function ($scope, apiConstant, httpService, $state, $stateParams, toastr, appUtility) {
            "use strict";

            $scope.files = null;
            $scope.Data = {
                Dobi: {
                    DobiId: "",
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
                    ConfirmPassword: ""
                },
                FileErrorMsg: null,
                SaveOrUpdateBtn: "Save",
                TemporaryPhoneNumber: "",
                TemporaryEmergencyNumber: ""
            };

            $scope.Methods = {
                Init: function () {
                    if ($stateParams.id) {
                        $scope.Data.SaveOrUpdateBtn = "Update";
                        $scope.Methods.GetDobiById($stateParams.id);
                    }
                },
                GetDobiById: function (id) {
                    httpService.get(apiConstant.dobi + "?dobiId=" + id, function (dobi) {
                        $scope.Data.Dobi = dobi.Data;
                        $scope.Data.Dobi.Phone = appUtility.RemoveMalaysiaCC(dobi.Data.Phone);
                        $scope.Data.Dobi.EmergencyContactNumber = appUtility.RemoveMalaysiaCC(dobi.Data.EmergencyContactNumber);
                        $scope.Data.TemporaryPhoneNumber = $scope.Data.Dobi.Phone;
                        $scope.Data.TemporaryEmergencyNumber = $scope.Data.Dobi.EmergencyContactNumber;
                    }, true);
                },
                AddOrUpdateDobi: function () {
                    if (($scope.Data.Dobi.Photo == null || $scope.Data.Dobi.Photo.length<=0) && ($scope.files === null || $scope.files === "")) {
                        $scope.Data.FileErrorMsg = "Please Upload a photo";
                        return;
                    }
                    if ($scope.files && $scope.files.length > 0) {
                        $scope.Data.Dobi.Photo = $scope.files;
                    }
                    $scope.Data.Dobi.Phone = appUtility.AddMalaysiaCC($scope.Data.TemporaryPhoneNumber);
                    $scope.Data.Dobi.EmergencyContactNumber = appUtility.AddMalaysiaCC($scope.Data.TemporaryEmergencyNumber);
                    $scope.httpLoading = true;

                    var api = $scope.Data.Dobi.DobiId ? apiConstant.updateDobi : apiConstant.dobi;
                    var message = $scope.Data.Dobi.DobiId ? "Dobi Updated Successfully." : "New Dobi Added Successfully";
                    httpService.postMultipart(api, { Files: $scope.files }, $scope.Data.Dobi, message, function (response) {
                        $scope.Data.Dobi.Phone = appUtility.RemoveMalaysiaCC($scope.Data.Dobi.Phone);
                        $scope.Data.Dobi.EmergencyContactNumber = appUtility.RemoveMalaysiaCC($scope.Data.Dobi.EmergencyContactNumber);
                        if (response.status === 200) {
                            toastr.success(response.Message, "Success!");
                            $scope.httpLoading = false;
                            $state.go('dobimanage');
                        }
                    }, false);
                }
            };

            $scope.Methods.Init();
        }]);

});