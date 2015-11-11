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
<script src="<%= Url.Content("~/Scripts/TCDN/jsBang_TaiChinhDoanhNghiep.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   

<%
    String iID_MaDoanhNghiep = Convert.ToString(ViewData["iID_MaDoanhNghiep"]);
    String iQuy = Convert.ToString(ViewData["iQuy"]);
    if (iQuy == null && iQuy == "")
    {
        iQuy = "1";
    }
    String ParentID = "Edit";
    
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
    String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    
    String sTenDoanhNghiep = "", sTenThuongGoi = "", sTenGiaoDich = "", sTenTheoQuocPhong = "", sQuyetDinhThanhLap = "", sSoDanhKy = "",
    dNgayDangKy = "", sNganhNgheKinhDoanh = "", sDiaChi = "", sDienThoai = "", sFax = "", sWebsite = "", sEmail = "",
    sMaChungKhoan = "", rVonDieuLe = "", rVonNhaNuoc = "", rVonChuSoHuu = "", sNguoiDaiDienVonNhaNuoc = "", sSoCoPhan = "",
    sChucVu = "", iID_MaLoaiDoanhNghiep = "", iID_MaNhomDoanhNghiep = "", sGhiChu = "";
    Boolean bHoatDong = false;

    DataTable dt = TCSN_DoanhNghiepModels.Get_DoanhNghiep_Row(iID_MaDoanhNghiep);
    
    if (dt.Rows.Count > 0) { 
        DataRow R = dt.Rows[0];
        sTenDoanhNghiep = Convert.ToString(R["sTenDoanhNghiep"]);
        sTenThuongGoi = Convert.ToString(R["sTenThuongGoi"]);
        sTenGiaoDich = Convert.ToString(R["sTenGiaoDich"]);
        sTenTheoQuocPhong = Convert.ToString(R["sTenTheoQuocPhong"]);
        sQuyetDinhThanhLap = Convert.ToString(R["sQuyetDinhThanhLap"]);
        sSoDanhKy = Convert.ToString(R["sSoDanhKy"]);
        dNgayDangKy = Convert.ToString(R["dNgayDangKy"]);
        sNganhNgheKinhDoanh = Convert.ToString(R["sNganhNgheKinhDoanh"]);
        sDiaChi = Convert.ToString(R["sDiaChi"]);
        sDienThoai = Convert.ToString(R["sDienThoai"]);
        sFax = Convert.ToString(R["sFax"]);
        sWebsite = Convert.ToString(R["sWebsite"]);
        sEmail = Convert.ToString(R["sEmail"]);
        sMaChungKhoan = Convert.ToString(R["sMaChungKhoan"]);
        rVonDieuLe = Convert.ToString(R["rVonDieuLe"]);
        rVonNhaNuoc = Convert.ToString(R["rVonNhaNuoc"]);
        rVonChuSoHuu = Convert.ToString(R["rVonChuSoHuu"]);
        sNguoiDaiDienVonNhaNuoc = Convert.ToString(R["sNguoiDaiDienVonNhaNuoc"]);
        sSoCoPhan = Convert.ToString(R["sSoCoPhan"]);
        sChucVu = Convert.ToString(R["sChucVu"]);
        iID_MaLoaiDoanhNghiep = Convert.ToString(R["iID_MaLoaiDoanhNghiep"]);
        iID_MaNhomDoanhNghiep = Convert.ToString(R["iID_MaNhomDoanhNghiep"]);
        sGhiChu = Convert.ToString(R["sGhiChu"]);
        bHoatDong = Convert.ToBoolean(R["bHoatDong"]);
    }
    String tgHoatDong = "";
    if (bHoatDong == true)
    {
        tgHoatDong = "on";
    }

    DataTable dtQuy = DanhMucModels.DT_Quy();
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    dtQuy.Dispose();

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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_DoanhNghiep"), "Danh sách doanh nghiệp")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                    <span>Hồ sơ doanh nghiệp năm <%=iNam%></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2" style="padding: 10px;">
            <table class="tblhost-filter">
               <tr>
                    <td>
                        1. Tên doanh nghiệp: <%=sTenDoanhNghiep %>
                    </td>
                </tr>
                <tr>
                    <td>
                        2. Giấy đăng ký kinh doanh số : <%=sSoDanhKy %>. Ngày: <%=dNgayDangKy %>
                    </td>
                </tr>  
                <tr>
                    <td>
                        3. Ngành nghề kinh doanh chính: <%=sNganhNgheKinhDoanh %>
                    </td>
                </tr> 
                <tr>
                    <td>
                        4. Địa chỉ: <%=sDiaChi %>. Điện thoại: <%=sDienThoai %>. Fax: <%=sFax %>
                    </td>
                </tr>
                <tr>
                    <td>
                        5. Mã chứng khoán: <%=sMaChungKhoan %>
                    </td>
                </tr>
                <tr>
                    <td>
                        6. Vốn điều lệ: <%=CommonFunction.DinhDangSo(rVonDieuLe) %><br />
                        &nbsp;&nbsp;&nbsp;Trong đó: Vốn nhà nước: <%=CommonFunction.DinhDangSo(rVonNhaNuoc) %>
                    </td>
                </tr> 
                <tr>
                    <td>
                        7. Người đại diện vốn nhà nước:<br />
                        - Họ và tên: <%=sNguoiDaiDienVonNhaNuoc %>.<br />
                        - Số cổ phần được giao đại diện: <%=sSoCoPhan %><br />
                        - Chức vụ tham gia ban quản lý điều hành doanh nghiệp: <%=sChucVu %>
                    </td>
                </tr> 
            </table><br />
            <table cellpadding="5" cellspacing="5" width="100%">
                <tr>
                    <td class="td_form2_td1" style="width: 20%">
                        <div><b>Quý làm việc</b></div>
                    </td>
                    <td class="td_form2_td5"  style="width: 80%">
                        <div>
                            <script type="text/javascript">
                                function ddlQuy_SelectedValueChanged(ctl) {
                                    var url = "<%=Url.Action("Edit", "TCDN_HoSoDoanhNghiep", new {iID_MaDoanhNghiep=iID_MaDoanhNghiep})%>";
                                    if(ctl.selectedIndex>=0)
                                    {
                                        var value = ctl.options[ctl.selectedIndex].value;
                                        if(value!="")
                                        {
                                            url += "&iQuy=" + value;
                                        }
                                    }
                                    location.href = url;
                                }
                            </script>
                            <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "onChange=\"ddlQuy_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <%Html.RenderPartial("~/Views/TCDN/HoSoDoanhNghiep/TCDN_HoSoDoanhNghiep_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>    
        </div>
    </div>
</div><br />
</asp:Content>
