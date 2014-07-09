/*
 * Allow use dot when other decimal separator is used (for using numeric pad) *
 */
(function ($) {
    var _keypress = $.fn.numeric.keypress;
    var _keyup = $.fn.numeric.keyup;
    $.extend($.fn.numeric, {
        defaults: {
            decimal: "."
        },
        keypress: function (e) {
            // get decimal character
            var decimal = $(this).data("numeric.decimal");
            // get the key that was pressed
            var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;


            // If we are not using dot as decimal separator
            // and user pressed dot (for example in numeric pad)
            // process this character as a valid decimal separator
            if (decimal != "." && key == 46) {
                $(this).data("numeric.decimal.prev", decimal);
                $(this).data("numeric.decimal", ".");
            }

            _keypress.call(this, e);
        },
        keyup: function (e) {
            var decimalPrev = $(this).data("numeric.decimal.prev");

            if (decimalPrev) {
                // Replace dot for decimal separator
                if (this.value.indexOf(".") != -1)
                    this.value = this.value.replace(".", decimalPrev);

                // Restore decimal separator
                $(this).data("numeric.decimal.prev", null);
                $(this).data("numeric.decimal", decimalPrev);
            }

            _keyup.call(this, e);
        },
        dataApi_init: function (context) {
            $('[data-numeric]', context || document).each(function(i, elem) {
                // Init only once
                if ($(elem).data("numeric.callback"))
                    return;
                var decimal = $(elem).data("numeric-decimal") !== false;
                if (decimal)
                    decimal = $.fn.numeric.defaults.decimal;
                var negative = $(elem).data("numeric-negative") !== false;
                $(elem).numeric({
                    decimal: decimal,
                    negative: negative
                });
            });
        }
    });
})(jQuery);