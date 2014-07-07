module Advertiser {

    export class SDK {

        referrer: string;

        constructor() {
            this.referrer = document.referrer;
            var body = document.getElementsByTagName("body")[0];
            body.onclick = (e) => this.handleEvent(e);
            body.onsubmit = (e) => this.handleEvent(e);
        }

        handleEvent(e: Event) {
            var target = (<HTMLElement>e.target);
            window.parent.postMessage(
                JSON.stringify({
                    eventType: e.type,
                    positionX: (<any>e).x,
                    positionY: (<any>e).y,
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
                        method: target.getAttribute("method"),
                    },
                    date: new Date()
                }),
                this.referrer
            );
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
