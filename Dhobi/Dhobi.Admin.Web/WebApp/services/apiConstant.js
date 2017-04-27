define(['app'], function (app) {
    app = app || angular.module("apiConstantModule", []);
    app.constant('apiConstant', (function () {
        'use strict';

        var host = window.dhobiUrlConfig.baseUrl + '/';
        var baseUrl = host + "api/";

        var accountBase = baseUrl + "Account/";

        return {
            login: host + "token",
            switchAccount: accountBase + "Switch",

            approveOrchestraClient: accountBase + "OrchestraRequestApprove",
            denyOrchestraClient: accountBase + "OrchestraRequestDenied",

            //getAllOrchestra: orchestraBase + "GetAll",
            //activeOrchestra: orchestraBase + "Active",
            //suspendedOrchestra: orchestraBase + "Suspended",
            //requestedOrchestra: orchestraBase + "Requested",
        }

    })());
});