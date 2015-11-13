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

<%--<link rel="stylesheet" href="../../Content/Rating/jRating.jquery.css" type="text/css" />    
<script type="text/javascript" src="../../Content/Rating/jquery.js"></script>    
<script type="text/javascript" src="../../Content/Rating/jRating.jquery.js"></script> --%>
<%
    SqlCommand cmd;
    String ParentID = "Index";
  
    String sID_MaNguoiDung = User.Identity.Name;
    cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                           "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                        "FROM QT_NguoiDung " +
                                                        "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
    cmd.Parameters.AddWithValue("@sID_MaNguoiDung", sID_MaNguoiDung);
    String iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
    cmd.Dispose();

    String sTenNhaCungCap = Convert.ToString(ViewData["sTenNhaCungCap"]); ;
    String sDiaChiNhaCungCap = Convert.ToString(ViewData["sDiaChiNhaCungCap"]); ;
    String SQL = "SELECT * FROM DM_NhaCungCap ";
    String DK = "";
    
    if (!String.IsNullOrEmpty(sTenNhaCungCap))
        DK += "WHERE sTen like '%" + sTenNhaCungCap + "%' ";
    if (!String.IsNullOrEmpty(sDiaChiNhaCungCap))
    {
        if ( DK != "")
            DK += "AND sDiaChi like '%" + sDiaChiNhaCungCap + "%' ";
        else
            DK += "WHERE sDiaChi like '%" + sDiaChiNhaCungCap + "%' ";
    }
    SQL += DK;
    
    Bang bang = new Bang("DM_NhaCungCap");
    int CurrentPage = 1;
    if (ViewData["NhaCungCap_page"] != null) CurrentPage = (int)ViewData["NhaCungCap_page"];
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
     using (Html.BeginForm("Search", "NhaCungCap", new { ParentID = ParentID}))
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
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Chọn thông tin</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="5" cellspacing="5" width="50%">
                <tr>
                    <td class="td_form2_td1"><div><b>Tên nhà cung cấp</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sTenNhaCungCap, "sTenNhaCungCap", "", "class=\"input1_2\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div><b>Địa chỉ nhà cung cấp</b></div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, sDiaChiNhaCungCap, "sDiaChiNhaCungCap", "", "class=\"input1_2\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1" align="right">
                 <div style="text-align:right; float:right;">
            	        <input type="submit" class="button4" value="Lọc" />
            	      </div> 
            	    </td>
            	     <td class="td_form2_td5">
            	     <div>
            	        &nbsp;
            	        </div>
            	    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<br />
<%if (TotalRecords > 0)
  { %>
  <div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 50%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b>Danh sách từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> nhà cung cấp</b>
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
                                <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "NhaCungCap", new { }), "Thêm mới", "", "", "class=\"level1-a\"")%>
                            </li>
                            <li class="level1-li sub">
                                <%=MyHtmlHelper.ActionLink(Url.Action("ExportExcel", "NhaCungCap", new { ParentID = ParentID, SQL = SQL }), "Xuất Excel", "", "", "class=\"level1-a\"")%>
                            </li>
                        </ul>
                    </div>
                </div>
            </td>
            <td style="width: 23%">
                <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { NhaCungCap_page = x }))%>
                </div>
            </td>
        </tr>
    </table>
</div>
<%} %>
    <table cellpadding="0" cellspacing="0" border="0" class="table_form3" >
        <tr class="tr_form3">
            <td width="3%" align="center"><b>STT</b></td>
            <td width="22%" align="center"><b>Tên</b></td>
            <td width="10%" align="center"><b>Tên viết tắt</b></td>
            <td width="20%" align="center"><b>Địa chỉ</b></td>
            <td width="10%" align="center"><b>Số điện thoại</b></td>
            <td width="20%" align="center"><b>Đánh giá chi tiết</b></td>
            <td width="10%" align="center"><b>Hoạt động</b></td>
            <td width="5%" align="center"><b>&nbsp;</b></td>
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
            <td><%=i+1 %></td>
            <td><%= MyHtmlHelper.ActionLink(Url.Action("Edit", "NhaCungCap", new { iID_MaNhaCungCap = dt.Rows[i]["iID_MaNhaCungCap"] }), dt.Rows[i]["sTen"])%></td>
            <td><%= MyHtmlHelper.ActionLink(Url.Action("Edit", "NhaCungCap", new { iID_MaNhaCungCap = dt.Rows[i]["iID_MaNhaCungCap"] }), dt.Rows[i]["sTenVietTat"])%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sDiaChi"], "sDiaChi")%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sSoDienThoai"], "sSoDienThoai")%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sDanhGiaChiTiet"], "sDanhGiaChiTiet")%></td>
            <td align="center"><%=MyHtmlHelper.LabelCheckBox(ParentID, dt.Rows[i]["bHoatDong"], "bHoatDong")%></td>
            <td>
            <%if ((Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) == iID_MaDonViDangNhap) || (Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) == "" && iID_MaDonViDangNhap == "-1"))
              {%>
                <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "NhaCungCap", new { iID_MaNhaCungCap = dt.Rows[i]["iID_MaNhaCungCap"] }), "Sửa")%>
            <%} %>&nbsp;
            </td>
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
                    <b>Danh sách từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> nhà cung cấp</b>
                </td>
                <td>
                    <div class="msdn" style="padding-top: 5px;">
                        <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { NhaCungCap_page = x }))%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
<%
    }
%>
</asp:Content>
