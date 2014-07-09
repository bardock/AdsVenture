///<reference path='../Includes.ts'/>

module Helpers {

    export class Numbers {

        decimals: number;
        minDecimals: number;

        constructor(decimals = 2, minDecimals: number = null) {
            this.decimals = decimals;
            this.minDecimals = minDecimals != null ? minDecimals : this.decimals;
        }

        format(n: number) {
            return this.trimZeros(Globalize.format(n, "n" + this.decimals));
        }

        parse(n: any): number {
            return parseFloat(
                Globalize.parseFloat(n).toFixed(this.decimals)
            ) || 0;
        }

        private trimZeros(formatted: string) {
            if (this.decimals <= this.minDecimals)
                return formatted;

            var split = formatted.split(Numbers.getDecimalsSeparator())
            var integerPart = split[0]
            var decimalPart = split[1].replace(/0+$/, "")
            var n = integerPart.concat(Numbers.getDecimalsSeparator(), decimalPart)

            if (decimalPart.length < this.minDecimals) {
                for (var i = 0; i < this.minDecimals - decimalPart.length; i++)
                    n = n.concat("0");
            }
            return n
        }

        static getDecimalsSeparator(): string {
            return Globalize.culture().numberFormat["."];
        }

        parseAndFormat(n: any) {
            return this.format(this.parse(n));
        }

        formatNumberNoThousands(n) {
            return Globalize.format(n, "n").replace(new RegExp("\\" + Globalize.culture().numberFormat[","], "g"), "");
        }

    }
}