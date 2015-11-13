<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/jsBang_BaoHiem_PhaiThuChiTiet.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%

String iID_MaBaoHiemPhaiThu = Request.QueryString["iID_MaBaoHiemPhaiThu"];
if (String.IsNullOrEmpty(iID_MaBaoHiemPhaiThu)) iID_MaBaoHiemPhaiThu = Convert.ToString(ViewData["iID_MaBaoHiemPhaiThu"]);
NameValueCollection data = BaoHiem_PhaiThuModels.LayThongTin(iID_MaBaoHiemPhaiThu);
data["sTen_DonVi"]=DonViModels.Get_TenDonVi(Convert.ToString(data["iID_MaDonVi"]));
String strThoiGianBH = "";

        strThoiGianBH = "Tháng";
 
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "BaoHiem_PhaiThu"), "Chứng từ bảo hiểm")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "BaoHiem_PhaiThuChiTiet", new { iID_MaBaoHiemPhaiThu = iID_MaBaoHiemPhaiThu}), "Chi tiết chứng từ bảo hiểm")%>
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
                        <td align="right"><span>F10- Lưu</span></td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                        <tr>
                            <td class="td_form2_td1"><div><b>Số chứng từ</b></div></td>
                            <td class="td_form2_td5"><div><b><%=data["sTienToChungTu"]%><%=data["iSoChungTu"]%></b></div></td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Đơn vị</b></div></td>
                            <td class="td_form2_td5"><div><b><%=data["sTen_DonVi"]%></b></div></td>
                        </tr>
                      
                        <tr>
                            <td class="td_form2_td1"><div><b><%=HttpUtility.HtmlEncode(strThoiGianBH)%></b></div></td>
                            <td class="td_form2_td5"><div><b><%=data["iThang_Quy"]%></b></div></td>
                        </tr>                        
                        <tr>
                            <td class="td_form2_td1"><div><b>Nội dung chứng từ</b></div></td>
                            <td class="td_form2_td5"><div><b><%=HttpUtility.HtmlEncode(data["sNoiDung"])%></b></div></td>
                        </tr>
                    </table>
                    <%Html.RenderPartial("~/Views/BaoHiem/PhaiThuChiTiet/BaoHiem_PhaiThuChiTiet_Index_DanhSach.ascx", new { ControlID = "BaoHiem_PhaiThuChiTiet", MaND = User.Identity.Name}); %>    
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>
