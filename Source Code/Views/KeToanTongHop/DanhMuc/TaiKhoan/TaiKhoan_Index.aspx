<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <%
        string ParentID = "TK";
        string sTaiKhoan = "", sKyHieu = "";
        int i;
        sTaiKhoan = Request.QueryString["Ten"];
        sKyHieu = Request.QueryString["KyHieu"];
        String page = Request.QueryString["page"];

        //đoạn code để khi chọn thêm mới
        String strThemMoi = Url.Action("Edit", "LoaiTaiKhoan", new { ID = string.Empty });
        //
        String strSort = Url.Action("Sort", "LoaiTaiKhoan", new { iID_MaTaiKhoan_Cha = string.Empty });
        //sự kiện tìm kiếm được chọn
        using (Html.BeginForm("SearchSubmit", "LoaiTaiKhoan", new
        {
            ParentID = ParentID
        }))
        {
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%">
                <div style="padding-left: 20px; padding-top: 5px; text-transform: uppercase;
                    color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-top: 5px; padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop"), "Kế toán tổng hợp")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_KhoBac"), "Kế toán kho bạc")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_TienMat_ChungTu"), "Kế toán tiền gửi")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_TienGui_ChungTu"), "Kế toán tiền mặt")%>
                </div>
            </td>
             <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
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
                        <span>Danh sách tài khoản</span>
                    </td>
                    <td align="right">
                        <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                        <input id="Button1" type="button" class="button_title" value="Sắp xếp" onclick="javascript:location.href='<%=strSort %>'" />
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 10%;" align="center">
                    Mã tài khoản
                </th>
                <th style="width: 30%;" align="center">
                    Tên tài khoản
                </th>
                <th align="center">
                    Mô tả
                </th>
                <th style="width: 10%;" align="center">
                    TK cấp
                </th>
                <th style="width: 10%;" align="center">
                    Hiển thị CĐTK
                </th>
                <th style="width: 25%;" align="center">
                    Thao tác
                </th>
            </tr>
            <%
                string urlCreate = Url.Action("Create", "LoaiTaiKhoan", new { iID_MaTaiKhoan_Cha = "##" });
                string urlDetail = Url.Action("Index", "LoaiTaiKhoan", new { iID_MaTaiKhoan_Cha = "##" });
                string urlEdit = Url.Action("Edit", "LoaiTaiKhoan", new { iID_MaTaiKhoan = "##" });
                string urlDelete = Url.Action("Delete", "LoaiTaiKhoan", new { iID_MaTaiKhoan = "##" });
                string urlGiaiThich = Url.Action("Index", "KeToanTongHop_GiaiThichTaiKhoan", new { iID_MaTaiKhoan = "##" });
                string urlSort = Url.Action("Sort", "LoaiTaiKhoan", new { iID_MaTaiKhoan_Cha = "##" });
                int ThuTu = 0;
                String XauHanhDong = "";
                String XauSapXep = "";
                String XauTaiKhoanChiTiet = "";

                XauHanhDong += MyHtmlHelper.ActionLink(urlCreate, NgonNgu.LayXau("Thêm TK con"), "Create", "") + " | ";
                XauHanhDong += MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", "") + " | ";
                XauHanhDong += MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", "");
                XauTaiKhoanChiTiet = " | " + MyHtmlHelper.ActionLink(urlGiaiThich, NgonNgu.LayXau("Thêm CTTK"), "CreateEdit", "");
                XauSapXep = " | " + MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", "");      
            %>
            <%=TaiKhoanModels.LayXauTaiKhoanKeToan(sTaiKhoan, sKyHieu, Url.Action("", ""), XauHanhDong, XauSapXep, XauTaiKhoanChiTiet, "", 0, ref ThuTu, User.Identity.Name)%>
        </table>
    </div>
</asp:Content>
