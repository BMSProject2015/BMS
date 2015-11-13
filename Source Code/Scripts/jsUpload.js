if (typeof (km_scripts) == 'undefined') var km_scripts = new Object();
var gaJsHost = (("https:" == document.location.protocol) ? "https://ssl." : "http://www.");

try {
    var pageTracker = _gat._getTracker("UA-9522013-1");
    pageTracker._trackPageview();
} catch (err) { }

function km_myclass_import(jsFile) {
    if (km_scripts[jsFile] != null) return;
    var scriptElt = document.createElement('script');
    scriptElt.type = 'text/javascript';
    scriptElt.src = jsFile;
    document.getElementsByTagName('head')[0].appendChild(scriptElt);
    km_scripts[jsFile] = jsFile; // or whatever value your prefer
}



// JScript File
var CUpload_arrUpload = new Array();


function CUpload_writeFlash(div_id, id, flash_name, width, height, color, transparent, FlashVars) {
    var element = document.getElementById(div_id)
    if (element) {
        element.innerHTML = CUpload_toString(id, flash_name, width, height, color, transparent, FlashVars);
    }
}

function CUpload_toString(id, flash_name, width, height, color, transparent, FlashVars) {
    if (navigator.appName.indexOf("Microsoft") != -1) {
        return CUpload_toStringForExplorer(id, flash_name, width, height, color, transparent, FlashVars);
    }
    else {
        return CUpload_toStringForStandard(id, flash_name, width, height, color, transparent, FlashVars);
    }
}

function CUpload_toStringForExplorer(id, flash_name, width, height, color, transparent, FlashVars) {
    var str = "<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\"";
    str += " id=\"" + id + "\"";
    str += " codebase=\"" + location.protocol + "//download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,115,0\"";
    str += " width=\"" + width + "\" ";
    str += " height=\"" + height + "\"";
    str += ">";
    if (typeof FlashVars != "undefined") {
        str += "<param name=\"FlashVars\" value=\"" + FlashVars + "\" />";
    }
    str += "<param name=\"width\" value=\"" + width + "\" />";
    str += "<param name=\"height\" value=\"" + height + "\" />";
    str += "<param name=\"movie\" value=\"" + flash_name + "\" />";
    str += "<param name=\"allowFullScreen\" value=\"true\" />";
    str += "<param name=\"allowNetworking\" value=\"all\" />";
    str += "<param name=\"allowscriptaccess\" value=\"always\" />";
    str += "<param name=\"wmode\" value=\"transparent\" />";
    str += "<param name=\"bgcolor\" value=\"" + color + "\" />";
    str += "</object>";
    return str;
}

function CUpload_toStringForStandard(id, flash_name, width, height, color, transparent, FlashVars) {
    var str = "<embed src=\"" + flash_name + "\" width=\"" + width + "\" height=\"" + height + "\" salign=\"tl\" scale=\"noScale\" quality=\"high\" ";
    if (typeof FlashVars != "undefined") {
        str += "FlashVars=\"" + FlashVars + "\" ";
    }
    str += "type=\"application/x-shockwave-flash\" ";
    str += "pluginspage=\"http://www.macromedia.com/go/getflashplayer\" ";
    str += "id=\"" + id + "\"";
    str += "allowFullScreen=\"true\" ";
    str += "allowNetworking=\"all\" ";
    str += "allowscriptaccess=\"always\" ";
    str += "wmode=\"transparent\" ";
    str += "bgcolor=\"" + color + "\" ";
    str += "></embed>";
    return str;
}

function CUpload_initUpload(uploadID) {
    var upload = CUpload_getUploadByID(uploadID);
    upload.init();
}

function CUpload_getUploadByID(uploadID) {
    for (var i = 0; i < CUpload_arrUpload.length; i++) {
        if (CUpload_arrUpload[i].uploadID == uploadID) {
            return CUpload_arrUpload[i];
        }
    }
    return null;
}

function CUpload(uploadID, divID, serverPath, key, uploadMode, fileType, imageWidth, imageHeight, imageThumsWidth, imageThumsHeight, imageThumsPath) {
    this.defineConstant();
    this.divID = divID;
    this.uploadID = uploadID;
    this.key = key;
    this.serverPath = serverPath;
    this.uploadMode = this.MULTIFILE;
    this.ctlDanhSachAnhID = "";
    this.ctlAnhDaiDienID = "";
    this.ctlDivAnhID = "";
    if (typeof uploadMode != "undefined") {
        this.uploadMode = uploadMode;
    }
    this.fileType = fileType;
    if (typeof this.fileType != "undefined" && this.fileType == this.FILETYPE_IMAGE) {
        this.imageWidth = imageWidth;
        this.imageHeight = imageHeight;
        this.imageThumsWidth = imageThumsWidth;
        this.imageThumsHeight = imageThumsHeight;
        this.imageThumsPath = imageThumsPath;
    }
    this.arrFilterTitle = new Array();
    this.arrFilterString = new Array();
    CUpload_arrUpload.push(this);
    this.load(divID, "100%", "90px");
    try {
        var pageTracker = _gat._getTracker("UA-9522013-1");
        pageTracker._trackPageview();
    } catch (err) { }
}

CUpload.prototype.defineConstant = function() {
    this.MULTIFILE = 0;
    this.FILE = 1;

    this.FILETYPE_NORMAL = 0;
    this.FILETYPE_IMAGE = 1;

    this.UPLOAD_COMPLETE = "Upload Complete";
}

CUpload.prototype.load = function(divID, width, height) {
    //CUpload_writeFlash(divID, this.UploadID, "http://nUpload.oneres.net/bando/Upload.swf", width, height, "", "", "UploadID=" + this.UploadID);
    CUpload_writeFlash(divID, this.uploadID,  "http://" + location.host +"/Content/Flash/Upload.swf", width, height, "", "", "uploadID=" + this.uploadID + "&UploadMode=" + this.uploadMode);
}

CUpload.prototype.addListener = function(eventID, func) {
    switch (eventID) {
        case this.UPLOAD_COMPLETE:
            this.onUploadComplete = func;
            break;
    }
}

CUpload.prototype.init = function() {
    var i;

    for (i = 0; i <= this.arrFilterTitle.length - 1; i++) {
        this.addFilter(this.arrFilterTitle[i], this.arrFilterString[i]);
    }
    var fupload = document.getElementById(this.uploadID);
    if (fupload && typeof fupload.init != "undefined") {
        var path = "http://" + location.host;
        var url = path + "/UploadServer.axd";

        url += "?path=" + this.serverPath;
        if (this.fileType == this.FILETYPE_IMAGE) {
            url += "&type=" + this.fileType;
            if (typeof this.imageWidth != "undefined") {
                url += "&imageWidth=" + this.imageWidth;
            }
            if (typeof this.imageHeight != "undefined") {
                url += "&imageHeight=" + this.imageHeight;
            }
            if (typeof this.imageThumsWidth != "undefined") {
                url += "&imageThumsWidth=" + this.imageThumsWidth;
            }
            if (typeof this.imageThumsHeight != "imageThumsHeight") {
                url += "&imageThumsHeight=" + this.imageThumsHeight;
            }
            if (typeof this.imageThumsPath != "undefined") {
                url += "&imageThumsPath=" + this.imageThumsPath;
            }
        }
        return fupload.init(url, this.key, "CUpload_UploadComplete");
    }
    return null;
}

CUpload.prototype.addFilter = function(strTitle, strFilter) {
    var fupload = document.getElementById(this.uploadID);
    if (fupload && typeof fupload.addFilter != "undefined") {
        fupload.addFilter(strTitle, strFilter);
    }
    else {
        this.arrFilterTitle.push(strTitle);
        this.arrFilterString.push(strFilter);
    }
    return null;
}

CUpload.prototype.HienThiAnh = function() {
    var ctrID = this.uploadID;
    var i;
    var sDanhSachAnh = document.getElementById(this.ctlDanhSachAnhID).value;
    var arrAnh = sDanhSachAnh.split("#|");
    var strR = "";

    strR += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\">";
    //Hàng tiêu đề
    strR += "<tr><td align=\"center\" style=\"width: 98px;\">Ảnh</td><td align=\"center\">Chú thích</td><td align=\"center\" style=\"width: 80px;\">Ảnh đại diện</td><td align=\"center\" style=\"width: 80px;\">&nbsp; </td></tr>";

    var strMau = "<tr><td align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td></tr>";
    var strHang;
    for (i = 1; i <= arrAnh.length - 1; i++) 
    {
        var arrChiTiet = arrAnh[i].split("##");
        strHang = strMau;

        var str0 = "<input name=\"{0}_url\" type=\"hidden\" value=\"{1}\"/>";
        str0 += "<img src='http://" + location.host + "/{2}' style='border:solid 1px;' />";
        str0 = str0.replace("{0}", ctrID);
        str0 = str0.replace("{1}", arrChiTiet[0]);
        str0 = str0.replace("{2}", arrChiTiet[0]);
        strHang = strHang.replace("{0}", str0);

        var str1 = "<input name=\"{0}_txt\" type=\"text\" value=\"{1}\" onfocus=\"this.select();\" onchange=\"CUpload_ThayDoiThongTinAnh('{2}');\" style=\"width: 95%;\" />";
        str1 = str1.replace("{0}", ctrID);
        str1 = str1.replace("{1}", arrChiTiet[1]);
        str1 = str1.replace("{2}", ctrID);
        strHang = strHang.replace("{1}", str1);

        var str2 = "<input name=\"{0}_rd\" type=\"radio\" {1} onclick=\"CUpload_ThayDoiThongTinAnh('{2}');\" />";
        str2 = str2.replace("{0}", ctrID);
        if (arrChiTiet[2] == "1") 
        {
            str2 = str2.replace("{1}", "checked=\"checked\"");
        }
        else 
        {
            str2 = str2.replace("{1}", "");
        }
        str2 = str2.replace("{2}", ctrID);
        strHang = strHang.replace("{2}", str2);

        var str3 = "<input type=\"button\" class=\"button4\" value=\"Xóa\" onclick=\"CUpload_XoaAnh('{0}',{1});\" />";
        str3 = str3.replace("{0}", ctrID);
        str3 = str3.replace("{1}", i);
        strHang = strHang.replace("{3}", str3);

        strR += strHang;
    }

    strR += "</table>";

    document.getElementById(this.ctlDivAnhID).innerHTML = strR;
}

function CUpload_UploadComplete(uploadID, FileName, url) {
    var upload = CUpload_getUploadByID(uploadID);
    if(upload.ctlAnhDaiDienID!="")
    {
        var strtg="";
          
        strtg += upload.imageThumsPath + "/" + url;
        strtg += "##Chú thích...";
        if(document.getElementById(upload.ctlAnhDaiDienID).value=="")
        {
            document.getElementById(upload.ctlAnhDaiDienID).value=upload.imageThumsPath + "/" + url;
            strtg += "##1";
        }
        else
        {
            strtg += "##0";
        }
        document.getElementById(upload.ctlDanhSachAnhID).value += "#|" + strtg;
        upload.HienThiAnh();
    }
    if (typeof upload.onUploadComplete != "undefined") 
    {
        upload.onUploadComplete(FileName, url);
    }
}



function CUpload_ThayDoiThongTinAnh(uploadID) {
    var upload = CUpload_getUploadByID(uploadID);
    var arrUrl = document.getElementsByName(uploadID + "_url");
    var arrTxt = document.getElementsByName(uploadID + "_txt");
    var arrRd = document.getElementsByName(uploadID + "_rd");
    var i;
    var strR="";
    
    for(i=0;i<=arrUrl.length-1;i++)
    {
        var strtg="";
          
        strtg += arrUrl[i].value;
        strtg += "##" + arrTxt[i].value;
        if(arrRd[i].checked)
        {
            strtg += "##1";
            document.getElementById(upload.ctlAnhDaiDienID).value = arrUrl[i].value;
        }
        else
        {
            strtg += "##0";
        }
        strR += "#|" + strtg;
    }
    document.getElementById(upload.ctlDanhSachAnhID).value = strR;
}

function CUpload_XoaAnh(uploadID, cs) {
    var upload = CUpload_getUploadByID(uploadID);
    var arrChiTiet;
    var i,strR="";
    var sDanhSachAnh = document.getElementById(upload.ctlDanhSachAnhID).value;
    var arrAnh = sDanhSachAnh.split("#|");
    if(cs<=arrAnh.length-1 && arrAnh.length>0)
    {
        arrChiTiet = arrAnh[cs].split("##");
        if(arrChiTiet[2]=="1")
        {
            document.getElementById(upload.ctlAnhDaiDienID).value = "";
        }
    }
    var ok = document.getElementById(upload.ctlAnhDaiDienID).value == "";
    for (i=1;i<=arrAnh.length-1;i++)
    {
        if(i!=cs)
        {
            if(ok)
            {
                arrChiTiet = arrAnh[i].split("##");
                document.getElementById(upload.ctlAnhDaiDienID).value = arrChiTiet[0];
                arrAnh[i] = arrChiTiet[0] + "##" + arrChiTiet[1] + "##1";
                ok=false;
            }
            strR += "#|" + arrAnh[i];
        }
    }
    document.getElementById(upload.ctlDanhSachAnhID).value = strR;
    upload.HienThiAnh();
}