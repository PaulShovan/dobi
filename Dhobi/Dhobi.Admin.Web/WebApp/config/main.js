require.config({
    baseUrl: "/WebApp",
    waitSeconds: 30,
    // alias libraries paths
    paths: {
        //Third Party Libraries
        'jquery': "lib/jquery",
        'angular': "lib/angular.min",
        'angularAMD': "lib/angularAMD.min",
        'angular-route': "lib/angular-route.min",
        'angular-ui-router': "lib/angular-ui-router.min",
        'angular-resource': "lib/angular-resource.min",
        'angular-cookies': "lib/angular-cookies.min",
        'angular-ngStorage': "lib/ngStorage.min",
        'ng-messages': "lib/ngMessages",

        'ngload': "lib/ngload.min",
        'underscore': "lib/underscore",

        'bootstrap': "lib/bootstrap",
        
        //Necessary Plugins
        'bootstrap-ui': 'lib/ui-bootstrap-tpls-1.3.3.min',
        'angular-loading-bar': 'lib/plugins/angular-loading-bar/loading-bar.min',
        'angular-toastr': 'lib/angular-toastr.tpls',
        'angular-confirm': 'lib/angular-confirm.min',
        'moment': 'lib/plugins/datepicker/moment',
        'angular-moment': 'lib/angular-moment',
        'angular-datepicker': 'lib/plugins/datepicker/angular-datepicker',
        'ng-file-upload': 'lib/plugins/ng-file-upload/ng-file-upload',
        'i-check': 'lib/plugins/iCheck/icheck.min',

        
        //Theme
        
        //Constants
        'role-constant': 'constants/roleConstant',

        //Directives
        'common-directives': 'directives/commonDirective',
        'http-loader': 'directives/loader/loader',
        'top-menu': 'directives/topMenu/topMenu',
        'dir-pagination': 'directives/pager/dirPagination',

        //Factories
        'http-service': 'factories/http/httpFactory',
        'modal-factory': 'factories/modal/modalFactory',
        'utility': 'factories/utility',


        //Services
        'api-constant': 'services/apiConstant',

        //Controllers
        'dobi-add': 'controllers/superAdmin/dobi/dobiAddController'
    },

    // Add angular modules that does not support AMD out of the box, put it in a shim
    shim: {
        'jquery': {
            exports: "$"
        },
        'angular': {
            deps: ["jquery"],
            exports: "angular"
        },
        'angular-route': ["angular"],
        'angularAMD': ["angular"],
        'angular-resource': ["angular"],
        'angular-ui-router': ["angular"],
        'angular-cookies': ["angular"],
        'ng-messages': ["angular"],
        'underscore': {
            exports: "_"
        },
        'bootstrap': { deps: ["jquery"] },

        //Necessary Plugins
        'bootstrap-ui': { deps: ['angular'] },
        'angular-loading-bar': { deps: ['angular'] },
        'angular-toastr': { deps: ['angular', 'bootstrap'] },
        'angular-confirm': { deps: ['angular'] },
        'angular-moment': { deps: ['angular', 'moment'] },
        'angular-datepicker': { deps: ['angular', 'moment'] },
        'ng-file-upload': { deps: ['angular'] }
    },

    // kick start application
    deps: ["app"]
})