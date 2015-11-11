<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/QLDA/jsBang_QLDA_CapPhat.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
    String ParentID = "Edit";
    String iID_MaDotCapPhat = Request.QueryString["iID_MaDotCapPhat"];
    String iID_MaHopDong = Request.QueryString["iID_MaHopDong"];
    String iID_MaDanhMucDuAn = Request.QueryString["iID_MaDanhMucDuAn"];

    if (iID_MaDotCapPhat == null) iID_MaDotCapPhat = "";
    if (iID_MaHopDong == null) iID_MaHopDong = "";
    if (iID_MaDanhMucDuAn == null) iID_MaDanhMucDuAn = "";

    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
    String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);

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

    String sTenChuDauTu = "", sTenNhaThau = "", sNoiDungCapPhat = "", sBanQuanLyDuAn = "", sDonViThuHuong = "", sSoTaiKhoan = "", sNganHang = "", sTenDonViChuQuan = "";
    String sSoDuTamUng = "", sSoDaThanhToan = "", sSoTienHopDong = "", sNgoaiTeHopDong = "";
    String sSoTienHangMucDuAn = "", sNgoaiTeHangMucDuAn = "";
    String sLoaiNgoaiTeHopDong = "", sLoaiNgoaiTeHangMucDuAn = "";

    sNoiDungCapPhat = Convert.ToString(dtDotCapPhat.Rows[0]["sNoiDungCapPhat"]);

    if (iID_MaHopDong != "" && iID_MaDanhMucDuAn != "")
    {
        NameValueCollection data = QLDA_CapPhatModels.LayThongTinHopDong(iID_MaHopDong, iID_MaDanhMucDuAn);
        DataTable dtChuDauTu = QLDA_DonViThiCongModels.Get_Row_Data(data["iID_MaDonViThiCong"]);
        sDonViThuHuong = Convert.ToString(dtChuDauTu.Rows[0]["sTen"]);
        sTenNhaThau = Convert.ToString(dtChuDauTu.Rows[0]["sTen"]);
        sSoTaiKhoan = Convert.ToString(dtChuDauTu.Rows[0]["sSoTaiKhoan"]);
        sNganHang = Convert.ToString(dtChuDauTu.Rows[0]["sTenNganHangGiaoDich"]);
        dtChuDauTu.Dispose();
        
        DataTable dtTienHopDong =  QLDA_HopDongModels.Get_Sum_Tien_HopDong(iID_MaHopDong);
        sSoTienHopDong = CommonFunction.DinhDangSo(Convert.ToString(dtTienHopDong.Rows[0]["rSoTien"]));
        sNgoaiTeHopDong = CommonFunction.DinhDangSo(Convert.ToString(dtTienHopDong.Rows[0]["rNgoaiTe"]));
        sLoaiNgoaiTeHopDong = Convert.ToString(dtTienHopDong.Rows[0]["sTenNgoaiTe"]);
        dtTienHopDong.Dispose();

        DataTable dtTienDuAn = QLDA_HopDongModels.Get_Sum_Tien_HopDong_Duan(iID_MaHopDong, iID_MaDanhMucDuAn);
        sSoTienHangMucDuAn = CommonFunction.DinhDangSo(Convert.ToString(dtTienDuAn.Rows[0]["rSoTien"]));
        sNgoaiTeHangMucDuAn = CommonFunction.DinhDangSo(Convert.ToString(dtTienDuAn.Rows[0]["rNgoaiTe"]));
        sLoaiNgoaiTeHangMucDuAn = Convert.ToString(dtTienDuAn.Rows[0]["sTenNgoaiTe"]);
        dtTienDuAn.Dispose();

        DataTable dtDuAnHT = QLDA_DanhMucDuAnModels.Row_DanhMucDuAn(iID_MaDanhMucDuAn);
        if (dtDuAnHT.Rows.Count > 0 && dtDuAnHT!=null)
        {
            sTenDonViChuQuan = Convert.ToString(dtDuAnHT.Rows[0]["sTenDonVi"]);
            sTenChuDauTu = Convert.ToString(dtDuAnHT.Rows[0]["sTenChuDauTu"]);
            sBanQuanLyDuAn = Convert.ToString(dtDuAnHT.Rows[0]["sTenBanQuanLy"]);   
        }
        if (dtDuAnHT != null)
            dtDuAnHT.Dispose();

        sSoDuTamUng =  CommonFunction.DinhDangSo(QLDA_CapPhatModels.GetSoDuTamUng(iID_MaDanhMucDuAn));
        sSoDaThanhToan = CommonFunction.DinhDangSo(QLDA_CapPhatModels.GetSoDaThanhToan(iID_MaDanhMucDuAn));
    }
    
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_CapPhat"), "Đợt cấp phát")%>
            </div>
        </td>
         <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#dvContent').slideToggle('slow');
        $('#pHeader').click(function () {
            $('#dvContent').slideToggle('slow');
        });
    });
    $(document).ready(function () {
        $("DIV.ContainerPanel > DIV.collapsePanelHeader > DIV.ArrowExpand").toggle(
            function () {
                $(this).parent().next("div.Content").show("slow");
                $(this).attr("class", "ArrowClose");
            },
            function () {
                $(this).parent().next("div.Content").hide("slow");
                $(this).attr("class", "ArrowExpand");
            });
    });            
</script>
<div id="ContainerPanel" class="ContainerPanel">
    <div id="pHeader" class="collapsePanelHeader"> 
        <div id="dvHeaderText" class="HeaderContent" style="width: 80%;">
            <div style="width: 100%; float: left;">
                <span><%=NgonNgu.LayXau("Thông tin hợp đồng")%></span>
            </div>
        </div>
        <div id="dvArrow" class="ArrowExpand"></div>
    </div>
    <div id="dvContent" class="Content" style="display:none">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" align="left" style="width: 50%;">
                    <div id="nhapform">
                        <div id="form2">
                            <table class="tblhost-filter">
                                <tr>
                                    <td style="width: 10%">Hợp đồng</td>
                                    <td style="width: 23%">
                                        <input type="hidden" id="sMaHopDong" name="sMaHopDong" value="<%=iID_MaHopDong %>" />
                                        <%=MyHtmlHelper.DropDownList(ParentID, slHopDong, iID_MaHopDong, "iID_MaHopDong", "", "onchange=\"ChonHopDong(this.value)\" class=\"textbox_uploadbox\"")%><br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaHopDong")%>
                                        <script type="text/javascript">                           
                                            function ChonHopDong(MaHopDong) {
                                                jQuery.ajaxSetup({ cache: false });
                                                document.getElementById("sMaHopDong").value = MaHopDong;
                                                var url = unescape('<%= Url.Action("get_dtMucLucDuan?ParentID=#0&iID_MaHopDong=#1&iID_MaDanhMucDuAn=#2", "QLDA_CapPhat") %>');
                                                url = unescape(url.replace("#0", "<%= ParentID %>"));
                                                url = unescape(url.replace("#1", MaHopDong));
                                                url = unescape(url.replace("#2", ''));
                                                $.getJSON(url, function (data) {
                                                    document.getElementById("<%= ParentID %>_tdMucLucDuAn").innerHTML = data;
                                                });
                                            }                                            
                                        </script>
                                        <script type="text/javascript">
                                            function ddlHangMucDuan_SelectedValueChanged(ctl) {
                                                var sMaHopDong = document.getElementById("sMaHopDong").value;
                                                var url = "<%=Url.Action("Detail", "QLDA_CapPhat", new {iID_MaDotCapPhat = iID_MaDotCapPhat})%>";
                                                url += "&iID_MaHopDong=" + sMaHopDong;
                                                if(ctl.selectedIndex>=0)
                                                {
                                                    var value = ctl.options[ctl.selectedIndex].value;
                                                    if(value!="")
                                                    {
                                                        url += "&iID_MaDanhMucDuAn=" + value;
                                                    }
                                                }
                                                location.href = url;
                                            }
                                        </script>
                                    </td>
                                    <td style="width: 10%">Dự án</td>
                                    <td style="width: 24%" id="<%= ParentID %>_tdMucLucDuAn">
                                        <%= QLDA_CapPhatController.get_objMucLucDuan(ParentID,iID_MaHopDong,iID_MaDanhMucDuAn)%>
                                    </td>
                                    <td style="width: 10%">Chủ đầu tư</td>
                                    <td style="width: 23%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sTenChuDauTu, "sTenChuDauTu", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\"background:#ebebeb;\"")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">Tên nhà thầu</td>
                                    <td style="width: 23%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sTenNhaThau, "sTenNhaThau", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\"background:#ebebeb;\"")%>
                                    </td>
                                    <td style="width: 10%">Đơn vị chủ quản</td>
                                    <td style="width: 24%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sTenDonViChuQuan, "sTenDonViChuQuan", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\"background:#ebebeb;\"")%>    
                                    </td>
                                    <td style="width: 10%">BQL Dự án</td>
                                    <td style="width: 23%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sBanQuanLyDuAn, "sBanQuanLyDuAn", "", "class=\"textbox_uploadbox\" style=\"background:#ebebeb;\"")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">Đơn vị thụ hưởng</td>
                                    <td style="width: 23%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sDonViThuHuong, "sDonViThuHuong", "", "class=\"textbox_uploadbox\" style=\"background:#ebebeb;\"")%><br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sDonViThuHuong")%>
                                    </td>
                                    <td style="width: 10%">Số tài khoản</td>
                                    <td style="width: 24%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sSoTaiKhoan, "sSoTaiKhoan", "", "class=\"textbox_uploadbox\" style=\"background:#ebebeb;\"")%>
                                    </td>
                                    <td style="width: 10%">Tên ngân hàng</td>
                                    <td style="width: 23%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sNganHang, "sNganHang", "", "class=\"textbox_uploadbox\" style=\"background:#ebebeb;\"")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">Tổng giá trị hợp đồng</td>
                                    <td style="width: 23%;">
                                        <%=MyHtmlHelper.TextBox(ParentID, sSoTienHopDong, "sSoTienHopDong", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\"background:#ebebeb; text-align: right;\"")%>
                                    </td>
                                    <td style="width: 10%">Tổng ngoại tệ hợp đồng</td>
                                    <td style="width: 24%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sNgoaiTeHopDong, "sNgoaiTeHopDong", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\"background:#ebebeb; text-align: right;\"")%>   
                                    </td>
                                    <td style="width: 10%">Loại ngoại tệ hợp đồng</td>
                                    <td style="width: 23%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sLoaiNgoaiTeHopDong, "sLoaiNgoaiTeHopDong", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\"background:#ebebeb;\"")%>    
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">Giá trị của hạng mục</td>
                                    <td style="width: 23%;">
                                        <%=MyHtmlHelper.TextBox(ParentID, sSoTienHangMucDuAn, "sSoTienHangMucDuAn", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\"background:#ebebeb; text-align: right;\"")%>
                                    </td>
                                    <td style="width: 10%">Ngoại tệ của hạng mục</td>
                                    <td style="width: 24%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sNgoaiTeHangMucDuAn, "sNgoaiTeHangMucDuAn", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\"background:#ebebeb; text-align: right;\"")%>    
                                    </td>
                                    <td style="width: 10%">Loại ngoại tệ của hạng mục</td>
                                    <td style="width: 23%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sLoaiNgoaiTeHangMucDuAn, "sLoaiNgoaiTeHangMucDuAn", "", "readonly=\"readonly\" class=\"textbox_uploadbox\" style=\"background:#ebebeb;\"")%>    
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">Nội dung cấp phát</td>
                                    <td style="width: 23%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sNoiDungCapPhat, "sNoiDungCapPhat", "", "readonly=\"readonly\" class=\"textbox_uploadbox\"")%>    
                                    </td>
                                    <td style="width: 10%">Số dư tạm ứng</td>
                                    <td style="width: 24%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sSoDuTamUng, "sSoDuTamUng", "", "class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>    
                                    </td>
                                    <td style="width: 10%">Số đã thanh toán</td>
                                    <td style="width: 23%">
                                        <%=MyHtmlHelper.TextBox(ParentID, sSoDaThanhToan, "sSoDaThanhToan", "", "class=\"textbox_uploadbox\" style=\" text-align: right;\"")%>   
                                    </td>
                                </tr>
                            </table>    
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<br />
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Thông tin cấp phát</span>
                        </td>
                        <td align="right">
                            <span>F2: Thêm hàng -- DELETE: Xóa Hàng -- F10: Lưu thông tin -- Space: Sửa thông tin</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">         
                    <%Html.RenderPartial("~/Views/QLDA/CapPhat/QLDA_CapPhat_Index_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>
