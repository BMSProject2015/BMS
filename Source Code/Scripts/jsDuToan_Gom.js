var jsDuToan_iID_MaChungTu = '';
var jsDuToan_Url_Frame = '';
var jsDuToan_Url = '';

function jsDuToan_LoadLaiChiTiet() {
    if ($("#tabs-1").css("display") != "none") {
        var url = jsDuToan_Url_Frame;
        var controls = $('input[search-control="1"]');
        var i;
        for (i = 0; i < controls.length; i++) {
            var field = $(controls[i]).attr("search-field");
            var value = $(controls[i]).val();
            url += "&" + field + "=" + encodeURI(value);
        }
        document.getElementById("ifrChiTietChungTu").src = url;
    
    }
}

var jsDuToan_Search_inteval = null;

function jsDuToan_Search_onkeypress(e) {
    jsDuToan_Search_clearInterval();
    if (e.keyCode == 13) {
        jsDuToan_LoadLaiChiTiet();
    }
    else {
        jsDuToan_Search_inteval = setInterval(function () { jsDuToan_Search_clearInterval(); jsDuToan_LoadLaiChiTiet(); }, 2000);
    }
}

function jsDuToan_TrinhDuyetTuChoi(iAction) {
    var x = document.getElementById("ifrChiTietChungTu");
    var y = (x.contentWindow || x.contentDocument);
    var z = (y.document) ? y.document : y;
    return y.Bang_HamTruocKhiKetThuc(iAction);
}

function jsDuToan_Search_clearInterval() {
    clearInterval(jsDuToan_Search_inteval);
}

var jsDuToan_TreeHienThi = true;
function Tree_Open_Close() {
    var imgTree = document.getElementById("imgTree");
    jsDuToan_TreeHienThi = !jsDuToan_TreeHienThi;
    var wKhungChinh = fnGetWidthById('divKhungTongThe');
    var w1, w2;
    if (jsDuToan_TreeHienThi) {
        w2 = 300;
        imgTree.src = "../Content/Themes/images/P_page.gif";
        imgTree.title = "Đóng cây chứng từ";
        Tree_Create();
        $('#scrollbar1').tinyscrollbar();
    }
    else {
        w2 = 1;
        imgTree.src = "../Content/Themes/images/N_page.gif";
        imgTree.title = "Mở cây chứng từ";
    }
    //    alert($("#tbBangNhap").css("width"));
    w1 = wKhungChinh - w2 - 5;
    $("#tdKhungCot1").css("width", w2 + "px");
    $("#divKhungCot1").css("width", w2 + "px");
    $("#divKhungCot2").css("width", w1 + "px");
    $("#tdKhungCot2").css("width", w1 + "px");
    jsDuToan_LichDuVaNhap_onWindowResize(w1);
    //    $("#tbBangNhap").css("width", w1 + "px");
    //    alert($("#tbBangNhap").css("width"));
}

var okTree_Create = false;
function Tree_Create() {
    if ($("#divTree").css("display") != "none" && okTree_Create == false) {
        okTree_Create = true;
        $("#tree").treeview({
            collapsed: false,
            animated: "medium",
            control: "#sidetreecontrol",
            persist: "location"
        });
    }
}


function reloadPage() {
    window.location.href = jsDuToan_Url;
}
//Lấy giá trị của đơn vị gán cho trợ lý duyệt
function LayGiaTri(value, ParentID, id) {
    var to = id.indexOf("_DonVi");
    var _id = ParentID + "_" + id.substring(0, to);
    if (id.indexOf("_DonVi") > 0 && (document.getElementById(_id).value == "" || document.getElementById(_id).value == "0")) {
        document.getElementById(_id).value = UnFormatNumber(value);
        document.getElementById(_id + "_show").value = value;
    }
    TinhTong(ParentID);
}
function TinhTong(ParentID) {
    var Tong = 0;
    var DSTruongTien = "_rChiTaiKhoBac,_rTonKho,_rTuChi,_rHangNhap,_rHangMua,_rHienVat,_rDuPhong,_rPhanCap";
    var _arrDSTruongTien = DSTruongTien.split(',');
    var arrDSValue = new Array();
    for (var i = 0; i < 8; i++) {
        arrDSValue[i] = document.getElementById(ParentID + _arrDSTruongTien[i]).value;
            if(arrDSValue[i]!="")
                Tong += parseFloat(arrDSValue[i]);
        }
        document.getElementById(ParentID + "_rTongSo").value = Tong;
        document.getElementById(ParentID + "_rTongSo_show").value = FormatNumber(Tong);
}