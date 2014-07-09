/*
 *
 * Use jquery Globalize in validations.
 *
 */
(function ($) {

    // Override jquery validator
    $.extend($.validator.prototype, {
        parse: function (value) {
            return typeof (value) == "string"
                ? Globalize.parseFloat(value)
                : value;
        }
    });
    var _min = $.validator.methods.min;
    var _max = $.validator.methods.max;
    var _range = $.validator.methods.range;
    $.extend($.validator.methods, {
        number: function (value, element) {
            return this.optional(element)
                || !isNaN(this.parse(value));
        },
        min: function (value, element, param) {
            return _min.call(this, this.parse(value), element, param);
        },
        max: function (value, element, param) {
            return _max.call(this, this.parse(value), element, param);
        },
        range: function (value, element, param) {
            return _range.call(this, this.parse(value), element, param);
        },
        date: function (value, element) {
            return this.optional(element) || Globalize.parseDate(value) != null;
        }
    });

})(jQuery);