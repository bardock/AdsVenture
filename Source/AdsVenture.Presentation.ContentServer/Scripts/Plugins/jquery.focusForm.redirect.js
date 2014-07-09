/* ===================================================
 * jquery.focusForm.redirect.js
 * ===================================================
 * 
 * When a specified element receives focus, redirect focus to another element.
 * 
 * ===================================================
 */

(function ($, focusForm) {

    $(document).on("focusin", "[data-focusform-redirect]", function (e) {

        if ($(e.target).is("select"))
            return;

        // Find parent redirect container or use current focused element
        var $elem = $(this)
            .closest("[data-focusform-redirect]")
            .andSelf()
            .filter("[data-focusform-redirect]");

        var targetSelector = $elem.data("focusform-redirect");

        if(targetSelector)
            $(targetSelector).focusForm();
    })

})(jQuery);