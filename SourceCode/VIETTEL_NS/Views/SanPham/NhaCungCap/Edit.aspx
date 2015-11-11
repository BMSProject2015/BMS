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
    String ParentID = "Edit";
    String iID_MaNhaCungCap = Convert.ToString(ViewData["iID_MaNhaCungCap"]);
    String sID_MaNguoiDung = User.Identity.Name;
    DataTable dt = null;

    cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                           "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                        "FROM QT_NguoiDung " +
                                                        "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
    cmd.Parameters.AddWithValue("@sID_MaNguoiDung", sID_MaNguoiDung);
    String iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
    cmd.Dispose();
    
    String sTen = "";
    String sTenVietTat = "";
    String sDiaChi = "";
    String sSoDienThoai = "";
    String sFax = "";
    String sEmail = "";
    String sMaSoThue = "";
    String sDanhGiaChiTiet = "";
    String iID_MaDonVi = "";
    String TenDonVi = "";
    String bHoatDong = "true";
    if (!String.IsNullOrEmpty(iID_MaNhaCungCap))
    {
        cmd = new SqlCommand("SELECT * FROM DM_NhaCungCap WHERE iID_MaNhaCungCap = @iID_MaNhaCungCap");
        cmd.Parameters.AddWithValue("@iID_MaNhaCungCap", iID_MaNhaCungCap);
        dt = Connection.GetDataTable(cmd);
        cmd.Dispose();
        if (dt.Rows.Count > 0)
        {
            sTen = Convert.ToString(dt.Rows[0]["sTen"]);
            sTenVietTat = Convert.ToString(dt.Rows[0]["sTenVietTat"]);
            sDiaChi = Convert.ToString(dt.Rows[0]["sDiaChi"]);
            sSoDienThoai = Convert.ToString(dt.Rows[0]["sSoDienThoai"]);
            sFax = Convert.ToString(dt.Rows[0]["sFax"]);
            sEmail = Convert.ToString(dt.Rows[0]["sEmail"]);
            sMaSoThue = Convert.ToString(dt.Rows[0]["sMaSoThue"]);
            sDanhGiaChiTiet = Convert.ToString(dt.Rows[0]["sDanhGiaChiTiet"]);
            bHoatDong = Convert.ToString(dt.Rows[0]["bHoatDong"]);
            
            iID_MaDonVi = Convert.ToString(dt.Rows[0]["iID_MaDonVi"]);            
            if (iID_MaDonVi == "")
            {
                iID_MaDonVi = null;
                TenDonVi = "BQP";
            }
        }
    }
    else
        iID_MaDonVi = iID_MaDonViDangNhap;
    if (iID_MaDonVi == "-1")
    {
        iID_MaDonVi = null;
        TenDonVi = "BQP";
    }
    else if(iID_MaDonVi != null)
    {
        
        cmd = new SqlCommand("SELECT sTen FROM NS_DonVi WHERE iID_MaDonVi = " + iID_MaDonVi);
        TenDonVi = Connection.GetValueString(cmd, "");
        cmd.Dispose();
    }

    using (Html.BeginForm("EditSubmit", "NhaCungCap", new { ParentID = ParentID, iID_MaNhaCungCap = iID_MaNhaCungCap, iID_MaDonViDangNhap = iID_MaDonViDangNhap }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>    
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
                            <td class="td_form2_td1"><div><b>Tên</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Tên viết tắt</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sTenVietTat, "sTenVietTat", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Mã số thuế</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sMaSoThue, "sMaSoThue", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Địa chỉ</b></div></td>
                            <td class="td_form2_td5">
                                <div>
                                <%=MyHtmlHelper.TextArea(ParentID, sDiaChi, "sDiaChi", "", "style=\"width:98%;font:12px/20px Tahoma;height:50px;\"")%>
                                </div>
                            </td>
                        </tr>
                         <tr>
                            <td class="td_form2_td1"><div><b>Hoạt động</b></div></td>
                            <td class="td_form2_td5">
                                <div><%= MyHtmlHelper.CheckBox(ParentID, bHoatDong, "bHoatDong","")%></div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="50%">
                    <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                       <tr>
                            <td class="td_form2_td1"><div><b>Số điện thoại</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sSoDienThoai, "sSoDienThoai", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                       <tr>
                            <td class="td_form2_td1"><div><b>Fax</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sFax, "sFax", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                         <tr>
                            <td class="td_form2_td1"><div><b>Email</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sEmail, "sEmail", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Đánh giá chi tiết</b></div></td>
                            <td class="td_form2_td5">
                                <div>
                                <%=MyHtmlHelper.TextArea(ParentID, sDanhGiaChiTiet, "sDanhGiaChiTiet", "", "style=\"width:98%;font:12px/20px Tahoma;height:50px;\"")%>
                                </div>
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

<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
        	    <%if ((iID_MaDonVi == iID_MaDonViDangNhap) || (iID_MaDonVi == null && iID_MaDonViDangNhap == "-1"))
                    { %>
            	    <td>
            	        <input type="submit" id="Luu" class="button4" value="Lưu" />
            	    </td>
            	    <%} %>
                    <td width="5px"></td>
                    <td>
                        <input type="button" class="button4" value="Quay lại" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
<%
    }
%>
</asp:Content>
