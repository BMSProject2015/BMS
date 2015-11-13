<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String ParentID = "Detail";
        String sMoTa = "";
        String MaND = User.Identity.Name;
        int i;
        String iID_MaChungTu = Request.QueryString["MaID"];
        if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["MaID"]);
        DataTable dt = VayNoModels.getDetail(iID_MaChungTu);
        String NgayChungTu = "", SoChungTu = "", MaDonVi = "", TenDonVi = "", NoiDungVay = "", LoaiTinDung = "", NgayVay = "", LaiXuat = "", MienLai = "", DuVonCu = "", DuLaiCu = "", VayTrongThang = "", HanPhaiTra = "", ThoiGianThuHoi = "", ThuVon = "", ThuLai = "", GhiChu = "", TrangThai = "";
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            NgayChungTu = VayNoModels.getStringNull(VayNoModels.ConvertDateTime(dr["dNgayChungTu"]).ToString("dd/MM/yyyy"));
            SoChungTu = Convert.ToString(dr["sSoChungTu"]);
            MaDonVi = Convert.ToString(dr["iID_MaDonVi"]);
            TenDonVi = Convert.ToString(dr["sTen"]);
            NoiDungVay = Convert.ToString(dr["sTenNoiDung"]);
            LoaiTinDung = Convert.ToString(dr["iID_Loai"]);
            NgayVay = VayNoModels.getStringNull(VayNoModels.ConvertDateTime(dr["dNgayVay"]).ToString("dd/MM/yyyy"));
            LaiXuat = Convert.ToString(dr["rLaiSuat"]) + " %";
            MienLai = Convert.ToString(dr["rMienLai"]) + " %";
            DuVonCu = CommonFunction.DinhDangSo(Convert.ToString(dr["rDuVonCu"]));
            DuLaiCu = CommonFunction.DinhDangSo(Convert.ToString(dr["rDuLaiCu"]));
            VayTrongThang = CommonFunction.DinhDangSo(Convert.ToString(dr["rVayTrongThang"]));
            HanPhaiTra = VayNoModels.getStringNull(VayNoModels.ConvertDateTime(dr["dHanPhaiTra"]).ToString("dd/MM/yyyy"));
            ThoiGianThuHoi = Convert.ToString(dr["rThoiGianThuVon"]) + " tháng";
            ThuVon = CommonFunction.DinhDangSo(Convert.ToString(dr["rThuVon"]));
            ThuLai = CommonFunction.DinhDangSo(Convert.ToString(dr["rThuLai"]));
            GhiChu = Convert.ToString(dr["sMoTa"]);
            TrangThai = Convert.ToString(dr["iID_MaTrangThaiDuyet"]);
        }
        if (dt != null) dt.Dispose();
        dt = VayNoModels.Get_dtCayChiTiet(MaND);
        int iID_MaTrangThaiDuyet_TuChoi = VayNoModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
        int iID_MaTrangThaiDuyet_TrinhDuyet = VayNoModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
        Boolean KiemTra_TrangThaiTrinhDuyet = LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(PhanHeModels.iID_MaPhanHeTinDung, iID_MaTrangThaiDuyet_TrinhDuyet);
    %>
    <div style="width: 20%; float: left;">
        <script src="<%= Url.Content("~/Scripts/jquery.tinyscrollbar.min.js") %>" type="text/javascript"></script>
        <script src="<%= Url.Content("~/Scripts/jquery.cookie.js") %>" type="text/javascript"></script>
        <script src="<%= Url.Content("~/Scripts/jquery.treeview.js") %>" type="text/javascript"></script>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#scrollbar1').tinyscrollbar();
            });
        </script>
        <script type="text/javascript">
            $(function () {
                $("#tree").treeview({
                    collapsed: true,
                    animated: "medium",
                    control: "#sidetreecontrol",
                    persist: "location"
                });
            })
        </script>
        <div class="box_tong" style="background-color: #fff; background-repeat: repeat;">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <div style="width: 60%; float: left;">
                                <span style="text-indent: 5px;">Cây chứng từ</span>
                            </div>
                            <div style="width: 40%; float: right;">
                                <div id="sidetreecontrol" style="text-align: right; margin-right: 5px;">
                                    <%--  <a href="?#">Close</a> | <a href="?#">Open</a>--%>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="Div1">
                <div id="Div2" style="padding-left: 10px;">
                    <div id="scrollbar1">
                        <div class="scrollbar">
                            <div class="track">
                                <div class="thumb">
                                    <div class="end">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="viewport">
                            <div class="overview">
                                <ul id="tree">
                                    <%
                                        for (i = 0; i < dt.Rows.Count; i++)
                                        {
                                            DataRow R = dt.Rows[i];
                                    %>
                                    <li><a href="#"><strong>
                                        <%=MyHtmlHelper.ActionLink(Url.Action("Detail", "VayNo_Duyet", new { MaID = R["iID_Vay"] }).ToString(), Convert.ToString(R["sSoChungTu"]) + " (" + Convert.ToString(R["iID_MaDonVi"]) + " - " + Convert.ToString(R["sTen"]) + ")", "Detail", null, "title=\"Xem chi tiết chứng từ\"")%>
                                    </strong></a></li>
                                    <%} %>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <% if (dt != null) dt.Dispose(); %>
    </div>
    <div style="width: 80%; float: left;">
        <script type="text/javascript">
            $(document).ready(function () {
                $("#tabs").tabs();
            });    
        </script>
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">Chi tiết chứng từ vay nợ</a></li>
                <li><a href="#tabs-2">Lịch sử thao tác</a></li>
            </ul>
            <div id="tabs-1">
                <div id="nhapform">
                    <div id="form2">
                        <table cellpadding="5" cellspacing="5" width="100%" class="table_form3">
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div>
                                        <b>Ngày chứng từ</b>
                                    </div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=  NgayChungTu%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Số chứng từ</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <b>
                                            <%=  SoChungTu%></b>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" style="width: 15%;">
                                    <div>
                                        <b>Mã đơn vị</b>
                                    </div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=  MaDonVi%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Tên đơn vị</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=  TenDonVi%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Nội dung vay</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= NoiDungVay%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Loại tín dụng</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= LoaiTinDung%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Ngày vay</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= NgayVay %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Lãi xuất</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= LaiXuat %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Miễn lãi</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= MienLai %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Dư vốn cũ</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= DuVonCu %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Dư lãi cũ</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= DuLaiCu %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Vay trong tháng</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= VayTrongThang %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Hạn phải trả</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= HanPhaiTra %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Thời gian thu hồi vốn</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= ThoiGianThuHoi %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Thu vốn</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= ThuVon %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Thu lãi</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= ThuLai %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Ghi chú</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%= GhiChu %>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1">
                                    <div>
                                        <b>Trạng thái</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <b>
                                            <%= VayNoModels.getTrangThai( TrangThai)%></b>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <%
                                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                                {    
                            %>
                            <tr>
                                <td class="td_form2_td5">
                                    <div>
                                        <b>Nhận xét</b> &nbsp;<span style="color: Red;">*</span></div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "style=\"width:100%; resize:none;\"")%>
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sMoTa")%>
                                    </div>
                                </td>
                            </tr>
                            <%
                                }%>
                            <tr>
                                <td style="height: 10px;">
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <div style="text-align: center; width: 260px;">
                                        <div style="float: left;">
                                            <%
                                                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                                                {
                                            %>
                                            <div style="float: left;">
                                                <button class='button' style="float: left;" onclick="javascript:location.href='<%=Url.Action("TrinhDuyet", "VayNo_Duyet", new { MaID= iID_MaChungTu})%>';">
                                                    <% if (KiemTra_TrangThaiTrinhDuyet)
                                                       { %>
                                                    Trình duyệt
                                                    <% }
                                                       else
                                                       { %>
                                                    Phê duyệt
                                                    <%} %>
                                                </button>
                                            </div>
                                            <%} %>
                                        </div>
                                        <div style="float: right;">
                                            <div style="float: right; margin-left: 10px;">
                                                <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" /></div>
                                            <%
                                                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                                                {
                                            %>
                                            <div style="float: left;">
                                                <button class='button' style="float: left;" onclick="javascript:location.href='<%=Url.Action("TuChoi", "VayNo_Duyet", new { MaID= iID_MaChungTu})%>';">
                                                    Từ chối
                                                </button>
                                            </div>
                                            <%} %>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div id="tabs-2">
                <%Html.RenderPartial("~/Views/VayNo/VayNo_Duyet_LichSu.ascx", new { ControlID = "VayNo_Duyet", iID_MaChungTu = iID_MaChungTu }); %>
            </div>
        </div>
        <div>
        </div>
    </div>
</asp:Content>
