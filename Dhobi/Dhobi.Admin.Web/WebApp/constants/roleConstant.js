define(['app'], function (app) {
    app = app || angular.module("roleConstantModule", []);
    app.constant('roleConstant', (function () {
        'use strict';

        return {
            SuperAdmin: "SuperAdmin",
            Admin: "Admin",
            Client: 'Client',
            BroadCaster: 'BroadCaster'
        }

    })());
});