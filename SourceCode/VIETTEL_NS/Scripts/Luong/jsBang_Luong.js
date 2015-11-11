/* BangDuLieu_CoCotTongSo: xac dinh bang co cot duyet hay khong*/
var BangDuLieu_CoCotDuyet = false;
/* BangDuLieu_CoCotTongSo: xac dinh bang co cot tong so hay khong*/
var BangDuLieu_CoCotTongSo = true;
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";

var BangDuLieu_BangTruyLinh = false;

var BangDuLieu_iID_MaBangLuong = "";

/* Su kien BangDuLieu_onLoad
*/
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
    Bang_ThemHang(csH, hGiaTri);
    //Sua MaHang="": Day la hang them moi
    Bang_arrMaHang[csH] = "";
    Bang_keys.fnSetFocus(csH, 0);
}

/* Su kien BangDuLieu_onKeypress_F2
- Muc dinh: goi ham them hang khi an phim F2
*/
function BangDuLieu_onKeypress_F2(h, c) {
    if (BangDuLieu_BangTruyLinh == false) {
        parent.jsLuong_Dialog_ThemMoiCanBo_Show();
    }
}

/* Su kien BangDuLieu_onKeypress_F4
- Muc dinh: nhập thông tin chi tiết của 1 hàng
*/
function BangDuLieu_onKeypress_F4(h, c) {
    if (BangDuLieu_BangTruyLinh == false) {
        var iID_MaBangLuongChiTiet = Bang_LayGiaTri(h, "iID_MaBangLuongChiTiet");
        parent.jsLuong_Dialog_SuaCanBo_Show(iID_MaBangLuongChiTiet);    
    }
}

function BangDuLieu_onDblClick(h, c) {
    BangDuLieu_onKeypress_F4(h, c);
}
/*Sự kiện trích lương*/
function BangDuLieu_onKeypress_F6(h, c) {
    parent.jsLuong_Dialog_TrichLuong_Show();
}

/*Điều chỉnh hệ số khu vực*/
function BangDuLieu_onKeypress_F7(h, c) {
    parent.jsLuong_Dialog_HeSoKhuVuc_Show();
}

/*Điều chỉnh tiền ăn*/
function BangDuLieu_onKeypress_F8(h, c) {
    parent.jsLuong_Dialog_DieuChinhTienAn_Show();
}

/*Điều chỉnh hủy tập thể*/
function BangDuLieu_onKeypress_F11(h, c) {
    parent.jsLuong_Dialog_HuyTapThe_Show();
}
/* Su kien BangDuLieu_onKeypress_Delete
- Muc dinh: goi ham xoa hang khi an phim DELETE
*/

/*In danh sách */
function BangDuLieu_onKeypress_F9(h, c) {
    parent.jsLuong_Dialog_NguoiPhuThuoc_Show();
}
/*Nội dung thù lao, thưởng*/
function BangDuLieu_onKeypress_F1(h, c) {
    parent.jsLuong_Dialog_ThuLaoThuong_Show();
}

/*Nội dung nộp thuế đầu vào*/
function BangDuLieu_onKeypress_F12(h, c) {
    parent.jsLuong_Dialog_NopThueDauVao_Show();
}

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