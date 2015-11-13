<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String LoaiDanhMuc = Convert.ToString(ViewData["LoaiDanhMuc"]);
    String ParentID = "Index";
    SqlCommand cmd;

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomLoaiVatTu') ORDER BY sTenKhoa");
    DataTable dt = Connection.GetDataTable(cmd);
    DataRow R = dt.NewRow();
    //R[1] = "---All---";
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomLoaiVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    //R[1] = "---All---";
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomChinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    //R[1] = "---All---";
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomPhu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    //String iID_MaDanhMucCha = Convert.ToString(Request.QueryString["iID_MaDanhMucCha"]);
    String iDM_MaNhomLoaiVatTu = Convert.ToString(ViewData["iDM_MaNhomLoaiVatTu"]);
    String iDM_MaNhomChinh = Convert.ToString(ViewData["iDM_MaNhomChinh"]);
    String iDM_MaNhomPhu = Convert.ToString(ViewData["iDM_MaNhomPhu"]);
    String sTen = Convert.ToString(ViewData["sTen"]); ;
    String DK = "";

    String TenDanhSach = "";
    cmd = new SqlCommand("SELECT sTen FROM DC_LoaiDanhMuc WHERE sTenBang = @sTenBang");
    cmd.Parameters.AddWithValue("@sTenBang", LoaiDanhMuc);
    TenDanhSach = Connection.GetValueString(cmd, "");
    cmd.Dispose();

    String iID_MaDanhMucCha = "";
    switch (LoaiDanhMuc)
    {
        case "NhomChinh":
            iID_MaDanhMucCha = iDM_MaNhomLoaiVatTu;
            break;

        case "NhomPhu":
            iID_MaDanhMucCha = iDM_MaNhomChinh;
            break;

        case "ChiTietVatTu":
            iID_MaDanhMucCha = iDM_MaNhomPhu;
            break;
    }
    if (!String.IsNullOrEmpty(iID_MaDanhMucCha))
        DK += " AND iID_MaDanhMucCha = '" + iID_MaDanhMucCha + "'";
    if (!String.IsNullOrEmpty(sTen))
        DK += " AND sTen LIKE '%" + sTen + "%'";
    Bang bang = new Bang("DC_DanhMuc");
    int CurrentPage = 1;
    if (ViewData["DanhMuc_page"] != null) CurrentPage = (int)ViewData["DanhMuc_page"];
    String SQL = "SELECT * FROM DC_DanhMuc WHERE iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                                             "FROM DC_LoaiDanhMuc " +
                                                                                             "WHERE sTenBang = '" + LoaiDanhMuc + "')" + DK;
    bang.TruyVanLayDanhSach.CommandText = SQL;
    if (LoaiDanhMuc == "DonViTinh")
        dt = bang.dtData("sTen", CurrentPage);
    else
        dt = bang.dtData("sTenKhoa", CurrentPage);
    
    int TotalRecords = bang.TongSoBanGhi();
    int TotalPages = (int)(Math.Ceiling((double)TotalRecords / Globals.PageSize));
    int FromRecord = (CurrentPage - 1) * Globals.PageSize + 1;
    int ToRecord = CurrentPage * Globals.PageSize;
    if (TotalPages == CurrentPage)
    {
        ToRecord = FromRecord + dt.Rows.Count - 1;
    }
    String sID_MaNguoiDung = User.Identity.Name;

    using (Html.BeginForm("Search", "SanPham_DanhMuc", new { ParentID = ParentID, LoaiDanhMuc = LoaiDanhMuc }))
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
<% if ((LoaiDanhMuc != "DonViTinh") && (LoaiDanhMuc != "NhomLoaiVatTu") && (LoaiDanhMuc != "XuatXu"))
{%>
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
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" align="left" style="width: 50%">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1"><div><b>Nhóm loại vật tư</b></div></td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DropDownList(ParentID, slNhomLoaiVatTu, iDM_MaNhomLoaiVatTu, "iDM_MaNhomLoaiVatTu", "", "onchange=\"ChonNhomLoaiVatTu(this.value)\" style=\"width: 100%;\"")%></div>
                                    <script type="text/javascript">
                                        function ChonNhomLoaiVatTu(iDM_MaNhomLoaiVatTu) {
                                            var url = unescape('<%= Url.Action("get_dtNhomChinh?ParentID=#0&iDM_MaNhomLoaiVatTu=#1&iDM_MaNhomChinh=#2", "SanPham_DanhMuc") %>');
                                            url = unescape(url.replace("#0", "<%= ParentID %>"));
                                            url = unescape(url.replace("#1", iDM_MaNhomLoaiVatTu));
                                            url = unescape(url.replace("#2", "<%= iDM_MaNhomChinh %>"));
                                            $.getJSON(url, function (data) {
                                                document.getElementById("<%= ParentID %>_tdNhomChinh").innerHTML = data.ddlNhomChinh;
                                                ChonNhomChinh(data.iDM_MaNhomChinh);
                                            });
                                        }
                                    </script>
                                </td>
                            </tr>
                        <% if (LoaiDanhMuc == "NhomPhu" || LoaiDanhMuc == "ChiTietVatTu")
                           {%>
                            <tr>
                                <td class="td_form2_td1"><div><b>Nhóm chính</b></div></td>
                                <td class="td_form2_td5">
                                    <div id="<%= ParentID %>_tdNhomChinh">
                                        <% SanPham_DanhMucController.NhomChinh _NhomChinh = SanPham_DanhMucController.get_objNhomChinh(ParentID, iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh);%>
                                        <%=_NhomChinh.ddlNhomChinh %>
                                    </div>
                                    <script type="text/javascript">
                                        function ChonNhomChinh(iDM_MaNhomChinh) {
                                            var url = unescape('<%= Url.Action("get_dtNhomPhu?ParentID=#0&iDM_MaNhomChinh=#1&iDM_MaNhomPhu=#2", "SanPham_DanhMuc") %>');
                                            url = unescape(url.replace("#0", "<%= ParentID %>"));
                                            url = unescape(url.replace("#1", iDM_MaNhomChinh));
                                            url = unescape(url.replace("#2", "<%= iDM_MaNhomPhu %>"));
                                            $.getJSON(url, function (data) {
                                                if (document.getElementById("<%= ParentID %>_tdNhomPhu") != null) {
                                                document.getElementById("<%= ParentID %>_tdNhomPhu").innerHTML = data;
                                            }
                                            });
                                        }
                                    </script>
                                </td>
                            </tr>
                            <%} %>
                            <% if (LoaiDanhMuc == "ChiTietVatTu")
                            {%>
                            <tr>
                                <td class="td_form2_td1"><div><b>Nhóm phụ</b></div></td>
                                <td class="td_form2_td5">
                                    <div id="<%= ParentID %>_tdNhomPhu"> 
                                        <%= SanPham_DanhMucController.get_objNhomPhu(ParentID, iDM_MaNhomChinh, iDM_MaNhomPhu)%>
                                    </div>
                                </td>
                            </tr>
                            <%} %>
                            <script type="text/javascript">
                                //MaChiTietVatTuGoiY()
                                function MaChiTietVatTuGoiY() {
                                    jQuery.ajaxSetup({ cache: false });
                                } 
                            </script>
                            <tr>
                                <td class="td_form2_td1"><div><b>Tên nhóm</b></div></td>
                                <td class="td_form2_td5">
                                    <div> 
                                        <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "style=\"width:99%\"")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1" align="right"><div>&nbsp;</div></td>
            	                <td class="td_form2_td5" align="right">
                                    <div style="text-align:right; float:right;">
            	                        <input type="submit" class="button4" value="Lọc" />
            	                    </div>
            	                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left" style="width: 50%" class="td_form2_td1">&nbsp;</td>
                </tr>
            </table>
        </div>
    </div>
</div>
<br /> 
<%} %>
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 50%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b><%=NgonNgu.LayXau("Danh sách " +TenDanhSach)%></b>
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
                                <%
                                if (DuyeVatTuAjaxController.LayMaDonViDung(sID_MaNguoiDung) == "-1")
                                {%>
                                <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "SanPham_DanhMuc", new { LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu }), "Thêm mới", "", "", "class=\"level1-a\"")%>
                                <%} %>
                            </li>
                            <li class="level1-li sub">
                                <%=MyHtmlHelper.ActionLink(Url.Action("ExportExcel", "SanPham_DanhMuc", new { ParentID = ParentID, LoaiDanhMuc = LoaiDanhMuc, SQL = SQL }), "Xuất Excel", "", "", "class=\"level1-a\"")%>
                            </li>
                        </ul>
                    </div>
                </div>
            </td>
            <td style="width: 23%">
                <div class="msdn" style="padding-top: 5px;">
                    <%
                    switch(LoaiDanhMuc)
                    {
                        case "NhomLoaiVatTu":
                        %>
                            <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc}))%>
                        <%
                        break; 
                        case "NhomChinh":
                        %>
                            <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, sTen = sTen }))%>
                        <%
                        break;
                        case "NhomPhu":
                        %>
                            <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, sTen = sTen }))%>
                        <%
                        break;
                        case "ChiTietVatTu":
                        %>
                            <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu, sTen = sTen }))%>
                        <%
                        break;
                        case "XuatXu":
                        %>
                            <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc}))%>
                        <%
                        break;
                        case "DonViTinh":
                        %>
                            <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc}))%>
                        <%
                        break;
                    }        
                    %>
                </div>
            </td>
        </tr>
    </table>
</div>
<%if (TotalRecords > 0)
  { %>
    <table class="mGrid">
    <tr>
        <th width="3%" align="center">STT</th>
        <% if (LoaiDanhMuc == "NhomChinh" || LoaiDanhMuc == "NhomPhu")
           {%>
            <th width="5%" align="center">Mã Nhóm loại vật tư</th>
        <%} %>
        <% if (LoaiDanhMuc == "NhomPhu")
           {%>
            <th width="5%" align="center">Mã Nhóm chính</th>
        <%} %>
        <% if (LoaiDanhMuc != "DonViTinh")
        {
            String strCotMaChon = "";
            switch (LoaiDanhMuc)
            {
                case "NhomChinh":
                    strCotMaChon = "<th width=\"5\" align=\"center\">Mã nhóm chính</th>";
                    break;
                case "NhomPhu":
                    strCotMaChon = "<th width=\"5%\" align=\"center\">Mã nhóm phụ</th>";
                    break;
                default:
                    strCotMaChon = "<th width=\"5%\" align=\"center\">Mã</th>";
                    break;
            }  
        %>    
        <%=strCotMaChon %>     
        <%} %>
        <th width="30%" align="center">Tên</th>
        <th align="center">Ghi chú</th>
        <th width="10%" align="center">Hoạt động</th>
        <th width="5%" align="center">&nbsp;</th>
    </tr>
        <%
        int i;
        DataRow R1;
        for (i = 0; i < dt.Rows.Count; i++)
        {
            R1 = dt.Rows[i];
            String MaNhomLoaiVatTu = "";
            String MaNhomPhuVatTu = "";
            if (LoaiDanhMuc == "NhomChinh")
            {
                cmd = new SqlCommand("SELECT sTenKhoa FROM DC_DanhMuc WHERE iID_MaDanhMuc = @iID_MaDanhMucCha AND iID_MaLoaiDanhMuc =  (SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang = 'NhomLoaiVatTu')");
                cmd.Parameters.AddWithValue("@iID_MaDanhMucCha", R1["iID_MaDanhMucCha"].ToString());
                MaNhomLoaiVatTu = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }

            if (LoaiDanhMuc == "NhomPhu")
            {
                DataTable dt1;
                cmd = new SqlCommand("SELECT * FROM DC_DanhMuc WHERE iID_MaDanhMuc = @iID_MaDanhMuc AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang = 'NhomChinh')");
                cmd.Parameters.AddWithValue("@iID_MaDanhMuc", R1["iID_MaDanhMucCha"].ToString());
                //MaNhomPhuVatTu = Connection.GetValueString(cmd, "");
                dt1 = Connection.GetDataTable(cmd);
                cmd.Dispose();
                MaNhomPhuVatTu = Convert.ToString(dt1.Rows[0]["sTenKhoa"]);

                cmd = new SqlCommand("SELECT sTenKhoa FROM DC_DanhMuc WHERE iID_MaDanhMuc = @iID_MaDanhMuc AND iID_MaLoaiDanhMuc =  (SELECT iID_MaLoaiDanhMuc FROM DC_LoaiDanhMuc WHERE sTenBang = 'NhomLoaiVatTu')");
                cmd.Parameters.AddWithValue("@iID_MaDanhMuc", dt1.Rows[0]["iID_MaDanhMucCha"].ToString());
                MaNhomLoaiVatTu = Connection.GetValueString(cmd, "");
                cmd.Dispose();
            }
            String strCalss = "";
            if (i % 2 == 0) strCalss = "alt";
        %>
        <tr class="<%=strCalss%>">
            <td align="center" style="padding: 3px 3px;"><%=i + 1%></td>
            <% if (LoaiDanhMuc == "NhomChinh" || LoaiDanhMuc == "NhomPhu")
            {%>
            <td  style="padding: 3px 3px;" align="center"><b><%=MaNhomLoaiVatTu %></b></td>
            <%} %>
            <% if (LoaiDanhMuc == "NhomPhu")
            {%>
            <td align="center" style="padding: 3px 3px;"><b><%=MaNhomPhuVatTu%></b></td>
            <%} %>
            <% if (LoaiDanhMuc != "DonViTinh")
               {%>
            <td align="center" style="padding: 3px 3px;"><b><%= MyHtmlHelper.Label(ParentID, dt.Rows[i]["sTenKhoa"], "sTenKhoa", "", "")%></b></td>
            <%} %>
            <td style="padding: 3px 3px;">
                <%
            if (DuyeVatTuAjaxController.LayMaDonViDung(sID_MaNguoiDung) == "-1")
            {%>
                <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "SanPham_DanhMuc", new { MaDanhMuc = dt.Rows[i]["iID_MaDanhMuc"], LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu }), dt.Rows[i]["sTen"].ToString())%>
            <%}
            else
            { %>
                <%= MyHtmlHelper.Label(dt.Rows[i]["sTen"], "sTen")%>
            <%} %>
            </td>
            <td style="padding: 3px 3px;"><%= MyHtmlHelper.Label(dt.Rows[i]["sGhiChu"], "iID_MaDanhMuc")%></td>
            <td align="center" style="padding: 3px 3px;"><%=MyHtmlHelper.LabelCheckBox(ParentID, dt.Rows[i]["bHoatDong"], "bHoatDong")%></td>
            <td style="padding: 3px 3px;">
                <%
                    if (DuyeVatTuAjaxController.LayMaDonViDung(sID_MaNguoiDung) == "-1")
                {%>
                <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "SanPham_DanhMuc", new { MaDanhMuc = dt.Rows[i]["iID_MaDanhMuc"], LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu }), "Sửa")%>
                <%} %>
                <%--|
                <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "SanPham_DanhMuc", new { MaDanhMuc = dt.Rows[i]["iID_MaDanhMuc"], LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu, MaChiTietVatTuDaXoa = dt.Rows[i]["sTenKhoa"]}), "Xóa", "Delete", "")%>--%>
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
                    <b>Danh sách từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> <%=TenDanhSach%></b>
                </td>
                <td>
                    <div class="msdn" style="padding-top: 5px;">
                        <%
                        switch(LoaiDanhMuc)
                        {
                            case "NhomLoaiVatTu":
                            %>
                                <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc}))%>
                            <%
                            break; 
                            case "NhomChinh":
                            %>
                                <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, sTen = sTen }))%>
                            <%
                            break;
                            case "NhomPhu":
                            %>
                                <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, sTen = sTen }))%>
                            <%
                            break;
                            case "ChiTietVatTu":
                            %>
                                <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc, iDM_MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh = iDM_MaNhomChinh, iDM_MaNhomPhu = iDM_MaNhomPhu, sTen = sTen }))%>
                            <%
                            break;
                            case "XuatXu":
                            %>
                                <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc}))%>
                            <%
                            break;
                            case "DonViTinh":
                            %>
                                <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { DanhMuc_page = x, LoaiDanhMuc = LoaiDanhMuc}))%>
                            <%
                            break;
                        }        
                        %>
                    </div>
                </td>
            </tr>
        </table>
    </div>
<%
  }
}
%>
</asp:Content>
