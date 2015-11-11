/* BangDuLieu_CoCotTongSo: xac dinh bang co cot duyet hay khong*/
var BangDuLieu_CoCotDuyet = false;
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";
var BangDuLieu_hOld = 0;
/* Su kien BangDuLieu_onLoad
*/
function BangDuLieu_onLoad() {
    if (typeof Bang_arrCSMaCot["bDongY"] == "undefined") {
        BangDuLieu_CoCotDuyet = false;
    }
    else {
        BangDuLieu_CoCotDuyet = true;
    }
    if (Bang_ChiDoc == false && Bang_nH == 0) {
        BangDuLieu_ThemHangMoi(-1);
        Bang_DaThayDoi = false;
    }
}

function BangDuLieu_onCellBeforeEdit(h, c) {
    if (Bang_arrMaCot[c].endsWith("_No")) {
        if (Bang_arrMaCot[c] == "sTenTaiKhoan_No") {
            //            if (Bang_LayGiaTri(h, "sTenTaiKhoan_Co") != "") {
            //                //Trường hợp đã có tài khoản nợ thì không nhập tài khoản có
            //                return false;
            //            }
        }
        else {
            if (Bang_LayGiaTri(h, "sTenTaiKhoan_No") == "") {
                //Trường hợp phải có tên tài khoản mới nhập các thông tin thêm
                return false;
            }
        }
    }
    else if (Bang_arrMaCot[c].endsWith("_Co")) {
        if (Bang_arrMaCot[c] == "sTenTaiKhoan_Co") {
            //            if (Bang_LayGiaTri(h, "sTenTaiKhoan_No") != "") {
            //                //Trường hợp đã có tài khoản nợ thì không nhập tài khoản có
            //                return false;
            //            }
        }
        else {
            if (Bang_LayGiaTri(h, "sTenTaiKhoan_Co") == "") {
                //Trường hợp phải có tên tài khoản mới nhập các thông tin thêm
                return false;
            }
        }
    }
    return true;
}

function BangDuLieu_onCellValueChanged(h, c) {
    if (h == 0 && Bang_arrMaCot[c] == "sNoiDung") {
        if (document.getElementById("sNoiDung").value == "") {
            BangDuLieu_GoiHamThayDoiNoiDung(Bang_arrGiaTri[h][c]);
        }
    }
    if (Bang_arrMaCot[c] == "sTenTaiKhoan_Co" && Bang_arrGiaTri[h][c] == "") {
        Bang_GanGiaTriThatChoO_colName(h, "sTenGiaiThich_Co", "");
        Bang_GanGiaTriThatChoO_colName(h, "sTenPhongBan_Co", "");
        Bang_GanGiaTriThatChoO_colName(h, "sTenDonVi_Co", "");
    }
    if (Bang_arrMaCot[c] == "sTenTaiKhoan_No" && Bang_arrGiaTri[h][c] == "") {
        Bang_GanGiaTriThatChoO_colName(h, "sTenGiaiThich_No", "");
        Bang_GanGiaTriThatChoO_colName(h, "sTenPhongBan_No", "");
        Bang_GanGiaTriThatChoO_colName(h, "sTenDonVi_No", "");
    }
}

/* Ham BangDuLieu_CheckAll_value
- Muc dinh: Check or Uncheck tat ca cac o co kieu du lieu la kieu 2(Checkbox)
*/
var BangDuLieu_CheckAll_value = false;
function BangDuLieu_CheckAll(checked) {
    if (typeof checked == "undefined") {
        BangDuLieu_CheckAll_value = !BangDuLieu_CheckAll_value;
    }
    else {
        BangDuLieu_CheckAll_value = checked;
    }
    var value = "0";
    if (BangDuLieu_CheckAll_value) {
        value = "1";
    }
    else {
        value = "0";
    }
    var h, c = Bang_arrCSMaCot["bDongY"];
    for (h = 0; h < Bang_arrMaHang.length; h++) {
        Bang_GanGiaTriThatChoO(h, c, value);
    }
}

/* Ham BangDuLieu_TinhTongHangCon
*   - Muc dinh: Tao 1 hang moi o tai vi tri 'h' lay du lieu tai vi tri 'hGiaTri'
*   - Dau vao:  + h: vi tri them
*               + hGiaTri: vi tri hang lay du lieu, =null: them 1 hang trong
*/
function BangDuLieu_ThemHangMoi(h, hGiaTri) {
    if (Bang_ChiDoc == false) {
        var csH = 0;
        if (h != null && h >= 0) {
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
        if (Bang_nH == 1) {
            var iThang = parseInt(document.getElementById('iThang').value);
            cs = Bang_arrCSMaCot["iThang"];
            Bang_GanGiaTriThatChoO(csH, cs, iThang);
        }
        cs = Bang_arrCSMaCot["bDongY"];
        Bang_GanGiaTriThatChoO(csH, cs, "0");
        cs = Bang_arrCSMaCot["sLyDo"];
        Bang_GanGiaTriThatChoO(csH, cs, "");
        cs = Bang_arrCSMaCot["sMauSac"];
        Bang_GanGiaTriThatChoO(csH, cs, "");
    }
}

/* Su kien BangDuLieu_onKeypress_F2
- Muc dinh: goi ham them hang khi an phim F2
*/
function BangDuLieu_onKeypress_F2(h, c) {
    if (Bang_ChiDoc == false) {
        BangDuLieu_ThemHangMoi(h + 1, h);
        //Gán các giá trị của hàng mới thêm =0
        var arrTruongTien = "rSoTien".split(',');
        var i, cs;
        for (i = 0; i < arrTruongTien.length; i++) {
            cs = Bang_arrCSMaCot[arrTruongTien[i]];
            Bang_GanGiaTriThatChoO(h + 1, cs, 0);
        }
        for (i = 0; i < Bang_nC; i++) {
            if (Bang_arrMaCot[i] != 'bDongY' && Bang_arrMaCot[i] != 'sLyDo') {
                Bang_arrEdit[h + 1][i] = true;
            }
        }
        BangDuLieu_CapNhapSTT();
    }
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
        BangDuLieu_CapNhapSTT();
    }
}

function BangDuLieu_CapNhapSTT() {
    for (var h = 0; h < Bang_nH; h++) {
        var cs = Bang_arrCSMaCot["iSTT"];
        Bang_GanGiaTriThatChoO(h, cs, h + 1);
    }
}

/* Su kien BangDuLieu_onCellAfterEdit
*   - Muc dinh: Su kien xuat hien sau khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*/
function BangDuLieu_onCellAfterEdit(h, c, item) {
    var cs;
    if (Bang_arrMaCot[c] == "sTenTaiSan") {
        //
        var ThongTinThem = item.ThongTinThem;
        if (ThongTinThem != "") {
            var arr = ThongTinThem.split('##');
            for (i = 0; i < arr.length; i = i + 2) {
                var Truong = arr[i];
                var GiaTri = arr[i + 1];
                Bang_GanGiaTri(h, Truong, GiaTri);
            }
        }
    }
    return true;
}

function BangDuLieu_onEnter_NotSetCellFocus() {
    BangDuLieu_onKeypress_F2(Bang_nH - 1);
}

function BangDuLieu_onBodyFocus() {
    Bang_keys.focus();
}

function BangDuLieu_onBodyBlur() {
    Bang_keys.blur();
}

function BangDuLieu_save() {
    Bang_HamTruocKhiKetThuc();
}

function BangDuLieu_onKeypress_F10(h, c) {
    parent.ChungTuChiTiet_onKeypress_F10(-1, -1);
    return false;
}

//Frame chứng từ gọi khi có thay đổi giá trị các trường của chứng từ
function ChungTuChiTiet_ThayDoiTruongChungTu(Truong, GiaTri) {
    document.getElementById(Truong).value = GiaTri;
    if (Truong == "iNgay" && Bang_nH == 1) {
        var cs = Bang_arrCSMaCot["iNgayCT"];
        if (Bang_arrGiaTri[0][cs] != "") {
            Bang_GanGiaTriThatChoO(0, cs, parseInt(GiaTri));
        }
    }
}

//Parent kiểm tra có thay đổi dữ liệu hay không
function ChungTuChiTiet_KiemTraCoThayDoi() {
    return Bang_DaThayDoi;
}

//Parent hủy có thay đổi dữ liệu
function ChungTuChiTiet_HuyCoThayDoi() {
    Bang_DaThayDoi = false;
}

//Gọi hàm thay đổi nội dung trên form parent
function BangDuLieu_GoiHamThayDoiNoiDung(sNoiDung) {
    parent.ChungTuChiTiet_ThayDoiTruongNoiDung(sNoiDung);
}

//Xem chi tiết tài sản
function BangDuLieu_onKeypress_F4(h, c) {
    if (h != null && h >= 0) {
        parent.ChungTuChiTiet_HienThi_TaiSanChiTiet();
    }
}

//Lấy giá trị 1 trường của hàng focus
function BangDuLieu_LayGiaTriTruong(Truong) {
    var h = Bang_keys.Row();
    return Bang_LayGiaTri(h, Truong);
}

//Lấy giá trị 1 trường của hàng focus
function BangDuLieu_LayGiaTriHienThiTruong(Truong) {
    var h = Bang_keys.Row();
    var c = Bang_arrCSMaCot[Truong];
    return Bang_arrHienThi[h][c];
}

//Gán giá trị 1 trường của hàng focus
function BangDuLieu_GanGiaTriTruong(Truong, GiaTri) {
    var h = Bang_keys.Row();
    Bang_GanGiaTri(h, Truong, GiaTri);
}

//Lấy danh sách mã cột của bảng
function BangDuLieu_Lay_arrMaCot() {
    return Bang_arrMaCot;
}

