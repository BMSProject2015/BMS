var BangDuLieu_arrGiaTriNhom;
var BangDuLieu_arrChiSoNhom;
var BangDuLieu_arrDonVi;
var BangDuLieu_hOld = -1;
var BangDuLieu_CoThayDoiTrenHang = false;
var BangDuLieu_MaDonViChoPhanBo = "99";
var BangDuLieu_TenDonViChoPhanBo = "Chờ phân bổ";

function BangDuLieu_onLoad() {
    BangDuLieu_arrDonVi = document.getElementById('idDSDonVi').value.split("##");
    BangDuLieu_arrChiSoNhom = document.getElementById('idDSChiSoNhom').value.split(",");
    BangDuLieu_arrGiaTriNhom = Bang_LayMang2ChieuGiaTri('idXauGiaTriNhom');
    var i, j;

    for (i = 0; i < BangDuLieu_arrChiSoNhom.length; i++) {
        BangDuLieu_arrChiSoNhom[i] = parseInt(BangDuLieu_arrChiSoNhom[i]);
    }
    for (i = 0; i < BangDuLieu_arrGiaTriNhom.length; i++) {
        for (j = 0; j < BangDuLieu_arrGiaTriNhom[i].length; j++) {
            BangDuLieu_arrGiaTriNhom[i][j] = parseFloat(BangDuLieu_arrGiaTriNhom[i][j]);
        }
    }
}

function BangDuLieu_LayTenDonViTheoMa(MaDonVi) {
    var strMaDonVi = String(MaDonVi);
    var i;
    for (i = 0; i < BangDuLieu_arrDonVi.length; i = i + 2) {
        if (BangDuLieu_arrDonVi[i] == strMaDonVi) {
            return BangDuLieu_arrDonVi[i + 1];
        }
    }
    return "";
}

function BangDuLieu_CapNhapLaiCacHangChoPhanBo(csH) {
    var iMaNhom = BangDuLieu_arrChiSoNhom[csH];
    if (iMaNhom == -1) {
        return;
    }
    var h, c, h0 = -1, arrGT = new Array();
    //Lay gia tri cua nhom
    for (c = Bang_nC_Fixed + 1; c < Bang_nC - 1; c++) {
        arrGT.push(BangDuLieu_arrGiaTriNhom[iMaNhom][c - Bang_nC_Fixed - 1]);
    }
    //Tim chi so hang dau tien cua nhom
    var csD = csH, csC = -1;
    for (h = csH-1; h >= 0; h--) {
        if (BangDuLieu_arrChiSoNhom[h] != iMaNhom) {
            break;
        }
        csD = h;
    }
    var csCotDonVi = Bang_arrCSMaCot["iID_MaDonVi"];
    for (h = csD; h < Bang_nH; h++) {
        if (BangDuLieu_arrChiSoNhom[h] == iMaNhom) {
            if (Bang_arrGiaTri[h][csCotDonVi] == BangDuLieu_MaDonViChoPhanBo) {
                //Hang don vi CHO PHAN BO: gan cac gia tri bang 0
                h0 = h;
                for (c = csCotDonVi + 1; c < Bang_nC - 1; c++) {
                    if (Bang_arrMaCot[c].startsWith('r') && Bang_arrMaCot[c].endsWith('_ChiTieu')==false) {
                        Bang_GanGiaTriThatChoO(h, c, 0);
                    }
                    else {
                        Bang_GanGiaTriThatChoO(h, c, '');
                    }
                }
            }
            else {
                //Hang don vi: tru gia tri cua don vi trong gia tri nhom
                for (c = Bang_nC_Fixed + 1; c < Bang_nC - 1; c++) {
                    if (Bang_arrMaCot[c].startsWith('r') && Bang_arrMaCot[c].endsWith('_ChiTieu') == false) {
                        arrGT[c - Bang_nC_Fixed - 1] -= Bang_arrGiaTri[h][c];
                    }
                }
             }
             csC = h;
        }
        else if (BangDuLieu_arrChiSoNhom[h] > iMaNhom) {
            break;
        }
    }
    if (h0 == -1) {
        //Neu khong CHO PHAN BO thi them 1 hang moi vao
        BangDuLieu_ThemHangMoi(csC + 1, csH);
        Bang_GanGiaTriThatChoO_colName(csC + 1, "iID_MaDonVi", BangDuLieu_MaDonViChoPhanBo);
        Bang_GanGiaTriThatChoO_colName(csC + 1, "TenDonVi", BangDuLieu_TenDonViChoPhanBo);
        h0 = csC + 1;
    }
    //Gan gia tri vao hang CHO PHAN BO
    for (c = Bang_nC_Fixed + 1; c < Bang_nC - 1; c++) {
        if (Bang_arrMaCot[c].startsWith('r') && Bang_arrMaCot[c].endsWith('_ChiTieu') == false) {
            Bang_GanGiaTriThatChoO(h0, c, arrGT[c - Bang_nC_Fixed - 1]);
        }
    }
    BangDuLieu_TinhOTongSo(h0);
}

function BangDuLieu_onCellBeforeEdit(h, c) {
    var csCotDonVi = Bang_arrCSMaCot["iID_MaDonVi"];
    if (c > csCotDonVi && Bang_arrGiaTri[h][csCotDonVi] == BangDuLieu_MaDonViChoPhanBo) {
        return false;
    }
    return true;
}

function BangDuLieu_onCellFocus(y, x) {
    var h = y;
    var c = x;
    var h0 = BangDuLieu_hOld;
    var csCotDonVi = Bang_arrCSMaCot["iID_MaDonVi"];
    if (h0 >= 0 && h0 != h && Bang_arrLaHangCha[h0] == false) {
        //Chuyen sang hang khac
        if (BangDuLieu_CoThayDoiTrenHang) {
            if (Bang_arrGiaTri[h0][csCotDonVi] != BangDuLieu_MaDonViChoPhanBo) {
                var CoCotKhac0 = false, i;
                for (i = Bang_nC_Fixed + 1; i < Bang_nC - 1; i++) {
                    if (Bang_arrMaCot[i].startsWith('r') &&
                        Bang_arrType[i] == 1 && 
                        Bang_arrGiaTri[h0][i] != 0) {
                            CoCotKhac0 = true;
                            break;
                    }
                }
                if (CoCotKhac0 == false) {
                    Bang_GanGiaTriThatChoO_colName(h0, "iID_MaDonVi", BangDuLieu_MaDonViChoPhanBo);
                    Bang_GanGiaTriThatChoO_colName(h0, "TenDonVi", BangDuLieu_TenDonViChoPhanBo);
                }
            }
            BangDuLieu_CapNhapLaiCacHangChoPhanBo(h0);
        }
        BangDuLieu_CoThayDoiTrenHang = false;
    }
    BangDuLieu_hOld = h;
}

function BangDuLieu_KiemTraDonVi(h0) {
    var h, c, iMaNhom = BangDuLieu_arrChiSoNhom[h0];
    var csD = h0;
    for (h = h0 - 1; h >= 0; h--) {
        if (BangDuLieu_arrChiSoNhom[h] != iMaNhom) {
            break;
        }
        csD = h;
    }
    var csCotDonVi = Bang_arrCSMaCot["iID_MaDonVi"];
    for (h = csD; h < Bang_nH; h++) {
        if (BangDuLieu_arrChiSoNhom[h] == iMaNhom) {
            if (h0 != h && Bang_arrGiaTri[h][csCotDonVi] == Bang_arrGiaTri[h0][csCotDonVi]) {
                return false;
            }
        }
        else if (BangDuLieu_arrChiSoNhom[h] > iMaNhom) {
            break;
        }
    }
    return true;
}

function BangDuLieu_onCellAfterEdit(h, c) {
    if (Bang_arrMaCot[c]=="iID_MaDonVi") {
        //Nhap don vi
        var TenDonVi = BangDuLieu_LayTenDonViTheoMa(Bang_arrGiaTri[h][c]);
        if (TenDonVi != "" && BangDuLieu_KiemTraDonVi(h)) {
            Bang_GanGiaTriThatChoO_colName(h, "TenDonVi", TenDonVi);
            var i;
            for (i = Bang_nC_Fixed + 1; i < Bang_nC - 1; i++) {
                Bang_arrThayDoi[h][i] = true;
            }
        }
        else {
            Bang_GanGiaTriThatChoO(h, c, BangDuLieu_MaDonViChoPhanBo);
            Bang_GanGiaTriThatChoO_colName(h, "TenDonVi", BangDuLieu_TenDonViChoPhanBo);
        }
    }
    //nghiepnc them khi them gia tri 0 o cac o truong tien, tranh ma don vi nhay ve 99
    if (Bang_arrMaCot[c].startsWith("r")) {
        var GiaTri = Bang_arrGiaTri[h][c];
        if (GiaTri == 0) GiaTri = GiaTri + 0.01;
        Bang_GanGiaTriThatChoO(h, c, GiaTri);
    }
    BangDuLieu_TinhOTongSo(h);
    BangDuLieu_CoThayDoiTrenHang = true;
}

function BangDuLieu_TinhOTongSo(h) {
    var GT = 0, c;
    var cs_rTongSo = Bang_arrCSMaCot["rTongSo"];
    for (c = Bang_nC_Fixed + 1; c < cs_rTongSo; c++) {
        if (Bang_arrMaCot[c].startsWith('r') && Bang_arrMaCot[c].endsWith('_ChiTieu')==false && Bang_arrMaCot[c] != "rChiTapTrung") {
            GT = GT + Bang_arrGiaTri[h][c];
        }
    }
    Bang_GanGiaTriThatChoO(h, cs_rTongSo, GT);
}

function BangDuLieu_ThemHangMoi(h, hGiaTri) {
    if (h != null) {
        if (Bang_arrLaHangCha[hGiaTri] == false) {
            Bang_ThemHang(h, hGiaTri);
            //Them ca vao bang BangDuLieu_arrChiSoNhom
            BangDuLieu_arrChiSoNhom.splice(h, 0, Bang_TaoDoiTuongMoi(BangDuLieu_arrChiSoNhom[hGiaTri]));
            //Gan cac gia tri tien bang 0
            if (Bang_arrLaHangCha[h] == false) {
                for (var c = Bang_nC_Fixed + 1; c < Bang_nC; c++) {
                    if (Bang_arrMaCot[c].startsWith('r')) {
                        Bang_GanGiaTriThatChoO(h, c, 0);
                    }
                    else {
                        Bang_GanGiaTriThatChoO(h, c, '');
                    }
                }
            }
            Bang_GanGiaTriThatChoO_colName(h, "iID_MaDonVi", BangDuLieu_MaDonViChoPhanBo);
            Bang_GanGiaTriThatChoO_colName(h, "TenDonVi", BangDuLieu_TenDonViChoPhanBo);
            var csCotDonVi = Bang_arrCSMaCot["iID_MaDonVi"];
            Bang_keys.fnSetFocus(h, 0);
            h = Bang_keys.Row();
        }
    }
}

function BangDuLieu_onKeypress_F2(h, c) {
    BangDuLieu_ThemHangMoi(h, h);
}

function BangDuLieu_onKeypress_Delete(h, c) {
    if (h != null) {
        if (Bang_arrLaHangCha[h] == false) {
            Bang_XoaHang(h);
            if (h < Bang_nH) {
                Bang_keys.fnSetFocus(h, c);
            }
            else if (Bang_nH > 0) {

            }
        }
    }
}



