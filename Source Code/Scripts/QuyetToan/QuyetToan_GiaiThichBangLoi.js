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

    if (Bang_nH == 0) {
        BangDuLieu_ThemHangMoi();
    }
    Bang_keys.fnSetFocus(0, 1);
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
    //Bang_keys.fnSetFocus(csH, 0);
  
}

/* Su kien BangDuLieu_onKeypress_F2
- Muc dinh: goi ham them hang khi an phim F2
*/
//function BangDuLieu_onKeypress_F2(h, c) {
//  
//        BangDuLieu_ThemHangMoi(h + 1, h);       
//    
//}

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
    var cs = Bang_arrCSMaCot["iSTT"];
    var cs1 = Bang_arrCSMaCot["sGiaiThich"];
    var cs2 = Bang_arrCSMaCot["sKienNghi"];
    if (Bang_arrGiaTri[h][cs1] == "" && Bang_arrGiaTri[h][cs2] == "") {
        Bang_arrThayDoi[h][cs] = false;
        Bang_arrThayDoi[h][cs1] = false;
        Bang_arrThayDoi[h][cs2] = false;
    } else {
        Bang_arrThayDoi[h][cs] = true;
        Bang_arrThayDoi[h][cs1] = true;
        Bang_arrThayDoi[h][cs2] = true;
    }
        
}