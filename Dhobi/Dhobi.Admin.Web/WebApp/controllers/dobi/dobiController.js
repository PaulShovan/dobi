define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('dobiController', ['$scope', 'apiConstant', 'httpService', '$state', '$stateParams', 'toastr',
        function ($scope, apiConstant, httpService, $state, $stateParams, toastr) {
            "use strict";

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
                SaveOrUpdateBtn: "Save"
            };

            $scope.Methods = {
                Init: function () {
                    if ($stateParams.id) {
                        $scope.Data.SaveOrUpdateBtn = "Update";
                        $scope.Methods.GetDobiById($stateParams.id);
                    }
                },

                GetDobiById: function(id) {
                    httpService.get(apiConstant.dobiById + "/" + id, function (dobi) {
                        $timeout(function () {
                            $scope.Data.Dobi = dobi.Data.Dobi;
                        });
                    }, true);
                },
                AddOrUpdateDobi: function (files) {
                    if (!files || files.length <= 0) {
                        $scope.Data.FileErrorMsg = "Please Upload a file";
                        return;
                    } else {
                        $scope.Data.Dobi.Photo = files;
                        $scope.Data.Dobi.Phone = "006" + $scope.Data.Dobi.Phone;
                        $scope.httpLoading = true;

                        var message = $stateParams.id ? "Dobi Updated Successfully." : "New Dobi Added Successfully";
                        httpService.postMultipart(apiConstant.dobi, { Files: files }, $scope.Data.Dobi, message, function (response) {
                            if (response.status === 200) {
                                toastr.success(response.Message, "Success!");
                                $scope.httpLoading = false;
                                $state.go('dobimanage');
                            }
                        }, false);
                    }
                    
                }

            };

            console.log("Add Dobi Controller");
            $scope.Methods.Init();
        }]);

});