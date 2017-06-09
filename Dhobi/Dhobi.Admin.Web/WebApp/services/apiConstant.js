define(['app'], function (app) {
    app = app || angular.module("apiConstantModule", []);
    app.constant('apiConstant', (function () {
        'use strict';

        var version = "v1";
        var host = window.dhobiUrlConfig.baseUrl + "/";
        var baseUrl = host + "api/" + version + "/";
        
        return {
            getAllDobi: baseUrl + "dobi",
            addNewDobi: baseUrl + "dobi",

            addNewUser: baseUrl + "manager",
            getAllUser: baseUrl + "manager",

            addPromo: baseUrl + "promo",
            updatePromo: baseUrl + "promo"
        }
    })());
});