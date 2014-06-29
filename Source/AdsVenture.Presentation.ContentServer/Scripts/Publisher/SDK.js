var Publisher;
(function (Publisher) {
    var SDK = (function () {
        function SDK() {
            this.initStyles();
            var containers = document.querySelectorAll(".avt-container");
            for (var i = 0; i < containers.length; i++) {
                var c = containers[i];
                var slot = c.getAttribute("data-slot");
                if (!slot)
                    return;
                this.api("GET", "http://dev.content.avt.com/api/content/slot/" + slot, this.getAppendContentHandler(c));
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

        SDK.prototype.getAppendContentHandler = function (container) {
            return function (response) {
                container.insertAdjacentHTML('beforeend', response);
            };
        };

        // TODO: move to a helper class
        SDK.prototype.api = function (method, url, onSuccess) {
            var xmlhttp = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject("Microsoft.XMLHTTP");

            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4) {
                    if (xmlhttp.status == 200) {
                        onSuccess(JSON.parse(xmlhttp.responseText));
                    } else {
                        console.error('AJAX request error ' + xmlhttp.status);
                    }
                }
            };

            xmlhttp.open(method, url, true);
            xmlhttp.send();
        };
        return SDK;
    })();
    Publisher.SDK = SDK;
})(Publisher || (Publisher = {}));
new Publisher.SDK();
//# sourceMappingURL=SDK.js.map
