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
                 <!--Thuong-->
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">1</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptLuong_BangKeTrichThue_TNCN"), "Bảng kê trích thuế thu nhập cá nhân")%></td>                        
                    <td>Th</td>
                </tr>  
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptLuong_DanhSachCapPhat"), "Bảng lương - Phụ cấp")%></td>                        
                    <td></td>
                </tr> 
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptLuong_TongHopLuongPhuCap"), "Tổng hợp lương")%></td>                        
                    <td></td>
                </tr>  
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu 01/QS</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptLuong_TongHopQuanSo"), "Tổng hợp quân số")%></td>                        
                    <td>Th</td>
                </tr> 
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">5</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptLuong_ChiTietTienLuong"), "Giải thích chi tiết")%></td>                        
                    <td></td>
                </tr>   
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">6</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptLuong_GiaiThich_PhuCap"), "Giải thích chi tiết phụ cấp")%></td>                        
                    <td>Th</td>
                </tr> 
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">8</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptLuong_Giay_CCTC"), "Giấy cung cấp tài chính")%></td>                        
                    <td>Th</td>
                </tr>     
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">9</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptLuong_DanhSachChiTra"), "Gửi ngân hàng - Danh sách chi trả tiền lương")%></td>                        
                    <td>Th</td>
                </tr>  
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">9</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptLuong_TongHopThuThueDauVao"), "Tổng hợp thu thuế đầu vào")%></td>                        
                    <td>Th</td>
                </tr>                  
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">10</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptLuong_BangKeChiTiet"), "Quyết toán thuế - Bảng kê chi tiết")%></td>                        
                    <td>Th</td>
                </tr>       
                         
            </table>
        </div>
    </div>
</div>
</asp:Content>

