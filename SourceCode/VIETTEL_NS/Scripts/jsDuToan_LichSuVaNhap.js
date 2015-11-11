var jsLichSuVaNhap_UrlSave = '';
var jsLichSuVaNhap_Url_getGiaTri_public = '';
var jsLichSuVaNhap_Url_getGiaTri = '';
var jsLichSuVaNhap_nH = 10;
var jsLichSuVaNhap_bDangThem = false;

function jsDuToan_LichDuVaNhap_onWindowResize(w) {
    $("#" + strParentID + "_div2").css('width', "1px");
      DuToan_LichSuVaNhap_fnAdjustTable(strParentID + '_FirstCol', strParentID + '_div2', strParentID + '_firstTR');
 
}

function DuToan_LichSuVaNhap_fnAdjustTable(FirstTableCol_ClassName, SecondTableDiv_Id, SecondTableFirstTR_Id) {
    var colCount = $('#' + SecondTableFirstTR_Id + '>td').length; //get total number of column
    var m = 0;
    var n = 0;
    var brow = 'mozilla';

    jQuery.each(jQuery.browser, function (i, val) {
        if (val == true) {
            brow = i.toString();
        }
    });

    var w = fnGetWidthById(strParentID + "_tieude2");
    $("#" + strParentID + "_div2").css('width', w + "px");
    
}

//function to support scrolling of title and first column
fnScroll = function (FirstTableDiv_Id, SecondTableDiv_Id) {
    $('#' + FirstTableDiv_Id).scrollTop($('#' + SecondTableDiv_Id).scrollTop());
}

function func_Auto_Complete(id, ui) {
    if (id == "iID_MaDonVi") {
        $('#' + strParentID + '_iID_MaDonVi1').val(ui.item.label);
    }
}

//Lưu lại các mã chi tiết mục lục ngân sách
function LuuGiaTriHienTai() {
    var arr = (strDSTruong+',sTenCongTrinh').split(',');
    var i, ctlID;
    for (i = 0; i < arr.length; i++) {
        if (arr[i] != "sMoTa") {
            ctlID = strParentID + '_' + arr[i];
            if (document.getElementById(ctlID + "1")) {
                arrGiaTriCu[ctlID] = document.getElementById(ctlID + "1").value;
            }
            else {
                arrGiaTriCu[ctlID] = document.getElementById(ctlID).value;
            }
        }
    }
}

//Hiển thị hoặc ẩn cột tiền
function show_hide_column(col_no, do_show) {
    var stl;
    if (do_show) stl = '';
    else stl = 'none';
    //var cells = $('[col-index="' + col_no + '"]');
    //for (var row = 0; row < cells.length; row++) {
    //    cells[row].style.display = stl;
    //}
    $('[col-index="' + col_no + '"]').css('display', stl);
}

//Cập nhật ẩn hoặc hiển thị trường tiền
function CapNhapHienThiCot() {
    var dTruongTien = 0;
    for (var i = 0; i < arrDSTruongTien.length; i++) {
        if (arrDSHienThiTruongTien[i] == "1" || arrDSDuocNhapTruongTien[i] == "1") {
            show_hide_column(i, true);
            dTruongTien++;
        }
        else {
            show_hide_column(i, false);
        }
        var okCoControl = false;
        var ctlID = strParentID + '_' + arrDSTruongTien[i];
        if (document.getElementById(ctlID + '_show')) {
            ctlID = ctlID + '_show';
            okCoControl = true;
        } else if (document.getElementById(ctlID)) {
            okCoControl = true;
        }
        if (okCoControl) {
            $('#' + ctlID).attr('khong-nhap', (arrDSDuocNhapTruongTien[i] == '1') ? '0' : '1');
            $('#' + ctlID).css('display', (arrDSDuocNhapTruongTien[i] == '1') ? '' : 'none');
        }
    }
    var DoRongBangTien = DoRongCotTiet * (dTruongTien + 1);
    $("#" + strParentID + "_table2").css('width', DoRongBangTien);
}

//Điền thông tin vào các mã chi tiết mục lục ngân sách
function func_DienMucLucNganSach(item) {
    var i, ctlID, TenTruong, GiaTri;
    var okDaQuaCot = false;

    if (item.CoChiTiet == "1") {
        //Có 1 mục lục ngân sách gần với mã chi tiết nhập vào
        //Gán lại tất cả các giá trị đã có
        var ThongTinThem = item.ThongTinThem;
        if (ThongTinThem != undefined) {
            var arr = ThongTinThem.split("#|");
            for (i = 0; i < arr.length; i++) {
                var arr1 = arr[i].split("##");
                TenTruong = arr1[0];
                GiaTri = arr1[1];
                ctlID = strParentID + '_' + arr1[0];
                if (TenTruong.startsWith('s')) {
                    if (document.getElementById(ctlID)) {
                        document.getElementById(ctlID).value = arr1[1];
                    }
                    if (document.getElementById(ctlID + '1')) {
                        document.getElementById(ctlID + '1').value = arr1[1];
                    }
                }
                else if (TenTruong.startsWith('b')) {
                    for (var j = 0; j < arrDSTruongTien.length; j++) {
                        if (arrDSTruongTien[j] == TenTruong.substring(1)) {
                            arrDSDuocNhapTruongTien[j] = GiaTri;
                            break;
                        }
                    }
                }
            }
        }
        if (LSN_MaCotNganSach_Dien != "sTNG") {
            jsControl_Next(LSN_MaCotNganSach);
        }
    }
    else {
        var arrMucLuc = strDSTruong.split(",");
        for (i = 1; i < arrMucLuc.length; i++) {
            TenTruong = arrMucLuc[i];
            if (okDaQuaCot) {
                //Những ô sau ô 'c' sẽ được gán giá trị ""
                ctlID = strParentID + '_' + TenTruong;
                if (document.getElementById(ctlID)) {
                    document.getElementById(ctlID).value = "";
                }
                if (document.getElementById(ctlID + '1')) {
                    document.getElementById(ctlID + '1').value = "";
                }
            }
            if (LSN_MaCotNganSach_Dien == TenTruong) {
                okDaQuaCot = true;
            }
        }
        for (i = 0; i < arrDSDuocNhapTruongTien.length; i++) {
            arrDSDuocNhapTruongTien[i] = "1";
        }
    }
    LuuGiaTriHienTai();
    CapNhapHienThiCot();
}

function CapNhapLaiDuToanNamTruoc() {
    var TruongURL = "rTongSoNamTruoc";
    var DSGiaTri = document.getElementById(strParentID + '_iID_MaDonVi').value + "," + funcGhepMa("");
    var GT = DuToan_LichSuVaNhap_iID_MaChungTu;
    var url = unescape(jsLichSuVaNhap_Url_getGiaTri + '?Truong=' + TruongURL + '&GiaTri=' + GT + '&DSGiaTri=' + DSGiaTri);
    document.getElementById(strParentID + '_rTongSoNamTruoc').value = 0;
    document.getElementById(strParentID + '_rTongSoNamTruoc_show').value = '';
    document.getElementById(strParentID + '_rUocThucHien').value = 0;
    document.getElementById(strParentID + '_rUocThucHien_show').value = '';
    $.getJSON(url, function (data) {
        document.getElementById(strParentID + '_rTongSoNamTruoc').value = data.value;
        document.getElementById(strParentID + '_rTongSoNamTruoc_show').value = FormatNumber(parseFloat(data.value));
    });
}

//function Control_onFocus(id, lastID) {
//    if (lastID && id != lastID && lastID != '') {
//        var TenTruong = $('#' + lastID).attr('ten-truong');
//        if (TenTruong && TenTruong != '') {
//            $("#" + lastID).autocomplete("close");
//        }
//    }
//}

//Hàm blur khi nhập mã chi tiết mục lục ngân sách
function func_Auto_Complete_onblur(id, Truong) {
    //if (id != jsInit_ControlID_Focus) {
        var ctlID = strParentID + '_' + Truong;
        var ctlID_HienThi = ctlID + "1";
        var CoThayDoi = false;
        if (Truong == "sMaCongTrinh") {
            ctlID_HienThi = strParentID + "_sTenCongTrinh";
        }

        if (arrGiaTriCu[ctlID] != document.getElementById(ctlID_HienThi).value) {
            CoThayDoi = true;
        }
        if (CoThayDoi) {
            //Trong trường hợp giá trị nhập vào khác rỗng và khác giá trị cũ thì sẽ được tìm giá trị mới và nhập vào

            var DSGiaTri = "";
            var GT = document.getElementById(ctlID_HienThi).value.split('-')[0];
            GT = $.trim(GT);

            arrGiaTriCu[ctlID] = GT;
            document.getElementById(ctlID).value = GT;


            if (funcCoMaMucLucNganSach() == false) {
                //Trường hợp không có nhập mã chi tiết mục lục ngân sách thì không cho phép nhập trường nào
                var i;
                for (i = 0; i < arrDSDuocNhapTruongTien.length; i++) {
                    arrDSDuocNhapTruongTien[i] = "0";
                }
                CapNhapHienThiCot();
            }

            //Trường hợp có nhập mã chi tiết mục lục ngân sách
            var TruongURL = '';
            if (Truong == "iID_MaDonVi") {
                TruongURL = "sTenDonViCoTen";
            }
            else if (Truong == "sMaCongTrinh") {
                TruongURL = "sTenCongTrinh";
                DSGiaTri = funcLayMaDonVi(Truong);
            }
            else {
                TruongURL = "iID_MaMucLucNganSach";
                DSGiaTri = funcGhepMa(Truong);
            }
            var url = unescape(jsLichSuVaNhap_Url_getGiaTri_public + '?Truong=' + TruongURL + '&GiaTri=' + GT + '&DSGiaTri=' + DSGiaTri);
            LSN_MaCotNganSach_Dien = Truong;
            $.getJSON(url, function (data) {
                document.getElementById(ctlID).value = data.value;
                document.getElementById(ctlID_HienThi).value = data.label;
                arrGiaTriCu[ctlID] = data.value;
                if (Truong == "iID_MaDonVi") {
                    CapNhapLaiDuToanNamTruoc();
                }
                else if (Truong == "sMaCongTrinh") {
                }
                else {
                    func_DienMucLucNganSach(data);
                    CapNhapLaiDuToanNamTruoc();
                }
                //jsControl_Next(id,500);
            });
        }
    //}
}

//Hàm ghép các mã chi tiết mục lục ngân sách
function funcGhepMa(Truong) {
    if (typeof Truong == "undefined") {
        Truong = LSN_MaCotNganSach;
    }
    var vR = "";
    var arr = strDSTruong.split(',');
    var i, ctlID;
    for (i = 1; i < arr.length - 1; i++) {
        if (arr[i] == Truong) {
            break;
        }
        ctlID = strParentID + '_' + arr[i] + '1';
        if (i > 1) vR += ',';
        vR += document.getElementById(ctlID).value;
    }
    return vR;
}

//Hàm lấy mã đơn vị cho trường Mã công trình
function funcLayMaDonVi() {
    var ctlID = strParentID + '_iID_MaDonVi';
    var vR = document.getElementById(ctlID).value;
    return vR;
}

//Hàm kiểm tra có dữ liệu trong mã chi tiết mục lục ngân sách hay không?
function funcCoMaMucLucNganSach() {
    var vR = false;
    var arr = strDSTruong.split(',');
    var i, ctlID;
    for (i = 1; i < arr.length - 1; i++) {
        ctlID = strParentID + '_' + arr[i] + '1';
        if (document.getElementById(ctlID).value != "") {
            vR = true;
            break;
        }
    }
    return vR;
}

function ThemMoiChiTietDuToan() {
    if (jsLichSuVaNhap_bDangThem == false) {
        jsLichSuVaNhap_bDangThem = true;
        Bang_ShowCloseDialog();

        jQuery.ajaxSetup({ cache: false });
        var url = jsLichSuVaNhap_UrlSave;
        var controls = $('input[chi-tiet-du-toan="1"]');
        var i;

        var ctlID_sTenDonVi = strParentID + '_sTenDonVi';
        var ctlID_sMaCongTrinh = strParentID + '_sMaCongTrinh';
        var sTenDonVi = '', sMaCongTrinh = '';
        
        for (i = 0; i < controls.length; i++) {
            var ctlID = $(controls[i]).attr("id");
            if (ctlID.endsWith("iID_MaDonVi1")) {
                sTenDonVi = $("#" + ctlID).val();
            }
            if (ctlID.endsWith("1")) {
                ctlID = ctlID.substring(0, ctlID.length - 1);
            }
            var value = $("#" + ctlID).val();
            if (ctlID.endsWith("_show")) {
                ctlID = ctlID.substring(0, ctlID.length - 5);
                value = $("#" + ctlID).val();
                if (value == "") {
                    value = 0;
                }
            }
            url += "&" + ctlID + "=" + encodeURI(value);
        }
        sMaCongTrinh = $("#" + ctlID_sMaCongTrinh).val();
        url += "&" + ctlID_sTenDonVi + "=" + encodeURI(sTenDonVi);
        url += "&" + ctlID_sMaCongTrinh + "=" + encodeURI(sMaCongTrinh);

        //url = (url);
        $.getJSON(url, function (item) {
            jsLichSuVaNhap_bDangThem = false;
            Bang_HideCloseDialog();
            if (item == null) {
                alert('Bạn không được phép sử dụng chức năng này!');
            }
            else if (item.value == '-1') {
                parent.reloadPage();
            }
            else if (item.value == '1') {

                
                //Đã thêm được dữ liệu
                DaThemXongChiTietDuToan();
            }
            else {
                //Không thêm được dữ liệu
                alert('Không thêm được dữ liệu!');
            }
        });
    }
}

function DaThemXongChiTietDuToan() {
    var n = jsLichSuVaNhap_nH;
    var i, j, cs1, cs2, d, idText;
    var HangTieuDe_Sau = null;
    var HangTieuDe_Truoc = null;
    var HangTien_Sau = null;
    var HangTien_Truoc = null;
    for (var i = n - 2; i >= 0; i--) {
        cs1 = i + 1;
        cs2 = i;
        HangTieuDe_Sau = document.getElementById('idHangTieuDe' + cs1);
        HangTieuDe_Truoc = document.getElementById('idHangTieuDe' + cs2);
        HangTien_Sau = document.getElementById('idHangTien' + cs1);
        HangTien_Truoc = document.getElementById('idHangTien' + cs2);
        if (HangTieuDe_Sau && HangTien_Sau) {
            d = -1;
            for (j = 0; j < HangTieuDe_Sau.cells.length; j++) {
                d++;
                HangTieuDe_Sau.cells[j].innerHTML = HangTieuDe_Truoc.cells[j].innerHTML;
            }
            for (j = 0; j < HangTien_Sau.cells.length; j++) {
                d++;
                HangTien_Sau.cells[j].innerHTML = HangTien_Truoc.cells[j].innerHTML;
            }
        }
        if (HangTieuDe_Truoc.style.display == ''){
            HangTieuDe_Sau.style.display = '';
            HangTien_Sau.style.display = '';
        }
    }
    if (HangTieuDe_Truoc && HangTieuDe_Truoc) {
        d = -1;
        for (j = 0; j < HangTieuDe_Sau.cells.length; j++) {
            d++;
            idText = $('input[cot="' + d + '"]').attr("id");
            if (idText.endsWith('_sMoTa') || idText.endsWith('_iID_MaDonVi1')) {
                HangTieuDe_Truoc.cells[j].innerHTML = '<span style="display: inline-block;overflow: hidden;white-space:nowrap;">' + $('input[cot="' + d + '"]').val() + '</span>';
            }
            else {
                HangTieuDe_Truoc.cells[j].innerHTML = $('input[cot="' + d + '"]').val();
            }
        }
        for (j = 0; j < HangTien_Sau.cells.length; j++) {
            d++;
            idText = $('input[cot="' + d + '"]').attr("id");
            HangTien_Truoc.cells[j].innerHTML = $('input[cot="' + d + '"]').val();
            if (idText.endsWith('_show')) {
                idText = idText.substring(0, idText.length - 5);
                $('#' + idText).val('0');
            }
            $('input[cot="' + d + '"]').val('');
        }
    }
    //Load lại danh sách
    jsDuToan_LoadLaiChiTiet();
    jsControl_Next('btnOK');
}

$(document).ready(function () {
    $('input:text[truong-ngan-sach="1"]').keydown(function (event) {
        
        var keyCode = event.keyCode;
        if (48 <= keyCode && keyCode <= 59) {
            var txt_id = $(this).attr("id");

            if (jsDuToan_Interval != null) {
                clearInterval(jsDuToan_Interval);
                jsDuToan_Interval = null;
            }
            jsDuToan_Interval = setInterval(function () { jsDuToan_txt_onchange(txt_id); }, 500);
        }
    });
});

var jsDuToan_arrTruong = 'sLNS1,L1,K1,M1,TM1,TTM1,NG1,TNG1'.split(',');
var jsDuToan_arrDoDai = '7,3,3,4,4,2,2,2'.split(',');
var jsDuToan_Interval = null;

function jsDuToan_txt_onchange(txt_id) {
    var value = $('#' + txt_id).val();
    for (var i = 0; i < jsDuToan_arrTruong.length; i++) {
        if (txt_id.endsWith(jsDuToan_arrTruong[i])) {
            if (value.length == parseInt(jsDuToan_arrDoDai[i])) {
                jsControl_Next(txt_id, 10);
            }
        }
    }
    clearInterval(jsDuToan_Interval);
    jsDuToan_Interval = null;
}