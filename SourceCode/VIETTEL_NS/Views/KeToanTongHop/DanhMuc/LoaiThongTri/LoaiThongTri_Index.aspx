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
        int i;
        String ParentID = "Loai";
        String MaND = User.Identity.Name;
        String iID_MaTaiKhoanNo = Request.QueryString["TKNo"];
        String iID_MaTaiKhoanCo = Request.QueryString["TKCo"];
        String sLoaiThongTri = Request.QueryString["LoaiTT"];
        String sTenLoaiNS = Request.QueryString["LoaiNS"];
        String page = Request.QueryString["page"];
        String UserName = User.Identity.Name;
        //đoạn lệnh nhảy đến phần thêm mới
        String strThemMoi = Url.Action("Edit", "LoaiThongTri");
        //đổ dữ liệu vào Combobox tài khoản
        var dt = TaiKhoanModels.DT_DSTaiKhoan(true, "--Chọn tất cả--");
        SelectOptionList slTaiKhoan = new SelectOptionList(dt, "iID_MaTaiKhoan", "sTen");
        if (dt != null) dt.Dispose();
        using (Html.BeginForm("SearchSubmit", "LoaiThongTri", new { ParentID = ParentID }))
        {
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
                                            <b>Tài khoản nợ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoanNo, "iID_MaTaiKhoanNo", "", "class=\"input1_2\" tab-index='-1'")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Tài khoản có</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoanCo, "iID_MaTaiKhoanCo", "", "class=\"input1_2\"")%>
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
                                            <b>Loại thông tri</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sLoaiThongTri, "sLoaiThongTri", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Tên loại ngân sách</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sTenLoaiNS, "sTenLoaiNS", "", "class=\"input1_2\"")%>
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
                    <tr>
                        <td colspan="2" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" value="Tìm kiếm" />
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        <input id="TaoMoi" type="button" class="button" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%  } %>
    <br />
    <script type="text/javascript">
        List_QuyetToan_ChungTu();
        function List_QuyetToan_ChungTu() {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("get_List?TKNo=#0&TKCo=#1&LoaiTT=#2&LoaiNS=#3&page=#4&UserName=#5", "LoaiThongTri")%>');
            url = unescape(url.replace("#0", "<%=iID_MaTaiKhoanNo %>"));
            url = unescape(url.replace("#1", "<%=iID_MaTaiKhoanCo %>"));
            url = unescape(url.replace("#2", "<%=sLoaiThongTri %>"));
            url = unescape(url.replace("#3", "<%=sTenLoaiNS %>"));
            url = unescape(url.replace("#4", "<%=page %>"));
            url = unescape(url.replace("#5", "<%=UserName %>"));         
            $.getJSON(url, function (data) {
                document.getElementById("divListQuyetToan").innerHTML = data;
            });
        }      
    
    </script>
    <div id="divListQuyetToan">
    </div>
</asp:Content>
