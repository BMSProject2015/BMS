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
  <script src="<%= Url.Content("~/Scripts/jsMucLucDuAn.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>"
        type="text/javascript"></script>
        
    <%
        
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>|
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div style="width: 100%; float: left;">
           <div id="divChungTuChiTietHT" style="float: right; position: relative; width: 100%;">
            <table width="100%" cellpadding="2" cellspacing="2">
                <tr>
                    
                    <td>
                    
                        <div style="width: 100%; float: left; margin-top: 2px;">
                            <div class="box_tong">
                                <div class="title_tong">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                         <tr>
                        <td>
                            <span>Danh mục công trình</span>
                        </td>
                          <td align="right">
                                <span>F2:Thêm dòng</span>
                            </td>
                            <td align="right" style="width: 100px;">
                                <span>Delete: Xóa</span>
                            </td>
                            <td align="right" style="width: 140px;">
                                <span>Backspace: Sửa </span>
                            </td>
                            <td align="left">
                                <span>F10: Lưu</span>
                            </td>
                    </tr>
                                    </table>
                                </div>
                            </div>
                            <div>
                                <%Html.RenderPartial("~/Views/DungChung/MucLucDuAn/MucLucDuAnGrid_Index_DanhSach.ascx", new { ControlID = "ChungTuChiTiet_Index_DanhSach", MaND = User.Identity.Name }); %>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            jsDuToan_Url_Frame = '<%=Url.Action("ChungTuChiTiet_Frame", "MucLucDuAn", new {Ma=1})%>';
            $("#tabs").tabs();

        });
    </script>
</asp:Content>
