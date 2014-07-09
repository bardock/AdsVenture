/* ===================================================
* fileicon.js v1.0.0
* ===================================================
* @author fsanchez
*
* =================================================== */
/* PUBLIC CLASS DEFINITION
* ======================= */
var Plugins;
(function (Plugins) {
    var FileIcon = (function () {
        function FileIcon(container, config) {
            this.$container = $(container);

            // Init config using default and overriding with specified one
            this.config = $.extend(FileIcon.CONFIG, config);

            this.setFilename(this.config.filename);
        }
        FileIcon.prototype.setFilename = function (filename) {
            // Extract extension from filename
            var extensionMatch = /\.([^.]*)$/.exec(filename);
            var extension = extensionMatch && extensionMatch[1] || "";

            this.setExtension(extension);
        };

        FileIcon.prototype.setExtension = function (extension) {
            this.removeAllClasses();
            var iconClass = this.getClass(extension);
            this.addClass(iconClass);
        };

        FileIcon.prototype.removeAllClasses = function () {
            var allClassSuffixes = Object.keys(FileIcon.ICON_CLASS_EXTENSIONS);
            var allClasses = $.map(allClassSuffixes, function (s) {
                return FileIcon.ICON_CLASS_PREFIX + s;
            });
            allClasses.push(FileIcon.ICON_CLASS_PREFIX + FileIcon.ICON_CLASS_DEFAULT); //remove icon default cssclass
            this.$container.removeClass(allClasses.join(" "));
        };

        FileIcon.prototype.addClass = function (iconClass) {
            this.$container.addClass(FileIcon.ICON_CLASS_PREFIX + iconClass);
        };

        FileIcon.prototype.getClass = function (extension) {
            var iconClass = FileIcon.ICON_CLASS_DEFAULT;
            for (var group in FileIcon.ICON_CLASS_EXTENSIONS) {
                var extensions = FileIcon.ICON_CLASS_EXTENSIONS[group];
                if (extensions.indexOf(extension) != -1) {
                    iconClass = group;
                    break;
                }
            }
            return iconClass;
        };
        FileIcon.CONFIG = {
            filename: null
        };
        FileIcon.ICON_CLASS_PREFIX = "bg_icon_";
        FileIcon.ICON_CLASS_DEFAULT = "file";
        FileIcon.ICON_CLASS_EXTENSIONS = {
            "excel": ["xls", "xlsx"],
            "doc": ["doc", "docx"],
            "txt": ["txt"],
            "img": ["jpg", "jpeg", "png", "bmp", "gif"],
            "pdf": ["pdf"],
            "ppt": ["ppt", "pptx"],
            "zip": ["zip"],
            "rar": ["rar"]
        };
        return FileIcon;
    })();
    Plugins.FileIcon = FileIcon;
})(Plugins || (Plugins = {}));

(function ($, window) {
    /* PLUGIN DEFINITION
    * ================= */
    jQuery.fn.fileicon = function (option) {
        return this.each(function (i, elem) {
            // Use received option or data-api as config
            var config = typeof option == 'object' && option || $(this).data("fileicon-apply") || { filename: $(this).data("fileicon-filename") };

            // Initialize instance if necessary
            var instance = $(this).data("fileicon");
            if (!instance) {
                $(this).data("fileicon", new Plugins.FileIcon(this, config));
            }

            // Apply instance function
            if (typeof option == 'string')
                instance[option]();
        });
    };

    /* DATA-API INITIALIZATION
    * ======================= */
    jQuery.fn.fileicon.dataApi_init = function () {
        $("[data-fileicon-filename]").fileicon();
    };
    $(document).ready(jQuery.fn.fileicon.dataApi_init);
})(jQuery, window);
//# sourceMappingURL=fileicon.js.map
