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
    String ParentID = "Index";
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);

    SqlCommand cmd = new SqlCommand("SELECT * FROM NS_DonVi ORDER BY iSTT");
    DataTable dtDonVi = Connection.GetDataTable(cmd);
    DataRow R = dtDonVi.NewRow();
    R["iID_MaDonVi"] = "-1";
    R["sTen"] = "BQP";
    dtDonVi.Rows.InsertAt(R, 0);
    R = dtDonVi.NewRow();
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                 "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                             "FROM DC_LoaiDanhMuc " +
                                                             "WHERE sTenBang = 'DonViTinh') ORDER BY sTenKhoa");
    DataTable dtDonViTinh = Connection.GetDataTable(cmd);
    cmd.Dispose();

    String sMaVatTu = "";
    String SQL = "";
    String DK = "";
    DataTable dtDonVi_TonKho = null;
    if (iID_MaDonVi == "-1")
    {
        SQL = "SELECT iID_MaVatTu, sMaVatTu, sTen, iDM_MaDonViTinh, rSoLuongTonKho, dNgayCapNhatTonKho FROM DM_VatTu ";
    }
    else if (iID_MaDonVi != "")
    {
        DK = " WHERE iID_MaDonVi = " + iID_MaDonVi;
        SQL = "SELECT iID_MaVatTu, sMaVatTu, sTen, iDM_MaDonViTinh FROM DM_VatTu " +
              "WHERE iID_MaVatTu IN (SELECT iID_MaVatTu FROM DM_DonVi_TonKho " + DK + ")";

        cmd = new SqlCommand("SELECT iID_MaVatTu, rSoLuongTonKho, dNgaySua AS dNgayCapNhatTonKho FROM DM_DonVi_TonKho" + DK);
        dtDonVi_TonKho = Connection.GetDataTable(cmd);
        cmd.Dispose();
    }

    Bang bang = new Bang("DM_VatTu");
    int CurrentPage = 1;
    if (Request.QueryString["XemTonKhoTheoDonVi_page"] != null)
        CurrentPage = Convert.ToInt32(Request.QueryString["XemTonKhoTheoDonVi_page"]);

    bang.TruyVanLayDanhSach.CommandText = SQL;
    DataTable dt = null;
    int TotalRecords = 0;
    int TotalPages = 0;
    int FromRecord = 0;
    int ToRecord = 0;
    if (iID_MaDonVi != "")
    {
        dt = bang.dtData("sMaVatTu", CurrentPage);
        TotalRecords = bang.TongSoBanGhi();
        TotalPages = (int)(Math.Ceiling((double)TotalRecords / Globals.PageSize));
        FromRecord = (CurrentPage - 1) * Globals.PageSize + 1;
        ToRecord = CurrentPage * Globals.PageSize;
        if (TotalPages == CurrentPage)
        {
            ToRecord = FromRecord + dt.Rows.Count - 1;
        }
    }
        

    using (Html.BeginForm("Search", "XemTonKhoTheoDonVi", new { ParentID = ParentID }))
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
            <%Html.RenderPartial("~/Views/Shared/LinkNhanhVattu.ascx"); %>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" >
        	<tr>
            	<td><span>Chọn thông tin</span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="5" cellspacing="5" width="50%" >
                <tr>
                    <td class="td_form2_td1"><div><b>Đơn vị</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" align="right">
                      <div style="text-align:right; float:right; width:100%">
            	        <input type="submit" class="button4" value="Lọc" style="float:right;"/>
            	      </div>
            	    </td>
            	    <td class="td_form2_td5">
            	     <div>&nbsp;</div>
            	    </td>
                </tr>
            </table>
        </div>
    </div>
</div><br />
<%if (iID_MaDonVi != "")
  {%>
<%} %>
 <%if (TotalRecords > 0)
{%>
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 50%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b>Danh sách từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> vật tư</b>
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
                                <%=MyHtmlHelper.ActionLink(Url.Action("ExportExcel", "XemTonKhoTheoDonVi", new { ParentID = ParentID, iID_MaDonVi = iID_MaDonVi }), "Xuất Excel", "", "", "class=\"level1-a\"")%>
                            </li>
                        </ul>
                    </div>
                </div>
            </td>
            <td style="width: 23%">
                <div class="msdn" style="padding-top: 5px;">
                <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { XemTonKhoTheoDonVi_page = x, iID_MaDonVi = iID_MaDonVi }))%>
                </div>
            </td>
        </tr>
    </table>
</div>
<%} %>
<table cellpadding="0" cellspacing="0" border="0" class="table_form3" >
    <tr class="tr_form3">
        <td width="3%" align="center"><b>STT</b></td>
        <td width="10%" align="center"><b>Mã vật tư</b></td>
        <td width="43%" align="center"><b>Tên</b></td>
        <td width="15%" align="center"><b>Số lượng tồn kho</b></td>
        <td width="22%" align="center"><b>Ngày cập nhật tồn kho</b></td>
        <td width="7%" align="center"><b>Đơn vị tính</b></td>
    </tr>
    <%
    int i, j;
    if (dt != null)
    {
    String sTen = "";
    DateTime dNgayCapNhatTonKho = DateTime.Now;
    String DonViTinh = "";
    String SoLuongTonKho = "0";
    for (i = 0; i < dt.Rows.Count; i++)
    {
        DonViTinh = "";
        SoLuongTonKho = "0";
        sMaVatTu = Convert.ToString(dt.Rows[i]["sMaVatTu"]);
        sTen = Convert.ToString(dt.Rows[i]["sTen"]);
        if (iID_MaDonVi == "-1")
        {
            if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["rSoLuongTonKho"])))
                SoLuongTonKho = CommonFunction.DinhDangSo(dt.Rows[i]["rSoLuongTonKho"]);
            else
                SoLuongTonKho = "0";
            if (!(dt.Rows[i]["dNgayCapNhatTonKho"] is DBNull))
                dNgayCapNhatTonKho = Convert.ToDateTime(dt.Rows[i]["dNgayCapNhatTonKho"]);
        }
        else
        {
            if (dtDonVi_TonKho != null)
            {
                for (j = 0; j < dtDonVi_TonKho.Rows.Count; j++)
                {
                    if (Convert.ToString(dt.Rows[i]["iID_MaVatTu"]) == Convert.ToString(dtDonVi_TonKho.Rows[j]["iID_MaVatTu"]))
                    {
                        SoLuongTonKho = CommonFunction.DinhDangSo(dtDonVi_TonKho.Rows[j]["rSoLuongTonKho"]);
                        dNgayCapNhatTonKho = Convert.ToDateTime(dtDonVi_TonKho.Rows[j]["dNgayCapNhatTonKho"]);
                        break;
                    }
                }
            }
        }
        
        for (j = 0; j < dtDonViTinh.Rows.Count; j++)
        {
            if (Convert.ToString(dt.Rows[i]["iDM_MaDonViTinh"]) == Convert.ToString(dtDonViTinh.Rows[j]["iID_MaDanhMuc"]))
            {
                DonViTinh = Convert.ToString(dtDonViTinh.Rows[j]["sTen"]);
                break;
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
            <td><%= MyHtmlHelper.Label(sMaVatTu, "sMaVatTu")%></td>
            <td><%= MyHtmlHelper.Label(sTen, "sTen")%></td>
            <td><%= MyHtmlHelper.Label(SoLuongTonKho + " " + DonViTinh, "SoLuongTonKho")%></td>
            <td>
            <%if (iID_MaDonVi == "-1"){
                if (!(dt.Rows[i]["dNgayCapNhatTonKho"] is DBNull))
              { %>
            <%= MyHtmlHelper.Label(String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dNgayCapNhatTonKho), "dNgayCapNhatTonKho")%>
            <%}else{ %>&nbsp;<%} }
              else{%>
              <%= MyHtmlHelper.Label(String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dNgayCapNhatTonKho), "dNgayCapNhatTonKho")%>
              <%}%>
            </td>
            <td align="center"><%= MyHtmlHelper.Label(DonViTinh, "iDM_MaDonViTinh")%></td>
        </tr>
        <%
    }
    dt.Dispose();
    }%>
</table>
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="padding-left: 20px; padding-top: 3px; color: #ec3237; text-transform:uppercase;">
                <b>Danh sách từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> vật tư</b>
            </td>
            <td>
                <div class="msdn" style="padding-top: 5px;">
                <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { XemTonKhoTheoDonVi_page = x, iID_MaDonVi = iID_MaDonVi }))%>
                </div>
            </td>
        </tr>
    </table>
</div>
<%   }
%>  
</asp:Content>
