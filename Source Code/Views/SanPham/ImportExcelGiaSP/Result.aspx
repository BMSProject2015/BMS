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
    String ParentID = "";
    DataTable dt = (DataTable)ViewData["dtImportResult"];
    String sLoi = Convert.ToString(ViewData["sLoi"]);
    String ResultSheetName = Convert.ToString(ViewData["ResultSheetName"]);
    int CountRecord = Convert.ToInt16(ViewData["CountRecord"]);
                       
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
<table cellpadding="0" cellspacing="0" border="0" width="100%" style="margin:0px 0 2px;">
    <%        
        if (string.IsNullOrEmpty(sLoi) == false)
        {
    %>
    <h3>
        <%=NgonNgu.LayXau("Xảy ra sự cố")%>
        <br />
        &nbsp;<span style="color: Red"><%=sLoi%></span>
    </h3>    
    <%
        }
        if (CountRecord > 0)
        {
    %>
	<tr>
        <td>
            <h3>
                <%=NgonNgu.LayXau("Đã thêm")%>&nbsp;<%=CountRecord%>&nbsp;<%=NgonNgu.LayXau("bản ghi")%>&nbsp;<%=NgonNgu.LayXau("từ sheet")%>&nbsp;<%=ResultSheetName%>
            </h3>
        </td>
	</tr>
</table>

<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span><%=NgonNgu.LayXauChuHoa("Thông tin chi tiết")%></span>
                </td>
            </tr>
        </table>
    </div>
    <div class="form_nhap">
        <div class="form2">
            <div class="content" style="overflow:auto">                            
                <table cellpadding="0" cellspacing="0" border="0" class="table_form3">
                    <tr class="tr_form3">
                        <%if (dt != null && dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Columns.Count; i++)
                                {%>
                        <td><b><%=NgonNgu.LayXau(""+  dt.Columns[i].ColumnName +"")%></b></td>
                        <%}%>
                    </tr>
                    <%
                        for (int j = 0; j <= dt.Rows.Count - 1; j++)
                        {
                            DataRow dataRow = dt.Rows[j];
                    %>
                    <tr>
                        <%
                            for (int m = 0; m < dataRow.Table.Columns.Count; m++)
                            {
                                string dataRowText = dataRow[m].ToString().Trim();
                        %>
                        <td><%=MyHtmlHelper.Label(dataRowText, dt.Columns[m].ColumnName)%></td>
                        <%}%>
                    </tr>
                    <%}
                      dt.Dispose();
                    }%>
                </table>
            </div>
        </div>
    </div>
</div>

<div class="cao5px">&nbsp;</div>
<table cellpadding="0" cellspacing="0" border="0" align="right">
    <tr><td><input type="button" class="button4" onclick="history.go(-1)" value="<%=NgonNgu.LayXau("Quay lại")%>" /></td></tr>
</table>
<div class="cao5px">&nbsp;</div>
        <%
        }
        else
        {
        %>          
        <tr>
            <td style="width:5px">
                <h3>
                    <%=NgonNgu.LayXau("Không nhập được bản ghi nào")%> 
                </h3>
            </td>
        </tr>              
        <%}%>
<div class="cao5px">&nbsp;</div>
</asp:Content>
