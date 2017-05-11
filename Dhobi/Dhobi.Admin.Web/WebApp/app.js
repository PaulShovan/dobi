define(['angularAMD', 'angular-route', 'angular-ui-router', 'angular-resource', 'angular-cookies', 'angular-ngStorage', 'ng-messages', 'bootstrap-ui', 'angular-toastr', 'angular-confirm', 'modal-factory', 'role-constant', 'api-constant', 'http-service', 'top-menu', /*'footer', */'common-directives', 'angular-loading-bar', 'angular-datepicker', 'ng-file-upload', 'utility', 'angular-moment'], function (angularAMD) {
    var app = angular.module("ngreq-app", ['ui.router', 'ngResource', 'ngCookies', 'ngStorage', 'ngMessages', 'ui.bootstrap', 'toastr', 'angular-confirm', 'modalPropertiesModule', 'roleConstantModule', 'apiConstantModule', 'httpServiceModule', 'topMenuModule', /*'footerModule',*/ 'commonDirectiveModule', 'angular-loading-bar', 'datePicker', 'ngFileUpload', 'appUtilityModule', 'angularMoment']);

    app.config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
    }]);
    
    app.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
        "use strict";

        $stateProvider.state('viewswich', angularAMD.route({
            url: '/viewswich',
            //templateUrl: '/WebApp/views/dashboard/dashboard.html',
            controllerUrl: 'controllers/viewSwitchController',
            controller: 'viewSwichController',
            authorization: {
                role: [app.RoleName.SuperAdmin, app.RoleName.Admin, app.RoleName.Manager]
            }
        }));

        app.registerSuperAdminRoute($stateProvider);
        app.registerAdminRoute($stateProvider);
        app.registerManagerRoute($stateProvider);

        $urlRouterProvider
          .otherwise('/viewswich');
    }]);

    app.config(function ($httpProvider) {
        // Activate Loader on every request
        $httpProvider.interceptors.push(function ($q, $rootScope, $cookieStore) {
            return {
                'request': function (config) {
                    var accessToken = $cookieStore.get('accessToken');
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

    app.run(function ($rootScope, $location, $cookieStore, $localStorage, modalFactory, toastr, $state) {
        $rootScope.$on('$stateChangeSuccess', function (event, toState) {

            $rootScope.CURRENTSTATE = toState.name;
            var token = $cookieStore.get('accessToken');
            if (!token || token.length < 1) {
                event.preventDefault();
                $location.path('/login');
            } else {
                var role = $localStorage.UserInfo.Role;
                if (role === app.RoleName.SuperAdmin) {
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

    app.registerSuperAdminRoute = function (stateProvider) {
        stateProvider
            .state('dashboard', angularAMD.route({
                url: '/dashboard',
                templateUrl: '/WebApp/views/dashboard/dashboard.html',
                controllerUrl: 'controllers/dashboard/dashboardController',
                controller: 'dashboardController',
                authorization: {
                    role: [app.RoleName.SuperAdmin]
                }
            }))
            .state('dobiadd', angularAMD.route({
                url: '/dobi/add',
                templateUrl: '/WebApp/views/dobi/dobiAdd.html',
                controllerUrl: 'controllers/dobi/dobiAddController',
                controller: 'dobiAddController',
                authorization: {
                    role: [app.RoleName.SuperAdmin]
                }
            }))
            .state('dobimanage', angularAMD.route({
                url: '/dobi/manage',
                templateUrl: '/WebApp/views/dobi/dobiManage.html',
                controllerUrl: 'controllers/dobi/dobiManageController',
                controller: 'dobiManageController',
                authorization: {
                    role: [app.RoleName.SuperAdmin]
                }
            }))
            .state('manageradd', angularAMD.route({
                url: '/manager/add',
                templateUrl: '/WebApp/views/superAdmin/manager/managerAdd.html',
                controllerUrl: 'controllers/superAdmin/manager/managerAddController',
                controller: 'managerAddController',
                authorization: {
                    role: [app.RoleName.SuperAdmin]
                }
            }))
            .state('managepromo', angularAMD.route({
                url: '/promo/manage',
                templateUrl: '/WebApp/views/promo/managePromo.html',
                controllerUrl: 'controllers/promo/managePromoController',
                controller: 'managePromoController',
                authorization: {
                    role: [app.RoleName.SuperAdmin]
                }
            }))
            .state('orders', angularAMD.route({
                url: '/orders',
                templateUrl: '/WebApp/views/orders/order.html',
                controllerUrl: 'controllers/orders/orderController',
                controller: 'orderController',
                authorization: {
                    role: [app.RoleName.SuperAdmin]
                }
            }))
        ;
    }
    app.registerAdminRoute = function (stateProvider) {
        stateProvider
            .state('performances', angularAMD.route({
                url: '/performances',
                templateUrl: '/WebApp/views/orchestra/performances/performance.html',
                controllerUrl: 'controllers/orchestra/performances/performanceController',
                controller: 'performanceController',
                authorization: {
                    role: [app.RoleName.SuperAdmin, app.RoleName.Admin]
                }
            }))
        ;
    }
    app.registerManagerRoute = function(stateProvider) {
        
    }

    app.RoleName = {
        SuperAdmin: 'SuperAdmin',
        Admin: 'Admin',
        Manager: 'Manager'
    }
    $("#initialLoader").remove();

    // Bootstrap Angular when DOM is ready
    return angularAMD.bootstrap(app);
});
