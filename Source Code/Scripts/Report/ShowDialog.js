function ShowDialog(swidth) {
    var dlg = $("#confirmBox").dialog({
        draggable: true,
        resizable: false,
        width: swidth,
        autoOpen: true,
        minHeight: 200,
        minwidth: 300
    });
    dlg.parent().appendTo($("form:first"));
}
function Hide() {
    $("#confirmBox").dialog("close");
}
function EscKey() {
    $('*').keyup(function (e) {
        if (e.keyCode == '27') {
            Hide();
        }
    });
}