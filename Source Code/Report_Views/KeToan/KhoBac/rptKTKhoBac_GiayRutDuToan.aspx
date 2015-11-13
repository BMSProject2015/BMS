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
    dtCauHinh.Dispose();
    String sSoChungTuChiTiet = Convert.ToString(ViewData["sSoChungTuChiTiet"]);
    String iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
    String iLoai = Convert.ToString(ViewData["iLoai"]);
    String InTenMLNS =Convert.ToString(ViewData["InTenMLNS"]);
    String[] arrSoChungTuChiTiet = sSoChungTuChiTiet.Split(',');
    int ChiSo=Convert.ToInt16(ViewData["ChiSo"]);
    String SoChungTuChiTiet = arrSoChungTuChiTiet[ChiSo];
    if (ChiSo < arrSoChungTuChiTiet.Length - 1) ChiSo = ChiSo + 1;
    String UrlReport = Url.Action("ViewPDF", "rptKTKhoBac_GiayRutDuToan", new { iNamLamViec = iNamLamViec, iID_MaChungTu = iID_MaChungTu, sSoChungTuChiTiet = SoChungTuChiTiet, InTenMLNS = InTenMLNS, iLoai = iLoai });
         
    using (Html.BeginForm("EditSubmit", "rptKTKhoBac_GiayRutDuToan", new { ParentID = ParentID,ChiSo=ChiSo}))
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
                        <span>Báo cáo</span>
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
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                    </td>
                                </tr>
                  
                       
                </table>
            </div>
        </div>
    </div>
   <%}%>
   <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTKhoBac_GiayRutDuToan", new { iNamLamViec = iNamLamViec, iID_MaChungTu = iID_MaChungTu, sSoChungTuChiTiet = SoChungTuChiTiet, InTenMLNS = InTenMLNS,iLoai=iLoai }), "Xuất ra Excel")%>
     <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>
    
</body>
</html>

