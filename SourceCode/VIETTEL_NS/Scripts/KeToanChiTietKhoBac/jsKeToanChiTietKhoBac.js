var jsKeToan_url_ChungTu = '';
var jsKeToan_url_ChungTuChiTiet = '';
var jsKeToan_url_KTCT_GIAYRUTTIEN = '';
var jsKeToan_url_KTCT_TTCHUYENTIEN = '';
var jsKeToan_url_KTCT_TTLOAI = '';
var jsKeToan_url_KTCT_TTTONGHOP = '';
var jsKeToan_url_KTCT_TTLOAINS = '';
var jsKeToan_url_KTTH_LICHSUCHUNGTU = '';
var jsKeToan_iID_MaChungTuChiTiet = '';
//<<<<<<<<<<<<
//Các hàm từ Frame chứng từ gọi
var jsKeToan_Confirm_OK = false;
var jsKeToan_Confirm_Cancel = false;
var jsKeToan_Check_MaChungTu = false;
var iSoLuongChungTu = 0;
var BangDuLieu_Url_CheckTrungSoGhiSo = "";
var sSoChungTuTimKiem = '';
var jsNamLamViec = 2012;
var jsThangLamViec = 0;
var jsLoai = 1;
function ChungTu_KiemTraCapNhapChiTietChungTu(h, c) {
    if (GoiHam_ChungTuChiTiet_BangDuLieu_KiemTraCoThayDoi()) {
        $("#idConfirm").dialog({
            resizable: false,
            height: 200,
            modal: true,
            title: 'Lưu dữ liệu đã thay đổi',
            buttons: {
                "Hủy bỏ": function () {
                    jsKeToan_Confirm_Cancel = true;
                    $(this).dialog("close");
                },
                "Tiếp tục": function () {
                    jsKeToan_Confirm_OK = true;
                    $(this).dialog("close");
                }

            },
            close: function (event, ui) {
                if (jsKeToan_Confirm_OK) {
                    ChungTuChiTiet_onKeypress_F10(h, c);
                }
                if (jsKeToan_Confirm_Cancel) {
                    GoiHam_ChungTuChiTiet_BangDuLieu_HuyCoThayDoi();
                    GoiHam_ChungTu_BangDuLieu_fnSetFocus();
                }
                jsKeToan_Confirm_OK = false;
                jsKeToan_Confirm_Cancel = false;
            }
        });
        //$(":button:contains('Tiếp tục')").focus(); 
        return false;
    }
    return true;
}

function ChungTu_ThayDoiMaChungTu() {
    dll_CanXoa = true;
    ddl_reset();
    dll_CanXoa = true;
    jsKeToan_Load_ChungTuChiTiet();
    GoiHam_ChungTu_BangDuLieu_fnSetFocus();
}

function ChungTu_ThayDoiNgay(iNgay) {
    GoiHam_ChungTuChiTiet_ThayDoiTruong('iNgay', iNgay);
}

function ChungTu_YeuCauLuuDuLieu(iID_MaChungTu, DaXoa, iAction) {
    if (DaXoa) {
        GoiHam_ChungTu_BangDuLieu_ChungTuChiTiet_Saved();
    }
    else {
        document.getElementById("divTongHop_sSoChungTu").value = GoiHam_ChungTu_Bang_LayTruong("sSoChungTu");
        document.getElementById("divTongHop_iNgay").value = GoiHam_ChungTu_Bang_LayTruong("iNgay");
        document.getElementById("divTongHop_sNoiDung").value = GoiHam_ChungTu_Bang_LayTruong("sNoiDung");

        //Kiểm tra trùng số chứng từ trước khi lưu
        //
        var Trung_CoLuu = false;
        var KhongTrung = true;
        var sSoChungTu = GoiHam_ChungTu_Bang_LayTruong("sSoChungTu");
        jQuery.ajaxSetup({ cache: false });
        var vR;
        var sSoChungTuMoi = "";
        var url = BangDuLieu_Url_CheckTrungSoGhiSo;
        url += "?iID_MaChungTu=" + iID_MaChungTu + "&sSoChungTu=" + sSoChungTu;
        $.getJSON(url, function (item) {
            if (item.Trung) {
                KhongTrung = false;
                var r = confirm("Số ghi sổ đã có. Bạn có muốn lưu vào số đã có không?");
                if (r) {
                    GoiHam_ChungTuChiTiet_ThayDoiTruong("iID_MaChungTu", item.iID_MaChungTu);
                    Trung_CoLuu = true;
                    show_dialog(iAction);
                }
            }
            else {
                show_dialog(iAction);
            }
        });


    }

    return false;
}

function show_dialog(iAction) {
    if (iAction == "") {
        if (iSoLuongChungTu > 1 || jsKeToan_Check_MaChungTu == true) {
            $("#divTongHop").dialog({
                resizable: false,
                width: 500,
                height: 240,
                modal: true,
                title: 'Thông tin chứng từ',
                buttons: [{
                    id: "btn_cancel",
                    style: "display:none;",
                    text: "Cancel",
                    click: function () {
                        $(this).dialog("close");
                    }
                },
                {
                    id: "btn_submit",
                    style: "display:none;",
                    text: "submit",
                    click: function () {
                        $(this).dialog("close");
                        jsKeToan_divTongHop_onOKClick();
                    }

                }]
            });
        }
        else {

            jsKeToan_divTongHop_onOKClick();

        }
    }
    else {
        jsKeToan_divTongHop_onOKClick();
    }
    //$(":button:contains('Tiếp tục')").focus(); 
}
//>>>>>>>>>>>>
function jsKiemTra_MaChungTu(iID_MaChungTu) {
    jQuery.ajaxSetup({ cache: false });
    var url = jsKeToan_url_MaChungTu_Check;
    url += "?iID_MaChungTu=" + iID_MaChungTu;
    $.getJSON(url, function (item) {
        jsKeToan_Check_MaChungTu = item.Trung;
    });
}
//<<<<<<<<<<<<
//Các hàm từ Frame chứng từ chi tiết gọi
function ChungTuChiTiet_ThayDoiTruongNoiDung(sNoiDung) {
    GoiHam_ChungTu_ThayDoiTruongNoiDung(sNoiDung);
}

function ChungTuChiTiet_Saved() {
    Bang_HideCloseDialog();
    GoiHam_ChungTu_BangDuLieu_ChungTuChiTiet_Saved();
    return false;
}

function ChungTuChiTiet_onKeypress_F10() {
    GoiHam_ChungTu_BangDuLieu_onKeypress_F10();
}
//>>>>>>>>>>>>

//<<<<<<<<<<<<
//Các hàm gọi Frame chứng từ
function GoiHam_ChungTu_BangDuLieu_ChungTuChiTiet_Saved() {
    var x = document.getElementById("ifrChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.BangDuLieu_ChungTuChiTiet_Saved();
}

function GoiHam_ChungTu_BangDuLieu_onKeypress_F10() {
    var x = document.getElementById("ifrChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.BangDuLieu_onKeypress_F10();
}

function GoiHam_ChungTu_BangDuLieu_LayMaChungTu() {
    var x = document.getElementById("ifrChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.BangDuLieu_LayMaChungTu();
}

function GoiHam_ChungTu_BangDuLieu_fnSetFocus() {
    GoiHam_ChungTuChiTiet_BangDuLieu_fnSetBlur();

    var x = document.getElementById("ifrChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    x.focus();
    z.body.focus();
    y.BangDuLieu_onBodyFocus();
}

function GoiHam_ChungTu_BangDuLieu_fnSetBlur() {
    var x = document.getElementById("ifrChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    y.BangDuLieu_onBodyBlur();
}

function GoiHam_ChungTu_Bang_GanTruong(TenTruong, GiaTri) {
    var x = document.getElementById("ifrChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    y.BangDuLieu_GanTruong(TenTruong, GiaTri);
}

function GoiHam_ChungTu_ThayDoiTruongNoiDung(GiaTri) {
    var x = document.getElementById("ifrChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    y.ChungTuChiTiet_ThayDoiTruongNoiDung(GiaTri);
}


function GoiHam_ChungTu_Bang_LayTruong(TenTruong) {
    var x = document.getElementById("ifrChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.BangDuLieu_LayTruong(TenTruong);
}

function GoiHam_ChungTu_Bang_LayTruong(TenTruong) {
    var x = document.getElementById("ifrChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.BangDuLieu_LayTruong(TenTruong);
}
//>>>>>>>>>>>>

//<<<<<<<<<<<<
//Các hàm gọi Frame chứng từ chi tiết
function GoiHam_ChungTuChiTiet_BangDuLieu_fnSetFocus() {
    GoiHam_ChungTu_BangDuLieu_fnSetBlur();

    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    x.focus();
    z.body.focus();
    y.BangDuLieu_onBodyFocus();
}

function GoiHam_ChungTuChiTiet_BangDuLieu_fnSetBlur() {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    try {
        y.BangDuLieu_onBodyBlur();
    }
    catch (e) {
    }
}

function GoiHam_ChungTuChiTiet_ThayDoiTruong(Truong, GiaTri) {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    try {
        y.ChungTuChiTiet_ThayDoiTruongChungTu(Truong, GiaTri);
    }
    catch (e) {
    }
}

function GoiHam_ChungTuChiTiet_BangDuLieu_KiemTraCoThayDoi() {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    var okCoThayDoi = false;
    try {
        okCoThayDoi = y.ChungTuChiTiet_KiemTraCoThayDoi();
    }
    catch (ex) {
    }
    return okCoThayDoi;
}

function GoiHam_ChungTuChiTiet_BangDuLieu_HuyCoThayDoi() {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    y.ChungTuChiTiet_HuyCoThayDoi();
}

function GoiHam_ChungTuChiTiet_BangDuLieu_Save() {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    y.BangDuLieu_save();
}

function GoiHam_ChungTuChiTiet_BangDuLieu_CheckAll(checked) {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    y.BangDuLieu_CheckAll(checked);
}
//>>>>>>>>>>>>


function jsKeToan_divTongHop_onOKClick() {
    var sSoChungTu = document.getElementById("divTongHop_sSoChungTu").value;
    GoiHam_ChungTu_Bang_GanTruong("sSoChungTu", sSoChungTu);
    GoiHam_ChungTuChiTiet_ThayDoiTruong("sSoChungTu", sSoChungTu);

    var iNgay = document.getElementById("divTongHop_iNgay").value;
    GoiHam_ChungTu_Bang_GanTruong("iNgay", iNgay);
    GoiHam_ChungTuChiTiet_ThayDoiTruong("iNgay", iNgay);

    var sNoiDung = document.getElementById("divTongHop_sNoiDung").value;
    GoiHam_ChungTu_Bang_GanTruong("sNoiDung", sNoiDung);
    GoiHam_ChungTuChiTiet_ThayDoiTruong("sNoiDung", sNoiDung);

    Bang_ShowCloseDialog();

    GoiHam_ChungTuChiTiet_BangDuLieu_Save();
}

//Các hàm của trang kế toán

var dll_CanXoa = true;
var ddlBlock = false;
function ddl_reset(strDS) {
    ddlBlock = true;
    var ddl = document.getElementById('ddlMaND');
    if (ddl && dll_CanXoa) {
        while (ddl.options.length > 1) {
            ddl.remove(1);
        }
        if (typeof strDS == 'undefined') {
            strDS = '';
        }
        if (strDS != '') {
            var arr = strDS.split('#|');
            for (var i = 1; i < arr.length; i++) {
                if (arr[i] != '') {
                    var arr1 = arr[i].split('##');
                    var opt = new Option(arr1[0], arr1[1]);
                    ddl.options.add(opt);
                }
            }
        }
        ddl.selectedIndex = 0;
    }
    dll_CanXoa = false;
    ddlBlock = false;
}

function dll_getValue(ddlID) {
    var ddl = document.getElementById(ddlID);
    if (ddl) {
        return ddl.options[ddl.selectedIndex].value;
    }
    return "";
}

function ddl_onChange(ddlID) {
    if (ddlBlock == false) {
        jsKeToan_Load_ChungTuChiTiet();
    }
}

function jsKeToan_Load_ChungTuChiTiet() {
    var iID_MaChungTu = GoiHam_ChungTu_BangDuLieu_LayMaChungTu();
    var iThang = document.getElementById("CauHinh_iThangLamViec").value;
    var iNam = document.getElementById("CauHinh_iNamLamViec").value;
    GetChungTuGhiSo(iID_MaChungTu);
    jQuery.ajaxSetup({ cache: false });
    var url = jsKeToan_url_ChungTuChiTiet;
    var iLoai = document.getElementById("txtLoai").value;
    url += "?iLoai=" + iLoai;
    url += '&iID_MaChungTu=' + iID_MaChungTu;
    url += '&MaND=' + dll_getValue('ddlMaND');
    url += '&TrangThai=' + dll_getValue('ddlTrangThai');
    if (jsKeToan_iID_MaChungTuChiTiet != '') {
        url += '&iID_MaChungTuChiTiet=' + jsKeToan_iID_MaChungTuChiTiet;
        jsKeToan_iID_MaChungTuChiTiet = '';
    }
    document.getElementById('ifrChiTietChungTu').src = url;
    document.getElementById("idKTCT_GIAYRUTTIEN").href = jsKeToan_url_KTCT_GIAYRUTTIEN + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam + '&iLoai=' + iLoai;
    document.getElementById("divGiayRutTien").setAttribute('onclick', "OnInit('250','Giấy rút tiền','" + iID_MaChungTu + "','" + iNam + "')");
    document.getElementById("idKTCT_TTCHUYENTIEN").href = jsKeToan_url_KTCT_TTCHUYENTIEN + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam;
    document.getElementById("idKTCT_TTLOAI").href = jsKeToan_url_KTCT_TTLOAI + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam;
    document.getElementById("idKTCT_TTLOAINS").href = jsKeToan_url_KTCT_TTLOAINS + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam;
    document.getElementById("idKTCT_TTTONGHOP").href = jsKeToan_url_KTCT_TTTONGHOP + '?iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam;
    document.getElementById("idKTTH_LICHSUCHUNGTU").href = jsKeToan_url_KTTH_LICHSUCHUNGTU + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam;





    return true;
}

//<<<<<<<<<<<<
//Các hàm từ Frame chứng từ chi tiết gọi
function jsKeToan_LoadLaiChungTu(iID_MaChungTu) {
    var url = jsKeToan_url_ChungTu;
    var iLoai = document.getElementById("txtLoai").value;
    var sSoChungTu = document.getElementById("txtSoChungTu").value;
    //Gán lại số chứng từ tìm kiếm 
    sSoChungTuTimKiem = sSoChungTu;
    var iID_MaTrangThaiDuyet = document.getElementById("CauHinh_iID_MaTrangThaiDuyet").value;
    url += "?iLoai=" + iLoai;
    url += "&sSoChungTu=" + sSoChungTu;
    url += "&TimKiem=1";
    url += "&iID_MaTrangThaiDuyet=" + iID_MaTrangThaiDuyet;
    if (typeof iID_MaChungTu != "undefined") {
        url += "&iID_MaChungTu=" + iID_MaChungTu;
    }
    document.getElementById("ifrChungTu").src = url;
}
var jsKeToan_Search_inteval = null;

function jsKeToan_Search_onkeypress(e) {
    jsKeToan_Search_clearInterval();
    if (e.keyCode == 13) {
        jsKeToan_LoadLaiChungTu();
    }
    else {
        jsKeToan_Search_inteval = setInterval(function () { jsKeToan_Search_clearInterval(); jsKeToan_LoadLaiChungTu(); }, 2000);
    }
}

function jsKeToan_Search_clearInterval() {
    clearInterval(jsKeToan_Search_inteval);
}
//>>>>>>>>>>>>