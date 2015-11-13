var jsLuong_Url = '';
var jsLuong_Url_ThemMoiCanBo = '';
var jsLuong_iID_MaBangLuong = '';
var jsLuong_LoadLaiDuLieu = false;
var jsLuong_Url_TrichLuong = '';
var jsLuong_Url_DieuChinhTienAn = '';
var jsLuong_Url_HeSoKhuVuc = '';
var jsLuong_Url_HuyTapThe = '';
var jsLuong_Url_NguoiPhuThuoc = '';
var jsLuong_Url_NopThueDauVao = '';
var jsLuong_Url_ThuLaoThuong = '';

var jsLuong_URL_ChiTiet = '';
var jsLuong_URL_BaoHiem = '';
var jsLuong_URL_ThueTNCN = '';

function jsLuong_ThayDoiUrl(url) {

    if (typeof url == "undefined") {
        var x = document.getElementById("ifrChiTiet");
        var y = (x.contentWindow || x.contentDocument);
        var z = (y.document) ? y.document : y;
        var h = y.Bang_keys.Row();
        var c = y.Bang_keys.Col() - y.Bang_nC_Fixed;
        url = jsLuong_Url + "&csH=" + h + "&csC=" + c;
    }
    else {
        jsLuong_Url = url;

    }
    document.getElementById('ifrChiTiet').src = url;
    //lay ten thao tac
    var CauHinh_iID_MaChungTu = document.getElementById('CauHinh_iID_MaChungTu');
    if (url == jsLuong_URL_ChiTiet) {
        CauHinh_iID_MaChungTu.value = "NHẬP BẢNG LƯƠNG";
    }
    else if (url == jsLuong_URL_BaoHiem) {
        CauHinh_iID_MaChungTu.value = "NHẬP BẢO HIỂM";
    }
    else if (url == jsLuong_URL_ThueTNCN) {
        CauHinh_iID_MaChungTu.value = "NHẬP THUẾ TNCN";
    } else {
        CauHinh_iID_MaChungTu.value = "NHẬP BẢNG LƯƠNG";
    }
}

function jsLuong_Dialog_Close(LoadLaiDuLieu) {
    if (typeof LoadLaiDuLieu != "undefined") {
        jsLuong_LoadLaiDuLieu = LoadLaiDuLieu;
    }
    $('#divLuong_BangLuongChiTiet').dialog('close');
}

function jsLuong_Dialog_Closing() {
    jsLuong_BangDuLieu_fnSetFocus();
    if (jsLuong_LoadLaiDuLieu) {
        jsLuong_ThayDoiUrl();
    }
    jsLuong_LoadLaiDuLieu = false;
    //document.getElementById('BangLuongChiTiet_iFrame').src = urlServerPath + "Home/Blank";
    //jsLuong_BangLuongChiTiet_iFrame_ShowCloseDialog();
}

//function jsLuong_BangLuongChiTiet_iFrame_ShowCloseDialog() {
//    try {
//        var x = document.getElementById("BangLuongChiTiet_iFrame");
//        var y = (x.contentWindow || x.contentDocument);
//        var z = (y.document) ? y.document : y;
//        y.Bang_ShowCloseDialog();
//    }
//    catch (e) {
//    }
//}

function jsLuong_BangDuLieu_fnSetFocus() {
    try {
        var x = document.getElementById("ifrChiTiet");
        var y = (x.contentWindow || x.contentDocument);
        var z = (y.document) ? y.document : y;
        x.focus();
        z.body.focus();
    }
    catch (e) {
    }
}

//Thêm mới cán bộ
function jsLuong_ThemMoiCanBo_Dialog_Open() {
    var url = jsLuong_Url_ThemMoiCanBo;
    document.getElementById("BangLuongChiTiet_iFrame").src = url;
}

function jsLuong_Dialog_ThemMoiCanBo_Show() {
    $("#divLuong_BangLuongChiTiet").dialog({
        width: 900,
        height: 560,
        top: 10,
        modal: true,
        resizable: false,
        draggable: false,
        title: 'Cập nhật số liệu lương (Phím F10: Ghi; ESC: Thoát)',
        close: jsLuong_Dialog_Closing
    });
    setTimeout("jsLuong_ThemMoiCanBo_Dialog_Open()", 100);
}

function jsLuong_SuaCanBo_Dialog_Open(iID_MaBangLuongChiTiet) {
    var url = jsLuong_Url_ThemMoiCanBo;
    document.getElementById("BangLuongChiTiet_iFrame").src = url + "&iID_MaBangLuongChiTiet=" + iID_MaBangLuongChiTiet;
}

function jsLuong_Dialog_SuaCanBo_Show(iID_MaBangLuongChiTiet) {
    $("#divLuong_BangLuongChiTiet").dialog({
        width: 900,
        height: 560,
        modal: true,
        resizable: false,
        draggable: false,
        title: 'Cập nhật số liệu lương (Phím F10: Ghi; ESC: Thoát)',
        close: jsLuong_Dialog_Closing
    });
    setTimeout("jsLuong_SuaCanBo_Dialog_Open('" + iID_MaBangLuongChiTiet + "')", 100);
}
//End

//Trích lương
function jsLuong_TrichLuong_Dialog_Open() {
    var url = jsLuong_Url_TrichLuong;
    document.getElementById("BangLuongChiTiet_iFrame").src = url;
}

function jsLuong_Dialog_TrichLuong_Show() {
    $("#divLuong_BangLuongChiTiet").dialog({
        width: 400,
        height: 250,
        modal: true,
        title: 'Trích lương',
        resizable: false,
        draggable: false,
        close: jsLuong_Dialog_Closing
    });
    setTimeout("jsLuong_TrichLuong_Dialog_Open()", 100);
}
//End trich lương

//Điều chỉnh tiền ăn
function jsLuong_DieuChinhTienAn_Dialog_Open() {
    var url = jsLuong_Url_DieuChinhTienAn;
    document.getElementById("BangLuongChiTiet_iFrame").src = url;
}

function jsLuong_Dialog_DieuChinhTienAn_Show() {
    $("#divLuong_BangLuongChiTiet").dialog({
        width: 400,
        height: 250,
        modal: true,
        title: 'Điều chỉnh ăn tập thể',
        resizable: false,
        draggable: false,
        close: jsLuong_Dialog_Closing
    });
    setTimeout("jsLuong_DieuChinhTienAn_Dialog_Open()", 100);
}
//End Điều chỉnh tiền ăn

//Điều chỉnh hệ số khu vực
function jsLuong_HeSoKhuVuc_Dialog_Open() {
    var url = jsLuong_Url_HeSoKhuVuc;
    document.getElementById("BangLuongChiTiet_iFrame").src = url;
}

function jsLuong_Dialog_HeSoKhuVuc_Show() {
    $("#divLuong_BangLuongChiTiet").dialog({
        width: 400,
        height: 250,
        modal: true,
        title: 'Điều chỉnh tập thể',
        resizable: false,
        draggable: false,
        close: jsLuong_Dialog_Closing
    });
    setTimeout("jsLuong_HeSoKhuVuc_Dialog_Open()", 100);
}
//End Điều chỉnh hệ số khu vực

//Hủy tập thể
function jsLuong_HuyTapThe_Dialog_Open() {
    var url = jsLuong_Url_HuyTapThe;
    document.getElementById("BangLuongChiTiet_iFrame").src = url;
}

function jsLuong_Dialog_HuyTapThe_Show() {
    $("#divLuong_BangLuongChiTiet").dialog({
        width: 400,
        height: 250,
        modal: true,
        title: 'Hủy tập thể',
        resizable: false,
        draggable: false,
        close: jsLuong_Dialog_Closing
    });
    setTimeout("jsLuong_HuyTapThe_Dialog_Open()", 100);
}
//End Hủy tập thể

//Số người phụ thuộc
function jsLuong_NguoiPhuThuoc_Dialog_Open() {
    var url = jsLuong_Url_NguoiPhuThuoc;
    document.getElementById("BangLuongChiTiet_iFrame").src = url;
}

function jsLuong_Dialog_NguoiPhuThuoc_Show() {
    $("#divLuong_BangLuongChiTiet").dialog({
        width: 1000,
        height: 700,
        modal: true,
        title: 'Danh sách người phụ thuộc',
        resizable: false,
        draggable: false,
        close: jsLuong_Dialog_Closing
    });
    setTimeout("jsLuong_NguoiPhuThuoc_Dialog_Open()", 100);
}
//End Số người phụ thuộc

//Nội dung nộp thuế đầu vào
function jsLuong_NopThueDauVao_Dialog_Open() {
    var url = jsLuong_Url_NopThueDauVao;
    document.getElementById("BangLuongChiTiet_iFrame").src = url;
}

function jsLuong_Dialog_NopThueDauVao_Show() {
    $("#divLuong_BangLuongChiTiet").dialog({
        width: 1000,
        height: 700,
        modal: true,
        title: 'Nội dung nộp thuế đầu vào',
        resizable: false,
        draggable: false,
        close: jsLuong_Dialog_Closing
    });
    setTimeout("jsLuong_NopThueDauVao_Dialog_Open()", 100);
}
//End Nội dung nộp thuế đầu vào


//Thù lao thưởng
function jsLuong_ThuLaoThuong_Dialog_Open() {
    var url = jsLuong_Url_ThuLaoThuong;
    document.getElementById("BangLuongChiTiet_iFrame").src = url;
}

function jsLuong_Dialog_ThuLaoThuong_Show() {
    $("#divLuong_BangLuongChiTiet").dialog({
        width: 1000,
        height: 700,
        modal: true,
        title: 'Nội dung thù lao - thưởng',
        resizable: false,
        draggable: false,
        close: jsLuong_Dialog_Closing
    });
    setTimeout("jsLuong_ThuLaoThuong_Dialog_Open()", 100);
}
//End Thù lao thưởng


