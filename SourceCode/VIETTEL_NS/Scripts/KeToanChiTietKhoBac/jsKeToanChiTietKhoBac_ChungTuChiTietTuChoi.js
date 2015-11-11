var jsKeToan_url_ChungTuChiTiet;
var jsKeToan_url_KeToanTongHop;
//<<<<<<<<<<<<
//Các hàm từ Frame chứng từ chi tiết gọi
function jsKeToan_LoadLaiChungTu() {
    var url = jsKeToan_url_ChungTuChiTiet;
    var sSoChungTu = document.getElementById("txtSoChungTu").value;
    url += "?sSoChungTu=" + sSoChungTu;
    document.getElementById("ifrChungTuChiTiet").src = url;
}
var jsKeToan_Search_inteval = null;

function jsKeToan_Search_onkeypress(e) {
    jsKeToan_Search_clearInterval();
    if (e.keyCode == 13) {
        jsKeToan_LoadLaiChungTu();
    }
    else {
        jsKeToan_Search_inteval = setInterval(function () { jsKeToan_Search_clearInterval(); jsKeToan_LoadLaiChungTu(); }, 2000);
    }
}

function jsKeToan_Search_clearInterval() {
    clearInterval(jsKeToan_Search_inteval);
}
//>>>>>>>>>>>>
function ChuyenSangTrang_KeToanTongHop(iID_MaChungTu, iID_MaChungTuChiTiet) {
    var url = jsKeToan_url_KeToanTongHop;
    url += "?iID_MaChungTu=" + iID_MaChungTu;
    url += "&iID_MaChungTuChiTiet=" + iID_MaChungTuChiTiet;
    location.href = url;
}