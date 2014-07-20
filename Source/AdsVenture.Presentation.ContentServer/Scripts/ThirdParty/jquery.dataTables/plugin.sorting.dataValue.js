jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "data-value-pre": function (a) {
        var value = $(a).data("value")
        if (typeof value == 'undefined') {
            return '';
        }
        if (/^\d+$/.test(value)) {
            return parseInt(value);
        }
        if (/^\d+\.\d+$/.test(value)) {
            return parseFloat(value);
        }
        return value;
    },

    "data-value-asc": function (a, b) {
        return ((a < b) ? -1 : ((a > b) ? 1 : 0));
    },

    "data-value-desc": function (a, b) {
        return ((a < b) ? 1 : ((a > b) ? -1 : 0));
    }
});