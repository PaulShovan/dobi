var app = angular.module("app", ['ngCookies', 'ngStorage', 'ngMessages', 'toastr', 'accountDirectiveModule']);
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

                $http.post(loginUrl, userCredentials, { headers: { 'Content-Type': 'application/json' } })
                    .success(function (result) {
                        if (!result.ResponseStatus) { return; }
                        if (result.Data.Token) {
                            $cookieStore.put('Token', result.Data.Token);
                            var userInfo = {
                                Name: result.Data.Name
                            }
                            $localStorage.UserInfo = userInfo;
                            window.location.href = '/Admin';
                        } else if (result.ResponseStatus) {
                            $scope.errorMsg = result.Message;
                            toastr.error(result.Message, "Error!");
                        }
                    })
                    .error(function (result, httpstatus) {
                        toastr.error(result.Message, "Error!");
                    });
            }
        }
    }]);