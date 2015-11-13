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
    String ParentID = "KTCS_HachToanTaiSan";
    String iTuThang = Request.QueryString["iTuThang"];
    String iDenThang = Request.QueryString["iDenThang"];
    String MaND = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
    DataRow R = dtCauHinh.Rows[0];
    String iNamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);   

    DataTable dtNam = DanhMucModels.DT_Nam(true);
    SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
    dtNam.Dispose();

    DataTable dtMonth = DanhMucModels.DT_Thang(true);
    SelectOptionList slMonth = new SelectOptionList(dtMonth, "MaThang", "TenThang");
    dtMonth.Dispose();

    using (Html.BeginForm("DetailSubmit", "KTCS_TinhHachToan", new { ParentID = ParentID, NamLamViec = iNamLamViec }))
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
                	<span>Tính hạch toán tài sản trong năm <%= R["iNamLamViec"]%></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="right" style="width: 15%; padding: 10px;">
                        Từ tháng
                    </td>
                    <td align="left" style="width: 34%; padding: 10px;"">
                        <%=MyHtmlHelper.DropDownList(ParentID, slMonth, iTuThang, "iTuThangHachToan", "", "class=\"input1_2\" style=\"width:90%;\"")%>
                    </td>
                    <td align="right" style="width: 15%; padding: 10px;">
                        Đến tháng
                    </td>
                    <td align="left" style="width: 34%; padding: 10px;"">
                        <%=MyHtmlHelper.DropDownList(ParentID, slMonth, iDenThang, "iDenThangHachToan", "", "class=\"input1_2\" style=\"width:90%;\"")%>
                    </td>
                </tr>
                <tr><td align="left" colspan="4" class="td_form2_td1" style="height: 10px;"></td></tr>
                <tr>
                    <td align="right" colspan="4" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
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
                    <span>Danh sách tài sản hạch toán đến tháng <%=iDenThang %> năm <%=iNamLamViec %></span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>       
            <th style="width: 3%;" align="center">STT</th>  
            <th style="width: 17%;" align="center">Mã tài sản</th>
            <th style="width: 10%;" align="center">Năm hao mòn</th>
            <th style="width: 10%;" align="center">Nguyên giá</th>
            <th style="width: 10%;" align="center">Số tiền tăng</th>
            <th style="width: 10%;" align="center">Số tiền giảm</th>
            <th style="width: 10%;" align="center">Số tiền hao mòn trong năm</th>
            <th style="width: 10%;" align="center">Số tiền hao mòn lũy kế</th>
            <th style="width: 10%;" align="center">Giá trị còn lại</th>
            <th style="width: 10%;" align="center">Số năm hao mòn còn</th>
        </tr>
        <%        
        DataTable dt = KTCS_HachToanModels.Get_Table_TinhHachToanTaiSan(Convert.ToInt32(iNamLamViec));
        if(dt.Rows.Count > 0){
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R1 = dt.Rows[i];

            String strURL = MyHtmlHelper.ActionLink(Url.Action("Detail", "KTCS_TinhHachToan", new { iNam = iNamLamViec, iID_MaTaiSan = R1["iID_MaTaiSan"] }).ToString(), R1["iID_MaTaiSan"].ToString(), "Detail", null, "title=\"Chi tiết hạch toán\"");
            
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
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["rGiaTriConLai"], false)%></td>
                <td align="right" <%=bold %>><%=CommonFunction.DinhDangSo(R1["iSoNamKhauHao"], false)%></td>
            </tr>          
        <%}} dt.Dispose(); %>
    </table>
</div>
</asp:Content>

