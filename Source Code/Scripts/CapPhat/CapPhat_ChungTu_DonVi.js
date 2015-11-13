var BangDuLieu_CoCotDuyet = false;
var BangDuLieu_CoCotTongSo = true;

var jsCapPhat_Url_Frame = '';
var jsCapPhat_Url = '';
function jsCapPhat_LoadLaiChiTiet() {
    var url = jsCapPhat_Url_Frame;
  
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
function jsPhanBo_Search_clearInterval() {
    clearInterval(jsPhanBo_Search_inteval);
}
function jsCapPhat_Search_onkeypress(e) {

    jsPhanBo_Search_clearInterval();
    if (e.keyCode == 13) {
        jsCapPhat_LoadLaiChiTiet();
    }
    else {
        jsPhanBo_Search_inteval = setInterval(function () { jsPhanBo_Search_clearInterval(); jsCapPhat_LoadLaiChiTiet(); }, 2000);
    }
}
function BangDuLieu_onLoad() {
    if (typeof Bang_arrCSMaCot["bDongY"] == "undefined") {
        BangDuLieu_CoCotDuyet = false;
    }
    else {
        BangDuLieu_CoCotDuyet = true;
    }
    if (typeof Bang_arrCSMaCot["rTongSo"] == "undefined") {
        BangDuLieu_CoCotTongSo = false;
    }
    else {
        BangDuLieu_CoCotTongSo = true;
    }
}

function BangDuLieu_onCellAfterEdit(h, c) {
    BangDuLieu_CapNhapLaiHangCha(h, c);
}

function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrType[c] == 1) {
        if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
            return;
        }
        BangDuLieu_TinhOConLai(h, c);
        BangDuLieu_TinhOTongSo(h);
        var csCha = h, GiaTri;
        while (Bang_arrCSCha[csCha] >= 0) {
            csCha = Bang_arrCSCha[csCha];
            GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
            Bang_GanGiaTriThatChoO(csCha, c, GiaTri);
            BangDuLieu_TinhOConLai(csCha, c);
            BangDuLieu_TinhOTongSo(csCha);
        }
    }
}

function BangDuLieu_TinhOConLai(h, c) {
    if (Bang_arrMaCot[c] != "rNgay" && Bang_arrMaCot[c] != "rSoNguoi") {
        var GiaTri1 = Bang_arrGiaTri[h][c - 2];
        var GiaTri2 = Bang_arrGiaTri[h][c - 1];
        var GiaTri3 = Bang_arrGiaTri[h][c];
        Bang_GanGiaTriThatChoO(h, c + 1, GiaTri1 - GiaTri2 - GiaTri3);
    }
}

function BangDuLieu_TinhOTongSo(h) {
    if (BangDuLieu_CoCotTongSo) {
        var GT = 0, c, cTongSo = Bang_arrCSMaCot["rTongSo"];
        var cMax = cTongSo - 1;
        for (c = Bang_nC_Fixed; c < cMax; c++) {
            if (Bang_arrMaCot[c] != "rChiTapTrung") {
                GT = GT + Bang_arrGiaTri[h][c];
            }
        }
        Bang_GanGiaTriThatChoO(h, cTongSo, GT);
        BangDuLieu_TinhOConLai(h, cTongSo);
    }
}

function BangDuLieu_TinhTongHangCon(csCha, c) {
    var h, vR = 0;
    for (h = 0; h < Bang_arrCSCha.length; h++) {
        if (csCha == Bang_arrCSCha[h]) {
            vR += parseFloat(Bang_arrGiaTri[h][c]);
        }
    }
    return vR;
}

var BangDuLieu_CheckAll_value = false;
function BangDuLieu_CheckAll() {
    BangDuLieu_CheckAll_value = !BangDuLieu_CheckAll_value;
    var value = "0";
    if (BangDuLieu_CheckAll_value) {
        value = "1";
    }
    else {
        value = "0";
    }
    var h, c = Bang_arrCSMaCot["bDongY"];
    for (h = 0; h < Bang_arrMaHang.length; h++) {
        if (Bang_arrLaHangCha[h] == false) {
            Bang_GanGiaTriThatChoO(h, c, value);
        }
    }
}