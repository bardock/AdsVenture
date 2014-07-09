/* ===================================================
* fileinput.js v1.7.0
* ===================================================
* @author fsanchez
*
* Envuelve un input de tipo file y permite utilizar elementos custom
* como botón para seleccionar un archivo, eliminarlo,
* mostrar el nombre del archivo y poder descargarlo.
* Además, brinda soporte para realizar drag and drop sobre el contenedor
* y validación de extensión y tamaño de archivo.
*
* ========================================================== */
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
///<reference path='../Helpers/Event.ts'/>
///<reference path='FileIcon.ts'/>
/* PUBLIC CLASS DEFINITION
* ======================= */
var Plugins;
(function (Plugins) {
    var FileInputHelper = (function () {
        function FileInputHelper() {
        }
        FileInputHelper.isFileApiSupported = function () {
            return window.FileReader !== undefined && window.FormData !== undefined;
        };

        FileInputHelper.getFilenameFromUrl = function (url) {
            var matches = new RegExp("\/?([^/]+)$").exec(url.replace(/\\/gi, "/"));

            if (matches.length <= 1)
                return null;

            return matches[1];
        };
        return FileInputHelper;
    })();
    Plugins.FileInputHelper = FileInputHelper;

    var FileInput = (function () {
        function FileInput(container, config) {
            this.onSelected = new Helpers.Event();
            this.onSelectedInvalid = new Helpers.Event();
            this.onRemoved = new Helpers.Event();
            // Private Members
            this.file = null;
            this.url = null;
            // Init config using default and overriding with specified one
            this.config = $.extend(FileInput.CONFIG, config);

            // Init jquery objects
            this.$container = $(container);
            this.$uploadButton = this.$container.find(this.config.uploadButton);
            this.$removeButton = this.$container.find(this.config.removeButton);
            this.$filename = this.$container.find(this.config.filename);
            this.$icon = this.$container.find(this.config.icon).fileicon();
            this.icon = this.$icon.data("fileicon");
            this.$downloadLink = this.$container.find(this.config.downloadLink);
            this.$selectedContainer = this.$container.find(this.config.selectedContainer);
            this.$emptyContainer = this.$container.find(this.config.emptyContainer);
            this.$emptyDroppableContainer = this.$container.find(this.config.emptyDroppableContainer);
            this.$emptyNoDroppableContainer = this.$container.find(this.config.emptyNoDroppableContainer);
            this.$isEmptyInput = this.$container.find(this.config.isEmptyInput);

            // Init with url in DOM
            var url = this.$downloadLink.attr("href");
            this.initUrl(url);

            this.bindEvents();

            this.initMimeTypes();
        }
        FileInput.prototype.Selected = function () {
            return this.onSelected;
        };

        FileInput.prototype.SelectedInvalid = function () {
            return this.onSelectedInvalid;
        };

        FileInput.prototype.Removed = function () {
            return this.onRemoved;
        };

        FileInput.prototype.initUrl = function (url) {
            url = url == "javascript:void(0)" ? null : url;
            this.setUrl(url);

            // Init filename with url by default
            if (url && !this.getFilename()) {
                var matches = new RegExp("\/?([^/]+)$").exec(url);
                if (matches.length > 1)
                    this.setFilename(decodeURIComponent(matches[1]));
            }

            // Init selected style
            var isSelected = (this.getFilename().length > 0);
            this.toggleSelectedAndEmptyStyles(isSelected);
        };

        /* Initialize accept attribute in order to filter files by mime type in the picker */
        FileInput.prototype.initMimeTypes = function () {
            if (window.MimeType) {
                var $fileInput = this.getFileInput();

                // If data-val-accept-exts is defined and accept attribute is empty,
                // convert extensions to mime types
                var exts = this.getAcceptExtensions();
                if (exts && !$fileInput.attr("accept")) {
                    var mimes = [];
                    $.each(exts.split(","), function (i, x) {
                        var mime = window.MimeType.lookup(x);
                        if (mime)
                            mimes.push(mime);
                    });
                    $fileInput.attr("accept", mimes.join(", "));
                }
            }
        };

        FileInput.prototype.bindEvents = function () {
            var _this = this;
            // Bind file input change
            this.$container.on("change", this.getFileInput().selector, function () {
                _this.handleFileInputChanged();
            });

            // Bind file input disabled change
            this.$container.on("custom.disabled", this.getFileInput().selector, function (e, _, disabled) {
                _this.disable(disabled);
            });

            // Bind upload button
            this.$container.on("click", this.config.uploadButton, function () {
                _this.showPicker();
            });

            // Bind remove button
            this.$container.on("click", this.config.removeButton, function () {
                _this.remove();
            });

            // Bind drop
            if (this.config.drop) {
                this.$container.bind('drop', function (e) {
                    _this.handleDrop(e);
                    return false;
                }).bind('dragover', function (e) {
                    e.preventDefault();
                    return false;
                });
            }

            // Bind file selected
            if (this.config.onSelected)
                this.Selected().on(this.config.onSelected);

            // Bind invalid file selected
            if (this.config.onSelectedInvalid)
                this.SelectedInvalid().on(this.config.onSelectedInvalid);

            // Bind file removed
            if (this.config.onRemoved)
                this.Removed().on(this.config.onRemoved);
        };

        FileInput.prototype.getFileInput = function () {
            return this.$container.find("[type=file]:first");
        };

        FileInput.prototype.getFile = function () {
            return this.file;
        };

        FileInput.prototype.setFile = function (file) {
            return this.file = file;
        };

        FileInput.prototype.showPicker = function () {
            this.getFileInput().click();
        };

        FileInput.prototype.handleFileInputChanged = function () {
            var $fileInput = this.getFileInput();
            var file = $fileInput[0].files[0];

            if (!this.isValid(file)) {
                this.resetFileInput();
                return;
            }
            this.select(file);
        };

        FileInput.prototype.handleDrop = function (e) {
            e.preventDefault();

            if (this.isDisabled())
                return;

            if (e.dataTransfer.files.length == 0)
                return;

            var file = e.dataTransfer.files[0];

            if (!this.isValid(file))
                return false;

            if (this.config.onDropped(file, this) !== false)
                this.select(file);
        };

        FileInput.prototype.isValid = function (file) {
            var result = {
                fileInput: this,
                file: file,
                validExtension: this.hasValidExtension(file),
                validSize: this.hasValidSize(file),
                customValidation: this.config.onValidating(file, this)
            };
            if (result.validExtension && result.validSize && (result.customValidation === null || result.customValidation === undefined || result.customValidation === true)) {
                return true;
            } else {
                this.onSelectedInvalid.trigger(result);
                return false;
            }
        };

        FileInput.prototype.hasValidExtension = function (file) {
            var exts = this.getAcceptExtensions();
            if (!exts)
                return true;

            var regex = new RegExp("\." + exts.replace(/,/g, "|") + "$", "i");
            return regex.test(file.name);
        };

        FileInput.prototype.getAcceptExtensions = function () {
            return this.getFileInput().attr(this.config.acceptExtensionsAttribute) || this.config.acceptExtensions || null;
        };

        FileInput.prototype.hasValidSize = function (file) {
            var maxSize = this.getMaxSize();
            return !maxSize || !file.size || file.size <= maxSize;
        };

        FileInput.prototype.getMaxSize = function () {
            return this.getFileInput().attr(this.config.maxSizeAttribute) || this.config.maxSize || null;
        };

        /* Show selected file name and download link */
        FileInput.prototype.select = function (file, filename) {
            if (typeof filename === "undefined") { filename = null; }
            var _this = this;
            this.setFile(file);

            this.setFilename(filename || this.getFile().name);

            this.getFileUrlAsync(function (url) {
                _this.setUrl(url);
            });

            this.toggleSelectedAndEmptyStyles(true);

            this.onSelected.trigger(this);
        };

        FileInput.prototype.remove = function () {
            this.clear();
            this.onRemoved.trigger(this);
        };

        /* Clear file input and show upload */
        FileInput.prototype.clear = function () {
            this.setFile(null);
            this.resetFileInput();

            this.setFilename(null);
            this.setUrl(null);

            this.toggleSelectedAndEmptyStyles(false);
        };

        FileInput.prototype.disable = function (disabled) {
            this.getFileInput().disable(disabled);
        };

        FileInput.prototype.isDisabled = function () {
            return this.getFileInput().is(":disabled");
        };

        FileInput.prototype.getUrl = function () {
            return this.url;
        };

        FileInput.prototype.setUrl = function (url) {
            this.url = url;
            this.$downloadLink.attr("href", url);
            this.setIsEmpty(this.url == null);
        };

        FileInput.prototype.getFilename = function () {
            return $.trim(this.$filename.html());
        };

        FileInput.prototype.getIsEmpty = function () {
            return this.isEmpty;
        };

        FileInput.prototype.setIsEmpty = function (val) {
            this.isEmpty = val;
            this.$isEmptyInput.val(val);
        };

        FileInput.prototype.setFilename = function (val) {
            this.$filename.html(val);
            this.icon.setFilename(val);
        };

        FileInput.prototype.resetFileInput = function () {
            var $fileInput = this.getFileInput();
            $fileInput.wrap('<form>').closest('form').get(0).reset();
            $fileInput.unwrap();
        };

        FileInput.prototype.getFileUrlAsync = function (onload) {
            var _this = this;
            var reader = new FileReader();
            reader.onload = function (event) {
                onload.call(_this, event.target.result);
            };
            reader.readAsDataURL(this.getFile());
        };

        FileInput.prototype.toggleSelectedAndEmptyStyles = function (selected) {
            this.toggleSelectedStyle(selected);
            this.toggleEmptyStyle(!selected);
        };

        FileInput.prototype.toggleSelectedStyle = function (selected) {
            this.$container.toggleClass(FileInput.CONTAINER_SELECTED_CSSCLASS, selected);
            this.$selectedContainer.toggle(selected);
        };

        FileInput.prototype.toggleEmptyStyle = function (empty) {
            this.$container.toggleClass(FileInput.CONTAINER_EMPTY_CSSCLASS, empty);
            this.$emptyContainer.toggle(empty);
            if (this.config.drop) {
                this.$emptyDroppableContainer.toggle(empty);
                this.$emptyNoDroppableContainer.toggle(false);
            } else {
                this.$emptyDroppableContainer.toggle(false);
                this.$emptyNoDroppableContainer.toggle(empty);
            }
        };
        FileInput.CONTAINER_EMPTY_CSSCLASS = "fileinput-empty";
        FileInput.CONTAINER_SELECTED_CSSCLASS = "fileinput-selected";
        FileInput.CONFIG = {
            uploadButton: "[data-fileinput=upload]",
            removeButton: "[data-fileinput=remove]",
            filename: "[data-fileinput=filename]",
            icon: "[data-fileinput=icon]",
            downloadLink: "[data-fileinput=filename]",
            selectedContainer: "[data-fileinput=selected]",
            emptyContainer: "[data-fileinput=empty],[data-fileupload=empty]",
            emptyDroppableContainer: "[data-fileinput=empty-droppable]",
            emptyNoDroppableContainer: "[data-fileinput=empty-nodroppable]",
            isEmptyInput: "[data-fileinput=isempty]",
            drop: FileInputHelper.isFileApiSupported(),
            onDropped: function (file, fileinput) {
                return true;
            },
            onValidating: function (file, fileinput) {
            },
            onSelected: function (fileinput) {
            },
            onSelectedInvalid: function (fileinput) {
            },
            onRemoved: function (fileinput) {
            },
            acceptExtensionsAttribute: "data-val-accept-exts",
            acceptExtensions: null,
            maxSizeAttribute: "data-val-maxsize",
            maxSize: null
        };
        return FileInput;
    })();
    Plugins.FileInput = FileInput;

    var FileInputLegacy = (function (_super) {
        __extends(FileInputLegacy, _super);
        function FileInputLegacy() {
            _super.apply(this, arguments);
        }
        FileInputLegacy.prototype.handleFileInputChanged = function () {
            var $fileInput = this.getFileInput();
            if (!$fileInput.val())
                return;

            var filename = FileInputHelper.getFilenameFromUrl($fileInput.val());
            if (!filename)
                return;

            if (!this.isValid({ name: filename })) {
                this.resetFileInput();
                return;
            }
            this.select(null, filename);
        };

        FileInputLegacy.prototype.showPicker = function () {
            throw "This operation is not supported in your browser";
        };

        FileInputLegacy.prototype.getFileUrlAsync = function (onload) {
            return;
        };
        return FileInputLegacy;
    })(FileInput);
    Plugins.FileInputLegacy = FileInputLegacy;
})(Plugins || (Plugins = {}));

(function ($, window) {
    /* PLUGIN DEFINITION
    * ================= */
    jQuery.fn.fileinput = function (option) {
        return this.each(function (i, elem) {
            // Use received option or data-api as config
            var config = typeof option == 'object' && option || $(this).data("fileinput-apply") || {};

            // Initialize instance if necessary
            var instance = $(this).data("fileinput");
            if (!instance) {
                $(this).data("fileinput", instance = Plugins.FileInputHelper.isFileApiSupported() ? new Plugins.FileInput(this, config) : new Plugins.FileInputLegacy(this, config));
            }

            // Apply instance function
            if (typeof option == 'string')
                instance[option]();
        });
    };

    /* DATA-API INITIALIZATION
    * ======================= */
    jQuery.fn.fileinput.dataApi_init = function () {
        $("[data-fileinput-apply]").fileinput();
    };
    $(document).ready(jQuery.fn.fileinput.dataApi_init);

    /* DATA-API DROP
    * ============= */
    $.event.props.push('dataTransfer');

    $(document).on("drop", "[data-fileinput=drop][data-target]", function (e) {
        var target = $(this).data("target");
        var fileinput = $(target).data("fileinput");
        if (fileinput)
            fileinput.handleDrop(e);
        return false;
    }).on("dragover", "[data-fileinput=drop][data-target]", function (e) {
        // Prevent that the browser open the file
        e.preventDefault();
        return false;
    });

    /* HANDLE FORM/INPUT CLEARED EVENT
    * =============================== */
    $(document).ready(function () {
        var selector = "." + Plugins.FileInput.CONTAINER_SELECTED_CSSCLASS;

        // Clear element when parent form is resetted
        $(document).on("reset", "form", function (e) {
            $(this).find(selector).each(function () {
                $(this).data("fileinput").remove();
            });
        });

        // Clear element when element is cleared
        // Dependecy: clearForm plugin
        if ($.fn.clearForm) {
            $(document).on("cleared", selector, function (e, elem, defaultValue) {
                $(this).data("fileinput").remove();
            });
        }
    });
})(jQuery, window);
//# sourceMappingURL=fileinput.js.map
