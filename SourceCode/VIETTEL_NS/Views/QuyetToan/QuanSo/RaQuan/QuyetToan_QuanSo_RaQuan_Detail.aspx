<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/jsBang_QuyetToanQuanSo_RaQuan.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
    String iThang = Request.QueryString["iThang"];
    if (String.IsNullOrEmpty(iThang)) iThang = Convert.ToString(ViewData["iThang"]);

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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_QuanSo_RaQuan"), "Danh sách quân số ra quân")%> | 
                <%=MyHtmlHelper.ActionLink(Url.Action("Detail", "QuyetToan_QuanSo_RaQuan", new { iThang = iThang }), "Chi tiết")%>
            </div>
        </td>
    </tr>
</table>
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Thông tin quyết toán quân số ra quân</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td style="width: 50%;" valign="top">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Tháng</b></div></td>
                                        <td class="td_form2_td5"><div><b><%=iThang %></b></div></td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>&nbsp;</b></div></td>
                                        <td class="td_form2_td5"><div><b>&nbsp;</b></div></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 50%;" valign="top">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 15%">
                                            <div><b>&nbsp;</b></div>
                                        </td>
                                        <td class="td_form2_td5"><div><b>&nbsp;
                                            </b></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>&nbsp;</b></div></td>
                                        <td class="td_form2_td5"><div><b>&nbsp;</b></div></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                      
                    </table>                    
                    <%Html.RenderPartial("~/Views/QuyetToan/QuanSo/RaQuan/QuyetToan_QuanSo_RaQuan_Detail_DanhSach.ascx", new { ControlID = "RaQuanChiTiet", iThang = iThang, MaND = User.Identity.Name }); %>    
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>


