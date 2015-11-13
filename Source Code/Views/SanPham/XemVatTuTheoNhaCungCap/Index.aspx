<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="System.Reflection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXau("Cổng thông tin điện tử Vicem")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% 
    String ParentID = "Index";
    String iID_MaNhaCungCap = Convert.ToString(ViewData["iID_MaNhaCungCap"]);

    SqlCommand cmd = new SqlCommand("SELECT iID_MaNhaCungCap, sTen FROM DM_NhaCungCap " +
                                "WHERE bHoatDong = 1 ORDER BY sTen");
    DataTable dtNCC = Connection.GetDataTable(cmd);
    DataRow R = dtNCC.NewRow();
    dtNCC.Rows.InsertAt(R, 0);
    SelectOptionList slNCC = new SelectOptionList(dtNCC, "iID_MaNhaCungCap", "sTen");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                     "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                 "FROM DC_LoaiDanhMuc " +
                                                                 "WHERE sTenBang = 'DonViTinh') ORDER BY sTenKhoa");
    DataTable dtDonViTinh = Connection.GetDataTable(cmd);
    cmd.Dispose();

    String sMaVatTu = "";
    String SQL = "";
    DataTable dt = null;

    if (iID_MaNhaCungCap != "")
    {
        SQL = "SELECT iID_MaVatTu, sMaVatTu, sTen, sQuyCach, sTenGoc, iDM_MaDonViTinh FROM DM_VatTu " +
              "WHERE iID_MaVatTu IN (SELECT iID_MaVatTu FROM DM_VatTu_NhaCungCap " + 
                                    "WHERE iID_MaNhaCungCap = '" + iID_MaNhaCungCap + "')";        
    }

    Bang bang = new Bang("DM_VatTu");
    int CurrentPage = 1;
    if (ViewData["XemVatTuTheoNhaCungCap_page"] != null) CurrentPage = (int)ViewData["XemVatTuTheoNhaCungCap_page"];
    bang.TruyVanLayDanhSach.CommandText = SQL;
    int TotalRecords = 0;
    int TotalPages = 0;
    int FromRecord = 0;
    int ToRecord = 0;
    if (iID_MaNhaCungCap != "")
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
    
    using (Html.BeginForm("Search", "XemVatTuTheoNhaCungCap", new { ParentID = ParentID }))
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
                    <td class="td_form2_td1"><div><b>Nhà cung cấp</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, slNCC, iID_MaNhaCungCap, "iID_MaNhaCungCap", "", "class=\"input1_2\"")%></div>
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
 <%if (TotalRecords > 0)
   {%>
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="padding-left: 20px; padding-top: 3px; color: #ec3237; text-transform:uppercase;">
                <b>Danh sách từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> vật tư</b>
            </td>
            <td>
                <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { XemVatTuTheoNhaCungCap_page = x, iID_MaNhaCungCap = iID_MaNhaCungCap }))%>
                </div>
            </td>
        </tr>
    </table>
</div>
<table cellpadding="0" cellspacing="0" border="0" class="table_form3" >
    <tr class="tr_form3">
        <td width="3%" align="center"><b>STT</b></td>
        <td width="10%" align="center"><b>Mã vật tư</b></td>
        <td width="40%" align="center"><b>Tên</b></td>
        <td width="20%" align="center"><b>Quy cách</b></td>
        <td width="20%" align="center"><b>Tên gốc</b></td>
        <td width="7%" align="center"><b>Đơn vị tính</b></td>
    </tr>
    <%
       int i, j;
       if (dt != null)
       {
           String sTen = "";
           String DonViTinh = "";
           for (i = 0; i < dt.Rows.Count; i++)
           {
               sMaVatTu = Convert.ToString(dt.Rows[i]["sMaVatTu"]);
               sTen = Convert.ToString(dt.Rows[i]["sTen"]);

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
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sQuyCach"], "sQuyCach")%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sTenGoc"], "sTenGoc")%></td>
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
                    <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { XemVatTuTheoNhaCungCap_page = x, iID_MaNhaCungCap = iID_MaNhaCungCap }))%>
                </div>
            </td>
        </tr>
    </table>
</div>
<%}
   else
   { %>
   <div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="padding-left: 20px; padding-top: 3px; color: #ec3237; text-transform:uppercase;">
                <b>Không có vật tư nào!</b>
            </td>
        </tr>
    </table>
</div>
<%} %>
<%}
%>  
</asp:Content>
