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
        int i;
        String iID_MaTaiKhoanGoc = "";
        String ParentID = "Loai";
        String MaND = User.Identity.Name;
        String page = Request.QueryString["page"];
        String iNam = DanhMucModels.NamLamViec(MaND).ToString();
        //đổ dữ liệu vào Combobox tài khoản
        var dt = TaiKhoanModels.DT_DSTaiKhoan(true, "Tất cả", MaND);
        SelectOptionList slTaiKhoan = new SelectOptionList(dt, "iID_MaTaiKhoan", "sTen");
        iID_MaTaiKhoanGoc = Convert.ToString(dt.Rows[0]["iID_MaTaiKhoan"]);
        if (dt != null) dt.Dispose();

        //đoạn lệnh nhảy đến phần thêm mới
        String strThemMoi = Url.Action("Edit", "TuDien", new { iID_MaTaiKhoanGoc = iID_MaTaiKhoanGoc });
        using (Html.BeginForm("AddSubmit", "TuDien", new { ParentID = ParentID }))
        {
    %>
    <%= Html.Hidden(ParentID + "_iID_MaTaiKhoan", iID_MaTaiKhoanGoc)%>
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
                        <span>Chọn tài khoản</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>
                                <b>Tài khoản gốc <span style="color: Red;">(*)</span></b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 500px;">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoanGoc, "iID_MaTaiKhoanGoc", "", "class=\"input1_2\" tab-index='-1' onchange=\"Chon_LNS(this.value)\" style=\"with:100%\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            &nbsp;
                            <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTaiKhoan")%>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" colspan="2">
                            <input type="submit" class="button" value="Tạo mới" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 20px;">
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%
        } 
    %>
    <br />
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách từ điển</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divDotNganSach">
            <%=  TuDienModels.get_TuDien(iID_MaTaiKhoanGoc, iNam)%>
        </div>
    </div>
    <script type="text/javascript">

        function Chon_LNS(iID_MaTaiKhoanGoc) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("get_TuDien?iID_MaTaiKhoanGoc=#0", "TuDien")%>');
            // url = unescape(url.replace("#0", "<%= ParentID %>"));
            url = unescape(url.replace("#0", iID_MaTaiKhoanGoc));
            $.getJSON(url, function (data) {
                document.getElementById("divDotNganSach").innerHTML = data;
            });
        }        
        
    </script>
</asp:Content>
