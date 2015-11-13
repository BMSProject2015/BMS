/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";
var BangDuLieu_hOld = -1;
var BangDuLieu_cOld = -1;
var BangDuLieu_CoResetDDL = false;

/* Su kien BangDuLieu_onLoad
*/
function BangDuLieu_onLoad() {
    if (Bang_nH == 0) {
        BangDuLieu_onKeypress_F2();
    }
    Bang_keys.fnSetFocus(Bang_nH-1, 0);
    
}

function BangDuLieu_onCellKeyUp(h, c, e, iKey) {
    if (iKey == 13) {
        if (Bang_LayGiaTri(h, "sTenTrangThaiDuyet") != '') {
            parent.GoiHam_ChungTuChiTiet_BangDuLieu_fnSetFocus();
            return false;
        }
        
    }
    return true;
}

var BangDuLieu_hLuu;
var BangDuLieu_cLuu;
function BangDuLieu_onCellBeforeFocus(h, c) {
    BangDuLieu_hLuu = h;
    BangDuLieu_cLuu = c;
    var h0 = BangDuLieu_hOld;
    var c0 = BangDuLieu_cOld;
    var csCotDonVi = Bang_arrCSMaCot["iNgay"];
    if (h0 >= 0 && c0 >= 0 && h0 != h) {
        //Chuyen sang hang khac
        return parent.ChungTu_KiemTraCapNhapChiTietChungTu(h0, c0);
    }
    return true;
}


function BangDuLieu_onCellFocus(h, c) {
    BangDuLieu_hLuu = -1;
    BangDuLieu_cLuu = -1;
    var h0 = BangDuLieu_hOld;
    var c0 = BangDuLieu_cOld;
    var csCotDonVi = Bang_arrCSMaCot["iNgay"];
    if (h >= 0 && h0 != h) {
        //Chuyen sang hang khac
        parent.ChungTu_ThayDoiMaChungTu();
    }
    BangDuLieu_hOld = h;
    BangDuLieu_cOld = c;
}

/* Su kien BangDuLieu_onCellAfterEdit
*   - Muc dinh: Su kien xuat hien sau khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*/
function BangDuLieu_onCellAfterEdit(h, c) {
    if (Bang_arrMaCot[c] == 'iNgay') {
        parent.GoiHam_ChungTuChiTiet_BangDuLieu_fnSetFocus();
        return false;
    }
    if (Bang_arrMaCot[c] == 'bDongYChiTiet') {
        if (Bang_arrGiaTri[h][c] == "1") {
            parent.GoiHam_ChungTuChiTiet_BangDuLieu_CheckAll(true);
        }
        else {
            parent.GoiHam_ChungTuChiTiet_BangDuLieu_CheckAll(false);
        }
        return false;
    }
    return true;
}

/* Ham BangDuLieu_TinhTongHangCon
*   - Muc dinh: Tao 1 hang moi o tai vi tri 'h' lay du lieu tai vi tri 'hGiaTri'
*   - Dau vao:  + h: vi tri them
*               + hGiaTri: vi tri hang lay du lieu, =null: them 1 hang trong
*/
function BangDuLieu_ThemHangMoi(h) {
    var csH = Bang_nH;
    Bang_ThemHang(csH);
    if (h >= 0) {
        BangDuLieu_hOld = h + 1;
    }
}

/* Su kien BangDuLieu_onKeypress_F2
- Muc dinh: goi ham them hang khi an phim F2
*/
function BangDuLieu_onKeypress_F2(h, c) {
    var csH = Bang_nH;
    if (Bang_ChiDoc == false) {
        {
            BangDuLieu_ThemHangMoi(csH);
        }
        //Gán các giá trị của hàng mới thêm =0

        for (j = 0; j < Bang_nC; j++) {
            if (Bang_arrMaCot[j] == "sSoChungTu" ||
                Bang_arrMaCot[j] == "iNgay" ||
                Bang_arrMaCot[j] == "sNoiDung" ||
                Bang_arrMaCot[j] == "bDongYChiTiet") {
                Bang_arrEdit[csH][j] = true;
            }
            else {
                Bang_arrEdit[csH][j] = false;
            }
        }

        BangDuLieu_CapNhapSTT();

        var url = urlServerPath + "KTCS_ChungTuChiTiet/get_ThongTinChungTuMoi";
        $.getJSON(url, function (item) {
            var cs;
            var iID_MaChungTu = item.iID_MaChungTu;
            cs = Bang_arrCSMaCot["sSoChungTu"];
            Bang_GanGiaTriThatChoO(csH, cs, item.sSoChungTu);
            cs = Bang_arrCSMaCot["iNgay"];
            Bang_GanGiaTriThatChoO(csH, cs, item.iNgay);
            //Bang_HideCloseDialog();
            //Sua MaHang="": Day la hang them moi
            Bang_arrMaHang[csH] = iID_MaChungTu;
            cs = Bang_arrCSMaCot["sSoChungTu"];
            Bang_keys.fnSetFocus(csH, cs);
            parent.ChungTu_ThayDoiMaChungTu();
        });
        //Bang_ShowCloseDialog();
    }
}

/* Su kien BangDuLieu_onKeypress_Delete
- Muc dinh: goi ham xoa hang khi an phim DELETE
*/
function BangDuLieu_onKeypress_Delete(h, c) {
    if (Bang_ChiDoc==false && h != null) {
        Bang_XoaHang(h);
        if (h < Bang_nH) {
            Bang_keys.fnSetFocus(h, c);
        }
        else if (Bang_nH > 0) {

        }
        BangDuLieu_CapNhapSTT();
    }
}

function BangDuLieu_CapNhapSTT() {
    for (var h = 0; h < Bang_nH; h++) {
        var cs = Bang_arrCSMaCot["sSTT"];
        Bang_GanGiaTriThatChoO(h, cs, h + 1);
    }
}

function BangDuLieu_onCellValueChanged(h, c) {
    if (h == Bang_keys.Row()) {
        if (Bang_arrMaCot[c] == "iID_MaChungTu") {
            parent.ChungTu_ThayDoiMaChungTu();
        }
        if (Bang_arrMaCot[c] == "iNgay") {
            parent.ChungTu_ThayDoiNgay(Bang_arrGiaTri[h][c]);
        }
    }
}

function BangDuLieu_onKeypress_F10(h, c, iAction) {
    if (typeof iAction == "undefined") {
        iAction = "";
    }
    var bChon = parent.GoiHam_ChungTu_Bang_LayTruong("bChon");
    if (iAction == 1) {
        if (bChon == 0) {
            alert("Bạn phải chọn chứng từ ghi sổ để từ chối!");
            return false;
        }
    }
    if (iAction == 2) {
        if (bChon == 0) {
            alert("Bạn phải chọn chứng từ ghi sổ để trình duyệt!");
            return false;
        }
    }
    if (typeof h == "undefined" || h == null || h < 0) {
        h = Bang_keys.Row();
    }
    if (typeof c == "undefined" || c == null || c < 0) {
        c = Bang_keys.Col();
    }
    if (h != null && c != null) {
        var iID_MaChungTu = Bang_arrMaHang[h];
        if (document.getElementById("idAction")) document.getElementById("idAction").value = iAction;
        if (document.getElementById("id_iID_MaChungTu_Action")) {
            document.getElementById("id_iID_MaChungTu_Action").value = iID_MaChungTu;
        }

        parent.ChungTu_YeuCauLuuDuLieu(iID_MaChungTu, Bang_arrHangDaXoa[h], iAction);
        jsKeToan_CheckChungTuGhiSo = parent.jsKiemTraChungSoChungTuGhiSo(iID_MaChungTu, parent.GoiHam_ChungTu_Bang_LayTruong("sSoChungTu"));
    }
    return false;
}

function BangDuLieu_onBodyFocus() {
    Bang_keys.focus();
}

function BangDuLieu_onBodyBlur() {
    Bang_keys.blur();
}

function ChungTuChiTiet_ThayDoiTruongNoiDung(GiaTri) {
    var csH = Bang_nH;
    var c = Bang_arrCSMaCot["sNoiDung"];
    if (Bang_arrGiaTri[csH-1][c] == "") {
        Bang_GanGiaTriThatChoO(Bang_keys.Row(), c, GiaTri);
    }
    else {
        Bang_GanGiaTriThatChoO(Bang_keys.Row(), c, GiaTri);
    }
}

//<<<<<<<<<<<<
//Các hàm do Parent gọi
function BangDuLieu_LayMaChungTu() {
    var h = Bang_keys.Row();
    if (h >= 0) {
        return Bang_arrMaHang[h];
    }
    return "";
}

function BangDuLieu_fnSetFocus() {
    Bang_keys.fnSetFocus(BangDuLieu_hLuu, BangDuLieu_cLuu);
}

function BangDuLieu_GanTruong(TenTruong, GiaTri) {
    var h = Bang_keys.Row();
    var cs = Bang_arrCSMaCot[TenTruong];
    Bang_GanGiaTriO(h, cs, GiaTri);
}

function BangDuLieu_LayTruong(TenTruong) {
    var h = Bang_keys.Row();
    return Bang_LayGiaTri(h, TenTruong);
}


function BangDuLieu_ChungTuChiTiet_Saved() {
    if (BangDuLieu_hLuu && BangDuLieu_hLuu >= 0) {
        var iID_MaChungTu = Bang_arrMaHang[BangDuLieu_hLuu];
        document.getElementById("id_iID_MaChungTu_Focus").value = iID_MaChungTu;
    }
    Bang_HamTruocKhiKetThuc(document.getElementById("idAction").value);
    return false;
}
//>>>>>>>>>>>>

//<<<<<<<<<<<<
//Các hàm gọi Parent

//>>>>>>>>>>>>