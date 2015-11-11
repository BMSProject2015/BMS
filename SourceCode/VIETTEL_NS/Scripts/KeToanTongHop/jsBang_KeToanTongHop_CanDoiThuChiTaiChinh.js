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
    
    return true;
}

function BangDuLieu_onCellValueChanged(h, c) {
    if (h == 0 && Bang_arrMaCot[c] == "sNoiDung") {
        BangDuLieu_GoiHamThayDoiNoiDung(Bang_arrGiaTri[h][c]);
        if (document.getElementById("sNoiDung").value==""){
            BangDuLieu_GoiHamThayDoiNoiDung(Bang_arrGiaTri[h][c]);
        }
    }
    if (Bang_arrMaCot[c] == "sTenTaiKhoan_NgoaiTe") {
        Bang_GanGiaTriThatChoO_colName(h, "bCoTKGT_NgoaiTe", "");
     
    }
    if (Bang_arrMaCot[c] == "sTenTaiKhoan_Tong") {
        Bang_GanGiaTriThatChoO_colName(h, "bCoTKGT_Tong", "");
       
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
        if (h != null && h>=0) {
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
        for (i = 0; i < Bang_nC; i++) {
            if (Bang_arrMaCot[i] != 'bDongY' && Bang_arrMaCot[i] != 'sLyDo' && Bang_arrMaCot[i] != 'sID_MaNguoiDungTao') {
                Bang_arrEdit[0][i] = true;
            }
            if (Bang_arrMaCot[i] == 'sID_MaNguoiDungTao') {
                Bang_arrEdit[0][i] = false;
            }     
        }
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
            if (Bang_arrMaCot[i] != 'bDongY' && Bang_arrMaCot[i] != 'sLyDo' && Bang_arrMaCot[i] != 'sID_MaNguoiDungTao') {
                Bang_arrEdit[h + 1][i] = true;
            }
            if (Bang_arrMaCot[i] == 'sID_MaNguoiDungTao') {
                Bang_arrEdit[h + 1][i] = false;
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
//var GhepKyTu = "";
//function BangDuLieu_onCellKeypress(h, c, e, iKey) {
//    var MaKhoa;
//    if (iKey == 0) {
//        MaKhoa = e.charCode;
//    } else {
//        MaKhoa = iKey;
//    }
//    if (browserID == 'msie') {
//        if (96 <= MaKhoa && MaKhoa <= 105) {
//            MaKhoa = MaKhoa - 48;
//        }
//    }
//    else if (browserID == 'mozilla') {
//    }
//    else {
//    }

//    if ((48 <= MaKhoa && MaKhoa <= 58) ||
//                    (96 <= MaKhoa && MaKhoa <= 122) ||
//                    (65 <= MaKhoa && MaKhoa <= 90)) {
//        //Nhập số
//        KeyTable_sKyTuVuaNhap = String.fromCharCode(MaKhoa);
//        GhepKyTu = GhepKyTu + KeyTable_sKyTuVuaNhap;
//    }
//    if (Bang_arrMaCot[c] == "iNgay") {
//        var GiaTri = Bang_arrGiaTri[h][c];
//        if (GiaTri != GhepKyTu) {
//            if (GhepKyTu.length == 2) {
//                Bang_keys.fnSetFocus(h, c + 1);
//                GhepKyTu = "";
//            }
//        }
//    }
//    return true;
//}
//function BangDuLieu_onCellKeyUp(h, c, item) {
//    if (Bang_arrType[c] == 0) {
//        var GiaTri = new String(Bang_arrGiaTri[h][c]);
//        var KyTu = new String("");
//        var sStr = new String("");
//        if (GiaTri.toString().length > 5) {
//            if (Bang_arrMaCot[c] == "sSoChungTuChiTiet") {
//                KyTu = GiaTri.toUpperCase();
//                Bang_GanGiaTriThatChoO(h, c, KyTu);
//                return true;
//            }
//            KyTu = GiaTri.toString().substring(0, 1);
//            KyTu = KyTu.toUpperCase();
//            sStr = GiaTri.toString().substring(1, GiaTri.toString().length);
//            Bang_GanGiaTriThatChoO(h, c, KyTu + sStr);
//            return true;
//        }
//    }
//}

/* Su kien BangDuLieu_onCellAfterEdit
*   - Muc dinh: Su kien xuat hien sau khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*/
function BangDuLieu_onCellAfterEdit(h, c, item) {
    var cs;
    if (Bang_arrType[c] == 0) {
        var GiaTri = new String(Bang_arrGiaTri[h][c]);
        var KyTu = new String("");
        var sStr = new String("");
        if (GiaTri.toString().length > 0) {
            if (Bang_arrMaCot[c] == "sSoChungTuChiTiet") {
                KyTu = GiaTri.toUpperCase();
                Bang_GanGiaTriThatChoO(h, c, KyTu);
                return true;
            }
            KyTu = GiaTri.toString().substring(0, 1);
            KyTu = KyTu.toUpperCase();
            sStr = GiaTri.toString().substring(1, GiaTri.toString().length);
            Bang_GanGiaTriThatChoO(h, c, KyTu + sStr);                        
        }
    }
    if (Bang_arrMaCot[c] == "sTenTaiKhoan_NgoaiTe") {
        if (Bang_arrGiaTri[h][c] == "") {
            Bang_GanGiaTriThatChoO_colName(h, "bCoTKGT_NgoaiTe", "");
            cs = Bang_arrCSMaCot["sTenTaiKhoan_Tong"];
            Bang_keys.fnSetFocus(h, cs);
            return false;
        }

    }
    if (Bang_arrMaCot[c] == "sTenTaiKhoan_Tong") {
        if (Bang_arrGiaTri[h][c] == "") {
            Bang_GanGiaTriThatChoO_colName(h, "bCoTKGT_Tong", "");
            return false;
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


//Gán giá trị cho trường iID_MaChungTu

function GanGiaTriTruong_iID_MaChungTu(GiaTri)
{
    document.getElementById('iID_MaChungTu').value = GiaTri;
}

//Frame chứng từ gọi khi có thay đổi giá trị các trường của chứng từ
function ChungTuChiTiet_ThayDoiTruongChungTu(Truong, GiaTri) {
    document.getElementById(Truong).value = GiaTri;
    if (Truong == "iNgay" && Bang_nH == 1) {
        Bang_GanGiaTriThatChoO(0, 0, parseInt(GiaTri));
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


