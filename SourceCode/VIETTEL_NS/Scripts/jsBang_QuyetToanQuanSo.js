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
    if (Bang_arrMaCot[c] != "sGhiChu") {
        BangDuLieu_TinhTongMotHang(h, c);
        BangDuLieu_CapNhapLaiHangCha(h, c);
        
    }
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

function BangDuLieu_TinhTongMotHang(h, c) {
    var i = 0;
    var GT = 0;
    var cs = Bang_arrCSMaCot["rTongSo"];
    for (i = 2; i <= Bang_nC_Slide - 1; i++) {
        GT =GT+parseFloat(Bang_arrGiaTri[h][i]);
    }
    Bang_GanGiaTriThatChoO(h, cs, GT);
    BangDuLieu_CapNhapLaiHangCha(h, cs);

}
function BangDuLieu_TongHang(c) {
    if (Bang_arrMaCot[c] != "sGhiChu") {
        var h = 0;
        var cs = Bang_arrCSMaCot["sKyHieu"];
        var h100, h2, h3, h400, h500, h600, h700;
        var rSQ = Bang_arrCSMaCot["rSQ_KH"];
        if (rSQ != 2) {
            for (h = 0; h < Bang_arrMaHang.length; h++) {
                switch (Bang_arrGiaTri[h][cs].toString()) {
                case "100":
                    h100 = h;
                    break;
                case "2":
                    h2 = h;
                    break;
                case "3":
                    h3 = h;
                    break;
                case "400":
                    h400 = h;
                    break;
                case "500":
                    h500 = h;
                    break;
                case "600":
                    h600 = h;
                    break;
                case "700":
                    h700 = h;
                    break;
                }
            }
            var GT_400 = parseFloat(Bang_arrGiaTri[h100][c]) + parseFloat(Bang_arrGiaTri[h2][c]) - parseFloat(Bang_arrGiaTri[h3][c]);
            Bang_GanGiaTriThatChoO(h400, c, GT_400);
            var GT_500, GT_600, GT_700;
            GT_500 = parseFloat(Bang_arrGiaTri[h500][c]);
            GT_600 = parseFloat(Bang_arrGiaTri[h600][c]);
            GT_700 = GT_400 + GT_500 - GT_600;
            Bang_GanGiaTriThatChoO(h700, c, GT_700);
        }
    }
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