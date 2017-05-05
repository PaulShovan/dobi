define(['app', 'underscore', 'i-check'], function (app, _) {
    app.controller('dobiAddController', ['$scope', 'apiConstant', 'httpService', '$state', 'toastr',
        function ($scope, apiConstant, httpService, $state, toastr) {
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
                FileErrorMsg: null
            };

            $scope.Methods = {
                Init: function () {

                },
                AddNewDobi: function (files) {
                    if (!files || files.length <= 0) {
                        $scope.Data.FileErrorMsg = "Please Upload a file";
                        return;
                    } else {
                        $scope.Data.Dobi.Photo = files;
                        httpService.postMultipart(apiConstant.addNewDobi, { Files: files }, $scope.Data.Dobi, "New Dobi Added Successfully", function (response) {
                            if (response.status === 200) {
                                toastr.success(response.Message, "Success!");
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