var BangDuLieu_CoCotDuyet = false;
var BangDuLieu_CoCotTongSo = true;
var jsQuyetToan_Url_Frame = "";
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
    if (Bang_arrMaCot[c] == "rDonViDeNghi") {
        var rSoTien = Bang_arrGiaTri[h][c];
        var rTuChi = Bang_arrGiaTri[h][c + 1];
        if (rTuChi == 0 || Bang_GiaTriO_BeforEdit == rTuChi) {
            Bang_GanGiaTriThatChoO_colName(h, "rTuChi", rSoTien);
            BangDuLieu_CapNhapLaiHangCha(h, c + 1);
            var tongDuyet = 0;
            for (var i = 0; i < Bang_nH - 1; i++) {
                if (Bang_arrLaHangCha[i] == false) {
                    tongDuyet += Bang_arrGiaTri[i][c+1];
                }
            }
            Bang_GanGiaTriThatChoO(Bang_nH - 1, c + 1, tongDuyet);

        }
    }
    var tong = 0;
    for (var i = 0; i < Bang_nH-1; i++) {
        if (Bang_arrLaHangCha[i] == false)  {
            tong += Bang_arrGiaTri[i][c];
        }
    }
    Bang_GanGiaTriThatChoO(Bang_nH-1, c, tong);
    BangDuLieu_CapNhapLaiHangCha(h, c);
}

function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrType[c] == 1) {
        if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
            return;
        }
        BangDuLieu_TinhOConLai(h, c);
        BangDuLieu_TinhOConLai_CotDonVi(h, c);
        BangDuLieu_TinhOTongSo(h);
        var csCha = h, GiaTri;
        while (Bang_arrCSCha[csCha] >= 0) {
            csCha = Bang_arrCSCha[csCha];
            GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
            Bang_GanGiaTriThatChoO(csCha, c, GiaTri);
            BangDuLieu_TinhOConLai(csCha, c);     
            BangDuLieu_TinhOConLai_CotDonVi(csCha, c);                 
            BangDuLieu_TinhOTongSo(csCha);
        }
    }
}

function BangDuLieu_TinhOConLai(h, c) {
//    var ColName = Bang_arrMaCot[c];
//    if (Bang_arrMaCot[c] != "rNgay" && Bang_arrMaCot[c] != "rSoNguoi" && ColName.indexOf("_DonVi")<0) {
//        var GiaTri1 = Bang_arrGiaTri[h][c - 1];
//        var GiaTri2 = Bang_arrGiaTri[h][c];
//        var GiaTri3 = Bang_arrGiaTri[h][c + 1];
//        Bang_GanGiaTriThatChoO(h, c + 2, GiaTri1 - GiaTri2 - GiaTri3);
//    }
}

function BangDuLieu_TinhOConLai_CotDonVi(h, c) {
    var ColName = Bang_arrMaCot[c];
    if (Bang_arrMaCot[c] != "rNgay" && Bang_arrMaCot[c] != "rSoNguoi" && ColName.indexOf("_DonVi") > 0) {
        var GiaTri1 = Bang_arrGiaTri[h][c];
        if (Bang_arrGiaTri[h][c + 2] == 0 ) {
            Bang_GanGiaTriThatChoO(h, c + 2, GiaTri1);
            BangDuLieu_TinhOConLai(h, c + 2);
        }
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

//function BangDuLieu_onKeypress_F4(h, c) {
//    parent.jsBH_Dialog_Show();
//}


//function BangDuLieu_onKeypress_F5(h, c) {
//    parent.jsGTST_Dialog_Show();
//}

//function BangDuLieu_onKeypress_F6(h, c) {
//    parent.jsGTBL_Dialog_Show();
//}
