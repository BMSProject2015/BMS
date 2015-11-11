var Luong_Dialog_id;

$(document).ready(function () {
    Luong_Dialog_HienThiGiaTri();
});

function Luong_Dialog_HienThiGiaTri() {
    var c, ctlID;
    var h = parent.BangDuLieu_Dialog_h;
    for (c = 0; c < parent.Bang_nC; c++) {
        ctlID = Luong_Dialog_id + "_" + parent.Bang_arrMaCot[c];
        if (document.getElementById(ctlID)) {
            if (document.getElementById(ctlID).nodeName == "SPAN") {
                document.getElementById(ctlID).innerHTML = parent.Bang_arrHienThi[h][c];
            }
            else {
                $("#" + ctlID).val(parent.Bang_arrGiaTri[h][c]);
                if (document.getElementById(ctlID + '_show')) {
                    $("#" + ctlID + "_show").val(parent.Bang_arrHienThi[h][c]);
                }
            }
        }
    }
    c = parent.Bang_arrCSMaCot["sHoDem_CanBo"];
    var HoDem = parent.Bang_arrGiaTri[h][c];
    c = parent.Bang_arrCSMaCot["sTen_CanBo"];
    var Ten = parent.Bang_arrGiaTri[h][c];
    document.getElementById(Luong_Dialog_id + '_CanBo_HoTenDayDu').value = HoDem + " - " + Ten;
}

function Luong_Dialog_GanGiaTri() {
    var c, ctlID;
    var h = parent.BangDuLieu_Dialog_h;
    for (c = 0; c < parent.Bang_nC; c++) {
        ctlID = Luong_Dialog_id + "_" + parent.Bang_arrMaCot[c];
        if (document.getElementById(ctlID)) {
            if (document.getElementById(ctlID).nodeName != "SPAN") {
                parent.Bang_GanGiaTriO(h, c, $("#" + ctlID).val());
            }
        }
    }
}

function Luong_Dialog_btnOK_Click() {
    Luong_Dialog_GanGiaTri();
    Luong_Dialog_Close();
}

function Luong_Dialog_btnCancel_Click() {
    Luong_Dialog_Close();
}

function Luong_Dialog_Close() {
    parent.BangDuLieu_Dialog_Close();
}

function func_Auto_Complete(id, ui) {
    document.getElementById(Luong_Dialog_id + '_sSoSoLuong').value = ui.item.sSoSoLuong;
    document.getElementById(Luong_Dialog_id + '_sSoTaiKhoan').value = ui.item.sSoTaiKhoan;
    document.getElementById(Luong_Dialog_id + '_iID_MaNgachLuong').value = ui.item.iID_MaNgachLuong;
    document.getElementById(Luong_Dialog_id + '_iID_MaBacLuong').value = ui.item.iID_MaBacLuong;
    document.getElementById(Luong_Dialog_id + '_sTen').value = ui.item.sTen;
    document.getElementById(Luong_Dialog_id + '_sHoDem').value = ui.item.sHoDem;
    document.getElementById(Luong_Dialog_id + '_rLuongToiThieu').value = ui.item.LuongToiThieu;
    document.getElementById(Luong_Dialog_id + '_iSoNguoiPhuThuoc').value = ui.item.iSoNguoiPhuThuoc;
    document.getElementById(Luong_Dialog_id + '_iSoNguoiPhuThuoc_show').value = ui.item.iSoNguoiPhuThuoc;
}