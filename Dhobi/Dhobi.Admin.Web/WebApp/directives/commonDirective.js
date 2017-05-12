define(['app', 'jquery', 'bootstrap'], function (app, $) {
    app = app || angular.module("commonDirectiveModule", []);
    
    app.directive('validateEmail', validateEmail);
    app.directive('fallbackSrc', fallbackSrc);
    app.directive('iCheck', iCheck);
    app.directive("limitTo", limitTo);


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

});