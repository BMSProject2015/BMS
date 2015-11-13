<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    int i;
    String ParentID = "QuyetToan_QuanSo_RaQuan";
    String MaND = User.Identity.Name;
    String iSoRaQuan = Request.QueryString["SoRaQuan"];
    String dTuNgay = Request.QueryString["TuNgay"];
    String dDenNgay = Request.QueryString["DenNgay"];
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
    String iThang = Request.QueryString["iThang"];
    String page = Request.QueryString["page"];

    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet ) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";

    DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
    dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
    dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");

    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }
    Boolean LayTheoMaNDTao = false;
    if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND)) LayTheoMaNDTao = true;
    String MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(QuyetToanModels.iID_MaPhanHeQuyetToan);
    DataTable dt = QuyetToan_QuanSo_RaQuanModels.Get_DanhSachRaQuan(iThang, MaND, CurrentPage, Globals.PageSize);

    DataTable dtThang = DanhMucModels.DT_Thang_CoThangKhong();
    DataRow Row;    
    Row = dtThang.NewRow();      
    Row[0] = "";
    Row[1] = "-- Chọn tháng --";
    dtThang.Rows.InsertAt(Row, 0);
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();
    double nums = QuyetToan_QuanSo_RaQuanModels.Get_DanhSachRaQuan_Count(iThang, MaND);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { MaND = MaND, SoRaQuan = iSoRaQuan, TuNgay = dTuNgay, DenNgay = dDenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, iThang = iThang, iID_MaDonVi = iID_MaDonVi, page = x }));

    String strThemMoi = Url.Action("Edit", "QuyetToan_QuanSo_RaQuan");

    using (Html.BeginForm("SearchSubmit", "QuyetToan_QuanSo_RaQuan", new { ParentID = ParentID}))
    {
%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_QuanSo_RaQuan"), "Chứng từ quyết toán quân số")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Thông tin</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
               
                <tr><td colspan="2" align="center" class="td_form2_td1" style="height: 10px;"></td></tr>
                <tr>
                    <td colspan="2" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                        <input id="TaoMoi" type="button" class="button" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<%  } %>
<br />
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách chứng từ quyết toán quân số</span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: 20%;" align="center">Tháng</th>           
            <th style="width: 25%;" align="center">Năm</th>

            <th style="width: 25%;" align="center">Sửa</th>
            <th style="width: 25%;" align="center">Xóa</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";         
            String sTrangThai = "";
            String strColor = "";
                       
            String strDelete = "";
         
               
                strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "QuyetToan_QuanSo_RaQuan", new { iThang = R["iThang"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
         

            String strURL = MyHtmlHelper.ActionLink(Url.Action("Detail", "QuyetToan_QuanSo_RaQuan", new { iThang = R["iThang"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết\"");
                       
            %>
            <tr <%=strColor %>>
                <td align="center"><%=i+1%></td>                                  
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Detail", "QuyetToan_QuanSo_RaQuan", new { iThang = R["iThang"] }).ToString(), Convert.ToString(R["iThang"]), "Detail", "")%></b>
                </td>
                <td align="left"><%=dt.Rows[i]["iNamLamViec"]%></td>
                
                <td align="center">
                    <%=strURL %>
                </td>                
                <td align="center">
                    <%=strDelete%>                                       
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="11" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%  
dt.Dispose();
dtTrangThai_All.Dispose();
dtTrangThai.Dispose();
%>
</asp:Content>



