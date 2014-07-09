///<reference path='../Includes.ts'/>
var Helpers;
(function (Helpers) {
    var Numbers = (function () {
        function Numbers(decimals, minDecimals) {
            if (typeof decimals === "undefined") { decimals = 2; }
            if (typeof minDecimals === "undefined") { minDecimals = null; }
            this.decimals = decimals;
            this.minDecimals = minDecimals != null ? minDecimals : this.decimals;
        }
        Numbers.prototype.format = function (n) {
            return this.trimZeros(Globalize.format(n, "n" + this.decimals));
        };

        Numbers.prototype.parse = function (n) {
            return parseFloat(Globalize.parseFloat(n).toFixed(this.decimals)) || 0;
        };

        Numbers.prototype.trimZeros = function (formatted) {
            if (this.decimals <= this.minDecimals)
                return formatted;

            var split = formatted.split(Numbers.getDecimalsSeparator());
            var integerPart = split[0];
            var decimalPart = split[1].replace(/0+$/, "");
            var n = integerPart.concat(Numbers.getDecimalsSeparator(), decimalPart);

            if (decimalPart.length < this.minDecimals) {
                for (var i = 0; i < this.minDecimals - decimalPart.length; i++)
                    n = n.concat("0");
            }
            return n;
        };

        Numbers.getDecimalsSeparator = function () {
            return Globalize.culture().numberFormat["."];
        };

        Numbers.prototype.parseAndFormat = function (n) {
            return this.format(this.parse(n));
        };

        Numbers.prototype.formatNumberNoThousands = function (n) {
            return Globalize.format(n, "n").replace(new RegExp("\\" + Globalize.culture().numberFormat[","], "g"), "");
        };
        return Numbers;
    })();
    Helpers.Numbers = Numbers;
})(Helpers || (Helpers = {}));
//# sourceMappingURL=Numbers.js.map
