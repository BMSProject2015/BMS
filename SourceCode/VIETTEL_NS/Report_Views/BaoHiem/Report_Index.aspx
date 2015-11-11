<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String sLoaiBaoCao = Request.QueryString["bChi"];    
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
                 <%
                    switch (sLoaiBaoCao) { 
                        case "0":
                            %>
                                <tr class="alt">
                                    <td align="center" style="padding: 3px 2px;">1</td>
                                    <td align="center" style="padding: 3px 2px;">Mục - 3</td>
                                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptBH_ThongTri64"), "Thông tri bảo hiểm")%></td>                        
                                    <td></td>
                                </tr>                   
                                <tr>
                                    <td align="center" style="padding: 3px 2px;">2</td>
                                    <td align="center" style="padding: 3px 2px;">Mục - 4</td>
                                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptBH_TongHop67"), "Tổng hợp bảo hiểm")%></td>                        
                                    <td></td>
                                </tr>
                                 <tr>
                                    <td align="center" style="padding: 3px 2px;">3</td>
                                    <td align="center" style="padding: 3px 2px;">3</td>
                                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptBH_TongQuyetToan"), "Tổng quyết toán")%></td>                        
                                    <td></td>
                                </tr> 
                             <%
                                     break;
                            case "1":
                            %> 
                             <tr>
                                    <td align="center" style="padding: 3px 2px;">1</td>
                                    <td align="center" style="padding: 3px 2px;">Mục - 70-1</td>
                                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptBH_Chi_70_1"), "Nhập số liệu")%></td>                        
                                    <td></td>
                                </tr>  
                                 <tr>
                                    <td align="center" style="padding: 3px 2px;">2</td>
                                    <td align="center" style="padding: 3px 2px;">Mục - 2</td>
                                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptBH_Chi_ThongTri_72_2"), "Thông Tri ")%></td>                        
                                    <td></td>
                                </tr>  
                                <tr>
                                    <td align="center" style="padding: 3px 2px;">3</td>
                                    <td align="center" style="padding: 3px 2px;">Mục - 73-3</td>
                                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptBH_Chi_73_3"), "Tổng hợp theo đơn vị")%></td>                        
                                    <td></td>
                                </tr> 
                                 <tr>
                                    <td align="center" style="padding: 3px 2px;">4</td>
                                    <td align="center" style="padding: 3px 2px;">Mục - 74-4</td>
                                    <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptBH_Chi_74_4"), "Tổng quyết toán")%></td>                        
                                    <td></td>
                                </tr>  
                                  
                              <%
                                      break;
                      }    
                 %>                   
            </table>
        </div>
    </div>
</div>
</asp:Content>

