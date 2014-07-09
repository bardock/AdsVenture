/* ===================================================
* thousandsSeparator.js v1.0.0
* ===================================================
* @author fsanchez
*
* This plugin shows thousands separators in disabled text inputs
* and removes thousands separators when input is enabled.
*
* Dependencies:
* ------------
* >> jQuery Globalize :: https://github.com/jquery/globalize
*    You must initialize the culture in this way:
*      Globalize.culture("es");
*
* ========================================================== */
/* PUBLIC CLASS DEFINITION
* ======================= */
var Plugins;
(function (Plugins) {
    var ThousandsSeparator = (function () {
        function ThousandsSeparator(input) {
            var _this = this;
            this.$input = $(input);

            $(document).on("custom.disabled", this.$input.selector, function () {
                _this.handleDisabledChanged();
            });

            this.handleDisabledChanged();
        }
        ThousandsSeparator.getThousandsSeparator = function () {
            return Globalize.culture().numberFormat[","];
        };

        ThousandsSeparator.getDecimalsSeparator = function () {
            return Globalize.culture().numberFormat["."];
        };

        ThousandsSeparator.prototype.handleDisabledChanged = function () {
            if (this.$input.is(":disabled"))
                this.add();
            else
                this.remove();
        };

        ThousandsSeparator.prototype.add = function () {
            var val = this.$input.val();
            if (val == "")
                return;

            // Calculate the number of decimal places in specified value
            var decimalPlaces = val.length - val.indexOf(ThousandsSeparator.getDecimalsSeparator()) - 1;
            decimalPlaces = decimalPlaces == val.length ? 0 : decimalPlaces;

            this.$input.val(Globalize.format(Globalize.parseFloat(val), "n" + decimalPlaces));
        };

        ThousandsSeparator.prototype.remove = function () {
            var val = this.$input.val();
            if (val == "")
                return;

            var replacePattern = new RegExp("[" + ThousandsSeparator.getThousandsSeparator() + "]", "gi");

            this.$input.val(val.replace(replacePattern, ""));
        };
        return ThousandsSeparator;
    })();
    Plugins.ThousandsSeparator = ThousandsSeparator;
})(Plugins || (Plugins = {}));

(function ($, window) {
    /* PLUGIN DEFINITION
    * ================= */
    $.fn.thousandsSeparator = function () {
        if (!Globalize) {
            console.log("ThousandsSeparator: jQuery Globalize is required");
            return;
        }

        return this.each(function () {
            var instance = $(this).data("thousands-separator");
            if (!instance)
                $(this).data("thousands-separator", instance = new Plugins.ThousandsSeparator($(this)));
        });
    };

    /* DATA-API INITIALIZATION
    * ======================= */
    $.fn.thousandsSeparator.dataApi_init = function () {
        $("[data-thousands-separator-apply]").thousandsSeparator();
    };
    $(document).ready($.fn.thousandsSeparator.dataApi_init);
})(jQuery, window);
//# sourceMappingURL=thousandsSeparator.js.map
