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
            },
            RemoveMalaysiaCC: function (phoneNumber) { // CC means Country Code
                return phoneNumber.toString().replace(/\D/g, '').substr(3, 10);
            },
            AddMalaysiaCC: function(phoneNumber) {
                var phone = phoneNumber.toString().replace(/\D/g, '');
                var phoneWithExt = "006" + phone;
                return phoneWithExt;
            },
            AddParamWithPhotoUrl: function (list) {
                _.each(list, function (item) {
                    item.Photo = item.Photo + "?" + (+new Date);
                });
                return list;
            },

            ConvertImgToDataURLviaCanvas: function (url, callback, outputFormat) {
                var img = new Image();
                img.crossOrigin = 'Anonymous';
                img.onload = function () {
                    var canvas = document.createElement('CANVAS');
                    var ctx = canvas.getContext('2d');
                    var dataURL;
                    canvas.height = this.height;
                    canvas.width = this.width;
                    ctx.drawImage(this, 0, 0);
                    dataURL = canvas.toDataURL(outputFormat);
                    callback(dataURL);
                    canvas = null;
                };
                img.src = url;
            },
            ConvertDataURLtoFile: function (dataurl, filename) {
                var arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
                    bstr = atob(arr[1]), n = bstr.length, u8arr = new Uint8Array(n);
                while (n--) {
                    u8arr[n] = bstr.charCodeAt(n);
                }
                return new File([u8arr], filename, { type: mime });
            }

        };
    });

});