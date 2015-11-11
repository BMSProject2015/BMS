<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String sLoaiBaoCao = Request.QueryString["sLoai"];    
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
    <div id="nhapform">
        <div id="form2">
            <table class="mGrid">
                <tr>
                    <th style="width: 3%;">STT</th>
                    <th style="width: 15%;">Mã báo cáo - phụ lục</th>                        
                    <th style="width: 60%;">Tên báo cáo</th>
                    <th style="width: 22%;">Ghi chú</th>
                </tr>
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">1</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số B 01 - DN</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTCDN_BangCanDoiKeToan_B01DN"), "Báo cáo cân đối kế toán")%></td>                        
                    <td>Th</td>
                </tr>         
                <tr>
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số B 02 - DN</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTCND_BC_KQHDKinhDoanh"), "Báo cáo kết quả hoạt động kinh doanh")%></td>                        
                    <td>Th</td>
                </tr>
                <tr>
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số B 03 - DN</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTCDN_BaoCaoTaiChinhQuy"), "Báo cáo tài chính năm của doanh nghiệp cổ phần")%></td>                        
                    <td></td>
                </tr> 
                <tr>
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số 01 - BC/DNK</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTCDN_HoSoDoanhNghiep"), "Hồ sơ doanh nghiệp")%></td>                        
                    <td>Th</td>
                </tr>   
                <tr>
                    <td align="center" style="padding: 3px 2px;">5</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số 02 BC/DNK</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTCDN_BC_ChiTieuTaiChinh_QuyNam_02"), "Báo cáo một số chỉ tiêu tài chính quý năm")%></td>                        
                    <td>Th</td>
                </tr>   
                <tr>
                    <td align="center" style="padding: 3px 2px;">6</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số 03 BC/DNK</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTCDN_TinhHinhThuNop_QuyNam_03"), "Báo cáo tình hình thu nộp quý năm")%></td>                        
                    <td>Th</td>
                </tr>                       
            </table>
        </div>
    </div>
</div>
</asp:Content>