<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.TCDN" %>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <%
        string ParentID = "MauDanhMuc";
        string sTenDanhMuc = "", sKyHieu = "";
        int i;
        //đoạn code để khi chọn thêm mới
        String strThemMoi = Url.Action("Edit", "SanPham_MauDanhMuc", new { });
        //
        String strSort = Url.Action("Sort", "SanPham_MauDanhMuc", new { iID_MaDanhMucCha = string.Empty});
        //sự kiện tìm kiếm được chọn
        using (Html.BeginForm("SaveSubmit", "SanPham_MauDanhMuc", new { ParentID = ParentID }))
        {
    %>
    <%--<%= Html.Hidden(ParentID + "_iID_MaSanPham", iID_MaSanPham)%>--%>
    <div id="idDialog" style="display: none;">
    </div>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>         
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color:#ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                </div>
            </td>
        </tr>
    </table>
    <%  } %>
    <br />
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                      <span>Danh sách khoản mục</span>
                    </td>
                    <td align="right">
                        <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                        <input id="Button1" type="button" class="button_title" value="Sắp xếp" onclick="javascript:location.href='<%=strSort %>'" />
                    </td>
                </tr>
            </table>
            
        </div>
        <div id="frame_danh_muc">
        <table class="mGrid">
            <tr>
                <th style="width: 20%;" align="center">
                    Mã  khoản mục
                </th>
                <th style="width: 50%;" align="center">
                    Tên khoản mục
                </th>
                <th style="width: 30%;" align="center">
                    Hành động
                </th>
            </tr>
            <%
                int ThuTu = 0;
                ArrayList listChiTiet = SanPham_MauDanhMucController.LayDanhSachDanhMuc("", 0, ref ThuTu);
                int c = listChiTiet.Count;
            %>
            <%--<%=SanPham_DanhMucGiaModels.LayXauChiTieu(sTenDanhMuc, sKyHieu, Url.Action("", ""), XauHanhDong, XauThemVT, XauThemCon, XauSapXep, "", 0, ref ThuTu)%>--%>
        <%
            foreach(Hashtable row in listChiTiet){
                string urlDelete = Url.Action("Delete", "SanPham_MauDanhMuc", new { iID_MaDanhMuc = row["iID_MaDanhMuc"]});
                string urlSort = Url.Action("Sort", "SanPham_MauDanhMuc", new { iID_MaDanhMucCha = row["iID_MaDanhMuc"]});
                string urlAdd = Url.Action("Edit", "SanPham_MauDanhMuc", new { iID_MaDanhMucCha = row["iID_MaDanhMuc"] });
                string urlEdit = Url.Action("Edit", "SanPham_MauDanhMuc", new { iID_MaDanhMuc = row["iID_MaDanhMuc"] });
        %>
            <tr>
                    <td style="background-color:#dff0fb;padding: 3px 3px;<%if ((int)row["laCha"] == 1){ %>font-weight:bold<%} %>"><%=row["sTenKhoa"]%></td>
                    <td style="background-color:#dff0fb;padding: 3px 3px;<%if ((int)row["laCha"] == 1){ %>font-weight:bold<%} %>"><%=row["sTen"]%></td>
                    <td style="background-color:#dff0fb;padding: 3px 3px;<%if ((int)row["laCha"] == 1){ %>font-weight:bold<%} %>">
                        <span><%=MyHtmlHelper.ActionLink(urlAdd, NgonNgu.LayXau("Thêm mục con"), "Create", "")%>|</span>
                        <span><%=MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", "")%>|</span>
                        <span><%=MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", "")%>|</span>
                        <%if ((int)row["laCha"] == 1){ %>  
                        <span><%=MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", "")%></span>
                        <%} %>
                    </td>
            </tr>
        <% 
            }
        %>
        </table>
        </div>
    </div>
</asp:Content>
