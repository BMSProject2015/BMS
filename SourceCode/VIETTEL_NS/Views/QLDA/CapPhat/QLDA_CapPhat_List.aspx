<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.QLDA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String iID_MaDotCapPhat = Convert.ToString(ViewData["iID_MaDotCapPhat"]);
    String iID_MaCapPhat = Convert.ToString(ViewData["iID_MaCapPhat"]);
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);

    String ParentID = "QLDA_CapPhat";
    String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

    NameValueCollection data = QLDA_CapPhatModels.LayThongTin(iID_MaDotCapPhat, iID_MaCapPhat, NamLamViec);

    DataTable dtDotCapPhat = QLDA_CapPhatModels.Get_Row_DotCapPhat(iID_MaDotCapPhat, NamLamViec);
    String strThongTinDotCapPhat = Convert.ToString(dtDotCapPhat.Rows[0]["sTen"] + " " + dtDotCapPhat.Rows[0]["iDot"] + " - Ngày/Tháng/Năm: " + CommonFunction.LayXauNgay(Convert.ToDateTime(dtDotCapPhat.Rows[0]["dNgayLap"])));
    dtDotCapPhat.Dispose();

    DataTable dtDuAn = QLDA_DanhMucDuAnModels.ddl_DanhMucDuAn(true);
    SelectOptionList slDuAn = new SelectOptionList(dtDuAn, "iID_MaDanhMucDuAn", "TenHT");
    dtDuAn.Dispose();

    DataTable dtHopDong = QLDA_HopDongModels.Get_DLL_HopDong(true);
    SelectOptionList slHopDong = new SelectOptionList(dtHopDong, "iID_MaHopDong", "sSoHopDong");
    dtHopDong.Dispose();

    DataTable dtNguonNganSach = DanhMucModels.NS_NguonNganSach();
    SelectOptionList slNguonNganSach = new SelectOptionList(dtNguonNganSach, "iID_MaNguonNganSach", "sTen");
    dtNguonNganSach.Dispose();


    String iID_MaTrangThaiDuyet = Convert.ToString(LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DuocSua(QLDAModels.iID_MaPhanHe, User.Identity.Name));

    DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(QLDAModels.iID_MaPhanHe, User.Identity.Name);
    //dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
    //dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    //dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
    //SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
    dtTrangThai.Dispose();

    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(QLDAModels.iID_MaPhanHe);
    dtTrangThai_All.Rows.InsertAt(dtTrangThai_All.NewRow(), 0);
    dtTrangThai_All.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    dtTrangThai_All.Rows[0]["sTen"] = "-- Chọn trạng thái --";
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai_All, "iID_MaTrangThaiDuyet", "sTen");
    dtTrangThai_All.Dispose();

    String sSoPheDuyet = "";
    String iID_MaHopDong = "";
    String strNgayDeNghi = "";
    if (ViewData["DuLieuMoi"] == "1")
    {
        sSoPheDuyet = Convert.ToString(QLDA_CapPhatModels.Get_Max_SoPheDuyetCapPhat() + 1);
        strNgayDeNghi = CommonFunction.LayXauNgay(DateTime.Now);
    }
    else
    {
        sSoPheDuyet = Convert.ToString(data["iSoPheDuyet"]);
        iID_MaHopDong = Convert.ToString(data["iID_MaHopDong"]);
        strNgayDeNghi = CommonFunction.LayXauNgay(Convert.ToDateTime(data["dNgayDeNghi"]));
    }
        
    using (Html.BeginForm("LuuCapPhatSubmit", "QLDA_CapPhat", new { ParentID = ParentID, iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaCapPhat = iID_MaCapPhat }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_CapPhat"), "Đợt cấp phát")%>
            </div>
        </td>
         <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Cấp phát <%=strThongTinDotCapPhat %></span>
                </td>
            </tr>
        </table>
    </div>
</div>
<br />
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Thông tin dự án</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table class="tblhost-filter">
                <tr>
                    <td style="width: 10%">Hợp đồng</td>
                    <td style="width: 40%">
                        <%=MyHtmlHelper.DropDownList(ParentID, slHopDong, data, "iID_MaHopDong", "", "onchange=\"ChonHopDong(this.value)\" class=\"textbox_uploadbox\"")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaHopDong")%>
                        <script type="text/javascript">
                            <%if(iID_MaHopDong != ""){ %>
                            ChonHopDong('<%=iID_MaHopDong %>','<%=NamLamViec %>');
                            <%} %>
                            function ChonHopDong(MaHopDong) {
                                jQuery.ajaxSetup({ cache: false });
                                var url = unescape('<%= Url.Action("get_dtHopDong?ParentID=#0&iID_MaHopDong=#1&iNam=#2", "QLDA_CapPhat") %>');
                                url = unescape(url.replace("#0", "<%= ParentID %>"));
                                url = unescape(url.replace("#1", MaHopDong));
                                url = unescape(url.replace("#2", "<%=NamLamViec %>"));
                                $.getJSON(url, function (data) {
                                    var strData = data.split("#############$$$$$");
                                    document.getElementById("<%= ParentID %>_sTenCongTrinhDuAn").value = strData[0];
                                    document.getElementById("<%= ParentID %>_sTenDonViChuQuan").value = strData[1];
                                    document.getElementById("<%= ParentID %>_sTenChuDauTu").value = strData[2];
                                    document.getElementById("<%= ParentID %>_sTenNhaThau").value = strData[3];
                                    document.getElementById("<%= ParentID %>_sSoTienHopDong").value = strData[4];

                                    document.getElementById("<%= ParentID %>_sChuDauTuTamUng_Last").value = strData[5];
                                    document.getElementById("<%= ParentID %>_sChuDauTuThanhToan_Last").value = strData[6];
                                    document.getElementById("<%= ParentID %>_sChuDauTuThuTamUng_Last").value = strData[7];
                                    document.getElementById("<%= ParentID %>_sPheDuyetTamUng_Last").value = strData[8];
                                    document.getElementById("<%= ParentID %>_sPheDuyetThanhToanTrongNam_Last").value = strData[9];
                                    document.getElementById("<%= ParentID %>_sPheDuyetThanhToanHoanThanh_Last").value = strData[10];
                                    document.getElementById("<%= ParentID %>_sPheDuyetThuTamUng_Last").value = strData[11];
                                    document.getElementById("<%= ParentID %>_sPheDuyetThuKhac_Last").value = strData[12];
                                });
                            }                                            
                        </script>
                    </td>
                    <td style="width: 10%">Dự án</td>
                    <td style="width: 40%">
                        <%=MyHtmlHelper.DropDownList(ParentID, slDuAn, data, "iID_MaDanhMucDuAn", "", "class=\"textbox_uploadbox\"")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDanhMucDuAn")%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">Tên nhà thầu</td>
                    <td style="width: 40%">
                        <%=MyHtmlHelper.TextBox(ParentID, "", "sTenNhaThau", "", "readonly=\"readonly\" class=\"textbox_uploadbox\"")%>
                    </td>
                    <td style="width: 10%">Tên công trình dự án</td>
                    <td style="width: 40%">
                        <%=MyHtmlHelper.TextBox(ParentID, "", "sTenCongTrinhDuAn", "", "readonly=\"readonly\" class=\"textbox_uploadbox\"")%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">Đơn vị chủ quản</td>
                    <td style="width: 40%">
                        <%=MyHtmlHelper.TextBox(ParentID, "", "sTenDonViChuQuan", "", "readonly=\"readonly\" class=\"textbox_uploadbox\"")%>
                    </td>
                    <td style="width: 10%">Chủ đầu tư</td>
                    <td style="width: 40%">
                        <%=MyHtmlHelper.TextBox(ParentID, "", "sTenChuDauTu", "", "readonly=\"readonly\" class=\"textbox_uploadbox\"")%>
                    </td>
                </tr>
            </table>
            <table class="tblhost-filter">
                <tr>
                    <td style="width: 10%">Đơn vị thụ hưởng</td>
                    <td style="width: 23%">
                        <%=MyHtmlHelper.TextBox(ParentID, data, "sDonViThuHuong", "", "class=\"textbox_uploadbox\"")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_sDonViThuHuong")%>
                    </td>
                    <td style="width: 10%">Số tài khoản</td>
                    <td style="width: 24%">
                        <%=MyHtmlHelper.TextBox(ParentID, data, "sSoTaiKhoan", "", "class=\"textbox_uploadbox\"")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_sSoTaiKhoan")%>
                    </td>
                    <td style="width: 10%">Tên ngân hàng</td>
                    <td style="width: 23%">
                        <%=MyHtmlHelper.TextBox(ParentID, data, "sNganHang", "", "class=\"textbox_uploadbox\"")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_sNganHang")%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">Tổng giá trị hợp đồng</td>
                    <td style="width: 23%;">
                        <%=MyHtmlHelper.TextBox(ParentID, "", "sSoTienHopDong", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                    </td>
                    <td style="width: 10%">Số dư tạm ứng</td>
                    <td style="width: 24%">
                        <%=MyHtmlHelper.TextBox(ParentID, "", "sSoDuTamUng", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                    </td>
                    <td style="width: 10%">Số đã thanh toán</td>
                    <td style="width: 23%">
                        <%=MyHtmlHelper.TextBox(ParentID, "", "sSoDaThanhToan", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Thông tin khác</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table class="tblhost-filter">
                <tr>
                    <td style="width: 10%">Số phê duyệt</td>
                    <td style="width: 23%">
                        <%=MyHtmlHelper.TextBox(ParentID, sSoPheDuyet, "sSoPheDuyet", "", "readonly=\"readonly\" class=\"textbox_uploadbox\"")%>
                    </td>
                    <td style="width: 10%">Nguồn ngân sách</td>
                    <td style="width: 24%">
                        <%=MyHtmlHelper.DropDownList(ParentID, slNguonNganSach, data, "iID_MaNguonNganSach", "", "class=\"textbox_uploadbox\"")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNguonNganSach")%>
                    </td>
                    <td style="width: 13%">Ngày đề nghị của chủ đầu tư</td>
                    <td style="width: 20%">
                        <%=MyHtmlHelper.DatePicker(ParentID, strNgayDeNghi, "dNgayDeNghi", "", "class=\"input1_2\"")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayDeNghi")%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<br />
<div style="width: 100%; float: left; margin-bottom: 10px;">
    <div style="width: 50%; float: left">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Năm trước</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <table class="tblhost-filter">
                        <tr>
                            <td colspan="2" class="title" align="center" style="text-align: center;">CHỦ ĐẦU TƯ ĐỀ NGHỊ</td>
                            <td colspan="2" class="title" align="center" style="text-align: center;">PHÊ DUYỆT</td>
                        </tr>
                        <tr>
                            <td style="width: 10%">Tạm ứng</td>
                            <td style="width: 23%">
                                <%=MyHtmlHelper.TextBox(ParentID, "", "sChuDauTuTamUng_Last", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                            </td>
                            <td style="width: 10%">Tạm ứng</td>
                            <td style="width: 24%">
                                <%=MyHtmlHelper.TextBox(ParentID, "", "sPheDuyetTamUng_Last", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">Thanh toán</td>
                            <td style="width: 23%">
                                <%=MyHtmlHelper.TextBox(ParentID, "", "sChuDauTuThanhToan_Last", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                            </td>
                            <td style="width: 10%">KLQT</td>
                            <td style="width: 24%">
                                <%=MyHtmlHelper.TextBox(ParentID, "", "sPheDuyetThanhToanTrongNam_Last", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%"></td>
                            <td style="width: 23%">
                            </td>
                            <td style="width: 10%">KTCTHT</td>
                            <td style="width: 24%">
                                <%=MyHtmlHelper.TextBox(ParentID, "", "sPheDuyetThanhToanHoanThanh_Last", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">Thu tạm ứng</td>
                            <td style="width: 23%">
                                <%=MyHtmlHelper.TextBox(ParentID, "", "sChuDauTuThuTamUng_Last", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                            </td>
                            <td style="width: 10%">Thu tạm ứng</td>
                            <td style="width: 24%">
                                <%=MyHtmlHelper.TextBox(ParentID, "", "sPheDuyetThuTamUng_Last", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%"></td>
                            <td style="width: 23%">
                            </td>
                            <td style="width: 10%">Thu khác</td>
                            <td style="width: 24%">
                                <%=MyHtmlHelper.TextBox(ParentID, "", "sPheDuyetThuKhac_Last", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div style="width: 50%; float: left">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Năm nay</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <table class="tblhost-filter">
                        <tr>
                            <td colspan="2" class="title" align="center" style="text-align: center;">CHỦ ĐẦU TƯ ĐỀ NGHỊ</td>
                            <td colspan="2" class="title" align="center" style="text-align: center;">PHÊ DUYỆT</td>
                        </tr>
                        <tr>
                            <td style="width: 10%">Tạm ứng</td>
                            <td style="width: 23%">
                                <%=MyHtmlHelper.TextBox(ParentID, data, "rChuDauTuTamUng", "", "class=\"textbox_uploadbox\" style=\" text-align: right;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_rChuDauTuTamUng")%>
                            </td>
                            <td style="width: 10%">Tạm ứng</td>
                            <td style="width: 24%">
                                <%=MyHtmlHelper.TextBox(ParentID, data, "rPheDuyetTamUng", "", "class=\"textbox_uploadbox\" style=\" text-align: right;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_rPheDuyetTamUng")%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">Thanh toán</td>
                            <td style="width: 23%">
                                <%=MyHtmlHelper.TextBox(ParentID, data, "rChuDauTuThanhToan", "", "class=\"textbox_uploadbox\" style=\" text-align: right;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_rChuDauTuThanhToan")%>
                            </td>
                            <td style="width: 10%">KLQT</td>
                            <td style="width: 24%">
                                <%=MyHtmlHelper.TextBox(ParentID, data, "rPheDuyetThanhToanTrongNam", "", "class=\"textbox_uploadbox\" style=\" text-align: right;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_rPheDuyetThanhToanTrongNam")%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%"></td>
                            <td style="width: 23%">
                            </td>
                            <td style="width: 10%">KTCTHT</td>
                            <td style="width: 24%">
                                <%=MyHtmlHelper.TextBox(ParentID, data, "rPheDuyetThanhToanHoanThanh", "", "class=\"textbox_uploadbox\" style=\" text-align: right;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_rPheDuyetThanhToanHoanThanh")%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">Thu tạm ứng</td>
                            <td style="width: 23%">
                                <%=MyHtmlHelper.TextBox(ParentID, data, "rChuDauTuThuTamUng", "", "class=\"textbox_uploadbox\" style=\" text-align: right;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_rChuDauTuThuTamUng")%>
                            </td>
                            <td style="width: 10%">Thu tạm ứng</td>
                            <td style="width: 24%">
                                <%=MyHtmlHelper.TextBox(ParentID, data, "rPheDuyetThuTamUng", "", "class=\"textbox_uploadbox\" style=\" text-align: right;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_rPheDuyetThuTamUng")%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%"></td>
                            <td style="width: 23%">
                            </td>
                            <td style="width: 10%">Thu khác</td>
                            <td style="width: 24%">
                                <%=MyHtmlHelper.TextBox(ParentID, data, "rPheDuyetThuKhac", "", "class=\"textbox_uploadbox\" style=\" text-align: right;\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_rPheDuyetThuKhac")%>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Thông tin trạng thái</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table class="tblhost-filter">
                <tr>
                    <td style="width: 10%">Lý do giảm cấp</td>
                    <td style="width: 40%">
                        <%=MyHtmlHelper.TextBox(ParentID, data, "sLyDoGiamCap", "", "class=\"textbox_uploadbox\"")%>
                    </td>
                    <td style="width: 10%">Trạng thái</td>
                    <td style="width: 40%">
                        <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, data, "iID_MaTrangThaiDuyet", "", "class=\"textbox_uploadbox\"")%><br />
                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTrangThaiDuyet")%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<br />
<%
    String strPrevDisabled="disabled=\"disabled\"";
    DataTable dtPrev = QLDA_CapPhatModels.Get_PrevRow_CapPhat(iID_MaDotCapPhat, iID_MaCapPhat, NamLamViec);
    if (dtPrev != null && dtPrev.Rows.Count > 0) { strPrevDisabled = ""; dtPrev.Dispose(); } 
    String strNextDisabled = "disabled=\"disabled\"";
    DataTable dtNext = QLDA_CapPhatModels.Get_NextRow_CapPhat(iID_MaDotCapPhat, iID_MaCapPhat, NamLamViec);
    if (dtNext != null && dtNext.Rows.Count > 0) { strNextDisabled = ""; dtNext.Dispose(); } 
    String strThemMoi = Url.Action("AddCapPhat", "QLDA_CapPhat", new { iID_MaDotCapPhat = iID_MaDotCapPhat });
    String strPrev = Url.Action("btnPrev", "QLDA_CapPhat", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaCapPhat = iID_MaCapPhat, iNam = NamLamViec });
    String strNext = Url.Action("btnNext", "QLDA_CapPhat", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaCapPhat = iID_MaCapPhat, iNam = NamLamViec });
    String strCheck = Url.Action("btnCheck", "QLDA_CapPhat", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaCapPhat = iID_MaCapPhat });
    String strTrinhDuyet = Url.Action("btnTrinhDuyet", "QLDA_CapPhat", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaCapPhat = iID_MaCapPhat });
    String strThongTri = Url.Action("btnThongTri", "QLDA_CapPhat", new { iID_MaDotCapPhat = iID_MaDotCapPhat, iID_MaCapPhat = iID_MaCapPhat });
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td style="width: 50%;">&nbsp;</td>
                <td align="right" style="padding-right: 10px;">
                    <input id="btnTruoc" type="button" <%=strPrevDisabled %> class="button_title_upload" value="Trước" onclick="javascript:location.href='<%=strPrev %>'" />
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button1" type="button" <%=strNextDisabled %> class="button_title_upload" value="Sau" onclick="javascript:location.href='<%=strNext %>'" />
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button2" type="button" class="button_title_upload" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'"/>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button3" type="submit" class="button_title_upload" value="Lưu lại" />
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button4" type="button" class="button_title_upload" value="Kiểm tra" onclick="javascript:location.href='<%=strCheck %>'"/>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button5" type="button" class="button_title_upload" value="Trình duyệt"  onclick="javascript:location.href='<%=strTrinhDuyet %>'"/>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button6" type="button" class="button_title_upload" value="Thông tri"  onclick="javascript:location.href='<%=strThongTri %>'"/>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button7" type="button" class="button_title_upload" value="Thoát" onclick="history.go(-1)"/>
                </td>
            </tr>
        </table>
    </div>
</div>
<%} %>
</asp:Content>
