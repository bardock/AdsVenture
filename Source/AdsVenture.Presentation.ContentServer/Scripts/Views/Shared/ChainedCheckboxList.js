var Views;
(function (Views) {
    (function (Shared) {
        var ChainedCheckboxList = (function () {
            function ChainedCheckboxList(config) {
                var _this = this;
                this.config = config;
                this.$container = $(this.config.selector);

                this.$parentsItems = this.$container.find('input[data-chainedcheckbox-itemname=' + this.config.parentName + ']');
                this.$childrenItems = this.$container.find('input[data-chainedcheckbox-itemname=' + this.config.childName + ']');

                this.bind();

                $(this.$container).on('change', 'input[data-chainedcheckbox-itemname=' + this.config.parentName + ']', function () {
                    _this.bind();
                });
            }
            ChainedCheckboxList.prototype.bind = function () {
                var _this = this;
                var selectorCheckedParentsIDs = this.$container.find('input[data-chainedcheckbox-itemname=' + this.config.parentName + ']:checked').map(function (index, elem) {
                    return "[data-chainedcheckbox-" + _this.config.parentName.toLowerCase() + "=" + $(elem).val().toString() + "]";
                }).toArray().toString();

                var parentSelector = "input[data-chainedcheckbox-itemname=" + this.config.childName + "]" + selectorCheckedParentsIDs;

                this.hideChildren(this.$container.find('input[data-chainedcheckbox-itemname=' + this.config.childName + ']').not(parentSelector));

                if (this.config.limit != null) {
                    this.changeMoreButtonDisplay(this.$container.find(parentSelector).length);
                    this.displayChildrenByParentSelectorWithLimit(parentSelector);
                } else {
                    this.showChild(this.$container.find(parentSelector));
                }
            };

            ChainedCheckboxList.prototype.changeMoreButtonDisplay = function (childrenCount) {
                if (childrenCount > this.config.limit) {
                    this.$container.find('[data-name=' + this.config.childName + '][' + this.config.showMoreSelector + ']').show();
                } else {
                    this.$container.find('[data-name=' + this.config.childName + '][' + this.config.showMoreSelector + ']').hide();
                }
            };

            ChainedCheckboxList.prototype.displayChildrenByParentSelectorWithLimit = function (selector) {
                this.showChild(this.$container.find(selector).slice(0, this.config.limit)); //el helper CheckboxListFilter genera <label><input.. / >< / label >
                this.hideChild(this.$container.find(selector).slice(this.config.limit));
            };

            ChainedCheckboxList.prototype.hideChildren = function (children) {
                var _this = this;
                children.each(function (index, elem) {
                    _this.hideChild($(elem));
                });
            };

            ChainedCheckboxList.prototype.showChild = function (child) {
                child.closest('label').show();
            };

            ChainedCheckboxList.prototype.hideChild = function (child) {
                child.removeAttr('checked');
                child.closest('label').hide();
            };
            return ChainedCheckboxList;
        })();
        Shared.ChainedCheckboxList = ChainedCheckboxList;
    })(Views.Shared || (Views.Shared = {}));
    var Shared = Views.Shared;
})(Views || (Views = {}));
//# sourceMappingURL=ChainedCheckboxList.js.map
