module Advertiser {

    export class SDK {

        private IFRAME_MESSAGE_USER_SLOT_EVENT = "__UserSlotEvent__";
        private IFRAME_MESSAGE_RESIZE = "__Resize__";

        referrer: string;
        height: number;

        constructor() {
            this.referrer = document.referrer;
            window.onload = () => {
                var body = document.getElementsByTagName("body")[0];
                this.on(body, "click", (e) => this.handleUserSlotEvent(e));
                this.on(body, "submit", (e) => this.handleUserSlotEvent(e));

                this.handleResize(null);
                this.on(window, "resize", (e) => this.handleResize(e));
            };
        }

        handleUserSlotEvent(e: Event) {
            var target = (<HTMLElement>e.target);
            this.postMessage(
                this.IFRAME_MESSAGE_USER_SLOT_EVENT,
                {
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
                }
            );
        }

        handleResize(e: Event) {
            if (this.height == this.getDocumentHeight())
                return;
            this.height = this.getDocumentHeight();
            this.postMessage(
                this.IFRAME_MESSAGE_RESIZE,
                { height: this.height }
            );
        }

        postMessage(type: string, data: any) {
            window.parent.postMessage(
                type + JSON.stringify(data),
                this.referrer
            );
        }

        getDocumentHeight(): number {
            var body = document.body,
                html = document.documentElement;

            return Math.max(
                body.offsetHeight,
                html.scrollHeight,
                html.offsetHeight);
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

        private on = (function () {
            if ("addEventListener" in window) {
                return function (target, type, listener) {
                    target.addEventListener(type, listener, false);
                };
            }
            else {
                return function (object, sEvent, fpNotify) {
                    object.attachEvent("on" + sEvent, function () {
                        fpNotify(window.event);
                    });
                };
            }
        } ());
    }
}
new Advertiser.SDK();
