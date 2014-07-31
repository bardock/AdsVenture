module Publisher {

    export class SDK {

        private IFRAME_MESSAGE_USER_SLOT_EVENT = "__UserSlotEvent__";
        private IFRAME_MESSAGE_RESIZE = "__Resize__";

        private baseUrl: string;

        constructor() {
            this.baseUrl = document.getElementById('avt-publisher-sdk').getAttribute("src");
            this.baseUrl = this.baseUrl.substring(0, this.baseUrl.indexOf("scripts/"));

            this.initStyles();
            var containers = document.querySelectorAll(".avt-container");
            for (var i = 0; i < containers.length; i++) {
                var c = <HTMLElement>containers[i];
                var slot = c.getAttribute("data-slot");
                if (!slot) return;
                this.api("GET",
                    this.baseUrl + "api/contentDelivery/slot/" + slot,
                    null,
                    this.getAppendContentHandler(c, slot)
                );
            }
        }

        private initStyles() {
            var css = ".avt-container {" +
                            "width: 100%;" +
                            "height: 300px;" +
                            "position: relative;" +
                        "}" +
                        ".avt-container iframe {" +
                            "border: 1px solid gray;" +
                            "width: 100%;" +
                            "position: absolute;" +
                            "top: 0;" +
                            "left: 0;" +
                            "height: 100%;" +
                        "}",
                head = document.head || document.getElementsByTagName('head')[0],
                style = document.createElement('style');

            style.type = 'text/css';
            if (style.styleSheet)
              (<any>style.styleSheet).cssText = css;
             else
              style.appendChild(document.createTextNode(css));
            head.insertBefore(style, head.firstChild);
        }

        private getAppendContentHandler(container: HTMLElement, slotID: string) {
            return response => {
                var contentID = <string>response.contentID;

                //container.insertAdjacentHTML('beforeend', response);
                //var iframe = <HTMLIFrameElement>container.lastChild;

                // TODO check is iframe
                var wrapper = document.createElement('div');
                wrapper.innerHTML = response.html;
                var iframe = <HTMLIFrameElement>wrapper.firstChild;
                iframe.src = iframe.src
                    + (iframe.src.indexOf("?") == -1 ? "?" : "&")
                    + "avt_ref=" // TODO
                    + encodeURIComponent(window.location.href);

                this.on(iframe, "load", (e) => {
                    this.bindSlotEvents(iframe, contentID, slotID);
                    this.bindResponsive(container, iframe);
                });

                container.appendChild(iframe);
            };
        }

        private bindSlotEvents(iframe: HTMLIFrameElement, contentID: string, slotID: string) {
            this.onIframeMessage(iframe, this.IFRAME_MESSAGE_USER_SLOT_EVENT, (data) => {
                data.contentID = contentID;
                this.api("POST",
                    this.baseUrl + "api/contentDelivery/slot/" + slotID + "/event",
                    data);
            });
        }

        private bindResponsive(container: HTMLElement, iframe: HTMLIFrameElement) {
            if (!this.elemHasClass(container, "avt-responsive"))
                return;
            this.onIframeMessage(iframe, this.IFRAME_MESSAGE_RESIZE, (data) => {
                container.style.height = data.height + 2;
            });
        }

        private elemHasClass(element: HTMLElement, _class: string) {
            return (' ' + element.className + ' ').indexOf(' ' + _class + ' ') > -1;
        }

        // TODO: move to a helper class
        private api(method: string, url: string, data, onSuccess?: (any) => void) {
            var xmlhttp: XMLHttpRequest =
                (<any>window).XMLHttpRequest
                    ? new XMLHttpRequest()
                    : new ActiveXObject("Microsoft.XMLHTTP"); // IE6, IE5

            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4) {
                    if (xmlhttp.status >= 200 && xmlhttp.status < 300) {
                        if(onSuccess)
                            onSuccess(xmlhttp.responseText && JSON.parse(xmlhttp.responseText) || null);
                    }
                    else {
                        console.error('AJAX request error ' + xmlhttp.status)
                    }
                }
            }
            xmlhttp.open(method, url, true);
            if (data) {
                xmlhttp.setRequestHeader("Content-type", "application/json");
                xmlhttp.send(JSON.stringify(data));
            } else {
                xmlhttp.send();
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

        private onIframeMessage(iframe: HTMLIFrameElement, type: string, handler: (any) => any) {
            this.on(window, 'message', (e: MessageEvent) => {
                if (iframe.src.indexOf(e.origin) == -1)
                    return;
                var indexOfType = e.data.indexOf(type);
                if (indexOfType == -1)
                    return;
                var data = JSON.parse((<string>e.data).substring(indexOfType + type.length));
                handler(data);
            });
        }
    }
}
new Publisher.SDK();
