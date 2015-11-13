<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
        String MaDiv = Request.QueryString["idDiv"];
        String MaDivDate = Request.QueryString["idDivDate"];
        String srcFile = Convert.ToString(ViewData["srcFile"]);
        String ParentID = "QuyetToanThongTri";
        String iNamLamViec = Request.QueryString["iNam"];
        String sSoChungTu = Request.QueryString["iID_MaChungTu"];
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
    %>
    <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptBangTongHopChungTuGoc", new { iNamLamViec = iNamLamViec, sSoChungTu = sSoChungTu }), "Xuất ra file Excel")%>
    <iframe src="<%=Url.Action("ViewPDF","rptBangTongHopChungTuGoc",new{ iNamLamViec=iNamLamViec, sSoChungTu=sSoChungTu})%>"
        height="600px" width="100%"></iframe>
</body>
</html>
