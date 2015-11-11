/* BangDuLieu_CoCotTongSo: xac dinh bang co cot duyet hay khong*/
var BangDuLieu_CoCotDuyet = false;
/* BangDuLieu_CoCotTongSo: xac dinh bang co cot tong so hay khong*/
var BangDuLieu_CoCotTongSo = true;
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri sau khi nhap xong o Autocomplete*/
var BangDuLieu_Url_getGiaTri = "";
/* BangDuLieu_Url_getGiaTri: url cua ham lay gia tri ngay khi bam 1 phim tren o Autocomplete*/
var BangDuLieu_Url_getDanhSach = "";
var jsKTCS_CheckMaTaiSan = false;

var jsKTCS_Check_MaTaiSan = "";
var jsQLDA_LayNamSuDung = "";
var jsKTCS_Url_ChiTietTaiSan_Dat = "";
var jsKTCS_Url_ChiTietTaiSan_Nha = "";
var jsKTCS_Url_ChiTietTaiSan_Oto = "";
var jsKTCS_Url_ChiTietTaiSan_Tren500Trieu = "";
var jsKTCS_Url_ChiTietTaiSan_Chung = "";
var jsKTCS_Url_Report = "";
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
/* Su kien BangDuLieu_onCellAfterEdit
*   - Muc dinh: Su kien xuat hien sau khi nhap du lieu moi tren o (h,c) cua bang du lieu
*   - Dau vao:  + h: chi so hang 
*               + c: chi so cot
*/
function BangDuLieu_onCellAfterEdit(h, c) {
    BangDuLieu_CapNhapLaiHangCha(h, c);
    var cs = Bang_arrCSMaCot["iID_MaTaiSan"];
    var csLoaiTS = Bang_arrCSMaCot["sTenLoaiTaiSan"];
    if (Bang_arrMaCot[c] == 'sTenNhomTaiSan') {
        BangDuLieu_TinhOConLai(h, c);
    }
    
    if (c > csLoaiTS && Bang_arrGiaTri[h][csLoaiTS] == "") {
        Bang_keys.fnSetFocus(h, csLoaiTS);
        alert("Bạn chưa nhập loại tài sản!");
        return false;
    }

    if (c > cs && Bang_arrGiaTri[h][cs] == "") {
        Bang_keys.fnSetFocus(h, cs);
        alert("Bạn chưa nhập mã tài sản!");
        return false;
    }
    
    if (Bang_arrMaCot[c] == 'iID_MaTaiSan') {
        var iID_MaTaiSan = Bang_arrGiaTri[h][c];
        jQuery.ajaxSetup({ cache: false });
        var vR;
        var url = jsKTCS_Check_MaTaiSan;
        url += "?iID_MaTaiSan=" + iID_MaTaiSan;
        $.getJSON(url, function (data) {
            if (data) {}
            else {
                Bang_GanGiaTriThatChoO(h, c, "");
                alert("Mã tài sản đã có. Bạn phải nhập mã khác!");
            }
        });
        return false;
    }
}

/* Kiểm tra đã nhập mã tài sản chưa*/

function KiemTraNhapMaTaiSan(h) {
    var GT = 0, c, cMax = Bang_arrMaCot.length - 1;
    var iID_MaBangTaiSan = Bang_arrMaHang[h]
   
    for (c = 0; c < cMax; c++) {        
        if (Bang_arrMaCot[c] == "iID_MaTaiSan") {
            var iID_MaTaiSan = Bang_arrGiaTri[h][c];                
            if (iID_MaTaiSan == "") {                
                alert("Chưa nhập mã tài sản!");
                return false;
            }
        }
    }
     if (iID_MaBangTaiSan == "") {
         alert("Bạn chưa lưu tài sản");
         return false;
    }
    return true;

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
        BangDuLieu_TinhOConLai(h, c);
        BangDuLieu_TinhOTongSo(h);
        var csCha = h, GiaTri;
        while (Bang_arrCSCha[csCha] >= 0) {
            csCha = Bang_arrCSCha[csCha];
            GiaTri = BangDuLieu_TinhTongHangCon(csCha, c);
            Bang_GanGiaTriThatChoO(csCha, c, GiaTri);
            BangDuLieu_TinhOConLai(csCha, c);
            BangDuLieu_TinhOTongSo(csCha);
        }
    }
    else if (Bang_arrType[c] == 0) {
        if (BangDuLieu_CoCotDuyet && c >= Bang_arrMaCot.length - 2) {
            return;
        }
        BangDuLieu_TinhOConLai(h, c);
    }
}

function BangDuLieu_TinhOConLai(h, c) {
    if (Bang_arrMaCot[c] == "sTenNhomTaiSan") {
        var csC = Bang_arrCSMaCot["iID_MaNhomTaiSan"];
        var iID_MaNhomTaiSan = Bang_arrGiaTri[h][csC];

        jQuery.ajaxSetup({ cache: false });
        var vR;
        var url = jsQLDA_LayNamSuDung;
        url += "?iID_MaNhomTaiSan=" + iID_MaNhomTaiSan;
        csC = Bang_arrCSMaCot["rSoNamKhauHao"];
        $.getJSON(url, function (item) {            
            Bang_GanGiaTriThatChoO(h, csC, item.rSoNamKhauHao);
        });
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
        BangDuLieu_TinhOConLai(h, cTongSo);
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
    BangDuLieu_CapNhapSTT();

    //Gán các giá trị của hàng mới thêm = ''
    var arrTruongTien = "iID_MaTaiSan,sTenTaiSan,sMoTa".split(',');
    for (var i = 0; i < arrTruongTien.length; i++) {
        var cs = Bang_arrCSMaCot[arrTruongTien[i]];
        Bang_GanGiaTriThatChoO(h + 1, cs, '');
    }
}

/* Su kien BangDuLieu_onKeypress_F4
- Muc dinh: nhập thông tin chi tiết của 1 hàng
*/
function BangDuLieu_onKeypress_F4(h, c) {    
    if (KiemTraNhapMaTaiSan(h)) {
        BangDuLieu_Dialog_Show(h, c);
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
        BangDuLieu_CapNhapSTT();
    }
}

function BangDuLieu_CapNhapSTT() {
    for (var h = 0; h < Bang_nH; h++) {
        var cs = Bang_arrCSMaCot["sSTT"];
        Bang_GanGiaTriThatChoO(h, cs, h + 1);
    }
}

//Dialog
var BangDuLieu_Dialog_h = -1;
var BangDuLieu_Dialog_c = -1;

function BangDuLieu_Dialog_Show(h, c) {
    csC = Bang_arrCSMaCot['iLoaiTS'];
    BangDuLieu_Dialog_h = h;
    BangDuLieu_Dialog_c = c;
    switch (Bang_arrGiaTri[h][csC]) {
        case 0:
            BangDuLieu_Dialog_ChiTiet_Show(jsKTCS_Url_ChiTietTaiSan_Chung, h, c, 700, 500);
            break;
        case 1:
            BangDuLieu_Dialog_ChiTiet_Show(jsKTCS_Url_ChiTietTaiSan_Dat, h, c, 800,500);
            break;

        case 2:
            BangDuLieu_Dialog_ChiTiet_Show(jsKTCS_Url_ChiTietTaiSan_Nha, h, c, 800, 500);
            break;

        case 3:
            BangDuLieu_Dialog_ChiTiet_Show(jsKTCS_Url_ChiTietTaiSan_Oto,h,c, 700, 500);
            break;

        case 4:
            BangDuLieu_Dialog_ChiTiet_Show(jsKTCS_Url_ChiTietTaiSan_Tren500Trieu,h, c,700,400);
            break;
    }
}

function BangDuLieu_Dialog_Close() {
    Bang_keys.fnSetFocus(BangDuLieu_Dialog_h, BangDuLieu_Dialog_c);
}

function BangDuLieu_Dialog_HienThiGiaTri(idDialog) {
    var c, ctlID;
    var h = BangDuLieu_Dialog_h;
    for (c = 0; c < Bang_nC; c++) {
        ctlID = idDialog + "_" + Bang_arrMaCot[c];
        if (document.getElementById(ctlID)) {
            if (document.getElementById(ctlID).nodeName == "SPAN") {
                document.getElementById(ctlID).innerHTML = Bang_arrHienThi[h][c];
            }
            else {
                $("#" + ctlID).val(Bang_arrGiaTri[h][c]);
                if (document.getElementById(ctlID + '_show')) {
                    $("#" + ctlID + "_show").val(Bang_arrHienThi[h][c]);
                }
            }
        }
    }
}

function BangDuLieu_Dialog_GanGiaTri(idDialog) {
    var c, ctlID;
    var h = BangDuLieu_Dialog_h;
    for (c = 0; c < Bang_nC; c++) {
        ctlID = idDialog + "_" + Bang_arrMaCot[c];
        if (document.getElementById(ctlID)) {
            if (document.getElementById(ctlID).nodeName != "SPAN") {
                Bang_GanGiaTriO(h, c, $("#" + ctlID).val());
                alert('dd');
            }
        }
    }
}

//Dialog Dialog_Dat

function jsKTCS_ChiTietTaiSan_Dialog_Open(jsKTCS_url,iID_MaTaiSan) {
    var url = jsKTCS_url + "?iID_MaTaiSan=" + iID_MaTaiSan;
    document.getElementById("KTCS_TaiSanChiTiet_iFrame").src = url;    
}

function BangDuLieu_Dialog_ChiTiet_Show(jsKTCS_url, h, c, width, height) {
    $("#div_KTCS_TaiSanChiTiet").dialog({
        width: width,
        height: height,
        modal: true,
        title: 'Chi tiết tài sản',
        close: KTCS_TaiSan_Dialog_Close
    });
   var csC = Bang_arrCSMaCot['iID_MaTaiSan'];   
    jsKTCS_ChiTietTaiSan_Dialog_Open(jsKTCS_url,Bang_arrGiaTri[h][csC]); 
}


//Dialog Dialog_Dat

function BangDuLieu_Dialog_ChiTiet_btnOK_Click(ControlId) {
    BangDuLieu_Dialog_GanGiaTri(ControlId);
    $("#div_KTCS_TaiSanChiTiet").dialog('close');
}

function BangDuLieu_Dialog_ChiTiet_btnIn_Click(ControlId,iID_MaTaiSan) {
    var obj = ControlId + "_in";
    BangDuLieu_Dialog_GanGiaTri(ControlId);    
    $("#div_Report").dialog({
        width: 900,
        height: 600,
        modal: true,
        title: 'In chi tiết',
        close: Report_Dialog_Close
    });
    //open frame
     var url = jsKTCS_Url_Report + "?iID_MaTaiSan=" + iID_MaTaiSan;
     document.getElementById("ifr_Report").src = url;  
    $("#div_KTCS_TaiSanChiTiet").dialog('close');

   
}

function BangDuLieu_Dialog_ChiTiet_btnCancel_Click(ControlId) {
    $("#div_KTCS_TaiSanChiTiet").dialog('close');
}
function BangDuLieu_Dialog_Close() {
    Bang_keys.fnSetFocus(BangDuLieu_Dialog_h, BangDuLieu_Dialog_c);
}

function KTCS_TaiSan_Dialog_Close() {
    parent.$("#div_KTCS_TaiSanChiTiet").dialog('close');
}


function Report_Dialog_Close() {
    $('#div_Report').dialog('close');
}