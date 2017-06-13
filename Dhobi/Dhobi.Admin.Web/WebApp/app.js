define(['angularAMD', 'angular-route', 'angular-ui-router', 'angular-resource', 'angular-cookies', 'angular-ngStorage', 'ng-messages', 'bootstrap-ui',
    'angular-toastr', 'angular-confirm', 'modal-factory', 'role-constant', 'api-constant', 'http-service', 'top-menu', /*'footer', */
    'common-directives', 'angular-loading-bar', 'angular-datepicker', 'ng-file-upload', 'utility', 'angular-moment', 'angular-ladda'], function (angularAMD) {

        var app = angular.module("ngreq-app", ['ui.router', 'ngResource', 'ngCookies', 'ngStorage', 'ngMessages', 'ui.bootstrap',
            'toastr', 'angular-confirm', 'modalPropertiesModule', 'roleConstantModule', 'apiConstantModule', 'httpServiceModule', 'topMenuModule', /*'footerModule',*/
            'commonDirectiveModule', 'angular-loading-bar', 'datePicker', 'ngFileUpload', 'appUtilityModule', 'angularMoment', 'angular-ladda']);

        app.config(['cfpLoadingBarProvider', 'laddaProvider', function (cfpLoadingBarProvider, laddaProvider) {
            cfpLoadingBarProvider.includeSpinner = false;

            laddaProvider.setOption({ /* optional */
                style: 'expand-right',
                spinnerSize: 30,
                spinnerColor: '#ffffff'
            });
        }]);

        app.config(['$stateProvider', '$urlRouterProvider', '$localStorageProvider', function ($stateProvider, $urlRouterProvider, $localStorageProvider) {
            "use strict";

            app.registerCommonRoute($stateProvider);
            app.registerSuperAdminRoute($stateProvider);
            app.registerManagerRoute($stateProvider);

            var accessToken = $localStorageProvider.get('accessToken');
            var viewName = $localStorageProvider.get('ViewName');

            if (accessToken && viewName) {
                $urlRouterProvider.otherwise('/dashboard');
            } else {
                //$urlRouterProvider.otherwise('/login');
                window.location.href = '/login';
            }

        }]);

        app.config(function ($httpProvider) {
            // Activate Loader on every request
            $httpProvider.interceptors.push(function ($q, $rootScope, $localStorage) {
                return {
                    'request': function (config) {
                        var accessToken = $localStorage.accessToken;
                        config.headers.Authorization = "Bearer " + accessToken;
                        return config || $q.when(config);
                    },
                    'response': function (response) {
                        return response || $q.when(response);
                    },
                    'responseError': function (rejection) {
                        return $q.reject(rejection);
                    }
                };
            });
        });

        app.run(function ($rootScope, $location, $localStorage, modalFactory, toastr, $state) {
            $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
                $("#ui-view").html("");
                $(".page-loading").removeClass("hidden");
            });

            $rootScope.$on('$stateChangeSuccess', function (event, toState) {
                $(".page-loading").addClass("hidden");
                $rootScope.CURRENTSTATE = toState.name;
                var token = $localStorage.accessToken;
                if (!token || token.length < 1) {
                    event.preventDefault();
                    $location.path('/login');
                } else {
                    var role = $localStorage.UserInfo.Role;
                    if (role === app.RoleName.Superadmin) {
                        return;
                    }
                    if (toState.authorization.role.indexOf(role) < 0) {
                        // ToDo Show access denied Message 
                        $location.path('/login');
                    }
                }

            });

            // Modal Popup and If window resize:Modal won't break
            $('.modal').on('show.bs.modal', function () {
                $(this).show();
                modalFactory.setModalMaxHeight(this);
            });
            $(window).resize(function () {
                if ($('.modal.in').length !== 0) {
                    modalFactory.setModalMaxHeight($('.modal.in'));
                }
            });
        });


        app.registerCommonRoute = function (stateProvider) {
            stateProvider
                .state('dashboard', angularAMD.route({
                    url: '/dashboard',
                    templateUrl: '/WebApp/views/dashboard/dashboard.html',
                    controllerUrl: 'controllers/dashboard/dashboardController',
                    controller: 'dashboardController',
                    authorization: {
                        role: [app.RoleName.Superadmin, app.RoleName.Admin, app.RoleName.Manager]
                    }
                }));
        };

        app.registerSuperAdminRoute = function (stateProvider) {
            stateProvider
                .state('dobiadd', angularAMD.route({
                    url: '/dobi/add',
                    templateUrl: '/WebApp/views/dobi/dobiAdd.html',
                    controllerUrl: 'controllers/dobi/dobiAddController',
                    controller: 'dobiAddController',
                    authorization: {
                        role: [app.RoleName.Superadmin, app.RoleName.Admin]
                    }
                }))
                .state('dobimanage', angularAMD.route({
                    url: '/dobi/manage',
                    templateUrl: '/WebApp/views/dobi/dobiManage.html',
                    controllerUrl: 'controllers/dobi/dobiManageController',
                    controller: 'dobiManageController',
                    authorization: {
                        role: [app.RoleName.Superadmin, app.RoleName.Admin]
                    }
                }))
                .state('useradd', angularAMD.route({
                    url: '/user/add',
                    templateUrl: '/WebApp/views/user/userAdd.html',
                    controllerUrl: 'controllers/user/userAddController',
                    controller: 'userAddController',
                    authorization: {
                        role: [app.RoleName.Superadmin, app.RoleName.Admin]
                    }
                }))
                .state('usermanage', angularAMD.route({
                    url: '/user/manage',
                    templateUrl: '/WebApp/views/user/userManage.html',
                    controllerUrl: 'controllers/user/userManageController',
                    controller: 'userManageController',
                    authorization: {
                        role: [app.RoleName.Superadmin, app.RoleName.Admin]
                    }
                }))
                .state('managepromo', angularAMD.route({
                    url: '/promo/manage',
                    templateUrl: '/WebApp/views/promo/managePromo.html',
                    controllerUrl: 'controllers/promo/managePromoController',
                    controller: 'managePromoController',
                    authorization: {
                        role: [app.RoleName.Superadmin, app.RoleName.Admin]
                    }
                }))
            ;
        }

        app.registerManagerRoute = function (stateProvider) {
            stateProvider
                .state('orders', angularAMD.route({
                    url: '/orders',
                    templateUrl: '/WebApp/views/orders/order.html',
                    controllerUrl: 'controllers/orders/orderController',
                    controller: 'orderController',
                    authorization: {
                        role: [app.RoleName.Superadmin, app.RoleName.Admin, app.RoleName.Manager]
                    }
                }));
        }

        app.RoleName = {
            Superadmin: 'Superadmin',
            Admin: 'Admin',
            Manager: 'Manager'
        }
        $("#initialLoader").remove();

        // Bootstrap Angular when DOM is ready
        return angularAMD.bootstrap(app);
    });
