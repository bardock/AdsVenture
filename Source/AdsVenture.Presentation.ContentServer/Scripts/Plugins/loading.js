﻿(function ($, window) {

    /* LOADING CLASS DEFINITION
     * ======================== */

    var Loading = function (selector) {

        return {
            elem: $(selector),

            count: 0,

            delay: 600,
            
            show: function () {
                this.count++;

                this.elem.delay(this.delay).fadeIn();

                return this;
            },

            hide: function () {
                this.count--;

                if (this.count == 0)
                    this.elem.stop().clearQueue().hide(0);

                return this;
            }
        };

    };
    
    /* DEFAULT INSTANCE AND MODULE DEFINITION
     * ====================================== */

    var loading = new Loading(".modal_loading");

    window.Plugins = window.Plugins || {};
    window.Plugins.Loading = Loading;
    window.Plugins.Loading.default = loading;


    /* Override jquery ajax function
     * ============================= */

    var _ajax = $.ajax;

    $.ajax = function( url, options ) {

        // If url is an object, simulate pre-1.5 signature
        if ( typeof url === "object" ) {
            options = url;
            url = undefined;
        }

        // Force options to be an object
        options = options || {};

        // Create the final options object
        options = jQuery.ajaxSetup({}, options);

        if (options.showLoading !== false) {

            // Use specified or default loading object
            var loading = options.loading || Loading.default;

            // Show/hide loading using specified functions in options
            var _showLoading = null;
            var _hideLoading = null;
            if (typeof (options.showLoading) == "function")
                _showLoading = options.showLoading;
            if(typeof (options.hideLoading) == "function")
                _hideLoading = options.hideLoading;
            // Or, if none was specified, use loading object functions
            if (!_showLoading && !_hideLoading) {
                _showLoading = loading.show;
                _hideLoading = loading.hide;
            }

            // Override beforeSend handler
            
            if (_showLoading) {
                var _beforeSend = options.beforeSend || function () {};

                options.beforeSend = function (jqXHR, settings) {
                    var ret = _beforeSend(jqXHR, settings);

                    if (ret !== false) {
                        _showLoading.call(loading);
                        return ret;
                    }
                    return false;
                }
            }

            // Override complete handler

            if (_hideLoading) {
                var _complete = options.complete || [];

                // Force _complete to be an array of handlers
                if (!$.isArray(_complete))
                    _complete = [_complete];

                _complete.push(function (jqXHR, textStatus) {
                    _hideLoading.call(loading);
                });

                options.complete = _complete;
            }
        }

        // Call original function
        return _ajax.call(this, url, options)
    }

    
    /* Show loading on form submit
     * =========================== */

    $(document).ready(function () {

        // We want this submit handler to be executed last.
        // Because handlers execution order is hard to manipulate, 
        // we try to bind it after any other binding (wait doc ready and then 1 second).
        setTimeout(function () {

            $(document).on("submit", "form", function (e) {
                $form = $(this);
                if (!e.isDefaultPrevented() && $form.data("loading") !== false) {
                    // Use a new loading object if a selector was specified
                    var loading = typeof($form.data("loading")) == "string"
                                    ? new Loading($form.data("loading"))
                                    : window.Plugins.Loading.default;

                    loading.show();
                }
            });

        }, 1000);

    });


})(jQuery, window);