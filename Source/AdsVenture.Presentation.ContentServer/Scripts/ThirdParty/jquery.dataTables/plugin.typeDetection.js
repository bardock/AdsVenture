(function ($, window) {

    $.fn.dataTableExt.aTypes.unshift(function (sData) {

        return globalize(sData)
            || dataValue(sData)
            || numHtml(sData);

    });

    function globalize(sData) {
        try {
            var parser = $.fn.dataTableExt.oSort["globalize-pre"];
            if (parser && parser(sData))
                return 'globalize';

            return null;
        }
        catch (err) { return null; }
    }

    function dataValue (sData) {
        try {
            if ($.fn.dataTableExt.oSort["data-value-pre"]
                    && typeof $(sData).data("value") !== 'undefined')
                return 'data-value';

            return null;
        }
        catch (err) { return null; }
    }

    function numHtml (sData) {
        if (!$.fn.dataTableExt.oSort["num-html-pre"])
            return null;

        sData = typeof sData.replace == 'function' ?
            sData.replace(/<[\s\S]*?>/g, "") : sData;
        sData = $.trim(sData);

        var sValidFirstChars = "0123456789-";
        var sValidChars = "0123456789.";
        var Char;
        var bDecimal = false;

        /* Check for a valid first char (no period and allow negatives) */
        Char = sData.charAt(0);
        if (sValidFirstChars.indexOf(Char) == -1) {
            return null;
        }

        /* Check all the other characters are valid */
        for (var i = 1 ; i < sData.length ; i++) {
            Char = sData.charAt(i);
            if (sValidChars.indexOf(Char) == -1) {
                return null;
            }

            /* Only allowed one decimal place... */
            if (Char == ".") {
                if (bDecimal) {
                    return null;
                }
                bDecimal = true;
            }
        }

        return 'num-html';
    }

})(jQuery, window);