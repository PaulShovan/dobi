define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('dobiController', ['$scope', 'apiConstant', 'httpService', '$state', '$stateParams', 'toastr',
        function ($scope, apiConstant, httpService, $state, $stateParams, toastr) {
            "use strict";

            $scope.Data = {
                Dobi: {
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
                    }
                },

                GetDobiById: function() {
                    httpService.get(apiConstant.getAllDobi + "?skip=" + skip, function (dobi) {
                        $timeout(function () {
                            $scope.Data.Dobies = dobi.Data.DobiList;
                            $scope.Data.TotalDobies = dobi.Data.TotalDobi;
                            $scope.Data.ShowingFrom = skip + 1;
                            $scope.Data.ShowingTo = skip + dobi.Data.DobiList.length;
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
                        httpService.postMultipart(apiConstant.dobi, { Files: files }, $scope.Data.Dobi, "New Dobi Added Successfully", function (response) {
                            if (response.status === 200) {
                                toastr.success(response.Message, "Success!");
                                $scope.httpLoading = false;
                                $state.go('dobimanage');
                            }
                        });
                    }
                    
                }

            };

            console.log("Add Dobi Controller");
            $scope.Methods.Init();
        }]);

});