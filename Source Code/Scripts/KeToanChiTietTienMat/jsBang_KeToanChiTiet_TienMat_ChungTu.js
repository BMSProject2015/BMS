/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";
var BangDuLieu_hOld = -1;
var BangDuLieu_cOld = -1;
var BangDuLieu_CoResetDDL = false;
var iSoDongCTGS = 0;
/* Su kien BangDuLieu_onLoad
*/
function BangDuLieu_onLoad() {
    if (Bang_nH == 0) {
        BangDuLieu_onKeypress_F2();
    }
    else {
        // BangDuLieu_onCellBeforeFocus(1, 0);
        BangDuLieu_onCellBeforeFocus(Bang_nH, 0);
        if (parent.sSoChungTuTimKiem) {
            document.getElementById("id_sSoChungTu").value = parent.sSoChungTuTimKiem;
        }
        iSoDongCTGS = parseFloat(Bang_nH);
    }
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
    BangDuLieu_hLuu = h;
    BangDuLieu_cLuu = c;
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
    for (i = 0; i < Bang_nC; i++) {
        if (Bang_arrMaCot[i] == "iNgay") {
            parent.ChungTu_ThayDoiNgay(Bang_arrGiaTri[h][i]);
            break;
        }
    }
    //    if (Bang_arrMaCot[c] == 'iNgay') {
    //        parent.GoiHam_ChungTuChiTiet_BangDuLieu_fnSetFocus();
    //        return false;
    //    }
    if (Bang_arrMaCot[c] == 'sNoiDung') {

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
    if (Bang_arrMaCot[c] == 'sSoChungTu') {
        var iNamLamViec = parent.jsNamLamViec;
        var url = urlServerPath + "KTCT_TienMat_ChungTuChiTiet/get_ThongTinChungTuMoi?iNamLamViec=" + iNamLamViec;
        var sSoChungTu = Bang_arrGiaTri[h][c];
        var iID_MaChungTu = Bang_arrMaHang[h]

        sSoChungTu = Bang_arrGiaTri[h][c];
        if (IsNumeric(sSoChungTu) == false) {
            alert('Bạn phải nhập số chứng từ kiểu số')

            $.getJSON(url, function (item) {
                var cs;
                iID_MaChungTu = item.iID_MaChungTu;
                cs = Bang_arrCSMaCot["sSoChungTu"];
                Bang_GanGiaTriThatChoO(h, cs, item.sSoChungTu);
                cs = Bang_arrCSMaCot["iNgay"];
                Bang_GanGiaTriThatChoO(h, cs, item.iNgay);
                Bang_arrMaHang[h] = iID_MaChungTu;
            });
            return false;
        }
        jQuery.ajaxSetup({ cache: false });
        var vR;
        var iNamLamViec = parent.jsNamLamViec;
        url = parent.BangDuLieu_Url_CheckTrungSoGhiSo;
        url += "?iID_MaChungTu=" + iID_MaChungTu + "&sSoChungTu=" + sSoChungTu + "&iNamLamViec=" + iNamLamViec;
        $.getJSON(url, function (item) {
            if (item.Trung) {
                var sText = "";
                if (item.DaDuyet) {
                    sText = "Số ghi số \"" + sSoChungTu + "\" đã được duyệt bạn không thể thêm!";
                    alert(sText);
                }
                else {
                    sText = "Số ghi số \"" + sSoChungTu + "\" đã có bạn có muốn thêm vào số ghi sổ này không!";
                    var Cf = confirm(sText);
                    if (Cf) {
                        var i = 0;
                        for (i = 0; i < Bang_nH; i++) {
                            if (item.iID_MaChungTu == Bang_arrMaHang[i]) {
                                Bang_keys.fnSetFocus(i, c);
                                parent.ChungTu_ThayDoiMaChungTu();
                                break;
                            }

                        }
                    }
                }

                url = urlServerPath + "KTCT_TienMat_ChungTuChiTiet/get_ThongTinChungTuMoi?iNamLamViec=" + iNamLamViec;
                $.getJSON(url, function (item) {
                    var cs;
                    var iID_MaChungTu = item.iID_MaChungTu;
                    cs = Bang_arrCSMaCot["sSoChungTu"];
                    Bang_GanGiaTriThatChoO(h, cs, item.sSoChungTu);
                    cs = Bang_arrCSMaCot["iNgay"];
                    Bang_GanGiaTriThatChoO(h, cs, item.iNgay);
                    //Bang_HideCloseDialog();
                    //Sua MaHang="": Day la hang them moi
                    Bang_arrMaHang[h] = iID_MaChungTu;
                    //                                    Bang_keys.fnSetFocus(0, cs);
                    //                                    parent.ChungTu_ThayDoiMaChungTu();
                });
            }
        });
        return false;
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
    return true;
}
function KiemTraHuyDuyet(iID_MaChungTu, sSoChungTu, iAction) {
    jQuery.ajaxSetup({ cache: false });
    var vR;
    var iNamLamViec = parent.jsNamLamViec;
    url = parent.BangDuLieu_Url_CheckTrungSoGhiSo;
    url += "?iID_MaChungTu=" + iID_MaChungTu + "&sSoChungTu=" + sSoChungTu + "&iNamLamViec=" + iNamLamViec;
    $.getJSON(url, function (item) {
        if (item.DaDuyet && iAction == "1") {
            document.getElementById("iHuyDuyet").value = 1;
            document.getElementById("btnHuyDuyet").click();
            Bang_HideCloseDialog();
            return 1;
        }
    });
    return 0;
}
/* Ham BangDuLieu_TinhTongHangCon
*   - Muc dinh: Tao 1 hang moi o tai vi tri 'h' lay du lieu tai vi tri 'hGiaTri'
*   - Dau vao:  + h: vi tri them
*               + hGiaTri: vi tri hang lay du lieu, =null: them 1 hang trong
*/
function BangDuLieu_ThemHangMoi(h) {
    var csH = 0;
    if (h != null && h >= 0) {
        //Thêm 1 hàng mới vào hàng h
        csH = Bang_nH; //csH = h;
    }
    else {
        //Thêm 1 hàng mới vào cuối bảng
        csH = Bang_nH;
    }
    Bang_ThemHang(csH);
    Bang_arrMaHang[csH] = "";
    var iNamLamViec = parent.jsNamLamViec;
    var url = urlServerPath + "KTCT_TienMat_ChungTuChiTiet/get_ThongTinChungTuMoi?iNamLamViec=" + iNamLamViec;
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
        parent.ChungTu_ThayDoiNgay(item.iNgay);
    });
    //Bỏ phần  sau vì khi thêm mới hàng tự gọi dialog hỏi ghi
    //    if (h >= 0) {
    //     BangDuLieu_hOld = h + 1;
    //    }
}

/* Su kien BangDuLieu_onKeypress_F2

- Muc dinh: goi ham them hang khi an phim F2
*/
function BangDuLieu_onKeypress_F2(h, c) {
    if (Bang_ChiDoc == false) {
        var csH = Bang_nH;
        if (parseFloat(Bang_nH) - iSoDongCTGS == 0) { //chi cho phep theo 1 dong chung tu GS
            BangDuLieu_ThemHangMoi(h + 1);
            BangDuLieu_CapNhatThayDoi(csH);
            //Tạo thêm 1 rows trong mảng Bang_arrEdit
            // Bang_arrEdit.splice(csH, 0, Bang_TaoDoiTuongMoi(Bang_arrEdit[h]));
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

        }
        //Bang_ShowCloseDialog();
    }
}
//Cập nhập thay đổi của hàng
function BangDuLieu_CapNhatThayDoi(h) {
    var c = 0;
    var cMax = Bang_nC;
    for (c = 0; c < cMax; c++) {
        Bang_arrThayDoi[h][c] = true;
    }
}
/* Su kien BangDuLieu_onKeypress_Delete
- Muc dinh: goi ham xoa hang khi an phim DELETE
*/
function BangDuLieu_onKeypress_Delete(h, c) {
    if (Bang_ChiDoc == false && h != null) {
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
    var bChon = 0;
    var sChuoi = "";
    for (var i = 0; i < Bang_nH; i++) {
        var rGiatri = Bang_LayGiaTri(i, "bChon");
        if (rGiatri == "1" || rGiatri == "true") {
            bChon = bChon + 1;
            sChuoi += "," + Bang_arrMaHang[i];
        }
    }
    if (typeof iAction == "undefined" || iAction == null || iAction < 0) {
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
        //đã duyệt ko duyệt tiếp
        if (iAction == 2) {
            var TrangThaiDuyet = Bang_arrGiaTri[h][6];
            if (TrangThaiDuyet == "Đã duyệt") {
                alert("Chứng từ đã được duyệt");
                return false;
            }
        }
        var iID_MaChungTu = Bang_arrMaHang[h];
        if (document.getElementById("idAction")) document.getElementById("idAction").value = iAction;
        if (document.getElementById("id_iID_MaChungTu_Action")) {
            document.getElementById("id_iID_MaChungTu_Action").value = sChuoi;
        }
        var jsKeToan_Check_MaChungTu = parent.jsKiemTra_MaChungTu(iID_MaChungTu);
        if (KiemTraHuyDuyet(iID_MaChungTu, parent.GoiHam_ChungTu_Bang_LayTruong("sSoChungTu"), iAction) == 0) {
            parent.ChungTu_YeuCauLuuDuLieu(iID_MaChungTu, Bang_arrHangDaXoa[h], iAction);
            jsKeToan_CheckChungTuGhiSo = parent.jsKiemTraChungSoChungTuGhiSo(iID_MaChungTu, parent.GoiHam_ChungTu_Bang_LayTruong("sSoChungTu"));
        }
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
    var c = Bang_arrCSMaCot["sNoiDung"];
    if (Bang_arrGiaTri[Bang_nH - 1][c] == "" || parent.jsKeToan_Check_MaChungTu == false) {//Nếu là thêm mới hoặc nội dung chứng từ ghi sổ chưa có thì thêm ghi lên trường nội dung
    //if (Bang_arrGiaTri[Bang_nH - 1][c] == "") {
        //Bang_GanGiaTriThatChoO(Bang_keys.Row(), c, GiaTri);
        var KyTu = new String("");
        var sStr = new String("");
        KyTu = GiaTri.toString().substring(0, 1);
        KyTu = KyTu.toUpperCase();
        sStr = GiaTri.toString().substring(1, GiaTri.toString().length);
        Bang_GanGiaTriThatChoO(Bang_keys.Row(), c, KyTu + sStr);
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
    document.getElementById("id_sSoChungTu").value = parent.sSoChungTuTimKiem;
    //if (BangDuLieu_hLuu && BangDuLieu_hLuu >= 0) {
    if (BangDuLieu_hLuu >= 0) {
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