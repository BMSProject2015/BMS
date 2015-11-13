<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
SqlCommand cmd;
cmd = new SqlCommand("SELECT * FROM TL_DanhMucTaiLieu ORDER BY iSTT");
DataTable dt = Connection.GetDataTable(cmd);
        
cmd.Dispose();
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TuLieu_DanhMuc"), "Danh mục lĩnh vực")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                    <span>Lĩnh vực</span>
                </td>
                <td>
                    <b class="btn_form3">
                        <%= MyHtmlHelper.ActionLink(Url.Action("Sort", "TuLieu_DanhMuc"), NgonNgu.LayXau("Sắp xếp"), "Sort", null)%>
                    </b>&nbsp;
                    <b class="btn_form3">
                        <%= MyHtmlHelper.ActionLink(Url.Action("Edit", "TuLieu_DanhMuc"), NgonNgu.LayXau("Thêm lĩnh vực"), "Create", null)%>
                    </b>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th width="3%" align="center">STT</th>
            <th width="67%" align="center">Tên lĩnh vực</th>
            <th width="30%" align="center">Thao tác</th>
        </tr>
        <%
            string urlCreate = Url.Action("Create", "TuLieu_DanhMuc", new { iID_MaKieuTaiLieu_Cha = "##" });
            string urlDetail = Url.Action("Index", "TuLieu_DanhMuc", new { iID_MaKieuTaiLieu_Cha = "##" });
            string urlEdit = Url.Action("Edit", "TuLieu_DanhMuc", new { iID_MaKieuTaiLieu = "##" });
            string urlDelete = Url.Action("Delete", "TuLieu_DanhMuc", new { iID_MaKieuTaiLieu = "##" });
            string urlSort = Url.Action("Sort", "TuLieu_DanhMuc", new { iID_MaKieuTaiLieu_Cha = "##" });
            int ThuTu = 0,STT=0;
            String XauHanhDong = "";
            String XauSapXep = "";

            XauHanhDong += MyHtmlHelper.ActionLink(urlCreate, NgonNgu.LayXau("Thêm mục con"), "Create", "") + " | ";
            XauHanhDong += MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", "") + " | ";
            XauHanhDong += MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", "");
            XauSapXep = " | " + MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", "");      
        %>

        <%=TuLieuLichSuModels.LayXauDanhMucTaiLieu(Url.Action("", ""), XauHanhDong, XauSapXep, "", 0, ref ThuTu, User.Identity.Name, STT)%>
    </table>
</div>
</asp:Content>
