/* ===================================================
 * jquery.focusForm.js
 * ===================================================
 */

(function ($) {

    /* PLUGIN DEFINITION
     * ================= */

    $.fn.focusForm = function (selector) {
        var $container = $(this).first();

        var selector = selector
            || $container.data("focusform")
            || $.fn.focusForm.defaultFocusserSelector;

        $.fn.focusForm.focusElem(
            $container.find(selector).filter(selector).first()
        );
    }

    $.fn.focusForm.focusElem = function ($elem) {
        $elem.focus();
    }

    $.fn.focusForm.defaultFocusserSelector = ":input:not(:hidden):not(:disabled)";
    $.fn.focusForm.dataApiInitSelector = "[data-focusform]";

    /* DATA-API INITIALIZATION
     * ======================= */

    $(document).ready(function () {
        var selector = $.fn.focusForm.dataApiInitSelector;
        $(selector).focusForm();
    });            

})(jQuery);