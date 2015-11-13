<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%
    String MaND = User.Identity.Name;
    String ParentID = "KeToan";
    DataTable dtThang = DanhMucModels.DT_Thang(false);
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(User.Identity.Name);
               
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    String PageLoad=Convert.ToString(ViewData["PageLoad"]);
    if (String.IsNullOrEmpty(iID_MaDonVi)) iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
    String[] arrMaDonVi = iID_MaDonVi.Split(',');
    int ChiSo = Convert.ToInt16(ViewData["ChiSo"]);
    Double d = 7.100;
    String MaDonVi = arrMaDonVi[ChiSo];
    if (ChiSo + 1 < arrMaDonVi.Length)
    {
        MaDonVi = arrMaDonVi[ChiSo];
        ChiSo = ChiSo + 1;
    }
    else
    {
        ChiSo = 0;
    }
    String iID_MaTrangThaiDuyet = Convert.ToString(ViewData["iID_MaTrangThaiDuyet"]);
    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = "0";
    }
    String iThang = Convert.ToString(ViewData["iThang"]);
    if (String.IsNullOrEmpty(iThang)) iThang = Convert.ToString(dtThang.Rows[2]["MaThang"]);
    dtThang.Dispose();
    String urlReport="";
    String urlHuy = Url.Action("Index", "rptKeToanTongHop_PhanHo");
    if (PageLoad == "1")
        urlReport = Url.Action("ViewPDF", "rptKeToanTongHop_PhanHo", new { iThang = iThang, iID_MaDonVi = MaDonVi, UserName = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet });

    using (Html.BeginForm("EditSubmit", "rptKeToanTongHop_PhanHo", new { ParentID = ParentID, iThang = iThang, ChiSo = ChiSo }))
      { 
%>
<%=MyHtmlHelper.Hidden(ParentID, iID_MaDonVi, "iID_MaDonVi", "")%>
<%-- <script runat="server">
  void Page_Load(object sender, EventArgs e) {
      string lang = "vi-VN";
    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
  }
</script>--%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Báo cáo phân hộ</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="Div1">
        <div id="Div2">           
            <table  width="100%" cellpadding="0" cellspacing="0" border="0">
              <tr>
                              
                    <td align="right">
                    <div>                    
                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thực hiện")%>" />                        
                        </div>
                    </td>
                    <td width="5px">&nbsp;</td>
                    <td align="left"><div><input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="location.href='<%=urlHuy %>'" /></div></td>
                </tr>
            </table>
        </div>
    </div>
</div>
   
<%} %>
<%=MyHtmlHelper.ActionLink(Url.Action("ExportToExcel", "rptKeToanTongHop_PhanHo", new { iThang = iThang, iID_MaDonVi = iID_MaDonVi, UserName = MaND, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet }), "Export To Excel")%>
<div>

    <iframe src="<%=urlReport%>"
        height="600px" width="100%"></iframe>
</div>
