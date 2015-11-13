<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        String KyHieu = Request.QueryString["KyHieu"];
        String Ten = Request.QueryString["Ten"];
        String LoaiTS = Request.QueryString["LoaiTS"];
        String DV = Request.QueryString["DV"];
        String page = Request.QueryString["page"];

        String sKyHieu = "", sTen = "";
        //đoạn lệnh nhảy đến phần thêm mới
        String strThemMoi = Url.Action("Edit", "TaiSan");
        //danh sách loại tài sản
        var dt = LoaiTaiSanModels.DT_LoaiTS(true, "--- Tất cả ---");
        SelectOptionList slLoaiTS = new SelectOptionList(dt, "iID_MaLoaiTaiSan", "TenHT");
        if (dt != null) dt.Dispose();

        //danh mục đơn vị tính
        DataTable dtLoaiCT = DanhMucModels.DT_DanhMuc("DonViTinh", true, "--- Tất cả ---");
        SelectOptionList slDonViTinh = new SelectOptionList(dtLoaiCT, "iID_MaDanhMuc", "sTen");
        if (dtLoaiCT != null) dtLoaiCT.Dispose();
        
        //tìm kiếm tài sản
        using (Html.BeginForm("SearchSubmit", "TaiSan", new { ParentID = ParentID }))
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
                                            <b>Ký hiệu</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, KyHieu, "sKyHieu", "", "class=\"input1_2\" tab-index='-1'")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Loại tài sản</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slLoaiTS, LoaiTS, "iID_MaLoaiTaiSan", "", "class=\"input1_2\" ")%>
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
                                            <b>Tên tài sản</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, Ten, "sTen", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Đơn vị tính </b>
                                        </div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, DV, "iID_MaDanhMuc", "", "class=\"input1_2\" ")%>
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
            var url = unescape('<%= Url.Action("get_List?KyHieu=#0&Ten=#1&LoaiTS=#2&DV=#3&page=#4", "TaiSan")%>');
            url = unescape(url.replace("#0", "<%=KyHieu %>"));
            url = unescape(url.replace("#1", "<%=Ten %>"));

            url = unescape(url.replace("#2", "<%=LoaiTS %>"));
            url = unescape(url.replace("#3", "<%=DV %>"));
            url = unescape(url.replace("#4", "<%=page %>"));

            $.getJSON(url, function (data) {
                document.getElementById("divListQuyetToan").innerHTML = data;
            });
        }      
    
    </script>
    <div id="divListQuyetToan">
    </div>
</asp:Content>
