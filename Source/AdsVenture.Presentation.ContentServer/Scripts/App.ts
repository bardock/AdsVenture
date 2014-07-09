///<reference path='Helpers/Notifications.ts'/>
///<reference path='Helpers/Api.ts'/>
///<reference path="Helpers/Numbers.ts" />

declare var notifications_init;

class App {

    config = null;

    notifications: Helpers.Notifications;
    api: Helpers.Api;
    currency: Helpers.Numbers;

    constructor(config) {
        this.config = config;

        $.Mustache.addFromDom();

        this.initGlobalize();

        this.initNotifications();

        this.initNumerics($(document));

        this.initApi();

        this.initForms($(document));

        this.initNumbers();

        this.initTypeahead($(document));

        this.initSelect2($(document));

        this.initMultipleSelect2($(document));

        this.initMenu();

    }

    private initMenu() {
        $('ul.dropdown-menu>li')
            .find('a')
            .mouseout((e: JQueryEventObject) => {
                $(e.target).blur();
            }).filter('a[href="#"]')
            .click((e: JQueryEventObject) => {
                e.preventDefault();
                e.stopPropagation();
            });
    }

    private initGlobalize() {
        Globalize.culture(this.config.lang);
    }

    private initNotifications() {
        this.notifications = new Helpers.Notifications();

        // Show initials notifications
        if (typeof notifications_init !== "undefined" && notifications_init.length)
            for (var i = 0; i < notifications_init.length; i++) {
                this.notifications.show(
                    notifications_init[i].data,
                    notifications_init[i].level
                    );
            }
    }

    public initChainSelectForContainer($container) {
        $container.chainSelect();
    }

    private initNumbers() {
        this.currency = new Helpers.Numbers(
            this.config.numbersFormat.currencyDecimals
            )
    }

    public initNumerics($parent) {
        $parent = $parent || $(document);

        var decimal = $parent.is("[data-numeric-decimal]") && $parent.data("numeric-decimal") !== false;
        var negative = $parent.is("[data-numeric-negative]") && $parent.data("numeric-negative") !== false;

        var numerics = null;
        if (decimal && negative) {
            numerics = $parent.find('[data-numeric]').numeric({
                decimal: decimal ? this.config.numbersFormat.decimalSep : false,
                negative: negative
            });
        } else {
            numerics = $parent.find('[data-numeric]').each((i, elem) => {
                var decimal = $(elem).data("numeric-decimal") !== false;
                if (decimal)
                    decimal = this.config.numbersFormat.decimalSep;
                var negative = $(elem).data("numeric-negative") !== false;
                $(elem).numeric({
                    decimal: decimal ? this.config.numbersFormat.decimalSep : false,
                    negative: negative
                });
            });
        }

        numerics.off('focus.numeric,blur.numeric')
            .on('focus.numeric', (e) => {
                if (e.target.value)
                    e.target.value = this.currency.formatNumberNoThousands(
                        this.currency.parse(e.target.value)
                        );
            }).on('blur.numeric', (e) => {
                if (e.target.value)
                    e.target.value = this.currency.format(
                        this.currency.parse(e.target.value)
                        );
            });

        var $forms = numerics.closest('form');

        $forms.filter('[data-api-controller]')
            .on('beforeSerialize.numeric', (e) => {
                $(e.target).find('[data-numeric]').each((i, e: any) => {
                    if (e.value)
                        e.value = this.currency.formatNumberNoThousands(
                            this.currency.parse(e.value)
                            );
                });
            }).on('afterSubmit.numeric', (e) => {
                $(e.target).find('[data-numeric]').each((i, e: any) => {
                    if (e.value)
                        e.value = this.currency.parseAndFormat(e.value);
                });
            });

        $forms.not('[data-api-controller]')
            .on('submit.numeric', (e) => {
                $(e.target).find('[data-numeric]').each((i, e: any) => {
                    if (e.value)
                        e.value = this.currency.formatNumberNoThousands(
                            this.currency.parse(e.value)
                            );
                })
            }).on('invalid-form.numeric', (e) => {
                $(e.target).find('[data-numeric]').each((i, e: any) => {
                    if (e.value)
                        e.value = this.currency.parseAndFormat(e.value)
                });
            });
    }

    private initApi() {
        Helpers.Api.overrideSettings(<CustomApiSettings>{
            baseUrl: this.config.baseUrl + "api/",
            baseMvcUrl: this.config.baseUrl,
            notifications: new Helpers.Notifications(),
            undefinedException: (settings, xhr, textStatus, errorThrown) => {
                var _settings = <CustomApiSettings>(<any>settings);
                if (_settings.notifications)
                    this.evaluate<Helpers.Notifications>(_settings.notifications).showError("An unexpected error has ocurred");
            },
            businessException: (settings, ex) => {
                var _settings = <CustomApiSettings>(<any>settings);
                if (!_settings.notifications || !ex) {
                    return;
                }
                var notifications = this.evaluate<Helpers.Notifications>(_settings.notifications);
                if (ex.Messages && ex.Messages.length > 0) {
                    for (var i = 0; i < ex.Messages.length; i++)
                        notifications.showError(ex.Messages[i]);
                } else {
                    notifications.showError(ex.Message ? ex.Message : ex);
                }
            },
            unauthorizedException: (settings, ex) => {
                window.location.href = window.location.href.replace(window.location.pathname, '') + '/' + this.config.WebSite.Login.LoginUrl.replace("{returnUrl}", window.location.href);
            },
            notImplementedException: (settings, ex) => {
                var _settings = <CustomApiSettings>(<any>settings);
                if (_settings.notifications)
                    this.evaluate<Helpers.Notifications>(_settings.notifications).showError("This feature is under construction");
            }
        });

        this.api = new Helpers.Api(<CustomApiSettings>{
            notifications: this.notifications
        });
    }

    public initForms($parent, notifications?: Helpers.Notifications) {
        $parent = $parent || $(document);
        $parent.find("form[data-api-controller]").each((i, form) => {
            var $form = $(form);

            var beforeSubmitHandler = null;
            if ($form.data('before-submit')) {
                beforeSubmitHandler = () => {
                    this.executeFunctionByName($form.data('before-submit'), window);
                }
            }

            var afterSubmitHandler = null;
            if ($form.data('after-submit')) {
                afterSubmitHandler = () => {
                    this.executeFunctionByName($form.data('after-submit'), window);
                }
            }

            //remove submit handler to prevent multiple submits
            $form.off('submit');

            if (notifications)
                $form.data('notifications', notifications);

            this.api.bindForm($form, $form.data("api-controller"), <Helpers.FormApiSettings>{
                action: $form.data("api-action"),
                beforeSerialize: ($form, options) => {
                    this.prepareDisabledInputsToSubmit($form);
                    setTimeout(() => { this.deleteHiddenGeneratedInputValues($form); }, 1000);
                    $form.trigger('beforeSerialize', [$form, options]);
                },
                beforeSubmit: (arr, $form, options) => {
                    ($form.data("notifications") || this.notifications).clear();
                    $form.trigger('beforeSubmit', [arr, $form, options]);
                },
                complete: () => {
                    $form.trigger('afterSubmit');
                },
                success: (data) => {
                    if ($form.data("success"))
                        eval($form.data("success"));

                    $form.trigger('success', data)
                },
                errorHandler: (settings, ex) => {
                    $form.trigger('invalid-form', [ex]);
                },
                notifications: () => { return ($form.data("notifications") || this.notifications); }
            });
        });
    }

    private prepareDisabledInputsToSubmit(form) {
        $(":disabled", form).each((i, elem) => {
            var $elem = $(elem);
            var hidden = $('input[type="hidden"][name="' + $elem.attr("name") + '"]', form);
            if (hidden.length == 0) {
                hidden = $("<input>", {
                    type: "hidden",
                    name: $elem.attr("name")
                }).addClass('hiddenValue').insertAfter($elem);
            }
            if ($elem.is(':checkbox')) {
                hidden.val($elem.prop('checked'));
            } else {
                hidden.val($elem.val());
            }
        });
    }

    exportCurrentView(type) {
        var url = this.getCurrentUrl();
        url = url
        + (url.indexOf("?") == -1 ? "?" : "&")
        + "export="
        + encodeURIComponent(type);

        this.go(url);
    }

    public deleteHiddenGeneratedInputValues(form) {
        $(form).find('.hiddenValue').remove();
    }

    public initTypeahead($parent) {
        $parent = $parent || $(document);
        $parent.find('[data-provide=typeahead][data-source-controller]').each((i, elem) => {
            var $elem = $(elem).attr("autocomplete", "off");
            $elem.typeahead({
                items: $elem.data("items"),
                source: (query, process) => {
                    var customFilterCallback = $elem.data('custom-filter-callback');
                    var customFilterSetCallback = $elem.data('custom-filter-set-callback');

                    if (customFilterCallback) {
                        var customFilters = this.executeFunctionByName(customFilterCallback, window);
                    }

                    return this.api.get($elem.data("source-controller"), <Helpers.ApiSettings>{
                        action: $elem.data("source-action") || null,
                        block: (settings) => false,
                        data: {
                            query: query,
                            items: $elem.data("items"),
                            filters: customFilters || null
                        },
                        success: (data) => {
                            if (customFilterSetCallback) {
                                this.executeFunctionByName(customFilterSetCallback, window, data);
                            }
                            return process(data);
                        },
                        error: (jqXHR: JQueryXHR, textStatus: string, errorThrow: string) => { }
                    });
                },
                updater: (item) => {
                    console.log(item);
                }
            });
        });
    }

    public initSelect2($parent) {

        $('[data-provide=select2]').each((i, elem) => {

            var $elem = $(elem);

            // Avoid re-init
            if ($elem.data("select2") !== undefined
                && $elem.data("select2") !== null)
                return;

            // Remove fake container
            $elem.prev(".select2-container").remove();

            var options: Select2Options = {
                dropdownCssClass: $elem.data('dropdowncssclass'),
                placeholder: $elem.data('placeholder')
            };

            if ($elem.data("source-controller")) {

                options.minimumInputLength = 1;
                options.formatInputTooShort = null;
                //options.formatInputTooLong = null;
                options.allowClear = allowClear;

                options.initSelection = function (element, callback) {
                    callback(undefined);
                };

                var allowClear = $elem.data('allow-clear') !== false;
                if ($elem.data('unconstrained-element') || allowClear) {
                    options.createSearchChoice = (term) => {
                        if (term != "" && $elem.data('unconstrained-element')) {
                            var $relatedElem = $($elem.data('unconstrained-element'));
                            $relatedElem.val(term);
                            $relatedElem.trigger('change');
                            return { id: "", text: term };
                        } else if (term == "" && allowClear) {
                            return { id: "", text: "" };
                        } else if ((term != "" && !$elem.data('unconstrained-element')) || (term == "" && !allowClear)) {
                            return null
                        }
                    }
                }

                options.query = (query) => {

                    var filterCallbackName = $elem.data('custom-filter-callback');
                    var customFilterObject = null;
                    if (filterCallbackName) {
                        customFilterObject = this.executeFunctionByName(filterCallbackName, window);
                    }

                    this.api.get($elem.data("source-controller"), <Helpers.ApiSettings>{
                        action: $elem.data("source-action") || null,
                        block: (settings) => false,
                        data: {
                            query: query.term,
                            items: $elem.data("items"),
                            filters: customFilterObject
                        },
                        success: (data) => {
                            query.callback({
                                results: $.map(data, function (x) {
                                    return $.extend(x, {
                                        id: x.ID,
                                        text: x.Description
                                    });
                                })
                            });
                        },
                        error: function (jqXHR: JQueryXHR, textStatus: string, errorThrow: string) { }
                    });
                };
            }

            $elem.select2(options);
        });
    }

    public initMultipleSelect2($parent) {

        $('[data-provide=multiple-select2]').each((i, elem) => {

            var $elem = $(elem);

            // Avoid re-init
            if ($elem.data("multiple-select2") !== undefined
                && $elem.data("multiple-select2") !== null)
                return;


            //var updateCallback = $elem.data("custom-update-callback");
            //if (updateCallback) {
            //    $elem.on('change', (e) =>
            //        this.executeFunctionByName(updateCallback, window,e)
            //        );
            //}

            // Remove fake container
            $elem.prev(".select2-container").remove();

            //var createSearchChoiceHandler = null
            var allowClear = $elem.data('allow-clear') !== false;

            // Init
            $elem.select2({

                minimumInputLength: 1,

                formatInputTooShort: null,
                formatInputTooLong: null,

                placeholder: $elem.data('placeholder'),
                allowClear: allowClear,



            });
        });
    }

    public initPopOvers($parent) {
        $parent = $parent || $(document);
        $parent.find("a[rel=popover][data-content]").popover().click((e) => {
            e.preventDefault();
            var $target = $(e.target);
            $parent.find("a[rel=popover][data-group=" + $target.data('group') + "]").not($target).popover('hide');
        });
    }

    private executeFunctionByName(functionName, context, data?) {
        var args = Array.prototype.slice.call(arguments).splice(2);
        var namespaces = functionName.split(".");
        var func = namespaces.pop();
        for (var i = 0; i < namespaces.length; i++) {
            context = context[namespaces[i]];
        }
        return context[func].apply(context, args);
    }

    evaluate<T>(val): T {
        return $.isFunction(val) ? val() : val;
    }

    getCurrentUrl() {
        return window.location.href;
    }

    goBack(skipBeforeUnload: boolean = true) {
        this.go(
            this.config.previousUrl
            && this.getCurrentUrl() != this.config.previousUrl
            ? this.config.previousUrl
            : this.config.baseUrl
            );
    }

    go(url: string, skipBeforeUnload: boolean = true) {
        window.location.href = url;
    }

    refresh(skipBeforeUnload: boolean = true) {
        this.go(window.location.href.replace(/#.*$/, ''));
    }
}

interface CustomApiSettings extends Helpers.ApiSettings {
    notifications: Helpers.Notifications;
}