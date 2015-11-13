<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%
    PartialModel dlChuyen = (PartialModel)Model;
    String ParentID = dlChuyen.ControlID;
    Dictionary<string, object> dicData = dlChuyen.dicData;

    string MaLoaiDanhMuc = (string)dicData["iID_MaLoaiDanhMuc"];

    Bang bang = new Bang("DC_DanhMuc");

    string TenBang = bang.TenBang;
    string TruongKhoa = bang.TruongKhoa;
    String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, TenBang);
    int CurrentPage = 1;
    if (dicData["DanhMuc_page"] != null) CurrentPage = (int)dicData["DanhMuc_page"];
    int TotalPages = bang.TongSoTrang();

    bang.TruyVanLayDanhSach.CommandText = "SELECT * FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc=@iID_MaLoaiDanhMuc AND iTrangThai=1";
    bang.TruyVanLayDanhSach.Parameters.AddWithValue("@iID_MaLoaiDanhMuc", MaLoaiDanhMuc);

    DataTable dt = bang.dtData("iSTT ASC", CurrentPage);
%>
<div class="title_form3">
    <b>
        <%=NgonNgu.LayXau("Danh mục") %></b> <b class="btn_form3">
            <%= MyHtmlHelper.ActionLink(Url.Action("Create", "DanhMuc", new { MaLoaiDanhMuc = MaLoaiDanhMuc }), NgonNgu.LayXau("Thêm danh mục"), "Create", sDanhSachChucNangCam)%>
        </b>
</div>
<table cellpadding="0" cellspacing="0" border="0" class="mGrid">
    <tr class="tr_form3">
        <th style="width: 3%;" align="center">
            STT
        </th>
        <th>
            <%=NgonNgu.LayXau("Tên") %>
        </th>
        <th style="width: 100px;">
            Hành động
        </th>
    </tr>
    <%
        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        {
            int STT = i + 1;
            DataRow Row = dt.Rows[i];
            string urlDetail = Url.Action("Detail", "DanhMuc", new { MaDanhMuc = Row[TruongKhoa] });
            string urlEdit = Url.Action("Edit", "DanhMuc", new { MaDanhMuc = Row[TruongKhoa], MaLoaiDanhMuc = MaLoaiDanhMuc });
            string urlDelete = Url.Action("Delete", "DanhMuc", new { MaDanhMuc = Row[TruongKhoa], MaLoaiDanhMuc = MaLoaiDanhMuc });
            String classtr = "";
            if (i % 2 == 0)
            {
                classtr = "class=\"alt\"";
            }
    %>
    <tr <%=classtr %>>
        <td style="padding: 3px 2px;" align="center">
            <%=STT%>
        </td>
        <td style="padding: 3px 2px;">
            <%= MyHtmlHelper.ActionLink(urlDetail, HttpUtility.HtmlEncode((string)Row["sTen"]), "Detail", sDanhSachChucNangCam)%>
        </td>
        <td style="padding: 3px 2px;">
            <%= MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", sDanhSachChucNangCam)%>
            |
            <%= MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", sDanhSachChucNangCam)%>
        </td>
    </tr>
    <%}
        dt.Dispose();
    %>
    <tr class="pgr">
        <td colspan="3" align="right">
            <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Detail", new { MaLoaiDanhMuc = MaLoaiDanhMuc, DanhMuc_page = x }))%>
        </td>
    </tr>
</table>
