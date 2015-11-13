<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <center>
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
            <img src="/Content/ajax-loader.gif" /><br />
	        <p>Please wait while we verify your identity...</p>
        </div>
    </center>
    </center>


	<script type="text/javascript">

		$(function() {
			// log user out from SSO service
		    $.get('<%=HamRiengModels.SSODomain%>/user/Logout?callback=?', {},
				function(ssodata) {
					// client's no longer logged in, redirect to logon page
					document.location = '<%=Url.Action("LogOn", "Account") %>';
				}, 'jsonp');
		});
		
	</script>

</asp:Content>
