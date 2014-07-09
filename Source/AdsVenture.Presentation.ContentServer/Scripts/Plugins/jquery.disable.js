(function ($) {

    jQuery.fn.disable = function (disabled) {

        var disabled = (disabled == true || typeof disabled == "undefined");

        return this.each(function () {

            var prevDisabled = $(this).is(":disabled");

            $(this).attr("disabled", disabled)
                   .toggleClass("disabled", disabled)
                   .closest(".input-prepend")
                   .toggleClass("disabled", disabled);

            if (prevDisabled != disabled)
                $(this)
                    .trigger("DOMAttrModified") // Fix for firefox event, equivalent to IE's propertychange
                    .trigger("custom.disabled", [this, disabled]);

        });

    }
})(jQuery);