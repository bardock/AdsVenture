/* ===================================================
 * fileicon.js v1.0.0
 * ===================================================
 * @author fsanchez
 * 
 * =================================================== */

/* PUBLIC CLASS DEFINITION
 * ======================= */

module Plugins {

    export interface FileIconConfig {
        filename: string;
    }

    export class FileIcon {

        public static CONFIG: FileIconConfig = {
            filename: null,
        };
        public static ICON_CLASS_PREFIX = "bg_icon_";
        public static ICON_CLASS_DEFAULT = "file";
        public static ICON_CLASS_EXTENSIONS = {
            "excel": ["xls", "xlsx"],
            "doc": ["doc", "docx"],
            "txt": ["txt"],
            "img": ["jpg", "jpeg", "png", "bmp", "gif"],
            "pdf": ["pdf"],
            "ppt": ["ppt", "pptx"],
            "zip": ["zip"],
            "rar": ["rar"]
        };

        // Private Members
        private $container: any;
        private config: FileIconConfig;
        private filename: string;

        constructor(container: any, config: FileIconConfig) {
            this.$container = $(container);

            // Init config using default and overriding with specified one
            this.config = $.extend(FileIcon.CONFIG, config);

            this.setFilename(this.config.filename);
        }

        public setFilename(filename: string) {
            // Extract extension from filename
            var extensionMatch = /\.([^.]*)$/.exec(filename);
            var extension: string = extensionMatch && extensionMatch[1] || "";

            this.setExtension(extension);
        }

        public setExtension(extension: string) {
            this.removeAllClasses();
            var iconClass = this.getClass(extension);
            this.addClass(iconClass);
        }

        private removeAllClasses() {
            var allClassSuffixes: string[] = Object.keys(FileIcon.ICON_CLASS_EXTENSIONS);
            var allClasses: string[] = $.map(allClassSuffixes, (s) => FileIcon.ICON_CLASS_PREFIX + s);
            allClasses.push(FileIcon.ICON_CLASS_PREFIX + FileIcon.ICON_CLASS_DEFAULT); //remove icon default cssclass
            this.$container.removeClass(allClasses.join(" "));
        }

        private addClass(iconClass: string) {
            this.$container.addClass(FileIcon.ICON_CLASS_PREFIX + iconClass);
        }

        private getClass(extension: string): string {
            var iconClass = FileIcon.ICON_CLASS_DEFAULT;
            for (var group in FileIcon.ICON_CLASS_EXTENSIONS) {
                var extensions: string[] = FileIcon.ICON_CLASS_EXTENSIONS[group]
                if (extensions.indexOf(extension) != -1) {
                    iconClass = group;
                    break;
                }
            }
            return iconClass;
        }
    }
}


(function ($, window) {

    /* PLUGIN DEFINITION
     * ================= */

    jQuery.fn.fileicon = function (option) {

        return this.each(function (i, elem) {

            // Use received option or data-api as config
            var config = typeof option == 'object' && option
                || $(this).data("fileicon-apply")
                || { filename: $(this).data("fileicon-filename") };

            // Initialize instance if necessary
            var instance = $(this).data("fileicon");
            if (!instance) {
                $(this).data("fileicon", new Plugins.FileIcon(this, config));
            }

            // Apply instance function
            if (typeof option == 'string')
                instance[option]()

        });

    };

    /* DATA-API INITIALIZATION
     * ======================= */

    jQuery.fn.fileicon.dataApi_init = function () {
        $("[data-fileicon-filename]").fileicon();
    }
    $(document).ready(
        jQuery.fn.fileicon.dataApi_init
    );

})(jQuery, window);