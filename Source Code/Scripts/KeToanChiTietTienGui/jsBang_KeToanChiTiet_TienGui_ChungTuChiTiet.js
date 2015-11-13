/* BangDuLieu_CoCotTongSo: xac dinh bang co cot duyet hay khong*/
var BangDuLieu_CoCotDuyet = false;
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";
var BangDuLieu_hOld = 0;

var jsKTCT_CapThu_Duyet = "";
var jsKTCT_ThongTinDonViNhanUNC = "";
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
    TinhTongChiTiet(); 
}

function TinhTongChiTiet() {
    var i = 0;
    var c = Bang_arrCSMaCot["rSoTien"];
    var TongSo = 0;
    var GiaTri = 0;
    var iSoLuongChungTuChiTiet = 0;
    for (i = 0; i < Bang_nH; i++) {
        GiaTri = parseFloat(Bang_arrGiaTri[i][c]);
        TongSo = TongSo + GiaTri;
        if (parseFloat(Bang_arrGiaTri[i][c]) > 0) {// ham kiem tra so luong chung tu chi tiet
            iSoLuongChungTuChiTiet++;
        }
    }
    document.getElementById('lblTongSo').innerHTML = FormatNumber(TongSo);
    parent.iSoLuongChungTu = iSoLuongChungTuChiTiet;
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
                return true;
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
                return true;
            }
        }
    }
    return true;
}

function BangDuLieu_onCellValueChanged(h, c) {
    if (h == 0 && Bang_arrMaCot[c]=="sNoiDung") {
        if (document.getElementById("sNoiDung").value==""){
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
/* Ham BangDuLieu_CapNhapLaiHangCha
*   - Muc dinh: Cap nhap lai cac hang cha cua hang 'h', Tinh tong so cua hang h va cac hang cha
*   - Dau vao:  + h: chi so hang con
*               + c: vi tri cot cua o can tinh gia tri
*/
function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrType[c] == 3) {
        BangDuLieu_TinhOConLai(h, c);
    }
}

function BangDuLieu_TinhOConLai(h, c) {
    if (Bang_arrMaCot[c] == "sSoChungTuCapThu") {
        var iID_MaChungTu_Duyet = Bang_arrGiaTri[h][c + 23];

        jQuery.ajaxSetup({ cache: false });
        var vR;
        var url = jsKTCT_CapThu_Duyet;
        url += "?iID_MaChungTu_Duyet=" + iID_MaChungTu_Duyet;
        $.getJSON(url, function (item) {
            Bang_GanGiaTriThatChoO(h, c + 1, item.sNoiDung);
            Bang_GanGiaTriThatChoO(h, c + 2, item.rTongCap);
            Bang_GanGiaTriThatChoO(h, c + 3, item.rTongThu);
            Bang_GanGiaTriThatChoO(h, c + 4, item.sRutDuToan);
            Bang_GanGiaTriThatChoO(h, c + 5, item.rSoTien);
            Bang_GanGiaTriThatChoO(h, c + 6, item.sTenDonVi_Co);
            Bang_GanGiaTriThatChoO(h, c + 7, item.sTenDonVi_No);
            Bang_GanGiaTriThatChoO(h, c + 10, item.sTenDonVi_No);
            Bang_GanGiaTriThatChoO(h, c + 11, item.sTenTaiKhoan_No);
            Bang_GanGiaTriThatChoO(h, c + 13, item.sTenTaiKhoan_Co);

            Bang_GanGiaTriThatChoO(h, c + 21, item.iID_MaDonVi_No);
            Bang_GanGiaTriThatChoO(h, c + 20, item.iID_MaDonVi_No);
            Bang_GanGiaTriThatChoO(h, c + 19, item.iID_MaDonVi_Co);
            Bang_GanGiaTriThatChoO(h, c + 18, item.iID_MaTaiKhoan_Co);
            Bang_GanGiaTriThatChoO(h, c + 17, item.iID_MaTaiKhoan_No);
         
           
        });
    }
    if (Bang_arrMaCot[c] == "sTenDonVi_Nhan") {
        var iID_MaDonViNhanUNC = Bang_arrGiaTri[h][c + 11];
        jQuery.ajaxSetup({ cache: false });
        var url = jsKTCT_ThongTinDonViNhanUNC;
        url += "?iID_MaDonViNhanUNC=" + iID_MaDonViNhanUNC;
        $.getJSON(url, function (item) {
            Bang_GanGiaTriThatChoO(h, c + 1, item.sSoTaiKhoan);
            Bang_GanGiaTriThatChoO(h, c + 2, item.sDiaChi);
        });
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
            cs = Bang_arrCSMaCot["iNgay"];
            var date = new Date();
            Bang_GanGiaTriThatChoO(csH, cs, date.getDate());
        }
        cs = Bang_arrCSMaCot["bDongY"];
        Bang_GanGiaTriThatChoO(csH, cs, "0");
        cs = Bang_arrCSMaCot["sLyDo"];
        Bang_GanGiaTriThatChoO(csH, cs, "");
        cs = Bang_arrCSMaCot["sMauSac"];
        Bang_GanGiaTriThatChoO(csH, cs, "");

        var i;
        for (i = 0; i < Bang_nC; i++) {
            if (Bang_arrMaCot[i] != 'bDongY' && Bang_arrMaCot[i] != 'sLyDo' && Bang_arrMaCot[i] != 'rTongCap' && Bang_arrMaCot[i] != 'rTongThu' && Bang_arrMaCot[i] != 'rRutDuToan' && Bang_arrMaCot[i] != 'sSoTaiKhoan' && Bang_arrMaCot[i] != 'sDiaChiDonViNhan') {
                Bang_arrEdit[csH][i] = true;
            }
            else {
                Bang_arrEdit[csH][i] = false;
            }
        }

        // thay doi gia tri cot so chung tu chi tiet
        var maxCT = 0;
        var str_maxCT = "";
        if (Bang_nH == 1) {
            var url = urlServerPath + "KTCT_TienGui_ChungTuChiTiet/getMaxSoChungTuChiTiet";
            $.getJSON(url, function (item) {
                str_maxCT = item.sSoChungTuChiTiet;
                if (str_maxCT == "") {
                    maxCT = 0;
                }
                else {
                    maxCT = parseFloat(str_maxCT);
                }
                var chuoiChungTu = maxCT + 1;
                var lenMax = parseFloat(str_maxCT.length) - parseFloat(chuoiChungTu.toString().length);

                if (lenMax == 1) {
                    chuoiChungTu = "0" + chuoiChungTu;
                }
                else if (lenMax == 2) {
                    chuoiChungTu = "00" + chuoiChungTu;
                }
                else if (lenMax == 3) {
                    chuoiChungTu = "000" + chuoiChungTu;
                }
                else if (lenMax == 4) {
                    chuoiChungTu = "0000" + chuoiChungTu;
                }
                else if (lenMax == 5) {
                    chuoiChungTu = "00000" + chuoiChungTu;
                }
                else if (lenMax == 6) {
                    chuoiChungTu = "000000" + chuoiChungTu;
                }
                else if (lenMax == 7) {
                    chuoiChungTu = "0000000" + chuoiChungTu;
                }
                else if (lenMax == 8) {
                    chuoiChungTu = "00000000" + chuoiChungTu;
                }
                else if (lenMax == 9) {
                    chuoiChungTu = "000000000" + chuoiChungTu;
                }
                else {
                    chuoiChungTu = "000" + chuoiChungTu;
                }

                if (chuoiChungTu != 0) {
                    cs = Bang_arrCSMaCot["sSoChungTuChiTiet"];
                    Bang_GanGiaTriThatChoO(csH, cs, chuoiChungTu);
                }
                
            });
        }
        else {
            for (var j = 0; j < Bang_nH; j++) {
                if ((parseFloat(maxCT) < parseFloat(Bang_LayGiaTri(j, "sSoChungTuChiTiet"))) && (typeof Bang_LayGiaTri(j, "sSoChungTuChiTiet") != "undefined")) {
                    maxCT = parseFloat(Bang_LayGiaTri(j, "sSoChungTuChiTiet"));
                    str_maxCT = Bang_LayGiaTri(j, "sSoChungTuChiTiet");
                }
            }
            var chuoiChungTu = maxCT + 1;
            var lenMax = parseFloat(str_maxCT.length) - parseFloat(chuoiChungTu.toString().length);

            if (lenMax == 1) {
                chuoiChungTu = "0" + chuoiChungTu;
            }
            else if (lenMax == 2) {
                chuoiChungTu = "00" + chuoiChungTu;
            }
            else if (lenMax == 3) {
                chuoiChungTu = "000" + chuoiChungTu;
            }
            else if (lenMax == 4) {
                chuoiChungTu = "0000" + chuoiChungTu;
            }
            else if (lenMax == 5) {
                chuoiChungTu = "00000" + chuoiChungTu;
            }
            else if (lenMax == 6) {
                chuoiChungTu = "000000" + chuoiChungTu;
            }
            else if (lenMax == 7) {
                chuoiChungTu = "0000000" + chuoiChungTu;
            }
            else if (lenMax == 8) {
                chuoiChungTu = "00000000" + chuoiChungTu;
            }
            else if (lenMax == 9) {
                chuoiChungTu = "000000000" + chuoiChungTu;
            }
            else {
                chuoiChungTu = "000" + chuoiChungTu;
            }

            if (chuoiChungTu != 0) {
                cs = Bang_arrCSMaCot["sSoChungTuChiTiet"];
                Bang_GanGiaTriThatChoO(csH, cs, chuoiChungTu);
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
    var iThang = parent.jsThangLamViec;
    var iNam = parent.jsNamLamViec;
    if (Bang_arrMaCot[c] == 'iNgay') {

        var iNgay = GetDaysInMonth(iThang, iNam);
        var iGiaTriNgay = Bang_LayGiaTri(h, "iNgay");
        if (parseFloat(iGiaTriNgay) > parseFloat(iNgay)) {
            var sText = "Tháng \"" + iThang + "\" chỉ có \"" + iNgay + "\" ngày, bạn không được nhập quá!";
            alert(sText);
            return false;
        }
    }
    if (Bang_arrMaCot[c] == 'iThang') {
        var iGiaTriThang = Bang_LayGiaTri(h, "iThang");
        if (parseInt(iGiaTriThang) > 12 || parseInt(iGiaTriThang) < 0) {
            var sTextThang = "Giá trị tháng không hợp lý!";
            alert(sTextThang);
            return false;
        }
    }

    if (Bang_arrType[c] == 0) {
        var GiaTri = new String(Bang_arrGiaTri[h][c]);
        var KyTu = new String("");
        var sStr = new String("");
        if (GiaTri.toString().length > 0) {
            //viet hoa so chung tu chi tiet
            if (Bang_arrMaCot[c] == "sSoChungTuChiTiet") {
                KyTu = GiaTri.toUpperCase();
                Bang_GanGiaTriThatChoO(h, c, KyTu);
                return true;
            }
            //viet hoa ky tu dau tien
            KyTu = GiaTri.toString().substring(0, 1);
            KyTu = KyTu.toUpperCase();
            sStr = GiaTri.toString().substring(1, GiaTri.toString().length);
            Bang_GanGiaTriThatChoO(h, c, KyTu + sStr);
        }
    }
    if (Bang_arrMaCot[c] == "sTenTaiKhoan_Co") {
        if (Bang_arrGiaTri[h][c] != "") {
            if (item.CoTaiKhoanGiaiThich == "1") {
                cs = Bang_arrCSMaCot["sTenTaiKhoanGiaiThich_Co"];
                Bang_keys.fnSetFocus(h, cs);
            }
            else {
                cs = Bang_arrCSMaCot["sTenPhongBan_Co"];
                Bang_keys.fnSetFocus(h, cs);
            }
            return false;
        }
        else {
        }
    }
    if (Bang_arrMaCot[c] == "sTenTaiKhoan_No") {
        if (Bang_arrGiaTri[h][c] != "") {
            if (item.CoTaiKhoanGiaiThich == "1") {
                cs = Bang_arrCSMaCot["sTenTaiKhoanGiaiThich_No"];
                Bang_keys.fnSetFocus(h, cs);
            }
            else {
                cs = Bang_arrCSMaCot["sTenPhongBan_No"];
                Bang_keys.fnSetFocus(h, cs);
            }
            return false;
        }
        else {
        }
    }
    if (Bang_arrMaCot[c] == "rSoTien") {
        TinhTongChiTiet();
        return false;
    }
    BangDuLieu_CapNhapLaiHangCha(h, c);
    
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
    //kiem tra ngay thang nhap tren luoi chi tiet
    for (var j = 0; j < Bang_nH; j++) {
        var b = Bang_LayGiaTri(j, "iNgay");
        if (b == 0) {
            alert('Hãy nhập ngày trong chứng từ chi tiết dòng số ' + (j + 1));
            return false;
        }
        var thang = Bang_LayGiaTri(j, "iThang");
        if (thang == 0) {
            alert('Hãy nhập tháng trong chứng từ chi tiết dòng số ' + (j + 1));
            return false;
        }

    }
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
        var cs = Bang_arrCSMaCot["iNgay"];
        //if (Bang_arrGiaTri[0][cs] != "") {
            Bang_GanGiaTriThatChoO(0, cs, GiaTri);
        //}
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