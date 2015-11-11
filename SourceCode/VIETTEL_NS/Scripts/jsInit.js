//Các trường của Control
//so-sau-dau-phay='5' 
//co-dau-hang-nghin='0'
//tab-index='3'

var jsInit_ControlID_Focus = null;
$(document).ready(function () {
    jQuery.ajaxSetup({ cache: false });
    //Định dạng TextBox
    jsInit_TextBox();
    //Định dạng Autocomplete
    jsInit_Autocomplete();

    //Định dạng tất cả control
    jsInit_AllControls();
    //Quản lý các control có tab-index
    jsControl_Init();
});

function jsInit_TextBox() {
    //Đối với các TextBox nhập số
    //{-----------------------------------    
    $('input:text[nhap-so="1"]').keyup(function () {
        return ValidateNumberKeyUp(this);
    });
    $('input:text[nhap-so="1"]').keypress(function (event) {
        return ValidateNumberKeyPress(this, event);
    });
    $('input:text[nhap-so="1"]').focusout(function () {
        if ($(this).attr("loai-nhap") == "2") {
            ValidateAndFormatNumber(this);
        }
    });
    //}-----------------------------------    

    //Bỏ nút Enter thì submit form
    //{-----------------------------------
    $('input:text').keypress(function (event) {
        if (event.which == 13) {
            if ($(this).attr("allow-enter") != "1") {
                var thisID = $(this).attr("id");
                jsControl_Next(thisID);
                event.preventDefault();
            }
        }
        else {
            //Không cho nhập dữ liệu với các ô có thuộc tính khong-nhap="1"]
            if ($(this).attr("khong-nhap") == "1") {
                event.preventDefault();
            }
        }
    });
    //}-----------------------------------

    //Focus vào thì chọn tất cả
    //{-----------------------------------
    $('input:text').focus(function () {
        var thisID = $(this).attr("id");
        var TenHam = 'Control_onFocus';
        var fn = window[TenHam];
        if (typeof fn == 'function') {
            fn(thisID, jsInit_ControlID_Focus);
        }
        $(this).select();
        if ($(this).attr("khong-nhap") != "1") {
            var GiaTriCu = $(this).val();
            $(this).attr("gia-tri-cu", GiaTriCu);
        }
        jsInit_ControlID_Focus = thisID;
    });
    //}-----------------------------------

    //Khi kích vào các nút submit thì sẽ hiển thị khung "Đang xử lý dữ liệu"
    //{-----------------------------------
    $('input:submit').click(function () {
        Bang_ShowCloseDialog('Đang xử lý dữ liệu...');
        return true;
    });
    //}-----------------------------------
}

function jsInit_Autocomplete() {
    var controls = $('input[autocomplete-control="1"]');
    var i;
    var vminLength, vdelay, controlID;

    for (i = 0; i < controls.length; i++) {
        vdelay = parseInt($(controls[i]).attr("delay"));
        vminLength = parseInt($(controls[i]).attr("minlength"));
        controlID = $(controls[i]).attr("id");
        $(controls[i]).autocomplete({
            source: function (request, response) {
                jsInit_Autocomplete_onSource(this.element[0].id, request, response);
            },
            select: function (event, ui) {
                jsInit_Autocomplete_onSelect(this.id, event, ui);
                return false;
            },
            delay: vdelay,
            minLength: vminLength
        }).data('autocomplete')._renderItem = function (ul, item) {
            return jsInit_Autocomplete_renderItem(this.element[0].id, ul, item);
        }
        $(controls[i]).focusout(function () {
            return jsInit_Autocomplete_onBlur($(this).attr("id"));
        });
    }
}

function jsInit_Autocomplete_onSource(txtID, request, response) {
    var url = $("#" + txtID).attr("url_danh_sach");
    var str_fnLayDSGiaTri = $("#" + txtID).attr("fnlay_ds_gia_tri");

    url = unescape(url);
    if (typeof str_fnLayDSGiaTri != "undefined") {
        var fnLayDSGiaTri = window[str_fnLayDSGiaTri];
        $.getJSON(url, { GiaTri: request.term, DSGiaTri: fnLayDSGiaTri }, response);
    }
    else {
        $.getJSON(url, { GiaTri: request.term }, response);
    }
}

function jsInit_Autocomplete_onSelect(txtID, event, ui) {
    var TenHam = $("#" + txtID).attr('fnselect');
    var fn = window[TenHam];
    if (typeof fn == 'function') {
        return fn(txt, event, ui);
    }
    return null;
}

function jsInit_Autocomplete_renderItem(txtID, ul, item) {
    var TenHam = $("#" + txtID).attr('fnrender_item');
    var fn = window[TenHam];
    if (typeof fn == 'function') {
        return fn(txt, ul, item);
    }
    var v = $("#" + txtID).val();
    var i;
    var text = item.label;
    for (i = text.length - v.length; i >= 0; i--) {
        if (v.toUpperCase() == text.substr(i, v.length).toUpperCase()) {
            text = text.substr(0, i) + '<b>' + text.substr(i, v.length) + '</b>' + text.substr(i + v.length);
        }
    }
    return $('<li></li>')
            .data('item.autocomplete', item)
            .append('<a>' + text + '</a>')
            .appendTo(ul);
}

function jsInit_Autocomplete_onBlur(txtID) {
    var jqTxtID = "#" + txtID;
    var GiaTri = $(jqTxtID).val();
    if ($(jqTxtID).attr("gia-tri-cu") != $(jqTxtID).val()) {
        $(jqTxtID).attr("gia-tri-cu", GiaTri);
        var ValueControlID = $(jqTxtID).attr("value_control_id");
        var url = $(jqTxtID).attr("url_gia_tri");
        if (typeof url == "undefined") {
            url = $(jqTxtID).attr("url_danh_sach");
        }
        var str_fnLayDSGiaTri = $(jqTxtID).attr("fnlay_ds_gia_tri");
        var str_fnComplete = $(jqTxtID).attr("fncomplete");

        GiaTri = $.trim(GiaTri);

        url += '&GiaTri=' + GiaTri;

        //Lấy các giá trị mà trường phụ thuộc
        if (str_fnLayDSGiaTri != "") {
            var fnLayDSGiaTri = window[str_fnLayDSGiaTri];
            url += '&DSGiaTri=' + fnLayDSGiaTri();
        }
        url = unescape(url);

        $.getJSON(url, function (item) {
            //Gán trường hiển thị
            $(jqTxtID).val(item.label);
            //Gán trường ẩn
            if (ValueControlID != null && ValueControlID != "") {
                $("#" + ValueControlID).val(item.value);
            }
            var fnComplete = window[str_fnComplete];
            if (typeof fnComplete == 'function') {
                fnComplete(txtID, item);
            }
        });
    }
}


function jsInit_AllControls() {
    //Focus thì lưu lại giá trị cũ
    //{-----------------------------------
    //    $('*').focus(function () {
    // $(this).attr("gia-tri-cu", $(this).val());
    //    });
    //}-----------------------------------

    //Xác định hàm kích các phím
    //{-----------------------------------
    if (jQuery.browser.mozilla || jQuery.browser.opera) {
        jQuery(document).bind("keypress", jsInit_fnKey);
    }
    else {
        jQuery(document).bind("keydown", jsInit_fnKey);
    }
    //jQuery(document).bind("keypress", jsInit_fnKey);
    //}-----------------------------------
}

function jsInit_fnKey(e) {
    /* If a modifier key is pressed (exapct shift), ignore the event */
    if (e.metaKey || e.altKey || e.ctrlKey) {
        return true;
    }

    /* Capture shift+tab to match the left arrow key */
    var iKey = (e.keyCode == 9 && e.shiftKey) ? -1 : e.keyCode;
    switch (iKey) {
        case 112: /* F1 */
        case 113: /* F2 */
        case 114: /* F3 */
        case 115: /* F4 */
        case 116: /* F5 */
        case 117: /* F6 */
        case 118: /* F7 */
        case 119: /* F8 */
        case 120: /* F9 */
        case 121: /* F10 */
        case 122: /* F11 */
        case 123: /* F12 */
            var nF = iKey - 111;
            jsInit_RaiseEvent("F" + nF);
            e.preventDefault();
            return false;
            //        case 8: /* BackSpace - chan phim Backspace tren ban phim de ko back lai trinh duyett */
            //            /* Call User event Keypress BackSpace*/
            //          var nodeName = e.target.nodeName.toLowerCase();
            //           if (nodeName == "td" || nodeName == "span") {
            ////           if ((nodeName == 'input' && e.target.type == 'text') || nodeName == 'td' || nodeName == 'textarea' || nodeName == "span") {
            //               // do nothing
            //               //return true;
            //            } else {
            //               e.preventDefault();
            //               return false;
            //                        }
            //            if (nodeName != "td" || nodeName != "span") {
            //                e.preventDefault();
            //            }
            //e.preventDefault();
            // jsInit_RaiseEvent("BackSpace");

        case 46: /* DELETE */
            /* Call User event Keypress DELETE*/
            e.preventDefault();
            jsInit_RaiseEvent("Delete");
            return false;

        case 27: /* esc */
            e.preventDefault();
            var fnEsc = window['Control_onKeypress_Esc'];
            if (typeof fnEsc == 'function') {
                fnEsc();
            }
            return true;
    }
}

function jsInit_RaiseEvent(strEvent) {
    var fn = window['Control_onKeypress_' + strEvent];
    if (typeof fn == 'function') {
        fn();
    }
    if (typeof Bang_keys != "undefined" && Bang_keys.isFocus()) {
        fn = window['Bang_onKeypress_F'];
        if (typeof fn == 'function') {
            fn(strEvent);
        }
    }
}