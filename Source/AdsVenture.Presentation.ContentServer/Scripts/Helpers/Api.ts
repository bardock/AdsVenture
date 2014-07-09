///<reference path='../Includes.ts'/>

module Helpers {

    export interface ApiSettings extends JQueryAjaxSettings {
        baseUrl?: string;
        baseMvcUrl?: string;
        action?: string;
        id?: number;
        undefinedException?: (settings: ApiSettings, xhr: any, textStatus: any, errorThrown: string) => any;
        businessException?: (settings: ApiSettings, ex: any) => any;
        unauthorizedException?: (settings: ApiSettings, ex: any) => any;
        notImplementedException?: (settings: ApiSettings, ex: any) => any;
        complete?: () => any;
        beforeSend?: (jqXHR: any, stt: any) => any;
        block?: (settings: ApiSettings) => any;
        unblock?: (settings: ApiSettings) => any;
        redirectOnSuccess?: boolean;
        success?: (data: any) => any;
        errorHandler?: (settings, ex) => any; //use promise
        showLoading?: any;
    }

    export interface FormApiSettings extends ApiSettings {
        beforeSerialize?: ($form, options) => any;
        beforeSubmit?: (arr, $form, options) => any;
        afterSubmit?: () => any;
        notifications?: () => Helpers.Notifications;
    }

    export class Api {
        private static _SETTINGS_DEFAULT: ApiSettings = {
            type: 'POST',
            dataType: 'json',
            redirectOnSuccess: false,
            showLoading: true
        };

        private settings: ApiSettings;

        constructor(settings: ApiSettings = null) {
            this.settings = $.extend({}, Api._SETTINGS_DEFAULT, settings);
        }

        static overrideSettings(settings: ApiSettings) {
            $.extend(Api._SETTINGS_DEFAULT, settings);
        }

        getUrl(controller, settings: ApiSettings = null) {
            settings = settings || {};
            var url = this.settings.baseUrl + controller;
            if (settings.action)
                url += ("/action/" + settings.action);
            if (settings.id)
                url += ("/" + settings.id);
            return url;
        }

        getMvcUrl(action, controller, settings: ApiSettings = null) {
            settings = settings || {};
            var url = this.settings.baseMvcUrl + controller + "/" + action;
            if (settings.id)
                url += ("/" + settings.id);
            return url;
        }

        private buildErrorHandler(settings: ApiSettings): (jqXHR: JQueryXHR, textStatus: string, errorThrow: string) => any {
            return (xhr, textStatus, errorThrown) => {
                var exception = typeof xhr.responseText == "string" ? xhr.responseText : errorThrown;
                try { exception = JSON.parse(xhr.responseText); } catch (ex) { }
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
            }
        }

        private buildAjaxSettings(settings: ApiSettings) {
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
                beforeSend: (jqXHR, stt) => {
                    // block ui
                    if (isBlockEnabled)
                        settings.block(settings);
                    // trigger default complete
                    if (beforeSend)
                        beforeSend(jqXHR, stt);
                },
                success: (data: any) => {
                    if (settings.redirectOnSuccess) {
                        window.location = data;
                    } else if (success) {
                        success(data);
                    }
                },
                complete: () => {
                    // unblock ui
                    if (isBlockEnabled)
                        settings.unblock(settings);
                    // trigger default complete
                    if (complete)
                        complete();
                }
            });

            return settings;
        }

        getSettings(controller: string, settings: ApiSettings, method: string = "POST"): ApiSettings {
            if (typeof (settings) != "object")
                settings = {};

            settings = $.extend({
                url: !settings.url
                ? this.getUrl(controller, settings)
                : settings.url,
                type: method
            }, settings);

            return this.buildAjaxSettings(settings);
        }

        ajax(settings: ApiSettings) {
            return $.ajax(
                this.getSettings(null, settings)
                );
        }

        call(controller: string, settings: ApiSettings, method?: string) {
            return $.ajax(
                this.getSettings(controller, settings, method)
                );
        }

        post(controller: string, settings: ApiSettings) {
            return this.call(controller, settings);
        }

        get(controller: string, settings: ApiSettings) {
            return this.call(controller, $.extend({ type: 'GET' }, settings));
        }

        delete(controller: string, settings: ApiSettings) {
            return this.call(controller, $.extend({ type: 'DELETE' }, settings));
        }

        put(controller: string, settings: ApiSettings) {
            return this.call(controller, $.extend({ type: 'PUT' }, settings));
        }

        getHtmlSettings(action: string, controller: string, settings: ApiSettings) {
            if (typeof (settings) != "object")
                settings = {};

            settings = $.extend({ type: 'GET' }, settings)

            if (!settings.dataType)
                settings.dataType = 'html';

            settings.url = this.getMvcUrl(action, controller, settings);

            return this.buildAjaxSettings(settings);
        }

        getHtml(action: string, controller: string, settings: ApiSettings) {
            return $.ajax(
                this.getHtmlSettings(action, controller, settings)
                );
        }

        bindForm(selector: any, controller: string, settings: FormApiSettings) {
            settings = settings || {};
            settings.url = Boolean($(selector).data('use-mvc')) == true ? this.getMvcUrl(settings.action, controller, settings) : this.getUrl(controller, settings);

            var method = $(selector).attr("method");
            if (method && !settings.type)
                settings.type = method;

            var redirectOnSuccess = $(selector).data('redirect');
            var _beforeSubmit = settings.beforeSubmit;
            var _afterSubmit = settings.afterSubmit;
            var _success = settings.success;

            settings.beforeSubmit = (formData, jqForm, options) => {
                $.validator.unobtrusive.parse(jqForm);
                if (!jqForm.valid())
                    return false;

                if (_beforeSubmit)
                    return _beforeSubmit(formData, jqForm, options);

                return true;
            }

            settings.afterSubmit = () => {
                if (_afterSubmit) {
                    _afterSubmit();
                }
            }

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
            }
            var $form = <any>$(selector);
            $form.ajaxForm(this.buildAjaxSettings(settings));
        }
    }

}