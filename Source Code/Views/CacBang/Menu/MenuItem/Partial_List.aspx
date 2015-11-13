<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.CacBang" %>
<%
    PartialModel dlChuyen = (PartialModel)Model;
    String ParentID = dlChuyen.ControlID;
    Dictionary<string, object> dicData = dlChuyen.dicData;
    BangMenuItem bang = new BangMenuItem();
    string TenBang = bang.TenBang;
    string TruongKhoa = bang.TruongKhoa;
    String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, TenBang);
%>
<div style="float: right; text-align: right;">
    <%=MyHtmlHelper.ActionLink(Url.Action("Create", new { MaMenuItemCha = 0 }), NgonNgu.LayXau("Thêm mục con"), "Create", sDanhSachChucNangCam) %> | 
    <%=MyHtmlHelper.ActionLink(Url.Action("Sort", new { MaMenuItemCha = 0 }), NgonNgu.LayXau("Sắp xếp menu"), "Sort", sDanhSachChucNangCam)%>
</div>
<table cellpadding="0" cellspacing=""="0" border="0" class="table_form3">
    <tr class="tr_form3">
        <td width="70%"><b><%=NgonNgu.LayXau("Menu") %></b></td>
        <td width="30%"><b><%=NgonNgu.LayXau("Hành động")%></b></td>
    </tr>
    <%
    string urlCreate = Url.Action("Create", new { MaMenuItemCha = "##" });
    string urlDetail = Url.Action("Detail", new { MaMenuItem = "##" });
    string urlEdit = Url.Action("Edit", new { MaMenuItem = "##" });
    string urlDelete = Url.Action("Delete", new { MaMenuItem = "##" });
    string urlSort = Url.Action("Sort", new { MaMenuItemCha = "##" });
    int ThuTu = 0;
    String XauHanhDong = "";
    String XauSapXep = "";
    XauHanhDong += MyHtmlHelper.ActionLink(urlCreate, NgonNgu.LayXau("Thêm mục con"), "Create", sDanhSachChucNangCam) + " | ";
    XauHanhDong += MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", sDanhSachChucNangCam) + " | ";
    XauHanhDong += MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", sDanhSachChucNangCam);
    XauSapXep = " | " + MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", sDanhSachChucNangCam);
    %>
    <%=bang.LayXauMenu(Url.Action("", ""), XauHanhDong, XauSapXep, 0, 0, ref ThuTu)%>
</table>