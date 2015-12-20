<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/CapPhat/CapPhat_ChungTu_DonVi.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>

<%
String iID_MaCapPhat = Request.QueryString["iID_MaCapPhat"];
if (String.IsNullOrEmpty(iID_MaCapPhat)) iID_MaCapPhat = Convert.ToString(ViewData["iID_MaCapPhat"]);
NameValueCollection data = CapPhat_ChungTuModels.LayThongTin(iID_MaCapPhat);
String strLoaiCapPhat = CommonFunction.LayTenDanhMuc(data["iDM_MaLoaiCapPhat"]);
String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(data["iID_MaDonVi"]));          
//Update lại các trường chỉ tiêu lấy từ cấp phát sang
//DataTable dtChungTuChiTiet = QuyetToan_ChungTuChiTietModels.Get_dtChungTuChiTiet(iID_MaCapPhat);
//QuyetToan_ChungTuChiTietModels.Update_TruongChiTieu(dtChungTuChiTiet);
//QuyetToan_ChungTuChiTietModels.Update_TruongDaQuyetToan(dtChungTuChiTiet);
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTu"), "Chứng từ quyết toán")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTuChiTiet", new { iID_MaCapPhat = iID_MaCapPhat }), "Chi tiết chứng từ quyết toán")%>
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
                            <span>Thông tin cấp phát cho đơn vị <%=strTenDonVi%></span>
                        </td>
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
                    <%Html.RenderPartial("~/Views/CapPhat/ChungTu_DonVi/CapPhat_DonVi_ChungTuChiTiet_Index_DanhSach.ascx", new { ControlID = "ChiTieuChiTiet", MaND = User.Identity.Name }); %>    
                </div>
            </div>
        </div>
    </div>
</div>    
<script type="text/javascript">
    $(document).ready(function () {
        jsCapPhat_Url_Frame = '<%=Url.Action("CapPhatChiTiet_Frame", "CapPhat_ChungTu_DonVi", new { iID_MaCapPhat = iID_MaCapPhat })%>';
    });
</script>
</asp:Content>
