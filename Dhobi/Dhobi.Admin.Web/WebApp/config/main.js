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
        
        //Theme
        
        //Directives
        
        //Factories

        //Services

        //Controllers
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
        'bootstrap': { deps: ["jquery"] }
    },

    // kick start application
    deps: ["app"]
})