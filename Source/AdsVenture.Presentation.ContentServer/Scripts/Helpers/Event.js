var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Helpers;
(function (Helpers) {
    var TEvent = (function () {
        function TEvent() {
            this.handlers = [];
        }
        TEvent.prototype.on = function (handler) {
            this.handlers.push(handler);
            return this;
        };

        TEvent.prototype.off = function (handler) {
            this.handlers = this.handlers.filter(function (h) {
                return h !== handler;
            });
            return this;
        };

        TEvent.prototype.trigger = function (data) {
            if (this.handlers) {
                this.handlers.forEach(function (h) {
                    return h(data);
                });
            }
            return this;
        };
        return TEvent;
    })();
    Helpers.TEvent = TEvent;

    var Event = (function (_super) {
        __extends(Event, _super);
        function Event() {
            _super.apply(this, arguments);
        }
        return Event;
    })(TEvent);
    Helpers.Event = Event;
})(Helpers || (Helpers = {}));
//# sourceMappingURL=Event.js.map
