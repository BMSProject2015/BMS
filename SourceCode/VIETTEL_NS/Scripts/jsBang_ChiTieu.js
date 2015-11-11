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
    BangDuLieu_CapNhapLaiHangCha(h, c);
}

function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrMaCot[c].startsWith('r') == false) {
        return;
    }
    if (BangDuLieu_CoCotDuyet && c >= Bang_nC - 2) {
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

function BangDuLieu_TinhOConLai(h, c) {
    var GiaTri1 = Bang_arrGiaTri[h][c - 1];
    var GiaTri2 = Bang_arrGiaTri[h][c];
    Bang_GanGiaTriThatChoO(h, c + 1, GiaTri1 - GiaTri2);
}

function BangDuLieu_TinhOTongSo(h) {
    var GT = 0, c, cTongSo = Bang_arrCSMaCot["rTongSo"];
    var cMax = cTongSo - 2;
    for (c = Bang_nC_Fixed + 1; c < cMax; c = c + 3) {
        if (Bang_arrMaCot[c].startsWith('r') && Bang_arrMaCot[c] != "rChiTapTrung") {
            GT = GT + Bang_arrGiaTri[h][c];
        }
    }
    Bang_GanGiaTriThatChoO(h, cTongSo, GT);
    BangDuLieu_TinhOConLai(h, cTongSo);
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

/* Su kien BangDuLieu_onCellAfterEdit
*   - Muc dinh: Su kien xuat hien sau khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*/
function BangDuLieu_onCellAfterEdit(h, c) {
    BangDuLieu_CapNhapLaiHangCha(h, c);
}