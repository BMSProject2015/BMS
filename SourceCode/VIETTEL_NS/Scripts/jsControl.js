var jsControl_arrID = null;
var jsControl_arrCS = null;

function jsControl_Init() {
    if (jsControl_arrID != null) {
        return;
    }
    jsControl_arrID = new Array();
    jsControl_arrCS = new Array();
    //Tạo danh sách các trường có tab-index
    var controls = $('input');
    var i, j, tab_index;
    for (i = 0; i < controls.length; i++) {
        tab_index = $(controls[i]).attr("tab-index");
        if (typeof tab_index != "undefined") {
            if (!isFinite(tab_index)) {
                tab_index = i;
            }
            else {
                tab_index = parseInt(tab_index);
            }
            controlID = $(controls[i]).attr("id");
            if (tab_index < 0 || jsControl_arrID.length==0 || tab_index >= jsControl_arrCS[jsControl_arrID.length - 1]) {
                //Thêm mới control
                jsControl_arrID.push(controlID);
                jsControl_arrCS.push(tab_index);
            }
            else {
                var j0 = 0;
                for (j = 0; j < jsControl_arrCS.length; j++) {
                    if (tab_index < jsControl_arrCS[j]) {
                        j0 = j;
                        break;
                    }
                }
                for (j = jsControl_arrCS.length; j0 + 1 <= j && 1<=j; j--) {
                    jsControl_arrID[j] = jsControl_arrID[j - 1];
                    jsControl_arrCS[j] = jsControl_arrCS[j - 1];
                }
                jsControl_arrID[j] = controlID;
                jsControl_arrCS[j] = tab_index;
            }
        }
    }
    //Focus vào control đầu tiên
    if (jsControl_arrID.length > 0) {
        $('#' + jsControl_arrID[0]).focus();
    }
}

function jsControl_Next(ctlID, iTime) {
    if (typeof iTime == "undefined") {
        iTime = 300;
    }
    setTimeout('jsControl_fnSetNextControl("' + ctlID + '")', iTime);
}

function jsControl_fnSetNextControl(ctlID) {
    var i = -1;
    for (var j = 0; j < jsControl_arrID.length; j++) {
        if (jsControl_arrID[j] == ctlID) {
            i = j;
            break;
        }
    }
    if (i >= 0) {
        var ChuaChonDuoc = true;
        j = i - 1;
        while (ChuaChonDuoc && j != i) {
            i = i + 1;
            var ctlID = "";
            if (jsControl_arrID.length <= i) {
                i = 0;
            }
            ctlID = '#' + jsControl_arrID[i];
            if ($(ctlID).attr("khong-nhap") != "1") {
                if (jsInit_ControlID_Focus != null) {
                    if($('#' + jsInit_ControlID_Focus).attr("autocomplete-control") == "1"){
                        $('#' + jsInit_ControlID_Focus).autocomplete("close");
                        //jsInit_Autocomplete_onBlur(jsInit_ControlID_Focus);
                    }else if($('#' + jsInit_ControlID_Focus).attr("autocomplete-oldcontrol") == "1") {
                        $('#' + jsInit_ControlID_Focus).autocomplete("close");
                    }
                }
                $(ctlID).focus();
                ChuaChonDuoc = false;
            }
        }
    }
}