var Publisher;
(function (Publisher) {
    var SDK = (function () {
        function SDK() {
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
                    _this.on(window, 'message', function (e) {
                        if (iframe.src.indexOf(e.origin) == -1)
                            return;
                        var data = JSON.parse(e.data);
                        console.log(data); // TODO remove
                        data.contentID = contentID;
                        _this.api("POST", "http://dev.content.avt.com/api/contentDelivery/slot/" + slotID + "/event", data);
                    });
                });

                container.appendChild(iframe);
            };
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
        return SDK;
    })();
    Publisher.SDK = SDK;
})(Publisher || (Publisher = {}));
new Publisher.SDK();
//# sourceMappingURL=SDK.js.map
