var Publisher;
(function (Publisher) {
    var SDK = (function () {
        function SDK() {
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
            this.initStyles();
            var containers = document.querySelectorAll(".avt-container");
            for (var i = 0; i < containers.length; i++) {
                var c = containers[i];
                var slot = c.getAttribute("data-slot");
                if (!slot)
                    return;
                this.api("GET", "http://dev.content.avt.com/api/contentDelivery/slot/" + slot, null, this.getAppendContentHandler(c, slot));
            }
        }
        SDK.prototype.initStyles = function () {
            var css = ".avt-container {" + "width: 100%;" + "height: 300px;" + "position: relative;" + "}" + ".avt-container iframe {" + "border: 1px solid gray;" + "width: 100%;" + "position: absolute;" + "top: 0;" + "left: 0;" + "height: 100%;" + "}", head = document.head || document.getElementsByTagName('head')[0], style = document.createElement('style');

            style.type = 'text/css';
            if (style.styleSheet)
                style.styleSheet.cssText = css;
            else
                style.appendChild(document.createTextNode(css));
            head.insertBefore(style, head.firstChild);
        };

        SDK.prototype.getAppendContentHandler = function (container, slotID) {
            var _this = this;
            return function (response) {
                var contentID = response.contentID;

                //container.insertAdjacentHTML('beforeend', response);
                //var iframe = <HTMLIFrameElement>container.lastChild;
                // TODO check is iframe
                var wrapper = document.createElement('div');
                wrapper.innerHTML = response.html;
                var iframe = wrapper.firstChild;
                iframe.src = iframe.src + (iframe.src.indexOf("?") == -1 ? "?" : "&") + "avt_ref=" + encodeURIComponent(window.location.href);

                _this.on(iframe, "load", function (e) {
                    _this.bindSlotEvents(iframe, contentID, slotID);
                    _this.bindResponsive(container, iframe);
                });

                container.appendChild(iframe);
            };
        };

        SDK.prototype.bindSlotEvents = function (iframe, contentID, slotID) {
            var _this = this;
            this.onIframeMessage(iframe, this.IFRAME_MESSAGE_USER_SLOT_EVENT, function (data) {
                data.contentID = contentID;
                _this.api("POST", "http://dev.content.avt.com/api/contentDelivery/slot/" + slotID + "/event", data);
            });
        };

        SDK.prototype.bindResponsive = function (container, iframe) {
            if (!this.elemHasClass(container, "avt-responsive"))
                return;
            this.onIframeMessage(iframe, this.IFRAME_MESSAGE_RESIZE, function (data) {
                container.style.height = data.height + 2;
            });
        };

        SDK.prototype.elemHasClass = function (element, _class) {
            return (' ' + element.className + ' ').indexOf(' ' + _class + ' ') > -1;
        };

        // TODO: move to a helper class
        SDK.prototype.api = function (method, url, data, onSuccess) {
            var xmlhttp = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");

            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4) {
                    if (xmlhttp.status >= 200 && xmlhttp.status < 300) {
                        if (onSuccess)
                            onSuccess(xmlhttp.responseText && JSON.parse(xmlhttp.responseText) || null);
                    } else {
                        console.error('AJAX request error ' + xmlhttp.status);
                    }
                }
            };
            xmlhttp.open(method, url, true);
            if (data) {
                xmlhttp.setRequestHeader("Content-type", "application/json");
                xmlhttp.send(JSON.stringify(data));
            } else {
                xmlhttp.send();
            }
        };

        SDK.prototype.onIframeMessage = function (iframe, type, handler) {
            this.on(window, 'message', function (e) {
                if (iframe.src.indexOf(e.origin) == -1)
                    return;
                var indexOfType = e.data.indexOf(type);
                if (indexOfType == -1)
                    return;
                var data = JSON.parse(e.data.substring(indexOfType + type.length));
                handler(data);
            });
        };
        return SDK;
    })();
    Publisher.SDK = SDK;
})(Publisher || (Publisher = {}));
new Publisher.SDK();
//# sourceMappingURL=SDK.js.map
