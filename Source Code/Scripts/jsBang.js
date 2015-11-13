/* Ba

*/

var browserID = 'mozilla';//ie = 'msie'
var urlServerPath = '/';
function DetectBrowser()
{
    jQuery.each(jQuery.browser, function (i, val) {
        if (val == true) {
            browserID = i.toString();
        }
    });
}

var Bang_keys;

var Bang_Scroll_Size = 17; //Chiều rộng của cột tiêu đề
var Bang_ID = "BangDuLieu";
var Bang_ID_Div = ""; //DIV bao ngoài của cả bảng
var Bang_ID_TB00 = "";
var Bang_ID_TB00_Div = "";
var Bang_ID_TB01 = "";
var Bang_ID_TB01_Div = "";
var Bang_ID_TB10 = "";
var Bang_ID_TB10_Div = "";
var Bang_ID_TB11 = "";
var Bang_ID_TB11_Div = "";
var Bang_ID_TR_BangDuLieu = "";
var BangDuLieuID_Slide = "";
var BangDuLieuID_Fixed = "";
var BangDuLieuID_Slide_Div = "";

/* Bang_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var Bang_Url_getGiaTri = "";
/* Bang_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var Bang_Url_getDanhSach = "";
var Bang_arrDSTruongTien = "";

function Bang_adjustTable(okInit) {
    //Hiệu chỉnh lại các bảng con khi khung thay đổi
    var w0 = fnGetWidthById(Bang_ID_Div);
    var h0 = fnGetHeightById(Bang_ID_Div);
    var Bang_FixedRow_Height = fnGetHeightById(Bang_ID_TB01); //Chiều cao của phần hàng tiêu đề
    var Bang_FixedCol_Width = fnGetWidthById(Bang_ID_TB00); //Chiều rộng của cột tiêu đề
    $('#' + Bang_ID_TB11_Div).css('width', w0 - 4 - Bang_FixedCol_Width);
    $('#' + Bang_ID_TB11_Div).css('height', h0 - 4 - Bang_FixedRow_Height);
    $('#' + Bang_ID_TB01_Div).css('width', w0 - 4 - Bang_FixedCol_Width - Bang_Scroll_Size);
    $('#' + Bang_ID_TB10_Div).css('height', h0 - 4 - Bang_FixedRow_Height - Bang_Scroll_Size);
    //alert(w0 +',' + Bang_FixedCol_Width + ','+(w0 - 4 - Bang_FixedCol_Width));
}

function Bang_GanGiaTriCacBienTongThe() {
    Bang_ID_Div = Bang_ID + "_div";
    Bang_ID_TB00 = Bang_ID + "_TB00";
    Bang_ID_TB00_Div = Bang_ID_TB00 + "_div";
    Bang_ID_TB01 = Bang_ID + "_TB01";
    Bang_ID_TB01_Div = Bang_ID_TB01 + "_div";
    Bang_ID_TB10 = Bang_ID + "_TB10";
    Bang_ID_TB10_Div = Bang_ID_TB10 + "_div";
    Bang_ID_TB11 = Bang_ID + "_TB11";
    Bang_ID_TB11_Div = Bang_ID_TB11 + "_div";
    Bang_ID_TR_BangDuLieu = Bang_ID + "_TR_DuLieu";
    BangDuLieuID_Slide = Bang_ID_TB11;
    BangDuLieuID_Fixed = Bang_ID_TB10;
    BangDuLieuID_Slide_Div = Bang_ID_TB11_Div;
}

$(document).ready(function () {
    Bang_GanGiaTriCacBienTongThe();

    Bang_KhoiTao();
    Bang_HienThiDuLieu();

    Bang_keys = new KeyTable({
        "table": document.getElementById(BangDuLieuID_Slide)
    });


    Bang_adjustTable(true);
    $('#' + BangDuLieuID_Slide_Div).scrollLeft(0);
    $('#' + BangDuLieuID_Slide_Div).scrollTop(0);

    if (Bang_ChiDoc == false) {
        Bang_Ready();
        //Bang_keys.fnSetFocusFirstEditCell();
    }
    if (Bang_nH > 0 && Bang_nC > 0) {
        Bang_keys.fnSetFocus(0, 0);
    }


    var fn = window[Bang_ID + '_onLoad'];
    if (typeof fn == 'function') {
        fn();
    }
});

$(window).resize(function () {
    Bang_adjustTable(false);
});

function Bang_onFocus(h, c) {
    var TenHam = Bang_ID + '_onCellFocus';
    var fn = window[TenHam];
    if (typeof fn == 'function') {
        fn(h, c);
    }
}

function Bang_onBeforeFocus(h, c) {
    var TenHam = Bang_ID + '_onCellBeforeFocus';
    var fn = window[TenHam];
    if (typeof fn == 'function') {
        return fn(h, c);
    }
    return true;
}

function Bang_onKeypress_F(strKeyEvent) {
    var h = Bang_keys.Row();
    var c = Bang_keys.Col() - Bang_nC_Fixed;
    var TenHam = Bang_ID + '_onKeypress_' + strKeyEvent;
    var fn = window[TenHam];
    if (typeof fn == 'function') {
        if (fn(h, c) == false) {
            return false;
        }
    }
    if (strKeyEvent == "F10") {
        Bang_HamTruocKhiKetThuc();
    }
}

function Bang_onDblClick(h, c) {
    var TenHam = Bang_ID + '_onDblClick';
    var fn = window[TenHam];
    if (typeof fn == 'function') {
        if (fn(h, c) == false) {
            return false;
        }
    }
    if(Bang_arrType[c]==2) {
        //Neu o khong duoc nhap thi bo qua
        if (Bang_arrEdit[h][c] == false) {
            return false;
        }

        //Goi ham BeforeEdit neu co, Neu ham tra lai gia tri la FALSE thi bo qua
        var fnBeforeEdit = window['Bang_onCellBeforeEdit'];
        if (typeof fnBeforeEdit == 'function') {
            if (fnBeforeEdit(h, c) == false) {
                return false;
            }
        }

        var checkbox_value = Bang_arrGiaTri[h][c];
        if (checkbox_value == "1") {
            checkbox_value = "0";
        }
        else {
            checkbox_value = "1";
        }
        Bang_GanGiaTriThatChoO(h, c, checkbox_value);
      
        var checkbox_fn = window[Bang_ID + '_onCellAfterEdit'];
       
        if (typeof checkbox_fn == 'function') {
            checkbox_fn(h, c);
        }
    }
    return true;
}

//Ham chi dinh JSON lay du lieu
var Bang_curH = -1;
var Bang_curC = -1;
function Bang_txtONhapDuLieu_Autocomplete_onSource(txt, request, response) {
    var TenHam = Bang_ID + '_txtONhapDuLieu_Autocomplete_onSource';
    var fn = window[TenHam];
    if (typeof fn == 'function') {
        return fn(txt, request, response);
    }
    var posCurr = Bang_keys.fnGetCurrentPosition();
    var h = posCurr[1];
    var c = posCurr[0] + Bang_nC_Fixed;
    if (h != null && c != null) {
              Bang_curH = h;
        Bang_curC = c;
        var arrTruong = Bang_LayTenTruongVaTruongGan(c);
        var Truong = arrTruong[0], TruongGan = arrTruong[1];

        var url = Bang_Url_getDanhSach;
        if (Truong == "iID_MaMucLucNganSach") {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term, DSGiaTri: Bang_GhepGiaTriCuaMucLucNganSach }, response);
        }
        else if (Truong == "sTenTaiKhoanGiaiThich") {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term, DSGiaTri: Bang_GhepGiaTriCuaMaTaiKhoanGiaiThich }, response);
        }
        else if (Truong == "sTenBacLuong") {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term, DSGiaTri: Bang_GhepGiaTriCuaMaNgachLuong }, response);
        }
        else if (Truong == "sTenCongTrinh") {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term, DSGiaTri: Bang_GhepGiaTriCuaMaDonVi }, response);
        }
        else if (Truong == "sTenNhomTaiSan") {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term, DSGiaTri: Bang_GhepGiaTriCuaTruong("sTenNhomTaiSan", "iID_MaLoaiTaiSan") }, response);
        }
        else if (Truong == "sTenTaiSan") {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term, DSGiaTri: Bang_GhepGiaTriCuaMaTaiSan}, response);
        }
        else if (Truong == "sTenChuDauTu") {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term, DSGiaTri: Bang_GhepGiaTriCuaTruong("sTenChuDauTu","iID_MaDonVi") }, response);
        }
        else if (Truong == "sTenBanQuanLy") {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term, DSGiaTri: Bang_GhepGiaTriCuaTruong("sTenBanQuanLy", "iID_MaChuDauTu") }, response);
        }
        else if (Truong == "sTinhChatCapThu") {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term, DSGiaTri: Bang_GhepGiaTriCuabLoaiTinhChat }, response);
        }
        else if (Truong == "sTenDonVi_BaoDam") {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term }, response);
        }
        else
        {
            $.getJSON(url, { Truong: Truong, GiaTri: request.term }, response);
        }
    }
}

function Bang_txtONhapDuLieu_Autocomplete_onSelect(txt, event, ui) {
    var TenHam = Bang_ID + '_txtONhapDuLieu_Autocomplete_onSelect';
    var fn = window[TenHam];
    if (typeof fn == 'function') {
        return fn(txt, event, ui);
    }
    return null;
}

function Bang_txtONhapDuLieu_Autocomplete_renderItem(txt, ul, item) {
    var TenHam = Bang_ID + '_txtONhapDuLieu_Autocomplete_renderItem';
    var fn = window[TenHam];
    if (typeof fn == 'function') {
        return fn(txt, ul, item);
    }
    var v = $(txt).val();
    var i;
    var text = item.label;
    for (i = text.length - v.length; i >= 0; i--) {
        if (v.toUpperCase() == text.substr(i, v.length).toUpperCase()) {
            text = text.substr(0, i) + '<b>' + text.substr(i, v.length) + '</b>' + text.substr(i + v.length);
        }
    }
    return $('<li></li>')
                .data('item.autocomplete', item)
                .append('<a>' + text + '</a>')
                .appendTo(ul);
}

//Hàm lấy tên trường và tên trường gán của cột 'c' trong bảng dữ liệu
function Bang_LayTenTruongVaTruongGan(c) {
    var Truong = "", TruongGan = "";
    //Xác định tên trường tìm dữ liệu
    if (Bang_arrMaCot[c]=="TN_sLNS") {
        Truong = 'TN_sLNS';
        TruongGan = 'TN_sLNS';
    }
    else if ("sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,".indexOf(Bang_arrMaCot[c] + ',') >= 0) {
        Truong = 'iID_MaMucLucNganSach';
        TruongGan = 'iID_MaMucLucNganSach';
    }
    else if (Bang_arrMaCot[c].startsWith("iID_MaPhongBanDich")) {
        Truong = 'iID_MaPhongBanDich';
        TruongGan = 'iID_MaPhongBanDich';
    }
    else if (Bang_arrMaCot[c].startsWith("iID_MaDonVi")) {
        Truong = 'sTenDonVi';
        TruongGan = 'iID_MaDonVi';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenDonVi_BaoDam")) {
        Truong = 'sTenDonVi_BaoDam';
        TruongGan = 'iID_MaDonVi';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenDonVi")) {
        Truong = 'sTenDonViCoTen';
        TruongGan = 'iID_MaDonVi';
    }
    
    else if (Bang_arrMaCot[c].startsWith("sTenCongTrinh")) {
        Truong = 'sTenCongTrinh';
        TruongGan = 'sMaCongTrinh';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenTaiKhoanGiaiThich")) {
        Truong = 'sTenTaiKhoanGiaiThich';
        TruongGan = 'iID_MaTaiKhoanGiaiThich';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenTaiKhoan")) {
        Truong = 'sTenTaiKhoan';
        TruongGan = 'iID_MaTaiKhoan';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenNhanVien")) {
        Truong = 'sTenNhanVien';
        TruongGan = 'iID_MaNhanVien';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenPhongBan")) {
        Truong = 'sTenPhongBan';
        TruongGan = 'iID_MaPhongBan';
    }
    else if (Bang_arrMaCot[c].startsWith("sTinhChatCapThu")) {
        Truong = 'sTinhChatCapThu';
        TruongGan = 'iID_MaTinhChatCapThu';
    }
    else if (Bang_arrMaCot[c].startsWith("sSoChungTuCapThu")) {
        Truong = 'sSoChungTuCapThu';
        TruongGan = 'iID_MaChungTu_Duyet';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenLoaiTaiSan")) {
        Truong = 'sTenLoaiTaiSan';
        TruongGan = 'iID_MaLoaiTaiSan';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenNhomTaiSan")) {
        Truong = 'sTenNhomTaiSan';
        TruongGan = 'iID_MaNhomTaiSan';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenTaiSan")) {
        Truong = 'sTenTaiSan';
        TruongGan = 'iID_MaTaiSan';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenChuongTrinhMucTieu")) {
        Truong = 'iID_MaChuongTrinhMucTieu';
        TruongGan = 'iID_MaChuongTrinhMucTieu';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenPhuCap")) {
        Truong = 'sTenPhuCap';
        TruongGan = 'iID_MaPhuCap';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenNgachLuong")) {
        Truong = 'sTenNgachLuong';
        TruongGan = 'iID_MaNgachLuong';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenBacLuong")) {
        Truong = 'sTenBacLuong';
        TruongGan = 'iID_MaBacLuong';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenNgoaiTe")) {
        Truong = 'sTenNgoaiTe';
        TruongGan = 'iID_MaNgoaiTe';
    }
    else if (Bang_arrMaCot[c].startsWith("sMaTruong")) {
        Truong = 'sMaTruong';
        TruongGan = 'sMaTruong';
    }
    else if (Bang_arrMaCot[c].startsWith("sTruongTienBangLuong")) {
        Truong = 'sTruongTienBangLuong';
        TruongGan = 'sTruongTienBangLuong';
    }
    else if (Bang_arrMaCot[c].startsWith("iID_MaKyHieuHachToan")) {
        Truong = 'iID_MaKyHieuHachToan';
        TruongGan = 'iID_MaKyHieuHachToan';
    }
    else if (Bang_arrMaCot[c].startsWith("sLoaiST")) {
        Truong = 'sLoaiST';
        TruongGan = 'sLoaiST';
    }
    else if (Bang_arrMaCot[c].startsWith("sThuChi")) {
        Truong = 'sThuChi';
        TruongGan = 'sThuChi';
    }
    else if (Bang_arrMaCot[c].startsWith("sKyHieuNgoaiTe")) {
        Truong = 'sKyHieuNgoaiTe';
        TruongGan = 'iID_MaNgoaiTe';
    }
    else if (Bang_arrMaCot[c].startsWith("iTinhChat")) {
        Truong = 'iTinhChat';
        TruongGan = 'iTinhChat';
    }
    else if (Bang_arrMaCot[c].startsWith("iNgay")) {
        Truong = 'iNgay';
        TruongGan = 'iNgay';
    }
    else if (Bang_arrMaCot[c].startsWith("iThang")) {
        Truong = 'iThang';
        TruongGan = 'iThang';
    }
    else if (Bang_arrMaCot[c].startsWith("iLoaiThuChi")) {
        Truong = 'iLoaiThuChi';
        TruongGan = 'iLoaiThuChi';
    }
    //Ghép thêm phần đuôi của trường gán
    if (Bang_arrMaCot[c].endsWith("_Co")) {
        TruongGan += '_Co';
    }
    else if (Bang_arrMaCot[c].endsWith("_No")) {
        TruongGan += '_No';
    }
    else if (Bang_arrMaCot[c].endsWith("_Nhan")) {
        TruongGan += '_Nhan';
    }
    else if (Bang_arrMaCot[c].endsWith("_Tra")) {
        TruongGan += '_Tra';
    }
    else if (Bang_arrMaCot[c].endsWith("_No_Thu")) {
        TruongGan += '_No_Thu';
    }
    else if (Bang_arrMaCot[c].endsWith("_Co_Thu")) {
        TruongGan += '_Co_Thu';
    }
    else if (Bang_arrMaCot[c].endsWith("_No_Chi")) {
        TruongGan += '_No_Chi';
    }
    else if (Bang_arrMaCot[c].endsWith("_Co_Chi")) {
        TruongGan += '_Co_Chi';
    }
    else if (Bang_arrMaCot[c].endsWith("_Co_Chi_NgoaiTe")) {
        TruongGan += '_Co_Chi_NgoaiTe';
    }
    else if (Bang_arrMaCot[c].endsWith("_Co_Thu_NgoaiTe")) {
        TruongGan += '_Co_Thu_NgoaiTe';
    }
    else if (Bang_arrMaCot[c].endsWith("_No_Thu_NgoaiTe")) {
        TruongGan += '_No_Thu_NgoaiTe';
    }
    else if (Bang_arrMaCot[c].endsWith("_No_Chi_NgoaiTe")) {
        TruongGan += '_No_Chi_NgoaiTe';
    }
    else if (Bang_arrMaCot[c].endsWith("_Thu")) {
        TruongGan += '_Thu';
    }
    else if (Bang_arrMaCot[c].endsWith("_Chi")) {
        TruongGan += '_Chi';
    }
    else if (Bang_arrMaCot[c].endsWith("_ThuChi")) {
        TruongGan += '_ThuChi';
    }
    else if (Bang_arrMaCot[c].endsWith("_TongDauTu")) {
        TruongGan += '_TongDauTu';
    }
    else if (Bang_arrMaCot[c].endsWith("_TongDuToan")) {
        TruongGan += '_TongDuToan';
    }
    else if (Bang_arrMaCot[c].endsWith("_SoTien")) {
        TruongGan += '_SoTien';
    }
    else if (Bang_arrMaCot[c].endsWith("_DauNam")) {
        TruongGan += '_DauNam';
    }
    else if (Bang_arrMaCot[c].endsWith("_SoTienDauNam")) {
        TruongGan += '_SoTienDauNam';
    }
    else if (Bang_arrMaCot[c].endsWith("_SoTienDieuChinh")) {
        TruongGan += '_SoTienDieuChinh';
    }
    else if (Bang_arrMaCot[c].endsWith("_DieuChinh")) {
        TruongGan += '_DieuChinh';
    }
    else if (Bang_arrMaCot[c].endsWith("_rSoTienBTCCap")) {
        TruongGan += '_rSoTienBTCCap';
    }
    else if (Bang_arrMaCot[c].endsWith("_rSoTienDVDeNghi")) {
        TruongGan += '_rSoTienDVDeNghi';
    }
    else if (Bang_arrMaCot[c].endsWith("_rSoTienDuToan")) {
        TruongGan += '_rSoTienDuToan';
    }
    else if (Bang_arrMaCot[c].endsWith("_rSoTienCapPhat")) {
        TruongGan += '_rSoTienCapPhat';
    }
    else if (Bang_arrMaCot[c].endsWith("_SoTien_rDonViDeNghi")) {
        TruongGan += '_SoTien_rDonViDeNghi';
    }
    else if (Bang_arrMaCot[c].endsWith("_rDonViDeNghi")) {
        TruongGan += '_rDonViDeNghi';
    }
    else if (Bang_arrMaCot[c].endsWith("_SoTien_rDonViThuTamUng")) {
        TruongGan += '_SoTien_rDonViThuTamUng';
    }
    else if (Bang_arrMaCot[c].endsWith("_rDonViThuTamUng")) {
        TruongGan += '_rDonViThuTamUng';
    }
    else if (Bang_arrMaCot[c].endsWith("_SoTien_rDonViThu")) {
        TruongGan += '_SoTien_rDonViThu';
    }
    else if (Bang_arrMaCot[c].endsWith("_rDonViThu")) {
        TruongGan += '_rDonViThu';
    }
    else if (Bang_arrMaCot[c].endsWith("_SoTien_rCucTaiChinhDeNghi")) {
        TruongGan += '_SoTien_rCucTaiChinhDeNghi';
    }
    else if (Bang_arrMaCot[c].endsWith("_rCucTaiChinhDeNghi")) {
        TruongGan += '_rCucTaiChinhDeNghi';
    }
    else if (Bang_arrMaCot[c].endsWith("_SoTien_rCucTaiChinhThuTamUng")) {
        TruongGan += '_SoTien_rCucTaiChinhThuTamUng';
    }
    else if (Bang_arrMaCot[c].endsWith("_rCucTaiChinhThuTamUng")) {
        TruongGan += '_rCucTaiChinhThuTamUng';
    }
    else if (Bang_arrMaCot[c].endsWith("_SoTien_rCucTaiChinhThu")) {
        TruongGan += '_SoTien_rCucTaiChinhThu';
    }
    else if (Bang_arrMaCot[c].endsWith("_rCucTaiChinhThu")) {
        TruongGan += '_rCucTaiChinhThu';
    }
    else if (Bang_arrMaCot[c].endsWith("_ChuDauTuTamUng")) {
        TruongGan += '_ChuDauTuTamUng';
    }
    else if (Bang_arrMaCot[c].endsWith("_ChuDauTuThanhToan")) {
        TruongGan += '_ChuDauTuThanhToan';
    }
    else if (Bang_arrMaCot[c].endsWith("_ChuDauTuThuTamUng")) {
        TruongGan += '_ChuDauTuThuTamUng';
    }
    else if (Bang_arrMaCot[c].endsWith("_ChuDauTuThuKhac")) {
        TruongGan += '_ChuDauTuThuKhac';
    }
    else if (Bang_arrMaCot[c].endsWith("_ChuDauTuDonViThuHuong")) {
        TruongGan += '_ChuDauTuDonViThuHuong';
    }
    else if (Bang_arrMaCot[c].endsWith("_DeNghiPheDuyetTamUng")) {
        TruongGan += '_DeNghiPheDuyetTamUng';
    }
    else if (Bang_arrMaCot[c].endsWith("_DeNghiPheDuyetThanhToan")) {
        TruongGan += '_DeNghiPheDuyetThanhToan';
    }
    else if (Bang_arrMaCot[c].endsWith("_DeNghiPheDuyetThuTamUng")) {
        TruongGan += '_DeNghiPheDuyetThuTamUng';
    }
    else if (Bang_arrMaCot[c].endsWith("_DeNghiPheDuyetThuKhac")) {
        TruongGan += '_DeNghiPheDuyetThuKhac';
    }
    else if (Bang_arrMaCot[c].endsWith("_DeNghiPheDuyetDonViThuHuong")) {
        TruongGan += '_DeNghiPheDuyetDonViThuHuong';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenChuDauTu")) {
        Truong = 'sTenChuDauTu';
        TruongGan = 'iID_MaChuDauTu';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenBanQuanLy")) {
        Truong = 'sTenBanQuanLy';
        TruongGan = 'iID_MaBanQuanLy';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenDonViThiCong")) {
        Truong = 'sTenDonViThiCong';
        TruongGan = 'iID_MaDonViThiCong';
    }
    else if (Bang_arrMaCot[c].startsWith("sLoaiKeHoachVon")) {
        Truong = 'sLoaiKeHoachVon';
        TruongGan = 'iID_MaLoaiKeHoachVon';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenLoaiDieuChinh")) {
        Truong = 'sTenLoaiDieuChinh';
        TruongGan = 'iID_MaLoaiDieuChinh';
    }
    else if (Bang_arrMaCot[c].startsWith("sTenDuAn")) {
        Truong = 'sTenDuAn';
        TruongGan = 'iID_MaDanhMucDuAn';
    }
    else if (Bang_arrMaCot[c].startsWith("iLoai")) {
        Truong = 'iLoai';
        TruongGan = 'iLoai';
    }
    else if (Bang_arrMaCot[c].startsWith("iThamQuyen")) {
        Truong = 'iThamQuyen';
        TruongGan = 'iThamQuyen';
    }
    else if (Bang_arrMaCot[c].startsWith("iTinhChat")) {
        Truong = 'iTinhChat';
        TruongGan = 'iTinhChat';
    }
    else if (Bang_arrMaCot[c].startsWith("iNhom")) {
        Truong = 'iNhom';
        TruongGan = 'iNhom';
    }
    return [Truong, TruongGan];
}

//Hàm ghép các giá trị của các trường Mục lục ngân sách tính đến cột 'c'
function Bang_GhepGiaTriCuaMucLucNganSach(csC) {
    var arrTG = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG".split(",");
    var vR = "";
    if (typeof csC == "undefined") {
        csC = Bang_curC;
    }
    for (var i = 0; i < arrTG.length; i++) {
        var cs = Bang_arrCSMaCot[arrTG[i]];
        if (cs == csC) {
            break;
        }
        if (i > 0) vR += ",";
        vR += Bang_arrGiaTri[Bang_curH][cs];
    }
    return vR;
}

//Hàm lấy giá trị của ô iID_MaTaiKhoan
function Bang_GhepGiaTriCuaMaTaiKhoanGiaiThich() {
    var vR = "";
    var cs;
    var h = Bang_keys.Row();
    var c = Bang_keys.Col();
    //Ghép thêm phần đuôi của trường gán
    var TruongGan = "iID_MaTaiKhoan";
    if (Bang_arrMaCot[c].endsWith("_Co")) {
        TruongGan += '_Co';
    }
    else if (Bang_arrMaCot[c].endsWith("_No")) {
        TruongGan += '_No';
    }
    else if (Bang_arrMaCot[c].endsWith("_Nhan")) {
        TruongGan += '_Nhan';
    }
    else if (Bang_arrMaCot[c].endsWith("_Tra")) {
        TruongGan += '_Tra';
    }
    else if (Bang_arrMaCot[c].endsWith("_No_Thu")) {
        TruongGan += '_No_Thu';
    }
    else if (Bang_arrMaCot[c].endsWith("_Co_Thu")) {
        TruongGan += '_Co_Thu';
    }
    else if (Bang_arrMaCot[c].endsWith("_No_Chi")) {
        TruongGan += '_No_Chi';
    }
    else if (Bang_arrMaCot[c].endsWith("_Co_Chi")) {
        TruongGan += '_Co_Chi';
    }


    else if (Bang_arrMaCot[c].endsWith("_No_Thu_NgoaiTe")) {
        TruongGan += '_No_Thu_NgoaiTe';
    }
    else if (Bang_arrMaCot[c].endsWith("_Co_Thu_NgoaiTe")) {
        TruongGan += '_Co_Thu_NgoaiTe';
    }
    else if (Bang_arrMaCot[c].endsWith("_No_Chi_NgoaiTe")) {
        TruongGan += '_No_Chi_NgoaiTe';
    }
    else if (Bang_arrMaCot[c].endsWith("_Co_Chi_NgoaiTe")) {
        TruongGan += '_Co_Chi_NgoaiTe';
    }
    
    
    else if (Bang_arrMaCot[c].endsWith("_Thu")) {
        TruongGan += '_Thu';
    }
    else if (Bang_arrMaCot[c].endsWith("_Chi")) {
        TruongGan += '_Chi';
    }
    cs = Bang_arrCSMaCot[TruongGan];
    //    if (Bang_arrMaCot[c] == "sTenTaiKhoanGiaiThich_No") {
    //        cs = Bang_arrCSMaCot["iID_MaTaiKhoan_No"];
    //    }
    //    else if (Bang_arrMaCot[c] == "sTenTaiKhoanGiaiThich_Co") {
    //        cs = Bang_arrCSMaCot["iID_MaTaiKhoan_Co"];
    //    }
    //    else if (Bang_arrMaCot[c] == "sTenTaiKhoanGiaiThich_Thu") {
    //        cs = Bang_arrCSMaCot["iID_MaTaiKhoan_Thu"];
    //    }
    //    else if (Bang_arrMaCot[c] == "sTenTaiKhoanGiaiThich_Chi") {
    //        cs = Bang_arrCSMaCot["iID_MaTaiKhoan_Chi"];
    //    }
    vR = Bang_arrGiaTri[h][cs];
    return vR;
}

//Hàm ghép mã của một trường
//sTruongTen là trường nhập trường mã là trường cần ghép
function Bang_GhepGiaTriCuaTruong(sTruongTen, sTruongMa) {
    var vR = "";
    var cs;
    var h = Bang_keys.Row();
    var c = Bang_keys.Col();
    if (Bang_arrMaCot[c] == sTruongTen) {
        cs = Bang_arrCSMaCot[sTruongMa];
        vR = Bang_arrGiaTri[h][cs];
    }
    return vR;
}

// Hàm lấy mã của ô ngạch lương
function Bang_GhepGiaTriCuabLoaiTinhChat() {
    var vR = "";
    var cs;
    var h = Bang_keys.Row();
    var c = Bang_keys.Col();
    if (Bang_arrMaCot[0] == "bLoai") {
        cs = Bang_arrCSMaCot["bLoai"];
        vR = Bang_arrGiaTri[h][cs];
    }
    return vR;
}


// Hàm lấy mã của ô ngạch lương
function Bang_GhepGiaTriCuaMaNgachLuong() {
    var vR = "";
    var cs;
    var h = Bang_keys.Row();
    var c = Bang_keys.Col();
    if (Bang_arrMaCot[c] == "sTenBacLuong") {
        cs = Bang_arrCSMaCot["iID_MaNgachLuong"];
        vR = Bang_arrGiaTri[h][cs];
    }    
    return vR;
}

// Hàm lấy mã của ô đơn vị
function Bang_GhepGiaTriCuaMaDonVi() {
    var vR = "";
    var cs;
    var h = Bang_keys.Row();
    var c = Bang_keys.Col();
    if (Bang_arrMaCot[c] == "sTenCongTrinh") {
        cs = Bang_arrCSMaCot["iID_MaDonVi"];
        vR = Bang_arrGiaTri[h][cs];
    }
    return vR;
}

// Hàm lấy mã của ô tài sản
function Bang_GhepGiaTriCuaMaTaiSan() {
    var vR = "";
    var cs;
    var h = Bang_keys.Row();
    var c = Bang_keys.Col();
    if (Bang_arrMaCot[c] == "sTenTaiSan") {
        cs = Bang_arrCSMaCot["sTinhChat"];
        vR = Bang_arrGiaTri[h][cs];
    }
    return vR;
}

//Điền thông tin vào các mã chi tiết mục lục ngân sách
function Bang_DienMucLucNganSach(h, c, item) {
    var i, cs, TenTruong, GiaTri;
    var arrMucLuc = "sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG,sMoTa".split(",");
    var okDaQuaCot = false;

    if (item.CoChiTiet == "1") {
        //Có 1 mục lục ngân sách gần với mã chi tiết nhập vào
        //Gán lại tất cả các giá trị đã có
        var ThongTinThem = item.ThongTinThem;
        var arr = ThongTinThem.split("#|");
        for (i = 0; i < arr.length; i++) {
            var arr1 = arr[i].split("##");
            TenTruong = arr1[0];
            GiaTri = arr1[1];
            cs = Bang_arrCSMaCot[TenTruong];
            if (cs >= 0) {
                Bang_GanGiaTriThatChoO(h, cs, GiaTri);
                if (okDaQuaCot) {
                    //Những ô sau ô 'c' sẽ được gán chỉ đọc không thay đổi
                    //Bang_arrEdit[h][cs] = false;
                    Bang_arrEdit[h][cs] = true;
                }
                if (Bang_arrMaCot[c] == TenTruong) {
                    okDaQuaCot = true;
                }
            }
            else if (TenTruong.startsWith('b')) {
                TenTruong = TenTruong.substring(1);
                cs = Bang_arrCSMaCot[TenTruong];
                if (cs >= 0) {
                    Bang_arrEdit[h][cs] = (GiaTri == "1") ? true : false;
                }
            }
        }
        Bang_XacDinhCotHienThi();
        ///Tuannn comment ngày 12/12/2012 để tránh bị nextcell()
//        if (Bang_arrMaCot[c] != "sTNG") {
//            Bang_keys.fnSetFocusNextCell();
//        }
    }
    else {
        for (i = 0; i < arrMucLuc.length; i++) {
            TenTruong = arrMucLuc[i];
            cs = Bang_arrCSMaCot[TenTruong];
            Bang_arrEdit[h][cs] = true;
            if (okDaQuaCot) {
                //Những ô sau ô 'c' sẽ được gán giá trị ""
                Bang_GanGiaTriThatChoO(h, cs, "");
            }
            if (Bang_arrMaCot[c] == TenTruong) {
                okDaQuaCot = true;
            }
        }
        if (item.CoHangPhuHop == "0") {
            for (i = 0; i < Bang_arrDSTruongTien.length; i++) {
                TenTruong = Bang_arrDSTruongTien[i];
                cs = Bang_arrCSMaCot[TenTruong];
                if (cs >= 0) {
                    Bang_arrEdit[h][cs] = true;
                }
            }
            Bang_XacDinhCotHienThi();
        }
    }
}

/* Su kien Bang_onCellBeforeEdit
*   - Muc dinh: Su kien xuat hien truoc khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*/
function Bang_onCellBeforeEdit(h, c) {
    //Nếu Mã tài khoản không có thì không nhập mã giải thích
    if (Bang_arrMaCot[c].startsWith("sTenTaiKhoanGiaiThich")) {
        if (Bang_GhepGiaTriCuaMaTaiKhoanGiaiThich() == "") {
            return false;
        }
    }
    //Goi ham BeforeEdit neu co, Neu ham tra lai gia tri la FALSE thi bo qua
    var fnBeforeEdit = window[Bang_ID + '_onCellBeforeEdit'];
    if (typeof fnBeforeEdit == 'function') {
        return fnBeforeEdit(h, c);
    }
    return true;
}

/* Su kien Bang_onCellAfterEdit
*   - Muc dinh: Su kien xuat hien sau khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*/
function Bang_onCellAfterEdit(h, c) {
    var fnCellAfterEdit = window[Bang_ID + '_onCellAfterEdit'];
    
    if (Bang_arrType[c] == "3") {
        var arrTruong = Bang_LayTenTruongVaTruongGan(c);
        var Truong = arrTruong[0], TruongGan = arrTruong[1];
        var url = Bang_Url_getGiaTri;
        var GiaTriDau = Bang_arrGiaTri[h][c].split(',')[0];
        var index = GiaTriDau.indexOf('-', 7);
        var GiaTri = Bang_arrGiaTri[h][c].split('-')[0];
        if (index > 0) GiaTri = GiaTriDau;
        GiaTri = $.trim(GiaTri);

        url += '?Truong=' + Truong;
        url += '&GiaTri=' + GiaTri;

        //Lấy các giá trị mà trường phụ thuộc
        if ("sLNS,sL,sK,sM,sTM,sTTM,sNG,sTNG".indexOf(Bang_arrMaCot[c]) >= 0) {
            url += '&DSGiaTri=' + Bang_GhepGiaTriCuaMucLucNganSach();
        }
        else if (Bang_arrMaCot[c].startsWith("sTenTaiKhoanGiaiThich")) {
            url += '&DSGiaTri=' + Bang_GhepGiaTriCuaMaTaiKhoanGiaiThich();
        }
        else if (Bang_arrMaCot[c].startsWith("sTenBacLuong")) {
            url += '&DSGiaTri=' + Bang_GhepGiaTriCuaMaNgachLuong();
        }
        else if (Bang_arrMaCot[c].startsWith("sTenChuDauTu")) {
            url += '&DSGiaTri=' + Bang_GhepGiaTriCuaTruong("sTenChuDauTu","iID_MaDonVi");
        }
        else if (Bang_arrMaCot[c].startsWith("sTenBanQuanLy")) {
            url += '&DSGiaTri=' + Bang_GhepGiaTriCuaTruong("sTenBanQuanLy", "iID_MaChuDauTu");
        }
        else if (Bang_arrMaCot[c].startsWith("sTenCongTrinh")) {
            url += '&DSGiaTri=' + Bang_GhepGiaTriCuaMaDonVi();
        }
        else if (Bang_arrMaCot[c].startsWith("sTenTaiSan")) {
            url += '&DSGiaTri=' + Bang_GhepGiaTriCuaMaTaiSan();
        }
        else if (Bang_arrMaCot[c].startsWith("sTinhChatCapThu")) {
            url += '&DSGiaTri=' + Bang_GhepGiaTriCuabLoaiTinhChat();
        }
        else if (Bang_arrMaCot[c].startsWith("sTenNhomTaiSan")) {
            url += '&DSGiaTri=' + Bang_GhepGiaTriCuaTruong("sTenNhomTaiSan", "iID_MaLoaiTaiSan");
        }
        url = unescape(url);

        var csMaMucLuc = Bang_arrCSMaCot["iID_MaMucLucNganSach"];
        if (csMaMucLuc >= 0) {
            Bang_arrThayDoi[h][csMaMucLuc] = true;
        }

        if (Bang_arrMaCot[c].startsWith("iID_MaTaiKhoan")) {
            var csMaTaiKhoanGiaiThich = "";
            var csTenTaiKhoanGiaiThich = "";
            if (Bang_arrMaCot[c] == "iID_MaTaiKhoan_No") {
                csMaTaiKhoanGiaiThich = Bang_arrCSMaCot["iID_MaTaiKhoanGiaiThich_No"];
                csTenTaiKhoanGiaiThich = Bang_arrCSMaCot["sTenTaiKhoanGiaiThich_No"];
            }
            if (Bang_arrMaCot[c] == "iID_MaTaiKhoan_Co") {
                csMaTaiKhoanGiaiThich = Bang_arrCSMaCot["iID_MaTaiKhoanGiaiThich_Co"];
                csTenTaiKhoanGiaiThich = Bang_arrCSMaCot["sTenTaiKhoanGiaiThich_Co"];
            }
            if (Bang_arrMaCot[c] == "iID_MaTaiKhoan_Thu") {
                csMaTaiKhoanGiaiThich = Bang_arrCSMaCot["iID_MaTaiKhoanGiaiThich_Thu"];
                csTenTaiKhoanGiaiThich = Bang_arrCSMaCot["sTenTaiKhoanGiaiThich_Thu"];
            }
            if (Bang_arrMaCot[c] == "iID_MaTaiKhoan_Chi") {
                csMaTaiKhoanGiaiThich = Bang_arrCSMaCot["iID_MaTaiKhoanGiaiThich_Chi"];
                csTenTaiKhoanGiaiThich = Bang_arrCSMaCot["sTenTaiKhoanGiaiThich_Chi"];
            }
            //Gán rỗng vào giải thích khi có thay đổi tài khoản
            Bang_GanGiaTriThatChoO(h, csMaTaiKhoanGiaiThich, "");
            Bang_GanGiaTriThatChoO(h, csTenTaiKhoanGiaiThich, "");
        }

        $.getJSON(url, function (item) {
            var csGan;
            if (Truong == "iID_MaMucLucNganSach") {
                Bang_GanGiaTriThatChoO(h, c, item.label);
                Bang_DienMucLucNganSach(h, c, item);
            }
            else{
                //Gán trường hiển thị
                if (item!=null) {
                    Bang_GanGiaTriThatChoO(h, c, item.label);
                    //Gán trường ẩn
                    csGan = Bang_arrCSMaCot[TruongGan];
                    Bang_GanGiaTriThatChoO(h, csGan, item.value);
                }
               
            }
            if (typeof fnCellAfterEdit == 'function') {
                return fnCellAfterEdit(h, c, item);
            }
        });
    }
    else {

        if (typeof fnCellAfterEdit == 'function') {
            return fnCellAfterEdit(h, c);
        }
    }
    return true;
}


var Bang_fnCellValueChanged_CoHam = false;
var Bang_fnCellValueChanged = null;
function Bang_onCellValueChanged(h, c, GiaTriCu) {
    //Đổi màu khi đồng ý, từ chối nếu có
    if (Bang_arrMaCot[c] == "bDongY" || Bang_arrMaCot[c] == "sLyDo") {
        var bDongY = Bang_LayGiaTri(h, "bDongY");
        var sLyDo = Bang_LayGiaTri(h, "sLyDo");
        var sMauSac = Bang_sMauSac_ChuaDuyet;
        if (bDongY == "1") {
            sMauSac = Bang_sMauSac_DongY;
        }
        else {
            if (sLyDo != "") {
                sMauSac = Bang_sMauSac_TuChoi;
            }
        }
        Bang_GanGiaTriThatChoO_colName(h, "sMauSac", sMauSac);
    }
    //Gọi hàm CellValueChanged
    if (Bang_fnCellValueChanged_CoHam == false) {
        Bang_fnCellValueChanged = window[Bang_ID + '_onCellValueChanged'];
        if (typeof Bang_fnCellValueChanged == 'function') {
            Bang_fnCellValueChanged_CoHam = true;
        }
    }
    if (Bang_fnCellValueChanged_CoHam) {
        Bang_fnCellValueChanged(h, c, GiaTriCu);
    }
}

function Bang_onCellKeyUp(h, c, e, iKey) {
    var fnKeyUp = window[Bang_ID + '_onCellKeyUp'];
    if (typeof fnKeyUp == 'function') {
        return fnKeyUp(h, c, e, iKey);
    }
    return true;
}

function Bang_onEnter_NotSetCellFocus() {
    var fn = window[Bang_ID + '_onEnter_NotSetCellFocus'];
    if (typeof fn == 'function') {
        return fn();
    }
    return true;
}

/* Hàm BangDuLieu_XacDinhCotHienThi
*   - Muc dinh: Xác định xem cột nào là hiển thị hay ẩn
*/
function Bang_XacDinhCotHienThi() {
//    var i, h, c, ok;
//    for (i = 0; i < Bang_arrDSTruongTien.length; i++) {
//        TenTruong = Bang_arrDSTruongTien[i];
//        c = Bang_arrCSMaCot[TenTruong];
//        if (c >= 0) {
//            ok = false;
//            for (h = 0; h < Bang_nC; h++) {
//                if (Bang_arrEdit[h][c]) {
//                    ok = true;
//                    break;
//                }
//            }
//            Bang_AnHienCot(c, ok);
//        }
//    }
}

Bang_fnScroll = function () {

    $('#' + Bang_ID_TB01_Div).scrollLeft($('#' + BangDuLieuID_Slide_Div).scrollLeft());
    //Hiệu chỉnh bảng cột tiêu đề theo bảng dữ liệu
    var yTop = $('#' + BangDuLieuID_Slide_Div).scrollTop();
    $('#' + Bang_ID_TB10_Div).scrollTop(yTop);
    Bang_SetPosition(yTop);
}
function ShowPopupThucHien() {
    $('#dvText').show();
    $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
    $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer 
}