/* ===================================================
 * jquery.focusForm.select2.js
 * ===================================================
 * 
 * select2 support for focusForm plugin
 * 
 * ===================================================
 */

(function ($, focusForm) {

    var focusForm = $.fn.focusForm;
    var _super = $.extend({}, focusForm);

    /* Override focusElem function in order to focus select2 element */

    focusForm.focusElem = function ($elem) {
        if ($elem.is(".select2-focusser"))
            $elem.closest(".select2-container").data("select2").open()
        else
            _super.focusElem.call(this, $elem);
    }

    /* Add select2-focusser to selector*/

    focusForm.defaultFocusserSelector += ",.select2-focusser:not(:hidden):not(:disabled)";

})(jQuery);