<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TongHop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <%
        String ParentID = "KeToanTongHop";
        String srcFile = Convert.ToString(ViewData["srcFile"]);       
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        String LoaiTK = Request.QueryString["LoaiTK"];
        String iThang = Request.QueryString["iThang"];
        String iNamLamViec = Request.QueryString["iNamLamViec"];
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String URL = Url.Action("Index", "KeToanChiTietTienGui");
        using (Html.BeginForm("EditSubmit", "rptThongTriCap", new { ParentID = ParentID }))
   {
    %>   
     <!---------Test----------------------->
      <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>thông tri</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                   
                                <tr>
                                    <td align="right">
                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" disabled="disabled" />
                                    </td>
                                    <td width="5px">
                                    </td>
                                    <td>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()" />
                                    </td>
                                </tr>
                  
                       
                </table>
            </div>
        </div>
    </div>
   <%}%>
   <script type="text/javascript">
       function Huy() {
           window.location.href = '<%=URL %>';
       }  
   </script>
    <iframe src="<%=Url.Action("ViewPDF","rptThongTriCap", new{iID_MaChungTu=iID_MaChungTu,iID_MaDonVi = iID_MaDonVi,LoaiTK=LoaiTK, iThang = iThang, iNam = iNam})%>"
        height="600px" width="100%" ></iframe>
</body>
</html>

