define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('dobiController', ['$scope', 'apiConstant', 'httpService', '$state', '$stateParams', 'toastr', 'appUtility', 'moment',
        function ($scope, apiConstant, httpService, $state, $stateParams, toastr, appUtility, moment) {
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
                    Photo: []
                },
                FileErrorMsg: null,
                SaveOrUpdateBtn: "Save",
                TemporaryPhoneNumber: ""
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
                        $scope.Data.TemporaryPhoneNumber = $scope.Data.Dobi.Phone;
                    }, true);
                },
                AddOrUpdateDobi: function () {
                    if ($scope.Data.Dobi.Photo == null && ($scope.files === null || $scope.files === "")) {
                        $scope.Data.FileErrorMsg = "Please Upload a file";
                        return;
                    }
                    if ($scope.files && $scope.files.length > 0) {
                        $scope.Data.Dobi.Photo = $scope.files;
                    }
                    $scope.Data.Dobi.Phone = appUtility.AddMalaysiaCC($scope.Data.TemporaryPhoneNumber);
                    $scope.httpLoading = true;

                    var api = $scope.Data.Dobi.DobiId ? apiConstant.updateDobi : apiConstant.dobi;
                    var message = $stateParams.id ? "Dobi Updated Successfully." : "New Dobi Added Successfully";
                    httpService.postMultipart(api, { Files: $scope.files }, $scope.Data.Dobi, message, function (response) {
                        $scope.Data.Dobi.Phone = appUtility.RemoveMalaysiaCC($scope.Data.Dobi.Phone);
                        if (response.status === 200) {
                            toastr.success(response.Message, "Success!");
                            $scope.httpLoading = false;
                            $state.go('dobimanage');
                        }
                    }, false);
                }
            };

            console.log("Add Dobi Controller");
            $scope.Methods.Init();
        }]);

});