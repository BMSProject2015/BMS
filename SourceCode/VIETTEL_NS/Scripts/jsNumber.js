//dinh dang so
var jsNumber_DauCach1000 = ".";
var jsNumber_DauCachThapPhan = ",";
var jsNumber_KieuSoTiengViet = true;
var jsNumber_oldValue = "";

function ValidateNumberKeyPress(field, e) {
    jsNumber_oldValue = field.value;
    var charCode = (e.keyCode == 9 && e.shiftKey) ? -1 : e.keyCode;
    switch (charCode) {
        case 13: /* return */
        case 27: /* esc */
        case 35: /* end */
        case 36: /* home */
        case -1:
        case 37: /* left arrow */
        case 38: /* up arrow */
        case 9: /* tab */
        case 39: /* right arrow */
        case 40: /* down arrow */
        case 34: /* page down */
            return true;
    }
    var keychar;
    if (charCode == 0) {
        keychar = String.fromCharCode(e.charCode);
    } else {
        keychar = String.fromCharCode(charCode);
    }

    charCode = (e.which) ? e.which : e.keyCode;    
    if (charCode > 31 && (charCode < 48 || charCode > 57) && keychar != jsNumber_DauCachThapPhan && keychar != "-") {
        return false;
    }

    if (keychar == jsNumber_DauCachThapPhan && field.value.indexOf(jsNumber_DauCachThapPhan) != -1) {
        return false;
    }

    if (keychar == "-") {
        if (field.value.indexOf("-") != -1 /* || field.value[0] == "-" */) {
            return false;
        }
        else {
            //save caret position
            var caretPos = getCaretPosition(field);
            if (caretPos != 0) {
                return false;
            }
        }
    }

    return true;
}

//Hàm chỉnh lại định dạng số ngay sau khi bấm số vào
function ValidateNumberKeyUp(field) {
    if (jsNumber_oldValue == field.value) {
        //jsNumber_oldValue = "";
        return true;
    }
    //jsNumber_oldValue = "";
    if (document.selection != undefined) {
        if (document.selection.type == "Text") {
            return false;
        }
    }

    //save caret position
    var caretPos = getCaretPosition(field);

    var fdlen = field.value.length;
//    var KyTuCuoi = field.value.chatAt(fdlen-1);

    field.value = UnFormatNumber(field.value);

    var idTxt = field.id;
    var start = idTxt.length - 5;
    if (start < idTxt.length) {
        if (idTxt.substr(start, 5) == "_show") {
            idTxt = idTxt.substr(0, start);
            document.getElementById(idTxt).value = field.value;
        }
    }

    var IsFound = isFinite(field.value) && (field.value != "");
    if (!IsFound) {
        setSelectionRange(field, caretPos, caretPos);
        return false;
    }

    var bCoDau1000 = $(field).attr("co-dau-hang-nghin");
    if (!isFinite(bCoDau1000)) {
        bCoDau1000 = 1;
    }
    else {
        bCoDau1000 = parseInt(bCoDau1000);
    }
    var DangNhap = true;
    field.value = FormatNumber(field.value, -1, bCoDau1000, DangNhap);

    fdlen = field.value.length - fdlen;

    setSelectionRange(field, caretPos + fdlen, caretPos + fdlen);
    return true;
}

//Hàm kiểm tra và chỉnh lại định dạng số cho 1 đối tượng
function ValidateAndFormatNumber(field) {
    var nToFixed = $(field).attr("so-sau-dau-phay");
    if (!isFinite(nToFixed)) {
        nToFixed = -1;
    }
    else {
        nToFixed = parseFloat(nToFixed);
    }
    var bCoDau1000 = $(field).attr("co-dau-hang-nghin");
    if (!isFinite(bCoDau1000)) {
        bCoDau1000 = 1;
    }
    else {
        bCoDau1000 = parseInt(bCoDau1000);
    }
    if (field.tagName == "INPUT") {
        if (field.value == "") return;

        field.value = UnFormatNumber(field.value);
        field.value = parseFloat(field.value);

        //Nếu là đối tượng TextBox dùng để nhập số
        var idTxt = field.id;
        var start = idTxt.length - 5;
        if (start < idTxt.length) {
            if (idTxt.substr(start, 5) == "_show") {
                idTxt = idTxt.substr(0, start);
                document.getElementById(idTxt).value = field.value;
            }
        }

        //sửa lại
        var IsFound = isFinite(field.value) && (field.value != "");
        if (!IsFound) {
            //Nếu giá trị không phải kiểu số thì sẽ gán lại bằng ""
            field.value = "";
            field.focus();
            field.select();
            return;
        }

        if (isNaN(parseFloat(field.value))) {
            //Nếu giá trị không phải kiểu số thì sẽ gán lại bằng ""
            field.value = "";
            field.focus();
            field.select();
        }


        field.value = FormatNumber(field.value, nToFixed, bCoDau1000, false);
    }
    else if (field.tagName == "SPAN") {
        if (field.innerHTML == "") return;

        field.innerHTML = UnFormatNumber(field.innerHTML);

        //sửa lại
        var IsFound = isFinite(field.innerHTML) && (field.innerHTML != "");
        //var IsFound = /^-?\d+\.{0,1}\d*$/.test(field.value);
        //ban dau
        // var IsFound = /^-?\d+\.{0,1}\d*$/.test(field.value);
        if (!IsFound) {
            //alert("Không phải kiểu số");
            field.innerHTML = "";
            field.focus();
            field.select();
            return;
        }

        if (isNaN(parseFloat(field.value))) {
            //alert("Number exceeding float range");
            field.innerHTML = "";
            field.focus();
            field.select();
        }

        field.value = FormatNumber(field.value, nToFixed, bCoDau1000, false);
    }
}

//Hàm chuyển từ số có định dạng về dạng không định dạng
function UnFormatNumber(value) {
    value = value.toString();
    if (value == "") return value;
    if (jsNumber_KieuSoTiengViet) {
        value = value.replace(/\./gi, "")//hàm đã sửa
        value = value.replace(/\,/gi, ".")//hàm đã sửa
    }
    else {
        value = value.replace(/\,/gi, "")//hàm đã sửa
    }
    //obj.value = obj.value.replace(/,/gi, ""); //hàm ban đầu
    return value;
}

//Định dạng số nhập vào
function FormatNumber(fnum, nToFixed, bCoDau1000, DangNhap) {
    if (typeof DangNhap == "undefined") {
        DangNhap = false;
    }
    if (DangNhap == false && (fnum == null || fnum == 0)) {
        return "";
    }
    var orgfnum = fnum.toString();

    
    if (DangNhap == false) {
        if (typeof nToFixed != "undefined" && nToFixed >= 0) {
            var dCham = orgfnum.indexOf(".");
            var strChuanSo = orgfnum;
            if (dCham > 0) {
                strChuanSo = orgfnum.substr(0, dCham);
            }
            else {
                dCham = orgfnum.length - 1;
            }
            if (nToFixed > 0) {
                strChuanSo = strChuanSo + '.';
            }
            for (var i1 = dCham + 1; i1 < dCham + 1 + nToFixed; i1++) {
                if (i1 < orgfnum.length) {
                    strChuanSo = strChuanSo + orgfnum.charAt(i1); ;
                }
                else {
                    strChuanSo = strChuanSo + '0';
                }
            }
            orgfnum = strChuanSo;
        }
    }
    if (typeof bCoDau1000 == "undefined") {
        bCoDau1000 = true;
    }
    
    var snum = orgfnum;
    var flagneg = false;

    if (snum.length > 0 && snum.charAt(0) == "-") {
        flagneg = true;
        snum = snum.substr(1, snum.length - 1);
    }

    psplit = snum.split("."); //sửa dấu "." thành dấu ","

    var cnum = psplit[0],
	            parr = '',
	            j = cnum.length, d = 0;

    for (var i = j - 1; i >= 0; i--) {
        d = d + 1;
        parr = cnum.charAt(i) + parr;
        if (bCoDau1000 && d % 3 == 0 && i > 0) parr = jsNumber_DauCach1000 + parr;
    }
    snum = parr;

    if (orgfnum.indexOf(".") != -1) {//sửa dấu "." thành dấu ","
        snum += jsNumber_DauCachThapPhan + psplit[1]; //sửa dấu "." thành dấu ","
    }

    if (flagneg == true) {
        snum = "-" + snum;
    }
    /*Tuannn Thêm để nhập dấu "," 27/9/2012
    var lastChar = snum.substr(snum.length - 1, 1);
    if (lastChar == ',') snum = snum.substr(0, snum.length - 1);
    /*end*/
    return snum;
}

function IsNumeric(value) {
    return IsNumber(value);
}

//Hàm kiểm tra giá trị có phải là kiểu số hay không
function IsNumber(value) {
    var v = value;
    v = UnFormatNumber(v);
    return isFinite(v) && (v != "");
}

//Chuyển 1 xâu định dạng số về kiểu số
function ParseNumber(value) {
    var vR = 0;
    
    if (IsNumber(value)) {
        vR = UnFormatNumber(value);
        vR = parseFloat(vR);
    }
    return vR;
}


function ValidateNumberKeyUp1(field) {
    if (document.selection != undefined) {
        if (document.selection.type == "Text") {
            return;
        }
    }
    //save caret position
    var caretPos = getCaretPosition(field);

    var fdlen = field.value.length;
    

    var v = field.value;
    var v1 = field.value;
    if (!IsNumber(v)) {        
        setSelectionRange(field, caretPos, caretPos);        
        return false;
    }

    v = String(v1);
    var ktcuoi = "";
    i = fdlen - 1;
    while (i >= 0 && (v.charAt(i) == ',' || v.charAt(i) == '0')) {
        ktcuoi = v.charAt(i) + ktcuoi;
        if (v.charAt(i) == ',') break;
        i--;
    }

    if (!(i >= 0 && v.charAt(i) == ',')) {
        ktcuoi = "";
    }
    v = ParseNumber(v);
    var vHT = FormatNumber(v) + ktcuoi;
    if (field.value != vHT) {
        field.value = vHT;
        fdlen = field.value.length - fdlen;

        if (fdlen != 0) {
            setSelectionRange(field, caretPos + fdlen, caretPos + fdlen);
        }
    }

    
}

function ValidateAndFormatNumber1(fieldID, nToFixed) {
    var field = document.getElementById(fieldID);
    var v;
    if (field.tagName == "INPUT") {
        if (!IsNumber(field.value)) return;

        v = ParseNumber(field.value);
        field.value = FormatNumber(v, nToFixed);
    }
    else if (field.tagName == "SPAN") {
        if (!IsNumber(field.innerHTML)) return;

        v = ParseNumber(field.innerHTML);
        field.innerHTML = FormatNumber(v, nToFixed);
    }
}




//Ket thuc dinh dang

function getCaretPosition(objTextBox) {

    if (window.event) {
        var objTextBox = window.event.srcElement;
    }
    var i = 0;
    if (objTextBox != null) {
        i = objTextBox.value.length;

        if (objTextBox.createTextRange) {
            objCaret = document.selection.createRange().duplicate();
            while (objCaret.parentElement() == objTextBox &&
                      objCaret.move("character", 1) == 1) --i;
        }
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

