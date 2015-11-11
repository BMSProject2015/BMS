var jsCongSan_Parent = 'ChiTietTaiSan';

$(document).ready(function () {
    jsCongSan_DienDuLieu();
});

function jsCongSan_DienDuLieu() {
    var arrMaCot = parent.GoiHam_ChungTuChiTiet_BangDuLieu_Lay_arrMaCot();
    var c, ctlID;
    for (c = 0; c < arrMaCot.length; c++) {
        ctlID = jsCongSan_Parent + "_" + arrMaCot[c];
        if (document.getElementById(ctlID)) {
            var GiaTri = parent.GoiHam_ChungTuChiTiet_BangDuLieu_LayGiaTriTruong(arrMaCot[c]);
            if (document.getElementById(ctlID).nodeName == "SPAN") {
                document.getElementById(ctlID).innerHTML = GiaTri;
            }
            else {
                $("#" + ctlID).val(GiaTri);
                if (document.getElementById(ctlID + '_show')) {
                    var GTHienThi = parent.GoiHam_ChungTuChiTiet_BangDuLieu_LayGiaTriHienThiTruong(arrMaCot[c]);
                    $("#" + ctlID + "_show").val(GTHienThi);
                }
            }
        }
    }
}

function jsCongSan_GanDuLieu() {
    var arrMaCot = parent.GoiHam_ChungTuChiTiet_BangDuLieu_Lay_arrMaCot();
    var c, ctlID;
    for (c = 0; c < arrMaCot.length; c++) {
        ctlID = jsCongSan_Parent + "_" + arrMaCot[c];
        if (document.getElementById(ctlID)) {
            if (document.getElementById(ctlID).nodeName != "SPAN") {
                var GiaTri = $("#" + ctlID).val();
                parent.GoiHam_ChungTuChiTiet_BangDuLieu_GanGiaTriTruong(arrMaCot[c], GiaTri);
            }
        }
    }
}