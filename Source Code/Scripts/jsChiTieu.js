var jsChiTieu_iID_MaChiTieu = '';
var jsChiTieu_Url_Frame = '';
var jsChiTieu_Url = '';

function jsChiTieu_LoadLaiChiTiet() {
    var url = jsChiTieu_Url_Frame;
    var controls = $('input[search-control="1"]');
    var i;
    for (i = 0; i < controls.length; i++) {
        var field = $(controls[i]).attr("search-field");
        var value = $(controls[i]).val();
        url += "&" + field + "=" + encodeURI(value);
    }
    document.getElementById("ifrChiTietChungTu").src = url;
}

var jsChiTieu_Search_inteval = null;

function jsChiTieu_Search_onkeypress(e) {
    jsChiTieu_Search_clearInterval();
    if (e.keyCode == 13) {
        jsChiTieu_LoadLaiChiTiet();
    }
    else {
        jsChiTieu_Search_inteval = setInterval(function () { jsChiTieu_Search_clearInterval(); jsChiTieu_LoadLaiChiTiet(); }, 2000);
    }
}

function jsChiTieu_TrinhDuyetTuChoi(iAction) {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.Bang_HamTruocKhiKetThuc(iAction);
}

function jsChiTieu_Search_clearInterval() {
    clearInterval(jsChiTieu_Search_inteval);
}