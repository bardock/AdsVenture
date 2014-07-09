interface MimeTypeDef {
    lookup(fname, include_charset?, default_mime_type?): any;
}
interface Window {
    MimeType: MimeTypeDef;
}