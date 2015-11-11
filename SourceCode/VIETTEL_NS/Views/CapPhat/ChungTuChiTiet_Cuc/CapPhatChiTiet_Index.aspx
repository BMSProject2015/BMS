<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/jsBang_CapPhat_Cuc.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
String iID_MaCapPhat = Request.QueryString["iID_MaCapPhat"];
if (String.IsNullOrEmpty(iID_MaCapPhat)) iID_MaCapPhat = Convert.ToString(ViewData["iID_MaCapPhat"]);
NameValueCollection data = CapPhat_ChungTuModels.LayThongTin(iID_MaCapPhat);
String strLoaiCapPhat = CommonFunction.LayTenDanhMuc(data["iDM_MaLoaiCapPhat"]);
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "CapPhat_ChungTu"), "Danh sách chứng từ cấp phát")%> | 
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "CapPhat_ChungTuChiTiet"), "Chi tiết chứng từ cấp phát")%>
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
                        <td align="right"><span>F2 - Thêm</span></td>
                        <td align="right" style="width:100px;"><span>Delete - Xóa</span></td>                        
                        <td align="left"><span>F10 - Lưu</span></td>
                        <td>&nbsp;</td>
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
                                        <td class="td_form2_td1"><div><b>Số chứng từ</b></div></td>
                                        <td class="td_form2_td5"><div><b><%=data["sTienToChungTu"]%><%=data["iSoCapPhat"]%></b></div></td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Loại cấp phát</b></div></td>
                                        <td class="td_form2_td5"><div><b><%=strLoaiCapPhat %></b></div></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 50%;" valign="top">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 15%">
                                            <div><b>Ngày chứng từ</b></div>
                                        </td>
                                        <td class="td_form2_td5"><div><b>
                                            <%=String.Format("{0:dd/MM/yyyy}",Convert.ToDateTime(data["dNgayCapPhat"]))%></b></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Nội dung</b></div></td>
                                        <td class="td_form2_td5"><div><b><%=data["sNoiDung"]%></b></div></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>                    
                    <%Html.RenderPartial("~/Views/CapPhat/ChungTuChiTiet_Cuc/CapPhatChiTiet_Index_DanhSach.ascx", new { ControlID = "ChiTieuChiTiet", MaND = User.Identity.Name }); %>    
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>


