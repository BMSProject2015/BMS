/* BangDuLieu_CoCotTongSo: xac dinh bang co cot duyet hay khong*/
var BangDuLieu_CoCotDuyet = false;
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";
var strParentID = "";
var BangDuLieu_sPhuCap_ChucVu_CongThuc = "";


var BangDuLieu_arrDSTruongBangLuongChiTiet = new Array();
var BangDuLieu_arrDSTruong = new Array();
var BangDuLieu_arrBangLuongChiTiet = new Array();

/* Su kien BangDuLieu_onLoad
*/
function BangDuLieu_onLoad() {
    if (typeof Bang_arrCSMaCot["bDongY"] == "undefined") {
        BangDuLieu_CoCotDuyet = false;
    }
    else {
        BangDuLieu_CoCotDuyet = true;
    }
    if (Bang_nH == 0) {
        BangDuLieu_ThemHangMoi();
    }
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
    for (h = 0; h < Bang_arrMaHang.length; h++) {
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
    var csH = 0;
    if (h != null) {
        //Thêm 1 hàng mới vào hàng h
        csH = h;
    }
    else {
        //Thêm 1 hàng mới vào cuối bảng
        csH = Bang_nH;
    }
    Bang_ThemHang(csH);
    //Sua MaHang="": Day la hang them moi
    Bang_arrMaHang[csH] = "";
    var c;
    for (c = 0; c < Bang_nC; c++) {
        Bang_arrEdit[csH][c] = false;
    }
    c = Bang_arrCSMaCot["sMaHT"];
    Bang_arrEdit[csH][c] = true;

    Bang_keys.fnSetFocus(csH, 0);
}

/* Su kien BangDuLieu_onKeypress_F2
- Muc dinh: goi ham them hang khi an phim F2
*/
function BangDuLieu_onKeypress_F2(h, c) {
    BangDuLieu_ThemHangMoi(h + 1, h);
}

/* Su kien BangDuLieu_onKeypress_Delete
- Muc dinh: goi ham xoa hang khi an phim DELETE
*/
function BangDuLieu_onKeypress_Delete(h, c) {
    if (h != null) {
        Bang_XoaHang(h);
        if (h < Bang_nH) {
            Bang_keys.fnSetFocus(h, c);
        }
        else if (Bang_nH > 0) {

        }
    }
}

function BangDuLieu_ThucHienCongThuc_replace(sCongThuc, sTu, sGiaTri) {
    while (sCongThuc.indexOf(sTu) >= 0) {
        sCongThuc = sCongThuc.replace(sTu, sGiaTri);
    }
    return sCongThuc;
}

function Control_onFocus() {
    var i, TenTruong, GiaTri, ctlID;
    for (i = 0; i < BangDuLieu_arrDSTruongBangLuongChiTiet.length; i++) {
        TenTruong = BangDuLieu_arrDSTruongBangLuongChiTiet[i];
        ctlID = strParentID + '_' + TenTruong;
        if (document.getElementById(ctlID)) {
            GiaTri = $('#' + ctlID).val();
            if (TenTruong.startsWith('r') || TenTruong.startsWith('i')) {
                if (GiaTri == '') {
                    GiaTri = '0';
                }
            }
            BangDuLieu_arrBangLuongChiTiet['[' + TenTruong + ']'] = GiaTri;
        }
    }
}

function Control_onKeypress_F10() {
    Bang_HamTruocKhiKetThuc();
}

function Control_onKeypress_Esc() {
    parent.jsLuong_Dialog_Close();
}

function BangDuLieu_ThucHienCongThuc_sPhuCap_ChucVu(sPhuCap_ChucVu_HeSo) {
    var sCongThuc = BangDuLieu_sPhuCap_ChucVu_CongThuc;
    if ($.trim(sCongThuc) == "") {
        return 0;
    }

    sCongThuc = BangDuLieu_ThucHienCongThuc_replace(sCongThuc, "[rHeSo]", sPhuCap_ChucVu_HeSo);

    var TenTruong, GiaTri;
    var okTiep = true;
    var TenTruongCu = '';
    while (okTiep) {
        TenTruong = LayTruongTrongCongThuc(sCongThuc);
        if (TenTruong != "" && TenTruong != TenTruongCu) {
            TenTruongCu = TenTruong;
            TenTruong = '[' + TenTruong + ']';
            GiaTri = BangDuLieu_arrBangLuongChiTiet[TenTruong];
            sCongThuc = BangDuLieu_ThucHienCongThuc_replace(sCongThuc, TenTruong, GiaTri);
        }
        else {
            okTiep = false;
        }
    }
    return eval(sCongThuc);
}

function BangDuLieu_ThucHienCongThuc(h, sCongThuc) {
    if ($.trim(sCongThuc) == "") {
        return 0;
    }
    var TenTruong, GiaTri;

    GiaTri = Bang_LayGiaTri(h, "rHeSo");
    sCongThuc = BangDuLieu_ThucHienCongThuc_replace(sCongThuc, '[rHeSo]', GiaTri);

    var okTiep = true;
    var TenTruongCu = '';
    while (okTiep)
    {
        TenTruong = LayTruongTrongCongThuc(sCongThuc);
        if (TenTruong != "" && TenTruong != TenTruongCu) {
            TenTruongCu = TenTruong;
            TenTruong = '[' + TenTruong + ']';
            GiaTri = BangDuLieu_arrBangLuongChiTiet[TenTruong];
            sCongThuc = BangDuLieu_ThucHienCongThuc_replace(sCongThuc, TenTruong, GiaTri);
        }
        else {
            okTiep = false;
        }
    }
    return eval(sCongThuc);
}

/* Su kien BangDuLieu_onCellAfterEdit
*   - Muc dinh: Su kien xuat hien sau khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*               + item: dữ liệu sau khi chọn Autocomplete
*/
function BangDuLieu_onCellAfterEdit(h, c) {
    if (Bang_arrMaCot[c] == "sMaHT") {
        BangDuLieu_NhapMaHT(h, c);
    }
    else {
//        if (Bang_arrMaCot[c] == "rHeSo") {
//            var rHeSo = Bang_arrGiaTri[h][c];
//            var cs;
//            cs = Bang_arrCSMaCot["iLoaiMa"];
//            if (Bang_arrGiaTri[h][cs] <= 1) {
//                cs = Bang_arrCSMaCot["iID_MaPhuCap"];
//                var iID_MaPhuCap = Bang_arrGiaTri[h][cs];

//                
//                //cs = Bang_arrCSMaCot["sMaHT"];
//                //Bang_GanGiaTriThatChoO(h, cs, sMaHT);
//            }
//        }        
        //BangDuLieu_ChayCongThuc(h, c);
    }
}

function BangDuLieu_ChayCongThuc(h, c) {
    var cs, TenTruong, GiaTri;
    cs = Bang_arrCSMaCot["bCongThuc"];
    if (Bang_arrGiaTri[h][cs] == "1") {
        cs = Bang_arrCSMaCot["sCongThuc"];
        var sCongThuc = Bang_arrGiaTri[h][cs];
        var cs_rHeSo = Bang_arrCSMaCot["rHeSo"];
        var rHeSo = Bang_arrGiaTri[h][cs_rHeSo];
        var cs_rSoTien = Bang_arrCSMaCot["rSoTien"];
        var rSoTien = BangDuLieu_ThucHienCongThuc(h, sCongThuc);
        Bang_GanGiaTriThatChoO(h, cs_rSoTien, rSoTien);
        var csMaTruongSoTien_BangLuong = Bang_arrCSMaCot["sMaTruongSoTien_BangLuong"];
        var sMaTruongSoTien_BangLuong = Bang_arrGiaTri[h][csMaTruongSoTien_BangLuong];
        if (sMaTruongSoTien_BangLuong != "") {
            var i;
            BangDuLieu_arrBangLuongChiTiet['[' + sMaTruongSoTien_BangLuong + ']'] = 0;
            for (i = 0; i < Bang_nH; i++) {
                if (Bang_arrGiaTri[i][csMaTruongSoTien_BangLuong] == sMaTruongSoTien_BangLuong) {
                    BangDuLieu_arrBangLuongChiTiet['[' + sMaTruongSoTien_BangLuong + ']'] += Bang_arrGiaTri[h][cs_rSoTien];
                }
            }
            cs = Bang_arrCSMaCot["sCongThuc"];
            for (i = 0; i < Bang_nH; i++) {
                sCongThuc = Bang_arrGiaTri[i][cs];
                if (sCongThuc.indexOf('[' + sMaTruongSoTien_BangLuong + ']') >= 0) {
                    BangDuLieu_ChayCongThuc(i, c);
                }
            }
        }
    }
}

function BangDuLieu_NhapMaHT(h, c) {
    var arrTruong = Bang_LayTenTruongVaTruongGan(c);
    var Truong = "sMaHT_PhuCap";
    var url = Bang_Url_getGiaTri;
    var GiaTri = $.trim(Bang_arrGiaTri[h][c]);

    url += '?Truong=' + Truong;
    url += '&GiaTri=' + GiaTri;
    url = unescape(url);

    var csMaMucLuc = Bang_arrCSMaCot["iID_MaMucLucNganSach"];
    if (csMaMucLuc >= 0) {
        Bang_arrThayDoi[h][csMaMucLuc] = true;
    }

    $.getJSON(url, function (item) {
        if (item.value == "") {
            Bang_GanGiaTriThatChoO(h, c, "");
            return;
        }
        //Gán lại tất cả các giá trị đã có
        var i, cs;
        var ThongTinThem = item.ThongTinThem;
        var arr = ThongTinThem.split("#|");
        for (i = 0; i < arr.length; i++) {
            var arr1 = arr[i].split("##");
            TenTruong = arr1[0];
            GiaTri = arr1[1];
            Bang_GanGiaTri(h, TenTruong, GiaTri);
        }
        Bang_arrEdit[h][c] = false;
        cs = Bang_arrCSMaCot["bDuocSuaChiTiet"];
        if (Bang_arrGiaTri[h][cs] == "1") {
            var csMin = Bang_arrCSMaCot["rSoTien"];
            for (i = csMin + 1; i < Bang_nC_Slide; i++) {
                Bang_arrEdit[h][i] = true;
            }
            cs = Bang_arrCSMaCot["sMoTa"];
            Bang_arrEdit[h][cs] = false;
        }
        cs = Bang_arrCSMaCot["bDuocSuaHeSo"];
        if (Bang_arrGiaTri[h][cs] == "1") {
            cs = Bang_arrCSMaCot["bCongThuc"];
            if (Bang_arrGiaTri[h][cs] == "1") {
                cs = Bang_arrCSMaCot["rHeSo"];
                Bang_arrEdit[h][cs] = true;
                cs = Bang_arrCSMaCot["rSoTien"];
                Bang_arrEdit[h][cs] = false;
            }
            else {
                cs = Bang_arrCSMaCot["rHeSo"];
                Bang_arrEdit[h][cs] = false;
                cs = Bang_arrCSMaCot["rSoTien"];
                Bang_arrEdit[h][cs] = true;
            }
        }
        BangDuLieu_ChayCongThuc(h, c);
        Bang_keys.fnSetFocusNextCell();
    });
}