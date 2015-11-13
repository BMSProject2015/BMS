<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
    String iID_MaDanhMucGia = Convert.ToString(ViewData["iID_MaDanhMucGia"]);
    String iID_MaDanhMucGia_Cha = Convert.ToString(ViewData["iID_MaDanhMucGia_Cha"]);
    String iID_MaSanPham = Convert.ToString(ViewData["iID_MaSanPham"]);
    String iID_MaLoaiHinh = Convert.ToString(ViewData["iID_MaLoaiHinh"]);
    SqlCommand cmd = new SqlCommand();
    if (iID_MaDanhMucGia_Cha != null && iID_MaDanhMucGia_Cha != "")
    {//sap xep con cua 1 danh muc
        cmd.CommandText = "SELECT * FROM DM_SanPham_DanhMucGia WHERE iID_MaDanhMucGia_Cha=@iID_MaDanhMucGia_Cha AND iTrangThai = 1 AND iID_MaChiTietGia = 0 ORDER BY iSTT";
        cmd.Parameters.AddWithValue("@iID_MaDanhMucGia_Cha", iID_MaDanhMucGia_Cha);
    }
    else
    {// danh muc ko co cha
        cmd.CommandText = "SELECT * FROM DM_SanPham_DanhMucGia WHERE iID_MaDanhMucGia_Cha = 0 AND iTrangThai = 1 AND iID_MaChiTietGia = 0 AND iID_MaSanPham = @iID_MaSanPham AND iID_MaLoaiHinh = @iID_MaLoaiHinh ORDER BY iSTT";
        cmd.Parameters.AddWithValue("@iID_MaSanPham", iID_MaSanPham);
        cmd.Parameters.AddWithValue("@iID_MaLoaiHinh", iID_MaLoaiHinh);
    }
    DataTable dt = Connection.GetDataTable(cmd);

    using (Html.BeginForm("SortSubmit", "SanPham_DanhMucGia", new { iID_MaDanhMucGia_Cha = iID_MaDanhMucGia_Cha, iID_MaSanPham = iID_MaSanPham, iID_MaLoaiHinh = iID_MaLoaiHinh }))
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
            <div style="padding-top: 5px; padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham_DanhMucGia"), "Danh sách chỉ tiêu")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Sắp xếp danh sách chỉ tiêu mẫu</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width: 50%; padding-top: 3px;">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
	                        <tr>
		                        <td width="30%" align="right">
		                            <div style="display:none;">
                                        <input id="btnLuu" onclick="javascript:checkOrderSubmit(hiddenOrder, SOrder);" type="submit" value="Lưu" />
                                        <script type="text/javascript">
                                            function btnLuu_click() {
                                                document.getElementById('btnLuu').click();
                                                return false;
                                            }
                                        </script>
                                    </div>
                                    <table cellpadding="0" cellspacing="0" border="0" align="right">
        	                            <tr>
            	                            <td>
            	                                <a href="#" onclick="javascript:return btnLuu_click();" class="button"><%=NgonNgu.LayXau("Lưu")%></a>
            	                            </td>
                                            <td width="5px"></td>
                                            <td>
                                                <input type="button" value="Hủy" class="button" onclick="javascript:history.go(-1);" />
                                            </td>
                                        </tr>
                                    </table>
		                        </td>
	                        </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <table cellpadding="0" cellspacing="0" border="0" class="table_form3">
            <tr>
                <td>
                    <input type='hidden' name='hiddenOrder'>
                    <select name="SOrder" size="20" style="width: 100%">
                        <%
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow Row = dt.Rows[i];
            string strSelected = null;
            if (i == 0)
            {
                strSelected = "selected";
            }%>                                       
                                <option value="<%= Row["iID_MaDanhMucGia"]%>" "<%= strSelected%>"><%= Row["sKyHieu"]%> - <%= Row["sTen"]%></option>
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
        </div>
    </div>
<%}%> 
</asp:Content>


