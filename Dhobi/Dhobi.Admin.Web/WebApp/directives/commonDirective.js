define(['app', 'jquery', 'bootstrap'], function (app, $) {
    app = app || angular.module("commonDirectiveModule", []);
    
    app.directive('validateEmail', validateEmail);
    app.directive('fallbackSrc', fallbackSrc);
    app.directive('iCheck', iCheck);

    /**
     * validateEmail - Directive for Validating email
     */
    function validateEmail() {
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
        }
    };

    function fallbackSrc() {
        var fallbackSrc = {
            link: function postLink(scope, iElement, iAttrs) {
                iElement.bind('error', function() {
                    angular.element(this).attr("src", iAttrs.fallbackSrc);
                });
            }
        }
        return fallbackSrc;
    };

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

});