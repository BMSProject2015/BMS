<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<%@ Import Namespace="DomainModel.Abstract" %>

<%
    PartialModel dlChuyen = (PartialModel)Model;
    String ParentID = dlChuyen.ControlID;
    Dictionary<string, object> dicData = dlChuyen.dicData;

    Bang bang = new Bang("PQ_Luat");
    string TenBang = bang.TenBang;
    string TruongKhoa = bang.TruongKhoa;
    String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, TenBang);

    int CurrentPage = 1;
    if (dicData["Luat_page"] != null) CurrentPage = (int)dicData["Luat_page"];
    int TotalPages = bang.TongSoTrang();
    DataTable dt = bang.dtData("sTen", CurrentPage);
%>
    
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td><span>Danh sách luật</span></td>
                <td align="right" style="padding-right: 10px;">
                    <b class="btn_form3">
                        <%= MyHtmlHelper.ActionLink(Url.Action("Create", "Luat"), NgonNgu.LayXau("Thêm luật"), "Create", sDanhSachChucNangCam)%>
                    </b>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th>Tên luật</th>
            <th style="width: 10%">Menu</th>
            <th style="width: 5%" align="center">Sửa</th>
            <th style="width: 5%" align="center">Xóa</th>
        </tr>
        <%
        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        {
            DataRow Row = dt.Rows[i];
            string urlDetail = Url.Action("Detail", "Luat", new { MaLuat = Row[TruongKhoa] });
            string urlMenu = Url.Action("MenuItem_Cam", "MenuItem", new { MaLuat = Row[TruongKhoa] });
            string urlEdit = Url.Action("Edit", "Luat", new { MaLuat = Row[TruongKhoa] });
            string urlDelete = Url.Action("Delete", "Luat", new { MaLuat = Row[TruongKhoa] });
            String classtr = "";
            if (i % 2 == 0)
            {
                classtr = "class=\"alt\"";
            }
            %>
            <tr <%=classtr %>>
                <td><%= MyHtmlHelper.ActionLink(urlDetail, (string)Row["sTen"], "Detail", sDanhSachChucNangCam)%></td>
                <td align="center"><%= MyHtmlHelper.ActionLink(urlMenu, "Menu", "Edit", sDanhSachChucNangCam)%></td>
                <td align="center">
                    <%= MyHtmlHelper.ActionLink(urlEdit, "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", sDanhSachChucNangCam)%>
                </td>
                <td align="center">
                    <%= MyHtmlHelper.ActionLink(urlDelete, "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", sDanhSachChucNangCam)%>
                </td>
            </tr>
        <%      }
        dt.Dispose();
        %>
        <tr class="pgr">
            <td colspan="8" align="right">
                <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { Luat_page = x }))%>
            </td>
        </tr>
    </table>
</div>
