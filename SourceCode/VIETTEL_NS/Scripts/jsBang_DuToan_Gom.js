/* BangDuLieu_CoCotTongSo: xac dinh bang co cot duyet hay khong*/
var BangDuLieu_CoCotDuyet = false;
/* BangDuLieu_CoCotTongSo: xac dinh bang co cot tong so hay khong*/
var BangDuLieu_CoCotTongSo = true;
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";

//link trang phân cấp
var BangDuLieu_Url_PhanCap = "";


var BangDuLieu_iID_MaChungTu = "";
var BangDuLieu_DuocSuaChiTiet = false;
var BangDuLieu_arrMaMucLucNganSach;
var BangDuLieu_arrChiSoNhom;
var sLNS = "";
var sL = "";
var sK = "";

var sM = "";
var sTM = "";
var sTTM = "";
/* Su kien BangDuLieu_onLoad
*/
function BangDuLieu_onLoad() {
    BangDuLieu_arrChiSoNhom = Bang_LayMang1ChieuGiaTri('idDSChiSoNhom');
    BangDuLieu_arrMaMucLucNganSach = Bang_LayMang1ChieuGiaTri('idMaMucLucNganSach');
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

    if (Bang_ChiDoc == false && Bang_nH == 0) {
        BangDuLieu_ThemHangMoi(-1);
       
    }
}

/* Ham BangDuLieu_TinhOTongSo
*   - Muc dinh: Tinh gia tri cua o tong so tren hang 'h'
*   - Dau vao:  + h: chi so hang can tinh tong so
*/
function BangDuLieu_TinhOTongSo(h) {
    if (BangDuLieu_CoCotTongSo) {
        var GT = 0, c, cTongSo = Bang_arrCSMaCot["rTongSo"];
        var cMax = cTongSo;
        var csUocThucHien = Bang_arrCSMaCot["rUocThucHien"];
        for (c = csUocThucHien + 1; c < cMax; c++) {
            if (Bang_arrMaCot[c] != "rChiTapTrung" && Bang_arrMaCot[c] != "sTenCongTrinh" && Bang_arrMaCot[c] != "rNgay" && Bang_arrMaCot[c] != "rSoNguoi" && Bang_arrMaCot[c] != "rSoNguoi_DonVi"
              && Bang_arrMaCot[c] != "rChiTaiKhoBac_DonVi"
               && Bang_arrMaCot[c] != "rTonKho_DonVi"
                && Bang_arrMaCot[c] != "rTuChi_DonVi"
                 && Bang_arrMaCot[c] != "rChiTapTrung_DonVi"
                  && Bang_arrMaCot[c] != "rHangNhap_DonVi"
                   && Bang_arrMaCot[c] != "rHangMua_DonVi"
            && Bang_arrMaCot[c] != "rHienVat_DonVi"
              && Bang_arrMaCot[c] != "rDuPhong_DonVi"
                && Bang_arrMaCot[c] != "rPhanCap_DonVi"
              && Bang_arrGiaTri[h][c] != "") {
                GT += parseFloat(Bang_arrGiaTri[h][c]);
            }
        }
        Bang_GanGiaTriThatChoO(h, cTongSo, GT);
    }
}

/* Ham BangDuLieu_TinhTongCacHang
*   - Muc dinh: Tính tổng tất cả các hàng
*/
function BangDuLieu_TinhTongCacHang() {
//    var h, c, S;
//    for (c = 0; c < Bang_nC; c++) {
//        if (Bang_arrMaCot[c].startsWith('r')) {
//            S = 0;
//            for (h = 0; h < Bang_nH - 1; h++) {
//                S += Bang_arrGiaTri[h][c];
//            }
//            Bang_GanGiaTriThatChoO(Bang_nH - 1, c, S);
//        }
//    }
}


/* Ham BangDuLieu_CheckAll_value
- Muc dinh: Check or Uncheck tat ca cac o co kieu du lieu la kieu 2(Checkbox)
*/
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
    for (h = 0; h < Bang_nH - 1; h++) {
        if (Bang_arrLaHangCha[h] == false) {
            Bang_GanGiaTriThatChoO(h, c, value);
        }
    }
}

/* Ham BangDuLieu_TinhTongHangCon
*   - Muc dinh: Tao 1 hang moi o tai vi tri 'h' lay du lieu tai vi tri 'hGiaTri'
*   - Dau vao:  + h: vi tri them
*               + hGiaTri: vi tri hang lay du lieu, =null: them 1 hang trong
*/
function BangDuLieu_ThemHangMoi(h, hGiaTri) {
    if (h != null && BangDuLieu_DuocSuaChiTiet) {
        if (Bang_arrLaHangCha[hGiaTri] == false) {
            Bang_ThemHang(h, hGiaTri);
            //Them vao bang BangDuLieu_arrChiSoNhom
            BangDuLieu_arrChiSoNhom.splice(h, 0, Bang_TaoDoiTuongMoi(BangDuLieu_arrChiSoNhom[hGiaTri]));
            //Them vao bang BangDuLieu_arrMaMucLucNganSach
            BangDuLieu_arrMaMucLucNganSach.splice(h, 0, Bang_TaoDoiTuongMoi(BangDuLieu_arrMaMucLucNganSach[hGiaTri]));
            //BangDuLieu_arrMaMucLucNganSach[h] = "";
            //Gan cac gia tri tien bang 0
            for (var c = Bang_nC_Fixed; c < Bang_nC; c++) {
                if (Bang_arrMaCot[c] == "sGhiChu" || Bang_arrMaCot[c] == "sSoCT" || Bang_arrMaCot[c] == "iID_MaDonVi" || Bang_arrMaCot[c] == "sTenDonVi") {
                    Bang_GanGiaTriThatChoO(h, c, "");
                }
                else {
                    Bang_GanGiaTriThatChoO(h, c, 0);
                }

            }
            //Gan lai ma hang ="": Hang moi
            var cs = Bang_arrCSMaCot['iID_MaMucLucNganSach'];
            Bang_arrMaHang[h] = "_" + Bang_arrGiaTri[h - 1][cs];
            Bang_GanGiaTriThatChoO(h, 26, Bang_arrGiaTri[h - 1][cs]);
            Bang_keys.fnSetFocus(h, 0);
        }
    }
         

}

/* Su kien BangDuLieu_onKeypress_F2
- Muc dinh: goi ham them hang khi an phim F2
*/
function BangDuLieu_onKeypress_F2(h, c) {

    BangDuLieu_ThemHangMoi(h + 1, h);

}
function BangDuLieu_onEnter_NotSetCellFocus(h, c) {
   // parent.Bang_HamTruocKhiKetThuc("");
//    var x = document.getElementById("ifrChiTietChungTu");
//    var y = (x.contentWindow || x.contentDocument);
//    var z = (y.document) ? y.document : y;
    //    y.Bang_HamTruocKhiKetThuc("");
    
    BangDuLieu_onKeypress_F2(Bang_nH - 2);
    
}
function BangDuLieu_onBodyFocus() {
    Bang_keys.focus();
}
/* Su kien BangDuLieu_onKeypress_Delete
- Muc dinh: goi ham xoa hang khi an phim DELETE
*/
function BangDuLieu_onKeypress_Delete(h, c) {
    if (BangDuLieu_DuocSuaChiTiet && h != null) {
        Bang_XoaHang(h);
        if (h < Bang_nH) {
            Bang_keys.fnSetFocus(h, c);
        }
        else if (Bang_nH > 0) {

        }
    }
}

function BangDuLieu_CapNhapLaiDuToanNamTruoc(h) {

    var TruongURL = "rTongSoNamTruoc";
    var cs = Bang_arrCSMaCot['iID_MaDonVi'];
    var DSGiaTri = Bang_arrGiaTri[h][cs] + "," + Bang_GhepGiaTriCuaMucLucNganSach(-1);
    var GT = BangDuLieu_iID_MaChungTu;
    var url = unescape(BangDuLieu_Url_getGiaTri + '?Truong=' + TruongURL + '&GiaTri=' + GT + '&DSGiaTri=' + DSGiaTri);
    $.getJSON(url, function (data) {
        cs = Bang_arrCSMaCot['rTongSoNamTruoc'];
        Bang_GanGiaTriThatChoO(h, cs, data.value);
    });
}
function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrType[c] == 1) {
        if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
            return;
        }
        var csCha = h, GiaTri;
        while (Bang_arrCSCha[csCha] >= 0) {
            csCha = Bang_arrCSCha[csCha];
            GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
            Bang_GanGiaTriThatChoO(csCha, c, GiaTri);
        }
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
/* Su kien BangDuLieu_onCellAfterEdit
*   - Muc dinh: Su kien xuat hien sau khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*/
function BangDuLieu_onCellAfterEdit(h, c) {
    if (h < Bang_nH - 1) {
        if (Bang_arrCSMaCot['sMoTa'] > c) {

        }
          
        else if (Bang_arrCSMaCot['rNgay'] <= c) {
            BangDuLieu_TinhOTongSo(h);
        }
        if (Bang_arrMaCot[c].startsWith("r")) {
            BangDuLieu_TinhTongCacHang();
        }
        if (Bang_arrMaCot[c] == "bPhanCap") {

            var iID_MaChiTiet = Bang_arrMaHang[h];

            var iID_MaChungTu = iID_MaChiTiet.split('_')[0];
          //  Bang_GanGiaTriThatChoO(h, 26, Bang_arrGiaTri[h - 1][cs]);
            var url = unescape(BangDuLieu_Url_PhanCap + '?iID_MaChungTu='+iID_MaChungTu);
            window.open(url, '_blank');
        }


//        var cs = Bang_arrCSMaCot['sTenDonVi'];
//        for (var j = cs + 1; j < Bang_nC; j++) {
//            if (Bang_arrMaCot[j].startsWith('r')) {
//                Bang_GanGiaTriThatChoO(h + 1, j, 0);

//            }
//            else {

//            }

        //        }
        var cs = 0;
        if (Bang_arrMaCot[c] == "sM") {
            cs = Bang_arrCSMaCot["sLNS"];
            Bang_GanGiaTriThatChoO(h, cs, sLNS);
            cs = Bang_arrCSMaCot["sL"];
            Bang_GanGiaTriThatChoO(h, cs, sL);
            cs = Bang_arrCSMaCot["sK"];
            Bang_GanGiaTriThatChoO(h, cs, sK);
        }
        if (Bang_arrMaCot[c] == "sLNS") {

            cs = Bang_arrCSMaCot["sL"];
            Bang_GanGiaTriThatChoO(h, cs, sL);
            cs = Bang_arrCSMaCot["sK"];
            Bang_GanGiaTriThatChoO(h, cs, sK);

            cs = Bang_arrCSMaCot["sM"];
            Bang_GanGiaTriThatChoO(h, cs, sM);
            cs = Bang_arrCSMaCot["sTM"];
            Bang_GanGiaTriThatChoO(h, cs, sTM);
            cs = Bang_arrCSMaCot["sTTM"];
            Bang_GanGiaTriThatChoO(h, cs, sTTM);  
        }
        var rTuChi = Bang_arrCSMaCot["rTuChi"];
        Bang_arrEdit[h][rTuChi] = true;
        var csCotMoTa = Bang_arrCSMaCot["sMoTa"];
        Bang_arrEdit[h][csCotMoTa] = false;
    }
    else {
        var rTuChi = Bang_arrCSMaCot["rTuChi"];
        Bang_arrEdit[h][rTuChi] = false;
    }
    BangDuLieu_CapNhapLaiHangCha(h, c);
    var tong = 0;
    for (var i = 0; i < Bang_nH - 1; i++) {
        if (Bang_arrLaHangCha[i] == false) {
            tong += Bang_arrGiaTri[i][c];
        }
    }
    if(c!=9)
    Bang_GanGiaTriThatChoO(Bang_nH - 1, c, tong);
 
}
function BangDuLieu_onCellKeypress(h, c) {
    var cs;
    var chuoi = Bang_arrMaCot[c];
    if (Bang_arrMaCot[c] == "sM") {
        var GiaTri = new String(Bang_arrGiaTri[h][c]);
        if (GiaTri.length == 4) {
            cs = Bang_arrCSMaCot["sTM"];
            Bang_keys.fnSetFocus(h, cs);
        }

    }
    if (Bang_arrMaCot[c] == "sTM") {

    }
    if (Bang_arrMaCot[c] == "sTTM") {

    }
    if (Bang_arrMaCot[c] == "sNG") {

    }
}
function BangDuLieu_onCellKeyUp(h, c, item) {   
//    var sM = new String(Bang_arrGiaTri[h][1]);
//    var cs;
//    var chuoi = Bang_arrMaCot[c];
//    if (Bang_arrMaCot[c] == "sM") {
//        var GiaTri = new String(Bang_arrGiaTri[h][c]);
//        if (GiaTri.length == 4 && GiaTri != sM) {
//            cs = Bang_arrCSMaCot["sTM"];
//            Bang_keys.fnSetFocus(h, cs);
//        }

//    }
//    if (Bang_arrMaCot[c] == "sTM") {

//    }
//    if (Bang_arrMaCot[c] == "sTTM") {

//    }
//    if (Bang_arrMaCot[c] == "sNG") {

//    }
}
function BangDuLieu_onCellBeforeEdit(h, c) {

}