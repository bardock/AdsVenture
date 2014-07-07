var Advertiser;
(function (Advertiser) {
    var SDK = (function () {
        function SDK() {
            var _this = this;
            this.referrer = document.referrer;
            var body = document.getElementsByTagName("body")[0];
            body.onclick = function (e) {
                return _this.handleEvent(e);
            };
            body.onsubmit = function (e) {
                return _this.handleEvent(e);
            };
        }
        SDK.prototype.handleEvent = function (e) {
            var target = e.target;
            window.parent.postMessage(JSON.stringify({
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
            }), this.referrer);
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
