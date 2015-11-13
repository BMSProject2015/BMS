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
    String ParentID = "SanPham_ChiTietGia_List";
    String MaND = User.Identity.Name;
    String iID_MaSanPham = Request.QueryString["iID_MaSanPham"];
    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }
    String sTen = "";
    DataTable dtSanPham = SanPham_VatTuModels.Get_SanPham(iID_MaSanPham);
    if (dtSanPham.Rows.Count > 0) {
        sTen = HamChung.ConvertToString(dtSanPham.Rows[0]["sTen"]);
    }

    String iID_LoaiDonVi = Request.QueryString["iID_LoaiDonVi"];
    if (String.IsNullOrEmpty(iID_LoaiDonVi)) iID_LoaiDonVi = Convert.ToString(ViewData["iID_LoaiDonVi"]);
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
    if (String.IsNullOrEmpty(iID_MaDonVi)) iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    String iID_MaLoaiHinh = Request.QueryString["iID_MaLoaiHinh"];
    if (String.IsNullOrEmpty(iID_MaLoaiHinh)) iID_MaLoaiHinh = Convert.ToString(ViewData["iID_MaLoaiHinh"]);

    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
    DataRow newRow = dtDonVi.NewRow();
    dtDonVi.Rows.InsertAt(newRow, 0);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();
    
    DataTable dt = SanPham_DanhMucGiaModels.Get_DanhSachChiTietGia(MaND, iID_MaSanPham, iID_LoaiDonVi, iID_MaDonVi, iID_MaLoaiHinh, CurrentPage, Globals.PageSize);

    double nums = SanPham_DanhMucGiaModels.Get_DanhSachChiTietGia_Count(MaND, iID_MaSanPham, iID_LoaiDonVi, iID_MaDonVi, iID_MaLoaiHinh);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {
        MaND = MaND,
        iID_MaSanPham = iID_MaSanPham,
        iID_LoaiDonVi = iID_LoaiDonVi,
        iID_MaDonVi = iID_MaDonVi,
        iID_MaLoaiHinh = iID_MaLoaiHinh,
        page = x
    }));
    String strThemMoi = Url.Action("Edit", "SP_ChiTietGia", new { iID_MaSanPham = iID_MaSanPham });
    String strExcel = Url.Action("Index", "ImportExcelGiaSP", new { iID_MaSanPham = iID_MaSanPham });
    using (Html.BeginForm("Index", "SP_ChiTietGia", new {ParentID = ParentID, iID_MaSanPham = iID_MaSanPham }))
    {
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
                	<span>Danh sách giải trình chi tiết giá sản phẩm <%=sTen%></span>
                </td>
                <td align="right">
                    <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                    <input id="Button2" type="button" class="button_title" value="Nhập từ Excel" onclick="javascript:location.href='<%=strExcel %>'" />
                </td>
            </tr>
        </table>
    </div>
    <div id="Div1" style="background-color:#F0F9FE;">
          <div id="rptMain" style="background-color:#F0F9FE;padding-top:5px;">
  <table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 68px">
  <tr>
    <td align="right" class="style1" width="25%"><b>Đơn vị:</b></td>
    <td width="25%">&nbsp;<%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\" style=\"width: 80%;height:24px;\"")%></td>
    <td align="right" class="style1" width="15%"><b>Loại sửa chữa: </b></td>
    <td width="25%">&nbsp;
        <select id="<%=ParentID %>_iID_MaLoaiHinh" name="<%=ParentID %>_iID_MaLoaiHinh" class="input1_2" style="width:80%">
            <option value = "" ></option>
            <option value = "1" <%if(iID_MaLoaiHinh == "1") {%> selected="selected" <%} %>>Sửa chữa lớn</option>
            <option value = "2" <%if(iID_MaLoaiHinh == "2") {%> selected="selected" <%} %>>Sửa chữa vừa</option>
            <option value = "3" <%if(iID_MaLoaiHinh == "3") {%> selected="selected" <%} %>>Sửa chữa nhỏ</option>
        </select>
    </td>
    <td align="right" class="style1" width="10%"></td>
  </tr>
  <tr>
    <td colspan="5" align="right">
        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Tìm kiếm")%>" />
    </td>
  </tr>
</table>
            </div>
        </div>
    <table class="mGrid" style="width:100%">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 7%;" align="center">Loại đơn vị</th>
            <th style="width: 15%;" align="center">Đơn vị sản xuất</th>
            <th style="width: 15%;" align="center">Đơn vị đặt hàng</th>
            <th style="width: 7%;" align="center">Loại sửa chữa</th>
            <th style="width: 7%;" align="center">Đơn vị tính</th>
            <th style="width: 8%;" align="center">Số lượng</th>
            <th style="width: 5%;" align="center">Lợi nhuận dự kiến (%)</th>
            <th style="width: 5%;" align="center">Thuế giá trị gia tăng (%)</th>
            <th style="width: 15%;" align="center">Quy cách</th>
            <th style="width: 5%;" align="center">Ngày tạo</th>
            <th style="width: 5%;" align="center">Chi tiết</th>
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
for (i = 0; i < dt.Rows.Count; i++)
{
    DataRow R = dt.Rows[i];
    String classtr = "";
    int STT = i + 1;
    ////Lấy tên đơn vị
    //String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));            

    //Lấy thông tin loại hình sản phẩm
    String strLoaiHinh = "", strTenDonVi_SX = "", strTenDonVi_DH = "";
    switch (Convert.ToString(R["iID_MaLoaiHinh"]))
    {
        case "1":
            strLoaiHinh = "Sửa chữa lớn";
            break;
        case "2":
            strLoaiHinh = "Sửa chữa vừa";
            break;
        case "3":
            strLoaiHinh = "Sửa chữa nhỏ";
            break;
    }
    //Lấy thông tin đơn vị tính
    String strDonViTinh = SanPham_VatTuModels.Get_TenDonViTinh(Convert.ToString(R["iDM_MaDonViTinh"]));
    String dNgayTao = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayTao"]));

    String strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "SP_ChiTietGia", new { iID_MaChiTietGia = R["iID_MaChiTietGia"], iID_MaSanPham = iID_MaSanPham }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
    String strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "SP_ChiTietGia", new { iID_MaChiTietGia = R["iID_MaChiTietGia"], iID_MaSanPham = iID_MaSanPham }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
    String strChiTiet = MyHtmlHelper.ActionLink(Url.Action("ChiTiet", "SP_ChiTietGia", new { iID_MaChiTietGia = R["iID_MaChiTietGia"], iID_MaSanPham = iID_MaSanPham, iID_LoaiDonVi = R["iID_LoaiDonVi"] }).ToString(), "<img src='../Content/Themes/images/iconOFFICE.gif' alt='' />", "ChiTiet", "");
    String strLoaiDonVi = "";
    switch (Convert.ToString(R["iID_LoaiDonVi"]))
    {
        case "1":
            strLoaiDonVi = "Sản xuất";
            strTenDonVi_SX = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
            break;
        case "2":
            strLoaiDonVi = "Đặt hàng";
            strTenDonVi_DH = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
            strTenDonVi_SX = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi_LienQuan"]));
            break;
        case "3":
            strLoaiDonVi = "Cục tài chính";
            strTenDonVi_DH = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
            strTenDonVi_SX = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi_LienQuan"]));
            break;
    }
    //}       
            %>
            <tr>
                <td align="center"><%=STT%></td> 
                <td align="left"><%=strLoaiDonVi%></td>  
                <td align="left"><%=strTenDonVi_SX%></td> 
                <td align="left"><%=strTenDonVi_DH%></td>    
                <td align="left"><%=strLoaiHinh%></td>
                <td align="center"><%=strDonViTinh%></td>
                <td align="left"><%=R["rSoLuong"]%></td>
                <td align="center"><%=HamChung.ConvertToDouble(R["rLoiNhuan"])%></td>
                <td align="center"><%=HamChung.ConvertToDouble(R["rThueGTGT"])%></td>
                <td align="left"><%=R["sQuyCach"]%></td>
                <td align="center"><%=dNgayTao%></td>
                <td align="center"><%=strChiTiet%></td>
                <td align="center"><%=strEdit%></td>
                <td align="center"><%=strDelete%></td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="14" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%  
dt.Dispose();
    }
%>
</asp:Content>



