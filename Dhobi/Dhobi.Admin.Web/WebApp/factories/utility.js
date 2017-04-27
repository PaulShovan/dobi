define(['app'], function (app) {
    app = app || angular.module("appUtilityModule", []);
    app.factory('appUtility', function () {
        "use strict";

        return {
            Clone: function (data) {
                return JSON.parse(JSON.stringify(data));
            },
            Copy: function (source, destination) {
                for (var key in source) {
                    if (source.hasOwnProperty(key)) {
                        destination[key] = source[key];
                    }
                }
            },
            IsNumber: function (n) {
                return !isNaN(parseFloat(n)) && isFinite(n);
            },
            IsNullOrEmpty: function (n) {
                return (n + '').length > 0;
            },
            IsvalidIp: function (value) {
                if (!value) {
                    return false;
                }
                var isValid = value !== '0.0.0.0' && value !== '255.255.255.255' && value.match(/\b^(\d|[1-9]\d|1\d\d|2([0-4]\d|5[0-5]))\.(\d|[1-9]\d|1\d\d|2([0-4]\d|5[0-5]))\.(\d|[1-9]\d|1\d\d|2([0-4]\d|5[0-5]))\.(\d|[1-9]\d|1\d\d|2([0-4]\d|5[0-5]))$\b/);
                return !!isValid;
            },
            ConvertEnterToBr: function (data) {
                if (!data) {
                    return false;
                }
                var convertedData = data.replace(/\r?\n/g, '<br />');
                return convertedData;
            }
        };
    });

});