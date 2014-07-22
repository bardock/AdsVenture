module Views.Shared {

    export interface ChainedCheckboxListConfig {
        selector: any;
        parentName: string;
        childName: string;
        limit?: number;
        showMoreSelector?: string;
    }

    export class ChainedCheckboxList {
        private config: ChainedCheckboxListConfig;
        private $container: JQuery;
        private $parentsItems: JQuery;
        private $childrenItems: JQuery;

        constructor(config: ChainedCheckboxListConfig) {
            this.config = config;
            this.$container = $(this.config.selector);

            this.$parentsItems = this.$container.find('input[data-chainedcheckbox-itemname=' + this.config.parentName + ']');
            this.$childrenItems = this.$container.find('input[data-chainedcheckbox-itemname=' + this.config.childName + ']');

            this.bind();

            $(this.$container).on('change', 'input[data-chainedcheckbox-itemname=' + this.config.parentName + ']', () => {
                this.bind();
            });
        }

        private bind() {
            var selectorCheckedParentsIDs = this.$container.find('input[data-chainedcheckbox-itemname=' + this.config.parentName + ']:checked')
                .map((index, elem) => {
                    return "[data-chainedcheckbox-" + this.config.parentName.toLowerCase() + "=" + $(elem).val().toString() + "]";
                })
                .toArray().toString();

            var parentSelector = "input[data-chainedcheckbox-itemname=" + this.config.childName + "]" + selectorCheckedParentsIDs;

            this.hideChildren(this.$container.find('input[data-chainedcheckbox-itemname=' + this.config.childName + ']').not(parentSelector));

            if (this.config.limit != null) {
                this.changeMoreButtonDisplay(this.$container.find(parentSelector).length);
                this.displayChildrenByParentSelectorWithLimit(parentSelector);
            } else {
                this.showChild(this.$container.find(parentSelector));
            }
        }

        private changeMoreButtonDisplay(childrenCount: number) {
            if (childrenCount > this.config.limit) {
                this.$container.find('[data-name=' + this.config.childName + '][' + this.config.showMoreSelector + ']').show();
            } else {
                this.$container.find('[data-name=' + this.config.childName + '][' + this.config.showMoreSelector + ']').hide();
            }
        }

        private displayChildrenByParentSelectorWithLimit(selector: string) {
            this.showChild(this.$container.find(selector).slice(0, this.config.limit)); //el helper CheckboxListFilter genera <label><input.. / >< / label >
            this.hideChild(this.$container.find(selector).slice(this.config.limit));
        }

        private hideChildren(children: JQuery) {
            children.each((index, elem) => {
                this.hideChild($(elem));
            });
        }

        private showChild(child: JQuery) {
            child.closest('label').show();
        }

        private hideChild(child: JQuery) {
            child.removeAttr('checked');
            child.closest('label').hide();
        }
    }
}