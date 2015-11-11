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
    SqlCommand cmd;
    String ParentID = "Create";
    
    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomLoaiVatTu') ORDER BY sTenKhoa");
    DataTable dt = Connection.GetDataTable(cmd);
    SelectOptionList slNhomLoaiVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slNhomChinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slNhomPhu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slChiTietVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'XuatXu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slXuatXu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();
    
    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                       "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                   "FROM DC_LoaiDanhMuc " +
                                                                   "WHERE sTenBang = 'DonViTinh') ORDER BY sTen");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slDonViTinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTen");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDonVi, sTen FROM NS_DonVi ORDER BY sTen");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slDonVi = new SelectOptionList(dt, "iID_MaDonVi", "sTen");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_DMTrangThai, sTen FROM DM_TrangThai ORDER BY iSTT");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slTrangThai = new SelectOptionList(dt, "iID_DMTrangThai", "sTen");
    cmd.Dispose();    
        
    String sMaVatTu = "";
    String sTen = "";
    String sTenGoc = "";
    String dNgayPhatSinhMa = "";
    String iTrangThai = "2";
    String iDM_MaNhomLoaiVatTu = Convert.ToString(ViewData["MaNhomLoaiVatTu"]);
    String iDM_MaNhomChinh = Convert.ToString(ViewData["MaNhomChinh"]);
    String iDM_MaNhomPhu = Convert.ToString(ViewData["MaNhomPhu"]);
    String iDM_MaChiTietVatTu = Convert.ToString(ViewData["MaChiTietVatTu"]);
    String iDM_MaXuatXu = Convert.ToString(ViewData["MaXuatXu"]);
    String iID_MaDonVi = "";
    String iDM_MaDonViTinh = "";
    using (Html.BeginForm("EditSubmit", "MaVatTu", new { ParentID = ParentID, ThemNhanh = "1" }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>  
 
<br /><br />
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
                    <td class="td_form2_td1"><div><b>Nhóm loại vật tư</b></div></td>
                    <td class="td_form2_td5"
                        <div><%=MyHtmlHelper.DropDownList(ParentID, slNhomLoaiVatTu, iDM_MaNhomLoaiVatTu, "iDM_MaNhomLoaiVatTu", "", "onchange=\"ChonMa()\" style=\"width: 60%;\"")%></div>        
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div><b>Nhóm chính</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, slNhomChinh, iDM_MaNhomChinh, "iDM_MaNhomChinh", "", "onchange=\"ChonMa()\" style=\"width: 60%;\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div><b>Nhóm phụ</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, slNhomPhu, iDM_MaNhomPhu, "iDM_MaNhomPhu", "", " onchange=\"ChonMa()\" style=\"width: 60%;\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div><b>Chi tiết vật tư</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, slChiTietVatTu, iDM_MaChiTietVatTu, "iDM_MaChiTietVatTu", "", " onchange=\"ChonMa()\" style=\"width: 60%;\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div><b>Tình trạng vật tư</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, slXuatXu, iDM_MaXuatXu, "iDM_MaXuatXu", "", " onchange=\"ChonMa()\" style=\"width: 60%;\"")%></div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div><br /> 

<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin</span>
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
                            <td class="td_form2_td1"><div><b>Mã vật tư</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sMaVatTu, "sMaVatTu", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Tên</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="td_form2_td1"><div><b>Trạng thái</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="50%">
                    <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                       <tr>
                            <td class="td_form2_td1"><div><b>Đơn vị tính</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, iDM_MaDonViTinh, "iDM_MaDonViTinh", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                       <tr>
                            <td class="td_form2_td1"><div><b>Tên gốc</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sTenGoc, "sTenGoc", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="td_form2_td1"><div><b>Đơn vị phát sinh mã</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td class="td_form2_td1"><div><b>Ngày phát sinh mã</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DatePicker(ParentID, dNgayPhatSinhMa, "dNgayPhatSinhMa", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr><td class="td_form2_td1" colspan="2">&nbsp;</td></tr>
        </table>
        </div>
    </div>
</div><br />
<script type="text/javascript">
      ChonMa();
     function ChonMa() {

         var MaNhomLoaiVatTu = document.getElementById('<%=ParentID %>_iDM_MaNhomLoaiVatTu');
         var MaNhomChinh = document.getElementById('<%=ParentID %>_iDM_MaNhomChinh');
         var MaNhomPhu = document.getElementById('<%=ParentID %>_iDM_MaNhomPhu');
         var MaChiTietVatTu = document.getElementById('<%=ParentID %>_iDM_MaChiTietVatTu');
         var MaXuatXu = document.getElementById('<%=ParentID %>_iDM_MaXuatXu');
         var url = '<%= Url.Action("get_dtMaVatTu?ParentID=#0&MaNhomLoaiVatTu=#1&MaNhomChinh=#2&MaNhomPhu=#3&MaChiTietVatTu=#4&MaXuatXu=#5", "MaVatTu") %>';

         url = url.replace("#0", "<%= ParentID %>");
         url = url.replace("#1", MaNhomLoaiVatTu.options[MaNhomLoaiVatTu.selectedIndex].text);
         url = url.replace("#2", MaNhomChinh.options[MaNhomChinh.selectedIndex].text);
         url = url.replace("#3", MaNhomPhu.options[MaNhomPhu.selectedIndex].text);
         url = url.replace("#4", MaChiTietVatTu.options[MaChiTietVatTu.selectedIndex].text);
         url = url.replace("#5", MaXuatXu.options[MaXuatXu.selectedIndex].text);
         $.getJSON(url, function(data) {
             document.getElementById("<%= ParentID %>_sMaVatTu").value = data;
         });
     } 
</script> 
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td>
            	        <input type="submit" class="button4" value="Lưu" onclick="return message();" />
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <input type="submit" class="button4" value="Hủy" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
<script type="text/javascript" language="javascript">
    function message() {
        return confirm("Đã thêm mới thành công!");
    }
</script> 
<%
    }
%>
</asp:Content>
