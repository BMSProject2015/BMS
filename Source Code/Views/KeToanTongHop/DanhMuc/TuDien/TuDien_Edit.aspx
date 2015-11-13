<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        String iID_Ma = Convert.ToString(ViewData["iID_MaTuDien"]);
        String iID_MaTaiKhoanGoc = Convert.ToString(Request.QueryString["iID_MaTaiKhoanGoc"]);
        String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
        String ParentID = "Edit";
        String iID_MaTaiKhoanNo = "", iID_MaTaiKhoanCo = "", sNhom = "", sLoai = "", sNoiDung = "", bPublic = "";
        String MaND = User.Identity.Name;
        String iNam = Convert.ToString(DanhMucModels.NamLamViec(MaND));
        //chi tiết tài khoản nếu trong trường hợp sửa
        DataTable dt = TuDienModels.getChiTiet(iID_Ma);
        if (dt.Rows.Count > 0 && iID_Ma != null && iID_Ma != "")
        {
            DataRow R = dt.Rows[0];
            iID_MaTaiKhoanNo = HamChung.ConvertToString(R["iID_MaTaiKhoanNo"]);
            iID_MaTaiKhoanCo = HamChung.ConvertToString(R["iID_MaTaiKhoanCo"]);
            sNhom = HamChung.ConvertToString(R["sNhom"]);
            sLoai = Convert.ToString(R["sLoai"]);
            sNoiDung = Convert.ToString(R["sNoiDung"]);
            bPublic = Convert.ToString(R["bPublic"]);
        }
        //đổ dữ liệu vào Combobox tài khoản
        var tbl = TaiKhoanModels.DT_DSTaiKhoan(false, "", MaND);
        SelectOptionList slTaiKhoan = new SelectOptionList(tbl, "iID_MaTaiKhoan", "sTen");
        if (tbl != null) tbl.Dispose();

        //đổ dữ liệu vào Combobox tài khoản
        var tblTK = TaiKhoanModels.DT_DSTaiKhoan(false, "", MaND);
        SelectOptionList slTaiKhoanTK = new SelectOptionList(tblTK, "iID_MaTaiKhoan", "sTen");
        if (tblTK != null) tbl.Dispose();

        using (Html.BeginForm("EditSubmit", "TuDien", new { ParentID = ParentID, iID_MaTuDien = iID_Ma }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaTuDien", iID_Ma)%>
    <%= Html.Hidden(ParentID + "_iID_MaTaiKhoan", iID_MaTaiKhoan)%>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TuDien"), "Danh sách từ điển")%>
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
                    <td>
                        <% if (ViewData["DuLieuMoi"] == "1")
                           {
                        %>
                        <span>Nhập thông tin từ điển</span>
                        <% 
                            }
                           else
                           { %>
                        <span>Sửa thông tin từ điển</span>
                        <% } %>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="50%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tài khoản gốc</b></div>
                        </td>
                        <td class="td_form2_td5">
                         <%--   <% if (ViewData["DuLieuMoi"] == "1")
                               {
                            %>--%>
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoanGoc", null, "class=\"input1_2\" onchange=\"ChonTaiKhoan(this.value)\"")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTaiKhoan")%>
                            </div>
                           <%-- <% 
                                }
                               else
                               { %>
                            <%= TaiKhoanModels.getTenTK( iID_MaTaiKhoan)%>
                            <% } %>--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tài khoản nợ</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoanNo, "iID_MaTaiKhoanNo", null, "class=\"input1_2\" tab-index='-1'")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTaiKhoanNo")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tài khoản có</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoanCo, "iID_MaTaiKhoanCo", null, "class=\"input1_2\"")%>
                                 <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTaiKhoanCo")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Nhóm</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sNhom, "sNhom", "", "class=\"input1_2\"", 2)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Loại</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sLoai, "sLoai", "", "class=\"input1_2\"", 2)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Nội dung từ điển</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\"", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sNoiDung")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Hoạt động</b></div>
                        </td>
                        <td class="td_form2_td5" align="left">
                            <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, bPublic, "bPublic", "")%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="70%">
                &nbsp;
            </td>
            <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
        } if (dt != null) { dt.Dispose(); };    
    %>
    <script type="text/javascript">

        function ChonTaiKhoan(Id) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("GetTaiKhoanCapCon?iID_MaTaiKhoan=#0&iNam=#1", "LoaiTaiKhoan")%>');
            url = unescape(url.replace("#0", Id));
            url = unescape(url.replace("#1", "<%=iNam%>"));
            $.getJSON(url, function (data) {
                document.getElementById("<%= ParentID %>_iID_MaTaiKhoanNo").innerHTML = data;
                document.getElementById("<%= ParentID %>_iID_MaTaiKhoanCo").innerHTML = data;
            });
        }
    </script>
</asp:Content>
