module Publisher {

    export class SDK {

        constructor() {
            this.initStyles();
            var containers = document.querySelectorAll(".avt-container");
            for (var i = 0; i < containers.length; i++) {
                var c = <HTMLElement>containers[i];
                var slot = c.getAttribute("data-slot");
                if (!slot) return;
                this.api("GET",
                    "http://dev.content.avt.com/api/content/slot/" + slot,
                    this.getAppendContentHandler(c)
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

        private getAppendContentHandler(container: HTMLElement) {
            return response => {
                //container.insertAdjacentHTML('beforeend', response);
                //var iframe = <HTMLIFrameElement>container.lastChild;

                // TODO check is iframe
                var wrapper = document.createElement('div');
                wrapper.innerHTML = response;
                var iframe = <HTMLIFrameElement>wrapper.firstChild;
                iframe.src = iframe.src
                    + (iframe.src.indexOf("?") == -1 ? "?" : "&")
                    + "avt_ref="
                    + encodeURIComponent(window.location.href);

                this.on(iframe, "load", (e) => {
                    this.on(window, 'message', (e) => {
                        if (iframe.src.indexOf(e.origin) == -1)
                            return;
                        console.log(JSON.parse(e.data));
                    });
                });

                container.appendChild(iframe);
            };
        }

        // TODO: move to a helper class
        api(method: string, url: string, onSuccess: (any) => void) {
            var xmlhttp: XMLHttpRequest =
                (<any>window).XMLHttpRequest
                    ? new XMLHttpRequest()
                    : new ActiveXObject("Microsoft.XMLHTTP"); // IE6, IE5

            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4) {
                    if (xmlhttp.status == 200) {
                        onSuccess(JSON.parse(xmlhttp.responseText));
                    }
                    else {
                        console.error('AJAX request error ' + xmlhttp.status)
                    }
                }
            }

            xmlhttp.open(method, url, true);
            xmlhttp.send();
        }

        on = (function () {
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
new Publisher.SDK();
