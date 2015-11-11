<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="System.Reflection" %>
<%
String ParentID = "", MaLuat = "";
PropertyInfo[] properties = Model.GetType().GetProperties();
int i;
for (i = 0; i < properties.Length; i++)
{
    switch (properties[i].Name)
    {
        case "ControlID":
            ParentID = (string)(properties[i].GetValue(Model, null));
            break;

        case "MaLuat":
            MaLuat = (string)(properties[i].GetValue(Model, null));
            break;
    }
}
using (Html.BeginForm("EditSubmit", "ChucNangCam", new { ControlID = ParentID, MaLuat = MaLuat }))
{
    
    SqlCommand cmd = new SqlCommand();
    cmd.CommandText = "SELECT * FROM PQ_Bang_ChucNangCam WHERE iID_MaLuat=@iID_MaLuat ORDER BY sTenBang";
    cmd.Parameters.AddWithValue("@iID_MaLuat", MaLuat);
    DataTable dt = Connection.GetDataTable(cmd);
    String TenLuat = (string)(CommonFunction.LayTruong("PQ_Luat", "iID_MaLuat", MaLuat, "sTen"));
    String SQL = "SELECT * FROM PQ_DanhMucBang WHERE bHoatDong=1 ORDER BY iSTT, sTenBangHT;";
    DataTable dtBang = Connection.GetDataTable(SQL);
%>
<div class="box_tong">
    <div class="title_tong">
		<table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span><%=NgonNgu.LayXau("Phân quyền chức năng cấm của các bảng")%> CHO LUẬT: <% =TenLuat%></span>
                </td>
                <td align="right">
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
	</div>    
	<div id="nhapform">		
		<table class="mGrid">
            <tr>
                <th align="center"><%=NgonNgu.LayXau("Tên bảng dữ liệu") %></th>
                <th align="center" style="width:70px">
                    <%=NgonNgu.LayXau("Xem")%><br />
                    <input type="checkbox" id="<%= ParentID %>_checkall_xem" onclick="setCheckboxes('xem')" />                    
                </th>
                <th align="center" style="width:70px">
                    <%=NgonNgu.LayXau("Thêm")%><br />
                    <input type="checkbox" id="<%= ParentID %>_checkall_them" onclick="setCheckboxes('them')" />
                </th>
                <th align="center" style="width:70px">
                    <%=NgonNgu.LayXau("Xóa")%><br />
                    <input type="checkbox" id="<%= ParentID %>_checkall_xoa" onclick="setCheckboxes('xoa')" />
                </th>
                <th align="center" style="width:70px">
                    <%=NgonNgu.LayXau("Sửa")%><br />
                    <input type="checkbox" id="<%= ParentID %>_checkall_sua" onclick="setCheckboxes('sua')" />
                </th>
                <th align="center" style="width:70px">
                    <%=NgonNgu.LayXau("Chia sẻ")%><br />
                    <input type="checkbox" id="<%= ParentID %>_checkall_chiase" onclick="setCheckboxes('chiase')" />
                </th>
                <th align="center" style="width:100px">
                    <%=NgonNgu.LayXau("Giao phụ trách")%><br />
                    <input type="checkbox" id="<%= ParentID %>_checkall_giaophutrach" onclick="setCheckboxes('giaophutrach')" />
                </th>
            </tr>
                <%
                int j=0;
                String strSua_TruongCam = NgonNgu.LayXau("Sửa");
                String TenBang, TenBangHT;
                String strXem = "", strThem = "", strXoa = "", strSua = "", strChiaSe = "", strGiaoPhuTrach = "", ChucNangCam, txtTenBang;
                for (i = 0; i <= dtBang.Rows.Count - 1; i++)
                {
                    DataRow Row = dtBang.Rows[i];
                    TenBang = (string)Row["sTenBang"];
                    TenBangHT = (string)Row["sTenBangHT"];
                    txtTenBang = TenBang +";";
                    strXem = "";
                    strThem = "";
                    strXoa = "";
                    strSua = "";
                    strChiaSe = "";
                    strGiaoPhuTrach = "";
                    for (j = 0; j <= dt.Rows.Count - 1; j++)
                    {
                        if (TenBang == (String)(dt.Rows[j]["sTenBang"]))
                        {
                            ChucNangCam = (String)(dt.Rows[j]["sChucNang"]);
                            if (ChucNangCam.IndexOf("Detail")>=0)
                            {
                                strXem = " checked='checked'";
                                txtTenBang += "0;";
                            }
                            if (ChucNangCam.IndexOf("Create") >= 0)
                            {
                                strThem = " checked='checked'";
                                txtTenBang += "1;";
                            }
                            if (ChucNangCam.IndexOf("Delete") >= 0)
                            {
                                strXoa = " checked='checked'";
                                txtTenBang += "2;";
                            }
                            if (ChucNangCam.IndexOf("Edit") >= 0)
                            {
                                strSua = " checked='checked'";
                                txtTenBang += "3;";
                            }
                            if (ChucNangCam.IndexOf("Share") >= 0)
                            {
                                strSua = " checked='checked'";
                                txtTenBang += "4;";
                            }
                            if (ChucNangCam.IndexOf("Responsibility") >= 0)
                            {
                                strSua = " checked='checked'";
                                txtTenBang += "5;";
                            }
                            break;
                        }
                    }
                    if (Convert.ToBoolean(Row["bXem"]))
                    {
                        strXem = String.Format("<input type=\"checkbox\" name=\"{0}_xem\" id=\"{0}_xem_{1}\" onclick=\"changeCheckbox({1})\" {2}/>",ParentID, i, strXem);
                    }
                    else
                    {
                        strXem = "&nbsp;";
                    }
                    if (Convert.ToBoolean(Row["bThem"]))
                    {
                        strThem = String.Format("<input type=\"checkbox\" name=\"{0}_them\" id=\"{0}_them_{1}\" onclick=\"changeCheckbox({1})\" {2}/>",ParentID, i, strThem);
                    }
                    else
                    {
                        strThem = "&nbsp;";
                    }
                    if (Convert.ToBoolean(Row["bSua"]))
                    {
                        strSua = String.Format("<input type=\"checkbox\" name=\"{0}_sua\" id=\"{0}_sua_{1}\" onclick=\"changeCheckbox({1})\" {2}/>",ParentID, i, strSua);
                    }
                    else
                    {
                        strSua = "&nbsp;";
                    }
                    if (Convert.ToBoolean(Row["bXoa"]))
                    {
                        strXoa = String.Format("<input type=\"checkbox\" name=\"{0}_xoa\" id=\"{0}_xoa_{1}\" onclick=\"changeCheckbox({1})\" {2}/>",ParentID, i, strXoa);
                    }
                    else
                    {
                        strXoa = "&nbsp;";
                    }
                    if (Convert.ToBoolean(Row["bChiaSe"]))
                    {
                        strChiaSe = String.Format("<input type=\"checkbox\" name=\"{0}_chiase\" id=\"{0}_chiase_{1}\" onclick=\"changeCheckbox({1})\" {2}/>", ParentID, i, strChiaSe);
                    }
                    else
                    {
                        strChiaSe = "&nbsp;";
                    }
                    if (Convert.ToBoolean(Row["bGiaoPhuTrach"]))
                    {
                        strGiaoPhuTrach = String.Format("<input type=\"checkbox\" name=\"{0}_giaophutrach\" id=\"{0}_giaophutrach_{1}\" onclick=\"changeCheckbox({1})\" {2}/>", ParentID, i, strGiaoPhuTrach);
                    }
                    else
                    {
                        strGiaoPhuTrach = "&nbsp;";
                    }

                    String classtr = "";
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    %>
                    <tr <%=classtr %>>
                        <td style="padding: 3px 2px;">
                            <%= TenBangHT%>
                            <input type="hidden" name="<%= ParentID %>_txt" id="<%= ParentID %>_txt_<%= i %>" value="<%= txtTenBang %>"/>
                        </td>
                        <td align="center">
                            <%= strXem%>
                        </td>
                        <td align="center">
                            <%= strThem%>
                        </td>
                        <td align="center">
                            <%= strXoa%>
                        </td>
                        <td align="center">
                            <%= strSua%>
                        </td>
                        <td align="center">
                            <%= strChiaSe%>
                        </td>
                        <td align="center">
                            <%= strGiaoPhuTrach%>
                        </td>
                    </tr>
                 <%   
                }
                dt.Dispose();
                dtBang.Dispose();
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
            cb[i].checked = v;
            changeCheckbox(i);
        }
    }

    function changeCheckbox(i) {
        var txt = document.getElementById('<%= ParentID %>_txt_' + i);
        var vXem = false;
        var vThem = false;
        var vXoa = false;
        var vSua = false;
        var vChiaSe = false;
        var vGiaoPhuTrach = false;

        if (document.getElementById('<%= ParentID %>_xem_' + i)) {
            vXem = document.getElementById('<%= ParentID %>_xem_' + i).checked;
        }
        if (document.getElementById('<%= ParentID %>_them_' + i)) {
            vThem = document.getElementById('<%= ParentID %>_them_' + i).checked;
        }
        if (document.getElementById('<%= ParentID %>_xoa_' + i)) {
            vXoa = document.getElementById('<%= ParentID %>_xoa_' + i).checked;
        }
        if (document.getElementById('<%= ParentID %>_sua_' + i)) {
            vSua = document.getElementById('<%= ParentID %>_sua_' + i).checked;
        }
        if (document.getElementById('<%= ParentID %>_chiase_' + i)) {
            vChiaSe = document.getElementById('<%= ParentID %>_chiase_' + i).checked;
        }
        if (document.getElementById('<%= ParentID %>_giaophutrach_' + i)) {
            vGiaoPhuTrach = document.getElementById('<%= ParentID %>_giaophutrach_' + i).checked;
        }
        txt.value = txt.value.split(";")[0]+";";
        if (vXem) {
            txt.value += "0;";
        }
        if (vThem) {
            txt.value += "1;";
        }
        if (vXoa) {
            txt.value += "2;";
        }
        if (vSua) {
            txt.value += "3;";
        }
        if (vChiaSe) {
            txt.value += "4;";
        }
        if (vGiaoPhuTrach) {
            txt.value += "5;";
        }
    }

</script>
<%} %>