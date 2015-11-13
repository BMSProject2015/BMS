<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
   <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/jsChiTieu.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>" type="text/javascript"></script>
    <%
    String iID_MaChiTieu = Request.QueryString["iID_MaChiTieu"];
    if (String.IsNullOrEmpty(iID_MaChiTieu)) iID_MaChiTieu = Convert.ToString(ViewData["iID_MaChiTieu"]);
    NameValueCollection data = PhanBo_ChiTieuModels.LayThongTin(iID_MaChiTieu);
    String MaDotPhanBo = Convert.ToString(data["iID_MaDotPhanBo"]);
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_DotPhanBo"), "Đợt phân bổ")%> |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_ChiTieu", new { MaDotPhanBo = MaDotPhanBo }), "Chỉ tiêu phân bổ")%> |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_ChiTieuChiTiet", new { iID_MaChiTieu = iID_MaChiTieu }), "Chi tiết chỉ tiêu phân bổ")%>
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
                            <td  style="width: 70%;">
                                <span>Thông tin chứng từ</span>
                            </td>
                            <td align="right" style="width: 10%;"><span>F2 - Thêm</span></td>
                            <td align="right" style="width: 10%;"><span>Delete - Xóa</span></td>                        
                            <td align="right" style="width: 10%;"><span>F10 - Lưu</span></td>
                            <td style="width: 2%">&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                            <tr>
                                <td class="td_form2_td1" style="width: 15%">
                                    <div><b>Ngày chứng từ</b></div>
                                </td>
                                <td class="td_form2_td5"><div>
                                    <%=String.Format("{0:dd/MM/yyyy}",Convert.ToDateTime(data["dNgayChungTu"]))%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Số chứng từ</b></div></td>
                                <td class="td_form2_td5"><div><%=data["sTienToChungTu"]%> + <%=data["iSoChungTu"]%></div></td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Nội dung chứng từ</b></div></td>
                                <td class="td_form2_td5"><div><%=data["sNoiDung"]%></div></td>
                            </tr>
                        </table>
                        <%Html.RenderPartial("~/Views/PhanBo/ChiTieuChiTiet/ChiTieuChiTiet_Index_DanhSach.ascx", new { ControlID = "ChiTieuChiTiet", MaND = User.Identity.Name }); %>    
                    </div>
                </div>
            </div>
        </div>
    </div>    
    <script type="text/javascript">
        $(document).ready(function () {
            jsChiTieu_iID_MaChiTieu = "<%=iID_MaChiTieu%>";
            jsChiTieu_Url_Frame = '<%=Url.Action("ChiTieuChiTiet_Frame", "PhanBo_ChiTieuChiTiet", new { iID_MaChiTieu = iID_MaChiTieu })%>';
            jsChiTieu_Url = '<%=Url.Action("Index", "PhanBo_ChiTieuChiTiet", new { iID_MaChiTieu = iID_MaChiTieu})%>';
        });
	</script>
</asp:Content>
