<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/QLDA/jsBang_QLDA_DuToan_Nam.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <%
        String ParentID = "Edit";
        String iID_MaDuToanNam_QuyetDinh = Request.QueryString["iID_MaDuToanNam_QuyetDinh"];

        DataTable dtDuAn = QLDA_DanhMucDuAnModels.ddl_DanhMucDuAn(true);
        SelectOptionList slDuAn = new SelectOptionList(dtDuAn, "iID_MaDanhMucDuAn", "TenHT");
        dtDuAn.Dispose();

        NameValueCollection data = QLDA_DuToan_NamModels.LayThongTin(iID_MaDuToanNam_QuyetDinh);
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_DuToan_Nam"), "Danh sách dự toán năm")%>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#pHeader').click(function () {
                $('#dvContent').slideToggle('slow');
            });
        });
        $(document).ready(function () {
            $("DIV.ContainerPanel > DIV.collapsePanelHeader > DIV.ArrowExpand").toggle(
            function () {
                $(this).parent().next("div.Content").show("slow");
                $(this).attr("class", "ArrowClose");
            },
            function () {
                $(this).parent().next("div.Content").hide("slow");
                $(this).attr("class", "ArrowExpand");
            });
        });            
    </script>
    <div style="width: 100%; float: left;">
        <div style="width: 100%; float: left;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Thông tin dự toán năm</span>
                            </td>
                            <td align="right">
                                <span>F2: Thêm hàng -- DELETE: Xóa Hàng -- F10: Lưu thông tin -- Space: Sửa thông tin</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1" style="width: 90px;">
                                        <div>
                                            <b>Đợt</b>&nbsp;<span style="color: Red;">*</span></div>
                                    </td>
                                    <td class="td_form2_td5" style="width: 200px;">
                                        <div>
                                            <%=data["iDot"]%>
                                        </div>
                                    </td>
                                    <td class="td_form2_td1" style="width: 150px;">
                                        <div>
                                            <b>Ngày lập</b>&nbsp;<span style="color: Red;">*</span></div>
                                    </td>
                                    <td class="td_form2_td5" style="width: 250px;">
                                        <div>
                                            <%=CommonFunction.LayXauNgay(Convert.ToDateTime(data["dNgayLap"]))%>
                                        </div>
                                    </td>
                                    <td class="td_form2_td1" style="width: 200px">
                                        <div>
                                            <b>Nội dung</b>&nbsp;<span style="color: Red;">*</span></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=data["sNoiDung"]%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </table>
                        <%Html.RenderPartial("~/Views/QLDA/DuToan/QLDA_DuToan_Nam_Index_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
