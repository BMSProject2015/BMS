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
    sLoaiBaoCao = "0";
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
                    <th>Tên báo cáo</th>                        
                    <th style="width: 25%;">Phụ lục số</th> 
                </tr>
                <%
                    switch (sLoaiBaoCao) { 
                        case "0":
                            %>
                            <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">1</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPBTong4_1"), "Thông báo ngân sách- Thông báo")%></td>                        
                                <td align="center" style="padding: 3px 2px;"></td>
                            </tr>  
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">2</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPBTong5"), "Tổng hợp chỉ tiêu")%></td>                        
                                <td align="center" style="padding: 3px 2px;"></td>
                            </tr>  
                             <tr class="alt">
                                <td align="center" style="padding: 3px 2px;">3</td>
                                <td style="padding: 3px 2px;"><%=MyHtmlHelper.ActionLink(Url.Action("Index", "rptPBTong6"), "Tổng hợp chỉ tiêu đơn vị")%></td>                        
                                <td align="center" style="padding: 3px 2px;"></td>
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
