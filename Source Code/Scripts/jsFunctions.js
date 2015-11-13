var Bang_CloseDialog_Block = false;
function Bang_ShowCloseDialog(strThongBao) {
//    if (parent) {
//        parent.Bang_ShowCloseDialog(strThongBao);
//    }
//    else {
//        Bang_CloseDialog_Block = true;
//        if (typeof strThongBao == "undefined") {
//            strThongBao = 'Đang đọc dữ liệu...';
//        }
//        if (document.getElementById("idDialogClose")) {
//            $("#idDialogClose").dialog({
//                width: 220,
//                height: 50,
//                modal: true,
//                title: strThongBao,
//                close: function (event, ui) {
//                    if (Bang_CloseDialog_Block) {
//                        Bang_ShowCloseDialog();
//                    }
//                }
//            });
//        }
//    }
}

function Bang_HideCloseDialog() {
    Bang_CloseDialog_Block = false;
    $("#idDialogClose").dialog("close");
}

function Dialog_close(ctlID) {
    $("#idDialog").dialog("close");
    return false;
}

function ImportJavascript(v) {
    var scriptElt = document.createElement('script');
    scriptElt.type = 'text/javascript';
    scriptElt.text = v;
    document.getElementsByTagName('head')[0].appendChild(scriptElt);
}

function CallPrint(strid) {
    var prtContent = document.getElementById(strid);
    var WinPrint = window.open('', '', 'width=800,height=650,resizable=1,scrollbars=1,left=100');
    WinPrint.document.write(prtContent.innerHTML);
    WinPrint.document.close();
    WinPrint.focus();    
    WinPrint.print();
    WinPrint.close();
    return false; 
}

function firstPage(strid) {
    document.getElementById(strid).ClientController.ActionHandler('PageNav', 1);
    document.getElementById("txtChangePage").value = 1;
    return false;
}
function previousPage(strid) {
    if (document.getElementById(strid).ClientController.CurrentPage > 1) {
        document.getElementById(strid).ClientController.ActionHandler('PageNav', document.getElementById(strid).ClientController.CurrentPage - 1);
        document.getElementById("txtChangePage").value = document.getElementById(strid).ClientController.CurrentPage - 1;
        return false;
    }
}
function nextPage(strid) {
    if (document.getElementById(strid).ClientController.CurrentPage < document.getElementById(strid).ClientController.TotalPages) {
        document.getElementById(strid).ClientController.ActionHandler('PageNav', document.getElementById(strid).ClientController.CurrentPage + 1);
        document.getElementById("txtChangePage").value = document.getElementById(strid).ClientController.CurrentPage + 1;
        return false;
    }
}
function lastPage(strid) {
    document.getElementById(strid).ClientController.ActionHandler('PageNav', document.getElementById(strid).ClientController.TotalPages);
    document.getElementById("txtChangePage").value = document.getElementById(strid).ClientController.TotalPages;
    return false;
}

function getCaretPosition(objTextBox) {

    if (window.event) {
        var objTextBox = window.event.srcElement;
    }

    var i = objTextBox.value.length;

    if (objTextBox.createTextRange) {
        objCaret = document.selection.createRange().duplicate();
        while (objCaret.parentElement() == objTextBox &&
                  objCaret.move("character", 1) == 1) --i;
    }
    return i;
}

function setSelectionRange(input, selectionStart, selectionEnd) {
    if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }
    else if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    }
}
//kiem tra dinh dang ngay thang dd/mm/yyy
//Dau vao la controlid
function isDate(obj) {
    var dateStr = obj.value;
    var datePat = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
    var matchArray = dateStr.match(datePat); // is the format ok?
    var str = dateStr.toString();

    if (matchArray == null) {
        obj.value = '';
    }
}
//BQP Functions
function TinhDiemChuyenCan(MaSinhVien, DiemMax, ThangDiem) {
    var XauMaCacCot = document.getElementById('idXauMaCacCot').value;
    var SoPhutCacCot = document.getElementById('idSoPhutCacCot').value;
    var arrMaCot = XauMaCacCot.split(',');
    var arrSoPhut = SoPhutCacCot.split(',');
    var Diem = DiemMax;
    var giatri, i, SoPhutVang, SoPhutBuoiHoc;
    var MaO;
    var DemCoPhep = 0, DemKhongPhep = 0, TinhHeSoTheoPhut = 0;
    for (i = 0; i < arrMaCot.length - 2; i++) {
        SoPhutVang = 0;
        SoPhutBuoiHoc = 10;
        MaO = MaSinhVien + "_" + arrMaCot[i];
        giatri = "";
        if (document.getElementById(MaO)) {
            if (typeof document.getElementById(MaO).value != "undefined") {
                giatri = document.getElementById(MaO).value;
            }
            else {
                giatri = document.getElementById(MaO).innerHTML;
            }
        }
        giatri = Trim(giatri.toUpperCase());
        switch (giatri) {
            case "":
                break;

            case "p":
            case "P":
                document.getElementById(MaO).value = "P";
                DemCoPhep++;
                break;

            case "0":
            case "o":
            case "O":
                document.getElementById(MaO).value = "O";
                DemKhongPhep++;
                break;

            default:
                if (isFinite(giatri)) {
                    SoPhutVang = parseFloat(giatri);
                    SoPhutBuoiHoc = parseFloat(arrSoPhut[i]);
                    TinhHeSoTheoPhut += SoPhutVang / SoPhutBuoiHoc;
                }
                else {
                    document.getElementById(MaO).value = "";
                }
        }
    }
    Diem = DiemMax - ThangDiem * (DemCoPhep / 2 + DemKhongPhep + TinhHeSoTheoPhut);
    if (Diem < 0) {
        Diem = 0;
    }
    return Diem;
}

function TinhDiemTru(Diem, SoNgayMuon1, SoNgayMuon2, NgayNopMuonToiDa) {
    var DiemCon = 0;

    if (typeof Diem == "undefined") {
        Diem = 0;
    }
    DiemCon = Diem;
    if (typeof SoNgayMuon1 == "undefined") {
        SoNgayMuon1 = 0;
    }
    if (typeof SoNgayMuon2 == "undefined") {
        SoNgayMuon2 = 0;
    }
    if (typeof NgayNopMuonToiDa == "undefined" || NgayNopMuonToiDa == "") {
        NgayNopMuonToiDa = 7;
    }
    if (SoNgayMuon1 == "") {
        SoNgayMuon1 = NgayNopMuonToiDa + 1;
    }
    if (SoNgayMuon2 == "") {
        SoNgayMuon2 = NgayNopMuonToiDa + 1;
    }
    if (SoNgayMuon1 > NgayNopMuonToiDa) {
        DiemCon = 0;
    }
    else {
        DiemCon = DiemCon - SoNgayMuon1;
    }
    if (SoNgayMuon2 > NgayNopMuonToiDa) {
        DiemCon = 0;
    }
    else {
        DiemCon = DiemCon - SoNgayMuon2;
    }
    if (DiemCon < 0) {
        DiemCon = 0;
    }
    return DiemCon;
}

function NhapTheoNhom(MaCotNhom, MaCotNhomTruong, MaCotDuLieu) {
    var XauMaCacHang = document.getElementById('idXauMaCacHang').value;
    var arrMaHang = XauMaCacHang.split(',');
    var MaSinhVien;
    var id1, id2, id3;
    var ctl1, ctl2, ctl3, i, j;
    for (i = 0; i < arrMaHang.length - 1; i++) {
        MaSinhVien = arrMaHang[i];
        id1 = MaSinhVien + "_" + MaCotNhomTruong;
        id2 = MaSinhVien + "_" + MaCotDuLieu;
        id3 = MaSinhVien + "_" + MaCotNhom;
        ctl1 = document.getElementById(id1);
        ctl2 = document.getElementById(id2);
        ctl3 = document.getElementById(id3);
        if (ctl1 && (ctl1.className == "check" || ctl1.checked)) {
            //ctl2.disabled = false;
        }
        else {
            //ctl2.disabled = true;
            for (j = 0; j < arrMaHang.length - 1; j++) {
                if (i != j &&
                    (document.getElementById(arrMaHang[j] + "_" + MaCotNhomTruong).className == "check" || document.getElementById(arrMaHang[j] + "_" + MaCotNhomTruong).checked)) {
                    if (typeof document.getElementById(arrMaHang[j] + "_" + MaCotNhom).value == "undefined") {
                        if (document.getElementById(arrMaHang[j] + "_" + MaCotNhom).innerHTML == ctl3.innerHTML) {
                            ctl2.alt = arrMaHang[j] + "_" + MaCotDuLieu;
                        }
                    }
                    else {
                        if (document.getElementById(arrMaHang[j] + "_" + MaCotNhom).value == ctl3.value) {
                            ctl2.alt = arrMaHang[j] + "_" + MaCotDuLieu;
                        }
                    }
                }
            }
        }
    }
}

function XacDinhDiemChu(DiemSo) {
    var vR = "";
    if (DiemSo != "" && isFinite(DiemSo)) {
        vR = "F";
        if (DiemSo >= 95) {
            vR = "A";
        }
        else if (DiemSo >= 90) {
            vR = "A-";
        }
        else if (DiemSo >= 87) {
            vR = "B+";
        }
        else if (DiemSo >= 84) {
            vR = "B";
        }
        else if (DiemSo >= 80) {
            vR = "B-";
        }
        else if (DiemSo >= 77) {
            vR = "C+";
        }
        else if (DiemSo >= 74) {
            vR = "C";
        }
        else if (DiemSo >= 70) {
            vR = "C-";
        }
    }
    return vR;
}

function XacDinhNgayUpload(TenFileUpdate) {
    if (typeof TenFileUpdate == "undefined") TenFileUpdate = "";
    var vR = "";
    var arr1 = TenFileUpdate.split('/');
    if (arr1.length > 1) {
        var arr2 = arr1[arr1.length - 1].split('_');
        var Nam = arr2[0];
        var Thang = arr2[1];
        var Ngay = arr2[2];
        var Gio = arr2[3];
        var Phut = arr2[4];

        vR = Ngay + "/" + Thang + "/" + Nam + " " + Gio + ":" + Phut;
    }
    return vR;
}

function XacDinhKhoangCachNgay(Xau1, Xau2) {
    if (typeof Xau1 == "undefined") {
        Xau1 = "";
    }
    if (typeof Xau2 == "undefined") {
        Xau2 = "";
    }
    var minutes = 1000 * 60;
    var hours = minutes * 60;
    var days = hours * 24;
    var years = days * 365;
    var arr1, arr2;
    var d1, M1, y1, H1, m1, Total1;
    var d2, M2, y2, H2, m2, Total2;
    var date1, date2;
    var Total;

    if (Xau1.length > 0 && Xau2.length > 0) {
        arr1 = Xau1.split(" ");
        arr2 = arr1[0].split("/");
        d1 = parseInt(arr2[0]);
        M1 = parseInt(arr2[1]);
        y1 = parseInt(arr2[2]);
        H1 = 0;
        m1 = 0;
        if (arr1.length > 1) {
            arr2 = arr1[1].split(":");
            H1 = parseInt(arr2[0]);
            m1 = parseInt(arr2[1]);
        }
        date1 = new Date(y1, M1, d1, H1, m1);
        Total1 = date1.getTime();

        arr1 = Xau2.split(" ");
        arr2 = arr1[0].split("/");
        d2 = parseInt(arr2[0]);
        M2 = parseInt(arr2[1]);
        y2 = parseInt(arr2[2]);
        H2 = 0;
        m2 = 0;
        if (arr1.length > 1) {
            arr2 = arr1[1].split(":");
            H2 = parseInt(arr2[0]);
            m2 = parseInt(arr2[1]);
        }
        date2 = new Date(y2, M2, d2, H2, m2);
        Total2 = date2.getTime();

        Total = Total2 - Total1;
        if (Total < 0) Total = 0;
        return Math.round(Total / days);
    }
    return 0;
}

function SetDisabledAll(value) {
    var XauMaCacCot = document.getElementById('idXauMaCacCot').value;
    var arrMaCot = XauMaCacCot.split(',');
    for (i = 0; i < arrMaCot.length - 1; i++) {
        var cckGhi = document.getElementById(arrMaCot[i]+'_Ghi');
        if (cckGhi && cckGhi.style.display != "none") {
            cckGhi.checked = value;
            SetDisabled(arrMaCot[i], !value);
        }
    }
}

function SetDisabled(MaCot, value) {
    var XauMaCacHang = document.getElementById('idXauMaCacHang').value;
    var XauChiDocCacHang = document.getElementById('idXauChiDocCacHang').value;
    var arrMaHang = XauMaCacHang.split(',');
    var arrChiDocHang = XauChiDocCacHang.split(',');
    var MaHang;
    var id1, id2;
    var ctl1, ctl2, i;
    for (i = 0; i < arrMaHang.length - 1; i++) {
        if (arrChiDocHang[i] == "0") {
            MaHang = arrMaHang[i];
            id1 = MaHang + "_" + MaCot+ "_show";
            ctl1 = document.getElementById(id1);
            if (ctl1==null) {
                id1 = MaHang + "_" + MaCot ;
                ctl1 = document.getElementById(id1);
            }
            if (ctl1.alt == "" || ctl1.tagName=="TEXTAREA") {
                ctl1.disabled = value;
                id2 = id1 + "_btn";
                ctl2 = document.getElementById(id2);
                if (ctl2) {
                    ctl2.disabled = value;
                }
            }
        }
    }
}

var iSTT = 0;
var iXepHang = 0;
var XepHang_oGiaTriCu = -1;
function STT(GiaTriMoi) {
    iSTT++;
    if (typeof GiaTriMoi == "undefined") {
        GiaTriMoi = 0;
    }
    if (GiaTriMoi != XepHang_oGiaTriCu) {
        iXepHang = iSTT;
        XepHang_oGiaTriCu = GiaTriMoi;
    }
    return iXepHang;
}


function AnHienHang(cs, sohangtieude) {
    var strID = "btn" + cs;
    var control = document.getElementById(strID);
    var strTG = 'none';
    if (control.innerHTML == "+") {
        control.innerHTML = "-";
        strTG = 'block';
    }
    else {
        control.innerHTML = "+";
        strTG = 'none';
    }
    HienThiTrangThai(cs, strTG, sohangtieude);
    return false;
}

function HienThiTrangThai(cs, GiaTriBatBuoc, sohangtieude) {
    var GiaTri = document.getElementById('btn' + cs).innerHTML;
    var strTG = 'none';
    if (GiaTri == '+') {
        strTG = 'none';
    }
    else {
        if (GiaTriBatBuoc != "none") {
            strTG = 'block';
        }
    }
    var arrMa = document.getElementById('txtXauMucCuaHang').value.split(',');
    var i;
    var gt = parseInt(arrMa[cs]);
    for (i = cs + 1; i < arrMa.length - 1; i++) {
        if (parseInt(arrMa[i]) == gt + 1) {
            document.getElementById('BangDuLieu').rows[i + sohangtieude].style.display = strTG;
            if (document.getElementById('btn' + i) != null) {
                HienThiTrangThai(i, strTG, sohangtieude);
            }
        }
        else if (parseInt(arrMa[i]) <= gt) {
            break;
        }
    }
}

function LayGiaTriCon(cs, sohangtieude, TenChecBoxChon) {
    var arrMa = document.getElementById('txtXauMucCuaHang').value.split(',');
    var i;
    var gt = parseInt(arrMa[cs]);
    if (document.getElementById(TenChecBoxChon + '_' + cs).checked == true) {
        for (i = cs + 1; i < arrMa.length - 1; i++) {
            if (parseInt(arrMa[i]) == gt + 1) {
                var tencheckbox = document.getElementById(TenChecBoxChon + '_' + i);
                if ((tencheckbox.type == "checkbox")) {
                    tencheckbox.checked = true;
                    if (tencheckbox != null) {
                        LayGiaTriCon(i, sohangtieude, TenChecBoxChon);
                    }
                }
            }
            else if (parseInt(arrMa[i]) <= gt) {
                break;
            }
        }
    }
    else {
        for (i = cs + 1; i < arrMa.length - 1; i++) {
            if (parseInt(arrMa[i]) == gt + 1) {
                var tencheckbox = document.getElementById(TenChecBoxChon + '_' + i);
                if ((tencheckbox.type == "checkbox")) {
                    tencheckbox.checked = false;
                    if (tencheckbox != null) {
                        LayGiaTriCon(i, sohangtieude, TenChecBoxChon);
                    }
                }
            }
            else if (parseInt(arrMa[i]) <= gt) {
                break;
            }
        }
    }
}

function disableEnterKey(e) {
    var key;

    if (window.event) {
        key = window.event.keyCode;     //IE
    }
    else {
        key = e.which;    //firefox
    }

    if (key == 13) {
        var id = e.srcElement.id;
        selectNextControl(id);
        return false;
    }
    else{
        return true;
    }
    return true;
}

var strControlIDs = "";
var strControlShows = "";

function selectNextControl(id) {
    if (typeof id != "undefined") {
        var arrID = strControlIDs.split(',');
        var arrShows = strControlShows.split(',');
        var ctlID = "";
        for (var i = 0; i < arrID.length; i++) {
            if (id == arrID[i]) {
                if (i < arrID.length - 1) {
                    ctlID = arrShows[i + 1];
                }
                else {
                    ctlID = arrShows[0];
                }
                break;
            }
        }
        if (ctlID != "") {
            document.getElementById(ctlID).focus();
        }
    }
}

function onfocus_txt(ID, value_default) {
    var txt = document.getElementById(ID);
    if (txt.value == value_default) {
        txt.value = '';
        txt.style.color = '#000000';
    }
}
function onblue_txt(ID, value_default) {
    var txt = document.getElementById(ID);
    if (txt.value == "" || txt.value == value_default) {
        txt.value = value_default;
        txt.style.color = '#606060';
    }
}

function fnGetHeightById(id) {
    var brow = 'mozilla';

    jQuery.each(jQuery.browser, function (i, val) {
        if (val == true) {
            brow = i.toString();
        }
    });
    if (brow == 'mozilla') {
        return $('#' + id).outerHeight();
    }
    else if (brow == 'msie') {
        return $('#' + id).innerHeight();
    }
    else {
        return $('#' + id).height();
    }
    return -1;
}

function fnGetHeightByObject(obj) {
    var brow = 'mozilla';

    jQuery.each(jQuery.browser, function (i, val) {
        if (val == true) {
            brow = i.toString();
        }
    });
    if (brow == 'mozilla') {
        return $(obj).outerHeight();
    }
    else if (brow == 'msie') {
        return $(obj).innerHeight();
    }
    else {
        return $(obj).height();
    }
    return -1;
}

function fnGetWidthById(id) {
    var brow = 'mozilla';

    jQuery.each(jQuery.browser, function (i, val) {
        if (val == true) {
            brow = i.toString();
        }
    });
    if (brow == 'mozilla') {
        return $('#' + id).outerWidth();
    }
    else if (brow == 'msie') {
        return $('#' + id).innerWidth();
    }
    else {
        return $('#' + id).width();
    }
    return -1;
}

function fnGetWidthByObject(obj) {
    var brow = 'mozilla';

    jQuery.each(jQuery.browser, function (i, val) {
        if (val == true) {
            brow = i.toString();
        }
    });
    if (brow == 'mozilla') {
        return $(obj).outerWidth();
    }
    else if (brow == 'msie') {
        return $(obj).innerWidth();
    }
    else {
        return $(obj).width();
    }
    return -1;
}

var fnDelay = (function () {
    var timer = 0;
    return function (callback, ms) {
        clearTimeout(timer);
        timer = setTimeout(callback, ms);
    };
})();



/**
* A Javascript object to encode and/or decode html characters using HTML or Numeric entities that handles double or partial encoding
* Author: R Reid
* source: http://www.strictly-software.com/htmlencode
* Licences: GPL, The MIT License (MIT)
* Copyright: (c) 2011 Robert Reid - Strictly-Software.com
*
* Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
* 
* Revision:
*  2011-07-14, Jacques-Yves Bleau: 
*       - fixed conversion error with capitalized accentuated characters
*       + converted arr1 and arr2 to object property to remove redundancy
*
* Revision:
*  2011-11-10, Ce-Yi Hio: 
*       - fixed conversion error with a number of capitalized entity characters
*
* Revision:
*  2011-11-10, Rob Reid: 
*		 - changed array format
*/

Encoder = {

    // When encoding do we convert characters into html or numerical entities
    EncodeType: "entity",  // entity OR numerical

    isEmpty: function (val) {
        if (val) {
            return ((val === null) || val.length == 0 || /^\s+$/.test(val));
        } else {
            return true;
        }
    },

    // arrays for conversion from HTML Entities to Numerical values
    arr1: ['&nbsp;', '&iexcl;', '&cent;', '&pound;', '&curren;', '&yen;', '&brvbar;', '&sect;', '&uml;', '&copy;', '&ordf;', '&laquo;', '&not;', '&shy;', '&reg;', '&macr;', '&deg;', '&plusmn;', '&sup2;', '&sup3;', '&acute;', '&micro;', '&para;', '&middot;', '&cedil;', '&sup1;', '&ordm;', '&raquo;', '&frac14;', '&frac12;', '&frac34;', '&iquest;', '&Agrave;', '&Aacute;', '&Acirc;', '&Atilde;', '&Auml;', '&Aring;', '&AElig;', '&Ccedil;', '&Egrave;', '&Eacute;', '&Ecirc;', '&Euml;', '&Igrave;', '&Iacute;', '&Icirc;', '&Iuml;', '&ETH;', '&Ntilde;', '&Ograve;', '&Oacute;', '&Ocirc;', '&Otilde;', '&Ouml;', '&times;', '&Oslash;', '&Ugrave;', '&Uacute;', '&Ucirc;', '&Uuml;', '&Yacute;', '&THORN;', '&szlig;', '&agrave;', '&aacute;', '&acirc;', '&atilde;', '&auml;', '&aring;', '&aelig;', '&ccedil;', '&egrave;', '&eacute;', '&ecirc;', '&euml;', '&igrave;', '&iacute;', '&icirc;', '&iuml;', '&eth;', '&ntilde;', '&ograve;', '&oacute;', '&ocirc;', '&otilde;', '&ouml;', '&divide;', '&oslash;', '&ugrave;', '&uacute;', '&ucirc;', '&uuml;', '&yacute;', '&thorn;', '&yuml;', '&quot;', '&amp;', '&lt;', '&gt;', '&OElig;', '&oelig;', '&Scaron;', '&scaron;', '&Yuml;', '&circ;', '&tilde;', '&ensp;', '&emsp;', '&thinsp;', '&zwnj;', '&zwj;', '&lrm;', '&rlm;', '&ndash;', '&mdash;', '&lsquo;', '&rsquo;', '&sbquo;', '&ldquo;', '&rdquo;', '&bdquo;', '&dagger;', '&Dagger;', '&permil;', '&lsaquo;', '&rsaquo;', '&euro;', '&fnof;', '&Alpha;', '&Beta;', '&Gamma;', '&Delta;', '&Epsilon;', '&Zeta;', '&Eta;', '&Theta;', '&Iota;', '&Kappa;', '&Lambda;', '&Mu;', '&Nu;', '&Xi;', '&Omicron;', '&Pi;', '&Rho;', '&Sigma;', '&Tau;', '&Upsilon;', '&Phi;', '&Chi;', '&Psi;', '&Omega;', '&alpha;', '&beta;', '&gamma;', '&delta;', '&epsilon;', '&zeta;', '&eta;', '&theta;', '&iota;', '&kappa;', '&lambda;', '&mu;', '&nu;', '&xi;', '&omicron;', '&pi;', '&rho;', '&sigmaf;', '&sigma;', '&tau;', '&upsilon;', '&phi;', '&chi;', '&psi;', '&omega;', '&thetasym;', '&upsih;', '&piv;', '&bull;', '&hellip;', '&prime;', '&Prime;', '&oline;', '&frasl;', '&weierp;', '&image;', '&real;', '&trade;', '&alefsym;', '&larr;', '&uarr;', '&rarr;', '&darr;', '&harr;', '&crarr;', '&lArr;', '&uArr;', '&rArr;', '&dArr;', '&hArr;', '&forall;', '&part;', '&exist;', '&empty;', '&nabla;', '&isin;', '&notin;', '&ni;', '&prod;', '&sum;', '&minus;', '&lowast;', '&radic;', '&prop;', '&infin;', '&ang;', '&and;', '&or;', '&cap;', '&cup;', '&int;', '&there4;', '&sim;', '&cong;', '&asymp;', '&ne;', '&equiv;', '&le;', '&ge;', '&sub;', '&sup;', '&nsub;', '&sube;', '&supe;', '&oplus;', '&otimes;', '&perp;', '&sdot;', '&lceil;', '&rceil;', '&lfloor;', '&rfloor;', '&lang;', '&rang;', '&loz;', '&spades;', '&clubs;', '&hearts;', '&diams;'],
    arr2: ['&#160;', '&#161;', '&#162;', '&#163;', '&#164;', '&#165;', '&#166;', '&#167;', '&#168;', '&#169;', '&#170;', '&#171;', '&#172;', '&#173;', '&#174;', '&#175;', '&#176;', '&#177;', '&#178;', '&#179;', '&#180;', '&#181;', '&#182;', '&#183;', '&#184;', '&#185;', '&#186;', '&#187;', '&#188;', '&#189;', '&#190;', '&#191;', '&#192;', '&#193;', '&#194;', '&#195;', '&#196;', '&#197;', '&#198;', '&#199;', '&#200;', '&#201;', '&#202;', '&#203;', '&#204;', '&#205;', '&#206;', '&#207;', '&#208;', '&#209;', '&#210;', '&#211;', '&#212;', '&#213;', '&#214;', '&#215;', '&#216;', '&#217;', '&#218;', '&#219;', '&#220;', '&#221;', '&#222;', '&#223;', '&#224;', '&#225;', '&#226;', '&#227;', '&#228;', '&#229;', '&#230;', '&#231;', '&#232;', '&#233;', '&#234;', '&#235;', '&#236;', '&#237;', '&#238;', '&#239;', '&#240;', '&#241;', '&#242;', '&#243;', '&#244;', '&#245;', '&#246;', '&#247;', '&#248;', '&#249;', '&#250;', '&#251;', '&#252;', '&#253;', '&#254;', '&#255;', '&#34;', '&#38;', '&#60;', '&#62;', '&#338;', '&#339;', '&#352;', '&#353;', '&#376;', '&#710;', '&#732;', '&#8194;', '&#8195;', '&#8201;', '&#8204;', '&#8205;', '&#8206;', '&#8207;', '&#8211;', '&#8212;', '&#8216;', '&#8217;', '&#8218;', '&#8220;', '&#8221;', '&#8222;', '&#8224;', '&#8225;', '&#8240;', '&#8249;', '&#8250;', '&#8364;', '&#402;', '&#913;', '&#914;', '&#915;', '&#916;', '&#917;', '&#918;', '&#919;', '&#920;', '&#921;', '&#922;', '&#923;', '&#924;', '&#925;', '&#926;', '&#927;', '&#928;', '&#929;', '&#931;', '&#932;', '&#933;', '&#934;', '&#935;', '&#936;', '&#937;', '&#945;', '&#946;', '&#947;', '&#948;', '&#949;', '&#950;', '&#951;', '&#952;', '&#953;', '&#954;', '&#955;', '&#956;', '&#957;', '&#958;', '&#959;', '&#960;', '&#961;', '&#962;', '&#963;', '&#964;', '&#965;', '&#966;', '&#967;', '&#968;', '&#969;', '&#977;', '&#978;', '&#982;', '&#8226;', '&#8230;', '&#8242;', '&#8243;', '&#8254;', '&#8260;', '&#8472;', '&#8465;', '&#8476;', '&#8482;', '&#8501;', '&#8592;', '&#8593;', '&#8594;', '&#8595;', '&#8596;', '&#8629;', '&#8656;', '&#8657;', '&#8658;', '&#8659;', '&#8660;', '&#8704;', '&#8706;', '&#8707;', '&#8709;', '&#8711;', '&#8712;', '&#8713;', '&#8715;', '&#8719;', '&#8721;', '&#8722;', '&#8727;', '&#8730;', '&#8733;', '&#8734;', '&#8736;', '&#8743;', '&#8744;', '&#8745;', '&#8746;', '&#8747;', '&#8756;', '&#8764;', '&#8773;', '&#8776;', '&#8800;', '&#8801;', '&#8804;', '&#8805;', '&#8834;', '&#8835;', '&#8836;', '&#8838;', '&#8839;', '&#8853;', '&#8855;', '&#8869;', '&#8901;', '&#8968;', '&#8969;', '&#8970;', '&#8971;', '&#9001;', '&#9002;', '&#9674;', '&#9824;', '&#9827;', '&#9829;', '&#9830;'],

    // Convert HTML entities into numerical entities
    HTML2Numerical: function (s) {
        return this.swapArrayVals(s, this.arr1, this.arr2);
    },

    // Convert Numerical entities into HTML entities
    NumericalToHTML: function (s) {
        return this.swapArrayVals(s, this.arr2, this.arr1);
    },


    // Numerically encodes all unicode characters
    numEncode: function (s) {

        if (this.isEmpty(s)) return "";

        var e = "";
        for (var i = 0; i < s.length; i++) {
            var c = s.charAt(i);
            if (c < " " || c > "~") {
                c = "&#" + c.charCodeAt() + ";";
            }
            e += c;
        }
        return e;
    },

    // HTML Decode numerical and HTML entities back to original values
    htmlDecode: function (s) {

        var c, m, d = s;

        if (this.isEmpty(d)) return "";

        // convert HTML entites back to numerical entites first
        d = this.HTML2Numerical(d);

        // look for numerical entities &#34;
        arr = d.match(/&#[0-9]{1,5};/g);

        // if no matches found in string then skip
        if (arr != null) {
            for (var x = 0; x < arr.length; x++) {
                m = arr[x];
                c = m.substring(2, m.length - 1); //get numeric part which is refernce to unicode character
                // if its a valid number we can decode
                if (c >= -32768 && c <= 65535) {
                    // decode every single match within string
                    d = d.replace(m, String.fromCharCode(c));
                } else {
                    d = d.replace(m, ""); //invalid so replace with nada
                }
            }
        }

        return d;
    },

    // encode an input string into either numerical or HTML entities
    htmlEncode: function (s, dbl) {

        if (this.isEmpty(s)) return "";

        // do we allow double encoding? E.g will &amp; be turned into &amp;amp;
        dbl = dbl || false; //default to prevent double encoding

        // if allowing double encoding we do ampersands first
        if (dbl) {
            if (this.EncodeType == "numerical") {
                s = s.replace(/&/g, "&#38;");
            } else {
                s = s.replace(/&/g, "&amp;");
            }
        }

        // convert the xss chars to numerical entities ' " < >
        s = this.XSSEncode(s, false);

        if (this.EncodeType == "numerical" || !dbl) {
            // Now call function that will convert any HTML entities to numerical codes
            s = this.HTML2Numerical(s);
        }

        // Now encode all chars above 127 e.g unicode
        s = this.numEncode(s);

        // now we know anything that needs to be encoded has been converted to numerical entities we
        // can encode any ampersands & that are not part of encoded entities
        // to handle the fact that I need to do a negative check and handle multiple ampersands &&&
        // I am going to use a placeholder

        // if we don't want double encoded entities we ignore the & in existing entities
        if (!dbl) {
            s = s.replace(/&#/g, "##AMPHASH##");

            if (this.EncodeType == "numerical") {
                s = s.replace(/&/g, "&#38;");
            } else {
                s = s.replace(/&/g, "&amp;");
            }

            s = s.replace(/##AMPHASH##/g, "&#");
        }

        // replace any malformed entities
        s = s.replace(/&#\d*([^\d;]|$)/g, "$1");

        if (!dbl) {
            // safety check to correct any double encoded &amp;
            s = this.correctEncoding(s);
        }

        // now do we need to convert our numerical encoded string into entities
        if (this.EncodeType == "entity") {
            s = this.NumericalToHTML(s);
        }

        return s;
    },

    // Encodes the basic 4 characters used to malform HTML in XSS hacks
    XSSEncode: function (s, en) {
        if (!this.isEmpty(s)) {
            en = en || true;
            // do we convert to numerical or html entity?
            if (en) {
                s = s.replace(/\'/g, "&#39;"); //no HTML equivalent as &apos is not cross browser supported
                s = s.replace(/\"/g, "&quot;");
                s = s.replace(/</g, "&lt;");
                s = s.replace(/>/g, "&gt;");
            } else {
                s = s.replace(/\'/g, "&#39;"); //no HTML equivalent as &apos is not cross browser supported
                s = s.replace(/\"/g, "&#34;");
                s = s.replace(/</g, "&#60;");
                s = s.replace(/>/g, "&#62;");
            }
            return s;
        } else {
            return "";
        }
    },

    // returns true if a string contains html or numerical encoded entities
    hasEncoded: function (s) {
        if (/&#[0-9]{1,5};/g.test(s)) {
            return true;
        } else if (/&[A-Z]{2,6};/gi.test(s)) {
            return true;
        } else {
            return false;
        }
    },

    // will remove any unicode characters
    stripUnicode: function (s) {
        return s.replace(/[^\x20-\x7E]/g, "");

    },

    // corrects any double encoded &amp; entities e.g &amp;amp;
    correctEncoding: function (s) {
        return s.replace(/(&amp;)(amp;)+/, "$1");
    },


    // Function to loop through an array swaping each item with the value from another array e.g swap HTML entities with Numericals
    swapArrayVals: function (s, arr1, arr2) {
        if (this.isEmpty(s)) return "";
        var re;
        if (arr1 && arr2) {
            //ShowDebug("in swapArrayVals arr1.length = " + arr1.length + " arr2.length = " + arr2.length)
            // array lengths must match
            if (arr1.length == arr2.length) {
                for (var x = 0, i = arr1.length; x < i; x++) {
                    re = new RegExp(arr1[x], 'g');
                    s = s.replace(re, arr2[x]); //swap arr1 item with matching item from arr2	
                }
            }
        }
        return s;
    },

    inArray: function (item, arr) {
        for (var i = 0, x = arr.length; i < x; i++) {
            if (arr[i] === item) {
                return i;
            }
        }
        return -1;
    }

}