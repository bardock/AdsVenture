jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "globalize-pre": function (a) {
        return Globalize.parseFloat(a);
    },

    "globalize-asc": function (a, b) {
        return ((a < b) ? -1 : ((a > b) ? 1 : 0));
    },

    "globalize-desc": function (a, b) {
        return ((a < b) ? 1 : ((a > b) ? -1 : 0));
    }
});