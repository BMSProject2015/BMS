<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>


<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
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
        string TenBang = (string)dicData["sTenBang"];
        SqlCommand cmd = new SqlCommand("SELECT * FROM PQ_DanhMucTruong WHERE sTenBang=@sTenBang ORDER BY iSTT;");
        cmd.Parameters.AddWithValue("@sTenBang", TenBang);
        DataTable dt = Connection.GetDataTable(cmd);
        String XauTomTatSapXep = "";
        int h = dt.Rows.Count * 16+10;
        using (Html.BeginForm("SortSubmit", "DanhMucTruong", new { ControlID = ParentID }))
        {
        %>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td width="70%"><span>Sắp xếp thứ tự trường</span></td>
                <td width="30%" align="right">
                    <div style="display:none;">
                        <input id="<%=ParentID%>_btnLuu" onclick="javascript:checkOrderSubmit(hiddenOrder, SOrder);" type="submit" value="Lưu" />
                        <script type="text/javascript">
                            function <%=ParentID%>_btnLuu_click()
                            {
                                document.getElementById('<%=ParentID%>_btnLuu').click();
                                return false;
                            }
                        </script>
                    </div>
                </td>
            </tr>
        </table>
	</div>
    <div id="nhapform">		
		<div id="form2">
            <table cellpadding="0" cellspacing="0" border="0" class="table_form3">
                <tr>
                    <td>
                        <input type='hidden' name='hiddenOrder'>
                        <select name="SOrder" size="20" style="width: 100%; height:<%=h%>px;">
                            <%
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    DataRow Row = dt.Rows[i];
                                    string strSelected = null;
                                    if (i == 0)
                                    {
                                        strSelected = "selected";
                                    }%>                                       
                                    <option value="<%= Row["iID_MaDanhMucTruong"]%>" "<%= strSelected%>"><%= Row["sTenTruong"]%></option>
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
            <div class="cao5px">&nbsp;</div>
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
	                <td width="70%">&nbsp;
	                </td>
	                <td  align="right" style="padding: 0px 15px 0px 0px;">						
                        <table cellpadding="0" cellspacing="0" border="0" align="right">
        	                <tr>
            	                <td><input type="submit" type="button" class="button4" onclick="javascript:return <%=ParentID%>_btnLuu_click();" value="<%=NgonNgu.LayXau("Lưu")%>"/></td>
                                <td width="5px"></td>
                                <td><input type="button" value="Hủy" class="button4" onclick="javascript:history.go(-1);" /></td>
                            </tr>
                        </table>         
	                </td>
                </tr>
            </table>
            <div class="cao5px">&nbsp;</div>
        </div>
    </div>    
</div>
<%= Html.Hidden(ParentID + "_DuLieuMoi", XauTomTatSapXep)%>
<%}%>