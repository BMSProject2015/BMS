<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%
    string ParentID = "Edit";
    using (Html.BeginForm("ForgotPasswordSubmit", "Account", new { ParentID = ParentID }))
    {
%>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXau("Quên mật khẩu")%></span>
                </td>
            </tr>
            <tr>
                <td class="td_form2_td1"></td>
                <td class="td_form2_td5">
                    <span id="error" style="display:none;"></span>
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
                     <%= Html.TextBox("username", "", new { style="width:100%" })%>
                </div></td>                
            </tr>
        </table>
      </div>
    </div>
</div>
<div class="cao5px">&nbsp;</div>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
	    <td width="70%">&nbsp;
	    </td>
	    <td  width="30%" align="right">						
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td><input id="logon" type="button" class="button4" value="<%= NgonNgu.LayXau("Xác nhận")%>" /></td>
                    <td width="5px"></td>
                    <td><input type="button" value="Hủy" class="button4" onclick="javascript:history.go(-1);" /></td>
                    <td width="3%"></td>
                </tr>
            </table>         
	    </td>
    </tr>
</table>
    
<%
    }
%>
    <script type="text/javascript">
        $(function() {
            $("#logon").click(function() {
                $("#error").text('').hide();
                $.get('<%=HamRiengModels.SSODomain%>/user/ResetPassword?callback=?',
    				{ username: $("#username").val()},
    				function(ssodata) {
    				    if (ssodata.Username == '') {
    				        $("#error").text('Tài khoản không tồn tại.').show();
    				    } else {
    				        // redirect to authentication page instead of duplicating code here
    				        document.location = '<%=Url.Action("ForgotPasswordSubmit", "Account") %>?username=' + ssodata.ResetPasswordResult.Username + '&email=' + ssodata.ResetPasswordResult.Email;
    				    }
    				    // make sure to tell jQuery this is a JSONP call
    				}, 'jsonp');
            });
        });
    </script>
</asp:Content>