var BangDuLieu_CoCotDuyet = false;

function BangDuLieu_onLoad() {
    if (typeof Bang_arrCSMaCot["bDongY"] == "undefined") {
        BangDuLieu_CoCotDuyet = false;
    }
    else {
        BangDuLieu_CoCotDuyet = true;
    }
}

function BangDuLieu_onCellAfterEdit(h, c) {
   BangDuLieu_TinhTongMotHang(h, c);   
}

function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
        return;
    }
    var csCha = h, GiaTri;
    while (Bang_arrCSCha[csCha] >= 0) {
        csCha = Bang_arrCSCha[csCha];
        GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
        Bang_GanGiaTriThatChoO(csCha, c, GiaTri);
    }
    BangDuLieu_TongHang(c);

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

function BangDuLieu_TinhTongMotHang(h,c) {
    var i = 0;
    var GT = 0;
    var cs = Bang_arrCSMaCot["rTongSo"];
    for (i = 1; i < Bang_nC_Slide - 1; i++) {
        GT=GT+parseFloat(Bang_arrGiaTri[h][i]);
    }
    Bang_GanGiaTriThatChoO(h, cs, GT);

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