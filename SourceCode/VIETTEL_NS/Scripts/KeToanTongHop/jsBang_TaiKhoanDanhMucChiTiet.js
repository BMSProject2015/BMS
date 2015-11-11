/* BangDuLieu_CoCotTongSo: xac dinh bang co cot duyet hay khong*/
var BangDuLieu_CoCotDuyet = false;
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";

var BangDuLieu_arrDSTruong = new Array();
var BangDuLieu_arrBangLuongChiTiet = new Array();
var Bang_Url_Check_KyHieu = ""
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
    Bang_keys.fnSetFocus(Bang_nH, 0);
}

/* Ham BangDuLieu_CheckAll_value
- Muc dinh: Check or Uncheck tat ca cac o co kieu du lieu la kieu 2(Checkbox)
*/


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
    Bang_GanGiaTriThatChoO_colName(csH, "sKyHieu", "");
    //Sua MaHang="": Day la hang them moi
    Bang_arrMaHang[csH] = "";
    var c;
    for (c = 0; c < Bang_nC; c++) {
        Bang_arrEdit[csH][c] = true;
    }
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

/* Su kien BangDuLieu_onCellAfterEdit
*   - Muc dinh: Su kien xuat hien sau khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*/
function BangDuLieu_onCellAfterEdit(h, c) {
    if (Bang_arrType[c] == 0) {
        var GiaTri = new String(Bang_arrGiaTri[h][c]);
        if (GiaTri.toString().length > 0) {
            if (Bang_arrMaCot[c] == "sTen") {
                var KyTu = new String("");
                var sStr = new String("");
                KyTu = GiaTri.toString().substring(0, 1);
                KyTu = KyTu.toUpperCase();
                sStr = GiaTri.toString().substring(1, GiaTri.toString().length);
                Bang_GanGiaTriThatChoO(h, c, KyTu + sStr);
                return true;
            }
        }
    }
    if (Bang_arrMaCot[c] == 'sKyHieu') {
        var sKyHieu = Bang_arrGiaTri[h][c];
        var iID_MaTaiKhoanDanhMucChiTiet =Bang_arrMaHang[h]
        jQuery.ajaxSetup({ cache: false });
        var vR;
        var url = Bang_Url_Check_KyHieu;
        url += "?iID_MaTaiKhoanDanhMucChiTiet="+iID_MaTaiKhoanDanhMucChiTiet+"&sKyHieu=" + sKyHieu;
        $.getJSON(url, function (data) {
            if (data) {
                Bang_GanGiaTriThatChoO(h, c, "");
                alert("Ký hiệu đã có. Bạn phải nhập ký hiệu khác!");
            }
        });
        return false;
    }
}

//function BangDuLieu_onCellBeforeEdit(h, c) {
//        if (Bang_arrMaCot[c] == "sTenBacLuong") {
//            if (Bang_LayGiaTri(h, "iID_MaNgachLuong") == "") {
//                //Trường hợp phải có tên tài khoản mới nhập các thông tin thêm
//                return false;
//            }
//        }
//    return true;
//}

//function BangDuLieu_onCellValueChanged(h, c) {    
//    if (Bang_arrMaCot[c] == "sTenNgachLuong") {
//        Bang_GanGiaTriThatChoO_colName(h, "iID_MaBacLuong", "");
//        Bang_GanGiaTriThatChoO_colName(h, "sTenBacLuong", "");        
//    }
//    
//}
