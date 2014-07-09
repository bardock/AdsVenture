(function ($) {

    // Override unobstrusive validator

    $.extend($.validator.unobtrusive, {

        reinit: function (selector) {
            $(selector)
                .data('unobtrusiveValidation', null)
                .data('validator', null);
            return $.validator.unobtrusive.parse(selector);
        }

    });

})(jQuery);