<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.IO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ChangeDirectory
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
      
        String OnSuccess = "";
        OnSuccess = Request.QueryString["OnSuccess"];
        String ParentID = "Change";
        string myDirectory = string.Empty;
        //String path = Request.QueryString["path"];
        string path = string.Empty;
        string sessionName = Request.QueryString["sessionName"];
        string contronllerName = Request.QueryString["contronllerName"];  
        if (Session[sessionName] != null)
        {
            path = Convert.ToString(Session[sessionName]);
        }
        else
        {
            path = System.Configuration.ConfigurationManager.AppSettings["FilePath"];

        }
        string flask1 = @"\";
        string flask2 = @"\\";
        path = path.Replace(flask2, flask1);
        if (!string.IsNullOrEmpty(path))
        {
            myDirectory = Convert.ToString(path);

        }
        string[] directories = null;
        if (!string.IsNullOrEmpty(myDirectory))
        {
            directories = Directory.GetDirectories(myDirectory);
        }

        using (Html.BeginForm("submit", "ChangeDirectory", new { ParentID = ParentID , sessionName = sessionName, contronllerName = contronllerName}))
        {
    %>
    <div style="background-color: #f0f9fe; background-repeat: repeat; border: solid 1px #ec3237">
        <div style="padding: 10px;">
            <div id="nhapform" style="width: 100%; height: 400px">
                <table style="width: 100%; height: 100%; vertical-align: top">
                    <tr style="height: 30px">
                        <td style="width: 50%">
                            <%-- <input type="text" id="txtURL" name="txtURL" value='<%=path %>' style="width: 80%"  />--%>
                            <%= MyHtmlHelper.TextBox(ParentID, path, "path", "", "style=\"width:95%;\"")%><br />                            
                            <%= Html.ValidationMessage(ParentID + "_" + "err_path")%>
                        </td>
                        <td style="width: 30%">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%; text-align: right">
                                        <input class="button" type="submit" value="Go" title="Go" />
                                    </td>
                                    <td style="width: 50%; text-align: left">
                                        <input class="button" type="button" value="Trở lại" onclick="history.go(-1)" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="height: 30px">
                        <td colspan="3">
                            Đường dẫn thư mục lựu chọn:
                            <%= path %>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <% if (directories != null)
                               {
                                   int count = 0;
                                   foreach (string directory in directories)
                                   { %>
                            <%int lastIndex = directory.Length - 1 - directory.LastIndexOf("\\");
                              string folder = directory.Substring(directory.LastIndexOf("\\") + 1, lastIndex);
                              count++;
                            %>
                            <% if (count % 5 == 0)
                               {%>
                            <br />
                            <%

                                } %>
                            <%=MyHtmlHelper.ActionLink(Url.Action("SubFoldersubmit", "ChangeDirectory", new { path = directory, sessionName = sessionName , contronllerName = contronllerName}), folder)%>
                            &nbsp;&nbsp;&nbsp;
                            <%}

                            }%>
                        </td>
                    </tr>
                    <tr style="text-align: center">
                        <td colspan="3">
                            <%=MyHtmlHelper.ActionLink(Url.Action("SaveSubmit", "ChangeDirectory", new {path = path, sessionName = sessionName, contronllerName = contronllerName}), "Lựa chọn")%>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%}%>
</asp:Content>
