<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.KhoBac" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
     <%
          String MaDiv = Request.QueryString["idDiv"];
    String MaDivDate = Request.QueryString["idDivDate"];
    String srcFile = Convert.ToString(ViewData["srcFile"]);
    String ParentID = "QuyetToanThongTri";
    String UserID = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
    String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
    dtCauHinh.Dispose();
    String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
    String OnSuccess = "";
    OnSuccess = Request.QueryString["OnSuccess"];
    String UrlReport = Url.Action("ViewPDF", "rptKTKB_ThongTriRutDT", new { MaND = UserID, iID_MaChungTu = iID_MaChungTu });

    //String BackURL = Url.Action("Index", "KeToanChiTietKhoBac");
    %>  

     <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                   
                                <tr>
                                    <td align= center>
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                    </td>
                                </tr>
                  
                       
                </table>
            </div>
        </div>
     <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>
    
</body>
</html>
