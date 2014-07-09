interface MustacheOptions {
    warnOnMissingTemplates: boolean;
    allowOverwrite: boolean;
    domTemplateType: string;
    externalTemplateDataType: string;
}

interface MustacheStatic{
    options: MustacheOptions;
    load(url: string, onComplete: () => void ): JQueryXHR;
    add(templateName: string, templateHtml: string): void;
    addFromDom(): void;
    remove(templateName: string): string;
    clear(): void;
    render(templateName: string, templateData: any): string;
    templates(): string[];
    instance: MustacheStatic;
}

interface JQuery{
    mustache(templateName: string, templateData: any): JQuery;
    mustache(templateName: string): JQuery;
}

interface JQueryStatic {
    mustache(templateName: string, templateData: any, options: any): JQuery;
    Mustache: MustacheStatic;
}
