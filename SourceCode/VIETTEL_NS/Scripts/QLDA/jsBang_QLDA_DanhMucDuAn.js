/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";

/* Su kien BangDuLieu_onLoad
*/
function BangDuLieu_onLoad() {

    if (Bang_nH == 0) {
        BangDuLieu_ThemHangMoi();
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
    Bang_ThemHang(csH,hGiaTri);
    //Sua MaHang="": Day la hang them moi
    Bang_arrMaHang[csH] = "";
    var c;
    for (c = 0; c < Bang_nC; c++) {
        Bang_arrEdit[csH][c] = true;
    }

    csCot = 0;
    for (j = 0; j < Bang_arrCotDuocPhepNhap.length; j++) {
        if (Bang_arrCotDuocPhepNhap[j]) {
                csCot = j;
                break;
            }            
        }

        Bang_keys.fnSetFocus(csH, csCot);
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

function BangDuLieu_onCellBeforeEdit(h, c) {
    if (Bang_arrMaCot[c] == "sTenChuDauTu") {
        if (Bang_LayGiaTri(h, "iID_MaDonVi") == "") {
            //Trường hợp phải có mã đơn vị mới nhập các thông tin thêm
            return false;
        }
    }
    if (Bang_arrMaCot[c] == "sTenBanQuanLy") {
        if (Bang_LayGiaTri(h, "iID_MaChuDauTu") == "") {
            //Trường hợp phải có mã chủ đầu tư mới nhập các thông tin thêm
            return false;
        }
    }
    return true;
}

function BangDuLieu_onCellValueChanged(h, c) {

    if (Bang_arrMaCot[c] == "sTenDonVi") {
        Bang_GanGiaTriThatChoO_colName(h, "iID_MaChuDauTu", "");
        Bang_GanGiaTriThatChoO_colName(h, "sTenChuDauTu", "");
        Bang_GanGiaTriThatChoO_colName(h, "iID_MaBanQuanLy", "");
        Bang_GanGiaTriThatChoO_colName(h, "sTenBanQuanLy", "");
    }

}


