var BangDuLieu_CoCotDuyet = false;
var BangDuLieu_CoCotTongSo = true;

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
    //tinh ty le
    var rKeHoach, rThucHien, rNamTruoc,rNTNN,rTHKH;
    rNamTruoc = Bang_arrGiaTri[h][2];
    rKeHoach = Bang_arrGiaTri[h][3];
    rDaThucHien = Bang_arrGiaTri[h][4];
    rThucHien = Bang_arrGiaTri[h][5];
    
    if(rNamTruoc!=0)
    {
        rNTNN = Math.round((rThucHien + rDaThucHien) / rNamTruoc * 100);
        Bang_GanGiaTriO(h, 6, rNTNN);
    }
    if (rKeHoach != 0) {
        rTHKH = Math.round((rThucHien + rDaThucHien) / rKeHoach * 100);
        Bang_GanGiaTriO(h, 7, rTHKH);
    }
   
    BangDuLieu_CapNhapLaiHangCha(h, c);
   
}

function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrType[c] == 1) {
        if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
            return;
        }
//        BangDuLieu_TinhOTongSo(h);
        var csCha = h, GiaTri;
        while (Bang_arrCSCha[csCha] >= 0) {
            csCha = Bang_arrCSCha[csCha];
            GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
            Bang_GanGiaTriThatChoO(csCha, c, GiaTri);
            //BangDuLieu_TinhOTongSo(csCha);
            BangDuLieu_TinhOTyLeCha(csCha);
        }
    }
}

function BangDuLieu_GanGiaTriChoHang(MaSo1,MaSo2) {
    var c = Bang_arrCSMaCot["sKyHieu"];
    var h,h1;
    var TrongKy, DauKy;
    for (h = 0; h < Bang_arrMaHang.length; h++) {
        if (Bang_arrGiaTri[h][c] == MaSo1) {
            TrongKy = Bang_arrGiaTri[h][3];
            DauKy = Bang_arrGiaTri[h][4];
        }
        if (Bang_arrGiaTri[h][c] == MaSo2) {
            h1 = h;
        }
    }
    Bang_GanGiaTriThatChoO(h1, 3, TrongKy);
    Bang_GanGiaTriThatChoO(h1, 4, DauKy);

}
function BangDuLieu_TinhOTongSo(h) {
    if(BangDuLieu_CoCotTongSo) {
        var GT = 0, c, cTongSo = Bang_arrCSMaCot["rTongSo"];
        var cMax = cTongSo - 1;
        for (c = Bang_nC_Fixed; c < cMax; c++) {
            if (Bang_arrMaCot[c] != "rChiTapTrung") {
                GT = GT + Bang_arrGiaTri[h][c];
            }
        }
        Bang_GanGiaTriThatChoO(h, cTongSo, GT);
    }
}

function BangDuLieu_TinhOTyLeCha(h) {
    var rKeHoach, rThucHien, rNamTruoc, rNTNN, rTHKH, rDaThucHien;
    rNamTruoc = Bang_arrGiaTri[h][2];
    rKeHoach = Bang_arrGiaTri[h][3];
    rDaThucHien = Bang_arrGiaTri[h][4];
    rThucHien = Bang_arrGiaTri[h][5];
if(rNamTruoc!=0)
    {
        rNTNN = Math.round((rThucHien + rDaThucHien) / rNamTruoc * 100);
        Bang_GanGiaTriO(h, 6, rNTNN);
    }
    if (rKeHoach != 0) {
        rTHKH = Math.round((rThucHien + rDaThucHien) / rKeHoach * 100);
        Bang_GanGiaTriO(h, 7, rTHKH);
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