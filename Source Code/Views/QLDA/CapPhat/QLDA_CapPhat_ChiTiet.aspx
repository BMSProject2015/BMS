<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/QLDA/jsBang_QLDA_CapPhatChiTiet.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
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
        sTenDonViChuQuan = Convert.ToString(dtDuAnHT.Rows[0]["sTenDonVi"]);
        sTenChuDauTu = Convert.ToString(dtDuAnHT.Rows[0]["sTenChuDauTu"]);
        sBanQuanLyDuAn = Convert.ToString(dtDuAnHT.Rows[0]["sTenBanQuanLy"]);        
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
    </tr>
</table>
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
                            <span>DELETE: Xóa Hàng -- F10: Lưu thông tin -- Space: Sửa thông tin       </span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">         
                    <%Html.RenderPartial("~/Views/QLDA/CapPhat/QLDA_CapPhat_ChiTiet_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>
                </div>
            </div>
        </div>
    </div>
</div>    
</asp:Content>
