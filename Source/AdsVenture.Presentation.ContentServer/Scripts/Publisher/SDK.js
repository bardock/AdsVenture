var Publisher;
(function (Publisher) {
    var SDK = (function () {
        function SDK() {
            var containers = document.querySelectorAll(".avt-container");
            for (var i = 0; i < containers.length; i++) {
                var c = containers[i];
                var slot = c.getAttribute("data-slot");
                if (!slot)
                    return;
                this.api("GET", "http://dev.content.avt.com/api/content/slot/" + slot, this.getAppendContentHandler(c));
            }
        }
        SDK.prototype.getAppendContentHandler = function (container) {
            return function (response) {
                container.insertAdjacentHTML('beforeend', response);
            };
        };

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
