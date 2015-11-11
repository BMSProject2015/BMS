<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    //PartialModel dlChuyen = (PartialModel)Model;
    String ParentID = "Index";
    String TImport = Convert.ToString(ViewData["TImport"]);

    //String Extension = Request.QueryString["Extension"];
    //String FilePath = Request.QueryString["FilePath"];
    string urlImport = Url.Action("Load", "ImportExcelVatTu", new { ParentID = ParentID, TImport = TImport });

    using (Html.BeginForm("Load", "ImportExcelVatTu", new { ParentID = ParentID, TImport = TImport }))
    {
%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td style="width: 10%">
            <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <%Html.RenderPartial("~/Views/Shared/LinkNhanhVattu.ascx"); %>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>
                        <%=NgonNgu.LayXau("Bước 1: Chọn file")%></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                <tr>
                    <td width="50%">
                        <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                            <tr>
                                <td class="td_form2_td3" style="padding-top: 15px">
                                    <div>
                                        <%=NgonNgu.LayXau("Tải tệp excel")%></div>
                                </td>
                                <td class="td_form2_td5">
                                    <% =MyHtmlHelper.UploadFile("upload", "Libraries/Excels", DateTime.Now.ToString("HHmmss"))%>
                                    <%= Html.Hidden(ParentID + "_sFileName")%>
                                    <%= Html.Hidden(ParentID + "_sDuongDan")%>

                                    <script type="text/javascript">                            
                                        upload.addFilter("Documents (*.xls)", "*.xls");                                        
                                        upload.addListener(upload.UPLOAD_COMPLETE, <%= ParentID%>_uploadFile);
                                                                            
                                        function <%=ParentID%>_uploadFile(filename, url) {                                                                                        
                                            document.getElementById("<%= ParentID%>_sFileName").value = filename;
                                            document.getElementById("<%= ParentID%>_sDuongDan").value = upload.serverPath + "/" + url;
                                        }
                                    </script>

                                </td>
                                <td width="5px">
                                </td>
                                <td class="td_form2_td5" style="padding-top:15px">
                                    <input type="submit" id="<%=ParentID%>_btnUpload" class="button6" value="<%=NgonNgu.LayXau("Nhập dữ liệu")%>" onclick="return checkChoise();" />
                                </td>
                            
                                <script type="text/javascript">
                                    function checkChoise() {
                                        if (document.getElementById("<%= ParentID%>_sFileName").value == "") {
                                            alert('<%=NgonNgu.LayXau("Không tìm thấy tệp excel")%>');                                            
                                            return false;
                                        }
                                        else {
                                            javascript:location.href = '<%=urlImport%>';
                                            return true;
                                        }                            
                                    }
                                </script>
                            
                            </tr>
                        </table>
                    </td>
                    <td width="50%">&nbsp;</td>
                </tr>
                <tr>
                    <td width="50%">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td width="50%" style="padding: 10px 10px;" align="right">
                                    <a href="../../Libraries/Excels/VATTUMAU/MauNhapExcel-VIETTEL.xls">
                                    File mẫu imports của Cục</a>
                                </td>
                                <td width="50%" style="padding: 10px 10px;" align="right">
                                    <a href="../../Libraries/Excels/VATTUMAU/MauNhapExcel-DonVi.xls">
                                    File mẫu imports của đơn vị
                                    </a>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="50%">&nbsp;</td>
                </tr>
            </table>
        </div>
    </div>
</div>
<div class="cao5px">&nbsp;</div>
<%}%>
<div class="cao5px">&nbsp;</div>
</asp:Content>
