<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#dvText').show();
            $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
            $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer 
            $(window).resize(function () {
                $('.popup_block').css({
                    position: 'absolute',
                    left: ($(window).width() - $('.popup_block').outerWidth()) / 2,
                    top: ($(window).height() - $('.popup_block').outerHeight()) / 2
                });
            });
            // To initially run the function:
            $(window).resize();
            //Fade in Background
        });
    </script>
    <center>
        <div id="dvText" class="popup_block">
            <img src="/Content/ajax-loader.gif" alt="" /><br />
	        <p>Please wait while we verify your identity...</p>
        </div>
    </center>

    <%
        String ReturnURL = Convert.ToString(Request.QueryString["ReturnURL"]);
        String strURL = "";
        if (ReturnURL != "" && ReturnURL != null)
        {
           strURL = ReturnURL.Replace("%26","&");
        }
    %>

	<script type="text/javascript">
	    $(function () {
	        $.get('<%=HamRiengModels.SSODomain%>/user/RequestToken?callback=?', {},
				function (ssodata) {
				    // get url to logon page in case this operation fails
				    var logonPage = '<%=Url.Action("LogOn", "Account", new { ReturnUrl = strURL}) %>';
				    if (ssodata.Status == 'SUCCESS') {
				        // get target url for successful authentication
				        var redirect = '<%=strURL%>';
				        if (redirect == '')
				            redirect = '<%=Url.Action("Index", "Home") %>';

				        // verify the token is genuine
				        $.post('<%=Url.Action("Authenticate", "Account", new { ReturnUrl = strURL}) %>',
							{ token: ssodata.Token, createPersistentCookie: false },
								function (data) {
								    // redirect user based on result
								    if (data.result == 'SUCCESS')
								        document.location = redirect;
								    else
								        document.location = logonPage;
								    // just regular json here
								}, 'json');
				    } else {
				        // user needs to logon to SSO service
				        document.location = logonPage;
				    }
				    // tell jQuery to use JSONP 
				}, 'jsonp');
	    });
		
	</script>

</asp:Content>
