<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
     <%
         String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
    String ParentID = "RutDuToan";
    String UserID = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
    String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
    dtCauHinh.Dispose();

    String UrlReport = Url.Action("ViewPDF", "rptKTKB_ThongTriTongHop", new {MaND=User.Identity.Name,iID_MaChungTu= iID_MaChungTu});

    using (Html.BeginForm("EditSubmit", "rptKTKB_ThongTriTongHop", new { ParentID = ParentID }))
   {
    %>   
  
      <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                   
                                <tr>
                                   
                                    <td align=center>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                    </td>
                                </tr>
                  
                       
                </table>
            </div>
        </div>
    </div>
   <%}%>

     <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>
    
</body>
</html>
