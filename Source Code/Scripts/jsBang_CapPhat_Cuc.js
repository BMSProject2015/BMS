var BangDuLieu_CoCotDuyet = false;
var BangDuLieu_MaDonViChoCapPhat = "99";
var BangDuLieu_arrMaMucLucNganSach;
var BangDuLieu_arrChiSoNhom;
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";
var BangDuLieu_DuocSuaChiTiet = false;
var BangDuLieu_iID_MaChungTu = "";

function BangDuLieu_onLoad() {
    BangDuLieu_arrDonVi = document.getElementById('idDSDonVi').value.split("##");
    BangDuLieu_arrChiSoNhom = Bang_LayMang1ChieuGiaTri('idDSChiSoNhom');
    BangDuLieu_arrMaMucLucNganSach = Bang_LayMang1ChieuGiaTri('idMaMucLucNganSach');
    for (i = 0; i < BangDuLieu_arrChiSoNhom.length; i++) {
        BangDuLieu_arrChiSoNhom[i] = parseInt(BangDuLieu_arrChiSoNhom[i]);
    }
    if (typeof Bang_arrCSMaCot["bDongY"] == "undefined") {
        BangDuLieu_CoCotDuyet = false;
    }
    else {
        BangDuLieu_CoCotDuyet = true;
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

function BangDuLieu_DienPhanBo_DaCapPhat(h)
{
    var i, j, cMax = Bang_arrMaCot.length;
    if (BangDuLieu_CoCotDuyet) {
        cMax = Bang_arrMaCot.length - 2;
    }
    for (j = Bang_nC_Fixed + 1; j < cMax; j++) {
        if (Bang_arrMaCot[j] != "sTenDonVi") {
            Bang_GanGiaTriThatChoO(h, j, 0);
        }
    }
    var iID_MaMucLucNganSach = BangDuLieu_arrMaMucLucNganSach[h];
    var iID_MaDonVi = Bang_arrGiaTri[h][Bang_nC_Fixed];
    if (iID_MaDonVi != BangDuLieu_MaDonViChoCapPhat) {
        var TruongURL = "PhanBo_DaCapPhat";
        var DSGiaTri = iID_MaDonVi + ',' + iID_MaMucLucNganSach;
        var GT = BangDuLieu_iID_MaChungTu;
        var url = unescape(BangDuLieu_Url_getGiaTri + '?Truong=' + TruongURL + '&GiaTri=' + GT + '&DSGiaTri=' + DSGiaTri);
        $.getJSON(url, function (item) {
            var arrTG = item.data.split('#');
            var arrTruong = arrTG[0].split(',');
            var arrPhanBo = arrTG[1].split(',');
            var arrDaCap = arrTG[2].split(',');
            for (i = 0; i < arrTruong.length; i++) {
                for (j = Bang_nC_Fixed; j < Bang_arrMaCot.length; j++) {
                    if (arrTruong[i] == Bang_arrMaCot[j]) {
                        Bang_GanGiaTriThatChoO(h, j - 1, parseFloat(arrPhanBo[i]));
                        Bang_GanGiaTriThatChoO(h, j + 1, parseFloat(arrDaCap[i]));
                        BangDuLieu_TinhOConLai(h, j);
                        BangDuLieu_CapNhapLaiHangCha(h, j);
                        break;
                    }
                }
            }
            BangDuLieu_TinhOTongSo(h);
        });
    }
}


function BangDuLieu_KiemTraDonVi(h0) {
    var h, c, iMaNhom = BangDuLieu_arrChiSoNhom[h0];
    for (h = 0; h < Bang_nH; h++) {
        if (BangDuLieu_arrChiSoNhom[h] == iMaNhom) {
            if (h0 != h && Bang_arrGiaTri[h][Bang_nC_Fixed] == Bang_arrGiaTri[h0][Bang_nC_Fixed]) {
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
    if (c == Bang_nC_Fixed) {
        //Nhap don vi
        if (BangDuLieu_KiemTraDonVi(h) == false) {
            Bang_GanGiaTriThatChoO(h, Bang_nC_Fixed, Bang_GiaTriO_BeforEdit);
            return;
        }
        else {
            if (Bang_arrMaCot[c] == "iID_MaDonVi") {
                var TenDonVi = BangDuLieu_LayTenDonViTheoMa(Bang_arrGiaTri[h][c]);
                if (TenDonVi != "" && BangDuLieu_KiemTraDonVi(h)) {
                    Bang_GanGiaTriThatChoO_colName(h, "sTenDonVi", TenDonVi);
                    var i;
                    for (i = Bang_nC_Fixed + 1; i < Bang_nC - 1; i++) {
                        Bang_arrThayDoi[h][i] = true;
                    }
                }
            }
        }
        BangDuLieu_DienPhanBo_DaCapPhat(h);
    }
    else {
        BangDuLieu_CapNhapLaiHangCha(h, c);
    }
    return true;
}

function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
        return;
    }
    var j, cTongSo = Bang_arrCSMaCot["rTongSo"];
    var cMax = cTongSo - 3;
    if (c > 0) {
        BangDuLieu_TinhOConLai(h, c);
    }
    BangDuLieu_TinhOTongSo(h);
    var csCha = h, GiaTri;
    while (Bang_arrCSCha[csCha] >= 0) {
        csCha = Bang_arrCSCha[csCha];
        for (j = Bang_nC_Fixed + 2; j < cMax; j = j + 4) {
            //Tính tổng các ô con của cột phân bổ
            GiaTri = BangDuLieu_TinhTongHangCon(csCha, j - 1);
            Bang_GanGiaTriThatChoO(csCha, j - 1, GiaTri);

            //Tính tổng các ô con của cột cấp phát
            GiaTri = BangDuLieu_TinhTongHangCon(csCha, j);
            Bang_GanGiaTriThatChoO(csCha, j, GiaTri);

            //Tính tổng các ô con của cột đã cấp phát
            GiaTri = BangDuLieu_TinhTongHangCon(csCha, j + 1);
            Bang_GanGiaTriThatChoO(csCha, j + 1, GiaTri);

            //Tính tổng các ô con của cột còn lại
            BangDuLieu_TinhOConLai(csCha, j);    
        }        
        BangDuLieu_TinhOTongSo(csCha);
    }
}

function BangDuLieu_TinhOConLai(h, c) {
    var GiaTri1 = Bang_arrGiaTri[h][c - 1];
    var GiaTri2 = Bang_arrGiaTri[h][c];
    var GiaTri3 = Bang_arrGiaTri[h][c + 1];
    Bang_GanGiaTriThatChoO(h, c + 2, GiaTri1 - GiaTri2 - GiaTri3);
}

function BangDuLieu_TinhOTongSo(h) {
    var cTongSo = Bang_arrCSMaCot["rTongSo"];
    var cMax = cTongSo - 3;
    var GT_DaCap = 0, GT_PhanBo = 0, GT = 0, c;
    //tuannn sua them dk cot sTenCongTrinh 31/7/12
    for (c = Bang_nC_Fixed + 2; c < cMax; c = c + 4) {
        if (Bang_arrMaCot[c] != "rChiTapTrung" && Bang_arrMaCot[c] != "sTenCongTrinh") {
            GT_PhanBo = GT_PhanBo + Bang_arrGiaTri[h][c - 1];
            GT = GT + Bang_arrGiaTri[h][c];
            GT_DaCap = GT_DaCap + Bang_arrGiaTri[h][c + 1];
        }
    }
    Bang_GanGiaTriThatChoO(h, cTongSo - 1, GT_PhanBo);
    Bang_GanGiaTriThatChoO(h, cTongSo, GT);
    Bang_GanGiaTriThatChoO(h, cTongSo + 1, GT_DaCap);
    BangDuLieu_TinhOConLai(h, cTongSo);
}

function BangDuLieu_TinhTongHangCon(csCha, c) {
    var h, vR = 0;
    //tuannn sua them dk cot sTenCongTrinh 31/7/12
    for (h = 0; h < Bang_arrCSCha.length; h++) {
        if (csCha == Bang_arrCSCha[h] && Bang_arrMaCot[c] != "sTenCongTrinh") {
            vR += parseFloat(Bang_arrGiaTri[h][c]);
        }
    }
    return vR;
}

function BangDuLieu_ThemHangMoi(h, hGiaTri) {
    if (h != null && BangDuLieu_DuocSuaChiTiet) {
       // if (Bang_arrLaHangCha[hGiaTri] == false) {
            Bang_ThemHang(h, hGiaTri);
            //Them vao bang BangDuLieu_arrChiSoNhom
            BangDuLieu_arrChiSoNhom.splice(h, 0, Bang_TaoDoiTuongMoi(BangDuLieu_arrChiSoNhom[hGiaTri]));
            //Them vao bang BangDuLieu_arrMaMucLucNganSach
            BangDuLieu_arrMaMucLucNganSach.splice(h, 0, Bang_TaoDoiTuongMoi(BangDuLieu_arrMaMucLucNganSach[hGiaTri]));
            //BangDuLieu_arrMaMucLucNganSach[h] = "";
            //Gan cac gia tri tien bang 0
            for (var c = Bang_nC_Fixed + 1; c < Bang_nC; c++) {
               // if (Bang_arrLaHangCha[h] == false) {
                    Bang_GanGiaTriThatChoO(h, c, 0);
              //  }
            }
            //Gan lai ma hang ="": Hang moi
            Bang_arrMaHang[h] = "";
            //Gan lai o đơn vị bằng rỗng
            Bang_GanGiaTriThatChoO(h, Bang_nC_Fixed, "");
            Bang_keys.fnSetFocus(h, 0);
        //}
    }
}

function BangDuLieu_onKeypress_F2(h, c) {
    BangDuLieu_ThemHangMoi(h+1, h);
}

function BangDuLieu_onKeypress_Delete(h, c) {
    if (h != null) {
        if (BangDuLieu_DuocSuaChiTiet && Bang_arrLaHangCha[h] == false) {
            Bang_XoaHang(h);
            //Xoa ca vao bang BangDuLieu_arrChiSoNhom
            BangDuLieu_arrChiSoNhom.splice(h, 1);
            //Xoa ca vao bang BangDuLieu_arrMaMucLucNganSach
            BangDuLieu_arrMaMucLucNganSach.splice(h, 1);
            if (h < Bang_nH) {
                Bang_keys.fnSetFocus(h, c);
            }
            else if (Bang_nH > 0) {

            }
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

function BangDuLieu_HamTruocKhiKetThuc(iAction) {
    //Luu lai bang BangDuLieu_arrMaMucLucNganSach
    var strMaMucLucNganSach = "";
    for (i = 0; i < Bang_nH; i++) {
        if (i > 0) {
            strMaMucLucNganSach += ",";
        }
        strMaMucLucNganSach += BangDuLieu_arrMaMucLucNganSach[i];
    }
    document.getElementById("idMaMucLucNganSach").value = strMaMucLucNganSach;    
    return true;
}