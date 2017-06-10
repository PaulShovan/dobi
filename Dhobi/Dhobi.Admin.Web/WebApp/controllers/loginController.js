var app = angular.module("app", ['ngCookies', 'ngStorage', 'ngMessages', 'toastr', 'accountDirectiveModule', 'angular-ladda']);
app.config(function(laddaProvider) {
    laddaProvider.setOption({ /* optional */
        style: 'expand-right',
        spinnerSize: 30,
        spinnerColor: '#ffffff'
    });
});

app.controller("loginCtrl", ['$scope', '$http', '$cookieStore', '$localStorage', 'toastr',
    function ($scope, $http, $cookieStore, $localStorage, toastr) {
        var loginUrl = window.dhobiUrlConfig.baseUrl + '/api/v1/manager/login';

        $scope.Data = {
            UserName: '',
            Password: ''
        },
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
                            toastr.warning(result.Message);
                            $scope.loginLoading = false;
                            return;
                        }
                        else if (result.Data && result.Data.Token) {
                            $cookieStore.put('accessToken', result.Data.Token);
                            var userInfo = {
                                Name: result.Data.Name,
                                Role: result.Data.Role
                            }
                            $localStorage.UserInfo = userInfo;
                            $localStorage.ViewName = userInfo.Role;

                            $scope.loginLoading = false;
                            window.location.href = '/DobiAdmin';
                        } else if (result.ResponseStatus) {
                            $scope.errorMsg = result.Message;
                            toastr.error(result.Message, "Error!");
                            $scope.loginLoading = false;
                        }
                    })
                    .error(function (result, httpstatus) {
                        toastr.error(result.Message, "Error!");
                        $scope.loginLoading = false;
                    });
            }
        }
    }]);