<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.QuyetToan" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.KeToan.TienMat" %>

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
    String LoaiBaoCao=Convert.ToString(ViewData["LoaiBaoCao"]);
    String iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);     
    dtCauHinh.Dispose();
    String siID_MaChungTuChiTiet =Convert.ToString(ViewData["siID_MaChungTuChiTiet"]);
    String[] arriID_MaChungTuChiTiet = siID_MaChungTuChiTiet.Split(',');
    int ChiSo=Convert.ToInt16(ViewData["ChiSo"]);
    String iID_MaChungTuChiTiet =arriID_MaChungTuChiTiet[0];
    iID_MaChungTuChiTiet = arriID_MaChungTuChiTiet[ChiSo];
    if (ChiSo < arriID_MaChungTuChiTiet.Length - 1) ChiSo = ChiSo + 1;
    else ChiSo = 0;
    String UrlReport = Url.Action("ViewPDF", "rptKTTienMat_PhieuThu1", new { UserID = UserID, iID_MaChungTuChiTiet = iID_MaChungTuChiTiet, LoaiBaoCao = LoaiBaoCao,iID_MaChungTu});
    String BackURL = Url.Action("Index", "KeToanChiTietTienMat");
    using (Html.BeginForm("EditSubmit", "rptKTTienMat_PhieuThu1", new { ParentID = ParentID, ChiSo = ChiSo }))
   {
    %>   
     <%=MyHtmlHelper.Hidden(ParentID,siID_MaChungTuChiTiet,"iID_MaChungTuChiTiet","") %>
     <%=MyHtmlHelper.Hidden(ParentID, iID_MaChungTu, "iID_MaChungTu", "")%>
     <%=MyHtmlHelper.Hidden(ParentID, LoaiBaoCao, "LoaiBaoCao", "")%>
      <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Báo cáo Phiếu thu</span>
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
                                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="Huy()"  />
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
   <%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKTTienMat_PhieuThu1", new { ParentID = ParentID, ChiSo = ChiSo, iID_MaChungTuChiTiet = iID_MaChungTuChiTiet, iNamLamViec = iNamLamViec, iThang = iThang, iID_MaChungTu = iID_MaChungTu }), "Xuất ra file Excel")%>
     <iframe src="<%=UrlReport%>" height="600px" width="100%">
     </iframe>
    
</body>
</html>
