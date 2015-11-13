<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/KeToanTongHop/jsKeToan.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/KeToanTongHop/jsBang_KeToanTongHop_CanDoiThuChiTaiChinh.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script> 
<%
    String MaND = User.Identity.Name;

%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 12%;">
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
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Cân đối thu chi tài chính</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <%--<table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                        <tr>
                            <td class="td_form2_td1"><div><b>Đơn vị</b></div></td>
                            <td class="td_form2_td5"><div><b><%=strDonVi%></b></div></td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Mã sản phẩm</b></div></td>
                            <td class="td_form2_td5"><div><b><%=sMa%></b></div></td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Tên sản phẩm</b></div></td>
                            <td class="td_form2_td5"><div><b><%=strTen%></b></div></td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Số lượng</b></div></td>
                            <td class="td_form2_td5"><div><b><%=rSoLuong%></b></div></td>
                        </tr>
                      
                        <tr>
                            <td class="td_form2_td1"><div><b>Quy cách</b></div></td>
                            <td class="td_form2_td5"><div><b><%=sQuyCach%></b></div></td>
                        </tr>
                    </table>--%>
                    <%Html.RenderPartial("~/Views/KeToanTongHop/ChungTuChiTiet/KeToanTongHop_CanDoiThuChiTaiChinh_Index_DanhSach.ascx", new { ControlID = "CanDoiThuChiTaiChinh", MaND = User.Identity.Name }); %>
                </div>
            </div>
        </div>
    </div>
</div>  
</asp:Content>
