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
    int i;
    String ParentID = "SanPham_SanPham_List";
    String MaND = User.Identity.Name;
    String sTen = Request.QueryString["sTen"];
    String dTuNgay = Request.QueryString["TuNgay"];
    String dDenNgay = Request.QueryString["DenNgay"];
    String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
    String page = Request.QueryString["page"];

    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet ) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";

    DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
    dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
    dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");

    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(QuyetToanModels.iID_MaPhanHeQuyetToan);
    DataTable dt = SanPham_VatTuModels.Get_DanhSachSanPham(MaND, sTen, dTuNgay, dDenNgay, CurrentPage, Globals.PageSize);

    double nums = SanPham_VatTuModels.Get_DanhSachSanPham_Count(MaND, sTen, dTuNgay, dDenNgay);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {
        MaND = MaND,
        sTen = sTen,
        TuNgay = dTuNgay,
        DenNgay = dDenNgay,
        page = x
    }));

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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham"), "Sản phẩm")%>
            </div>
        </td>
    </tr>
</table>
<br />
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách sản phẩm đã tạo</span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: 10%;" align="center">Đơn vị</th>
            <th style="width: 10%;" align="center">Loại hình sản phẩm</th>
            <th style="width: 10%;" align="center">Tên sản phẩm</th>
            <th style="width: 10%;" align="center">Đơn vị tính</th>
            <th style="width: 15%;" align="center">Số lượng</th>
            <th style="width: 28%;" align="center">Quy cách</th>
            <th style="width: 7%;" align="center">Ngày lập</th>
            <th style="width: 5%;" align="center">Trạng thái</th>
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;
            String NgayLap = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayLap"]));
            String sTrangThai = "";
            String strColor = "";
            for (int j = 0; j < dtTrangThai_All.Rows.Count; j++)
            {
                if (Convert.ToString(R["iID_MaTrangThaiDuyet"]) == Convert.ToString(dtTrangThai_All.Rows[j]["iID_MaTrangThaiDuyet"]))
                {
                    sTrangThai = Convert.ToString(dtTrangThai_All.Rows[j]["sTen"]);
                    strColor = String.Format("style='background-color: {0}; background-repeat: repeat;'", dtTrangThai_All.Rows[j]["sMauSac"]);
                    break;
                }
            }
            
            //Lấy tên đơn vị
            String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));            
            
            //Lấy thông tin loại hình sản phẩm
            String strLoaiHinh = Convert.ToString(R["iID_MaLoaiHinh"]);

            //Lấy thông tin đơn vị tính
            String strDonViTinh = Convert.ToString(R["iDM_MaDonViTinh"]);

            String strEdit = "";
            String strDelete = "";
            //if (LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND) &&
            //                    LuongCongViecModel.KiemTra_TrangThaiKhoiTao(QuyetToanModels.iID_MaPhanHeQuyetToan, Convert.ToInt32(R["iID_MaTrangThaiDuyet"])))
            //{
                strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "SanPham", new { iID_MaSanPham = R["iID_MaSanPham"]}).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "SanPham", new { iID_MaSanPham = R["iID_MaSanPham"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            //}       
            %>
            <tr <%=strColor %>>
                <td align="center"><%=STT%></td>  
                <td align="left"><%=strTenDonVi%></td>    
                <td align="center"><%=strLoaiHinh%></td>
                <td align="center"><%=R["sTen"]%></td>
                <td align="center"><%=strDonViTinh%></td>
                <td align="left"><%=R["rSoLuong"]%></td>
                <td align="center"><%=R["sQuyCach"]%></td>
                <td align="center"><%=NgayLap %></td>
                <td align="center"><%=sTrangThai%></td>
                <td align="center"><%=strEdit%></td>
                <td align="center"><%=strDelete%></td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="11" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%  
dt.Dispose();
dtTrangThai_All.Dispose();
dtTrangThai.Dispose();
%>
</asp:Content>



