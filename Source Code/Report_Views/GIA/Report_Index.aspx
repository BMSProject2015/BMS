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
                    <td align="center" style="padding: 3px1b2px;">Biểu 1</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("List", "rptGia_1"), "Bảng giải trình chi tiết tính giá sản phẩm")%></td>                        
                    <td>Đơn vị sản xuất</td>
                </tr>    
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">Biểu 2</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("List", "rptGia_2"), "Bảng Tổng hợp tính giá sản phẩm")%></td>                        
                    <td>Đơn vị sản xuất</td>
                </tr>     
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;">Biểu 7a</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptGia_7a"), "Bảng Tổng hợp kết quả thẩm định giá sản phẩm")%></td>                        
                    <td>Đơn vị sản xuất</td>
                </tr>       
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;">Biểu 3</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("List", "rptGia_3"), "Bảng giải trình chi tiết tính giá sản phẩm")%></td>                        
                    <td>Đơn vị đặt hàng</td>
                </tr>       
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">5</td>
                    <td align="center" style="padding: 3px 2px;">Biểu 4</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("List", "rptGia_4"), "Bảng Tổng hợp tính giá sản phẩm")%></td>                        
                    <td>Đơn vị đặt hàng</td>
                </tr>   
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">6</td>
                    <td align="center" style="padding: 3px 2px;">Biểu 7b</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptGia_7b"), "Bảng Tổng hợp kết quả thẩm định giá sản phẩm")%></td>                        
                    <td>Đơn vị đặt hàng</td>
                </tr>   
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">7</td>
                    <td align="center" style="padding: 3px 2px;">Biểu 5</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("List", "rptGia_5"), "Bảng giải trình chi tiết tính giá sản phẩm")%></td>                        
                    <td>Cục tài chính</td>
                </tr>
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">8</td>
                    <td align="center" style="padding: 3px 2px;">Biểu 6</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("List", "rptGia_6"), "Bảng Tổng hợp tính giá sản phẩm")%></td>                        
                    <td>Cục tài chính</td>
                </tr>  
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">9</td>
                    <td align="center" style="padding: 3px 2px;">Biểu 7c</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptGia_7c"), "Bảng Tổng hợp kết quả thẩm định giá sản phẩm")%></td>                        
                    <td>Cục tài chính</td>
                </tr>    
            </table>
        </div>
    </div>
</div>
</asp:Content>

