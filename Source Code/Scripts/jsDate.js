var Date_strXauChu = "dDmMyYhHsS";
var Date_strXauSo = "0123456789";

function ParseDatetime(GiaTri) {
    var vR = "";
    var year = 2000, month = 1, day = 1, hours = 0, minutes = 0, seconds = 0;
    var arr = GiaTri.split(" ");
    var i, arr1, ok = 1;
    for (i = 0; i < arr.length; i++) {
        if (arr[i].indexOf(":") >= 0) {
            arr1 = arr[i].split(":");
            if (arr1.length > 0) {
                hours = parseInt(arr1[0]);
                if (hours.toString() == "NaN" || !(0 <= hours && hours <= 23)) {
                    ok = 0;
                }
            }
            if (arr1.length > 1) {
                minutes = parseInt(arr1[1]);
                if (minutes.toString() == "NaN" || !(0 <= minutes && minutes <= 60)) {
                    ok = 0;
                }
            }
            if (arr1.length > 2) {
                seconds = parseInt(arr1[2]);
                if (seconds.toString() == "NaN" || !(0 <= seconds && seconds <= 60)) {
                    ok = 0;
                }
            }
        } else if (arr[i].indexOf("/") >= 0) {
            arr1 = arr[i].split("/");
            if (arr1.length > 0) {
                day = parseInt(arr1[0]);
                if (day.toString() == "NaN" || !(1 <= day && day <= 31)) {
                    ok = 0;
                }
            }
            if (arr1.length > 1) {
                month = parseInt(arr1[1]);
                if (month.toString() == "NaN" || !(1 <= month && month <= 12)) {
                    ok = 0;
                }
            }
            if (arr1.length > 2) {
                year = parseInt(arr1[2]);
                if (year.toString() == "NaN") {
                    ok = 0;
                }
            }
        }
    }
    if (ok == 1) {
        if (month == 4 || month == 6 || month == 9 || month == 11) {
            if (day == 31) {
                ok = 0;
            }
        }
        else if (month == 2) {
            if (day == 29 && ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)) {
                //Năm nhuận
            } else if (day >= 29) {
                ok = 0;
            }
        }
    }
    if (ok == 1) {
        vR = year + ":" + month + ":" + day + ":" + hours + ":" + minutes + ":" + seconds;
    }
    return vR;
}

function FormatDatetime(v, strFormat) {
    var vR="";
    if (v != "") {
        var i;
        var arr=v.split(":");
        var y = parseInt(arr[0], 10);
        var yyyy = y.toString();
        var yy = "";
        for (i = yyyy.length + 1; i <= 4; i++) yyyy = "0" + yyyy;
        for (i = 2; i <= 3; i++) yy = yy + yyyy.charAt(i);
        var M = parseInt(arr[1], 10);
        var MM = M.toString();
        if (M < 10) {
            MM = "0" + M;
        }
        var d = parseInt(arr[2], 10);
        var dd = d.toString();
        if (d < 10) {
            dd = "0" + d;
        }
        var H = parseInt(arr[3], 10);
        var HH = H.toString();
        if (H < 10) {
            HH = "0" + H;
        }
        var m = parseInt(arr[4], 10);
        var mm = m.toString();
        if (m < 10) {
            mm = "0" + m;
        }
        var s = parseInt(arr[5], 10);
        var ss = s.toString();
        if (s < 10) {
            ss = "0" + s;
        }
        vR = strFormat;
        vR = vR.replace("yyyy", yyyy.toString());
        vR = vR.replace("yy", yy.toString());
        vR = vR.replace("y", y.toString());
        vR = vR.replace("MM", MM.toString());
        vR = vR.replace("M", M.toString());
        vR = vR.replace("dd", dd.toString());
        vR = vR.replace("d", d.toString());
        vR = vR.replace("HH", HH.toString());
        vR = vR.replace("H", H.toString());
        vR = vR.replace("mm", mm.toString());
        vR = vR.replace("m", m.toString());
        vR = vR.replace("ss", ss.toString());
        vR = vR.replace("s", s.toString());
    }
    return vR;
}

function Date_GetStringDatetime(v, strFormat) {
    var vR = "";
    if (v != null) {
        var i, Ngay = -1, Thang = -1, Nam = -1, Gio = -1, Phut = -1, Giay = -1;

        
        var y = v.getFullYear();
        var yyyy = y.toString();
        var yy = "";
        for (i = yyyy.length + 1; i <= 4; i++) yyyy = "0" + yyyy;
        for (i = 2; i <= 3; i++) yy = yy + yyyy.charAt(i);

        var M = v.getMonth() + 1;
        var MM = M.toString();
        if (M < 10) {
            MM = "0" + M;
        }

        var d = v.getDate();
        var dd = d.toString();
        if (d < 10) {
            dd = "0" + d;
        }
        var H = v.getHours();
        var HH = H.toString();
        if (H < 10) {
            HH = "0" + H;
        }

        var m = v.getMinutes();
        var mm = m.toString();
        if (m < 10) {
            mm = "0" + m;
        }

        var s = v.getSeconds();
        var ss = s.toString();
        if (s < 10) {
            ss = "0" + s;
        }
        vR = strFormat;
        vR = vR.replace("yyyy", yyyy.toString());
        vR = vR.replace("yy", yy.toString());
        vR = vR.replace("y", y.toString());
        vR = vR.replace("MM", MM.toString());
        vR = vR.replace("M", M.toString());
        vR = vR.replace("dd", dd.toString());
        vR = vR.replace("d", d.toString());
        vR = vR.replace("HH", HH.toString());
        vR = vR.replace("H", H.toString());
        vR = vR.replace("mm", mm.toString());
        vR = vR.replace("m", m.toString());
        vR = vR.replace("ss", ss.toString());
        vR = vR.replace("s", s.toString());
    }
    return vR;
}

function TruNgay(v1, v2) {
    if (v1 == "" || v2 == "") return "";
    var arr1 = v1.split(":");
    var arr2 = v2.split(":");
    var d1 = new Date(parseInt(arr1[0]), parseInt(arr1[1]), parseInt(arr1[2]), parseInt(arr1[3]), parseInt(arr1[4]), parseInt(arr1[5]));
    var d2 = new Date(parseInt(arr2[0]), parseInt(arr2[1]), parseInt(arr2[2]), parseInt(arr2[3]), parseInt(arr2[4]), parseInt(arr2[5]));
    var vR = (d1 - d2) / 3600000 / 24;
    return vR;
}

/// <summary>
/// Xâu Format ngày tháng
/// </summary>
function Date_FormatString(str)
{
    return Date_TachXauFormat(str, Date_strXauChu);
}

function Date_KiemTraGiaTri(So, strFormat)
{
    switch (strFormat)
    {
        case "dd":
        case "d":
            return (1 <= So && So <= 31);
        case "MM":
        case "M":
            return (1 <= So && So <= 12);
        case "y":
        case "yy":
        case "yyyy":
            return (1 <= So && So <= 10000);
        case "HH":
        case "H":
            return (0 <= So && So <= 23);
        case "mm":
        case "m":
        case "ss":
        case "s":
            return (1 <= So && So <= 59);
    }
    return false;
}

function Date_TachXauFormat(str, strXauTach)
{
    var vR = new Array();
    var i;
    var tg = "";
    for (i = 0; i < str.length; i++){
        if (strXauTach.indexOf(str[i]) == -1)
        {
            vR.push(tg);
            tg = "";
        }
        else
        {
            tg += str[i];
        }
    }
    vR.push(tg);
    return vR;
}

function Date_GanDate(arrGT, date, strFormat)
{
    var vR;
    var i, Ngay = -1, Thang = -1, Nam = -1, Gio = -1, Phut = -1, Giay = -1;

    if (date != null)
    {
        Thang = date.getMonth() + 1;
        Ngay = date.getDate();
        Nam = date.getFullYear();
        Gio = date.getHours();
        Phut = date.getMinutes();
        Giay = date.getSeconds();
    }

    if (strFormat.indexOf('y') == -1 && strFormat.indexOf('Y') == -1)
    {
        Nam = 0;
    }
    if (strFormat.indexOf('M') == -1)
    {
        Thang = 1;
    }
    if (strFormat.indexOf('d') == -1 && strFormat.indexOf('D') == -1)
    {
        Ngay = 1;
    }
    if (strFormat.indexOf('h') == -1 && strFormat.indexOf('H') == -1)
    {
        Gio = 0;
    }
    if (strFormat.indexOf('m') == -1)
    {
        Phut = 0;
    }
    if (strFormat.indexOf('s') == -1 && strFormat.indexOf('S') == -1)
    {
        Giay = 0;
    }

    var arrFormat = Date_FormatString(strFormat);
    for (i = 0; i < arrFormat.length; i++)
    {
        switch (arrFormat[i])
        {
            case "dd":
            case "d":
                if (arrGT[i] >= 0)
                {
                    Ngay = arrGT[i];
                }
                break;
            case "MM":
            case "M":
                if (arrGT[i] >= 0)
                {
                    Thang = arrGT[i];
                }
                break;
            case "y":
            case "yy":
            case "yyyy":
                if (arrGT[i] >= 0)
                {
                    Nam = arrGT[i];
                    if (Nam < 30)
                    {
                        Nam = Nam + 2000;
                    }
                    else if (Nam < 100) {
                        Nam = Nam + 1900;
                    }
                }
                break;
            case "HH":
            case "H":
                if (arrGT[i] >= 0)
                {
                    Gio = arrGT[i];
                }
                break;
            case "mm":
            case "m":
                if (arrGT[i] >= 0)
                {
                    Phut = arrGT[i];
                }
                break;
            case "ss":
            case "s":
                if (arrGT[i] >= 0)
                {
                    Giay = arrGT[i];
                }
                break;
        }
    }
    if (Ngay > -1 && Thang > -1 && Nam > -1 && Gio > -1 && Phut > -1 && Giay > -1) {
        vR = new Date(Nam, Thang - 1, Ngay, Gio, Phut, Giay);
    }
    return vR;
}

function Date_XacDinhNgay(strXauNgay, csXauNgay, csFormat, arrGT, date, strFormat) {
    var vR = new Object();
    vR.arrGT = arrGT;
    vR.vR = null;
    
    var arrFormat = Date_FormatString(strFormat);
    var i, So, tgGT = arrGT[csFormat];
    var str;
    var tgVR;
    if (strXauNgay.length > csXauNgay && csFormat < arrFormat.length)
    {
        for (i = arrFormat[csFormat].length; i >= 1; i--)
        {
            if (csXauNgay + i <= strXauNgay.length)
            {
                str = strXauNgay.substring(csXauNgay, csXauNgay + i);
                if (IsNumeric(str)) {
                    So = parseInt(str);
                    if (Date_KiemTraGiaTri(So, arrFormat[csFormat]))
                    {
                        arrGT[csFormat] = So;
                        if (csFormat == arrFormat.Count - 1)
                        {
                            vR.vR = Date_GanDate(arrGT, date, strFormat);
                        }
                        else
                        {
                            vR = Date_XacDinhNgay(strXauNgay, csXauNgay + i, csFormat + 1, arrGT, date, strFormat);
                        }
                        if (vR.vR != null)
                        {
                            return vR;
                        }
                        arrGT[csFormat] = tgGT;
                    }
                }
            }
        }
        if (csFormat < arrFormat.Count - 1) {
            return Date_XacDinhNgay(strXauNgay, csXauNgay, csFormat + 1, arrGT, date, strFormat);
        }
    }
    else {
        vR.vR = Date_GanDate(arrGT, date, strFormat);
    }
    return vR;
}

function Date_GetDateTimeFromText(str, strFormat) {
    str = Trim(str);
    if (str == '') {
        return null;
    }
    var arrFormat = Date_FormatString(strFormat);
    var arrGT = new Array();
    var cs = 0, tg, i, j;
    var okCoDauNganCach = false;
    var vR = null;
    var date = new Date();

    //Khởi tạo ngày tháng năm
    for (i = 0; i < arrFormat.length; i++){
        arrGT.push(-1);
    }
    for (i = 0; i < str.length; i++)
    {
        if (Date_strXauSo.indexOf(str[i]) == -1)
        {
            okCoDauNganCach = true;
            break;
        }
    }
    if (okCoDauNganCach)
    {
        //Trường hợp có dấu ngăn cách
        var arr = Date_TachXauFormat(str, Date_strXauSo);
        var okSaiCuPhap = false;
        for (i = 0; i < arr.length; i++)
        {
            if (IsNumber(arr[i]))
            {
                okSaiCuPhap = true;
                if (arr[i].length < 5) {
                    tg = parseInt(arr[i]);
                }
                else{
                    tg = 0;
                }
                for (j = cs; j < arrFormat.length; j++){
                    if (Date_KiemTraGiaTri(tg, arrFormat[j])){
                        arrGT[j] = tg;
                        cs = j + 1;
                        break;
                    }
                }
            }
        }
        if(okSaiCuPhap){
            vR = Date_GanDate(arrGT, date, strFormat);
        }
    }
    else
    {
        //Trường hợp không có dấu ngăn cách
        var tgVR = Date_XacDinhNgay(str, 0, 0, arrGT, null, strFormat);
        if (tgVR.vR == null)
        {
            tgVR.vR = Date_XacDinhNgay(str, 0, 0, tgVR.arrGT, date, strFormat);
        }
        vR = tgVR.vR;
    }
    return vR;
}

function Date_ConvertTextToStringDatetime(str, strFormat) {
    var date = Date_GetDateTimeFromText(str, strFormat);
    return Date_GetStringDatetime(date, strFormat);
}

function Date_Control_onBlur(txt, strFormat) {
    var str = txt.value;
    txt.value = Date_ConvertTextToStringDatetime(str, strFormat);
}
function GetDaysInMonth(month, year) {
    if (month < 1 || month > 12) {
        return 31;
    }
    if (1 == month || 3 == month || 5 == month || 7 == month || 8 == month ||
            10 == month || 12 == month) {
        return 31;
    }
    else if (2 == month) {

        if (0 == (year % 4)) {

            if (0 == (year % 400)) {
                return 29;
            }
            else if (0 == (year % 100)) {
                return 28;
            }


            return 29;
        }

        return 28;
    }
    return 30;
}