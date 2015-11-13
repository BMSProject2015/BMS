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
    String ParentID = "KTCS_KhauHaoTaiSan";
    String iID_MaTaiSan = Request.QueryString["iID_MaTaiSan"];
    String iNamLamViec = Request.QueryString["iNam"];
    String MaND = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
    DataRow R = dtCauHinh.Rows[0];
    if (String.IsNullOrEmpty(iNamLamViec)) iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);    

    DataTable dtNam = DanhMucModels.DT_Nam(true);
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    dtNam.Dispose();

    DataTable dt = KTCS_KhauHaoModels.Get_Table_TinhHaoMonTaiSan_LichSu(Convert.ToInt32(iNamLamViec), iID_MaTaiSan);
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCS_TinhKhauHao"), "hao mòn tài sản")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                    <span>Nhật ký hao mòn tài sản</span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>       
            <th style="width: 3%;" align="center">STT</th>  
            <th style="width: 12%;" align="center">Mã tài sản</th>
            <th style="width: 5%;" align="center">Năm hao mòn</th>
            <th style="width: 10%;" align="center">Nguyên giá</th>
            <th style="width: 10%;" align="center">Số tiền tăng</th>
            <th style="width: 10%;" align="center">Số tiền giảm</th>
            <th style="width: 10%;" align="center">Số tiền hao mòn trong năm</th>
            <th style="width: 10%;" align="center">Số tiền hao mòn lũy kế</th>
            <th style="width: 10%;" align="center">Giá trị còn lại trước KH</th>
            <th style="width: 10%;" align="center">Giá trị còn lại</th>
            <th style="width: 10%;" align="center">Số năm hao mòn còn</th>
        </tr>
        <% 
        if(dt.Rows.Count > 0){
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R1 = dt.Rows[i];
            String bold = "style = \"padding: 3px 3px;\"";
            
            String strClass = "";
            if (i % 2 == 0) strClass = "alt";       
            %>
            <tr class="<%=strClass %>">      
                <td align="center" <%=bold %>><%=i+1 %></td>    
                <td align="left" <%=bold %>>
                   <%=R1["iID_MaTaiSan"]%>
                </td>
                <td align="center" <%=bold %>>
                   <%=R1["iNamLamViec"].ToString()%>
                </td>
                <td align="right"  <%=bold %>><%=CommonFunction.DinhDangSo(R1["rNguyenGia"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rSoTienTang"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rSoTienGiam"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rSoTienKhauHao"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rSoTienKhauHao_LuyKe"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rGiaTriConLaiTruocKhauHao"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rGiaTriConLai"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["iSoNamKhauHao"], false)%></td>
            </tr>          
        <%}} dt.Dispose(); %>
    </table>
</div>
</asp:Content>


