var Advertiser;
(function (Advertiser) {
    var SDK = (function () {
        function SDK() {
            var referrer = document.referrer;

            document.getElementsByTagName("body")[0].onclick = function (e) {
                window.parent.postMessage(JSON.stringify({
                    action: "click",
                    target: e.target.tagName,
                    x: e.x,
                    y: e.y
                }), referrer);
            };
        }
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
