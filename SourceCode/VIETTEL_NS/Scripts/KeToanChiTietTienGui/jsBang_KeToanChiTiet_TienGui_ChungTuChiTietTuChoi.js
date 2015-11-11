
function BangDuLieu_onDblClick(h, c) {
    var iID_MaChungTu = Bang_LayGiaTri(h, "iID_MaChungTu");
    var iID_MaChungTuChiTiet = Bang_LayGiaTri(h, "iID_MaChungTuChiTiet");
    parent.ChuyenSangTrang_KeToanTongHop(iID_MaChungTu, iID_MaChungTuChiTiet);
    return true;
}