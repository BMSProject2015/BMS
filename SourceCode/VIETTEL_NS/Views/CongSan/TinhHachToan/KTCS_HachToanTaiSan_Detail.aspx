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

    DataTable dt = KTCS_HachToanModels.Get_Table_ChiTietHachToanTaiSan(Convert.ToInt32(iNamLamViec), iID_MaTaiSan);
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCS_TinhHachToan"), "Hạch toán tài sản")%>
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
            <th style="width: 15%;" align="center">Mã tài sản</th>
            <th style="width: 8%;" align="center">Năm hao mòn</th>
            <th style="width: 7%;" align="center">Tháng</th>
            <th style="width: 7%;" align="center">Ngày</th>
            <th style="width: 10%;" align="center">Tài khoản có</th>
            <th style="width: 10%;" align="center">Tên tài khoản có</th>
            <th style="width: 10%;" align="center">Tài khoản nợ</th>
            <th style="width: 10%;" align="center">Tên tài khoản nợ</th>
            <th style="width: 20%;" align="center">Số tiền</th>
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
                <td align="left" <%=bold %>><%=R1["iID_MaTaiSan"]%></td>
                <td align="center" <%=bold %>><%=R1["iNamLamViec"].ToString()%></td>
                <td align="center" <%=bold %>><%=R1["iThang"].ToString()%></td>
                <td align="center" <%=bold %>><%=R1["iNgay"].ToString()%></td>
                <td align="center" <%=bold %>><%=R1["iID_MaTaiKhoan_Co"].ToString()%></td>
                <td align="center" <%=bold %>><%=R1["sTenTaiKhoan_Co"].ToString()%></td>
                <td align="center" <%=bold %>><%=R1["iID_MaTaiKhoan_No"].ToString()%></td>
                <td align="center" <%=bold %>><%=R1["sTenTaiKhoan_No"].ToString()%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rSoTien"], false)%></td>
            </tr>          
        <%}} dt.Dispose(); %>
    </table>
</div>
</asp:Content>



