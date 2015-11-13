<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/TCDN/jsTCDN_ChungTuChiTiet.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
   <style type="text/css">

#menu1 ul{
  background: url("images/ui-bg_gloss-wave_55_5c9ccc_500x100.png") repeat-x scroll 50% 50% #5c9ccc;
    border: 1px solid #4297d7;
    color: #fff;
    border-radius: 5px;
    font-weight: bold;
      margin: 0;
    padding: 0em 0.2em 0;
}
 
#menu1 ul li
{
     float: left;
    padding: 4px 2em;
    text-decoration: none;
    cursor: pointer;
display:inline;
}

#menu1 ul a
{
  
}
   </style>
    <%
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
        NameValueCollection data = TCDN_ChungTuModels.LayThongTin(iID_MaChungTu);
        String strTenDonVi = TCSN_DoanhNghiepModels.Get_TenDonVi(data["iID_MaDoanhNghiep"]);
        String iLoai = Request.QueryString["iLoai"];
        if (String.IsNullOrEmpty(iLoai)) iLoai = "1";
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_ChungTu", new { iLoai = iLoai }), "Danh sách chứng từ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu }), "Chi tiết chứng từ")%>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; float: left;">
        <div style="width: 100%; float: left;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Thông tin chứng từ</span>
                            </td>
                            <td align="right" style="width: 100px;">
                                <span>F10: Lưu</span>
                            </td>
                            <td align="right" style="width: 140px;">
                                <span>Backspace: Sửa </span>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                        <tr>
                                            <td class="td_form2_td1" style="width: 10%">
                                                <div>
                                                    <b>Số chứng từ</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 10%">
                                                <div>
                                                    <b>
                                                        <%=data["sTienToChungTu"]%><%=data["iSoChungTu"]%></b></div>
                                            </td>
                                      
                                            <td class="td_form2_td1" style="width: 5%">
                                                <div>
                                                    <b>Quý</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 5%">
                                                <div>
                                                    <b>
                                                        <%=data["iQuy"]%></b></div>
                                            </td>
                                        
                                            <td class="td_form2_td1" style="width: 10%">
                                                <div>
                                                    <b>Ngày chứng từ</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 10%">
                                                <div>
                                                    <b>
                                                        <%=String.Format("{0:dd/MM/yyyy}",Convert.ToDateTime(data["dNgayChungTu"]))%></b></div>
                                            </td>
                                     
                                            <td class="td_form2_td1" style="width: 10%">
                                                <div>
                                                    <b>Doanh nghiệp</b></div>
                                            </td>
                                            <td class="td_form2_td5">
                                                <div>
                                                    <b>
                                                        <%=strTenDonVi%></b></div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                        <tr>
                                            <td class="td_form2_td1" style="width: 10%">
                                                <div>
                                                    <b>Nội dung chứng từ</b></div>
                                            </td>
                                            <td class="td_form2_td5" colspan="7">
                                                <div>
                                                    <b>
                                                        <%=data["sNoiDung"]%></b></div>
                                            </td>
                                      
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <div id="menu1">
                            <%=TCDN_ChungTuChiTietModels.GetMeNuTCDN(iID_MaChungTu,iLoai)%>
                        </div>
                        <div style="clear:both ">
                         <%Html.RenderPartial("~/Views/TCDN/ChungTuChiTiet/TCDN_ChungTuChiTiet_Index_DanhSach.ascx", new { ControlID = "ChiTieuChiTiet", MaND = User.Identity.Name, iLoai = iLoai }); %>
                         </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
