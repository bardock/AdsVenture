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

///<reference path='../Helpers/Event.ts'/>
///<reference path='FileIcon.ts'/>

/* PUBLIC CLASS DEFINITION
 * ======================= */

module Plugins {

    export class FileInputHelper {

        public static isFileApiSupported(): boolean {
            return (<any>window).FileReader !== undefined
                && (<any>window).FormData !== undefined;
        }

        public static getFilenameFromUrl(url: string): string {
            var matches = new RegExp("\/?([^/]+)$").exec(url.replace(/\\/gi, "/"));

            if (matches.length <= 1)
                return null;

            return matches[1];
        }
    }

    export interface FileInputConfig {
        uploadButton: any; // selector or jquery object
        removeButton: any; // selector or jquery object
        filename: any; // selector or jquery object
        icon: any; // selector or jquery object
        downloadLink: any; // selector or jquery object
        selectedContainer: any; // selector or jquery object
        emptyContainer: any; // selector or jquery object
        emptyDroppableContainer: any; // selector or jquery object
        emptyNoDroppableContainer: any; // selector or jquery object
        isEmptyInput: any; // selector or jquery object
        drop: boolean;
        onDropped: (File, FileInput) => boolean;
        onValidating: (File, FileInput) => any;
        onSelected: (FileInput) => void;
        onSelectedInvalid: (FileInputValidationResult) => void;
        onRemoved: (FileInput) => void;
        acceptExtensionsAttribute: string;
        acceptExtensions: string;
        maxSizeAttribute: string;
        maxSize: number;
    }

    export interface FileInputValidationResult {
        fileInput: FileInput;
        file: File;
        validSize: boolean;
        validExtension: boolean;
        customValidation: any;
    }

    export class FileInput {

        private onSelected = new Helpers.Event();
        Selected(): Helpers.IEvent { return this.onSelected; }

        onSelectedInvalid = new Helpers.Event();
        SelectedInvalid(): Helpers.IEvent { return this.onSelectedInvalid; }

        private onRemoved = new Helpers.Event();
        Removed(): Helpers.IEvent { return this.onRemoved; }

        // Statics
        public static CONTAINER_EMPTY_CSSCLASS = "fileinput-empty";
        public static CONTAINER_SELECTED_CSSCLASS = "fileinput-selected";
        public static CONFIG: FileInputConfig = {
            uploadButton: "[data-fileinput=upload]",
            removeButton: "[data-fileinput=remove]",
            filename: "[data-fileinput=filename]",
            icon: "[data-fileinput=icon]",
            downloadLink: "[data-fileinput=filename]",
            selectedContainer: "[data-fileinput=selected]",
            emptyContainer: "[data-fileinput=empty],[data-fileupload=empty]", //added [data-fileupload=empty] for backward compatibility
            emptyDroppableContainer: "[data-fileinput=empty-droppable]",
            emptyNoDroppableContainer: "[data-fileinput=empty-nodroppable]",
            isEmptyInput: "[data-fileinput=isempty]",
            drop: FileInputHelper.isFileApiSupported(),
            onDropped: (file, fileinput) => { return true; },
            onValidating: (file, fileinput) => { },
            onSelected: (fileinput) => { },
            onSelectedInvalid: (fileinput) => { },
            onRemoved: (fileinput) => { },
            acceptExtensionsAttribute: "data-val-accept-exts",
            acceptExtensions: null,
            maxSizeAttribute: "data-val-maxsize",
            maxSize: null,
        };

        // Private Members
        private file: File = null;
        private config: FileInputConfig;
        private $container: any;
        private $uploadButton: any;
        private $removeButton: any;
        private $filename: any;
        private icon: FileIcon;
        private $icon: any;
        private $downloadLink: any;
        private $selectedContainer: any;
        private $emptyContainer: any;
        private $emptyDroppableContainer: any;
        private $emptyNoDroppableContainer: any;
        private $isEmptyInput: any;
        private url: string = null;
        private isEmpty: boolean;

        constructor(
            container: any, // preferably a form
            config: FileInputConfig) {
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

        initUrl(url: string) {
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
        }

        /* Initialize accept attribute in order to filter files by mime type in the picker */
        private initMimeTypes() {

            if (window.MimeType) { // mimetype.js dependency
                var $fileInput = this.getFileInput();
                // If data-val-accept-exts is defined and accept attribute is empty,
                // convert extensions to mime types
                var exts = this.getAcceptExtensions();
                if (exts && !$fileInput.attr("accept")) {

                    var mimes = [];
                    $.each(exts.split(","), (i, x) => {
                        var mime = window.MimeType.lookup(x);
                        if (mime)
                            mimes.push(mime);
                    });
                    $fileInput.attr("accept", mimes.join(", "));
                }
            }
        }

        private bindEvents() {
            // Bind file input change
            this.$container.on("change", this.getFileInput().selector, () => {
                this.handleFileInputChanged();
            });

            // Bind file input disabled change
            this.$container.on("custom.disabled", this.getFileInput().selector, (e, _, disabled) => {
                this.disable(disabled);
            });

            // Bind upload button
            this.$container.on("click", this.config.uploadButton, () => {
                this.showPicker();
            });

            // Bind remove button
            this.$container.on("click", this.config.removeButton, () => {
                this.remove();
            });

            // Bind drop
            if (this.config.drop) {
                this.$container
                    .bind('drop', (e) => {
                        this.handleDrop(e);
                        return false;
                    })
                    .bind('dragover', (e) => { // Prevent that the browser open the file
                        e.preventDefault(); return false;
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
        }

        getFileInput(): any {
            return this.$container.find("[type=file]:first");
        }

        getFile(): File {
            return this.file;
        }

        private setFile(file: File) {
            return this.file = file;
        }

        showPicker() {
            this.getFileInput().click();
        }

        handleFileInputChanged() {
            var $fileInput = this.getFileInput();
            var file = $fileInput[0].files[0];


            if (!this.isValid(file)) {
                this.resetFileInput();
                return;
            }
            this.select(file);
        }

        handleDrop(e: DragEvent) {
            e.preventDefault();

            if (this.isDisabled())
                return;

            if (e.dataTransfer.files.length == 0)
                return;

            var file: File = e.dataTransfer.files[0];

            if (!this.isValid(file))
                return false;

            if (this.config.onDropped(file, this) !== false)
                this.select(file);
        }

        isValid(file: File): boolean {
            var result: FileInputValidationResult = {
                fileInput: this,
                file: file,
                validExtension: this.hasValidExtension(file),
                validSize: this.hasValidSize(file),
                customValidation: this.config.onValidating(file, this)
            };
            if (result.validExtension
                && result.validSize
                && (result.customValidation === null
                    || result.customValidation === undefined
                    || result.customValidation === true)) {
                return true;
            } else {
                this.onSelectedInvalid.trigger(result);
                return false;
            }
        }

        private hasValidExtension(file: File): boolean {
            var exts = this.getAcceptExtensions();
            if (!exts) return true;

            var regex = new RegExp("\." + exts.replace(/,/g, "|") + "$", "i")
            return regex.test(file.name);
        }

        private getAcceptExtensions(): string {
            return this.getFileInput().attr(this.config.acceptExtensionsAttribute)
                || this.config.acceptExtensions
                || null;
        }

        private hasValidSize(file: File): boolean {
            var maxSize = this.getMaxSize();
            return !maxSize
                || !file.size
                || file.size <= maxSize;
        }

        private getMaxSize(): number {
            return this.getFileInput().attr(this.config.maxSizeAttribute)
                || this.config.maxSize
                || null;
        }

        /* Show selected file name and download link */
        select(file: File, filename: string = null) {
            this.setFile(file);

            this.setFilename(filename || this.getFile().name);

            this.getFileUrlAsync((url) => {
                this.setUrl(url);
            });

            this.toggleSelectedAndEmptyStyles(true);

            this.onSelected.trigger(this);
        }

        remove() {
            this.clear();
            this.onRemoved.trigger(this);
        }

        /* Clear file input and show upload */
        clear() {
            this.setFile(null);
            this.resetFileInput();

            this.setFilename(null);
            this.setUrl(null)

            this.toggleSelectedAndEmptyStyles(false);
        }

        disable(disabled) {
            this.getFileInput().disable(disabled);
        }

        isDisabled() {
            return this.getFileInput().is(":disabled");
        }

        getUrl(): string {
            return this.url;
        }

        setUrl(url: string) {
            this.url = url;
            this.$downloadLink.attr("href", url);
            this.setIsEmpty(this.url == null);
        }

        getFilename(): string {
            return $.trim(this.$filename.html());
        }

        getIsEmpty(): boolean {
            return this.isEmpty;
        }

        private setIsEmpty(val: boolean) {
            this.isEmpty = val;
            this.$isEmptyInput.val(val);
        }

        private setFilename(val: string) {
            this.$filename.html(val);
            this.icon.setFilename(val);
        }

        resetFileInput() {
            var $fileInput = this.getFileInput();
            $fileInput.wrap('<form>').closest('form').get(0).reset();
            $fileInput.unwrap();
        }

        public getFileUrlAsync(onload: (string) => void) {
            var reader = new FileReader();
            reader.onload = (event) => {
                onload.call(this, event.target.result);
            };
            reader.readAsDataURL(this.getFile());
        }

        private toggleSelectedAndEmptyStyles(selected: boolean) {
            this.toggleSelectedStyle(selected);
            this.toggleEmptyStyle(!selected);
        }

        public toggleSelectedStyle(selected: boolean) {
            this.$container.toggleClass(FileInput.CONTAINER_SELECTED_CSSCLASS, selected);
            this.$selectedContainer.toggle(selected);
        }

        private toggleEmptyStyle(empty: boolean) {
            this.$container.toggleClass(FileInput.CONTAINER_EMPTY_CSSCLASS, empty);
            this.$emptyContainer.toggle(empty);
            if (this.config.drop) {
                this.$emptyDroppableContainer.toggle(empty);
                this.$emptyNoDroppableContainer.toggle(false);
            } else {
                this.$emptyDroppableContainer.toggle(false);
                this.$emptyNoDroppableContainer.toggle(empty);
            }
        }
    }

    export class FileInputLegacy extends FileInput {

        public handleFileInputChanged() {
            var $fileInput = this.getFileInput();
            if (!$fileInput.val()) return;

            var filename = FileInputHelper.getFilenameFromUrl($fileInput.val());
            if (!filename) return;

            if (!this.isValid(<File>{ name: filename })) {
                this.resetFileInput();
                return;
            }
            this.select(null, filename);
        }

        showPicker() {
            throw "This operation is not supported in your browser";
        }

        public getFileUrlAsync(onload: (string) => void ) {
            return;
        }
    }
}


(function ($, window) {

    /* PLUGIN DEFINITION
     * ================= */

    jQuery.fn.fileinput = function (option) {

        return this.each(function (i, elem) {

            // Use received option or data-api as config
            var config = typeof option == 'object' && option
                || $(this).data("fileinput-apply")
                || {};

            // Initialize instance if necessary
            var instance = $(this).data("fileinput");
            if (!instance) {
                $(this).data("fileinput",
                    instance = Plugins.FileInputHelper.isFileApiSupported()
                    ? new Plugins.FileInput(this, config)
                    : new Plugins.FileInputLegacy(this, config)
                    );
            }

            // Apply instance function
            if (typeof option == 'string')
                instance[option]()

        });

    };

    /* DATA-API INITIALIZATION
     * ======================= */

    jQuery.fn.fileinput.dataApi_init = function () {
        $("[data-fileinput-apply]").fileinput();
    }
    $(document).ready(
        jQuery.fn.fileinput.dataApi_init
    );

    /* DATA-API DROP
     * ============= */

    $.event.props.push('dataTransfer');

    $(document)
        .on("drop", "[data-fileinput=drop][data-target]", function (e) {
            var target = $(this).data("target");
            var fileinput = <Plugins.FileInput>$(target).data("fileinput");
            if (fileinput) fileinput.handleDrop(e);
            return false;
        })
        .on("dragover", "[data-fileinput=drop][data-target]", function (e) {
            // Prevent that the browser open the file
            e.preventDefault(); return false;
        });

    /* HANDLE FORM/INPUT CLEARED EVENT
     * =============================== */

    $(document).ready(function () {
        var selector = "." + Plugins.FileInput.CONTAINER_SELECTED_CSSCLASS;
        // Clear element when parent form is resetted
        $(document).on("reset", "form", function (e) {
            $(this).find(selector)
                .each(function () {
                    $(this).data("fileinput").remove();
                });
        });
        // Clear element when element is cleared
        // Dependecy: clearForm plugin
        if ($.fn.clearForm) {
            $(document).on("cleared", selector,
                function (e, elem, defaultValue) {
                    $(this).data("fileinput").remove();
                });
        }
    });

})(jQuery, window);