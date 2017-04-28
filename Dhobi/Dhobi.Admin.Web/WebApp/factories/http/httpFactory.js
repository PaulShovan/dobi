define(['app', 'http-loader', 'ng-file-upload'], function (app) {
    app = app || angular.module("httpServiceModule", ['cfp.loadingBar', 'ngFileUpload']);
    app.factory('httpService', ['$http', 'toastr', '$cookieStore', '$localStorage', '$location', 'apiConstant', 'cfpLoadingBar', 'Upload', '$timeout',
        function ($http, toastr, $cookieStore, $localStorage, $location, apiConstant, cfpLoadingBar, Upload, $timeout) {
        "use strict";
        return {
            get: function (api, onResponse) {
                cfpLoadingBar.start();
                $http.get(api)
                    .success(function (result) {
                        if (!result) {
                            toastr.error(result, 'Error!');
                        } else {
                            onResponse(result);
                        }
                        cfpLoadingBar.complete();
                    })
                    .error(function (result, status) {
                        if (status === 401) {
                            toastr.error('Permission required', 'Error!');
                            $location.path('/login');
                        } else {
                            toastr.error(result, 'Error!');
                        }
                        cfpLoadingBar.complete();
                    });
            },

            post: function (api, data, successMessage, onResponse, isPopupHide) {
                cfpLoadingBar.start();
                $http.post(api, data)
                   .success(function (result, status) {
                       if (!result && status !== 200) {
                           if (result.Errors && result.Errors.length > 0) {
                               result.Error = result.Errors.join('\n');
                           }
                           toastr.error(result.Error, 'Error!');
                       }
                       else {
                           if (onResponse) {
                               onResponse(result);
                           }
                           if (!isPopupHide) {
                               toastr.success(successMessage, 'Success!');
                           }
                       }
                       cfpLoadingBar.complete();
                   })
                   .error(function (result, status) {
                       if (status === 401) {
                           toastr.error('Permission required', 'Error!');
                           $location.path('/login');
                       } else {
                           toastr.error(result.Message, 'Error!');
                       }
                       cfpLoadingBar.complete();
                   });
            },

            switchAccount: function (orchestaId, onSuccess) {
                var params = null;
                if (orchestaId) {
                    params = {
                        OrchestraId: orchestaId
                    }
                }
                $http.post(apiConstant.switchAccount, params)
                    .success(function (res) {
                        $http.post(apiConstant.login, $.param({ grant_type: 'password', account_switch: res }), { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
                            .success(function (result) {
                                $cookieStore.put('accessToken', result.access_token);
                                if (onSuccess) {
                                    onSuccess(result);
                                }
                            })
                            .error(function (result, status) {
                                if (status === 401) {
                                    toastr.error('Permission required', 'Error!');
                                    window.location.href = '/Admin';
                                } else {
                                    toastr.error(result, 'Error!');
                                }
                                cfpLoadingBar.complete();
                            });
                    })
                    .error(function (result, status) {
                        if (status === 401) {
                            toastr.error('Permission required', 'Error!');
                            window.location.href = '/Admin';
                        } else {
                            toastr.error(result, 'Error!');
                        }
                        cfpLoadingBar.complete();
                    });
            },

            postMultipart: function (api, files, data, successMessage, onResponse) {
                cfpLoadingBar.start();
                //Upload.setDefaults({ ngfMinSize: 20000, ngfMaxSize: 20000000 });
                var obj = { Files: files};

                for (var key in data) {
                    if (data.hasOwnProperty(key)) {
                        obj[key] = data[key];
                    }
                }

                files.upload = Upload.upload({
                    url: api,
                    data: obj
                });
                files.upload.then(function (response) {
                    $timeout(function () {
                        files.result = response.data;
                        if (response.status === 200) {
                            if (onResponse) {
                                onResponse(response);
                            }
                            //toastr.success("Image Uploaded Successfully.", 'Success');
                            cfpLoadingBar.complete();
                        }
                    });
                }, function (response) {
                    if (response.status !== 200) {
                        response.errorMsg = response.status + ': ' + response.data;
                        //toastr.error("Failed Uploading Image.", 'Error!');
                        cfpLoadingBar.complete();
                    }
                }, function (evt) {
                    files.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                });
            }
        };
    }]);
});