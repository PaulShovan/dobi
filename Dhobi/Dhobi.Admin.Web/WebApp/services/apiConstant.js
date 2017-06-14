define(['app'], function (app) {
    app = app || angular.module("apiConstantModule", []);
    app.constant('apiConstant', (function () {
        'use strict';

        var version = "v1";
        var host = window.dhobiUrlConfig.baseUrl + "/";
        var baseUrl = host + "api/" + version + "/";
        
        return {
            dobi: baseUrl + "dobi",
            user: baseUrl + "manager",
            promo: baseUrl + "promo"
        }
    })());
});