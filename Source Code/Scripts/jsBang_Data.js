var Bang_DauCachHang = "#|", Bang_DauCachO = "##";
var Bang_ChiDoc = true; // Giá trị xác định bảng chỉ đọc
var Bang_DaThayDoi = false;
var Bang_TroLyPhongBan = false; // Xác định tài khoản đó có phải ở cấp trợ lý phòng ban hay không
var Bang_arrGiaTri; // Mảng giá trị của bảng
var Bang_arrHienThi; // Mảng giá trị hiển thị của bảng
var Bang_arrEdit; // Mảng xác định các ô được nhập dữ liệu
var Bang_arrCotDuocPhepNhap; // Mảng xác định các cột được nhập dữ liệu
var Bang_arrThayDoi; // Mảng lưu các dữ liệu đã thay đổi

var Bang_arrCSCha; // Mảng lưu trữ chỉ số hàng cha của 1 hàng a=Bang_arrCSCha[b]: a là hàng cha của b
var Bang_arrLaHangCha; // Mảng xác định 1 hàng là hàng cha


var Bang_arrHangDaXoa = new Array(); //Mảng các mã hàng đã xóa
var Bang_arrMaHang; // Mảng các mã hàng
var Bang_arrMaCot; // Mảng các mã cột
var Bang_arrType; // Mảng kiểu mã của các cột: 0:Kieu xau; 1: Kieu so; 2:checkbox; 3:Autocomplete; 4:datetime
var Bang_arrFormat; // Mảng các định dạng

var Bang_nH = 0; // Số các hàng
var Bang_nC = 0; // Số các cột
var Bang_nC_Fixed = 0; // Số các cột Fixed
var Bang_nC_Slide = 0; // Số các cột Slide

var Bang_arrCSMaCot; // Mảng ánh xạ từ tên sang chỉ số của Bang_arrMaCot

var Bang_arrDoRongCot; // Mảng độ rộng của từng cột
var Bang_arrHienThiCot;
var Bang_DoRongHang = 25; // Giá trị xác định độ rộng cố định của 1 hàng

//<-- Phần khai báo cho Viewport-->
var Bang_Viewport_NMax = 50; // Giá trị số hàng tối đa của Viewport
var Bang_Viewport_hMin = 0; // Giá trị vị trí của Viewport trong Bang_arrMaHang
var Bang_Viewport_N = Bang_Viewport_NMax; // Giá trị số hàng của Viewport
var objBangDuLieu_Slide_Slide, objBangDuLieu_Fixed; // Đối tượng bảng Fixed, Slide
var Bang_arrCell_Slide = null, Bang_arrCell_Fixed = null; // Mảng các đối tượng cell bảng Fixed, Slide
var Bang_arrRow_Slide = null, Bang_arrRow_Fixed = null; // Mảng các đối tượng row bảng Fixed, Slide
var Bang_arrRowBold = null; // Mảng xác định các hàng có là hàng chữ đậm hay không
var Bang_arrRowDaXoa = null; // Mảng xác định các hàng có là đánh dấu đã xóa hay không


var Bang_sMauSac_ChuaDuyet = "";
var Bang_sMauSac_TuChoi = "#00BB00";
var Bang_sMauSac_DongY = "#0000BB";

/*  Ham Bang_Viewport_ThayDoiHang()
*   - Muc dich: Ham hien thi hoac an cac hang an khi co thay doi Viewport
*/
function Bang_Viewport_ThayDoiHang() {
    if (Bang_Viewport_NMax >= Bang_nH) {
        var i;
        if (Bang_Viewport_N < Bang_nH) {
            for (i = Bang_Viewport_N; i < Bang_nH; i++) {
                //Hien thi hang moi them
                $('#' + BangDuLieuID_Slide + ' tr:eq(' + i + ')').css('display', '');
                $('#' + BangDuLieuID_Fixed + ' tr:eq(' + i + ')').css('display', '');
            }
            Bang_Viewport_N = Bang_nH;
        }
        else {
            for (i = Bang_nH; i < Bang_Viewport_N; i++) {
                //An hang moi xoa
                $('#' + BangDuLieuID_Slide + ' tr:eq(' + i + ')').css('display', 'none');
                $('#' + BangDuLieuID_Fixed + ' tr:eq(' + i + ')').css('display', 'none');
            }
            Bang_Viewport_N = Bang_nH;
        }
    }
}

/*  Hàm Bang_TaoDoiTuongMoi
*   - Mục đích: tạo đối tượng mới dựa trên đối tượng obj để thêm dữ liệu
*   - Đầu vào:  + obj: đối tượng dùng copy dữ liệu
*   - Đầu ra: đối tượng được tạo mới
*/
function Bang_TaoDoiTuongMoi(obj) {
    var vR = obj;
    if (typeof obj == "object") {
        vR = new Array();
        for (i = 0; i < obj.length; i++) {
            vR[i] = obj[i];
        }
    }
    return vR;
}

/*  Hàm Bang_ThemHang
*   - Mục đích: chèm thêm hàng dữ liệu mới dựa trên hàng đã có
*               Các bảng sẽ bị thay đổi theo:
*                   + Bang_arrMaHang
*                   + Bang_arrCSCha
*                   + Bang_arrLaHangCha
*                   + Bang_arrGiaTri
*                   + Bang_arrHienThi
*                   + Bang_arrEdit
*                   + Bang_arrThayDoi
*                   + Bang_arrHangDaXoa
*   - Đầu vào:  + cs: chỉ số hàng chèn dữ liệu
*               + csGoc: chỉ số hàng lấy dữ liệu
*   - Đầu ra:   + true: nếu thêm được, false: ngược lại
*/
function Bang_ThemHang(cs, csGoc) {
    var i, j;
    if (typeof csGoc == "undefined") {
        //Them hang du lieu moi
        Bang_nH = Bang_nH + 1;
        Bang_Viewport_ThayDoiHang();

        Bang_arrMaHang.splice(cs, 0, "");
        Bang_arrCSCha.splice(cs, 0, -1);
        Bang_arrLaHangCha.splice(cs, 0, false);
        Bang_arrHangDaXoa.splice(cs, 0, false);
        var arrGT = new Array();
        var arrHT = new Array();
        var arrThayDoi = new Array();
        var arrEdit = new Array();

        for (j = 0; j < Bang_nC; j++) {
            switch (Bang_arrType[j]) {
                case 1:
                    arrGT.push(0);
                    break;

                case 2:
                    arrGT.push("0");
                    break;

                default:
                    arrGT.push("");
                    break;
            }
            arrHT.push("");
            arrThayDoi.push(false);
            arrEdit.push(true);
        }
        Bang_arrGiaTri.splice(cs, 0, arrGT);
        Bang_arrHienThi.splice(cs, 0, arrHT);
        Bang_arrThayDoi.splice(cs, 0, arrThayDoi);
        Bang_arrEdit.splice(cs, 0, arrEdit);

        //Sua lai Bang_arrCSCha: Cac hang cha co chi so lon hon cs se bi thay doi chi so
        for (i = 0; i < Bang_nH; i++) {
            if (Bang_arrCSCha[i] >= cs) {
                Bang_arrCSCha[i] = Bang_arrCSCha[i] + 1;
            }
        }
        Bang_HienThiDuLieu();
        return true;
    }
    else if (Bang_arrLaHangCha[csGoc] == false) {
        //Them hang du lieu dua tren 1 hang du lieu khac
        Bang_nH = Bang_nH + 1;
        Bang_Viewport_ThayDoiHang();

        Bang_arrMaHang.splice(cs, 0, Bang_TaoDoiTuongMoi(Bang_arrMaHang[csGoc]));
        Bang_arrCSCha.splice(cs, 0, Bang_TaoDoiTuongMoi(Bang_arrCSCha[csGoc]));
        Bang_arrLaHangCha.splice(cs, 0, Bang_TaoDoiTuongMoi(Bang_arrLaHangCha[csGoc]));
        Bang_arrGiaTri.splice(cs, 0, Bang_TaoDoiTuongMoi(Bang_arrGiaTri[csGoc]));
        Bang_arrHienThi.splice(cs, 0, Bang_TaoDoiTuongMoi(Bang_arrHienThi[csGoc]));
        Bang_arrThayDoi.splice(cs, 0, Bang_TaoDoiTuongMoi(Bang_arrThayDoi[csGoc]));
        Bang_arrEdit.splice(cs, 0, Bang_TaoDoiTuongMoi(Bang_arrEdit[csGoc]));
        Bang_arrHangDaXoa.splice(cs, 0, false);

        //Sua lai Bang_arrThayDoi
        for (j = 0; j < Bang_nC; j++) {
            Bang_arrThayDoi[cs][j] = true;
        }
        //Sua lai Bang_arrCSCha: Cac hang cha co chi so lon hon cs se bi thay doi chi so
        for (i = 0; i < Bang_nH; i++) {
            if (Bang_arrCSCha[i] >= cs) {
                Bang_arrCSCha[i] = Bang_arrCSCha[i] + 1;
            }
        }
        Bang_HienThiDuLieu();
        return true;
    }
    return false;
}

/*  Hàm Bang_XoaHang
*   - Mục đích: xóa hàng dữ liệu đã có
*   - Đầu vào:  + cs: chỉ số hàng cần xóa
*   - Đầu ra:   + true: nếu xóa được, false: ngược lại
*/
function Bang_XoaHang(cs) {
    if (cs != null && 0 <= cs && cs < Bang_nH && Bang_arrLaHangCha[cs] == false) {
        Bang_arrHangDaXoa[cs] = !Bang_arrHangDaXoa[cs];
        Bang_HienThiDuLieu();
        return true;
    }
    return false;
}

/*  Hàm Bang_AnHienCot
*   - Mục đích: Ẩn hiện cột
*   - Đầu vào:  + csC: chỉ số cột cần xóa
*               + TrangThai: true: hiển thị, false: ẩn
*   - Đầu ra:   + true: nếu xóa được, false: ngược lại
*/
function Bang_AnHienCot(csC, TrangThai) {
    if (Bang_arrHienThiCot[csC] != TrangThai) {
        Bang_arrHienThiCot[csC] = TrangThai;
        var strDisplay = '';
        if (Bang_arrHienThiCot[csC]) {
            //Hiển thị cột
            strDisplay = '';
        }
        else {
            //Ẩn cột
            strDisplay = 'none';
        }

        $('#' + Bang_ID + '_th' + csC).css('display', strDisplay);

        var i;
        var j = AnhXaCot_DuLieu_Fixed(csC);
        if (j >= 0) {
            for (i = 0; i < Bang_Viewport_NMax; i++) {
                $(Bang_arrCell_Fixed[i][j]).css('display', strDisplay);
            }
        }
        j = AnhXaCot_DuLieu_Slide(csC);
        if (j >= 0) {
            for (i = 0; i < Bang_Viewport_NMax; i++) {
                $(Bang_arrCell_Slide[i][j]).css('display', strDisplay);
            }
        }
    }
}


//
function Bang_KhoiTao() {
    var i, j;
    objBangDuLieu_Slide = $("#" + BangDuLieuID_Slide)[0];
    objBangDuLieu_Fixed = $("#" + BangDuLieuID_Fixed)[0];
    var TroLyPhongBan = document.getElementById('idTroLyPhongBan');
    if (TroLyPhongBan != null) {
        Bang_TroLyPhongBan = document.getElementById('idTroLyPhongBan').value;
    }
    Bang_Viewport_NMax = objBangDuLieu_Fixed.rows.length;

    Bang_Viewport_N = parseInt(document.getElementById('idViewport_N').value);


    Bang_arrMaHang = Bang_LayMang1ChieuGiaTri('idXauMaCacHang');
    Bang_arrMaCot = Bang_LayMang1ChieuGiaTri('idXauMaCacCot');
    Bang_arrDoRongCot = Bang_LayMang1ChieuGiaTri('idXauDoRongCot');
    Bang_arrHienThiCot = Bang_LayMang1ChieuGiaTri('idXauHienThiCot');
    Bang_arrCSCha = Bang_LayMang1ChieuGiaTri('idXauChiSoCha');
    Bang_arrLaHangCha = Bang_LayMang1ChieuGiaTri('idXauLaHangCha');
    Bang_arrType = Bang_LayMang1ChieuGiaTri('idXauKieuDuLieu');
    Bang_arrFormat = Bang_LayMang1ChieuGiaTri('idXauDinhDangDuLieu');
    Bang_arrCotDuocPhepNhap = Bang_LayMang1ChieuGiaTri('idXauDSCotDuocPhepNhap');

    Bang_arrGiaTri = Bang_LayMang2ChieuGiaTri('idXauGiaTriChiTiet');
    Bang_arrThayDoi = Bang_LayMang2ChieuGiaTri('idXauDuLieuThayDoi');
    Bang_arrEdit = Bang_LayMang2ChieuGiaTri('idXauEdit');


    Bang_nH = Bang_arrMaHang.length;
    Bang_nC = Bang_arrMaCot.length;
    Bang_nC_Fixed = parseInt(document.getElementById('idNC_Fixed').value);
    Bang_nC_Slide = parseInt(document.getElementById('idNC_Slide').value);

    //Xac dinh so hang cua Viewport
    if (Bang_Viewport_NMax > Bang_nH) {
        Bang_Viewport_N = Bang_nH;
    }

    //Tạo mảng CSMaCot, CSMaCot_Slide, CSMaCot_Fixed
    Bang_arrCSMaCot = new Array();
    for (j = 0; j < Bang_nC; j++) {
        Bang_arrCSMaCot[Bang_arrMaCot[j]] = j;
    }

    //Nếu không có bảng hiện thị cột thì sẽ tạo mới
    if (Bang_arrHienThiCot.length == 0) {
        for (j = 0; j < Bang_nC; j++) {
            Bang_arrHienThiCot.push("1");
        }
    }

    Bang_arrRowBold = new Array();
    Bang_arrRowDaXoa = new Array();
    Bang_arrRow_Fixed = new Array();
    Bang_arrRow_Slide = new Array();
    Bang_arrCell_Fixed = new Array();
    Bang_arrCell_Slide = new Array();
    for (i = 0; i < Bang_Viewport_NMax; i++) {
        //Them doi tuong hang
        Bang_arrRowBold.push(false);
        Bang_arrRowDaXoa.push(false);
        Bang_arrRow_Fixed.push(objBangDuLieu_Fixed.rows[i]);
        Bang_arrRow_Slide.push(objBangDuLieu_Slide.rows[i]);
        //Them doi tuong o
        Bang_arrCell_Fixed.push(new Array());
        for (j = 0; j < Bang_nC_Fixed; j++) {
            Bang_arrCell_Fixed[i].push(objBangDuLieu_Fixed.rows[i].cells[j]);
        }
        Bang_arrCell_Slide.push(new Array());
        for (j = 0; j < Bang_nC_Slide; j++) {
            Bang_arrCell_Slide[i].push(objBangDuLieu_Slide.rows[i].cells[j]);
        }
    }

    if (document.getElementById('idBangChiDoc') != null &&
            document.getElementById('idBangChiDoc').value != "1") {
        Bang_ChiDoc = false;
    }

    //Sửa lại giá trị của mảng Bang_arrType, Bang_arrDoRongCot, Bang_arrHienThiCot, Bang_arrCotDuocPhepNhap
    for (j = 0; j < Bang_nC; j++) {
        Bang_arrType[j] = parseInt(Bang_arrType[j]);
        Bang_arrDoRongCot[j] = parseInt(Bang_arrDoRongCot[j]);
        Bang_arrHienThiCot[j] = (Bang_arrHienThiCot[j] == "1") ? true : false;
        Bang_arrCotDuocPhepNhap[j] = (Bang_arrCotDuocPhepNhap[j] == "1") ? true : false;
    }

    //Sua lai mang Bang_arrCSCha
    for (i = 0; i < Bang_arrCSCha.length; i++) {
        Bang_arrCSCha[i] = parseInt(Bang_arrCSCha[i]);
    }

    //Sua lai mang Bang_arrLaHangCha
    for (i = 0; i < Bang_nH; i++) {
        Bang_arrHangDaXoa.push(false);
    }

    //Sua lai mang Bang_arrLaHangCha
    for (i = 0; i < Bang_nH; i++) {
        Bang_arrLaHangCha[i] = (Bang_arrLaHangCha[i] == "1") ? true : false;
    }

    //Sửa lại mảng giá trị
    Bang_arrHienThi = new Array();
 
    for (i = 0; i < Bang_nH; i++) {
        Bang_arrHienThi.push(new Array());
        for (j = 0; j < Bang_nC; j++) {
            Bang_arrHienThi[i].push("");
            switch (Bang_arrType[j]) {
                case 1:
                    if (Bang_arrGiaTri[i][j] == '' || isNaN(Bang_arrGiaTri[i][j])) {
                        Bang_arrGiaTri[i][j] = 0;
                    }
                    else {
                        Bang_arrGiaTri[i][j] = parseFloat(Bang_arrGiaTri[i][j]);
                    }
                    break;

                case 2:
                    if (Bang_arrGiaTri[i][j] == "1" || Bang_arrGiaTri[i][j].toUpperCase() == "TRUE") {
                        Bang_arrGiaTri[i][j] = "1";
                    }
                    else {
                        Bang_arrGiaTri[i][j] = "0";
                    }
                    break;
            }
            //Cập nhập kiểu mảng Bang_arrEdit
            if (Bang_arrEdit[i][j] == "1") {
                Bang_arrEdit[i][j] = true;
            }
            else {
                Bang_arrEdit[i][j] = false;
            }

            if (Bang_arrThayDoi[i][j] == "1") {
                Bang_arrThayDoi[i][j] = true;
            }
            else {
                Bang_arrThayDoi[i][j] = false;
            }
        }
    }
}

//Ánh xạ cột c trong Dữ liệu sang cột trong bảng Slide
function AnhXaCot_DuLieu_Slide(c) {
    var vR = c - Bang_nC_Fixed;
    if (0 <= vR && vR < Bang_nC_Slide)
        return vR;
    return -1;
}

//Ánh xạ cột c trong Slide sang cột trong bảng dữ liệu
function AnhXaCot_Slide_DuLieu(c) {
    return c + Bang_nC_Fixed;
}

//Ánh xạ cột c trong Dữ liệu sang cột trong bảng Fixed
function AnhXaCot_DuLieu_Fixed(c) {
    if (c < Bang_nC_Fixed)
        return c;
    return -1;
}

//Ánh xạ cột c trong Slide sang cột trong bảng dữ liệu
function AnhXaCot_Fixed_DuLieu(c) {
    return c;
}

///Ham khoi tao Bang
function Bang_Ready() {
    $('#' + BangDuLieuID_Slide + ' td').each(function () {
        Bang_keys.event.action(this, function (nCell) {
            KeyTable_bClicked = false;
            var h0 = nCell.parentNode.rowIndex + Bang_Viewport_hMin;
            var c0 = AnhXaCot_Slide_DuLieu(nCell.cellIndex);

            //Nếu cột không được nhập thì bỏ qua
            if (Bang_arrCotDuocPhepNhap[c0] == false) {
                return;
            }

            //Neu o khong duoc nhap thi bo qua
            if (Bang_arrEdit[h0][c0] == false) {
                return;
            }

            //Goi ham BeforeEdit neu co, Neu ham tra lai gia tri la FALSE thi bo qua
            var fnBeforeEdit = window['Bang_onCellBeforeEdit'];
            if (typeof fnBeforeEdit == 'function') {
                if (fnBeforeEdit(h0, c0) == false) {
                    return;
                }
            }

            //Luu lai gia tri truoc khi nhap de su dung lai neu gia tri nhap vao bi sai
            Bang_GiaTriO_BeforEdit = Bang_arrGiaTri[h0][c0];

            /*Xac dinh kieu nhap du lieu
            0:Kieu xau; 1: Kieu so; 2:checkbox;*/
            var strType = "text";
            switch (Bang_arrType[c0]) {
                case 1:
                    strType = "number";
                    break;

                case 2:
                    var checkbox_value = Bang_arrGiaTri[h0][c0];
                    if (checkbox_value == "1") {
                        checkbox_value = "0";
                    }
                    else {
                        checkbox_value = "1";
                    }
                    Bang_GanGiaTriThatChoO(h0, c0, checkbox_value);
                    var checkbox_fn = window['Bang_onCellAfterEdit'];
                    if (typeof checkbox_fn == 'function') {
                        checkbox_fn(h0, c0);
                    }
                    Bang_keys.fnSetFocusNextCell();
                    //Truong hop da nhap xong khong can xu ly tiep
                    return;

                case 3:
                    strType = "autocomplete";
                    break;
            }

            /* Khoa bang khong cho phep nhap o khac */
            Bang_keys.block = true;


            /* Ham duoc goi khi nhap xong du lieu cua o */
            $(nCell).editable(function (sVal) {
                var h = nCell.parentNode.rowIndex + Bang_Viewport_hMin;
                var c = AnhXaCot_Slide_DuLieu(nCell.cellIndex);
                var okCoChuyenDenOTiep = true;
                if (Bang_arrType[c0] == 3) {
                    $("#txtONhapDuLieu_Autocomplete").autocomplete("close");
                }
                if (Bang_GanGiaTriO(h, c, sVal)) {
                    var fn = window['Bang_onCellAfterEdit'];
                    if (typeof fn == 'function') {
                        if (fn(h, c) == false) {
                            okCoChuyenDenOTiep = false;
                        }
                    }
                }
                Bang_keys.block = false;
                if (KeyTable_bClicked == false && okCoChuyenDenOTiep) {
                    Bang_keys.fnSetFocusNextCell();
                }
                KeyTable_bClicked = false;
                sVal = Bang_LayDuLieuHienThiCuaO(h, c);
                return sVal;
            }, {
                "onblur": 'submit',
                "type": strType,
                "onreset": function () {
                    /* Unblock KeyTable, but only after this 'esc' key event has finished. Otherwise
                    * it will 'esc' KeyTable as well
                    */
                    setTimeout(function () { Bang_keys.block = false; }, 0);
                }
            });

            /* Dispatch click event to go into edit mode - Saf 4 needs a timeout... */
            setTimeout(function () { $(nCell).click(); }, 0);
        });
    });
}


var Bang_DangLuuThongTin = false;
function Bang_HamTruocKhiKetThuc(iAction) {
    ShowPopupThucHien();
    if ((Bang_ChiDoc == false && Bang_DangLuuThongTin == false)) {
        var fn = window[Bang_ID + '_HamTruocKhiKetThuc'];
        if (typeof fn == 'function') {
            if (fn(iAction) == false) {
                return false;
            }
        }
        Bang_DangLuuThongTin = true;

        Bang_ShowCloseDialog();

        if (document.getElementById("btnXacNhanGhi")) {
            Bang_GanMangGiaTri_Bang_arrGiaTri();
            if (document.getElementById("idAction")) document.getElementById("idAction").value = iAction;
            document.getElementById("btnXacNhanGhi").click();
        }
    }

    return true;
}

function Bang_GanMangGiaTri_Bang_arrGiaTri() {
    var strTG = "", strThayDoi = "";
    var i, j;
    //Luu gia tri cua Bang_arrGiaTri, Bang_arrThayDoi
    for (i = 0; i < Bang_nH; i++) {
        if (i > 0) {
            strTG += Bang_DauCachHang;
            strThayDoi += Bang_DauCachHang;
        }
        for (j = 0; j < Bang_nC; j++) {
            if (j > 0) {
                strTG += Bang_DauCachO;
                strThayDoi += Bang_DauCachO;
            }
            if (Bang_arrThayDoi[i][j]) {
                strTG += Bang_arrGiaTri[i][j];
                strThayDoi += "1";
            }
            else {
                strTG += "";
                strThayDoi += "0";
            }
        }
    }
    document.getElementById("idXauGiaTriChiTiet").value = strTG;
    document.getElementById("idXauDuLieuThayDoi").value = strThayDoi;

    //Luu lai bang Bang_arrMaHang, Bang_arrLaHangCha;
    var strMaHang = "", strLaHangCha = "";
    for (i = 0; i < Bang_nH; i++) {
        if (i > 0) {
            strMaHang += ",";
            strLaHangCha += ",";
        }
        strMaHang += Bang_arrMaHang[i];
        if (Bang_arrLaHangCha[i]) {
            strLaHangCha += "1";
        }
        else {
            strLaHangCha += "0";
        }
    }
    document.getElementById("idXauLaHangCha").value = strLaHangCha;
    document.getElementById("idXauMaCacHang").value = strMaHang;

    //Luu lai bang Bang_arrMaCot;
    var strMaCot = "";
    for (i = 0; i < Bang_nC; i++) {
        if (i > 0) {
            strMaCot += ",";
        }
        strMaCot += Bang_arrMaCot[i];
    }
    document.getElementById("idXauMaCacCot").value = strMaCot;

    if (document.getElementById("idXauCacHangDaXoa")) {
        var strMaHang_DaXoa = "";
        for (i = 0; i < Bang_arrHangDaXoa.length; i++) {
            if (i > 0) {
                strMaHang_DaXoa += ",";
            }
            strMaHang_DaXoa += Bang_arrHangDaXoa[i] ? '1' : '0';
        }
        document.getElementById("idXauCacHangDaXoa").value = strMaHang_DaXoa;
    }
}

function Bang_GTBangKhac(TenTruong) {
    return 0;
}

function Bang_LayMang2ChieuGiaTri(id) {
    var arr = new Array();
    var strTG = document.getElementById(id).value;
    if (strTG != "") {
        var arrTG = strTG.split(Bang_DauCachHang);
        for (var i = 0; i < arrTG.length; i++) {
            arr[i] = arrTG[i].split(Bang_DauCachO);
        }
    }
    return arr;
}

function Bang_LayMang1ChieuGiaTri(id) {
    var arr = new Array();
    if (document.getElementById(id)) {
        var strTG = document.getElementById(id).value;
        if (strTG != "") {
            arr = strTG.split(",");
        }
    }
    return arr;
}


/* Hàm Bang_DinhDangHienThiO
*   - Mục đích: định dạng dữ liệu của ô (h,c) với kiểu dữ liệu tương ứng
*/
function Bang_DinhDangHienThiO(h, c) {
    var gt = Bang_arrGiaTri[h][c];
    var vR = Bang_arrGiaTri[h][c];

    /*0:Kieu xau; 1: Kieu so; 2:checkbox;*/
    switch (Bang_arrType[c]) {
        case 1:
            if (gt == 0) {
                vR = "";
            }
            else {
                var nFixed = 0;
                if (Bang_arrFormat && Bang_arrFormat[c] != '') {
                    nFixed = parseInt(Bang_arrFormat[c]);
                }
                vR = FormatNumber(gt, nFixed);
            }
            break;
        case 4:
            var strFormat = Bang_arrFormat[c];
            vR = FormatDatetime(gt, strFormat);
            break;
    }
    return vR;
}

/* Hàm Bang_GanGiaTriO
*   - Mục đích: hàm được gọi sau khi nhập dữ liệu của ô (h,c), cần gán giá trị thật vào ô (h,c)
*/
function Bang_GanGiaTriO(h, c, GiaTri) {
    var value;
    if (h > -1 && c > -1) {
        value = GiaTri;
        /*0:Kieu xau; 1: Kieu so; 2:checkbox;*/
        switch (Bang_arrType[c]) {
            case 1:
                if (IsNumber(GiaTri) == false) {
                    value = 0;
                }
                else {
                    value = ParseNumber(GiaTri);
                }
                break;

            case 4:
                var strFormat = Bang_arrFormat[c];
                var dateTG = Date_GetDateTimeFromText(value, strFormat);
                value = Date_GetStringDatetime(dateTG, "yyyy:MM:dd:HH:mm:ss");
                //value = Date_GetStringDatetime(dateTG, "dd/MM/yyyy HH:mm:ss");
                break;
        }

        //Kiểm tra giá trị gán mới = giá trị hiện tại thì bỏ qua        
        if (Bang_arrGiaTri[h][c] == value) {
            return false;
        }
        return Bang_GanGiaTriThatChoO(h, c, value);
    }
    return true;
}

/* Hàm Bang_GanGiaTriThatChoO
*   - Mục đích: hàm gán giá trị thật của ô (h,c); đồng thời chỉnh lại giá trị của mảng arrThayDoi và arrHienThi
*/
function Bang_GanGiaTriThatChoO(h, c, GiaTri) {
    if (h > -1 && c > -1) {
        switch (Bang_arrType[c]) {
            case 1:
                if (GiaTri != "" && isNaN(GiaTri) == false) {
                    GiaTri = parseFloat(GiaTri);
                }
                else {
                    GiaTri = 0;
                }
                break;

            default:
                break;
        }
        if (Bang_arrGiaTri[h][c] != GiaTri) {
            var GiaTriCu = Bang_arrGiaTri[h][c];
            Bang_arrThayDoi[h][c] = true;
            Bang_arrGiaTri[h][c] = GiaTri;
            Bang_HienThiDuLieuO(h, c);

            if (Bang_arrMaCot[c] == "sMauSac" || Bang_arrMaCot[c] == "sFontColor" || Bang_arrMaCot[c] == "sFontBold") {
                Bang_HienThiDuLieu();
            }

            //Gọi sự kiện có thay đổi giá trị của ô
            Bang_onCellValueChanged(h, c, GiaTriCu);
//            if (Bang_nH == 1) {
//                if (Bang_arrGiaTri[h][7] != 0) {//KIEM TRA TRUONG TIEN CO KHAC RONG KHONG
//                    Bang_DaThayDoi = true;
//                }
//                else {
//                    Bang_DaThayDoi = false;
//                }
//            }
//            else {
                Bang_DaThayDoi = true;
            //}
            return true;
        }
    }
    return false;
}

function Bang_GanGiaTriThatChoO_colName(h, colName, value) {
    var c = Bang_arrCSMaCot[colName];
    return Bang_GanGiaTriThatChoO(h, c, value);
}

function Bang_LayChiSo(arr, id) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] == id) {
            return i;
        }
    }
    return -1;
}


function Bang_LayDuLieuHienThiCuaO(h, c) {
    var GTHienThi = Bang_arrHienThi[h][c];
    switch (Bang_arrType[c]) {
        case 0:
        case 3:
            //checkbox
            Bang_arrHienThi[h][c] = Bang_arrGiaTri[h][c];
            GTHienThi = '<span title="' + Bang_arrGiaTri[h][c] + '" style="width:' + Bang_arrDoRongCot[c] + 'px;">' + Bang_arrGiaTri[h][c] + '</span>';
            //GTHienThi = Bang_arrGiaTri[h][c];
            break;

        case 2:
            //checkbox
            Bang_arrHienThi[h][c] = Bang_arrGiaTri[h][c];

            var checkbox_value = Bang_arrGiaTri[h][c];
            if (checkbox_value == "1") {
                GTHienThi = '<div class="check"></div>';
            }
            else {
                GTHienThi = '';
            }
            break;

        default:
            Bang_arrHienThi[h][c] = Bang_DinhDangHienThiO(h, c);
            GTHienThi = Bang_arrHienThi[h][c];
            break;
    }
    return GTHienThi;
}

/* Hàm Bang_HienThiDuLieuO
*   - Mục đích: hàm hiển thị giá trị của ô (h,c) ra bảng Fixed, Slide nếu có
*/
function Bang_HienThiDuLieuO(h, c) {
    var GTHienThi = Bang_LayDuLieuHienThiCuaO(h, c);
    //Kiểm tra hàng có thuộc Viewport hay không
    if (0 <= h - Bang_Viewport_hMin && h - Bang_Viewport_hMin < Bang_Viewport_N) {
        var i = h - Bang_Viewport_hMin;
        var j = AnhXaCot_DuLieu_Fixed(c);
        if (j >= 0) {
            Bang_arrCell_Fixed[i][j].innerHTML = GTHienThi;
        }
        j = AnhXaCot_DuLieu_Slide(c);
        if (j >= 0) {
            Bang_arrCell_Slide[i][j].innerHTML = GTHienThi;
        }
    }
}

/* Hàm Bang_HienThiDuLieu
*   - Mục đích: hàm hiển thị giá trị của bảng ra bảng Fixed, Slide nếu có
*/
function Bang_HienThiDuLieu() {
    if (Bang_Viewport_N == 0) return;

    var n = 0;
    var hMin = Bang_Viewport_hMin;
    var hMax = Bang_Viewport_hMin + Bang_Viewport_N;
    var tgHeight;

    n = 0;
    tgHeight = (hMin + 1) * Bang_DoRongHang;
    $(Bang_arrRow_Slide[n]).css('height', tgHeight);
    $(Bang_arrRow_Fixed[n]).css('height', tgHeight);


    n = Bang_Viewport_N - 1;
    tgHeight = (Bang_nH - hMax + 1) * Bang_DoRongHang;
    $(Bang_arrRow_Slide[n]).css('height', tgHeight);
    $(Bang_arrRow_Fixed[n]).css('height', tgHeight);

    var c_sMauSac = Bang_arrCSMaCot["sMauSac"];
    var c_sFontColor = Bang_arrCSMaCot["sFontColor"];
    var c_sFontBold = Bang_arrCSMaCot["sFontBold"];

    n = 0;
    for (var h = hMin; h < hMax; h++) {
        //Hàng cha
        if (Bang_arrLaHangCha[h]) {
            if (Bang_arrRowBold[n] == false) {
                Bang_arrRowBold[n] = true;
                $(Bang_arrRow_Slide[n]).addClass('hangcha');
                $(Bang_arrRow_Fixed[n]).addClass('hangcha');
            }
        }
        else {
            if (Bang_arrRowBold[n]) {
                Bang_arrRowBold[n] = false;
                $(Bang_arrRow_Slide[n]).removeClass('hangcha');
                $(Bang_arrRow_Fixed[n]).removeClass('hangcha');
            }
        }

        //Hàng đã xóa
        if (Bang_arrHangDaXoa[h]) {
            if (c_sMauSac != null && c_sMauSac >= 0) {
                $(Bang_arrRow_Slide[n]).css('background-color', '');
                $(Bang_arrRow_Fixed[n]).css('background-color', '');
            }
            if (Bang_arrRowDaXoa[n] == false) {
                Bang_arrRowDaXoa[n] = true;
                $(Bang_arrRow_Slide[n]).addClass('removed');
                $(Bang_arrRow_Fixed[n]).addClass('removed');
            }
        }
        else {
            if (Bang_arrRowDaXoa[n]) {
                Bang_arrRowDaXoa[n] = false;
                $(Bang_arrRow_Slide[n]).removeClass('removed');
                $(Bang_arrRow_Fixed[n]).removeClass('removed');
            }
            if (c_sMauSac != null && c_sMauSac >= 0) {
                var bgcolor = Bang_arrGiaTri[h][c_sMauSac];
                $(Bang_arrRow_Slide[n]).css('background-color', bgcolor);
                $(Bang_arrRow_Fixed[n]).css('background-color', bgcolor);
            }
        }

        //Font Bold, Color
        if (c_sFontColor != null && c_sFontColor >= 0) {
            var font_color = Bang_arrGiaTri[h][c_sFontColor];
            $(Bang_arrRow_Slide[n]).css('color', font_color);
            $(Bang_arrRow_Fixed[n]).css('color', font_color);
        }
        if (c_sFontBold != null && c_sFontBold >= 0) {
            var font_bold = Bang_arrGiaTri[h][c_sFontBold];
            $(Bang_arrRow_Slide[n]).css('font-weight', font_bold);
            $(Bang_arrRow_Fixed[n]).css('font-weight', font_bold);
        }

        //Điền giá trị các ô của hàng
        for (var c = 0; c < Bang_nC; c++) {
            Bang_HienThiDuLieuO(h, c);
        }
        n++;
    }
}

/*
* Function: Bang_SetPosition
* Purpose:  Set table'position by scrolltop
* Returns:  nothing
* Inputs:   int:y - scrolltop
* Notes:    
*/
function Bang_SetPosition(y) {
    if (Bang_Viewport_N == 0) return;
    var yMin = Bang_Viewport_hMin * Bang_DoRongHang;
    var yMax = yMin + 30 * Bang_DoRongHang;
    if (yMin <= y && y <= yMax) return;

    var cs = parseInt(y / Bang_DoRongHang);

    if (yMax < y) {
        cs = cs + 20;
    }
    Bang_keys.fnSetViewportPosition(cs);
}

function Bang_LayGiaTri(h, colName) {
    var c = Bang_arrCSMaCot[colName];
    if (c != null && c >= 0) {
        return Bang_arrGiaTri[h][c];
    }
    return null;
}

function Bang_GanGiaTri(h, colName, value) {
    var c = Bang_arrCSMaCot[colName];
    return Bang_GanGiaTriThatChoO(h, c, value);
}