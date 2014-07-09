/* ===================================================
 * fileupload.js v1.8.0
 * ===================================================
 * @author fsanchez
 * 
 * Extiente FileInput subiendo y eliminando archivos mediante AJAX.
 * 
 * ========================================================== */

///<reference path='../Helpers/Event.ts'/>
///<reference path='FileInput.ts'/>

/* PUBLIC CLASS DEFINITION
 * ======================= */

module Plugins {

    export interface FileUploadConfig {
        fileInput: any;  // FileInput, selector or jquery object;
        uploadConfig: (FormData, FileUpload) => any;
        removeConfig: (FileUpload) => any; // optional
        urlInput: any; // selector or jquery object
        progressBar: any; // selector or jquery object
        progressCompletedBar: any; // selector or jquery object
        uploadingContainer: any; // selector or jquery object
        removingContainer: any; // selector or jquery object
        stopUploadButton: any; // selector or jquery object
        onUpload: (FileUpload) => void;
        onUploaded: (FileUpload) => void;
        onRemove: (FileUpload) => void;
        onRemoved: (FileUpload) => void;
        uploader?: IFileUploader;
    }

    export class FileUpload {

        private onUpload = new Helpers.Event();
        Upload(): Helpers.IEvent { return this.onUpload; }

        private onUploaded = new Helpers.Event();
        Uploaded(): Helpers.IEvent { return this.onUploaded; }

        private onRemove = new Helpers.Event();
        Remove(): Helpers.IEvent { return this.onRemove; }

        private onRemoved = new Helpers.Event();
        Removed(): Helpers.IEvent { return this.onRemoved; }

        // Statics
        public static CONTAINER_UPLOADING_CSSCLASS = "fileupload-uploading";
        public static CONTAINER_REMOVING_CSSCLASS = "fileupload-removing";
        public static CONFIG: FileUploadConfig = {
            fileInput: "[data-fileinput-apply]",
            uploadConfig: (formData, fileUpload) => { return null },
            removeConfig: (fileUpload) => { return null },
            urlInput: "[data-fileupload=url]",
            progressBar: "[data-fileupload=progress]",
            progressCompletedBar: "[data-fileupload=completed]",
            uploadingContainer: "[data-fileupload=uploading]",
            removingContainer: "[data-fileupload=removing]",
            stopUploadButton: "[data-fileupload=stop]",
            onUpload: (fileUpload) => { },
            onUploaded: (fileUpload) => { },
            onRemove: (fileUpload) => { },
            onRemoved: (fileUpload) => { }
        };

        config: FileUploadConfig;

        // Private Members
        private fileInput: FileInput = null;
        private $container: any;
        private $urlInput: any;
        private $progressBar: any;
        private $uploadingContainer: any;
        private $removingContainer: any;
        private url: string;

        constructor(
            container: any, // preferably a form
            config: FileUploadConfig) {
            // Init config using default and overriding with specified one
            this.config = $.extend(FileUpload.CONFIG, config);

            this.$container = $(container);

            // Initialize FileInput instance
            if (this.config.fileInput instanceof FileInput) {
                this.fileInput = this.config.fileInput;
            }
            else if (typeof this.config.fileInput == "string"
                || this.config.fileInput instanceof jQuery) {
                // Find instance using specified selector or jquery object, 
                // first check current container and then search in entire DOM
                this.fileInput = this.$container.is(this.config.fileInput)
                ? this.$container.data("fileinput")
                : $(this.config.fileInput).data("fileinput");
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
                if (this.getUrl()) this.fileInput.initUrl(this.getUrl()); // initialize FileInput url

            // Init uploader
            this.config.uploader = this.config.uploader
                || (Plugins.FileInputHelper.isFileApiSupported()
                    ? new AjaxUploader()
                    : new LegacyAjaxUploader());

            // Bind external events
            if (this.config.onUpload) this.Upload().on(this.config.onUpload);
            if (this.config.onUploaded) this.Uploaded().on(this.config.onUploaded);
            if (this.config.onRemove) this.Remove().on(this.config.onRemove);
            if (this.config.onRemoved) this.Removed().on(this.config.onRemoved);

            // Bind internal events
            this.fileInput.Selected().on((fileInput) => {
                this.upload();
            });
            $(document).on("click", this.config.stopUploadButton, () => {
                this.stopUpload();
            });
            this.fileInput.Removed().on((fileInput) => {
                this.remove();
            });
        }

        getFileInput(): FileInput {
            return this.fileInput;
        }

        getUrl(): string {
            return this.url;
        }

        private setUrl(url: string) {
            this.url = url || null;
            this.$urlInput.val(this.url);
            this.fileInput.setUrl(this.url);
        }

        upload() {
            this.progressCompletedChange(0);
            this.toggleSelectedStyle(false);
            this.toggleLoadingStyle(true);

            this.onUpload.trigger(this);

            this.config.uploader.start(
                this.getFileInput().getFile(),
                (formData) => this.config.uploadConfig(formData, this),
                (completed) => this.progressCompletedChange(completed),
                (url) => this.uploaded(url),
                () => this.clear()
            );
        }

        private progressCompletedChange(completed: number) {
            completed = completed || 0;
            var percentage = parseInt(completed * 100 + "") + "%";
            this.$progressBar
                .find(this.config.progressCompletedBar)
                .css("width", percentage);
        }

        private uploaded(url: string) {
            this.setUrl(url);
            this.toggleLoadingStyle(false);
            this.toggleSelectedStyle(true);
            this.onUploaded.trigger(this);
        }

        stopUpload() {
            this.config.uploader.stop();
            this.clear();
        }

        private clear() {
            this.setUrl(null);
            this.toggleLoadingStyle(false);
            this.toggleRemovingStyle(false);
            this.fileInput.clear();
        }

        remove() {
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

            $.ajax(config)
                .done(() => { this.removed(); })
                .fail(() => { this.clear(); });
        }

        private removed() {
            this.clear();
            this.onRemoved.trigger(this);
        }

        private toggleSelectedStyle(selected: boolean) {
            this.fileInput.toggleSelectedStyle(selected);
        }

        private toggleLoadingStyle(visible: boolean) {
            this.$uploadingContainer.toggle(visible);
            this.$container.toggleClass(FileUpload.CONTAINER_UPLOADING_CSSCLASS, visible);
        }

        private toggleRemovingStyle(visible: boolean) {
            this.$removingContainer.toggle(visible);
            this.$container.toggleClass(FileUpload.CONTAINER_REMOVING_CSSCLASS, visible);
        }
    }

    export interface IFileUploader {
        start(
            file: File,
            getConfig: (FormData) => any,
            onProgressChange: (completed: number) => void,
            onSuccess: (url: string) => void,
            onError: () => void
            );
        stop();
    }

    class AjaxUploader implements IFileUploader {

        private jqXHR;

        start(
            file: File,
            getConfig: (FormData) => any,
            onProgressChange: (completed: number) => void,
            onSuccess: (url: string) => void,
            onError: () => void)
        {
            this.jqXHR = $.ajax(this.getUploadConfig(file, getConfig, onProgressChange))
                .done((url) => { onSuccess(url); })
                .fail(() => { onError(); });
        }

        public getUploadConfig(
            file: File,
            getConfig: (FormData) => any,
            onProgressChange: (completed: number) => void)
        {
            var formData = new FormData();
            formData.append(0, file);

            // Make ajax request with default config 
            // and overriding with user specified config
            return $.extend(
                {
                    data: formData,
                    processData: false,
                    contentType: false,
                    dataType: "text",
                    xhr: () => {
                        // Bind progress
                        var xhr = new (<any>window).XMLHttpRequest();
                        xhr.upload.addEventListener(
                            "progress",
                            (e) => {
                                if (e.lengthComputable) {
                                    var completed = e.loaded / e.total;
                                    onProgressChange(completed);
                                }
                            },
                            false
                        );
                        return xhr;
                    }
                },
                getConfig(formData)
            );
        }

        stop() {
            if (this.jqXHR)
                this.jqXHR.abort();
        }
    }

    class LegacyAjaxUploader extends AjaxUploader {

        public getUploadConfig(
            file: File,
            getConfig: (FormData) => any,
            onProgressChange: (completed: number) => void)
        {
            // Depends on jQuery Iframe Transport plugin that supports file uploads through a hidden iframe
            // https://github.com/cmlenz/jquery-iframe-transport

            return $.extend(
                {
                    files: file,
                    iframe: true
                },
                getConfig(file),
                {
                    dataType: "text"
                }
            );
        }
    }

    export class FileUploaderMock implements IFileUploader {

        private completed: number;
        private timeoutId: number;

        start(
            file: File,
            getConfig: (FormData) => any,
            onProgressChange: (completed: number) => void,
            onSuccess: (url: string) => void,
            onError: () => void)
        {
            this.completed = 0;
            this.scheduleIncrementProgress(file, onProgressChange, onSuccess);
        }

        private scheduleIncrementProgress(file: File, onProgressChange: (completed: number) => void, onSuccess: (url: string) => void) {
            this.timeoutId = setTimeout(
                () => { this.incrementProgress(file, onProgressChange, onSuccess) },
                1000);
        }

        private incrementProgress(file: File, onProgressChange: (completed: number) => void, onSuccess: (url: string) => void) {
            this.completed += 0.2
            onProgressChange(this.completed);

            if (this.completed >= 1)
                onSuccess("http://google.com/" + encodeURI(file.name));
            else
                this.scheduleIncrementProgress(file, onProgressChange, onSuccess);
        }

        stop() {
            clearTimeout(this.timeoutId);
        }
    }
}


(function ($, window) {

    /* PLUGIN DEFINITION
     * ================= */

    jQuery.fn.fileupload = function (option) {

        return this.each(function (i, elem) {

            // Use received option or data-api as config
            var config = typeof option == 'object' && option
                || $(this).data("fileupload-apply")
                || {};

            // Initialize instance if necessary
            var instance = $(this).data("fileupload");
            if (!instance) {
                $(this).data("fileupload", new Plugins.FileUpload(this, config));
            }

            // Apply instance function
            if (typeof option == 'string')
                instance[option]()

        });

    };

    /* DATA-API INITIALIZATION
     * ======================= */

    jQuery.fn.fileupload.dataApi_init = function () {
        $("[data-fileupload-apply]").fileupload();
    }
    $(document).ready(
        jQuery.fn.fileupload.dataApi_init
        );

})(jQuery, window);