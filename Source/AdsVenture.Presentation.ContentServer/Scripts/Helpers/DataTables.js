var DataTables;

(function ($, window) {

    DataTables = {
        reinit: function (selector) {
            return DataTables.init(selector, $.extend(
                $(selector).data("datatables-opts"),
                { data: null }
            ));
        },
        init: function (selector, opts) {

            opts = $.extend(opts, $(selector).data());

            $(selector).data("datatables-opts", opts);

            opts = $.extend({
                data: null,
                url: undefined,
                cols: undefined,
                pageLength: 10,
                filterDelay: false,
                paginate: true,
                filter: true,
                onCheck: null,
                checkedIds: [],
                queryStringPrefix: "",
                history: true,
                fixedHeader: {
                    offsetTop: $('header.navbar.navbar-inverse').height()
                },
                showProcessing: false
            }, opts);

            if (opts.data) {
                $("tbody", selector).empty().mustache("datarow-template", opts.data);
            }

            if (typeof (History) == "undefined" || !History.pushState)
                opts.history = false;

            var isServer = opts.url && opts.url.length;

            var options = {};

            if (isServer)
                $.extend(options, DataTables.getServerOptions(selector, opts));

            options.aoColumns = options.aoColumns || [];

            if (opts.totalrecords && isServer)
                options.iDeferLoading = opts.totalrecords;

            // Init search filter and page
            if (opts.filter)
                options.oSearch = { "sSearch": getQueryStringParam(opts.queryStringPrefix + "search") };
            if (opts.paginate) {
                options.iDisplayLength = getQueryStringParam(opts.queryStringPrefix + "length") * 1 || opts.pageLength;
                options.iDisplayStart = getQueryStringParam(opts.queryStringPrefix + "startIndex") * 1 || 0;
            }

            // Define sortable config for each column and initialize sorting
            var aaSorting = [];
            var defaultSort = [];
            var sortBy = opts.history && getQueryStringParam(opts.queryStringPrefix + "sortBy");
            var sortDir = opts.history && getQueryStringParam(opts.queryStringPrefix + "sortDir") || "asc";
            var i = 0;
            $("thead th", selector).each(function () {
                options.aoColumns.push({
                    bSortable: $(this).data("sortable") != false,
                    sClass: $(this).data("col-class")
                });
                if (sortBy) {
                    var sortByAsInt = parseInt(sortBy);
                    if (sortByAsInt && sortByAsInt == i
                            || sortBy == $(this).data("sort-field"))
                        aaSorting.push([i, sortDir]);
                } else {
                    var sortDefaultDir = $(this).data("sort-default");
                    if (sortDefaultDir) {
                        defaultSort.push([i, sortDefaultDir]);
                        aaSorting.push([i, sortDefaultDir]);
                    }
                }
                i++;
            });
            var firstDraw = true;

            var dt = $(selector);
            dt = dt.dataTable($.extend(options, {
                "sAjaxSource": isServer ? opts.url : undefined,
                "bProcessing": isServer,
                "bServerSide": isServer,
                "bSort": true,
                "bPaginate": opts.paginate,
                "bProcessing": opts.showProcessing,
                "bFilter": opts.filter,
                "bLengthChange": true,
                "bInfo": opts.paginate,
                "bAutoWidth": false,
                "fnDrawCallback": function (oSettings) {

                    // Update History
                    if (opts.history) {

                        var params = $.parseParams(document.location.search);

                        if (oSettings._iDisplayLength != opts.pageLength || params.length && oSettings._iDisplayLength != params.length)
                            params.length = oSettings._iDisplayLength;

                        if (oSettings._iDisplayStart != 0 || params.startIndex && oSettings._iDisplayStart != params.startIndex)
                            params.startIndex = oSettings._iDisplayStart || undefined;

                        if (oSettings.oPreviousSearch.sSearch || params.search && oSettings.oPreviousSearch.sSearch != params.search)
                            params.search = oSettings.oPreviousSearch.sSearch;

                        if (oSettings.aaSorting && oSettings.aaSorting.length) {
                            params.sortBy = [];
                            params.sortDir = [];

                            for (var i = 0; i < oSettings.aaSorting.length; i++) {
                                var sort = oSettings.aaSorting[i];
                                if (sort.length > 1
                                    && (!defaultSort.length || i >= defaultSort.length || defaultSort[i][0] != sort[0] || defaultSort[i][1] != sort[1])) {
                                    var sortBy = $("thead th:eq(" + sort[0] + ")", selector).data("sort-field")
                                    params.sortBy.push(sortBy || sort[0]);
                                    params.sortDir.push(sort[1]);
                                }
                            }
                        }
                        // Generate query string
                        var queryString = $.param(params, true);
                        if (firstDraw || queryString && queryString != document.location.search.replace("?", "")) {

                            dt.data("history-pushing", true);

                            var url = window.location.href.split("?")[0] + (queryString ? "?" : "") + queryString;

                            History.pushState(
                                {
                                    params: params,
                                    settings: {
                                        _iDisplayLength: oSettings._iDisplayLength,
                                        _iDisplayStart: oSettings._iDisplayStart,
                                        oPreviousSearch: oSettings.oPreviousSearch,
                                        aaSorting: oSettings.aaSorting
                                    }
                                },
                                document.title,
                                url
                            );
                        }
                    }

                    var container = $(selector).closest(".dataTables_wrapper")
                    container = container.size() ? container : (selector);

                    // bind checkall input
                    $(".checkall", container).change(function () {
                        dt.checkPage($(this).is(":checked"));
                    });
                    $("[name=check-id]", container).change(function () {
                        if ($(this).is(":checked") == false)
                            $(".checkall", container).prop("checked", false);
                        else
                            DataTables.updateCheckAll(container);
                    });

                    DataTables.updateCheckAll(container);

                    // init and bind single select
                    for (var i = 0; i < opts.checkedIds.length; i++) {
                        $(":radio[name=check-id][value=" + opts.checkedIds[i] + "]", container).prop("checked", true);
                    }
                    if (opts.onCheck)
                        $(":radio[name=check-id]", container).change(function () {
                            if ($(this).is(":checked")) {
                                dt.check($(this).val(), true);
                                opts.onCheck($(this).val());
                            }
                        });

                    $(selector).css("visibility", "visible");

                    firstDraw = false;
                },
                "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                    for (var i = 0; i < aData.length; i++) {
                        if (aData[i] instanceof jQuery && aData[i].is('td')) {
                            $(nRow).find('td:nth-child(' + (i + 1) + ')').replaceWith($(aData[i]));
                        } else {
                            $(nRow).find('td:nth-child(' + (i + 1) + ')').html(aData[i]);
                        }
                    }
                },
                "aaSorting": aaSorting,
                "oLanguage": Resources.DataTables,
                "aLengthMenu": [5, 10, 25, 50, 100]
            }));

            // Handle history change
            if (opts.history) {
                History.Adapter.bind(window, 'statechange', function () {
                    if (dt.data("history-pushing")) {
                        dt.data("history-pushing", false);
                        return;
                    }
                    var state = History.getState();
                    if (state && state.data && state.data.settings) {
                        var oSettings = dt.fnSettings();
                        if (oSettings) {
                            $.extend(oSettings, state.data.settings);
                            oSettings.oApi._fnDraw(oSettings);
                        }
                    }
                });
            }

            if (opts.filterDelay)
                dt.fnSetFilteringDelay();

            //if (opts.fixedHeader)
            //    new FixedHeader(dt, opts.fixedHeader);

            return dt;
        },
        updateCheckAll: function (selector) {
            var checks = $("[name=check-id]:not(:disabled)", selector);
            $(".checkall", selector).prop(
                "checked",
                checks.size() > 0 && checks.is(":not(:checked)") == false
            );
        },
        getServerOptions: function (selector, opts) {
            var options = {};
            if (opts.cols && opts.cols.length) {

                actionTemplate = $(selector + '-action-template').html();
                $(selector + '-action-template').remove();

                options.aoColumns = [];
                for (var i in opts.cols) {
                    var col = opts.cols[i];
                    var isObj = typeof (col) == "object";
                    var colDef = { "mData": isObj ? col.name : col };
                    if (isObj) {
                        if (col.render)
                            colDef.mRender = col.render;
                        else if (col.dateFormat)
                            colDef.mRender = function (dateFormat) {
                                return function (data, type, full) {
                                    if (!data) return "";
                                    return Globalize.format(new Date(data), dateFormat);
                                };
                            }(col.dateFormat);
                        else if (col.actionTemplate)
                            colDef.mRender = function (data, type, full) {
                                var result = "";
                                $(actionTemplate).each(function () {
                                    if (this.outerHTML) {
                                        var e = $(this);
                                        var hideIf = e.attr("hideIf");
                                        if (hideIf) {
                                            var s = hideIf.split(":");
                                            if (s[1] != full[s[0]]) {
                                                result += this.outerHTML;
                                            }
                                        } else {
                                            result += this.outerHTML;
                                        }

                                    }
                                });
                                return result.replace(/9999/g, data);
                            };
                        else if (col.isBoolean)
                            colDef.mRender = function (data, type, full) {
                                return '<input type="checkbox" ' + ((data == true) ? 'checked="checked"' : '') + ' disabled="disabled" />';
                            };
                        else if (col.isEnum)
                            colDef.mRender = function (texts) {
                                return function (data, type, full) {
                                    return '<span>' + texts[data] + '</span>';
                                };
                            }(col.texts);
                    }
                    options.aoColumns.push(colDef);
                }
            }

            options.fnServerData = function (sSource, aoData, fnCallback) {
                var data = {};
                for (var i in aoData) {
                    if (aoData[i].name == "iDisplayStart")
                        data.startIndex = aoData[i].value;
                    if (aoData[i].name == "iDisplayLength")
                        data.length = aoData[i].value;
                    if (aoData[i].name == "sEcho")
                        data.echo = aoData[i].value;
                    if (aoData[i].name == "sSearch")
                        data.search = aoData[i].value;
                    if (aoData[i].name.indexOf("iSortCol") > -1) {
                        if (!data.sortBy)
                            data.sortBy = [];
                        data.sortBy.push($("th", selector).eq(aoData[i].value).data("sort-field"));
                    }
                    if (aoData[i].name.indexOf("sSortDir") > -1) {
                        if (!data.sortDir)
                            data.sortDir = [];
                        data.sortDir.push(aoData[i].value);
                    }
                }
                var success = fnCallback;
                if (opts.html == true) {
                    data.echo = undefined;
                    success = function (json, textStatus, jqXHR) {
                        var aaData = [];
                        $(json.Html).find("tbody tr").each(function (i, tr) {
                            var row = [];
                            $(tr).find("td").each(function (i, td) {
                                row.push($(td));
                            });
                            aaData.push(row);
                        });
                        var result = {
                            aaData: aaData,
                            iTotalRecords: json.TotalRecords,
                            iTotalDisplayRecords: json.TotalRecords,
                            sEcho: data.echo
                        };
                        return fnCallback(result, textStatus, jqXHR);
                    };
                }
                $.ajax({
                    "dataType": 'json',
                    "type": "GET",
                    "url": opts.url,
                    "data": data,
                    "success": success,
                    "traditional": true
                });
            }
            return options;
        }
    };

    /**** HELPERS ****/

    function getQueryStringParam(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.href);
        if (results == null)
            return "";
        else
            return decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    /**** API EXTENSIONS ****/

    $.fn.dataTableExt.oApi.getTableAndNodes = function (oSettings) {
        var arr = this.fnGetNodes();
        arr.push(this);
        return arr;
    }
    $.fn.dataTableExt.oApi.checkPage = function (oSettings, check) {
        this.checkAll(check, this);
    }
    $.fn.dataTableExt.oApi.checkAll = function (oSettings, check, container) {
        container = container || this.getTableAndNodes();
        $(".checkall,[name=check-id]:not(:disabled)", container)
            .prop("checked", check !== false);
        return this;
    }
    $.fn.dataTableExt.oApi.uncheckAll = function () {
        return this.checkAll(false);
    }
    $.fn.dataTableExt.oApi.check = function (oSettings, id, check) {
        var check = $("[name=check-id][value=" + id + "]:not(:disabled)", this.getTableAndNodes());
        if (check.is(":radio") && check !== false)
            this.uncheckAll();
        check.prop("checked", check !== false);
        return this;
    }
    $.fn.dataTableExt.oApi.getCheckedIds = function () {
        var ids = [];
        this.$('[name=check-id]:checked').each(function () {
            ids.push($(this).val());
        });
        return ids;
    }
    $.fn.dataTableExt.oApi.getCheckedRows = function () {
        return this.$('[name=check-id]:checked')
            .map(function () {
                return $(this).parents("tr")[0];
            });
    }
    $.fn.dataTableExt.oApi.getCheckedDescriptions = function () {
        if (this.find(".datarow-description").size()) {
            return this.getCheckedRows()
                .find(".datarow-description")
                .map(function () { //this way avoids inverted order
                    return $.trim($(this).html());
                })
                .get();
        } else {
            return this.getCheckedRows()
                .map(function () { //this way avoids inverted order
                    var item = {};
                    $(this).find("[data-descriptor-field]").each(function (i, field) {
                        $field = $(field);
                        item[$field.data("descriptor-field")] = $.trim($field.html());
                    });
                    return item;
                })
                .get();
        }
    }
    $.fn.dataTableExt.oApi.deleteCheckedRows = function () {
        var table = this;
        table.getCheckedRows().each(function () {
            table.fnDeleteRow(this, null, false);
        });
        table.fnDraw();
        return this;
    }
    $.fn.dataTableExt.oApi.prependRow = function (oSettings, row, templated, func) {
        var _this = this;
        this.doAndReinit(function () {
            if (templated === true)
                row = $("<div>").mustache('datarow-template', row).children();
            _this.prepend(row);
            if (func) func();
        });
    }
    $.fn.dataTableExt.oApi.appendRow = function (oSettings, row, templated, func) {
        return this.appendRows([row], templated, func);
    }
    $.fn.dataTableExt.oApi.appendRows = function (oSettings, rows, templated, func) {
        var _this = this;
        return this.doAndReinit(function () {
            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                if (templated === true)
                    row = $("<div>").mustache('datarow-template', row).children();
                _this.append(row);
            }
            if (func) func();
        });
    }
    $.fn.dataTableExt.oApi.replaceRow = function (oSettings, id, row) {
        var rowToReplace = this.$('[name=check-id][value=' + id + ']').parents("tr");
        return this.doAndReinit(function () {
            rowToReplace.replaceWith(row);
        });
    }
    $.fn.dataTableExt.oApi.doAndReinit = function (oSettings, func) {
        this.uncheckAll();
        var selector = this.selector;
        this.fnDestroy();
        if (func)
            func();
        return DataTables.reinit(selector);
    }
    $.fn.dataTableExt.oApi.Reinit = function () {
        this.uncheckAll();
        var selector = this.selector;
        this.fnDestroy();
        return DataTables.reinit(selector);
    }
})(jQuery, window);

(function ($) {
    var re = /([^&=]+)=?([^&]*)/g;
    var decode = function (str) {
        return decodeURIComponent(str.replace(/\+/g, ' '));
    };
    $.parseParams = function (query) {
        var params = {}, e;
        if (query) {
            if (query.substr(0, 1) == '?') {
                query = query.substr(1);
            }

            while (e = re.exec(query)) {
                var k = decode(e[1]);
                var v = decode(e[2]);
                if (params[k] !== undefined) {
                    if (!$.isArray(params[k])) {
                        params[k] = [params[k]];
                    }
                    params[k].push(v);
                } else {
                    params[k] = v;
                }
            }
        }
        return params;
    };
})(jQuery);