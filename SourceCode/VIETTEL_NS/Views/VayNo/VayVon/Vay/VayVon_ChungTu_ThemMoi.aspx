<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        String MaND = User.Identity.Name;
        int Thang = DanhMucModels.ThangLamViec(MaND);
        int Nam = DanhMucModels.NamLamViec(MaND);
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];

        if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]); ;
        NameValueCollection data = VayNoModels.LayThongTin(iID_MaChungTu);
        int iID_MaTrangThaiDuyet = Convert.ToInt32(data["iID_MaTrangThaiDuyet"]);      
        int iID_MaTrangThaiDuyet_TuChoi = VayNoModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);

        int iID_MaTrangThaiDuyet_TrinhDuyet = VayNoModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
        string TrangThaiDuyet = LuongCongViecModel.TrangThaiDuyet(iID_MaTrangThaiDuyet);
    %>
    <div style="width: 100%; float: left; margin-top: 10px;">
        <div style="width: 15%; float: left;">
            <%Html.RenderPartial("~/Views/VayNo/VayVon/Vay/VayNo_Duyet_CayChungTu.ascx", new { ControlID = "Tree", MaND = User.Identity.Name, Thang = Thang, Nam = Nam }); %>
        </div>
        <div style="width: 85%; float: left;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Thêm mới chứng từ</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                            <tr>
                                <td class="td_form2_td1" style="width: 15%">
                                    <div>
                                        <b>Ngày chứng từ</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data["dNgayChungTu"]))%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Số chứng từ</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=data["sSoChungTu"]%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Nội dung chứng từ</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=data["sNoiDung"]%></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Trạng thái chứng từ</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <b>
                                            <%=TrangThaiDuyet%></b></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                    </div>
                                </td>
                                <td class="td_form2_td5">
                                    <%
                                        if (iID_MaTrangThaiDuyet_TuChoi > 0)
                                        {
                                    %>
                                    <div style="float: left; padding-right: 10px;">
                                        <button class='button' style="float: left;" onclick="javascript:location.href='<%=Url.Action("TuChoi", "VayNo_ChungTuChiTiet", new { iID_MaChungTu= iID_MaChungTu})%>';">
                                            Từ chối</button>
                                    </div>
                                    <%
                                        }
                                    %>
                                    <%
                                        if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                                        {
                                    %>
                                    <div style="float: left;">
                                        <button class='button' style="float: left;" onclick="javascript:location.href='<%=Url.Action("TrinhDuyet", "VayNo_ChungTuChiTiet", new { iID_MaChungTu= iID_MaChungTu })%>';">
                                            Trình duyệt</button>
                                    </div>
                                    <%
                                        }
                                    %>
                                </td>
                            </tr>
                        </table>
                        <%Html.RenderPartial("~/Views/VayNo/VayVon/Vay/VayVon_ChungTu_DanhSach_Nhap.ascx", new { ControlID = "Nhap", MaND = User.Identity.Name }); %>
                    </div>
                </div>
            </div>
            <br />
            <div style="width: 100%; float: left;">
                <script type="text/javascript">
                    $(document).ready(function () {
                        $("#tabs").tabs();
                    });    
                </script>
                <div id="tabs">
                    <ul>
                        <li><a href="#tabs-1">Chi tiết vay vốn</a></li>
                       <%-- <li><a href="#tabs-2">Quá trình trả vốn</a></li>--%>
                        <li><a href="#tabs-3">Lịch sử chứng từ</a></li>
                    </ul>
                    <div id="tabs-1">
                        <%Html.RenderPartial("~/Views/VayNo/VayVon/Vay/VayVon_ChungTu_DanhSach_Sua.ascx", new { ControlID = "Duyet", iID_MaChungTu = iID_MaChungTu, MaND = User.Identity.Name, ChucNangCapNhap = true }); %>
                    </div>
                    <%--<div id="tabs-2">
                    </div>--%>
                    <div id="tabs-3">
                        <%Html.RenderPartial("~/Views/VayNo/VayVon/Vay/VayNo_Duyet_LichSu.ascx", new { ControlID = "View", iID_MaChungTu = iID_MaChungTu, MaND = User.Identity.Name }); %>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
