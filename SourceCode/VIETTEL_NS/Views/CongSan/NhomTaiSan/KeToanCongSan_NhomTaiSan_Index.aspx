<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
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
    string ParentID = "KTCS_NhomTaiSan";
    string sTaiKhoan = "", sKyHieu = "";
    int i;
    sTaiKhoan = Request.QueryString["Ten"];
    sKyHieu = Request.QueryString["KyHieu"];
    String page = Request.QueryString["page"];

    //đoạn code để khi chọn thêm mới
    String strThemMoi = Url.Action("Edit", "KTCS_NhomTaiSan", new { ID = string.Empty });
    //
    String strSort = Url.Action("Sort", "KTCS_NhomTaiSan", new { iID_MaLoaiTaiSan_Cha = string.Empty });
    //sự kiện tìm kiếm được chọn
    using (Html.BeginForm("SearchSubmit", "KTCS_NhomTaiSan", new
    {
        ParentID = ParentID
    }))
    {
%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td style="width: 10%">
            <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform: uppercase;
                color: #ec3237;">
                <b>
                    <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>
        </td>
        <td align="left">
            <div style="padding-top: 5px; padding-bottom: 5px; color: #ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCS_ChungTu"), "Chứng từ ghi sổ công sản")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong" style="display: none;">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Thông tin tìm kiếm</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>
                                            <%=NgonNgu.LayXau("Tên tài khoản")%></b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.TextBox(ParentID, sTaiKhoan, "sTaiKhoan", "", "class=\"input1_2\" tab-index='-1' ")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>
                                            <%=NgonNgu.LayXau("Ký hiệu")%></b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.TextBox(ParentID, sKyHieu, "sKyHieu", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" class="td_form2_td1" style="height: 10px;">
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
                    <span>Danh sách nhóm tài sản</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                    <input id="Button1" type="button" class="button_title" value="Sắp xếp" onclick="javascript:location.href='<%=strSort %>'" />
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 7%;" align="center">Loại tài sản</th>
            <th style="width: 20%;" align="center">
                Mã nhóm tài sản
            </th>
            <th style="width: 30%;" align="center">
                Tên nhóm tài sản
            </th>
            <th align="center">
                Mô tả
            </th>
            <th style="width: 10%;" align="center">
                Số năm hao mòn
            </th>
            <th style="width: 20%;" align="center">
                Hành động
            </th>
        </tr>
        <%
            string urlCreate = Url.Action("Create", "KTCS_NhomTaiSan", new { iID_MaNhomTaiSan_Cha = "##" });
            string urlDetail = Url.Action("Index", "KTCS_NhomTaiSan", new { iID_MaNhomTaiSan_Cha = "##" });
            string urlEdit = Url.Action("Edit", "KTCS_NhomTaiSan", new { iID_MaNhomTaiSan = "##" });
            string urlDelete = Url.Action("Delete", "KTCS_NhomTaiSan", new { iID_MaNhomTaiSan = "##" });
            string urlSort = Url.Action("Sort", "KTCS_NhomTaiSan", new { iID_MaNhomTaiSan_Cha = "##" });
            int ThuTu = 0;
            String XauHanhDong = "";
            String XauSapXep = "";

            XauHanhDong += MyHtmlHelper.ActionLink(urlCreate, NgonNgu.LayXau("Thêm mục con"), "Create", "") + " | ";
            XauHanhDong += MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", "") + " | ";
            XauHanhDong += MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", "");
            XauSapXep = " | " + MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", "");      
        %>
        <%=KTCS_NhomTaiSanModels.LayXauTaiKhoanKeToan(sTaiKhoan, sKyHieu, Url.Action("", ""), XauHanhDong, XauSapXep, "", 0, ref ThuTu, User.Identity.Name)%>
    </table>
</div>
</asp:Content>
