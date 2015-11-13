//Khóa chuột phải
var message = "Sorry, No Right Click Allowed!!!";
function clickIE6() {
    if (event.button == 2) {
        // alert(message);
        return false;
    }
}
function clickNS4(e) {
    if (document.layers || document.getElementById && !document.all) {
        if (e.which == 2 || e.which == 3) {
            // alert(message);
            return false;
        }
    }
}
if (document.layers) {
    document.captureEvents(Event.MOUSEDOWN);
    document.onmousedown = clickNS4;
}
else if (document.all && !document.getElementById) {
    document.onmousedown = clickIE6;
}
document.oncontextmenu = new Function("return false");

// Prevent the backspace key from navigating back. Khóa Backspase trên trình duyệt
$(document).unbind('keydown').bind('keydown', function (event) {
    var doPrevent = false;
    if (event.keyCode === 8) {
        var d = event.srcElement || event.target;
        if ((d.tagName.toUpperCase() === 'INPUT' && (d.type.toUpperCase() === 'TEXT' || d.type.toUpperCase() === 'PASSWORD' || d.type.toUpperCase() === 'FILE'))
             || d.tagName.toUpperCase() === 'TEXTAREA') {
            doPrevent = d.readOnly || d.disabled;
        }
        else {
            doPrevent = true;
        }
    }

    if (doPrevent) {
        event.preventDefault();
    }
});


function js_Search_PhongBan_LNS_onkeypress(e) {
    if (e.keyCode == 13) {
        Load_Search_PhongBan_LNS();
    }
    else {
        setInterval(function () { Load_Search_PhongBan_LNS(); }, 2000);
      
    }
}

function Load_Search_PhongBan_LNS() {
    var Code = document.getElementById("Edit_Code").value;
    var url = "/PhongBanLNS/EditDetail?Code=" + Code;
    var sLNS = document.getElementById("txtLNS").value;
    url += "&LNS=" + sLNS;
    //alert(sLNS);
    location.href = url;
    document.getElementById("txtLNS").value = sLNS;
}
