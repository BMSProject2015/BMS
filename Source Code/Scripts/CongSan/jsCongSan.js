//T,G,1:TGT,2:GGT
//0:thuộc loại nào,1:Đất,2:Nhà,3:Ô tô,4:Tài sản trên 500 triệu

var jsCongSan_url_ChungTu = '';
var jsCongSan_url_ChungTuChiTiet = '';
var jsCongSan_url_ChiTietTaiSan = '';
var jsCongSan_iID_MaChungTuChiTiet = '';


//<<<<<<<<<<<<
//Các hàm từ Frame chứng từ gọi
var jsCongSan_Confirm_OK = false;
var jsCongSan_Confirm_Cancel = false;
function ChungTu_KiemTraCapNhapChiTietChungTu(h, c) {
    if (GoiHam_ChungTuChiTiet_BangDuLieu_KiemTraCoThayDoi()) {
        $("#idConfirm").dialog({
            resizable: false,
            height: 200,
            modal: true,
            title: 'Lưu dữ liệu đã thay đổi',
            buttons: {
                "Hủy bỏ": function () {
                    jsCongSan_Confirm_Cancel = true;
                    $(this).dialog("close");
                },
                "Tiếp tục": function () {
                    jsCongSan_Confirm_OK = true;
                    $(this).dialog("close");
                }

            },
            close: function (event, ui) {
                if (jsCongSan_Confirm_OK) {
                    ChungTuChiTiet_onKeypress_F10(h, c);
                }
                if (jsCongSan_Confirm_Cancel) {
                    GoiHam_ChungTuChiTiet_BangDuLieu_HuyCoThayDoi();
                    GoiHam_ChungTu_BangDuLieu_fnSetFocus();
                }
                jsCongSan_Confirm_OK = false;
                jsCongSan_Confirm_Cancel = false;
            }
        });
        return false;
    }
    return true;
}

function ChungTu_ThayDoiMaChungTu() {
    dll_CanXoa = true;
    ddl_reset();
    dll_CanXoa = true;
    jsCongSan_Load_ChungTuChiTiet();
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
        if (iAction == "") {
            $("#divTongHop").dialog({
                resizable: false,
                width: 500,
                height: 240,
                modal: true,
                title: 'Thông tin hóa đơn',
                buttons: {
                    "Hủy bỏ": function () {
                        $(this).dialog("close");
                    },
                    "Tiếp tục": function () {
                        $(this).dialog("close");
                        jsCongSan_divTongHop_onOKClick();
                    }

                }
            });
        }
        else {
            jsCongSan_divTongHop_onOKClick();
        }
    }
    return false;
}
//>>>>>>>>>>>>

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

function ChungTuChiTiet_HienThi_TaiSanChiTiet() {
    var sTinhChat = GoiHam_ChungTuChiTiet_BangDuLieu_LayGiaTriTruong("sTinhChat");
    if (sTinhChat == "T") {
        var iLoaiTS = GoiHam_ChungTuChiTiet_BangDuLieu_LayGiaTriTruong("iLoaiTS");
        //0:thuộc loại nào,1:Đất,2:Nhà,3:Ô tô,4:Tài sản trên 500 triệu
        var dlgHeight = 500;
        if (iLoaiTS == 1) {
            dlgHeight = 500;
        }
        else if (iLoaiTS == 2) {
            dlgHeight = 500;
        }
        else if (iLoaiTS == 3) {
            dlgHeight = 500;
        }
        else if (iLoaiTS == 4) {
            dlgHeight = 450;
        }
        $("#divChiTietTaiSan").dialog({
            resizable: false,
            width: 800,
            height: dlgHeight,
            modal: true,
            title: 'Thông tin chi tiết tài sản',
            buttons: {
                "Hủy bỏ": function () {
                    $(this).dialog("close");
                },
                "Tiếp tục": function () {
                    GoiHam_jsCongSan_GanDuLieu();
                    $(this).dialog("close");
                }
            },
            close: function (event, ui) {
                GoiHam_ChungTuChiTiet_BangDuLieu_fnSetFocus();
            }
        });
        setTimeout("jsCongSan_HienThi_TaiSanChiTiet()", 100);
    }
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

function GoiHam_ChungTuChiTiet_BangDuLieu_LayGiaTriTruong(Truong) {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.BangDuLieu_LayGiaTriTruong(Truong);
}

function GoiHam_ChungTuChiTiet_BangDuLieu_LayGiaTriHienThiTruong(Truong) {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.BangDuLieu_LayGiaTriHienThiTruong(Truong);
}

function GoiHam_ChungTuChiTiet_BangDuLieu_GanGiaTriTruong(Truong, GiaTri) {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    y.BangDuLieu_GanGiaTriTruong(Truong, GiaTri);
}

function GoiHam_ChungTuChiTiet_BangDuLieu_Lay_arrMaCot() {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.BangDuLieu_Lay_arrMaCot();
}

//>>>>>>>>>>>>

//<<<<<<<<<<<<
//Các hàm gọi Frame chi tiết tài sản
function GoiHam_jsCongSan_GanDuLieu() {
    var x = document.getElementById("ifrChiTietTaiSan");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    y.jsCongSan_GanDuLieu();
}
//>>>>>>>>>>>>

function jsCongSan_divTongHop_onOKClick() {
    var sSoChungTu = document.getElementById("divTongHop_sSoChungTu").value;
    GoiHam_ChungTu_Bang_GanTruong("sSoChungTu", sSoChungTu);
    GoiHam_ChungTuChiTiet_ThayDoiTruong("sSoChungTu", sSoChungTu);

    var iNgay = document.getElementById("divTongHop_iNgay").value;
    GoiHam_ChungTu_Bang_GanTruong("iNgay", iNgay);
    GoiHam_ChungTuChiTiet_ThayDoiTruong("iNgay", iNgay);

//    var iTapSo = document.getElementById("divTongHop_iTapSo").value;
//    GoiHam_ChungTu_Bang_GanTruong("iTapSo", iTapSo);
//    GoiHam_ChungTuChiTiet_ThayDoiTruong("iTapSo", iTapSo);

//    var sDonVi = document.getElementById("divTongHop_sDonVi").value;
//    GoiHam_ChungTu_Bang_GanTruong("sDonVi", sDonVi);
//    GoiHam_ChungTuChiTiet_ThayDoiTruong("sDonVi", sDonVi);

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
        jsCongSan_Load_ChungTuChiTiet();
    }
}

function jsCongSan_Load_ChungTuChiTiet() {
    var iID_MaChungTu = GoiHam_ChungTu_BangDuLieu_LayMaChungTu();
    GetChungTuGhiSo(iID_MaChungTu);
    jQuery.ajaxSetup({ cache: false });
    var url = jsCongSan_url_ChungTuChiTiet;
    url += '?iID_MaChungTu=' + iID_MaChungTu;
    url += '&MaND=' + dll_getValue('ddlMaND');
    url += '&TrangThai=' + dll_getValue('ddlTrangThai');
    if (jsCongSan_iID_MaChungTuChiTiet != '') {
        url += '&iID_MaChungTuChiTiet=' + jsCongSan_iID_MaChungTuChiTiet;
        jsCongSan_iID_MaChungTuChiTiet = '';
    }
    document.getElementById('ifrChiTietChungTu').src = url;
    return true;
}

//<<<<<<<<<<<<
//Các hàm từ Frame chứng từ chi tiết gọi
function jsCongSan_LoadLaiChungTu(iID_MaChungTu) {
    var url = jsCongSan_url_ChungTu;
    var sSoChungTu = document.getElementById("txtSoChungTu").value;
    url += "?sSoChungTu=" + sSoChungTu;
    url += "&TimKiem=1";
    if (typeof iID_MaChungTu != "undefined") {
        url += "&iID_MaChungTu=" + iID_MaChungTu;
    }
    document.getElementById("ifrChungTu").src = url;
}
var jsCongSan_Search_inteval = null;

function jsCongSan_Search_onkeypress(e) {
    jsCongSan_Search_clearInterval();
    if (e.keyCode == 13) {
        jsCongSan_LoadLaiChungTu();
    }
    else {
        jsCongSan_Search_inteval = setInterval(function () { jsCongSan_Search_clearInterval(); jsCongSan_LoadLaiChungTu(); }, 2000);
    }
}

function jsCongSan_Search_clearInterval() {
    clearInterval(jsCongSan_Search_inteval);
}
//>>>>>>>>>>>>
//<<<<<<<<<<<<
//Các hàm từ Frame chứng từ chi tiết gọi
function jsCongSan_HienThi_TaiSanChiTiet() {
    var iLoaiTS = GoiHam_ChungTuChiTiet_BangDuLieu_LayGiaTriTruong("iLoaiTS");
    //0:thuộc loại nào,1:Đất,2:Nhà,3:Ô tô,4:Tài sản trên 500 triệu
    var url = jsCongSan_url_ChiTietTaiSan;
    url += "?iLoaiTS=" + iLoaiTS;
    document.getElementById("ifrChiTietTaiSan").src = url;
}
//>>>>>>>>>>>>