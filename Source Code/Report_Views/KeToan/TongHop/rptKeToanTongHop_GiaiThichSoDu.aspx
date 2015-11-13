<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <% 
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "KeToan";

        String iThang = Request.QueryString["iThang"];
        String iNam = Request.QueryString["iNam"];
        String optThu = "", optTamUng = "", optTra = "";
        optThu = Request.QueryString["optThu"];
        optTamUng = Request.QueryString["optTamUng"];
        optTra = Request.QueryString["optTra"];
        string iTrangThai = Request.QueryString["iTrangThai"];
        if (String.IsNullOrEmpty(optThu))
            optThu = "0";
        if (String.IsNullOrEmpty(optTamUng))
            optTamUng = "0";
        if (String.IsNullOrEmpty(optTra))
            optTra = "0";
        String KhoGiay = Request.QueryString["KhoGiay"];
        if (string.IsNullOrEmpty(KhoGiay)) KhoGiay = "1";
        String urlQuayLai = Url.Action("Edit", "KeToanTongHop_GiaiThichSoDu", new { iNam = iNam, iThang = iThang, optThu = optThu, optTamUng = optTamUng, optTra = optTra, KhoGiay = KhoGiay, iTrangThai = iTrangThai });
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Giải thích số dư</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                      
                        <td align="center">
                            <a href="<%=urlQuayLai %>">
                                <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" /></a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKeToanTongHop_GiaiThichSoDu", new { iNam = iNam, iThang = iThang, optThu = optThu, optTamUng = optTamUng, optTra = optTra, KhoGiay = KhoGiay, iTrangThai = iTrangThai }), "ExportToExcel")%>
    <iframe src="<%=Url.Action("ViewPDF","rptKeToanTongHop_GiaiThichSoDu",new{iNam=iNam,iThang=iThang,optThu=optThu,optTamUng=optTamUng,optTra=optTra,KhoGiay=KhoGiay, iTrangThai = iTrangThai })%>"
        height="600px" width="100%"></iframe>
</body>
</html>
