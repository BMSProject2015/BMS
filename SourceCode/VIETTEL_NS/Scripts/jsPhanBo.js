var jsPhanBo_Url_Frame = '';
var jsPhanBo_Url = '';

function jsPhanBo_LoadLaiChiTiet() {
    var url = jsPhanBo_Url_Frame;
    var controls = $('input[search-control="1"]');
    var i;
    for (i = 0; i < controls.length; i++) {
        var field = $(controls[i]).attr("search-field");
        var value = $(controls[i]).val();
        url += "&" + field + "=" + encodeURI(value);
    }
    document.getElementById("ifrChiTietChungTu").src = url;
}

var jsPhanBo_Search_inteval = null;

function jsPhanBoNganh_Search_onkeypress(e) {
    jsPhanBo_Search_clearInterval();
    if (e.keyCode == 13) {
        jsPhanBo_LoadLaiChiTiet();
    }
    else {
        jsPhanBo_Search_inteval = setInterval(function () { jsPhanBo_Search_clearInterval(); jsPhanBo_LoadLaiChiTiet(); }, 2000);
    }
}

function jsPhanBo_Search_onkeypress(e) {
    jsPhanBo_Search_clearInterval();
    if (e.keyCode == 13) {
        jsPhanBo_LoadLaiChiTiet();
    }
    else {
        jsPhanBo_Search_inteval = setInterval(function () { jsPhanBo_Search_clearInterval(); jsPhanBo_LoadLaiChiTiet(); }, 2000);
    }
}

function jsPhanBo_TrinhDuyetTuChoi(iAction) {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.Bang_HamTruocKhiKetThuc(iAction);
}

function jsPhanBo_Search_clearInterval() {
    clearInterval(jsPhanBo_Search_inteval);
}