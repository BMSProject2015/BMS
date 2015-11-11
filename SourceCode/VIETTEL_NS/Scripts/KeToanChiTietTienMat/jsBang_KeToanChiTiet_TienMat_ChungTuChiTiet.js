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
        Bang_ThemHang(csH, hGiaTri); // them dong du lieu moi khi nhan f2
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
        // thay doi gia tri cot so chung tu chi tiet
        var maxCT = 0;
        var str_maxCT = "";

        var strbThu = 0;
        var str_maxThu = "";
        var strbChi = 0;
        var str_maxChi = "";
        for (var j = 0; j < Bang_nH; j++) {
            if ((Bang_LayGiaTri(j, "bThu") == "1") && (parseFloat(maxCT) < parseFloat(Bang_LayGiaTri(j, "sSoChungTuChiTiet"))) && (typeof Bang_LayGiaTri(j, "sSoChungTuChiTiet") != "undefined")) {
                strbThu = parseFloat(Bang_LayGiaTri(j, "sSoChungTuChiTiet"));
                str_maxThu = Bang_LayGiaTri(j, "sSoChungTuChiTiet");
            }
            if (Bang_LayGiaTri(j, "bChi") == "1" && (parseFloat(maxCT) < parseFloat(Bang_LayGiaTri(j, "sSoChungTuChiTiet"))) && (typeof Bang_LayGiaTri(j, "sSoChungTuChiTiet") != "undefined")) {
                strbChi = parseFloat(Bang_LayGiaTri(j, "sSoChungTuChiTiet"));
                str_maxChi = Bang_LayGiaTri(j, "sSoChungTuChiTiet");
            }
        }
        var bThuCurrent = Bang_LayGiaTri(csH, "bThu");
        var bChiCurrent = Bang_LayGiaTri(csH, "bChi");
        if (bThuCurrent == "1") {
            maxCT = strbThu;
            str_maxCT = str_maxThu;
        }
        else if (bChiCurrent == "1") {
            maxCT = strbChi;
            str_maxCT = str_maxChi;
        }
        else {
            maxCT = strbThu;
            str_maxCT = str_maxThu;
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
        if (Bang_nH == 1) {
            cs = Bang_arrCSMaCot["bChi"];
            Bang_GanGiaTriThatChoO(csH, cs, "1");
            var url = urlServerPath + "KTCT_TienMat_ChungTuChiTiet/getMaxSoChungTuChiTiet?ThuChi=0";
            $.getJSON(url, function (item) {
                var sSoChungTuChiTiet = item.sSoChungTuChiTiet;
                var sCurrent = 0;
                if (sSoChungTuChiTiet == "") {
                    sCurrent = 0;
                }
                else {
                    sCurrent = parseFloat(sSoChungTuChiTiet) + 1;
                }
                var sChuoi = sCurrent;
                var lenMaxCurrent = parseFloat(sSoChungTuChiTiet.length) - sCurrent.toString().length;

                if (lenMaxCurrent == 1) {
                    sChuoi = "0" + sChuoi;
                }
                else if (lenMaxCurrent == 2) {
                    sChuoi = "00" + sChuoi;
                }
                else if (lenMaxCurrent == 3) {
                    sChuoi = "000" + sChuoi;
                }
                else if (lenMaxCurrent == 4) {
                    sChuoi = "0000" + sChuoi;
                }
                else if (lenMaxCurrent == 5) {
                    sChuoi = "00000" + sChuoi;
                }
                else if (lenMaxCurrent == 6) {
                    sChuoi = "000000" + sChuoi;
                }
                else if (lenMaxCurrent == 7) {
                    sChuoi = "0000000" + sChuoi;
                }
                else if (lenMaxCurrent == 8) {
                    sChuoi = "00000000" + sChuoi;
                }
                else if (lenMaxCurrent == 9) {
                    sChuoi = "000000000" + sChuoi;
                }
//                else {
//                    chuoiChungTu = "000" + chuoiChungTu;
//                }
                cs = Bang_arrCSMaCot["sSoChungTuChiTiet"];
                Bang_GanGiaTriThatChoO(csH, cs, sChuoi);
            });
        }
        else {
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
    if (Bang_arrMaCot[c] == "bThu") {
        if (Bang_arrGiaTri[h][c + 1] == "1") {// gia tri chi
            // alert("Bạn đã chọn là phiếu chi!");
            // value = "0";
            value = "1";
            Bang_GanGiaTriThatChoO(h, c, value);
            cs = Bang_arrCSMaCot["bChi"];
            Bang_GanGiaTriThatChoO(h, cs, "0");
            var iDot = Bang_arrCSMaCot["iDot"];
            Bang_keys.fnSetFocus(h, iDot);

        }
        else if (Bang_arrGiaTri[h][c + 1] == "0" && Bang_arrGiaTri[h][c] == "0") {
            alert('Hãy chọn loại thu hoặc chi trong chứng từ chi tiết dòng số ' + (c + 1));
            cs = Bang_arrCSMaCot["bThu"];
            Bang_keys.fnSetFocus(c, cs);
        }
    }
    if (Bang_arrMaCot[c] == "bChi") {
        if (Bang_arrGiaTri[h][c - 1] == "1") {//gia tri thu
            //alert("Bạn đã chọn là phiếu thu!");
            value = "1";
            Bang_GanGiaTriThatChoO(h, c, value);
            cs = Bang_arrCSMaCot["bThu"];
            Bang_GanGiaTriThatChoO(h, cs, "0");
            //Bang_keys.fnSetFocus(h, cs);
            var iDot = Bang_arrCSMaCot["iDot"];
            Bang_keys.fnSetFocus(h, iDot);
        }
        else if (Bang_arrGiaTri[h][c - 1] == "0" && Bang_arrGiaTri[h][c] == "0") {
            alert('Hãy chọn loại thu hoặc chi trong chứng từ chi tiết dòng số ' + (c + 1));
            cs = Bang_arrCSMaCot["bThu"];
            Bang_keys.fnSetFocus(c, cs);
        }
    }
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
        return true;
    }
    if (Bang_arrMaCot[c] == "sNoiDung") {
        var GiaTri = "";
        GiaTri = Bang_arrGiaTri[h][c];
        Bang_GanGiaTriThatChoO(h, "14", GiaTri);

    } return true;
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
        var bThu = Bang_LayGiaTri(j, "bThu");
        var bChi = Bang_LayGiaTri(j, "bChi");
        if (bChi == 0 && bThu == 0) {
            alert('Hãy chọn loại thu hoặc chi trong chứng từ chi tiết dòng số ' + (j + 1));
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