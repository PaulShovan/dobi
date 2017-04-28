define(['app'], function (app) {
    app = app || angular.module("apiConstantModule", []);
    app.constant('apiConstant', (function () {
        'use strict';

        var version = "v1";
        var host = window.dhobiUrlConfig.baseUrl + "/";
        var baseUrl = host + "api/" + version + "/";

        //var dobiBase = baseUrl + "Manage/";

        return {
            getAllDobi: baseUrl + "dobi",
            addNewDobi: baseUrl + "dobi"


            //login: host + "token",
            //switchAccount: accountBase + "Switch",

            //approveOrchestraClient: accountBase + "OrchestraRequestApprove",
            //denyOrchestraClient: accountBase + "OrchestraRequestDenied",

            //getAllOrchestra: orchestraBase + "GetAll",
            //activeOrchestra: orchestraBase + "Active",
            //suspendedOrchestra: orchestraBase + "Suspended",
            //requestedOrchestra: orchestraBase + "Requested",
        }

    })());
});