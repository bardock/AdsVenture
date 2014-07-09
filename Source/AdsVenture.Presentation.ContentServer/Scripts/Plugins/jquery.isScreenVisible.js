(function ($) {

    jQuery.fn.isScreenVisible = function (margin) {

        margin = margin || 0;

        var docViewTop = $(window).scrollTop() + margin;
        var docViewBottom = docViewTop + $(window).height() - margin;

        var elemTop = $(this).offset().top;
        var elemBottom = elemTop + $(this).height();

        return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));

    };
})(jQuery);