<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>

<%@ Import Namespace="System.Reflection" %>

<%
String ParentID = "", MaLuat = "", TenBang="";
PropertyInfo[] properties = Model.GetType().GetProperties();

for (int i = 0; i < properties.Length; i++)
{
    switch (properties[i].Name)
    {
        case "ControlID":
            ParentID = (string)(properties[i].GetValue(Model, null));
            break;

        case "MaLuat":
            MaLuat = (string)(properties[i].GetValue(Model, null));
            break;

        case "TenBang":
            TenBang = (string)(properties[i].GetValue(Model, null));
            break;
    }
}
using (Html.BeginForm("EditSubmit", "TruongCam", new { ControlID = ParentID, MaLuat = MaLuat, TenBang = TenBang }))
{
    
    SqlCommand cmd = new SqlCommand();
    cmd.CommandText = "SELECT sTenTruong FROM PQ_Bang_TruongCam WHERE iID_MaLuat=@iID_MaLuat AND sTenBang=@sTenBang ORDER BY sTenTruong";
    cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
    cmd.Parameters.AddWithValue("@sTenBang", TenBang);
    String DanhSachTruongCam = (String)(Connection.GetValue(cmd,""));
    String TenLuat = (string)(CommonFunction.LayTruong("PQ_Luat", "iID_MaLuat", MaLuat, "sTen"));
    cmd = new SqlCommand();
    //cmd.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TABLE_NAME;";
    //cmd.Parameters.AddWithValue("@TABLE_NAME", TenBang);
    cmd.CommandText = "SELECT * FROM PQ_DanhMucTruong WHERE sTenBang = @sTenBang ORDER BY iSTT;";
    cmd.Parameters.AddWithValue("@sTenBang", TenBang);
    DataTable dtTruong = Connection.GetDataTable(cmd);
%>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXau("Phân quyền trường cấm của các bảng")%> cho luật <% =TenLuat%></span>
                </td>
            </tr>
        </table>
	</div>    
	<div id="nhapform">		
		<table class="mGrid">
            <tr>
                <th align="center" style="width:150px"><%=NgonNgu.LayXau("Mã") %></th>
                <th align="center"><%=NgonNgu.LayXau("Tên") %></th>
                <th align="center" style="width:70px">
                    <%=NgonNgu.LayXau("Xem")%><br />
                    <input type="checkbox" id="<%= ParentID %>_checkall_xem" onclick="setCheckboxes('xem')"/>
                </th>
            </tr>
                <%
                int i,d=-1;
                for (i = 0; i <= dtTruong.Rows.Count - 1; i++)
                {
                    DataRow Row = dtTruong.Rows[i];
                    String TenTruong = (string)Row["sTenTruong"];
                    String TenTruongHT = (string)Row["sTenTruongHT"];
                    String txtTenTruong = TenTruong;
                    d = d + 1;
                    String strXem="";
                    if ((Boolean)Row["bLuonDuocXem"])
                    {
                        strXem = " disabled ";
                        txtTenTruong += "";
                    }
                    else
                    {
                        if (BaoMat.ChoPhepNhap(TenTruong, DanhSachTruongCam) == false)
                        {
                            strXem = " checked='checked'";
                            txtTenTruong += ";0";
                        }
                    }
                    String classtr = "";
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    
                    %>
                    <tr <%=classtr %>>
                        <td>
                            <%= TenTruong%>
                        </td>
                        <td>
                            <%= TenTruongHT%>
                            <input type="hidden" name="<%= ParentID %>_txt" id="<%= ParentID %>_txt_<%= d %>" value="<%=txtTenTruong %>"/>
                        </td>
                        <td align="center">
                            <input type="checkbox" name="<%= ParentID %>_xem" id="<%= ParentID %>_xem_<%= d %>" onclick="changeCheckbox(<%= d %>)" <%= strXem%>/>
                        </td>
                    </tr>
                    
                <%      
                }
                dtTruong.Dispose();
                %>
        </table>
    </div>
</div>
<div class="cao5px">&nbsp;</div>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
	    <td width="70%">&nbsp;
	    </td>
	    <td  width="30%" align="right">						
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td><input type="submit" type="button" class="button4" value="<%=NgonNgu.LayXau("Lưu")%>"/></td>
                    <td width="5px"></td>
                    <td><input type="button" value="<%=NgonNgu.LayXau("Hủy")%>" class="button4" onclick="javascript:history.go(-1);" /></td>
                </tr>
            </table>         
	    </td>
    </tr>
</table>
<div class="cao5px">&nbsp;</div>
<script type="text/javascript">
    function setCheckboxes(id) {
        var cb = document.getElementsByName('<%= ParentID %>_' + id);
        var v = document.getElementById('<%= ParentID %>_checkall_' + id).checked;
        for (var i = 0; i < cb.length; i++) {
            if (cb[i].disabled == false) {
                cb[i].checked = v;
                changeCheckbox(i);
            }
        }
    }

    function changeCheckbox(i) {
        var txt = document.getElementById('<%= ParentID %>_txt_' + i);
        var vXem = false;

        if (document.getElementById('<%= ParentID %>_xem_' + i)) {
            vXem = document.getElementById('<%= ParentID %>_xem_' + i).checked;
        }

        txt.value = txt.value.split(";")[0];
        if (vXem) {
            txt.value += ";0";
        }
    }

</script>
<%} %>