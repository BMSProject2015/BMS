<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.SanPham" %>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <%
        string ParentID = "CauHinhDonVi";
        String UserID = User.Identity.Name;
        String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        if (String.IsNullOrEmpty(iID_MaDonVi)) iID_MaDonVi = "0";
        DataTable dt = SanPham_CauHinhDonViController.LayCauHinh(iID_MaDonVi);
        string rTyLe311_211 = "", rTyLe312_212 = "", rTyLe411_211 = "", rTyLe412_212 = "", rTyLe511_211 = "", rTyLe512_212 = "";
        string rHeSo_211 = "", rHeSo_212 = "", rHeSo_311 = "", rHeSo_312 = "", rHeSo_411 = "", rHeSo_412 = "", rHeSo_511 = "", rHeSo_512 = "";
        if (dt.Rows.Count > 0)
        {
            DataRow R = dt.Rows[0];
            rTyLe311_211 = HamChung.ConvertToString(R["rTyLe311_211"]);
            rTyLe312_212 = HamChung.ConvertToString(R["rTyLe312_212"]);
            rTyLe411_211 = HamChung.ConvertToString(R["rTyLe411_211"]);
            rTyLe412_212 = HamChung.ConvertToString(R["rTyLe412_212"]);
            rTyLe511_211 = HamChung.ConvertToString(R["rTyLe511_211"]);
            rTyLe512_212 = HamChung.ConvertToString(R["rTyLe512_212"]);

            rHeSo_211 = HamChung.ConvertToString(R["rHeSo_211"]);
            rHeSo_212 = HamChung.ConvertToString(R["rHeSo_212"]);
            rHeSo_311 = HamChung.ConvertToString(R["rHeSo_311"]);
            rHeSo_312 = HamChung.ConvertToString(R["rHeSo_312"]);
            rHeSo_411 = HamChung.ConvertToString(R["rHeSo_411"]);
            rHeSo_412 = HamChung.ConvertToString(R["rHeSo_412"]);
            rHeSo_511 = HamChung.ConvertToString(R["rHeSo_511"]);
            rHeSo_512 = HamChung.ConvertToString(R["rHeSo_512"]);
        }
        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
        DataRow NewRow = dtDonVi.NewRow();
        NewRow["sTen"] = "Cục Tài chính";
        NewRow["iID_MaDonVi"] = 0;
        dtDonVi.Rows.InsertAt(NewRow, 0);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        dtDonVi.Dispose();
        //đoạn code để khi chọn thêm mới
        using (Html.BeginForm("EditSubmit", "SanPham_CauHinhDonVi", new { ParentID = ParentID}))
        {
    %>
    <script type="text/javascript">
        function ChonDonVi(iID_MaDonVi) {
            var url = "<%=Url.Action("Index", "SanPham_CauHinhDonVi")%>";
            location.href = url + "?iID_MaDonVi=" + iID_MaDonVi;
        }
    </script>
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
                        <span>Cấu hình tỷ lệ chi phí theo tiền lương của đơn vị</span>
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
                            <div><b><%=NgonNgu.LayXau("Đơn vị:")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "onchange = \"ChonDonVi(this.value)\" class=\"input1_2\"")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Tỷ lệ tiền lương nhân viên quản lý phân xưởng thuê ngoài (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rTyLe311_211, "rTyLe311_211", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Tỷ lệ tiền lương nhân viên quản lý phân xưởng NSBĐ (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rTyLe312_212, "rTyLe312_212", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Tỷ lệ tiền lương nhân viên bán hàng thuê ngoài (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rTyLe411_211, "rTyLe411_211", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Tỷ lệ tiền lương nhân viên bán hàng NSBĐ (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rTyLe412_212, "rTyLe412_212", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Tỷ lệ tiền lương cán bộ nhân viên quản lý thuê ngoài (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rTyLe511_211, "rTyLe511_211", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Tỷ lệ tiền lương cán bộ nhân viên quản lý NSBĐ (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rTyLe512_212, "rTyLe512_212", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class = "box_tong">
        <div style="background:#dff0fb;font-size:12px;text-transform:uppercase;padding:5px 0;border-bottom:1px solid #006666;">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span style="font-weight:bold;color:#ec3237;display:block;text-indent:20px;">Cấu hình hệ số bảo hiểm xã hội, bảo hiểm y tế, kinh phí công đoàn</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Hệ số BH-CĐ tiền lương, tiền công thuê ngoài (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rHeSo_211, "rHeSo_211", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Hệ số BH-CĐ tiền lương, tiền công NSBĐ (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rHeSo_212, "rHeSo_212", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Hệ số BH-CĐ tiền lương nhân viên quản lý phân xưởng thuê ngoài (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rHeSo_311, "rHeSo_311", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Hệ số BH-CĐ tiền lương nhân viên quản lý phân xưởng NSBĐ (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rHeSo_312, "rHeSo_312", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Hệ số BH-CĐ tiền lương nhân viên bán hàng thuê ngoài (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rHeSo_411, "rHeSo_411", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Hệ số BH-CĐ tiền lương nhân viên bán hàng NSBĐ (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rHeSo_412, "rHeSo_412", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Hệ số BH-CĐ tiền lương cán bộ nhân viên quản lý thuê ngoài (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rHeSo_511, "rHeSo_511", "", "class=\"input1_2\" tab-index='-1' ")%>
                        </td>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 10%;" class="td_form2_td1"></td>
                        <td valign="top" align="left" style="width: 30%;" class="td_form2_td1">
                            <div><b><%=NgonNgu.LayXau("Hệ số BH-CĐ tiền lương cán bộ nhân viên quản lý NSBĐ (%):")%></b></div>
                        </td>
                        <td valign="top" align="left" style="width: 50%;" class="td_form2_td1">
                            <%=MyHtmlHelper.TextBox(ParentID, rHeSo_512, "rHeSo_512", "", "class=\"input1_2\" tab-index='-1' ")%>
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
