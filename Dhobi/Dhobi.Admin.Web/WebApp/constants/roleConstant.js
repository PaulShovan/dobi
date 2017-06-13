define(['app'], function (app) {
    app = app || angular.module("roleConstantModule", []);
    app.constant('roleConstant', (function () {
        'use strict';

        return {
            Superadmin: "Superadmin",
            Admin: "Admin",
            Manager: 'Manager'
        }

    })());
});