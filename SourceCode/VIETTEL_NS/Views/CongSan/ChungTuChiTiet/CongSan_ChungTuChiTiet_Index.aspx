<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/jsBang_CongSan.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
NameValueCollection data = CongSan_ChungTuModels.LayThongTin(iID_MaChungTu);
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "CongSan_ChungTu"), "Danh sách chứng từ ghi sổ")%> 
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
                            <span>Thông tin chứng từ</span>
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
                                        <td class="td_form2_td1"><div><b>Số chứng từ ghi sổ</b></div></td>
                                        <td class="td_form2_td5"><div><b><%=data["sSoChungTu"]%></b></div></td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1">
                                            <div><b>Ngày / Tháng chứng từ ghi sổ</b></div>
                                        </td>
                                        <td class="td_form2_td5"><div><b>
                                            <%=(data["iNgay"])%> / <%=(data["iThang"])%></b></div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 50%;" valign="top">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                    <tr>
                                        <td class="td_form2_td1">
                                            <div><b>Ngày tạo chứng từ</b></div>
                                        </td>
                                        <td class="td_form2_td5"><div><b>
                                            <%=String.Format("{0:dd/MM/yyyy}",Convert.ToDateTime(data["dNgayChungTu"]))%></b></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Nội dung chứng từ</b></div></td>
                                        <td class="td_form2_td5"><div><b><%=data["sNoiDung"]%></b></div></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>                    
                    <%Html.RenderPartial("~/Views/CongSan/ChungTuChiTiet/CongSan_ChungTuChiTiet_Index_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>