/* ===================================================
* fileupload.js v1.8.0
* ===================================================
* @author fsanchez
*
* Extiente FileInput subiendo y eliminando archivos mediante AJAX.
*
* ========================================================== */
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
///<reference path='../Helpers/Event.ts'/>
///<reference path='FileInput.ts'/>
/* PUBLIC CLASS DEFINITION
* ======================= */
var Plugins;
(function (Plugins) {
    var FileUpload = (function () {
        function FileUpload(container, config) {
            var _this = this;
            this.onUpload = new Helpers.Event();
            this.onUploaded = new Helpers.Event();
            this.onRemove = new Helpers.Event();
            this.onRemoved = new Helpers.Event();
            // Private Members
            this.fileInput = null;
            // Init config using default and overriding with specified one
            this.config = $.extend(FileUpload.CONFIG, config);

            this.$container = $(container);

            // Initialize FileInput instance
            if (this.config.fileInput instanceof Plugins.FileInput) {
                this.fileInput = this.config.fileInput;
            } else if (typeof this.config.fileInput == "string" || this.config.fileInput instanceof jQuery) {
                // Find instance using specified selector or jquery object,
                // first check current container and then search in entire DOM
                this.fileInput = this.$container.is(this.config.fileInput) ? this.$container.data("fileinput") : $(this.config.fileInput).data("fileinput");
            }

            // Init jquery objects
            this.$urlInput = this.$container.find(this.config.urlInput);
            this.$progressBar = this.$container.find(this.config.progressBar);
            this.$uploadingContainer = this.$container.find(this.config.uploadingContainer);
            this.$removingContainer = this.$container.find(this.config.removingContainer);

            // Init styles
            this.toggleLoadingStyle(false);
            this.toggleRemovingStyle(false);

            // Init url
            this.setUrl(this.$urlInput.val());
            if (this.getUrl())
                this.fileInput.initUrl(this.getUrl()); // initialize FileInput url

            // Init uploader
            this.config.uploader = this.config.uploader || (Plugins.FileInputHelper.isFileApiSupported() ? new AjaxUploader() : new LegacyAjaxUploader());

            // Bind external events
            if (this.config.onUpload)
                this.Upload().on(this.config.onUpload);
            if (this.config.onUploaded)
                this.Uploaded().on(this.config.onUploaded);
            if (this.config.onRemove)
                this.Remove().on(this.config.onRemove);
            if (this.config.onRemoved)
                this.Removed().on(this.config.onRemoved);

            // Bind internal events
            this.fileInput.Selected().on(function (fileInput) {
                _this.upload();
            });
            $(document).on("click", this.config.stopUploadButton, function () {
                _this.stopUpload();
            });
            this.fileInput.Removed().on(function (fileInput) {
                _this.remove();
            });
        }
        FileUpload.prototype.Upload = function () {
            return this.onUpload;
        };

        FileUpload.prototype.Uploaded = function () {
            return this.onUploaded;
        };

        FileUpload.prototype.Remove = function () {
            return this.onRemove;
        };

        FileUpload.prototype.Removed = function () {
            return this.onRemoved;
        };

        FileUpload.prototype.getFileInput = function () {
            return this.fileInput;
        };

        FileUpload.prototype.getUrl = function () {
            return this.url;
        };

        FileUpload.prototype.setUrl = function (url) {
            this.url = url || null;
            this.$urlInput.val(this.url);
            this.fileInput.setUrl(this.url);
        };

        FileUpload.prototype.upload = function () {
            var _this = this;
            this.progressCompletedChange(0);
            this.toggleSelectedStyle(false);
            this.toggleLoadingStyle(true);

            this.onUpload.trigger(this);

            this.config.uploader.start(this.getFileInput().getFile(), function (formData) {
                return _this.config.uploadConfig(formData, _this);
            }, function (completed) {
                return _this.progressCompletedChange(completed);
            }, function (url) {
                return _this.uploaded(url);
            }, function () {
                return _this.clear();
            });
        };

        FileUpload.prototype.progressCompletedChange = function (completed) {
            completed = completed || 0;
            var percentage = parseInt(completed * 100 + "") + "%";
            this.$progressBar.find(this.config.progressCompletedBar).css("width", percentage);
        };

        FileUpload.prototype.uploaded = function (url) {
            this.setUrl(url);
            this.toggleLoadingStyle(false);
            this.toggleSelectedStyle(true);
            this.onUploaded.trigger(this);
        };

        FileUpload.prototype.stopUpload = function () {
            this.config.uploader.stop();
            this.clear();
        };

        FileUpload.prototype.clear = function () {
            this.setUrl(null);
            this.toggleLoadingStyle(false);
            this.toggleRemovingStyle(false);
            this.fileInput.clear();
        };

        FileUpload.prototype.remove = function () {
            var _this = this;
            // Get optional remove config
            var config = !this.config.removeConfig || this.config.removeConfig(this);

            // If it is empty, avoid ajax request and clear me
            if (!config) {
                this.clear();
                return;
            }
            this.toggleSelectedStyle(false);
            this.toggleRemovingStyle(true);

            this.onRemove.trigger(this);

            $.ajax(config).done(function () {
                _this.removed();
            }).fail(function () {
                _this.clear();
            });
        };

        FileUpload.prototype.removed = function () {
            this.clear();
            this.onRemoved.trigger(this);
        };

        FileUpload.prototype.toggleSelectedStyle = function (selected) {
            this.fileInput.toggleSelectedStyle(selected);
        };

        FileUpload.prototype.toggleLoadingStyle = function (visible) {
            this.$uploadingContainer.toggle(visible);
            this.$container.toggleClass(FileUpload.CONTAINER_UPLOADING_CSSCLASS, visible);
        };

        FileUpload.prototype.toggleRemovingStyle = function (visible) {
            this.$removingContainer.toggle(visible);
            this.$container.toggleClass(FileUpload.CONTAINER_REMOVING_CSSCLASS, visible);
        };
        FileUpload.CONTAINER_UPLOADING_CSSCLASS = "fileupload-uploading";
        FileUpload.CONTAINER_REMOVING_CSSCLASS = "fileupload-removing";
        FileUpload.CONFIG = {
            fileInput: "[data-fileinput-apply]",
            uploadConfig: function (formData, fileUpload) {
                return null;
            },
            removeConfig: function (fileUpload) {
                return null;
            },
            urlInput: "[data-fileupload=url]",
            progressBar: "[data-fileupload=progress]",
            progressCompletedBar: "[data-fileupload=completed]",
            uploadingContainer: "[data-fileupload=uploading]",
            removingContainer: "[data-fileupload=removing]",
            stopUploadButton: "[data-fileupload=stop]",
            onUpload: function (fileUpload) {
            },
            onUploaded: function (fileUpload) {
            },
            onRemove: function (fileUpload) {
            },
            onRemoved: function (fileUpload) {
            }
        };
        return FileUpload;
    })();
    Plugins.FileUpload = FileUpload;

    var AjaxUploader = (function () {
        function AjaxUploader() {
        }
        AjaxUploader.prototype.start = function (file, getConfig, onProgressChange, onSuccess, onError) {
            this.jqXHR = $.ajax(this.getUploadConfig(file, getConfig, onProgressChange)).done(function (url) {
                onSuccess(url);
            }).fail(function () {
                onError();
            });
        };

        AjaxUploader.prototype.getUploadConfig = function (file, getConfig, onProgressChange) {
            var formData = new FormData();
            formData.append(0, file);

            // Make ajax request with default config
            // and overriding with user specified config
            return $.extend({
                data: formData,
                processData: false,
                contentType: false,
                dataType: "text",
                xhr: function () {
                    // Bind progress
                    var xhr = new window.XMLHttpRequest();
                    xhr.upload.addEventListener("progress", function (e) {
                        if (e.lengthComputable) {
                            var completed = e.loaded / e.total;
                            onProgressChange(completed);
                        }
                    }, false);
                    return xhr;
                }
            }, getConfig(formData));
        };

        AjaxUploader.prototype.stop = function () {
            if (this.jqXHR)
                this.jqXHR.abort();
        };
        return AjaxUploader;
    })();

    var LegacyAjaxUploader = (function (_super) {
        __extends(LegacyAjaxUploader, _super);
        function LegacyAjaxUploader() {
            _super.apply(this, arguments);
        }
        LegacyAjaxUploader.prototype.getUploadConfig = function (file, getConfig, onProgressChange) {
            // Depends on jQuery Iframe Transport plugin that supports file uploads through a hidden iframe
            // https://github.com/cmlenz/jquery-iframe-transport
            return $.extend({
                files: file,
                iframe: true
            }, getConfig(file), {
                dataType: "text"
            });
        };
        return LegacyAjaxUploader;
    })(AjaxUploader);

    var FileUploaderMock = (function () {
        function FileUploaderMock() {
        }
        FileUploaderMock.prototype.start = function (file, getConfig, onProgressChange, onSuccess, onError) {
            this.completed = 0;
            this.scheduleIncrementProgress(file, onProgressChange, onSuccess);
        };

        FileUploaderMock.prototype.scheduleIncrementProgress = function (file, onProgressChange, onSuccess) {
            var _this = this;
            this.timeoutId = setTimeout(function () {
                _this.incrementProgress(file, onProgressChange, onSuccess);
            }, 1000);
        };

        FileUploaderMock.prototype.incrementProgress = function (file, onProgressChange, onSuccess) {
            this.completed += 0.2;
            onProgressChange(this.completed);

            if (this.completed >= 1)
                onSuccess("http://google.com/" + encodeURI(file.name));
            else
                this.scheduleIncrementProgress(file, onProgressChange, onSuccess);
        };

        FileUploaderMock.prototype.stop = function () {
            clearTimeout(this.timeoutId);
        };
        return FileUploaderMock;
    })();
    Plugins.FileUploaderMock = FileUploaderMock;
})(Plugins || (Plugins = {}));

(function ($, window) {
    /* PLUGIN DEFINITION
    * ================= */
    jQuery.fn.fileupload = function (option) {
        return this.each(function (i, elem) {
            // Use received option or data-api as config
            var config = typeof option == 'object' && option || $(this).data("fileupload-apply") || {};

            // Initialize instance if necessary
            var instance = $(this).data("fileupload");
            if (!instance) {
                $(this).data("fileupload", new Plugins.FileUpload(this, config));
            }

            // Apply instance function
            if (typeof option == 'string')
                instance[option]();
        });
    };

    /* DATA-API INITIALIZATION
    * ======================= */
    jQuery.fn.fileupload.dataApi_init = function () {
        $("[data-fileupload-apply]").fileupload();
    };
    $(document).ready(jQuery.fn.fileupload.dataApi_init);
})(jQuery, window);
//# sourceMappingURL=fileupload.js.map
