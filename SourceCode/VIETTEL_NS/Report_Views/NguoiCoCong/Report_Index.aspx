<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String sLoaiBaoCao = Request.QueryString["iLoai"];    
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
           <%if (sLoaiBaoCao == "1")
              { %>
            <table class="mGrid">
                <tr>
                    <th style="width: 3%;">STT</th>
                    <th style="width: 15%;">Mã báo cáo - phụ lục</th>                        
                    <th style="width: 60%;">Tên báo cáo</th>
                    <th style="width: 22%;">Ghi chú</th>
                </tr>
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">1</td>
                    <td align="center" style="padding: 3px 2px;">5.1 NSL</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNCC_TCCK_LuyKe_54"), "Nhập số liệu")%></td>                        
                    <td></td>
                </tr>        
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">56-ChiTiet</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNCC_ThongTriTongHop_2"), "Thông Tri Chuẩn Quyết Toàn Người có công")%></td>                        
                    <td></td>
                </tr>                   
                             
                <tr>
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;">57-Tổng Hợp ngân sách</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNCC_TongHopNganSach_57"), "Báo Cáo Tổng Hợp Ngân Sách Người Có Công")%></td>                        
                    <td></td>
               
                   <tr>
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;">58-NCC_4</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNCC_TCKK_58_4"), "Báo Cáo quyết toán NCC Tổng hợp quyết toán")%></td>                        
                    <td></td>
                </tr>                                      
            </table>
             <%}
              else if (sLoaiBaoCao == "2") 
              {%>
              <%-- Quyết toán nghiệp vụ sLoaiBaoCao=1 --%>
              <table class="mGrid">
                <tr>
                    <th style="width: 3%;">STT</th>
                    <th style="width: 15%;">Mã báo cáo - phụ lục</th>                        
                    <th style="width: 60%;">Tên báo cáo</th>
                    <th style="width: 22%;">Ghi chú</th>
                </tr>
                 <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">1</td>
                    <td align="center" style="padding: 3px 2px;">6.1-TCKK</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNCC_BaoCaoQTTroCapKhoKhan_5"), "Nhập số liệu")%></td>                        
                    <td></td>
                </tr>        
                <tr class="alt">
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">6-TCKK</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptTCKK_ThongTri"), "Thông Tri")%></td>                        
                    <td></td>
                </tr>                   
              
               
                   <tr>
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;">7</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNCC_TongHopTCKK_62"), "Tổng hợp trợ cấp khó khăn")%></td>                        
                    <td></td>
                   </tr>
                   <tr>
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;">8</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptNCC_TCKK_63_8"), "Tổng quyết toán")%></td>                        
                    <td>
                    </td>
                </tr> 
            </table>
            <%}%>
        </div>
    </div>
</div>
</asp:Content>
