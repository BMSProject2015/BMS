var jsKeToan_url_ChungTu = '';
var jsKeToan_url_ChungTuChiTiet = '';
var jsKeToan_url_NhanCongSan = '';
var jsKeToan_url_NhanQuyetToan = '';
var jsKeToan_url_NhanPhieuThuChi = '';
var jsKeToan_url_NhanUyNhiemChi = '';
var jsKeToan_url_NhanRutDuToan = '';
var jsKeToan_url_KTTH_THCTGS = '';
var jsKeToan_url_KTTH_CTGS = '';
var jsKeToan_url_KTTH_ThongTri = '';
var jsKeToan_url_KTTH_LICHSUCHUNGTU = '';
var jsKeToan_iID_MaChungTuChiTiet = '';
var BangDuLieu_iID_MaChungTu = "";
var jsKeToan_url_ChungTu_Check = '';
var jsKeToan_url_MaChungTu_Check = '';
var BangDuLieu_Url_CheckTrungSoGhiSo = "";
var jsKeToan_url_Tao_CTGS = '';
var jsKeToan_url_Tao_CTGS_Dialog = '';
var jsKeToan_url_TinhTong_Dialog = '';
//<<<<<<<<<<<<
//Các hàm từ Frame chứng từ gọi
var jsKeToan_Confirm_OK = false;
var jsKeToan_Confirm_Cancel = false;
var jsKeToan_CheckChungTuGhiSo = false;
var jsKeToan_Check_MaChungTu = false;
var sSoChungTuTimKiem = '';
var iID_MaTrangThai_TimKiem = '';
var iSoLuongChungTu = 0;
var jsNamLamViec = 2012;
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
                    //$("button:contains('Tiếp tục')").focus();                     
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
        //        $(":button:contains('Tiếp tục')").focus(); 
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
   //jsKiemTra_MaChungTu(iID_MaChungTu);
    if (DaXoa) {
        GoiHam_ChungTu_BangDuLieu_ChungTuChiTiet_Saved();
    }
    else {
        document.getElementById("divTongHop_sSoChungTu").value = GoiHam_ChungTu_Bang_LayTruong("sSoChungTu");
        document.getElementById("divTongHop_sSoChungTu_SoSanh").value = GoiHam_ChungTu_Bang_LayTruong("sSoChungTu");
        document.getElementById("divTongHop_iNgay").value = GoiHam_ChungTu_Bang_LayTruong("iNgay");
        if (GoiHam_ChungTu_Bang_LayTruong("iTapSo") != "0") {
            document.getElementById("divTongHop_iTapSo").value = GoiHam_ChungTu_Bang_LayTruong("iTapSo");
        }
        document.getElementById("divTongHop_sDonVi").value = GoiHam_ChungTu_Bang_LayTruong("sDonVi");
        document.getElementById("divTongHop_sNoiDung").value = GoiHam_ChungTu_Bang_LayTruong("sNoiDung");

        if (iAction == "") {
            if (iSoLuongChungTu > 1 || jsKeToan_Check_MaChungTu == true) {
                $("#divTongHop").dialog({
                    resizable: false,
                    width: 450,
                    height: 220,
                    modal: true,
                    title: 'Nội dung C.T.G.S',
                    buttons: [{
                        id: "btn_cancel",
                        name: "btn_cancel",
                        text: "Cancel",
                        style: "display:none;",
                        click: function () {
                            $(this).dialog("close");
                        }
                    },
                            {
                                id: "btn_submit",
                                text: "submit",
                                style: "display:none;",
                                click: function () {
                                    if (jsKeToan_CheckChungTuGhiSo) {
                                        $(this).dialog("close");
                                        jsKeToan_divTongHop_onOKClick();
                                    }
                                    else {
                                        alert("Số chứng từ ghi sổ đã tồn tại!");
                                        $(this).dialog("close");
                                    }
                                }
                            }]
                });
            }
            else {
                //                if (jsKeToan_CheckChungTuGhiSo) {
                jsKeToan_divTongHop_onOKClick();
                //                }
                //                else {
                //                    alert("Số chứng từ ghi sổ đã tồn tại!");
                //                }
            }
        }
        else {
            jsKeToan_divTongHop_onOKClick();
        }
        //$(":button:contains('Tiếp tục')").focus();
        //$("#btn_submit").setAttribute("tab-index","4");
    }
    return false;
}



//>>>>>>>>>>>>
function jsKiemTraChungSoChungTuGhiSo(iID_MaChungTu, sSoChungTu) {
    jQuery.ajaxSetup({ cache: false });
    var vR;
    var url = jsKeToan_url_ChungTu_Check;
    url += "?iID_MaChungTu=" + iID_MaChungTu + "&sSoChungTu=" + sSoChungTu;
    $.getJSON(url, function (data) {
        jsKeToan_CheckChungTuGhiSo = data;
    });
    //    $.getJSON(url, function (item) {
    //        jsKeToan_CheckChungTuGhiSo = item.Trung;
    //    });
}
function jsKiemTra_MaChungTu(iID_MaChungTu, DaXoa, iAction) {
    jQuery.ajaxSetup({ cache: false });
    var url = jsKeToan_url_MaChungTu_Check;
    url += "?iID_MaChungTu=" + iID_MaChungTu;
    $.getJSON(url, function (item) {
        jsKeToan_Check_MaChungTu = item.Trung;
        ChungTu_YeuCauLuuDuLieu(iID_MaChungTu, DaXoa, iAction);
        //alert(1);
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

function GoiHam_GanGiaTriTruong_iID_MaChungTu(GiaTri) {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.GanGiaTriTruong_iID_MaChungTu(GiaTri);
}
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
    Bang_ChiDoc = false;
    var sSoChungTu = document.getElementById("divTongHop_sSoChungTu").value;
    GoiHam_ChungTu_Bang_GanTruong("sSoChungTu", sSoChungTu);
    GoiHam_ChungTuChiTiet_ThayDoiTruong("sSoChungTu", sSoChungTu);

    // bỏ vì khi save trường iNgay bị gán lại giá trị ngày của chứng từ.
    var iNgay = document.getElementById("divTongHop_iNgay").value;
    GoiHam_ChungTu_Bang_GanTruong("iNgay", iNgay);
    //  GoiHam_ChungTuChiTiet_ThayDoiTruong("iNgay", iNgay);

    var iTapSo = document.getElementById("divTongHop_iTapSo").value;
    GoiHam_ChungTu_Bang_GanTruong("iTapSo", iTapSo);
    GoiHam_ChungTuChiTiet_ThayDoiTruong("iTapSo", iTapSo);

    var sDonVi = document.getElementById("divTongHop_sDonVi").value;
    GoiHam_ChungTu_Bang_GanTruong("sDonVi", sDonVi);
    GoiHam_ChungTuChiTiet_ThayDoiTruong("sDonVi", sDonVi);

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
    url += '?iID_MaChungTu=' + iID_MaChungTu;
    url += '&MaND=' + dll_getValue('ddlMaND');
    url += '&TrangThai=' + dll_getValue('ddlTrangThai');
    if (jsKeToan_iID_MaChungTuChiTiet != '') {
        url += '&iID_MaChungTuChiTiet=' + jsKeToan_iID_MaChungTuChiTiet;
        jsKeToan_iID_MaChungTuChiTiet = '';
    }
    document.getElementById('ifrChiTietChungTu').src = url;
    // document.getElementById("idKTTH_NHANCONGSAN").href = jsKeToan_url_NhanCongSan + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang;
    //document.getElementById("idKTTH_NHANQUYETTOAN").href = jsKeToan_url_NhanQuyetToan + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang;
    document.getElementById("idKTTH_NHANPHIEUTHUCHI").href = jsKeToan_url_NhanPhieuThuChi + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang;
    document.getElementById("idKTTH_NHANUYNHIEMCHI").href = jsKeToan_url_NhanUyNhiemChi + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang;
    document.getElementById("idKTTH_NHANRUTDUTOAN").href = jsKeToan_url_NhanRutDuToan + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang;
    document.getElementById("idKTTH_KTTH_THCTGS").href = jsKeToan_url_KTTH_THCTGS + '&iID_MaChungTu=' + iID_MaChungTu;
    document.getElementById("idKTTH_THCTGS").href = jsKeToan_url_KTTH_CTGS + '&iID_MaChungTu=' + iID_MaChungTu;
    document.getElementById("idKTTH_THONGTRI").href = jsKeToan_url_KTTH_ThongTri + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam;
    document.getElementById("idKTTH_LICHSUCHUNGTU").href = jsKeToan_url_KTTH_LICHSUCHUNGTU + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam;
    document.getElementById("idKTTH_TAO_CTGS").href = jsKeToan_url_Tao_CTGS + '&iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam;
    return true;
}

//<<<<<<<<<<<<
//Các hàm từ Frame chứng từ chi tiết gọi
function jsKeToan_LoadLaiChungTu(iID_MaChungTu, iThang) {
    var url = jsKeToan_url_ChungTu;
    var sSoChungTu = document.getElementById("txtSoChungTu").value;
    //Gán lại số chứng từ tìm kiếm 
    sSoChungTuTimKiem = sSoChungTu;
    var iID_MaTrangThaiDuyet = document.getElementById("CauHinh_iID_MaTrangThaiDuyet").value;
    iID_MaTrangThai_TimKiem = iID_MaTrangThaiDuyet;
    url += "?sSoChungTu=" + sSoChungTu;
    url += "&TimKiem=1";
    url += "&iID_MaTrangThaiDuyet=" + iID_MaTrangThaiDuyet;
    if (typeof iID_MaChungTu != "undefined") {
        url += "&iID_MaChungTu=" + iID_MaChungTu;
    }
    if (typeof iThang != "undefined") {
        url += "&iThang=" + iThang;
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
function jsKTTH_Dialog_Tao_CTGS_Show() {
    var iID_MaChungTu = GoiHam_ChungTu_BangDuLieu_LayMaChungTu();
    var iThang = document.getElementById("CauHinh_iThangLamViec").value;
    var iNam = document.getElementById("CauHinh_iNamLamViec").value;
    var url = jsKeToan_url_Tao_CTGS_Dialog + '?iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam;
    var $dialog = $('<div></div>')
        .html('<iframe style="border: 0px; " src="' + url + '" width="97%" height="98%"></iframe>')
        .dialog({
            autoOpen: false,
            modal: true,
            resizeable: false,
            draggable: false,
            width: 600, //'auto'
            title: "Tạo số ghi sổ từ số ghi sổ khác",
            open: function (event, ui) {
                $(event.target).parent().css('position', 'fixed');
                $(event.target).parent().css('top', '50px');
                // $(event.target).parent().css('left', '10px');
            },
            close: function (event, ui) {
                GoiHam_ChungTu_BangDuLieu_fnSetFocus(0, 0);
            }

        });
    $dialog.dialog('open');
    //e.preventDefault();
    // setTimeout("jsLuong_ThemMoiCanBo_Dialog_Open()", 100);

    //    var iID_MaChungTu = GoiHam_ChungTu_BangDuLieu_LayMaChungTu();
    //    var iThang = document.getElementById("CauHinh_iThangLamViec").value;
    //    var iNam = document.getElementById("CauHinh_iNamLamViec").value;
    //    $("#divTinhTong").dialog({
    //        resizable: false,
    //        width: 650,
    //        height: 220,
    //        modal: true,
    //        title: 'Tính tổng theo C.T.G.S'
    //    });
}
function jsKTTH_Dialog_TinhTong_Show() {
    var iID_MaChungTu = GoiHam_ChungTu_BangDuLieu_LayMaChungTu();
    var iThang = document.getElementById("CauHinh_iThangLamViec").value;
    var iNam = document.getElementById("CauHinh_iNamLamViec").value;
    var url = jsKeToan_url_TinhTong_Dialog + '?iID_MaChungTu=' + iID_MaChungTu + '&iThang=' + iThang + '&iNam=' + iNam;
    var $dialog = $('<div id="idDialog"></div>')
        .html('<iframe style="border: 0px; " src="' + url + '" width="100%" height="98%"></iframe>')
        .dialog({
            autoOpen: false,
            modal: true,
            resizeable: false,
            draggable: false,
            width: 600, //'auto'
            height: 450,
            title: "Tính tổng theo C.T.G.S",
            open: function (event, ui) {
                $(event.target).parent().css('position', 'fixed');
                $(event.target).parent().css('top', '50px');
                // $(event.target).parent().css('left', '10px');
            },
            close: function (event, ui) {
                //parent.BangDuLieu_onLoad();
                GoiHam_ChungTu_BangDuLieu_fnSetFocus(0, 0);
            }

        });
    $dialog.dialog('open');


    //        var iID_MaChungTu = GoiHam_ChungTu_BangDuLieu_LayMaChungTu();
    //        var iThang = document.getElementById("CauHinh_iThangLamViec").value;
    //        var iNam = document.getElementById("CauHinh_iNamLamViec").value;
    //        $("#divTinhTong").dialog({
    //            resizable: false,
    //            width: 650,
    //            height: 220,
    //            modal: true,
    //            title: 'Tính tổng theo C.T.G.S'
    //        });
}

function jsKTTH_Dialog_F12_Show(strNoidung, strLable) {
    var pSoChungTu = "Số: " + GoiHam_ChungTu_Bang_LayTruong("sSoChungTu") + " - " +
        GoiHam_ChungTu_Bang_LayTruong("sNoiDung") + " - Ngày: " +
            GoiHam_ChungTu_Bang_LayTruong("iNgay") + " Tháng " + document.getElementById("CauHinh_iThangLamViec").value;
    $("#divTinhTong").dialog({
        resizable: false,
        width: 650,
        height: 450,
        modal: true,
        title: 'Tính tổng theo C.T.G.S',
        open: function (event, ui) {
            document.getElementById("pMessNoiDung").innerHTML = pSoChungTu;
            document.getElementById("pMessData").innerHTML = strNoidung;
            document.getElementById("pMessChungTu").innerHTML = strLable;
        },
        close: function (event, ui) {

            GoiHam_ChungTu_BangDuLieu_fnSetFocus(0, 0);
        }
    });
}
