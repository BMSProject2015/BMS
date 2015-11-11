<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<VIETTEL.Models.LogOnModel>" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="VIETTEL.Models" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">

    <%
        String strURL = Convert.ToString(Request.QueryString["ReturnURL"]);
      
    %>
    <% using (Html.BeginForm()) { %>
        <table width="95%" border="0" cellspacing="0" cellpadding="0">
		  <tr height="50px">
			<td width="35%">&nbsp;</td>
			<td width="65%" class="formdn"><b><%= NgonNgu.LayXau("Đăng nhập") %></b></td>
		  </tr>
		  <tr>
			<td class="dn"><%= NgonNgu.LayXau("Tên đăng nhập")%></td>
			<td class="formdn">
			    <%= Html.TextBox("username", "", new { style = "width:100%", onKeyPress = "doClickSearchTitle('logon',event)" })%>
            </td>
		  </tr>
		  <tr>
			<td class="dn"><%= NgonNgu.LayXau("Mật khẩu")%></td>
			<td class="formdn">
			    <%= Html.Password("password", "", new { style = "width:100%", onKeyPress = "doClickSearchTitle('logon',event)" })%>
			</td>
		  </tr>
		   <tr>
		  	<td colspan="2" style="height:6px;font-size:2px;">&nbsp;</td>
		  </tr>
		  <tr>
			<td >&nbsp;</td>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
				  <tr>
					<td><%= Html.CheckBox("rememberMe", new { onKeyPress = "doClickSearchTitle('logon',event)" })%></td>
					<td><a href="#"><%= NgonNgu.LayXau("Nhớ tài khoản") %></a></td>
					<td>|&nbsp; <a href="<%=Url.Action("ForgotPassword", "Account") %>"><%= NgonNgu.LayXau("Quên mật khẩu")%></a></td>
				  </tr>
				</table>
			</td>			
		  </tr>
		  <tr>
		  	<td colspan="2"  style="height:6px;font-size:2px;">&nbsp;</td>
		  </tr>
		  <tr>
			<td>&nbsp;</td>
			<td style="padding-top:10px;">
			    <input id="logon" type="button" class="loginsubmit" value="<%= NgonNgu.LayXau("Đăng nhập")%>" />
			</td>
		  </tr>
		  <tr>
			<td>&nbsp;</td>
			<td style="padding-top:10px;">
			    <span id="error" style="display:none;"></span>
			</td>
		  </tr>
		</table>
   <% } %>    
    <script type="text/javascript">
        $(function () {
            $("#logon").click(function () {
                $("#error").text('').hide();
                jQuery.ajaxSetup({ cache: false });
                var url = '<%= Url.Action("get_CheckLogin?UserName=#0", "Public") %>';
                url = unescape(url);
                url = url.replace("#0", $("#username").val());
                $.getJSON(url, function (data) {
                    if (data.data == 1) {
                        $.get('<%=HamRiengModels.SSODomain%>/user/Login?callback=?',
    				    { username: $("#username").val(), password: $("#password").val() },
    				    function (ssodata) {
    				        if (ssodata.LoginResult.Status == 'DENIED') {
    				            $("#error").text('Sai tài khoản hoặc mật khẩu.').show();
    				        } else {
    				            // redirect to authentication page instead of duplicating code here
    				            document.location = '<%=Url.Action("Authenticate", "Account", new { ReturnUrl = strURL}) %>';
    				        }
    				        // make sure to tell jQuery this is a JSONP call
    				    }, 'jsonp');
                    }
                    else {
                        $("#error").text('Sai tài khoản hoặc mật khẩu.').show();
                    }
                });
            });
        });


        function doClickSearchTitle(buttonName, e) {//the purpose of this function is to allow the enter key to point to the correct button to click.
            var key;

            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(buttonName);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>
</asp:Content>
