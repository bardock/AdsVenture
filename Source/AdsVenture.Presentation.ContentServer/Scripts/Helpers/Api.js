///<reference path='../Includes.ts'/>
var Helpers;
(function (Helpers) {
    var Api = (function () {
        function Api(settings) {
            if (typeof settings === "undefined") { settings = null; }
            this.settings = $.extend({}, Api._SETTINGS_DEFAULT, settings);
        }
        Api.overrideSettings = function (settings) {
            $.extend(Api._SETTINGS_DEFAULT, settings);
        };

        Api.prototype.getUrl = function (controller, settings) {
            if (typeof settings === "undefined") { settings = null; }
            settings = settings || {};
            var url = this.settings.baseUrl + controller;
            if (settings.action)
                url += ("/action/" + settings.action);
            if (settings.id)
                url += ("/" + settings.id);
            return url;
        };

        Api.prototype.getMvcUrl = function (action, controller, settings) {
            if (typeof settings === "undefined") { settings = null; }
            settings = settings || {};
            var url = this.settings.baseMvcUrl + controller + "/" + action;
            if (settings.id)
                url += ("/" + settings.id);
            return url;
        };

        Api.prototype.buildErrorHandler = function (settings) {
            return function (xhr, textStatus, errorThrown) {
                var exception = typeof xhr.responseText == "string" ? xhr.responseText : errorThrown;
                try  {
                    exception = JSON.parse(xhr.responseText);
                } catch (ex) {
                }
                if (xhr.status == 409)
                    settings.businessException(settings, exception);
                else if (xhr.status == 401)
                    settings.unauthorizedException(settings, exception);
                else if (xhr.status == 501)
                    settings.notImplementedException(settings, exception);
                else
                    settings.undefinedException(settings, xhr, textStatus, errorThrown);

                if (settings.errorHandler)
                    settings.errorHandler(settings, exception);
            };
        };

        Api.prototype.buildAjaxSettings = function (settings) {
            settings = settings || {};

            // Get settings overriding defaults with received one
            settings = $.extend({}, this.settings, settings);

            if (!settings.error)
                settings.error = this.buildErrorHandler(settings);

            // Save default 'complete' callback
            var success = settings.success;
            var complete = settings.complete;
            var beforeSend = settings.beforeSend;

            var isBlockEnabled = typeof (settings.block) == "function" && settings.block(settings);

            // Add error, beforeSend and complete callback
            settings = $.extend(settings, {
                beforeSend: function (jqXHR, stt) {
                    // block ui
                    if (isBlockEnabled)
                        settings.block(settings);

                    // trigger default complete
                    if (beforeSend)
                        beforeSend(jqXHR, stt);
                },
                success: function (data) {
                    if (settings.redirectOnSuccess) {
                        window.location = data;
                    } else if (success) {
                        success(data);
                    }
                },
                complete: function () {
                    // unblock ui
                    if (isBlockEnabled)
                        settings.unblock(settings);

                    // trigger default complete
                    if (complete)
                        complete();
                }
            });

            return settings;
        };

        Api.prototype.getSettings = function (controller, settings, method) {
            if (typeof method === "undefined") { method = "POST"; }
            if (typeof (settings) != "object")
                settings = {};

            settings = $.extend({
                url: !settings.url ? this.getUrl(controller, settings) : settings.url,
                type: method
            }, settings);

            return this.buildAjaxSettings(settings);
        };

        Api.prototype.ajax = function (settings) {
            return $.ajax(this.getSettings(null, settings));
        };

        Api.prototype.call = function (controller, settings, method) {
            return $.ajax(this.getSettings(controller, settings, method));
        };

        Api.prototype.post = function (controller, settings) {
            return this.call(controller, settings);
        };

        Api.prototype.get = function (controller, settings) {
            return this.call(controller, $.extend({ type: 'GET' }, settings));
        };

        Api.prototype.delete = function (controller, settings) {
            return this.call(controller, $.extend({ type: 'DELETE' }, settings));
        };

        Api.prototype.put = function (controller, settings) {
            return this.call(controller, $.extend({ type: 'PUT' }, settings));
        };

        Api.prototype.getHtmlSettings = function (action, controller, settings) {
            if (typeof (settings) != "object")
                settings = {};

            settings = $.extend({ type: 'GET' }, settings);

            if (!settings.dataType)
                settings.dataType = 'html';

            settings.url = this.getMvcUrl(action, controller, settings);

            return this.buildAjaxSettings(settings);
        };

        Api.prototype.getHtml = function (action, controller, settings) {
            return $.ajax(this.getHtmlSettings(action, controller, settings));
        };

        Api.prototype.bindForm = function (selector, controller, settings) {
            settings = settings || {};
            settings.url = Boolean($(selector).data('use-mvc')) == true ? this.getMvcUrl(settings.action, controller, settings) : this.getUrl(controller, settings);

            var method = $(selector).attr("method");
            if (method && !settings.type)
                settings.type = method;

            var redirectOnSuccess = $(selector).data('redirect');
            var _beforeSubmit = settings.beforeSubmit;
            var _afterSubmit = settings.afterSubmit;
            var _success = settings.success;

            settings.beforeSubmit = function (formData, jqForm, options) {
                $.validator.unobtrusive.parse(jqForm);
                if (!jqForm.valid())
                    return false;

                if (_beforeSubmit)
                    return _beforeSubmit(formData, jqForm, options);

                return true;
            };

            settings.afterSubmit = function () {
                if (_afterSubmit) {
                    _afterSubmit();
                }
            };

            settings.success = function (data) {
                if (redirectOnSuccess) {
                    if (typeof (redirectOnSuccess) == 'string') {
                        if (_success) {
                            _success(data);
                        }
                        window.location = data;
                    } else {
                        window.location = data;
                    }
                } else {
                    _success(data);
                }
            };
            var $form = $(selector);
            $form.ajaxForm(this.buildAjaxSettings(settings));
        };
        Api._SETTINGS_DEFAULT = {
            type: 'POST',
            dataType: 'json',
            redirectOnSuccess: false,
            showLoading: true
        };
        return Api;
    })();
    Helpers.Api = Api;
})(Helpers || (Helpers = {}));
//# sourceMappingURL=Api.js.map
