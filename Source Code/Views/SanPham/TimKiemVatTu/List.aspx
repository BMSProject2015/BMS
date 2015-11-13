<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "VatTu_TimKiem_List";
    String MaND = User.Identity.Name;
    
    String sTen = Request.QueryString["sTen"];
    if (String.IsNullOrEmpty(sTen)) sTen = Convert.ToString(ViewData["sTen"]);
    String Nam = Request.QueryString["Nam"];
    if (String.IsNullOrEmpty(Nam)) Nam = Convert.ToString(ViewData["Nam"]);

    int iNam = DateTime.Today.Year;
    int NamMin = iNam - 10;
    int NamMax = iNam;
    if (!String.IsNullOrEmpty(Nam)) iNam = Convert.ToInt16(Nam);
    DataTable dtNam = new DataTable();
    dtNam.Columns.Add("MaNam", typeof(String));
    dtNam.Columns.Add("TenNam", typeof(String));
    DataRow R;
    for (int i = NamMax; i >= NamMin; i--)
    {
        R = dtNam.NewRow();
        dtNam.Rows.Add(R);
        R[0] = Convert.ToString(i);
        R[1] = Convert.ToString(i);
    }
    dtNam.Rows.InsertAt(dtNam.NewRow(), 0);
    dtNam.Rows[0]["TenNam"] = "-- Bạn chọn năm --";
    dtNam.Rows[0]["MaNam"] = "0";
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");


    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = VatTu_TimKiemController.Get_DanhSachVatTu(sTen, iNam, CurrentPage, Globals.PageSize);

    double nums = VatTu_TimKiemController.Get_DanhSachVatTu_Count(sTen, iNam);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {
        sTen = sTen,
        Nam = iNam,
        page = x
    }));
    using (Html.BeginForm("Index", "VatTu_TimKiem", new { ParentID = ParentID }))
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "MaVatTu"), "Vật tư")%>
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
                	<span>Tìm kiếm giá vật tư</span>
                </td>
                <td align="right">
                </td>
            </tr>
        </table>
    </div>
    <div id="Div1" style="background-color:#F0F9FE;">
          <div id="rptMain" style="background-color:#F0F9FE;padding-top:5px;">
  <table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 68px">
  <tr>
    <td align="right" class="style1" width="15%"><b>Tên vật tư: </b></td>
    <td width="25%">&nbsp;<%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\" style=\"width: 80%;height:24px;\"", 2)%></td>
    <td align="right" class="style1" width="10%"></td>
  </tr>
  <tr>
    <td align="right" class="style1" width="15%"><b>Năm: </b></td>
    <td width="25%">&nbsp;<%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam.ToString(), "Nam", "", "class=\"input1_2\" style=\"width: 30%;height:24px;\"")%></td>
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
    <table class="mGrid">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: 25%;" align="center">Tên vật tư</th>
            <th style="width: 15%;" align="center">Đơn vị tính</th>
            <th style="width: 25%;" align="center">Tên sản phẩm</th>
            <th style="width: 15%;" align="center">Giá (Đồng)</th>
            <th style="width: 15%;" align="center">Giá Ngân Sách (Đồng)</th>
        </tr>
        <%
            int STT = (CurrentPage - 1) * Globals.PageSize;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow Row = dt.Rows[i];
            STT++;
            DataTable dtGia = VatTu_TimKiemController.getGiaVatTu(Convert.ToString(Row["iID_MaVatTu"]), Convert.ToString(Row["iID_MaSanPham"]), iNam);
            String rGia = "", rGia_NS = "";
            if (dtGia.Rows.Count>0)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(dtGia.Rows[0]["rGia"]))) if (Convert.ToDecimal(dtGia.Rows[0]["rGia"]) > 0) rGia = Convert.ToString(dtGia.Rows[0]["rGia"]);
                if (!String.IsNullOrEmpty(Convert.ToString(dtGia.Rows[0]["rGia_NS"]))) if (Convert.ToDecimal(dtGia.Rows[0]["rGia_NS"]) > 0) rGia_NS = Convert.ToString(dtGia.Rows[0]["rGia_NS"]);
            }
            //Lấy tên đơn vị
            String strTenDonViTinh = SanPham_VatTuModels.Get_TenDonViTinh(Convert.ToString(Row["iDM_MaDonViTinh"]));
            String strTenSanPham = SanPham_VatTuModels.Get_TenSanPham(Convert.ToString(Row["iID_MaSanPham"]));   
            //}       
            %>
            <tr>
                <td align="center"><%=STT%></td>
                <td align="left"><%=Row["sTen"]%></td>
                <td align="center"><%=strTenDonViTinh%></td>
                <td align="left"><%=strTenSanPham%></td>
                <td align="center"><%=CommonFunction.DinhDangSo(rGia,2)%></td>
                <td align="center"><%=CommonFunction.DinhDangSo(rGia_NS,2)%></td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="6" align="right">
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



