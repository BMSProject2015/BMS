<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.SanPham" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "VatTu_TimKiem_List";
    String MaND = User.Identity.Name;

    String LoaiNganSach = Convert.ToString(ViewData["LoaiNganSach"]);
    
    String sTen = Convert.ToString(ViewData["sTen"]);
    String sTenSanPham = Convert.ToString(ViewData["sTenSanPham"]);
    
    
    String Nam = Nam = Convert.ToString(ViewData["Nam"]);
    String NamSoSanh =  Convert.ToString(ViewData["NamSoSanh"]);

    String KieuSoSanh = Convert.ToString(ViewData["KieuSoSanh"]);
    if (String.IsNullOrEmpty(KieuSoSanh)) KieuSoSanh = "1";
    DataTable dtKieuSoSanh = new DataTable();
    dtKieuSoSanh.Columns.Add("Kieu", typeof(String));
    dtKieuSoSanh.Columns.Add("TenKieu", typeof(String));
    dtKieuSoSanh.Rows.Add("1", "Giá lớn nhất");
    dtKieuSoSanh.Rows.Add("2", "Giá trung bình");
    SelectOptionList slKieuSoSanh = new SelectOptionList(dtKieuSoSanh, "Kieu", "TenKieu");
        
    int iNam = DateTime.Today.Year;
    int iNamSoSanh = iNam;
    if (!String.IsNullOrEmpty(NamSoSanh)) iNamSoSanh = Convert.ToInt16(NamSoSanh);
        
    int NamMin = iNam - 20;
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

    NamMin = iNam - 4;
    dtNam = new DataTable();
    dtNam.Columns.Add("MaNam", typeof(String));
    dtNam.Columns.Add("TenNam", typeof(String));
    for (int i = NamMax; i >= NamMin; i--)
    {
        R = dtNam.NewRow();
        dtNam.Rows.Add(R);
        R[0] = Convert.ToString(i);
        R[1] = Convert.ToString(i);
    }
    SelectOptionList slNamSoSanh = new SelectOptionList(dtNam,"MaNam", "TenNam");
    
    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = VatTu_SoSanhGiaController.Get_DanhSachVatTu(sTen, sTenSanPham, iNam, iNamSoSanh,int.Parse(KieuSoSanh), LoaiNganSach, CurrentPage, Globals.PageSize);

    double nums = VatTu_SoSanhGiaController.Get_DanhSachVatTu_Count(sTen,sTenSanPham, iNam);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {
        sTen = sTen,
        sTenSanPham = sTenSanPham,
        Nam = iNam,
        NamSoSanh = iNamSoSanh,
        KieuSoSanh = KieuSoSanh,
        LoaiNganSach = LoaiNganSach,
        page = x
    }));
    using (Html.BeginForm("Index", "VatTu_SoSanhGia", new { ParentID = ParentID }))
    {
%>
<script type="text/javascript">
    function ThayDoiNam(Nam) {
        var select = document.getElementById("<%=ParentID %>_NamSoSanh");
        select.options.length = 0;
//        select.options[select.options.length] = new Option(Nam, Nam);
        for (i = Nam; i > (Nam - 5); i = i - 1) {
            select.options[select.options.length] = new Option(i, i);
        }
}
</script>
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
                	<span>So sánh giá vật tư</span>
                </td>
                <td align="right">
                </td>
            </tr>
        </table>
    </div>
    <div id="Div1" style="background-color: #F0F9FE;">
        <div id="rptMain" style="background-color: #F0F9FE; padding-top: 5px;">
            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="height: 68px">
                <tr>
                    <td align="right" class="style1" width="15%">
                        <b>Tên vật tư: </b>
                    </td>
                    <td width="35%">
                        &nbsp;<%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\" style=\"width: 60%;height:24px;\"", 2)%>
                    </td>
                    <td align="right" class="style1" width="15%">
                        <b>Tên sản phẩm: </b>
                    </td>
                    <td width="35%">
                        &nbsp;<%=MyHtmlHelper.TextBox(ParentID, sTenSanPham, "sTenSanPham", "", "class=\"input1_2\" style=\"width: 60%;height:24px;\"", 2)%>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="style1" width="15%">
                        <b>Năm: </b>
                    </td>
                    <td width="35%">
                        &nbsp;<%=MyHtmlHelper.DropDownList(ParentID, slNam, iNam.ToString(), "Nam", "", "class=\"input1_2\" style=\"width: 61%;height:24px;\" onchange = \"ThayDoiNam(this.value);\"")%>
                    </td>
                    <td align="right" class="style1" width="15%">
                        <b>Loại giá: </b>
                    </td>
                    <td width="35%">
                        &nbsp;<%=MyHtmlHelper.DropDownList(ParentID, slKieuSoSanh, KieuSoSanh, "KieuSoSanh", "", "class=\"input1_2\" style=\"width: 61%;height:24px;\"")%>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="style1" width="15%">
                        <b>Năm so sánh: </b>
                    </td>
                    <td width="35%">
                        &nbsp;<%=MyHtmlHelper.DropDownList(ParentID, slNamSoSanh, NamSoSanh, "NamSoSanh", "", "class=\"input1_2\" style=\"width: 61%;height:24px;\"")%>
                    </td>
                    <td align="right" class="style1" width="15%">
                        <b>Giá ngân sách: </b>
                    </td>
                    <td width="35%">
                        &nbsp;<input type="checkbox" value="1" id = "<%=ParentID %>_LoaiNganSach" name= "<%=ParentID %>_LoaiNganSach" <%if(LoaiNganSach == "1") {%> checked="checked" <%} %> />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <input type="submit" class="button" id="Submit2" value="<%=NgonNgu.LayXau("Tìm kiếm")%>" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 5%;" align="center" >STT</th>
            <th style="width: 15%;" align="center" >Tên vật tư</th>
            <th style="width: 5%;" align="center" >Đơn vị tính</th>
            <th style="width: 15%;" align="center" >Tên sản phẩm</th>
            <%for(int i = 1; i<=5; i++) {%>
                <th style="width: 10%;" align="center">Năm <%=iNam + i - 5%></th>
            <%} %>
            <th style="width: 10%;" align="center" >Tỷ lệ (%)</th>
        </tr>
        <%
            int STT = (CurrentPage - 1) * Globals.PageSize;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow Row = dt.Rows[i];
            STT++;   
            %>
            <tr>
                <td align="center"><%=STT%></td>
                <td align="left"><%=Row["sTen"]%></td>
                <td align="center"><%=Row["sTen_DonViTinh"]%></td>
                <td align="left"><%=Row["sTenSanPham"]%></td>
                <%for(int j = 1; j<=5; j++) {%>
                <td align="center"><%=CommonFunction.DinhDangSo(Row["Nam" + j.ToString()],2)%></td>
                <%} %>
                <td align="center"><%=CommonFunction.DinhDangSo(Row["Sosanh"],2)%></td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="10" align="right">
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



