$.fn.dataTableExt.oApi.fnReloadAjax = function (oSettings, sNewSource, fnCallback, bStandingRedraw) {

    var stadingRedraw = function (dt, iStart, oSettings) {
        oSettings._iDisplayStart = iStart;
        dt.oApi._fnCalculateEnd(oSettings);
        dt.fnDraw(false);
    }

    if (sNewSource !== undefined && sNewSource !== null) {
        oSettings.sAjaxSource = sNewSource;
    }
    var iStart = oSettings._iDisplayStart;
    var that = this;

    // Server-side processing should just call fnDraw
    if (oSettings.oFeatures.bServerSide) {
        if (bStandingRedraw === true) {
            stadingRedraw(that, iStart, oSettings);
        } else {
            this.fnDraw();
        }
        return;
    }

    this.oApi._fnProcessingDisplay(oSettings, true);
    var aData = [];

    this.oApi._fnServerParams(oSettings, aData);

    oSettings.fnServerData.call(oSettings.oInstance, oSettings.sAjaxSource, aData, function (json) {
        /* Clear the old information from the table */
        that.oApi._fnClearTable(oSettings);

        /* Got the data - add it to the table */
        var aData = (oSettings.sAjaxDataProp !== "") ?
            that.oApi._fnGetObjectDataFn(oSettings.sAjaxDataProp)(json) : json;

        for (var i = 0 ; i < aData.length ; i++) {
            that.oApi._fnAddData(oSettings, aData[i]);
        }

        oSettings.aiDisplay = oSettings.aiDisplayMaster.slice();

        that.fnDraw();

        if (bStandingRedraw === true) {
            stadingRedraw(that, iStart, oSettings);
        }

        that.oApi._fnProcessingDisplay(oSettings, false);

        /* Callback user function - for event handlers etc */
        if (typeof fnCallback == 'function' && fnCallback !== null) {
            fnCallback(oSettings);
        }
    }, oSettings);
};