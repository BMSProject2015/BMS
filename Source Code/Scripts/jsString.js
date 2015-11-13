function Trim(sString)
{
    while (sString.substring(0,1) == ' ')
    {
        sString = sString.substring(1, sString.length);
    }
    while (sString.substring(sString.length-1, sString.length) == ' ')
    {
        sString = sString.substring(0,sString.length-1);
    }
    return sString;
}

function DoiThanhXau(v) {
    return String(v);
}

function DoiThanhSo(v) {
    return parseFloat(v);
}

String.prototype.trim = function () {
    return Trim(this);
}

String.prototype.startsWith = function (prefix) {
    return this.indexOf(prefix) == 0;
}

String.prototype.endsWith = function (suffix) {
    return this.match(suffix + "$") == suffix;
};


function LayTruongTrongCongThuc(sCongThuc)
{
    var vR = "";
    var cs1 = sCongThuc.indexOf("[");
    var cs2 = sCongThuc.indexOf("]");
    if (cs1 >= 0 && cs2 > cs1)
    {
        vR = sCongThuc.substring(cs1 + 1, cs2);
    }
    return vR;
}