<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
    
        String ParentID = "TaiKhoanDanhMucChiTiet";
        String iID_MaTaiKhoan = Request.QueryString["iID_MaTaiKhoan"];
        String strThemMoi = "";
        if (iID_MaTaiKhoan == Guid.Empty.ToString())
        {
            strThemMoi = Url.Action("Edit", "TaiKhoanDanhMucChiTiet");
        }
        else
        {
            strThemMoi = Url.Action("Edit", "TaiKhoanDanhMucChiTiet", new { iID_MaTaiKhoan = iID_MaTaiKhoan });
        }
        //đổ dữ liệu vào Combobox tài khoản
        var tbl = TaiKhoanModels.DT_DSTaiKhoanCha(true, "--Chọn tất cả--", User.Identity.Name, false);
        SelectOptionList slTaiKhoan = new SelectOptionList(tbl, "iID_MaTaiKhoan", "sTen");
        if (tbl != null) tbl.Dispose();
        String UrlGiaiThich = Url.Action("Index", "TaiKhoanDanhMucChiTiet");
        String strIn = Url.Action("Index", "rptKeToanTongHop_InTaiKhoanChiTiet", new { iID_MaTaiKhoan = iID_MaTaiKhoan });
        String strHinh = Url.Action("List", "TaiKhoanDanhMucChiTiet", new { iID_MaTaiKhoan = iID_MaTaiKhoan });
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop"), "Danh sách chứng từ ghi sổ")%>
                      |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "LoaiTaiKhoan"), "Danh mục Tài khoản kế toán")%>

                </div>
            </td>
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
                    <td style="width: 250px;">
                        <span>Danh mục tài khoản chi tiết </span>
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "onchange=\"ChonTaiKhoan(this.value)\" class=\"input1_2\" tab-index='-1' style=\"width:400px;\"")%>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input id="Button1" type="button" class="button_title" value="In danh sách" onclick="javascript:location.href='<%=strIn %>'" />
                        <input id="Button3" type="button" class="button_title" value="Xem dạng d.sách" onclick="javascript:location.href='<%=strHinh %>'" />
                         <input id="Button2" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 5%;" align="center">
                    STT
                </th>
                <th style="width: 15%;" align="center">
                    Ký hiệu
                </th>
                <th style="width: 40%;" align="center">
                    Nội dung giải thích
                </th>
                <th style="width: 7%;" align="center">
                    Ký hiệu cha
                </th>
                <th style="width: 18%;" align="center">
                    Tài khoản giải thích
                </th>
                <th style="width: 15%;" align="center">
                    Thao tác
                </th>
            </tr>
            <%
                string urlCreate = Url.Action("Create", "TaiKhoanDanhMucChiTiet", new { iID_MaTaiKhoanDanhMucChiTiet_Cha = "##", iID_MaTaiKhoan = iID_MaTaiKhoan });
                string urlDetail = Url.Action("Index", "TaiKhoanDanhMucChiTiet", new { iID_MaTaiKhoanDanhMucChiTiet_Cha = "##" });
                string urlEdit = Url.Action("Edit", "TaiKhoanDanhMucChiTiet", new { iID_MaTaiKhoanDanhMucChiTiet = "##", iID_MaTaiKhoan = iID_MaTaiKhoan });
                string urlDelete = Url.Action("Delete", "TaiKhoanDanhMucChiTiet", new { iID_MaTaiKhoanDanhMucChiTiet = "##" });
                // string urlSort = Url.Action("Sort", "TaiKhoanDanhMucChiTiet", new { iID_MaTaiKhoanDanhMucChiTiet_Cha = "##" });
                int ThuTu = 0;
                String XauHanhDong = "";
                String XauSapXep = "";
                XauHanhDong += MyHtmlHelper.ActionLink(urlCreate, NgonNgu.LayXau("Thêm mục con"), "Create", "") + " | ";
                XauHanhDong += MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", "") + " | ";
                XauHanhDong += MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", "");
                //XauSapXep = " | " + MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", "");
            %>
            <%=TaiKhoanDanhMucChiTietModels.LayXauTaiKhoanDanhMucChiTiet(Url.Action("", ""), XauHanhDong, XauSapXep, "", 0, ref ThuTu,iID_MaTaiKhoan)%>
        </table>
        <script type="text/javascript">
            function ChonTaiKhoan(value) {
                var url = '<%=UrlGiaiThich %>';
                url = url + "?iID_MaTaiKhoan=" + value;
                location.href = url;
            }
            
        </script>
    </div>
</asp:Content>
