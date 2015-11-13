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

var jsCapPhat_Url_Frame = '';
var jsCapPhat_Url = '';
var BangDuLieu_CoCotTongSo = true;
var BangDuLieu_iLoai = 2;
var BangDuLieu_Url_F12 = "";
function jsCapPhat_LoadLaiChiTiet() {
    var url = jsCapPhat_Url_Frame;
    var controls = $('input[search-control="1"]');
    var i;
    for (i = 0; i < controls.length; i++) {
        var field = $(controls[i]).attr("search-field");
        var value = $(controls[i]).val();
        url += "&" + field + "=" + encodeURI(value);
    }
    if (typeof Bang_arrCSMaCot["rTongThu"] == "undefined") {
        BangDuLieu_CoCotTongSo = false;
    }
    else {
        BangDuLieu_CoCotTongSo = true;
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

function BangDuLieu_DienPhanBo_DaCapPhat(h) {
    var iID_MaMucLucNganSach = BangDuLieu_arrMaMucLucNganSach[h];
    var iID_MaDonVi = Bang_arrGiaTri[h][Bang_nC_Fixed];
    if (iID_MaDonVi != BangDuLieu_MaDonViChoCapPhat) {
        var TruongURL = "PhanBo_DaCapPhat";
        var DSGiaTri = iID_MaDonVi + ',' + iID_MaMucLucNganSach;
        var GT = BangDuLieu_iID_MaChungTu;
        var url = unescape(BangDuLieu_Url_getGiaTri + '?Truong=' + TruongURL + '&GiaTri=' + GT + '&DSGiaTri=' + DSGiaTri + '&iLoai=' + BangDuLieu_iLoai);
        $.getJSON(url, function (item) {
            Bang_GanGiaTriThatChoO_colName(h, "rKeHoach", item.data);
            BangDuLieu_CapNhapLaiHangCha(h, 11); // cot rkehoach

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
//cot nhap la cot 11-madonvi
    if (c == Bang_nC_Fixed+3) {
        //Nhap don vi
        //if (BangDuLieu_KiemTraDonVi(h) == false) {
           // Bang_GanGiaTriThatChoO(h, Bang_nC_Fixed, Bang_GiaTriO_BeforEdit);
           // return;
       // }
       // else {
            if (Bang_arrMaCot[c] == "iID_MaDonVi") {
                var TenDonVi = BangDuLieu_LayTenDonViTheoMa(Bang_arrGiaTri[h][c]);
                if (TenDonVi != "" && BangDuLieu_KiemTraDonVi(h)) {
                    Bang_GanGiaTriThatChoO_colName(h, "sTenDonVi", TenDonVi);
                    var i;
                    for (i = Bang_nC_Fixed + 1; i < Bang_nC - 1; i++) {
                        Bang_arrThayDoi[h][i] = true;
                    }
                }
                else {
                    Bang_GanGiaTriThatChoO_colName(h, "sTenDonVi", TenDonVi);
                }
            }
       // }
        if (BangDuLieu_iLoai == 1) {
            BangDuLieu_DienPhanBo_DaCapPhat(h);
        }

    }
    else {
       BangDuLieu_CapNhapLaiHangCha(h, c);
    }
    return true;
}

function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrType[c] == 1) {
        if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
            return;
        }
        //BangDuLieu_TinhOTongSo(h);
        BangDuLieu_TinhOConLai(h, c);
        var csCha = h, GiaTri;
//        while (Bang_arrCSCha[csCha] >= 0) {
//            csCha = Bang_arrCSCha[csCha];
//            GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
//            Bang_GanGiaTriThatChoO(csCha, c, GiaTri);
//            BangDuLieu_TinhOConLai(csCha, c);
//            // BangDuLieu_TinhOTongSo(csCha);
//        }

    }
}

function BangDuLieu_TinhOConLai(h, c) {
    if (BangDuLieu_iLoai == 1) {
        if (Bang_arrMaCot[c] == "rThu") {
            var GiaTri1 = Bang_arrGiaTri[h][c];
            var GiaTri2 = Bang_arrGiaTri[h][c + 1];
            Bang_GanGiaTriThatChoO_colName(h, "rTongThu", GiaTri1 - GiaTri2);
        }
        if (Bang_arrMaCot[c] == "rThoaiThu") {
            var GiaTri1 = Bang_arrGiaTri[h][c];
            var GiaTri2 = Bang_arrGiaTri[h][c - 1];
            Bang_GanGiaTriThatChoO_colName(h, "rTongThu", GiaTri2 - GiaTri1);
        }
    }
    else {


        if (Bang_arrMaCot[c] == "rTongThu") {
            var rKhauHaoTSCD = Bang_arrGiaTri[h][c + 1];
            var rTienLuong = Bang_arrGiaTri[h][c + 2];
            var rQTNSKhac = Bang_arrGiaTri[h][c + 3];
            var rChiPhiKhac = Bang_arrGiaTri[h][c + 4];
            var rTong = rKhauHaoTSCD + rTienLuong + rQTNSKhac + rChiPhiKhac;
            var rThu = Bang_arrGiaTri[h][c];

            Bang_GanGiaTriThatChoO_colName(h, "rChenhLech", rThu - rTong);
            BangDuLieu_TinhOCHuaPhanPhoi(h);

        }

        if (Bang_arrMaCot[c] == "rNopNSQP") {
            var rNopNSQP = Bang_arrGiaTri[h][c];
            var rTongThu = Bang_arrGiaTri[h][c + 1];
            if (rTongThu == 0) {
                Bang_GanGiaTriThatChoO_colName(h, "rTongThu", rNopNSQP);
                Bang_GanGiaTriThatChoO_colName(h, "rChenhLech", rNopNSQP);
                BangDuLieu_TinhOCHuaPhanPhoi(h);
            }
        }
        if (Bang_arrMaCot[c] == "rKhauHaoTSCD") {
            var rKhauHaoTSCD = Bang_arrGiaTri[h][c];
            var rTienLuong = Bang_arrGiaTri[h][c + 1];
            var rQTNSKhac = Bang_arrGiaTri[h][c + 2];
            var rChiPhiKhac = Bang_arrGiaTri[h][c + 3];
            var rTong = rKhauHaoTSCD + rTienLuong + rQTNSKhac + rChiPhiKhac;
            var rThu = Bang_arrGiaTri[h][c - 1];
           Bang_GanGiaTriThatChoO_colName(h, "rTongSo", rTong);
            Bang_GanGiaTriThatChoO_colName(h, "rChenhLech", rThu - rTong);
//            Bang_GanGiaTriThatChoO_colName(h, "rTongThu", rKhauHaoTSCD + rTienLuong + rQTNSKhac + rChiPhiKhac);
        }
        if (Bang_arrMaCot[c] == "rTienLuong") {
            var rKhauHaoTSCD = Bang_arrGiaTri[h][c - 1];
            var rTienLuong = Bang_arrGiaTri[h][c];
            var rQTNSKhac = Bang_arrGiaTri[h][c + 1];
            var rChiPhiKhac = Bang_arrGiaTri[h][c + 2];
            var rTong = rKhauHaoTSCD + rTienLuong + rQTNSKhac + rChiPhiKhac;
            var rThu = Bang_arrGiaTri[h][c - 2];
            Bang_GanGiaTriThatChoO_colName(h, "rTongSo", rTong);
            Bang_GanGiaTriThatChoO_colName(h, "rChenhLech", rThu - rTong);
           
        }
        if (Bang_arrMaCot[c] == "rQTNSKhac") {
            var rKhauHaoTSCD = Bang_arrGiaTri[h][c - 2];
            var rTienLuong = Bang_arrGiaTri[h][c - 1];
            var rQTNSKhac = Bang_arrGiaTri[h][c];
            var rChiPhiKhac = Bang_arrGiaTri[h][c + 1];
            var rTong = rKhauHaoTSCD + rTienLuong + rQTNSKhac + rChiPhiKhac;
            var rThu = Bang_arrGiaTri[h][c - 3];
            Bang_GanGiaTriThatChoO_colName(h, "rTongSo", rTong);
            Bang_GanGiaTriThatChoO_colName(h, "rChenhLech", rThu - rTong);
        }
        if (Bang_arrMaCot[c] == "rChiPhiKhac") {
            var rKhauHaoTSCD = Bang_arrGiaTri[h][c - 3];
            var rTienLuong = Bang_arrGiaTri[h][c - 2];
            var rQTNSKhac = Bang_arrGiaTri[h][c - 1];
            var rChiPhiKhac = Bang_arrGiaTri[h][c];
            var rTong = rKhauHaoTSCD + rTienLuong + rQTNSKhac + rChiPhiKhac;
            var rThu = Bang_arrGiaTri[h][c - 4];
            Bang_GanGiaTriThatChoO_colName(h, "rTongSo", rTong);
            Bang_GanGiaTriThatChoO_colName(h, "rChenhLech", rThu - rTong);
        }
        BangDuLieu_TinhOCHuaPhanPhoi(h);
       
    }

}
function BangDuLieu_TinhOCHuaPhanPhoi(h) {
    var GT = 0, cot, rNopNSNN = Bang_arrCSMaCot["rNopThueTNDN"], rNOPNSQP = Bang_arrCSMaCot["rNopNSQP"]; // tinh gia tri tu truong nop ngan sach
    var rChenhLech = Bang_arrGiaTri[h][rNopNSNN - 1];

    for (cot = rNopNSNN; cot < rNopNSNN + 7; cot++) {
        if (Bang_arrMaCot[cot].startsWith("r")) {
            GT = GT + Bang_arrGiaTri[h][cot];
        }
    }
    GT = GT + Bang_arrGiaTri[h][rNOPNSQP];
    Bang_GanGiaTriThatChoO_colName(h, "rSoChuaPhanPhoi", rChenhLech - GT);
}
function BangDuLieu_TinhOTongSo(h) {
    if (BangDuLieu_CoCotTongSo) {

        var GT = 0, c, cTongSo = Bang_arrCSMaCot["rTongThu"];
        var cMax = cTongSo - 1;
        for (c = Bang_nC_Fixed; c < cMax; c++) {
            if (Bang_arrMaCot[c] != "rKeHoach") {
                GT = GT + Bang_arrGiaTri[h][c];
            }
        }
        Bang_GanGiaTriThatChoO(h, cTongSo, GT);
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
            for (var c = Bang_nC_Fixed+5 ; c < Bang_nC; c++) {
                if (Bang_arrMaCot[c] == "sGhiChu" || Bang_arrMaCot[c] == "sSoCT" || Bang_arrMaCot[c] == "iID_MaDonVi" || Bang_arrMaCot[c] == "sTenDonVi") {
                    Bang_GanGiaTriThatChoO(h, c, "");
                }
                else  {
                    Bang_GanGiaTriThatChoO(h, c, 0);
                }
               
            }
            //Gan lai ma hang ="": Hang moi
            Bang_arrMaHang[h] = "_" + Bang_arrGiaTri[h-1][5];
            //Gan lai o đơn vị bằng rỗng
            //Bang_GanGiaTriThatChoO(h, Bang_nC_Fixed, "");
            Bang_keys.fnSetFocus(h, 0);
        }
    }
}

function BangDuLieu_onKeypress_F2(h, c) {
    BangDuLieu_ThemHangMoi(h + 1, h);
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
function BangDuLieu_onKeypress_F12(h, c) {
    document.location.href = BangDuLieu_Url_F12;
    //window.location.href = BangDuLieu_Url_F12;
}