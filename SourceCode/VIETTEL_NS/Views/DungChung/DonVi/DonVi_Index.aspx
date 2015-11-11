<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
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
    String ParentID = "DonVi";
    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    SqlCommand cmd;
        
    if(String.IsNullOrEmpty(page) == false){
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = DanhMucModels.GetDonVi(CurrentPage, Globals.PageSize);

    double nums = DanhMucModels.GetDonVi_Count();
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {page  = x}));
    String strThemMoi = Url.Action("Edit", "DonVi");
    String strDanhMuc = Url.Action("ThemDanhMucDonVi", "DonVi");  
%>
  <table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
       
         <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách đơn vị</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button2" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button1" type="button" class="button_title" value="Thêm mới từ năm trước" onclick="javascript:location.href='<%=strDanhMuc %>'" />
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 7%;" align="center">Mã đơn vị</th>
            <th style="width: 15%;" align="center">Tên đơn vị</th>
            <th  align="left">Mô tả</th>
            <th style="width: 20%;" align="center">Khối đơn vị</th>
            <th style="width: 10%;" align="center">Nhóm đơn vị</th>
            <th style="width: 15%;" align="center">Loại đơn vị</th>
            <th style="width: 20%;" align="center">Thao tác</th>            
        </tr>
     <%--   <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String MaLoaiDonVi = Convert.ToString(R["iID_MaLoaiDonVi"]);
            String MaNhomDonVi = Convert.ToString(R["iID_MaNhomDonVi"]);
            String MaKhoiDonVi = Convert.ToString(R["iID_MaKhoiDonVi"]);
            String sLoaiDonVi = Convert.ToString(DanhMucModels.GetRow_DanhMuc(MaLoaiDonVi).Rows[0]["sTen"]);
            String sNhomDonVi = Convert.ToString(DanhMucModels.GetRow_DanhMuc(MaNhomDonVi).Rows[0]["sTen"]);
            String sKhoiDonVi = Convert.ToString(DanhMucModels.GetRow_DanhMuc(MaKhoiDonVi).Rows[0]["sTen"]);
            String classtr = "";          
            if (i % 2 == 0)
            {
                classtr = "class=\"alt\"";
            }
            %>
            <tr <%=classtr %>>
                <td align="center"><%=R["rownum"]%></td>  
                <td align="center"><%=dt.Rows[i]["iID_MaDonVi"]%></td>       
                <td align="left"><%=dt.Rows[i]["sTen"]%></td>
                <td align="left"><%=dt.Rows[i]["sMoTa"]%></td>
                <td align="left"><%=sKhoiDonVi%></td>
                <td align="left"><%=sNhomDonVi%></td>
                <td align="left"><%=sLoaiDonVi%></td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "DonVi", new { MaDonVi = R["iID_MaDonVi"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "DonVi", new { MaDonVi = R["iID_MaDonVi"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="9" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>--%>

        <%
            string urlCreate = Url.Action("Create", "DonVi", new { iID_MaDonViCha = "##" });
            string urlDetail = Url.Action("Index", "DonVi", new { iID_MaDonViCha = "##" });
            string urlEdit = Url.Action("Edit", "DonVi", new { iID_MaDonVi= "##" });
            string urlDelete = Url.Action("Delete", "DonVi", new { MaDonVi = "##" });
            string urlSort = Url.Action("Sort", "DonVi", new { iID_MaDonViCha = "##" });
            int ThuTu = 0;
            String XauHanhDong = "";
            String XauSapXep = "";
            XauHanhDong += MyHtmlHelper.ActionLink(urlCreate, NgonNgu.LayXau("Thêm mục con"), "Create", "") + " | ";
            XauHanhDong += MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", "") + " | ";
            XauHanhDong += MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", "");
            XauSapXep = " | " + MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", "");
        %>
        <%=DonViModels.LayXauDanhSachDonVi(Url.Action("", ""), XauHanhDong, XauSapXep, "", 0, ref ThuTu)%>
    </table>
</div>
</asp:Content>
