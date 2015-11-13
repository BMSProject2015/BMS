/* BangDuLieu_CoCotTongSo: xac dinh bang co cot duyet hay khong*/
var BangDuLieu_CoCotDuyet = false;
/* BangDuLieu_CoCotTongSo: xac dinh bang co cot tong so hay khong*/
var BangDuLieu_CoCotTongSo = true;
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";

/* Su kien BangDuLieu_onLoad
*/
function BangDuLieu_onLoad() {
    if (typeof Bang_arrCSMaCot["bDongY"] == "undefined") {
        BangDuLieu_CoCotDuyet = false;
    }
    else {
        BangDuLieu_CoCotDuyet = true;
    }
    if (typeof Bang_arrCSMaCot["rTongSo"] == "undefined") {
        BangDuLieu_CoCotTongSo = false;
    }
    else {
        BangDuLieu_CoCotTongSo = true;
    }
    if (Bang_nH == 0) {
        BangDuLieu_ThemHangMoi();
    }
}

/* Ham BangDuLieu_CapNhapLaiHangCha
*   - Muc dinh: Cap nhap lai cac hang cha cua hang 'h', Tinh tong so cua hang h va cac hang cha
*   - Dau vao:  + h: chi so hang con
*               + c: vi tri cot cua o can tinh gia tri
*/
function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrType[c] == 1) {
        if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
            return;
        }
        BangDuLieu_TinhOTongSo(h);
        var csCha = h, GiaTri;
        while (Bang_arrCSCha[csCha] >= 0) {
            csCha = Bang_arrCSCha[csCha];
            GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
            Bang_GanGiaTriThatChoO(csCha, c, GiaTri);
            BangDuLieu_TinhOTongSo(csCha);
        }
    }
}

/* Ham BangDuLieu_TinhOTongSo
*   - Muc dinh: Tinh gia tri cua o tong so tren hang 'h'
*   - Dau vao:  + h: chi so hang can tinh tong so
*/
function BangDuLieu_TinhOTongSo(h) {
    if (BangDuLieu_CoCotTongSo) {
        var GT = 0, c, cMax = Bang_arrMaCot.length - 1, cTongSo = Bang_arrMaCot.length - 1;
        if (BangDuLieu_CoCotDuyet) {
            cTongSo = Bang_arrMaCot.length - 3;
            cMax = Bang_arrMaCot.length - 3;
        }
        for (c = 0; c < cMax; c++) {
            if (Bang_arrMaCot[c] != "rChiTapTrung") {
                GT = GT + Bang_arrGiaTri[h][c];
            }
        }
        Bang_GanGiaTriThatChoO(h, cTongSo, GT);
    }
}

/* Ham BangDuLieu_TinhTongHangCon
*   - Muc dinh: Tinh tong cac gia tri cac hang con co chi so hang cha la CSCha
*   - Dau vao:  + csCha: chi so hang cha can tinh tong
*               + c: vi tri cot cua o can tinh gia tri
*/
function BangDuLieu_TinhTongHangCon(csCha, c) {
    var h, vR = 0;
    for (h = 0; h < Bang_arrCSCha.length; h++) {
        if (csCha == Bang_arrCSCha[h]) {
            vR += parseFloat(Bang_arrGiaTri[h][c]);
        }
    }
    return vR;
}


/* Ham BangDuLieu_CheckAll_value
- Muc dinh: Check or Uncheck tat ca cac o co kieu du lieu la kieu 2(Checkbox)
*/
var BangDuLieu_CheckAll_value = false;
function BangDuLieu_CheckAll() {
    BangDuLieu_CheckAll_value = !BangDuLieu_CheckAll_value;
    var value = "0";
    if (BangDuLieu_CheckAll_value) {
        value = "1";
    }
    else {
        value = "0";
    }
    var h, c = Bang_arrCSMaCot["bDongY"];
    for (h = 0; h < Bang_arrMaHang.length; h++) {
        if (Bang_arrLaHangCha[h] == false) {
            Bang_GanGiaTriThatChoO(h, c, value);
        }
    }
}

/* Ham BangDuLieu_TinhTongHangCon
*   - Muc dinh: Tao 1 hang moi o tai vi tri 'h' lay du lieu tai vi tri 'hGiaTri'
*   - Dau vao:  + h: vi tri them
*               + hGiaTri: vi tri hang lay du lieu, =null: them 1 hang trong
*/
function BangDuLieu_ThemHangMoi(h, hGiaTri) {
    var csH = 0;
    if (h != null) {
        //Thêm 1 hàng mới vào hàng h
        csH = h;
    }
    else {
        //Thêm 1 hàng mới vào cuối bảng
        csH = Bang_nH;
    }
    Bang_ThemHang(csH, hGiaTri);
    //Sua MaHang="": Day la hang them moi
    Bang_arrMaHang[csH] = "";
    Bang_keys.fnSetFocus(csH, 0);
}

/* Su kien BangDuLieu_onKeypress_F2
- Muc dinh: goi ham them hang khi an phim F2
*/
function BangDuLieu_onKeypress_F2(h, c) {
    BangDuLieu_ThemHangMoi(h + 1, h);
    //Gán các giá trị của hàng mới thêm =0
    var arrTruongTien = "rSoTien,rSoTienDauNam,rSoTienCap,rSoTienDieuChinh,rSoTienQuyetToan,rSoTienChuDauTuDeNghi,rSoTienPheDuyet,rSoTien_TongDauTu,rSoTienNgoaiTe_TongDauTu,rSoTien_TongDuToan,rSoTienNgoaiTe_TongDuToan,rDonViDeNghi,rDonViThuTamUng,rDonViThu,rCucTaiChinhDeNghi,rCucTaiChinhThuTamUng,rCucTaiChinhThu,rKeHoachVonNamTruoc,rKeHoachVonNamNay,rKeHoachVonUngTruoc,rKeHoachVonCon,rSoTienCap,rSoTienDaCap,rSoTienDuToan".split(',');
    for (var i = 0; i < arrTruongTien.length; i++) {
        var cs = Bang_arrCSMaCot[arrTruongTien[i]];
        Bang_GanGiaTriThatChoO(h + 1, cs, 0);
    }
}

/* Su kien BangDuLieu_onKeypress_Delete
- Muc dinh: goi ham xoa hang khi an phim DELETE
*/
function BangDuLieu_onKeypress_Delete(h, c) {
    if (h != null) {
        Bang_XoaHang(h);
        if (h < Bang_nH) {
            Bang_keys.fnSetFocus(h, c);
        }
        else if (Bang_nH > 0) {

        }
    }
}

/* Su kien BangDuLieu_onCellAfterEdit
*   - Muc dinh: Su kien xuat hien sau khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*/
function BangDuLieu_onCellAfterEdit(h, c) {
    BangDuLieu_CapNhapLaiHangCha(h, c);
}