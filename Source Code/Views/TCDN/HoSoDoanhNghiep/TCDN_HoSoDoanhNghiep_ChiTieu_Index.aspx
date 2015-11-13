<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
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
        string ParentID = "TCDN_HoSoDoanhNghiep_ChiTieu";
        string sTenChiTieu = "", sKyHieu = "";
        int i;
        sTenChiTieu = Request.QueryString["sTenChiTieu"];
        sKyHieu = Request.QueryString["KyHieu"];

        //đoạn code để khi chọn thêm mới
        String strThemMoi = Url.Action("Edit", "TCDN_HoSoDoanhNghiep_ChiTieu", new { ID = string.Empty });
        //
        String strSort = Url.Action("Sort", "TCDN_HoSoDoanhNghiep_ChiTieu", new { iID_MaChiTieuHoSo_Cha = string.Empty });
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_HoSoDoanhNghiep_ChiTieu"), "Danh sách chỉ tiêu hồ sơ doanh nghiệp")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                      <span>Danh sách chỉ tiêu</span>
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
                <th style="width: 5%;" align="center">
                    Mã  chỉ tiêu
                </th>
                <th style="width: 35%;" align="center">
                    Tên chỉ tiêu
                </th>
                <th style="width: 10%;" align="center">
                    Đơn vị tính
                </th>
                <th style="width: 15%;" align="center">
                    Loại chỉ tiêu
                </th>
                <th style="width: 15%;" align="center">
                    Map với chỉ tiêu cân đối kế toán
                </th>
                <th style="width: 20%;" align="center">
                    Hành động
                </th>
            </tr>
            <%
                string urlCreate = Url.Action("Create", "TCDN_HoSoDoanhNghiep_ChiTieu", new { iID_MaChiTieuHoSo_Cha = "##" });
                string urlDetail = Url.Action("Index", "TCDN_HoSoDoanhNghiep_ChiTieu", new { iID_MaChiTieuHoSo_Cha = "##" });
                string urlEdit = Url.Action("Edit", "TCDN_HoSoDoanhNghiep_ChiTieu", new { iID_MaChiTieuHoSo = "##" });
                string urlDelete = Url.Action("Delete", "TCDN_HoSoDoanhNghiep_ChiTieu", new { iID_MaChiTieuHoSo = "##" });
                string urlSort = Url.Action("Sort", "TCDN_HoSoDoanhNghiep_ChiTieu", new { iID_MaChiTieuHoSo_Cha = "##" });
                int ThuTu = 0;
                String XauHanhDong = "";
                String XauSapXep = "";
                XauHanhDong += MyHtmlHelper.ActionLink(urlCreate, NgonNgu.LayXau("Thêm mục con"), "Create", "") + " | ";
                XauHanhDong += MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", "") + " | ";
                XauHanhDong += MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", "");
                XauSapXep = " | " + MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", "");
            %>
            <%=TCDN_HoSoDoanhNghiep_ChiTieuModels.LayXauChiTieu(sTenChiTieu, sKyHieu, Url.Action("", ""), XauHanhDong, XauSapXep, "", 0, ref ThuTu)%>
        </table>
    </div>
</asp:Content>
