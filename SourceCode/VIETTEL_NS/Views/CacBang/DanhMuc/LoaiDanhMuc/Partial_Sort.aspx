<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>


<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<script type="text/javascript" language="javascript">
    function moveOptionUp(obj) {
        var selectedCount = 0;
        for (i = 0; i < obj.options.length; i++) {
            if (obj.options[i].selected) {
                selectedCount++;
            }
        }
        if (selectedCount > 1) {
            return;
        }
        var i = obj.selectedIndex;
        if (i == 0) {
            return;
        }
        swapOptions(obj, i, i - 1);
        obj.options[i - 1].selected = true;
    }

    function moveOptionDown(obj) {
        var selectedCount = 0;
        for (i = 0; i < obj.options.length; i++) {
            if (obj.options[i].selected) {
                selectedCount++;
            }
        }
        if (selectedCount > 1) {
            return;
        }
        var i = obj.selectedIndex;
        if (i == (obj.options.length - 1)) {
            return;
        }
        swapOptions(obj, i, i + 1);
        obj.options[i + 1].selected = true;
    }

    function swapOptions(obj, i, z) {

        var o = obj.options;
        var i_selected = o[i].selected;
        var z_selected = o[z].selected;

        var temp = new Option(o[i].text, o[i].value, o[i].defaultSelected, o[i].selected);
        var temp2 = new Option(o[z].text, o[z].value, o[z].defaultSelected, o[z].selected);
        o[i] = temp2;
        o[z] = temp;
        o[i].selected = z_selected;
        o[z].selected = i_selected;
    }

    function checkOrderSubmit(objText, obj) {
        doicho(objText, obj);
        return true;
    }

    function doicho(objText, obj) {
        var k;
        var tx;
        tx = "";

        for (k = 0; k < obj.options.length; ++k) {
            tx = tx + obj.options[k].value + "$";
        }

        objText.value = tx;
    }
</script>



    <% 
        PartialModel dlChuyen = (PartialModel)Model;
        String ParentID = dlChuyen.ControlID;
        Dictionary<string, object> dicData = dlChuyen.dicData;
        NameValueCollection data = (NameValueCollection)dicData["data"];
        string MaLoaiDanhMuc = (string)(data["iID_MaLoaiDanhMuc"]);
        String SQL = string.Format("SELECT * FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc='{0}' ORDER BY iSTT, iID_MaLoaiDanhMuc", MaLoaiDanhMuc);
        DataTable dt = Connection.GetDataTable(SQL);
        String XauTomTatSapXep = "";
        using (Html.BeginForm("SortSubmit", "LoaiDanhMuc", new { ControlID = "ctlDanhMuc_list"}))
        {
        %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">
			<h3><%=NgonNgu.LayXau("Sắp xếp thứ tự")%>
            </h3>
		</td>
		<td width="30%" align="right">
		    <div style="display:none;">
                <input id="ctlDanhMuc_list_btnLuu" onclick="javascript:checkOrderSubmit(hiddenOrder, SOrder);" type="submit" value="Lưu" />
                <script type="text/javascript">
                    function ctlDanhMuc_list_btnLuu_click()
                    {
                        document.getElementById('ctlDanhMuc_list_btnLuu').click();
                       
                        return false;
                    }
                </script>
            </div>
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td>
            	        <a href="#" onclick="javascript:return ctlDanhMuc_list_btnLuu_click();" class="button4"><%=NgonNgu.LayXau("Lưu")%></a>
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <a href="#" class="button4"><%=NgonNgu.LayXau("Hủy")%></a>
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
<table cellpadding="0" cellspacing="0" border="0" class="table_form3">
    <tr class="tr_form3">
        <td colspan="2"><b>Sắp xếp thứ tự</b></td>
    </tr>
    <tr>
        <td>
            <input type='hidden' name='hiddenOrder'>
            <select name="SOrder" size="5" style="width: 100%; height:300px;">
                <%
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow Row = dt.Rows[i];
                        string strSelected = null;
                        if (i == 0)
                        {
                            strSelected = "selected";
                        }%>                                       
                        <option value="<%= Row["iID_MaDanhMuc"]%>" "<%= strSelected%>"><%= Row["sTen"]%></option>
                    <%}
                    dt.Dispose();    
                    %>                                 
            </select>
        </td>
        <td>
            <input type='button' class='textbox' value='↑' onClick='moveOptionUp(SOrder);'><br><br>
            <input type='button' class='textbox' value='↓' onClick='moveOptionDown(SOrder);'><br><br>
        </td>
    </tr>
</table>
<%= Html.Hidden(ParentID + "_DuLieuMoi", XauTomTatSapXep)%>

<%}%>