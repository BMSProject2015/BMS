//Hàm ghép các mã chi tiết mục lục ngân sách
function funcGhepMa(Truong) {
    if (typeof Truong == "undefined") {
        Truong = LSN_MaCotNganSach;
    }
    var vR = "";
    var arr = strDSTruong.split(',');
    var i, ctlID;
    for (i = 0; i < arr.length - 1; i++) {
        if (arr[i] == Truong) {
            break;
        }
        ctlID = strParentID + '_' + arr[i] + '1';
        if (i > 0) vR += ',';
        vR += document.getElementById(ctlID).value;
    }
    return vR;
}

//Hàm blur khi nhập mã chi tiết mục lục ngân sách
function func_Auto_Complete_onblur(id, Truong) {
    var ctlID = strParentID + '_' + Truong;
    var ctlID_HienThi = ctlID + "1";
    var CoThayDoi = false;

    if (arrGiaTriCu[ctlID] != document.getElementById(ctlID_HienThi).value) {
        CoThayDoi = true;
    }
    if (CoThayDoi) {
        //Trong trường hợp giá trị nhập vào khác rỗng và khác giá trị cũ thì sẽ được tìm giá trị mới và nhập vào

        var DSGiaTri = funcGhepMa(Truong);
        var GT = document.getElementById(ctlID_HienThi).value.split('-')[0];
        GT = $.trim(GT);

        arrGiaTriCu[ctlID] = GT;
        document.getElementById(ctlID).value = GT;


        //Trường hợp có nhập mã chi tiết mục lục ngân sách
        var TruongURL = "iID_MaMucLucNganSach";
        var url = unescape(Bang_Url_getGiaTri + '?Truong=' + TruongURL + '&GiaTri=' + GT + '&DSGiaTri=' + DSGiaTri);
        LSN_MaCotNganSach_Dien = Truong;
        $.getJSON(url, function (data) {
            document.getElementById(ctlID).value = data.value;
            document.getElementById(ctlID_HienThi).value = data.label;
            arrGiaTriCu[ctlID] = data.value;
            func_DienMucLucNganSach(data);
        });
    }
}

//Điền thông tin vào các mã chi tiết mục lục ngân sách
function func_DienMucLucNganSach(item) {
    var i, ctlID, TenTruong, GiaTri;
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
            ctlID = strParentID + '_' + arr1[0];
            if (document.getElementById(ctlID)) {
                document.getElementById(ctlID).value = arr1[1];
            }
            if (document.getElementById(ctlID + '1')) {
                document.getElementById(ctlID + '1').value = arr1[1];
            }
        }
        if (LSN_MaCotNganSach_Dien != "sTNG") {
            jsControl_Next(LSN_MaCotNganSach);
        }
    }
    else {
        var arrMucLuc = strDSTruong.split(",");
        for (i = 0; i < arrMucLuc.length; i++) {
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
    }
}

function func_Auto_Complete(id, ui) {

}

//Lưu lại các mã chi tiết mục lục ngân sách
function LuuGiaTriHienTai() {
    var arr = strDSTruong.split(',');
    var i, ctlID;
    for (i = 0; i < arr.length; i++) {
        if (arr[i] != "sMoTa") {
            ctlID = strParentID + '_' + arr[i];
            arrGiaTriCu[ctlID] = document.getElementById(ctlID).value;
        }
    }
}