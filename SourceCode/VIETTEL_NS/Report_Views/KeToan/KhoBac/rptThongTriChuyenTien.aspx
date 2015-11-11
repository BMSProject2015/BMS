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
    String ParentID = "RutDuToan";
    String UserID = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(UserID);
    String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    String iThang = Convert.ToString(dtCauHinh.Rows[0]["iThangLamViec"]);
    String iID_MaNamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
    String iID_MaNguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
    dtCauHinh.Dispose();    
    

    String sSoChungTuChiTiet = Convert.ToString(ViewData["sSoChungTuChiTiet"]);
    String[] arriID_MaChungTuChiTiet = sSoChungTuChiTiet.Split(',');
    String iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
    String InTenMLNS = Convert.ToString(ViewData["InTenMLNS"]);
    String iID_MaThongTri = Convert.ToString(ViewData["iID_MaThongTri"]);
    int ChiSo=Convert.ToInt16(ViewData["ChiSo"]);
    String iID_MaChungTuChiTiet =arriID_MaChungTuChiTiet[ChiSo];
    if(ChiSo<arriID_MaChungTuChiTiet.Length-1) ChiSo=ChiSo+1;
    String UrlReport = Url.Action("ViewPDF", "rptThongTriChuyenTien", new { MaND = UserID, iID_MaChungTu = iID_MaChungTu, sSoChungTuChiTiet = iID_MaChungTuChiTiet, InTenMLNS = InTenMLNS, iID_MaThongTri = iID_MaThongTri });
    String BackURL = Url.Action("Index", "KeToanChiTietKhoBac", new { iLoai=1});
    using (Html.BeginForm("EditSubmit", "rptThongTriChuyenTien", new { ParentID = ParentID, ChiSo = ChiSo }))
   {
    %>   
      <%=MyHtmlHelper.Hidden(ParentID, sSoChungTuChiTiet, "sSoChungTuChiTiet", "")%>
      <%=MyHtmlHelper.Hidden(ParentID,iID_MaChungTu,"iID_MaChungTu","") %>
      <%=MyHtmlHelper.Hidden(ParentID, InTenMLNS, "InTenMLNS", "")%>
      <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo thông tri loại ngân sách</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
                   
                    <tr>
                        <td align="right">
                            <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
   <%}%>
   <script type="text/javascript">
       function Huy() {
           window.location.href = '<%=BackURL %>';
       }
 </script>
   <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptThongTriChuyenTien", new { ParentID = ParentID, ChiSo = ChiSo, iID_MaChungTuChiTiet = iID_MaChungTuChiTiet, iNamLamViec = iNamLamViec, iThang = iThang, iID_MaThongTri = iID_MaThongTri }), "Xuất ra file Excel")%>
     <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>
</body>
</html>

