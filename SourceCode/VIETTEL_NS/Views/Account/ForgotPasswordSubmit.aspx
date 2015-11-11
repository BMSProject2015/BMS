<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXau("Yêu cầu xác nhận thiết lập mật khẩu")%></span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<div id="form2">	
          <table width="100%" cellpadding="0"  cellspacing=""="0" border="0" class="table_form2" >
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Tài khoản của bạn")%></div></td>
               <td class="td_form2_td5"><div>
                     <%=Request.QueryString["username"]%>
                </div></td>                
            </tr>
            <tr>
               <td class="td_form2_td1"><div><%=NgonNgu.LayXau("Email")%></div></td>
               <td class="td_form2_td5"><div>
                     <%=Request.QueryString["email"]%>
                </div></td>                
            </tr>
        </table>
      </div>
    </div>
</div>
</asp:Content>