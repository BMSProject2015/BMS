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
        string ParentID = "SanPham";
        String UserID = User.Identity.Name;
        string iID_MaSanPham = "", sTen = "",sMa = "", rSoLuong = "",iID_MaDonVi = "", sQuyCach = "", iDM_MaDonViTinh = "";
        int i;
        iID_MaSanPham =  HamChung.ConvertToString(ViewData["iID_MaSanPham"]);
        DataTable dt = new DataTable();
        DataRow R;
        if (!String.IsNullOrEmpty(iID_MaSanPham))
        {
            dt = SanPham_VatTuModels.Get_SanPham(iID_MaSanPham);
            if (dt.Rows.Count > 0)
            {
                R = dt.Rows[0];
                sTen = HamChung.ConvertToString(R["sTen"]);
                sMa = HamChung.ConvertToString(R["sMa"]);
                rSoLuong = HamChung.ConvertToString(R["rSoLuong"]);
                sQuyCach = HamChung.ConvertToString(R["sQuyCach"]);
                iDM_MaDonViTinh = HamChung.ConvertToString(R["iDM_MaDonViTinh"]);
                iID_MaDonVi = HamChung.ConvertToString(R["iID_MaDonVi"]);
            }
        }
        String isError = Convert.ToString(ViewData["isError"]);
        if (isError=="1")
        {
            sTen = Convert.ToString(ViewData["sTen"]);
            sMa = Convert.ToString(ViewData["sMa"]);
        }
        //chon don vi tinh
        SqlCommand cmd;
        cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                       "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                   "FROM DC_LoaiDanhMuc " +
                                                                   "WHERE sTenBang = 'DonViTinh') ORDER BY sTen");
        dt = Connection.GetDataTable(cmd);
        R = dt.NewRow();
        R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
        R["sTen"] = "-- Đơn vị tính --";
        dt.Rows.InsertAt(R, 0);
        SelectOptionList slDonViTinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTen");
        cmd.Dispose();

        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        dtDonVi.Dispose();
        //đoạn code để khi chọn thêm mới
        using (Html.BeginForm("EditSubmit", "SanPham", new { ParentID = ParentID, iID_MaSanPham = iID_MaSanPham }))
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham"), "Danh sách sản phẩm")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Thông tin sản phẩm</span>
                    </td>
                    <td align="right">
                        <input id="Button3" type="submit" class="button_title" value="Lưu" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Mã sản phẩm:")%></b><span style="color:Red">*</span></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sMa, "sMa", "", "class=\"input1_2\" tab-index='-1' ")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sMa")%>
                            </div>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Tên sản phẩm:")%></b><span style="color:Red">*</span></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\" tab-index='-1' ")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                            </div>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Đơn vị tính:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, iDM_MaDonViTinh, "iDM_MaDonViTinh", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Quy cách phẩm chất:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <div>
                                <%=MyHtmlHelper.TextArea(ParentID, sQuyCach, "sQuyCach", "", "class=\"input1_2\" tab-index='-1' ")%>
                            </div>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%  } %>
    <br />
</asp:Content>
