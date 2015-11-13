

/* Su kien BangDuLieu_onLoad
*/
function BangDuLieu_onLoad() {

    if (Bang_nH == 0) {
        BangDuLieu_ThemHangMoi();
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
    Bang_ThemHang(csH,hGiaTri);
    //Sua MaHang="": Day la hang them moi
    Bang_arrMaHang[csH] = "";
    var c;
    for (c = 0; c < Bang_nC; c++) {
        Bang_arrEdit[csH][c] = true;
    }

    csCot = 0;
    for (j = 0; j < Bang_arrCotDuocPhepNhap.length; j++) {
        if (Bang_arrCotDuocPhepNhap[j]) {
                csCot = j;
                break;
            }            
        }

        Bang_keys.fnSetFocus(csH, csCot);
}

/* Su kien BangDuLieu_onKeypress_F2
- Muc dinh: goi ham them hang khi an phim F2
*/
function BangDuLieu_onKeypress_F2(h, c) {
    BangDuLieu_ThemHangMoi(h + 1, h);
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

function BangDuLieu_onCellAfterEdit(h, c) {
    BangDuLieu_CapNhapLaiHangCha(h, c);
}

function BangDuLieu_CapNhapLaiHangCha(h, c) {
    if (Bang_arrMaCot[c].startsWith('r') == false) {
        return;
    }
  
    //BangDuLieu_TinhOTongSo(h);
    var csCha = h, GiaTri;
    while (Bang_arrCSCha[csCha] >= 0) {
        csCha = Bang_arrCSCha[csCha];
        GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
        Bang_GanGiaTriThatChoO(csCha, c, GiaTri);        
      //  BangDuLieu_TinhOTongSo(csCha);
    }
}
function BangDuLieu_TinhOTongSo(h) {
    var GT = 0, c, cTongSo = Bang_arrCSMaCot["rTongSo"];
    var cMax = cTongSo - 2;
    for (c = Bang_nC_Fixed + 1; c < cMax; c = c + 3) {
        if (Bang_arrMaCot[c].startsWith('r') && Bang_arrMaCot[c] != "rChiTapTrung") {
            GT = GT + Bang_arrGiaTri[h][c];
        }
    }
    Bang_GanGiaTriThatChoO(h, cTongSo, GT);
    BangDuLieu_TinhOConLai(h, cTongSo);
}

function BangDuLieu_TinhTongHangCon(csCha, c) {
    var h, vR = 0;
    for (h = 0; h < Bang_arrCSCha.length; h++) {
        if (csCha == Bang_arrCSCha[h]) {
            vR += parseFloat(Bang_arrGiaTri[h][c]);
        }
    }
    return vR;
}


