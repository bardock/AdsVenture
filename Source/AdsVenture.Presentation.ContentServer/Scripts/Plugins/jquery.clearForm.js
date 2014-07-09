/* ===================================================
 * jquery.clearForm.js
 * ===================================================
 * @author fsanchez
 *
 * Inspired in jQuery Form Plugin version.
 * 
 * Clears the form data using "data-default-value" attribute defined in element or
 * takes the following actions:
 *  - input text fields will have their 'value' property set to the empty string
 *  - select elements will have their 'selectedIndex' property set to empty string
 *  - checkbox and radio inputs will have their 'checked' property set to false
 *  - inputs of type submit, button, reset, and hidden will *not* be effected
 *  - button elements will *not* be effected
 * 
 * ========================================================== */

(function ($) {

    $.fn.clearForm = function (includeHidden) {
        return this.each(function () {
            $(":input:not(:button):not([type=submit]):not([type=reset])", this).clearField(includeHidden);
        });
    };

    $.fn.clearForm.includeHidden = false;

    $.fn.clearField = $.fn.clearInputs = function (includeHidden) {
        // use specified includeHidden or default if its empty
        includeHidden = typeof includeHidden == "undefined" || includeHidden == null
            ? $.fn.clearForm.includeHidden
            : includeHidden;

        return this.each(function () {

            var $field = $(this),
                cleared = true;

            // returns default value defined in element or a specified default value
            var defaultValue = function (_default) {
                var _fieldDefault = $field.data("default-value");
                return typeof _fieldDefault == "undefined" || _fieldDefault == null
                    ? _default
                    : _fieldDefault;
            }

            if ($field.is(":checkbox") || $field.is(":radio")) {
                $field.attr("checked", defaultValue() === true);
            }
            else if ($field.is("select")) {
                var firstOptionValue = $field.find("option:first").val();
                $field.val(defaultValue(firstOptionValue));
            }
            else if ($field.is(":file")) {
                if (/MSIE/.test(navigator.userAgent)) {
                    $field.replaceWith($(this).clone());
                } else {
                    $field.val("");
                }
            }
            else if (!$field.is("[type=hidden]")
                || includeHidden === true
                || typeof includeHidden == 'string' && $(this).is(includeHidden)) {
                $field.val(defaultValue(""));
            }
            else cleared = false;

            if (cleared)
                $field.trigger("cleared", [this, defaultValue]);
        });
    };

    // Clicks on elements with clearform data attribute will execute clearForm in specified or parent form 
    $(document).on("click", "[data-clearform]", function (e) {
        $button = $(this);
        $form = $button.data("clearform")
            ? $($button.data("clearform"))
            : $button.closest("form").clearForm();
        $form.clearForm();
        e.preventDefault();
    });

})(jQuery);