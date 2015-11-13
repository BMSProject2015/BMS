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
                    <td align="center" style="padding: 3px 2px;">79_3</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCapPhat_79_3"), "Báo cáo chi tiết cấp ngân sách")%></td>                        
                    <td></td>
                </tr>
              <tr>
                    <td align="center" style="padding: 3px 2px;">2</td>
                    <td align="center" style="padding: 3px 2px;">80_4</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCapPhat_80_4"), "Tổng hợp chi tiêu - Cấp ngân sách- Quyết Toán")%></td>                        
                    <td></td>
                </tr>
                  <tr>
                    <td align="center" style="padding: 3px 2px;">3</td>
                    <td align="center" style="padding: 3px 2px;">81</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCapPhat_TongHopChiTieuCapNganSach_81"),"Tổng hợp chi tiêu - Cấp ngân sách")%></td>                        
                    <td></td>
                </tr>  
                 <tr>
                    <td align="center" style="padding: 3px 2px;">4</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCapPhat_PhanHoTheoLNS"), "Báo cáo phân hộ theo loại ngân sách")%></td>                        
                    <td></td>
                </tr>   
                  <tr>
                    <td align="center" style="padding: 3px 2px;">5</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCP_ChiTieu_QT"), "Tổng hợp chỉ tiêu - quyết toán")%></td>                        
                    <td></td>
                </tr>  
                 <tr>
                    <td align="center" style="padding: 3px 2px;">6</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCapPhat_TheoDonVi"), "Tổng hợp chỉ tiêu - Theo đơn vị")%></td>                        
                    <td></td>
                </tr>      
                 <tr>
                    <td align="center" style="padding: 3px 2px;">7</td>
                    <td align="center" style="padding: 3px 2px;"></td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCapPhatThongTri_ChonChungTu"), "Thông tri chọn chứng từ")%></td>                        
                    <td></td>
                </tr>  
                <tr>
                    <td align="center" style="padding: 3px 2px;">8</td>
                    <td align="center" style="padding: 3px 2px;">Mẫu số C08-D</td>
                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptCapPhat_ThongTri"), "Thông tri cấp phát")%></td>                        
                    <td align="center" style="padding: 3px 2px;">VungNV: 2015/09/30</td>
                </tr>                      
            </table>
        </div>
    </div>
</div>
</asp:Content>

