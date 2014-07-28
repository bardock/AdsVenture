var Advertiser;
(function (Advertiser) {
    var SDK = (function () {
        function SDK() {
            var _this = this;
            this.IFRAME_MESSAGE_USER_SLOT_EVENT = "__UserSlotEvent__";
            this.IFRAME_MESSAGE_RESIZE = "__Resize__";
            this.on = (function () {
                if ("addEventListener" in window) {
                    return function (target, type, listener) {
                        target.addEventListener(type, listener, false);
                    };
                } else {
                    return function (object, sEvent, fpNotify) {
                        object.attachEvent("on" + sEvent, function () {
                            fpNotify(window.event);
                        });
                    };
                }
            }());
            this.referrer = document.referrer;
            window.onload = function () {
                var body = document.getElementsByTagName("body")[0];
                _this.on(body, "click", function (e) {
                    return _this.handleUserSlotEvent(e);
                });
                _this.on(body, "submit", function (e) {
                    return _this.handleUserSlotEvent(e);
                });

                _this.handleResize(null);
                _this.on(window, "resize", function (e) {
                    return _this.handleResize(e);
                });
            };
        }
        SDK.prototype.handleUserSlotEvent = function (e) {
            var target = e.target;
            this.postMessage(this.IFRAME_MESSAGE_USER_SLOT_EVENT, {
                eventType: e.type,
                positionX: e.x,
                positionY: e.y,
                target: {
                    tagName: target.tagName,
                    elemId: target.getAttribute("id"),
                    elemClass: target.getAttribute("class"),
                    name: target.getAttribute("name"),
                    type: target.getAttribute("type"),
                    value: (target.tagName == "A" || target.tagName == "BUTTON" ? target.innerText : target.getAttribute("value")),
                    href: target.getAttribute("href"),
                    onclick: target.getAttribute("onclick"),
                    action: target.getAttribute("action"),
                    method: target.getAttribute("method")
                },
                date: new Date()
            });
        };

        SDK.prototype.handleResize = function (e) {
            if (this.height == this.getDocumentHeight())
                return;
            this.height = this.getDocumentHeight();
            this.postMessage(this.IFRAME_MESSAGE_RESIZE, { height: this.height });
        };

        SDK.prototype.postMessage = function (type, data) {
            window.parent.postMessage(type + JSON.stringify(data), this.referrer);
        };

        SDK.prototype.getDocumentHeight = function () {
            var body = document.body, html = document.documentElement;

            return Math.max(body.offsetHeight, html.scrollHeight, html.offsetHeight);
        };

        SDK.prototype.getQueryValue = function (name) {
            var q = window.location.search;
            var params = q.split(/[\?&]+/);
            for (var i = 0; i < params.length; i++) {
                var keyValue = params[i].split("=");
                if (keyValue[0] == name) {
                    return decodeURIComponent(keyValue[1]);
                }
            }
        };
        return SDK;
    })();
    Advertiser.SDK = SDK;
})(Advertiser || (Advertiser = {}));
new Advertiser.SDK();
//# sourceMappingURL=SDK.js.map
