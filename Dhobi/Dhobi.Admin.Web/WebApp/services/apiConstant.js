define(['app'], function (app) {
    app = app || angular.module("apiConstantModule", []);
    app.constant('apiConstant', (function () {
        'use strict';

        var version = "v1";
        var host = window.dhobiUrlConfig.baseUrl + "/";
        var baseUrl = host + "api/" + version + "/";
        
        return {
            dobi: baseUrl + "dobi",
            updateDobi: baseUrl + "dobi/update",

            user: baseUrl + "manager",
            updateUser: baseUrl + "manager/update",

            promo: baseUrl + "promo",

            orders: baseUrl + "orders"
        }
    })());
});