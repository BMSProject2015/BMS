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
    String iNamLamViec = Request.QueryString["iNam"];
    String MaND = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
    DataRow R = dtCauHinh.Rows[0];
    if (String.IsNullOrEmpty(iNamLamViec)) iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);    

    DataTable dtNam = DanhMucModels.DT_Nam(true);
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    dtNam.Dispose();

    using (Html.BeginForm("DetailSubmit", "KTCS_TinhKhauHao", new { ParentID = ParentID, iNam = iNamLamViec }))
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
                	<span>Tính hao mòn tài sản trong năm <%= R["iNamLamViec"]%></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="right" style="width: 20%; padding: 10px;">
                        Năm hao mòn
                    </td>
                    <td align="left" style="width: 78%; padding: 10px;"">
                        <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamLamViec, "iNamKhauHao", "", "class=\"input1_2\" style=\"width:90%;\"")%>
                    </td>
                </tr>
                <tr><td align="left" colspan="2" class="td_form2_td1" style="height: 10px;"></td></tr>
                <tr>
                    <td align="right" colspan="2" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <input type="submit" class="button" value="Thực hiện"/>
                                </td>
                                <td style="width: 10px;"></td>
                                <td>
                                    <input type="button" class="button" value="Hủy" onclick="history.go(-1);"/>
                                </td>
                                <td style="width: 10px;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<%  } %>
<br />
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                    <span>Danh sách tài sản tính hao mòn trong năm <%=iNamLamViec %></span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>       
            <th style="width: 3%;" align="center">STT</th>  
            <th style="width: 12%;" align="center">Mã tài sản</th>
            <th style="width: 5%;" align="center">Năm đưa vào sử dụng</th>
            <th style="width: 10%;" align="center">Nguyên giá</th>
            <th style="width: 10%;" align="center">Số tiền tăng nguyên giá</th>
            <th style="width: 10%;" align="center">Số tiền giảm nguyên giá</th>
            <th style="width: 10%;" align="center">Số tiền hao mòn trong năm</th>
            <th style="width: 10%;" align="center">Số tiền hao mòn lũy kế</th>
            <th style="width: 10%;" align="center">Giá trị còn lại trước khi tính HM</th>
            <th style="width: 10%;" align="center">Giá trị còn lại</th>
            <th style="width: 10%;" align="center">Thời gian sử dụng còn lại (năm)</th>
        </tr>
        <%        
        DataTable dt = KTCS_KhauHaoModels.Get_Table_TinhHaoMonTaiSan(Convert.ToInt32(iNamLamViec));
        if(dt.Rows.Count > 0){
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R1 = dt.Rows[i];

            String strURL = MyHtmlHelper.ActionLink(Url.Action("Detail", "KTCS_TinhKhauHao", new { iNam = iNamLamViec, iID_MaTaiSan = R1["iID_MaTaiSan"] }).ToString(), R1["iID_MaTaiSan"].ToString(), "Detail", null, "title=\"Xem lịch sử hao mòn\"");
            
            String bold = "style = \"padding: 3px 3px;\"";
            
            String strClass = "";
            if (i % 2 == 0) strClass = "alt";       
            %>
            <tr class="<%=strClass %>">      
                <td align="center" <%=bold %>><%=i+1 %></td>    
                <td align="left" <%=bold %>>
                   <b><%=strURL%></b>
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

