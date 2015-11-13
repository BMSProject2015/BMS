<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QLDA" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>
    </title>
</head>

<body>
    <%   String ParentID = "QLDA_CP";
         String PageLoad = "1";
         String sLNS = Convert.ToString(ViewData["sLNS"]);
        if(String.IsNullOrEmpty(sLNS)) PageLoad="0";
        String iID_MaDotCapPhat = Convert.ToString(ViewData["iID_MaDotCapPhat"]);
        if (String.IsNullOrEmpty(iID_MaDotCapPhat)) PageLoad = "0";
         String iLoai = Convert.ToString(ViewData["iLoai"]);
         if (String.IsNullOrEmpty(iLoai)) PageLoad = "0";
         String UrlReport = "";
         if (PageLoad == "1")
             UrlReport = Url.Action("ViewPDF", "rptQLDAThongTri", new { sLNS = sLNS, iID_MaDotCapPhat = iID_MaDotCapPhat, iLoai = iLoai });
         String urlExport = Url.Action("ExportToExcel", "rptQLDAThongTri", new { sLNS = sLNS, iID_MaDotCapPhat = iID_MaDotCapPhat, iLoai = iLoai });
          %>
   <%
   {
    %>   
      <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo QLDA thông tri cấp phát</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                       <tr>
                        <td align="center">
                            <input class="button" type="button"  value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="window.history.back()"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
   <%}%>
    <%=MyHtmlHelper.ActionLink(urlExport, "Xuất ra Excel") %>
         <iframe src="<%=UrlReport%>"
        height="600px" width="100%"></iframe>
</body>
</html>
