///<reference path='Helpers/Notifications.ts'/>
///<reference path='Helpers/Api.ts'/>
///<reference path="Helpers/Numbers.ts" />

var App = (function () {
    function App(config) {
        this.config = null;
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
    App.prototype.initMenu = function () {
        $('ul.dropdown-menu>li').find('a').mouseout(function (e) {
            $(e.target).blur();
        }).filter('a[href="#"]').click(function (e) {
            e.preventDefault();
            e.stopPropagation();
        });
    };

    App.prototype.initGlobalize = function () {
        Globalize.culture(this.config.lang);
    };

    App.prototype.initNotifications = function () {
        this.notifications = new Helpers.Notifications();

        // Show initials notifications
        if (typeof notifications_init !== "undefined" && notifications_init.length)
            for (var i = 0; i < notifications_init.length; i++) {
                this.notifications.show(notifications_init[i].data, notifications_init[i].level);
            }
    };

    App.prototype.initChainSelectForContainer = function ($container) {
        $container.chainSelect();
    };

    App.prototype.initNumbers = function () {
        this.currency = new Helpers.Numbers(this.config.numbersFormat.currencyDecimals);
    };

    App.prototype.initNumerics = function ($parent) {
        var _this = this;
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
            numerics = $parent.find('[data-numeric]').each(function (i, elem) {
                var decimal = $(elem).data("numeric-decimal") !== false;
                if (decimal)
                    decimal = _this.config.numbersFormat.decimalSep;
                var negative = $(elem).data("numeric-negative") !== false;
                $(elem).numeric({
                    decimal: decimal ? _this.config.numbersFormat.decimalSep : false,
                    negative: negative
                });
            });
        }

        numerics.off('focus.numeric,blur.numeric').on('focus.numeric', function (e) {
            if (e.target.value)
                e.target.value = _this.currency.formatNumberNoThousands(_this.currency.parse(e.target.value));
        }).on('blur.numeric', function (e) {
            if (e.target.value)
                e.target.value = _this.currency.format(_this.currency.parse(e.target.value));
        });

        var $forms = numerics.closest('form');

        $forms.filter('[data-api-controller]').on('beforeSerialize.numeric', function (e) {
            $(e.target).find('[data-numeric]').each(function (i, e) {
                if (e.value)
                    e.value = _this.currency.formatNumberNoThousands(_this.currency.parse(e.value));
            });
        }).on('afterSubmit.numeric', function (e) {
            $(e.target).find('[data-numeric]').each(function (i, e) {
                if (e.value)
                    e.value = _this.currency.parseAndFormat(e.value);
            });
        });

        $forms.not('[data-api-controller]').on('submit.numeric', function (e) {
            $(e.target).find('[data-numeric]').each(function (i, e) {
                if (e.value)
                    e.value = _this.currency.formatNumberNoThousands(_this.currency.parse(e.value));
            });
        }).on('invalid-form.numeric', function (e) {
            $(e.target).find('[data-numeric]').each(function (i, e) {
                if (e.value)
                    e.value = _this.currency.parseAndFormat(e.value);
            });
        });
    };

    App.prototype.initApi = function () {
        var _this = this;
        Helpers.Api.overrideSettings({
            baseUrl: this.config.baseUrl + "api/",
            baseMvcUrl: this.config.baseUrl,
            notifications: new Helpers.Notifications(),
            undefinedException: function (settings, xhr, textStatus, errorThrown) {
                var _settings = settings;
                if (_settings.notifications)
                    _this.evaluate(_settings.notifications).showError("An unexpected error has ocurred");
            },
            businessException: function (settings, ex) {
                var _settings = settings;
                if (!_settings.notifications || !ex) {
                    return;
                }
                var notifications = _this.evaluate(_settings.notifications);
                if (ex.Messages && ex.Messages.length > 0) {
                    for (var i = 0; i < ex.Messages.length; i++)
                        notifications.showError(ex.Messages[i]);
                } else {
                    notifications.showError(ex.Message ? ex.Message : ex);
                }
            },
            unauthorizedException: function (settings, ex) {
                window.location.href = window.location.href.replace(window.location.pathname, '') + '/' + _this.config.WebSite.Login.LoginUrl.replace("{returnUrl}", window.location.href);
            },
            notImplementedException: function (settings, ex) {
                var _settings = settings;
                if (_settings.notifications)
                    _this.evaluate(_settings.notifications).showError("This feature is under construction");
            }
        });

        this.api = new Helpers.Api({
            notifications: this.notifications
        });
    };

    App.prototype.initForms = function ($parent, notifications) {
        var _this = this;
        $parent = $parent || $(document);
        $parent.find("form[data-api-controller]").each(function (i, form) {
            var $form = $(form);

            var beforeSubmitHandler = null;
            if ($form.data('before-submit')) {
                beforeSubmitHandler = function () {
                    _this.executeFunctionByName($form.data('before-submit'), window);
                };
            }

            var afterSubmitHandler = null;
            if ($form.data('after-submit')) {
                afterSubmitHandler = function () {
                    _this.executeFunctionByName($form.data('after-submit'), window);
                };
            }

            //remove submit handler to prevent multiple submits
            $form.off('submit');

            if (notifications)
                $form.data('notifications', notifications);

            _this.api.bindForm($form, $form.data("api-controller"), {
                action: $form.data("api-action"),
                beforeSerialize: function ($form, options) {
                    _this.prepareDisabledInputsToSubmit($form);
                    setTimeout(function () {
                        _this.deleteHiddenGeneratedInputValues($form);
                    }, 1000);
                    $form.trigger('beforeSerialize', [$form, options]);
                },
                beforeSubmit: function (arr, $form, options) {
                    ($form.data("notifications") || _this.notifications).clear();
                    $form.trigger('beforeSubmit', [arr, $form, options]);
                },
                complete: function () {
                    $form.trigger('afterSubmit');
                },
                success: function (data) {
                    if ($form.data("success"))
                        eval($form.data("success"));

                    $form.trigger('success', data);
                },
                errorHandler: function (settings, ex) {
                    $form.trigger('invalid-form', [ex]);
                },
                notifications: function () {
                    return ($form.data("notifications") || _this.notifications);
                }
            });
        });
    };

    App.prototype.prepareDisabledInputsToSubmit = function (form) {
        $(":disabled", form).each(function (i, elem) {
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
    };

    App.prototype.exportCurrentView = function (type) {
        var url = this.getCurrentUrl();
        url = url + (url.indexOf("?") == -1 ? "?" : "&") + "export=" + encodeURIComponent(type);

        this.go(url);
    };

    App.prototype.deleteHiddenGeneratedInputValues = function (form) {
        $(form).find('.hiddenValue').remove();
    };

    App.prototype.initTypeahead = function ($parent) {
        var _this = this;
        $parent = $parent || $(document);
        $parent.find('[data-provide=typeahead][data-source-controller]').each(function (i, elem) {
            var $elem = $(elem).attr("autocomplete", "off");
            $elem.typeahead({
                items: $elem.data("items"),
                source: function (query, process) {
                    var customFilterCallback = $elem.data('custom-filter-callback');
                    var customFilterSetCallback = $elem.data('custom-filter-set-callback');

                    if (customFilterCallback) {
                        var customFilters = _this.executeFunctionByName(customFilterCallback, window);
                    }

                    return _this.api.get($elem.data("source-controller"), {
                        action: $elem.data("source-action") || null,
                        block: function (settings) {
                            return false;
                        },
                        data: {
                            query: query,
                            items: $elem.data("items"),
                            filters: customFilters || null
                        },
                        success: function (data) {
                            if (customFilterSetCallback) {
                                _this.executeFunctionByName(customFilterSetCallback, window, data);
                            }
                            return process(data);
                        },
                        error: function (jqXHR, textStatus, errorThrow) {
                        }
                    });
                },
                updater: function (item) {
                    console.log(item);
                }
            });
        });
    };

    App.prototype.initSelect2 = function ($parent) {
        var _this = this;
        $('[data-provide=select2]').each(function (i, elem) {
            var $elem = $(elem);

            // Avoid re-init
            if ($elem.data("select2") !== undefined && $elem.data("select2") !== null)
                return;

            // Remove fake container
            $elem.prev(".select2-container").remove();

            var options = {
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
                    options.createSearchChoice = function (term) {
                        if (term != "" && $elem.data('unconstrained-element')) {
                            var $relatedElem = $($elem.data('unconstrained-element'));
                            $relatedElem.val(term);
                            $relatedElem.trigger('change');
                            return { id: "", text: term };
                        } else if (term == "" && allowClear) {
                            return { id: "", text: "" };
                        } else if ((term != "" && !$elem.data('unconstrained-element')) || (term == "" && !allowClear)) {
                            return null;
                        }
                    };
                }

                options.query = function (query) {
                    var filterCallbackName = $elem.data('custom-filter-callback');
                    var customFilterObject = null;
                    if (filterCallbackName) {
                        customFilterObject = _this.executeFunctionByName(filterCallbackName, window);
                    }

                    _this.api.get($elem.data("source-controller"), {
                        action: $elem.data("source-action") || null,
                        block: function (settings) {
                            return false;
                        },
                        data: {
                            query: query.term,
                            items: $elem.data("items"),
                            filters: customFilterObject
                        },
                        success: function (data) {
                            query.callback({
                                results: $.map(data, function (x) {
                                    return $.extend(x, {
                                        id: x.ID,
                                        text: x.Description
                                    });
                                })
                            });
                        },
                        error: function (jqXHR, textStatus, errorThrow) {
                        }
                    });
                };
            }

            $elem.select2(options);
        });
    };

    App.prototype.initMultipleSelect2 = function ($parent) {
        $('[data-provide=multiple-select2]').each(function (i, elem) {
            var $elem = $(elem);

            // Avoid re-init
            if ($elem.data("multiple-select2") !== undefined && $elem.data("multiple-select2") !== null)
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
                allowClear: allowClear
            });
        });
    };

    App.prototype.initPopOvers = function ($parent) {
        $parent = $parent || $(document);
        $parent.find("a[rel=popover][data-content]").popover().click(function (e) {
            e.preventDefault();
            var $target = $(e.target);
            $parent.find("a[rel=popover][data-group=" + $target.data('group') + "]").not($target).popover('hide');
        });
    };

    App.prototype.executeFunctionByName = function (functionName, context, data) {
        var args = Array.prototype.slice.call(arguments).splice(2);
        var namespaces = functionName.split(".");
        var func = namespaces.pop();
        for (var i = 0; i < namespaces.length; i++) {
            context = context[namespaces[i]];
        }
        return context[func].apply(context, args);
    };

    App.prototype.evaluate = function (val) {
        return $.isFunction(val) ? val() : val;
    };

    App.prototype.getCurrentUrl = function () {
        return window.location.href;
    };

    App.prototype.goBack = function (skipBeforeUnload) {
        if (typeof skipBeforeUnload === "undefined") { skipBeforeUnload = true; }
        this.go(this.config.previousUrl && this.getCurrentUrl() != this.config.previousUrl ? this.config.previousUrl : this.config.baseUrl);
    };

    App.prototype.go = function (url, skipBeforeUnload) {
        if (typeof skipBeforeUnload === "undefined") { skipBeforeUnload = true; }
        window.location.href = url;
    };

    App.prototype.refresh = function (skipBeforeUnload) {
        if (typeof skipBeforeUnload === "undefined") { skipBeforeUnload = true; }
        this.go(window.location.href.replace(/#.*$/, ''));
    };
    return App;
})();
//# sourceMappingURL=App.js.map
