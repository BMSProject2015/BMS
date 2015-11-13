<%@ Page Title="" Language="C#" culture="vi-VN" uiCulture="vi-VN"  MasterPageFile="~/Views/Shared/Site_Report.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%    
        String path = Convert.ToString(ViewData["path"]);
    %>
   <script runat="server">
  void Page_Load(object sender, EventArgs e) {
      string lang = "vi-VN";
    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
    System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(lang);
  }
</script>
      <div id="main">
            <div id="left">
                <div class="content" style="margin-bottom:20px;">                
                     <%Html.RenderPartial(path); %>
                </div>
            </div>
        </div>
     
</asp:Content>
