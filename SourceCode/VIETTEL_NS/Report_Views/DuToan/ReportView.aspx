<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%    
        String path = Convert.ToString(ViewData["path"]);
    %>
      <div id="main">
            <div id="left">
                <div class="content" style="margin-bottom:20px;">
                     <%Html.RenderPartial(path); %>
                </div>
            </div>
        </div>
     
</asp:Content>
