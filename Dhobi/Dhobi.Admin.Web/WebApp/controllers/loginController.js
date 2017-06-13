var app = angular.module("app", ['ngCookies', 'ngStorage', 'ngMessages', 'accountDirectiveModule', 'angular-ladda']);
app.config(function(laddaProvider) {
    laddaProvider.setOption({ /* optional */
        style: 'expand-right',
        spinnerSize: 30,
        spinnerColor: '#ffffff'
    });
});

app.controller("loginCtrl", ['$scope', '$http', '$localStorage',
    function ($scope, $http, $localStorage) {
        var loginUrl = window.dhobiUrlConfig.baseUrl + '/api/v1/manager/login';

        $scope.Data = {
            UserName: "",
            Password: ""
        },
        $scope.Error = {
            UserNameRequired: "Username is required.",
            PasswordRequired: "Password is required.",
            PasswordMin: "Minimum 6 character required.",
            PasswordMax: "Maximum 64 character allowed.",
            FromServer: ""
        }
        
        $scope.Methods = {
            Login: function () {
                var userCredentials = {
                    UserName: $scope.Data.UserName,
                    Password: $scope.Data.Password
                };
                $scope.loginLoading = true;

                $http.post(loginUrl, userCredentials, { headers: { 'Content-Type': 'application/json' } })
                    .success(function (result) {
                        if (!result.ResponseStatus) {
                            $scope.Error.FromServer = result.Message;
                            $scope.loginLoading = false;
                            return;
                        }
                        else if (result.Data && result.Data.Token) {
                            $localStorage.accessToken = result.Data.Token;
                            var userInfo = {
                                Name: result.Data.Name,
                                Role: result.Data.Role
                            }
                            $localStorage.UserInfo = userInfo;
                            $localStorage.ViewName = userInfo.Role;

                            $scope.loginLoading = false;
                            window.location.href = '/DobiAdmin';
                        }
                    })
                    .error(function (result, httpstatus) {
                        $scope.Error.FromServer = result.Message;
                        $scope.loginLoading = false;
                    });
            },
            FlashErrors: function ($event) {
                $scope.Error.FromServer = "";
            }
        }
    }]);