<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="DomainModel.Abstract" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%    
    String ParentID = "XemNCC";
    String iID_MaVatTu = Convert.ToString(ViewData["iID_MaVatTu"]);
    String sMaVatTu = Convert.ToString(ViewData["sMaVatTu"]);
    String sTen = Convert.ToString(ViewData["sTen"]);

    Bang bang = new Bang("DM_NhaCungCap");
    int CurrentPage = 1;
    if (ViewData["VTNCC_page"] != null) CurrentPage = (int)ViewData["VTNCC_page"];
    String SQL = "SELECT * FROM DM_NhaCungCap WHERE iID_MaNhaCungCap IN (SELECT iID_MaNhaCungCap " +
                                                                    "FROM DM_VatTu_NhaCungCap " +
                                                                    "WHERE iID_MaVatTu = '" + iID_MaVatTu + "')";
    bang.TruyVanLayDanhSach.CommandText = SQL;
    DataTable dt = bang.dtData("sTen", CurrentPage);
    int TotalRecords = bang.TongSoBanGhi();
    int TotalPages = (int)(Math.Ceiling((double)TotalRecords / Globals.PageSize));
    int FromRecord = (CurrentPage - 1) * Globals.PageSize + 1;
    int ToRecord = CurrentPage * Globals.PageSize;
    if (TotalPages == CurrentPage)
    {
        ToRecord = FromRecord + dt.Rows.Count - 1;
    }
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
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Thông tin vật tư</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="5" cellspacing="5" width="50%">
                <tr>
                    <td class="td_form2_td1"><div><b>Mã vật tư</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sMaVatTu, "sMaVatTu", "", "class=\"input1_2\" readonly=\"readonly\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div><b>Tên vật tư</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\" readonly=\"readonly\"")%></div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div><br /> 
<%if (TotalRecords > 0)
{ %>
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 50%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b>Danh sách nhà cung cấp từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> nhà cung cấp</b>
                </div>         
            </td>
            <td style="width: 50%">
                <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("XemNCC", new { VTNCC_page = x, iID_MaVatTu = iID_MaVatTu, sMaVatTu = sMaVatTu, sTen = sTen }))%>
                </div>
            </td>
        </tr>
    </table>
</div>
<table cellpadding="0" cellspacing="0" border="0" class="table_form3" >
        <tr class="tr_form3">
            <td width="3%" align="center"><b>STT</b></td>
            <td width="22%" align="center"><b>Tên</b></td>
            <td width="10%" align="center"><b>Tên viết tắt</b></td>
            <td width="20%" align="center"><b>Địa chỉ</b></td>
            <td width="10%" align="center"><b>Số điện thoại</b></td>
            <td width="20%" align="center"><b>Đánh giá chi tiết</b></td>
            <td width="10%" align="center"><b>Hoạt động</b></td>
        </tr>
    <%
      int i;
      for (i = 0; i < dt.Rows.Count; i++)
      {
          if (i % 2 == 0)
          {%>
        <tr >
        <%}
          else
          {%>
        <tr style="background-color:#FFC">
        <%} %>
            <td><%=i + 1%></td>
            <td><%= MyHtmlHelper.ActionLink(Url.Action("Edit", "NhaCungCap", new { iID_MaNhaCungCap = dt.Rows[i]["iID_MaNhaCungCap"] }), dt.Rows[i]["sTen"])%></td>
            <td><%= MyHtmlHelper.ActionLink(Url.Action("Edit", "NhaCungCap", new { iID_MaNhaCungCap = dt.Rows[i]["iID_MaNhaCungCap"] }), dt.Rows[i]["sTenVietTat"])%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sDiaChi"], "sDiaChi")%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sSoDienThoai"], "sSoDienThoai")%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sDanhGiaChiTiet"], "sDanhGiaChiTiet")%></td>
            <td align="center"><%=MyHtmlHelper.LabelCheckBox(ParentID, dt.Rows[i]["bHoatDong"], "bHoatDong")%></td>
        </tr>
    <%
      }
      dt.Dispose();
    %>
</table>
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="padding-left: 20px; padding-top: 3px; color: #ec3237; text-transform:uppercase;">
                <b>Danh sách nhà cung cấp từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> nhà cung cấp</b>
            </td>
            <td>
                <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("XemNCC", new { VTNCC_page = x, iID_MaVatTu = iID_MaVatTu, sMaVatTu = sMaVatTu, sTen = sTen }))%>
                </div>
            </td>
        </tr>
    </table>
</div>
<%}
  else
{ 
%>
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 50%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b>Không có nhà cung cấp nào!</b>
                </div>         
            </td>
        </tr>
    </table>
</div>
<%
}
%>    
<br />
<table cellpadding="0" cellspacing="0" border="0" width="100%">
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
