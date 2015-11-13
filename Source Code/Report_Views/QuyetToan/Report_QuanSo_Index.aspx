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
                
                 <tr>
                    <td align="center" style="padding: 3px 2px;">1</td>
                    <td align="center" style="padding: 3px 2px;">Phụ lục 4</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQTQS_THQS_TungDonVi"), "Tổng hợp tình hình quân số")%></td>                        
                    <td></td>
                </tr> 
                     <tr>
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">Phụ lục 5</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQTQS_THQS_TongHop"), "Tổng hợp tình hình quân số theo đơn vị")%></td>                        
                    <td></td>
                </tr>            
                  
                    <tr>
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQTQS_TongHop"), "Tổng hợp quân số quyết toán")%></td>                        
                    <td></td>
                </tr>  
                
                  <tr>
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQTQS_Thang"), "Tổng hợp quân số quyết toán từ tháng đến tháng")%></td>                        
                    <td></td>
                </tr>  
                
                  <tr>
                    <td align="center" style="padding: 3px 2px;">5</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptQTQS_ThuongXuyen"), "Tổng hợp quyết toán thường xuyên")%></td>                        
                    <td></td>
                </tr>             
            </table>
        </div>
    </div>
</div>
</asp:Content>