<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models.DuToanBS" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <script src="<%= Url.Content("~/Scripts/jsDuToan_Gom.js?id=") %><%=DateTime.Now.ToString("yyMMddHHmmss") %>"
        type="text/javascript"></script>
        
    <%
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
        String ChiNganSach = Convert.ToString(ViewData["ChiNganSach"]);
        if (String.IsNullOrEmpty(ChiNganSach)) ChiNganSach = Request.QueryString["ChiNganSach"];
        String MaDotNganSach = Request.QueryString["MaDotNganSach"];
      
       
        NameValueCollection  data = DuToan_ChungTuModels.LayThongTin_Gom(iID_MaChungTu);
        
        int iID_MaTrangThaiDuyet = Convert.ToInt32(data["iID_MaTrangThaiDuyet"]);
        string MaND = User.Identity.Name;

        String sLNS = Convert.ToString(Request.QueryString["sLNS"]);
        if (String.IsNullOrEmpty(sLNS))
        {
            sLNS = data["sDSLNS"];
        }
        String iID_MaDonVi = Convert.ToString(Request.QueryString["iID_MaDonVi"]);
        int iID_MaTrangThaiDuyet_TuChoi = 0;
        int iID_MaTrangThaiDuyet_TrinhDuyet = 0;
             iID_MaTrangThaiDuyet_TuChoi = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_TuChoi(MaND, iID_MaChungTu);
             iID_MaTrangThaiDuyet_TrinhDuyet = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_TrinhDuyet(MaND, iID_MaChungTu);
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToan_DotNganSach", new {ChiNganSach=ChiNganSach }), "Đợt ngân sách")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToan_ChungTu", new { ChiNganSach = ChiNganSach,MaDotNganSach = MaDotNganSach }), "Danh sách chứng từ")%>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div style="width: 100%; float: left;">
        <%--<div id="divTree" style="float: left; position: relative; width: 18%;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>TÌM KIẾM NHANH</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <%Html.RenderPartial("~/Views/DuToan/ChungTuChiTiet/ChungTuChiTiet_Index_Cay.ascx"); %>
                </div>
            </div>
        </div>--%>
        <div id="divChungTuChiTietHT" style="float: right; position: relative; width: 100%;">
            <table width="100%" cellpadding="2" cellspacing="2">
                <tr>
                    
                    <td>
                        <div class="box_tong">
                            <div id="nhapform">
                                <div id="form2">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                        <tr>
                                            <%--<td class="td_form2_td1" style="width: 80px;">
                                                <div>
                                                    <b>Đợt ngày:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 60px;">
                                                <div>
                                                    <%=String.Format("{0:dd/MM/yyyy}",Convert.ToDateTime(data["dNgayChungTu"]))%></div>
                                            </td>
                                            <td class="td_form2_td1" style="width: 70px;">
                                                <div>
                                                    <b>Loại NS:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 200px;">
                                                <div>
                                                    <%=data["sMoTa"]%></div>
                                            </td>--%>
                                            <td class="td_form2_td1" style="width: 10%">
                                                <div>
                                                    <b>BQL:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 30%">
                                                <div>
                                                    <%=data["sTenPhongBan"]%></div>
                                            </td>
                                            <td class="td_form2_td1" style="width: 15%">
                                                <div>
                                                    <b>Nội dung đợt:</b></div>
                                            </td>
                                            <td class="td_form2_td5" style="width: 45%">
                                                <div>
                                                    <%=data["sNoiDung"]%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <div style="width: 100%; float: left; margin-top: 2px;">
                            <div class="box_tong">
                                <div class="title_tong">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                         <tr>
                        <td>
                            <span>Thông tin chứng từ</span>
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
                                <%Html.RenderPartial("~/Views/DuToan/ChungTuChiTiet/ChungTuChiTiet_Index_DanhSach_Gom.ascx", new { ControlID = "ChungTuChiTiet_Index_DanhSach", MaND = User.Identity.Name, ChucNangCapNhap = true, sLNS = sLNS }); %>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            jsDuToan_iID_MaChungTu = "<%=iID_MaChungTu%>";
            jsDuToan_Url_Frame = '<%=Url.Action("ChungTuChiTiet_Frame", "DuToan_ChungTuChiTiet_Gom", new { iID_MaChungTu = iID_MaChungTu })%>';
            jsDuToan_Url = '<%=Url.Action("Index", "DuToan_ChungTuChiTiet_Gom", new { iID_MaChungTu = iID_MaChungTu , iID_MaDonVi=iID_MaDonVi, sLNS=sLNS})%>';
            $("#tabs").tabs();
        });
    </script>
</asp:Content>
