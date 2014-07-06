module Advertiser {

    export class SDK {

        constructor() {
            var referrer = document.referrer;

            document.getElementsByTagName("body")[0].onclick = (e) => {
                window.parent.postMessage(
                    JSON.stringify({
                        action: "click",
                        target: (<HTMLElement>e.target).tagName,
                        x: e.x,
                        y: e.y
                    }),
                    referrer
                );
            }
        }

        getQueryValue(name: string) {
            var q = window.location.search;
            var params = q.split(/[\?&]+/);
            for (var i = 0; i < params.length; i++) {
                var keyValue = params[i].split("=");
                if (keyValue[0] == name) {
                    return decodeURIComponent(keyValue[1]);
                }
            }
        }
    }
}
new Advertiser.SDK();
