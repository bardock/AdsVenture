// **FORK** 
// > Removed unused mime types
// > Undefined checks fixed using typeof
// > Lookup by extension without filename


// mimetype.js - A catalog object of mime types based on file extensions
//
// @author: R. S. Doiel, <rsdoiel@gmail.com>
// copyright (c) 2012 all rights reserved
//
// Released under New the BSD License.
// See: http://opensource.org/licenses/bsd-license.php
//

(function (self) {
	var path;
	
	// If we're NodeJS I can use the path module.
	// If I'm MongoDB shell, not available.
	if (typeof require != "undefined") {
		path = require('path');
	} else {
		path = {
			extname: function (filename) {
			    if (filename.lastIndexOf(".") > 0) {
			        return filename.substr(filename.lastIndexOf("."));
			    } else if (filename.lastIndexOf(".") == 0) {
			        return filename;
			    } else {
			        return "." + filename;
			    }
			}
		};
	}

	if (typeof exports != "undefined") {
		exports = {};
	}

	MimeType = {
		charset: 'UTF-8',
		catalog: {},
		lookup: function (fname, include_charset, default_mime_type) {
			var ext, charset = this.charset;
			
			if (include_charset === undefined) {
				include_charset = false;
			}
			
			if (typeof include_charset === "string") {
				charset = include_charset;
				include_charset = true;
			}
	
			if (path.extname !== undefined) {
				ext = path.extname(fname).toLowerCase();
			} else if (fname.lastIndexOf('.') > 0) {
				ext = fname.substr(fname.lastIndexOf('.')).toLowerCase();
			} else {
				ext = fname;
			}
			
			// Handle the special cases where their is no extension
			// e..g README, manifest, LICENSE, TODO
			if (ext == "") {
				ext = fname;
			}
	
			if (this.catalog[ext] !== undefined) {
				if (include_charset === true && 
					this.catalog[ext].indexOf('text/') === 0 &&
					this.catalog[ext].indexOf('charset') < 0) {
					return this.catalog[ext] + '; charset=' + charset;
				} else {
					return this.catalog[ext];
				}
			} else if (default_mime_type !== undefined) {
				if (include_charset === true && 
					default_mime_type.indexOf('text/') === 0) {
					return default_mime_type + '; charset=' + charset;
				}
				return default_mime_type;
			}
			return false;
		},
		set: function (exts, mime_type_string) {
			var result = true, self = this;
			if (exts.indexOf(',')) {
				exts.split(',').forEach(function (ext) {
					ext = ext.trim();
					self.catalog[ext] = mime_type_string;
					if (self.catalog[ext] !== mime_type_string) {
						result = false;
					}
				});
			} else {
				result = (self.catalog[exts] === mime_type_string);
			}
			return result;
		},
		del: function (ext) {
			delete this.catalog[ext];
			return (this.catalog[ext] === undefined);
		},
		forEach: function (callback) {
			var self = this, ext;
			// Mongo 2.2. Shell doesn't support Object.keys()
			for (ext in self.catalog) {
				if (self.catalog.hasOwnProperty(ext)) {
					callback(ext, self.catalog[ext]);
				}
			}
			return self.catalog;
		}
	};
	
	// From Apache project's mime type list.
	MimeType.set(".doc,.dot", "application/msword");
	MimeType.set(".ogx", "application/ogg");
	MimeType.set(".pdf", "application/pdf");
	MimeType.set(".pgp", "application/pgp-encrypted");
	MimeType.set(".rss", "application/rss+xml");
	MimeType.set(".rtf", "application/rtf");
	MimeType.set(".cab", "application/vnd.ms-cab-compressed");
	MimeType.set(".xls,.xlm,.xla,.xlc,.xlt,.xlw", "application/vnd.ms-excel");
	MimeType.set(".eot", "application/vnd.ms-fontobject");
	MimeType.set(".ppt,.pps,.pot", "application/vnd.ms-powerpoint");
	MimeType.set(".xps", "application/vnd.ms-xpsdocument");
	MimeType.set(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
	MimeType.set(".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide");
	MimeType.set(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow");
	MimeType.set(".potx", "application/vnd.openxmlformats-officedocument.presentationml.template");
	MimeType.set(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
	MimeType.set(".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template");
	MimeType.set(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
	MimeType.set(".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template");
	MimeType.set(".pbd", "application/vnd.powerbuilder6");
	MimeType.set(".wsdl", "application/wsdl+xml");
	MimeType.set(".otf", "application/x-font-otf");
	MimeType.set(".pcf", "application/x-font-pcf");
	MimeType.set(".snf", "application/x-font-snf");
	MimeType.set(".ttf,.ttc", "application/x-font-ttf");
	MimeType.set(".pfa,.pfb,.pfm,.afm", "application/x-font-type1");
	MimeType.set(".application", "application/x-ms-application");
	MimeType.set(".exe,.dll,.com,.bat,.msi", "application/x-msdownload");
	MimeType.set(".pub", "application/x-mspublisher");
	MimeType.set(".p12,.pfx", "application/x-pkcs12");
	MimeType.set(".p7b,.spc", "application/x-pkcs7-certificates");
	MimeType.set(".p7r", "application/x-pkcs7-certreqresp");
	MimeType.set(".rar", "application/x-rar-compressed");
	MimeType.set(".sh", "application/x-sh");
	MimeType.set(".shar", "application/x-shar");
	MimeType.set(".swf", "application/x-shockwave-flash");
	MimeType.set(".xap", "application/x-silverlight-app");
	MimeType.set(".src", "application/x-wais-source");
	MimeType.set(".xhtml,.xht", "application/xhtml+xml");
	MimeType.set(".xml,.xsl", "application/xml");
	MimeType.set(".dtd", "application/xml-dtd");
	MimeType.set(".xop", "application/xop+xml");
	MimeType.set(".xslt", "application/xslt+xml");
	MimeType.set(".xspf", "application/xspf+xml");
	MimeType.set(".mxml,.xhvml,.xvml,.xvm", "application/xv+xml");
	MimeType.set(".zip", "application/zip");
	MimeType.set(".oga,.ogg,.spx", "audio/ogg");
	MimeType.set(".aac", "audio/x-aac");
	MimeType.set(".aif,.aiff,.aifc", "audio/x-aiff");
	MimeType.set(".m3u", "audio/x-mpegurl");
	MimeType.set(".wax", "audio/x-ms-wax");
	MimeType.set(".wma", "audio/x-ms-wma");
	MimeType.set(".ram,.ra", "audio/x-pn-realaudio");
	MimeType.set(".rmp", "audio/x-pn-realaudio-plugin");
	MimeType.set(".wav", "audio/x-wav");
	MimeType.set(".bmp", "image/bmp");
	MimeType.set(".cgm", "image/cgm");
	MimeType.set(".g3", "image/g3fax");
	MimeType.set(".gif", "image/gif");
	MimeType.set(".ief", "image/ief");
	MimeType.set(".jp2", "image/jp2");
	MimeType.set(".jpeg,.jpg,.jpe", "image/jpeg");
	MimeType.set(".pict,.pic,.pct", "image/pict");
	MimeType.set(".png", "image/png");
	MimeType.set(".btif", "image/prs.btif");
	MimeType.set(".svg,.svgz", "image/svg+xml");
	MimeType.set(".tiff,.tif", "image/tiff");
	MimeType.set(".psd", "image/vnd.adobe.photoshop");
	MimeType.set(".djvu,.djv", "image/vnd.djvu");
	MimeType.set(".dwg", "image/vnd.dwg");
	MimeType.set(".dxf", "image/vnd.dxf");
	MimeType.set(".fbs", "image/vnd.fastbidsheet");
	MimeType.set(".fpx", "image/vnd.fpx");
	MimeType.set(".fst", "image/vnd.fst");
	MimeType.set(".mmr", "image/vnd.fujixerox.edmics-mmr");
	MimeType.set(".rlc", "image/vnd.fujixerox.edmics-rlc");
	MimeType.set(".mdi", "image/vnd.ms-modi");
	MimeType.set(".npx", "image/vnd.net-fpx");
	MimeType.set(".wbmp", "image/vnd.wap.wbmp");
	MimeType.set(".xif", "image/vnd.xiff");
	MimeType.set(".ras", "image/x-cmu-raster");
	MimeType.set(".cmx", "image/x-cmx");
	MimeType.set(".fh,.fhc,.fh4,.fh5,.fh7", "image/x-freehand");
	MimeType.set(".ico", "image/x-icon");
	MimeType.set(".pntg,.pnt,.mac", "image/x-macpaint");
	MimeType.set(".pcx", "image/x-pcx");
	MimeType.set(".pnm", "image/x-portable-anymap");
	MimeType.set(".pbm", "image/x-portable-bitmap");
	MimeType.set(".pgm", "image/x-portable-graymap");
	MimeType.set(".ppm", "image/x-portable-pixmap");
	MimeType.set(".qtif,.qti", "image/x-quicktime");
	MimeType.set(".rgb", "image/x-rgb");
	MimeType.set(".xbm", "image/x-xbitmap");
	MimeType.set(".xpm", "image/x-xpixmap");
	MimeType.set(".xwd", "image/x-xwindowdump");
	MimeType.set(".eml,.mime", "message/rfc822");
	MimeType.set(".ics,.ifb", "text/calendar");
	MimeType.set(".css", "text/css");
	MimeType.set(".csv", "text/csv");
	MimeType.set(".html,.htm", "text/html");
	MimeType.set(".txt,.text,.conf,.def,.list,.log,.in", "text/plain");
	MimeType.set(".t,.tr,.roff,.man,.me,.ms", "text/troff");
	MimeType.set(".uri,.uris,.urls", "text/uri-list");
	MimeType.set(".curl", "text/vnd.curl");
	MimeType.set(".dcurl", "text/vnd.curl.dcurl");
	MimeType.set(".scurl", "text/vnd.curl.scurl");
	MimeType.set(".mcurl", "text/vnd.curl.mcurl");
	MimeType.set(".java", "text/x-java-source");
	MimeType.set(".vcs", "text/x-vcalendar");
	MimeType.set(".vcf", "text/x-vcard");
	MimeType.set(".3gp", "video/3gpp");
	MimeType.set(".3g2", "video/3gpp2");
	MimeType.set(".h261", "video/h261");
	MimeType.set(".h263", "video/h263");
	MimeType.set(".h264", "video/h264");
	MimeType.set(".jpgv", "video/jpeg");
	MimeType.set(".jpm,.jpgm", "video/jpm");
	MimeType.set(".mj2,.mjp2", "video/mj2");
	MimeType.set(".mp4,.mp4v,.mpg4,.m4v", "video/mp4");
	MimeType.set(".mpeg,.mpg,.mpe,.m1v,.m2v", "video/mpeg");
	MimeType.set(".ogv", "video/ogg");
	MimeType.set(".qt,.mov", "video/quicktime");
	MimeType.set(".fvt", "video/vnd.fvt");
	MimeType.set(".mxu,.m4u", "video/vnd.mpegurl");
	MimeType.set(".pyv", "video/vnd.ms-playready.media.pyv");
	MimeType.set(".viv", "video/vnd.vivo");
	MimeType.set(".dv,.dif", "video/x-dv");
	MimeType.set(".f4v", "video/x-f4v");
	MimeType.set(".fli", "video/x-fli");
	MimeType.set(".flv", "video/x-flv");
	MimeType.set(".asf,.asx", "video/x-ms-asf");
	MimeType.set(".wm", "video/x-ms-wm");
	MimeType.set(".wmv", "video/x-ms-wmv");
	MimeType.set(".wmx", "video/x-ms-wmx");
	MimeType.set(".wvx", "video/x-ms-wvx");
	MimeType.set(".avi", "video/x-msvideo");
	MimeType.set(".movie", "video/x-sgi-movie");
	MimeType.set("README,LICENSE,COPYING,TODO,ABOUT,AUTHORS,CONTRIBUTORS", 
		"text/plain");
	MimeType.set("manifest,.manifest,.mf,.appcache", "text/cache-manifest");
	if (typeof exports != "undefined") {
		exports.charset = MimeType.charset;
		exports.catalog = MimeType.catalog;
		exports.lookup = MimeType.lookup;
		exports.set = MimeType.set;
		exports.del = MimeType.del;
		exports.forEach = MimeType.forEach;
	}

	self.MimeType = MimeType;
	return self;
}(this));
