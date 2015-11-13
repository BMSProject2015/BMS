<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="System.Reflection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% 
    String ParentID = "XemTonKho";
    String iID_MaVatTu = Convert.ToString(ViewData["iID_MaVatTu"]);
    String sMaVatTu = Convert.ToString(ViewData["sMaVatTu"]);
    String TongTon = Convert.ToString(ViewData["SoLuongTonKho"]);
    String DonViTinh = Convert.ToString(ViewData["DonViTinh"]);

    SqlCommand cmd = new SqlCommand("SELECT * FROM NS_DonVi ORDER BY iID_MaDonVi");
    DataTable dtDonVi = Connection.GetDataTable(cmd);
    cmd.Dispose();

    String SQL = "SELECT iID_MaDonVi, rSoLuongTonKho, dNgaySua FROM DM_DonVi_TonKho " +
                    "WHERE iID_MaVatTu = '" + iID_MaVatTu + "' ORDER BY iID_MaDonVi";
    cmd = new SqlCommand(SQL);
    DataTable dt = Connection.GetDataTable(cmd);
    cmd.Dispose();
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
        <table cellpadding="0" cellspacing="0" border="0" width="100%" >
        	<tr>
            	<td><span>Chi tiết tồn kho</span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="5" cellspacing="5" width="50%" >
                 <tr>
                    <td class="td_form2_td1"><div><b>Mã vật tư</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sMaVatTu, "sMaVatTu", "", "style=\"width: 50%;\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div><b>Tổng số lượng tồn kho</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, TongTon + ' ' + DonViTinh, "TongTon", "", "style=\"width: 50%;\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%></div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div><br />
<%
if (dt.Rows.Count > 0)
{%>
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 50%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b><%=NgonNgu.LayXau("Danh sách đơn vị tồn kho")%></b>
                </div>         
            </td>
            <td style="width: 7%">
                <div style="padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b><%=NgonNgu.LayXau("Chức năng")%></b>
                </div>
            </td>
            <td style="width: 20%">
                <div style="width: 100%; float: left;">
                    <div id="titlemenu">
                        <ul id="titleheader">
                            <li class="level1-li sub">
                                <%=MyHtmlHelper.ActionLink(Url.Action("ExportExcel", "TimKiemVatTu", new { SQL = SQL, iID_MaVatTu = iID_MaVatTu, sMaVatTu = sMaVatTu, SoLuongTonKho = TongTon, DonViTinh = DonViTinh }), "Xuất Excel", "", "", "class=\"level1-a\"")%>
                            </li>
                        </ul>
                    </div>
                </div>
            </td>
            <td style="width: 23%">&nbsp;</td>
        </tr>
    </table>
</div>
<table cellpadding="0" cellspacing="0" border="0" class="table_form3" >
    <tr class="tr_form3">
        <td width="3%" align="center"><b>STT</b></td>
        <td width="50%" align="center"><b>Đơn vị</b></td>
        <td width="22%" align="center"><b>Số lượng tồn kho</b></td>
        <td width="25%" align="center"><b>Ngày cập nhật tồn kho</b></td>
    </tr>
    <%
    int i, j;
    if (dt != null)
    {
        String TenDonVi = "";
        DateTime dNgayCapNhatTonKho = DateTime.Now;
        String SoLuongTonKho = "0";
        for (i = 0; i < dt.Rows.Count; i++)
        {
            SoLuongTonKho = CommonFunction.DinhDangSo(dt.Rows[i]["rSoLuongTonKho"]);
            dNgayCapNhatTonKho = Convert.ToDateTime(dt.Rows[i]["dNgaySua"]);

            if (Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) == "") TenDonVi = "BQP";
            else
            {
                for (j = 0; j < dtDonVi.Rows.Count; j++)
                {
                    if (Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) == Convert.ToString(dtDonVi.Rows[j]["iID_MaDonVi"]))
                    {
                        TenDonVi = Convert.ToString(dtDonVi.Rows[j]["sTen"]);
                        break;
                    }
                }
            }
            if (i % 2 == 0)
            {%>
            <tr >
            <%}
            else
            {%>
            <tr style="background-color:#FFC">
            <%} %>
            <td><%=i + 1%></td>
            <td><%=MyHtmlHelper.ActionLink(Url.Action("Index", "XemTonKhoTheoDonVi", new { iID_MaDonVi = dt.Rows[i]["iID_MaDonVi"] }), TenDonVi)%></td>
            <td><%= MyHtmlHelper.Label(SoLuongTonKho + " " + DonViTinh, "SoLuongTonKho")%></td>
            <td><%= MyHtmlHelper.Label(String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dNgayCapNhatTonKho), "dNgayCapNhatTonKho")%></td>
            </tr>
        <%}
    }
    dt.Dispose();%>
</table>
<%         
}
else
{
%>
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 100%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b><%=NgonNgu.LayXau("Không có danh dách đơn vị tồn kho vật tư này!")%></b>
                </div>         
            </td>
        </tr>
    </table>
</div>
<%
}
%>
<br /><table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
                    <td width="5px"></td>
                    <td>
                        <input type="button" class="button4" value="Quay lại" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
    </table>
</asp:Content>
