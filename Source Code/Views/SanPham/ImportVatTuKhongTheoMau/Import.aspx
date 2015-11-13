<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="System.Data.OleDb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Import
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%  
    String ParentID = "Import";
    
    String FileName = Request.QueryString["FileName"];
    String FilePath = Request.QueryString["FilePath"];
    String TImport = Request.QueryString["TImport"];
    string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
    conStr = String.Format(conStr, FilePath);
    OleDbConnection connExcel = new OleDbConnection(conStr);
    OleDbCommand cmdExcel = new OleDbCommand();
    OleDbDataAdapter oda = new OleDbDataAdapter();
    cmdExcel.Connection = connExcel;
    connExcel.Open();

    DataTable dtSheet = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
    SqlCommand cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                              "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                           "FROM QT_NguoiDung " +
                                                           "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
    cmd.Parameters.AddWithValue("@sID_MaNguoiDung", User.Identity.Name);
    String iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
    cmd.Dispose();
    DataRow[] dataRow = null;
    //if (iID_MaDonViDangNhap == "-1")
        dataRow = dtSheet.Select("TABLE_NAME IN ('MaVatTu$','Sheet1$')");
    DataTable dt = dtSheet.Clone();
    foreach (DataRow dr in dataRow)
    {
        dt.ImportRow(dr);
    }
    SelectOptionList slSheets = new SelectOptionList(dt, "TABLE_NAME", "TABLE_NAME");
    connExcel.Close();
    using (Html.BeginForm("EditSubmit", "ImportVatTuKhongTheoMau", new { ParentID = ParentID }))
    {          
%>
<%= Html.Hidden(ParentID + "_FileName", FileName)%>
<%= Html.Hidden(ParentID + "_FilePath", FilePath)%>
<br />
    <div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Chọn thông tin</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="Div1">
        <div id="Div2">
            <table cellpadding="5" cellspacing="5" width="50%">
                <tr>
                    <td class="td_form2_td1"><div><b>SheetName</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, slSheets, string.Empty, "SheetName", "", "style=\"width: 50%;\"")%>
                        <%--<%= Html.ValidationMessage(ParentID + "_" + "err_TABLE_NAME")%>--%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" align="right">
                 <div style="text-align:right; float:right;">
                        <input type="button" class="button4" value="Xác nhận" onclick="btnXem();" />
            	        <%--<input type="submit" class="button4" value="Lọc" />--%>
            	      </div> 
            	    </td>
            	     <td class="td_form2_td5">
            	     <div>
            	        &nbsp;
            	        </div>
            	    </td>
                </tr>
            </table>
        </div>
    </div>
</div> 
<div class="doancach">&nbsp;</div>
<div class="box_tong" style="display:none" id="<%= ParentID %>_ChiTiet">
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
            <div class="content" style="overflow:auto" id="<%= ParentID %>_tdList">                            
                <%ImportExcelVatTuController.SheetData dataSheet = ImportExcelVatTuController.get_objSheet(ParentID, FilePath, string.Empty);%>
                <%= dataSheet.sData%>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    function btnXem() {
        var FilePath, SheetName;
        var url = '<%= Url.Action("get_dtSheet?ParentID=#0&FilePath=#1&SheetName=#2", "ImportVatTuKhongTheoMau") %>';
        FilePath = document.getElementById('<%= ParentID %>_FilePath').value;
        SheetName = document.getElementById('<%= ParentID %>_SheetName').value;

        url = url.replace("#0", "<%= ParentID %>");
        url = url.replace("#1", FilePath);
        url = url.replace("#2", SheetName);

        $.getJSON(url, function (data) {
            document.getElementById("<%=ParentID%>_ChiTiet").style.display = 'block';
            document.getElementById("<%= ParentID %>_tdList").innerHTML = data.sData;
        });
    }
    function setCheckboxes() {
        var cb = document.getElementById('<%= ParentID %>_tdList').getElementsByTagName('input');

        for (var i = 0; i < cb.length; i++) {
            cb[i].checked = document.getElementById('checkall').checked;
        }
    }                                       
</script>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td>
            	        <input type="submit" class="button4" value="Lưu" />
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <input type="button" class="button4" value="Quay lại" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
<%} %>
</asp:Content>
