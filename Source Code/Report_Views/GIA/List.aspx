<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.GIA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String Controller = ViewData["Controller"].ToString();
    String sLoaiBaoCao = Request.QueryString["sLoai"];
    String ParentID = "DanhSachBaoCao";
    String MaND = User.Identity.Name;

    String iID_LoaiDonVi = Request.QueryString["iID_LoaiDonVi"];
    if (String.IsNullOrEmpty(iID_LoaiDonVi)) iID_LoaiDonVi = Convert.ToString(ViewData["iID_LoaiDonVi"]);
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
    if (String.IsNullOrEmpty(iID_MaDonVi)) iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    String iID_MaLoaiHinh = Request.QueryString["iID_MaLoaiHinh"];
    if (String.IsNullOrEmpty(iID_MaLoaiHinh)) iID_MaLoaiHinh = Convert.ToString(ViewData["iID_MaLoaiHinh"]);

    DataTable dtDonVi = GIA_ReportController.getDtDonVi(MaND,iID_LoaiDonVi);
    DataRow newRow = dtDonVi.NewRow();
    dtDonVi.Rows.InsertAt(newRow, 0);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();
    
    int CurrentPage = 1;
    String page = Request.QueryString["page"];
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }
    DataTable dt = SanPham_DanhMucGiaModels.Get_DanhSachChiTietGia(MaND, "", iID_LoaiDonVi, iID_MaDonVi, iID_MaLoaiHinh, CurrentPage, Globals.PageSize);
    double nums = SanPham_DanhMucGiaModels.Get_DanhSachChiTietGia_Count(MaND, iID_LoaiDonVi,  "", iID_MaDonVi, iID_MaLoaiHinh);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("List", new
    {
        MaND = MaND,
        iID_LoaiDonVi = iID_LoaiDonVi,
        iID_MaDonVi = iID_MaDonVi,
        iID_MaLoaiHinh = iID_MaLoaiHinh,
        sLoai = sLoaiBaoCao,
        page = x
    }));
    using (Html.BeginForm("List", Controller, new { ParentID = ParentID }))
    {
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách báo cáo</span>
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
    <div id="nhapform">
        <table class="mGrid" style="width:100%">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: 15%;" align="center">Đơn vị</th>
            <th style="width: 10%;" align="center">Loại sửa chữa</th>
             <th style="width: 20%;" align="center">Sản phẩm</th>
            <th style="width: 10%;" align="center">Đơn vị tính</th>
            <th style="width: 5%;" align="center">Số lượng</th>
            <th style="width: 20%;" align="center">Quy cách</th>
            <th style="width: 10%;" align="center">Ngày tạo</th>
            <th style="width: 5%;" align="center">Chi tiết</th>
        </tr>
        <%
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;
            ////Lấy tên đơn vị
            //String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));            
            
            //Lấy thông tin loại hình sản phẩm
            String strLoaiHinh = "";
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
            String iID_MaSanPham = Convert.ToString(R["iID_MaSanPham"]);
            String strTenSanPham = SanPham_VatTuModels.Get_TenSanPham(iID_MaSanPham);
            String strDonViTinh =SanPham_VatTuModels.Get_TenDonViTinh(Convert.ToString(R["iDM_MaDonViTinh"]));
            String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
            if (iID_LoaiDonVi == "3") strTenDonVi = "Cục tài chính";
            String dNgayTao = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayTao"]));

            String strChiTiet = MyHtmlHelper.ActionLink(Url.Action("Index", new { iID_MaChiTietGia = R["iID_MaChiTietGia"], iID_MaSanPham = iID_MaSanPham }).ToString(), "<img src='../Content/Themes/images/iconOFFICE.gif' alt='' />", "ChiTiet", "");
            String strSanPham = MyHtmlHelper.ActionLink(Url.Action("Index", new { iID_MaChiTietGia = R["iID_MaChiTietGia"], iID_MaSanPham = iID_MaSanPham }).ToString(), strTenSanPham);
            //}       
            %>
            <tr>
                <td align="center"><%=STT%></td>  
                <td align="left"><%=strTenDonVi%></td>    
                <td align="left"><%=strLoaiHinh%></td>
                <td align="left"><%=strSanPham%></td>
                <td align="center"><%=strDonViTinh%></td>
                <td align="left"><%=R["rSoLuong"]%></td>
                <td align="left"><%=R["sQuyCach"]%></td>
                <td align="center"><%=dNgayTao%></td>
                <td align="center"><%=strChiTiet%></td>
            </tr>
        <%}%>
        <tr class="pgr">
            <td colspan="10" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
    </div>
</div>
<%} %>
</asp:Content>

