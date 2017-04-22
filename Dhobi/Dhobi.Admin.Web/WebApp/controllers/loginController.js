var app = angular.module("app", ['ngCookies', 'ngStorage', 'ngMessages', 'toastr', 'accountDirectiveModule']);
app.controller("loginCtrl", ['$scope', '$http', '$cookieStore', '$localStorage', 'toastr', '$rootScope',
    function ($scope, $http, $cookieStore, $localStorage, toastr, $rootScope) {
        var loginUrl = window.dhobiUrlConfig.baseUrl + '/token';

        $scope.Data = {
            Username: '',
            Password: ''
        },
        $scope.Methods = {
            Login: function () {
                //var l = $('.login-button').ladda();
                //l.ladda('start');
                var userCredentials = {
                    grant_type: 'password',
                    username: $scope.Data.Username,
                    password: $scope.Data.Password
                };

                $http.post(loginUrl, $.param(userCredentials), { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                    .success(function (result) {
                        if (!result) { return; }
                        if (result.Email && result.access_token) {
                            $cookieStore.put('accessToken', result.access_token);
                            var userInfo = {
                                FirstName: result.FirstName,
                                LastName: result.LastName,
                                Email: result.Email,
                                Role: result.Role
                            }
                            $localStorage.UserInfo = userInfo;
                            $localStorage.IsOpusView = result.IsOpusView === "True";

                            //l.ladda('stop');

                            window.location.href = '/Admin';
                        } else if (result.error_description) {
                            $scope.errorMsg = result.error_description;
                            toastr.error(result.error_description, "Error!");
                        }
                        //l.ladda('stop');
                    })
                    .error(function (result, httpstatus) {
                        toastr.error(result.error_description, "Error!");
                        //l.ladda('stop');
                    });
            }
        }
    }]);