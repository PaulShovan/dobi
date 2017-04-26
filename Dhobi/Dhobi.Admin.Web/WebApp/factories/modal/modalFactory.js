define(['app', 'jquery', 'bootstrap'], function (app, $) {
    app = app || angular.module("modalPropertiesModule", []);

    app.factory('modalFactory', function () {
        "use strict";
        return {
            setModalMaxHeight: function(element) {
                this.$element = $(element);
                this.$content = this.$element.find('.modal-content');
                var borderWidth = this.$content.outerHeight() - this.$content.innerHeight();
                var dialogMargin = $(window).width() < 768 ? 20 : 60;
                var contentHeight = $(window).height() - (dialogMargin + borderWidth);
                var headerHeight = this.$element.find('.modal-header').outerHeight() || 0;
                var footerHeight = this.$element.find('.modal-footer').outerHeight() || 0;
                var maxHeight = contentHeight - (headerHeight + footerHeight);
                //console.log(maxHeight);
                this.$content.css({
                    'overflow': 'hidden'
                });

                this.$element
                    .find('.modal-body').css({
                        'max-height': maxHeight,
                        'overflow-y': 'auto'
                    });
            }
        }
    });
});
