define(['app', 'jquery', 'bootstrap'], function (app, $) {
    app = app || angular.module("commonDirectiveModule", []);

    app.directive('validateEmail', validateEmail);
    app.directive('fallbackSrc', fallbackSrc);
    app.directive('autofocus', autofocus);
    app.directive('iCheck', iCheck);
    app.directive("limitTo", limitTo);
    app.directive("onlyDigits", onlyDigits);
    app.directive('ngMaxvalue', ngMaxvalue); // need to test
    app.directive('ngMinvalue', ngMinvalue); // need to test
    app.directive('twoPrecision', twoPrecision);
    app.directive('ageLimit', ageLimit);
    app.directive('icheckCheckbox', icheckCheckbox);


    /*** validateEmail - Directive for Validating email. Usage [ as attr ] [ validate-email ] will validate email type input. */
    function validateEmail() {
        "use strict";
        var EMAIL_REGEXP = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/;

        return {
            require: 'ngModel',
            restrict: '',
            link: function (scope, elm, attrs, ctrl) {
                // only apply the validator if ngModel is present and Angular has added the email validator
                if (ctrl && ctrl.$validators.email) {

                    // this will overwrite the default Angular email validator
                    ctrl.$validators.email = function (modelValue) {
                        return ctrl.$isEmpty(modelValue) || EMAIL_REGEXP.test(modelValue);
                    };
                }
            }
        };
    }

    /*** fallbackSrc - Directive for optional image when ng-src is not available or null */
    function fallbackSrc() {
        var fallbackSrc = {
            restrict: 'A',
            link: function postLink(scope, element, attrs) {
                if (_.isEmpty(attrs.ngSrc)) {
                    element.attr('src', attrs.fallbackSrc);
                }
                element.bind('error', function () {
                    element.attr('src', attrs.fallbackSrc);
                });
            }
        }
        return fallbackSrc;
    };

    /*** autofocus - Directive for autofocus input. Usage: [as attr]  [ autofocus ] will focus on page load on that input. */
    function autofocus($timeout) {
        "use strict";
        return {
            restrict: 'A',
            link: function ($scope, $element) {
                $timeout(function () {
                    $element[0].focus();
                });
            }
        }
    };

    /*** iCheck - Directive to work iCheck with angularjs */
    function iCheck($timeout, $parse) {
        return {
            require: 'ngModel',
            link: function ($scope, element, $attrs, ngModel) {
                return $timeout(function () {
                    var value = $attrs.value;
                    var $element = $(element);
                    // Instantiate the iCheck control.                            
                    $element.iCheck({
                        checkboxClass: 'icheckbox_square-green',
                        radioClass: 'iradio_square-green',
                        increaseArea: '20%'
                    });
                    // If the model changes, update the iCheck control.
                    $scope.$watch($attrs.ngModel, function (newValue) {
                        $element.iCheck('update');
                    });
                    // If the iCheck control changes, update the model.
                    $element.on('ifChanged', function (event) {
                        if ($element.attr('type') === 'radio' && $attrs.ngModel) {
                            $scope.$apply(function () {
                                ngModel.$setViewValue(value);
                            });
                        }
                    });
                });
            }
        };
    };


    function icheckCheckbox ($timeout, $parse) {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attr, ngModel) {
                $timeout(function () {
                    var value = attr.value;

                    function update(checked) {
                        if (attr.type === 'radio') {
                            ngModel.$setViewValue(value);
                        } else {
                            ngModel.$setViewValue(checked);
                        }
                    }

                    $(element).iCheck({
                        checkboxClass: attr.checkboxClass || 'icheckbox_square-green',
                        radioClass: attr.radioClass || 'iradio_square-green'
                    }).on('ifChanged', function (e) {
                        scope.$apply(function () {
                            update(e.target.checked);
                        });
                    });

                    scope.$watch(attr.ngChecked, function (checked) {
                        if (typeof checked === 'undefined') checked = !!ngModel.$viewValue;
                        update(checked)
                    }, true);

                    scope.$watch(attr.ngModel, function (model) {
                        $(element).iCheck('update');
                    }, true);

                })
            }
        }
    };



    /*** limit-to - Directive for input limit. Usage on html: [as attr]  [ limit-to="5" ] will take only 5 characters on that input. */
    function limitTo() {
        "use strict";
        return {
            restrict: "A",
            link: function (scope, elem, attrs) {
                var limit = parseInt(attrs.limitTo);
                angular.element(elem).on("keypress", function (e) {
                    if (this.value.length === limit) e.preventDefault();
                });
            }
        }
    }

    /*** only-digits - Directive for just input the digits, no special characters. Usage on html: [as attr]  [ only-digits ] will take just digits as input. */
    function onlyDigits() {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, element, attr, ctrl) {
                function inputValue(val) {
                    if (val) {
                        var digits = (val + '').replace(/[^0-9]/g, '');

                        if (digits !== val) {
                            ctrl.$setViewValue(digits);
                            ctrl.$render();
                        }
                        return parseInt(digits, 10);
                    }
                    return undefined;
                }
                ctrl.$parsers.push(inputValue);
            }
        };
    };

    /*** ng-minvalue - Directive for checking the minimum value input. Usage on html: [as attr] [ ng-minvalue="5" ] will take 5 as minimum value ***/
    function ngMinvalue() {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elem, attr, ctrl) {
                scope.$watch(attr.ngMin, function () {
                    ctrl.$setViewValue(ctrl.$viewValue);
                });
                var minValidator = function (value) {
                    var min = scope.$eval(attr.ngMin) || 0;
                    if (!isEmpty(value) && value < min) {
                        ctrl.$setValidity('ngMin', false);
                        return undefined;
                    } else {
                        ctrl.$setValidity('ngMin', true);
                        return value;
                    }
                };

                ctrl.$parsers.push(minValidator);
                ctrl.$formatters.push(minValidator);
            }
        };
    };

    /*** ng-maxvalue - Directive for checking the maximum value input. Usage on html: [as attr] [ ng-maxvalue="100" ] will take 100 as maximum value ***/
    function ngMaxvalue() {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elem, attr, ctrl) {
                scope.$watch(attr.ngMax, function () {
                    ctrl.$setViewValue(ctrl.$viewValue);
                });
                var maxValidator = function (value) {
                    var max = scope.$eval(attr.ngMax) || Infinity;
                    if (!isEmpty(value) && value > max) {
                        ctrl.$setValidity('ngMax', false);
                        return undefined;
                    } else {
                        ctrl.$setValidity('ngMax', true);
                        return value;
                    }
                };

                ctrl.$parsers.push(maxValidator);
                ctrl.$formatters.push(maxValidator);
            }
        };
    };

    /*** two-precision - Directive for checking 2 precision floating point number. Usage on html: [as attrr] [ two-precision ] will validate 2 precision floating point numbher ***/
    function twoPrecision() {
        return {
            require: '?ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                if (!ngModelCtrl) {
                    return;
                }

                ngModelCtrl.$parsers.push(function (val) {
                    if (angular.isUndefined(val)) {
                        var val = '';
                    }

                    var clean = val.replace(/[^-0-9\.]/g, '');
                    var negativeCheck = clean.split('-');
                    var decimalCheck = clean.split('.');
                    if (!angular.isUndefined(negativeCheck[1])) {
                        negativeCheck[1] = negativeCheck[1].slice(0, negativeCheck[1].length);
                        clean = negativeCheck[0] + '-' + negativeCheck[1];
                        if (negativeCheck[0].length > 0) {
                            clean = negativeCheck[0];
                        }

                    }

                    if (!angular.isUndefined(decimalCheck[1])) {
                        decimalCheck[1] = decimalCheck[1].slice(0, 2);
                        clean = decimalCheck[0] + '.' + decimalCheck[1];
                    }

                    if (val !== clean) {
                        ngModelCtrl.$setViewValue(clean);
                        ngModelCtrl.$render();
                    }
                    return clean;
                });

                element.bind('keypress', function (event) {
                    if (event.keyCode === 32) {
                        event.preventDefault();
                    }
                });
            }
        };
    };


    function ageLimit() {
        /*jslint unparam: true*/
        return {
            link: function ($scope, $element, $attrs, ngModelCtrl) {
                var settings = {
                    minAge: 18,
                    underAgeMsg: 'You need to be at least 18 years old.',
                    title: null,
                    pattern: null
                },
                    copyUserSettings = function (attrs, settings) {
                        var property;
                        for (property in settings) {
                            if (settings.hasOwnProperty(property) && attrs.hasOwnProperty(property)) {
                                settings[property] = attrs[property];
                            }
                        }
                    },
                    calculateAge = function (birthday, settings) { // birthday is a string 
                        var regex, today, birthDate, age, month;
                        if (!settings.pattern) {
                            regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/; //if no pattern provided
                            if (!regex.test(birthday)) {
                                return -1;
                            }
                        }
                        today = new Date();
                        birthDate = new Date(birthday);
                        age = today.getFullYear() - birthDate.getFullYear();
                        month = today.getMonth() - birthDate.getMonth();
                        if (month < 0 || (month === 0 && today.getDate() < birthDate.getDate())) {
                            age = age - 1;
                        }

                        if (isNaN(age)) {
                            return -1;  //if age is invalid
                        }

                        return age;
                    };

                copyUserSettings($attrs, settings);
                $element.on('focusout keydown', function (e) {
                    var age = calculateAge(e.target.value, settings);
                    e.target.setCustomValidity(""); //clear validation               
                    if (-1 === age && settings.title) {
                        e.target.setCustomValidity(settings.title);
                        return;
                    }
                    if (age < parseInt(settings.minAge, 10)) {
                        e.target.setCustomValidity(settings.underAgeMsg);
                    }
                });
            }
        };
    };


});