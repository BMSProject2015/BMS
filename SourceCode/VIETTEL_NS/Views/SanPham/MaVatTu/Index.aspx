<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    SqlCommand cmd;
    String ParentID = "Index";

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomLoaiVatTu') ORDER BY sTenKhoa");
    DataTable dt = Connection.GetDataTable(cmd);
    DataRow R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomLoaiVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomChinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomPhu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slChiTietVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'XuatXu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slXuatXu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_DMTrangThai, sTen FROM DM_TrangThai ORDER BY iSTT");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slTrangThai = new SelectOptionList(dt, "iID_DMTrangThai", "sTen");
    cmd.Dispose();    
   
    cmd = new SqlCommand("SELECT * FROM NS_DonVi ORDER BY iSTT");
    DataTable dtDonVi = Connection.GetDataTable(cmd);
    cmd.Dispose();

    String sID_MaNguoiDung = User.Identity.Name;
    cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                           "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                        "FROM QT_NguoiDung " +
                                                        "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
    cmd.Parameters.AddWithValue("@sID_MaNguoiDung", sID_MaNguoiDung);
    String iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
    cmd.Dispose();

    String iDM_MaNhomLoaiVatTu = Convert.ToString(ViewData["MaNhomLoaiVatTu"]); ;
    String iDM_MaNhomChinh = Convert.ToString(ViewData["MaNhomChinh"]);
    String iDM_MaNhomPhu = Convert.ToString(ViewData["MaNhomPhu"]);
    String iDM_MaChiTietVatTu = Convert.ToString(ViewData["MaChiTietVatTu"]);
    String iDM_MaXuatXu = Convert.ToString(ViewData["MaXuatXu"]);
    String iTrangThai = Convert.ToString(ViewData["iTrangThai"]);
    String DK = "";

    if (!String.IsNullOrEmpty(iDM_MaNhomLoaiVatTu))
        DK += " AND iDM_MaNhomLoaiVatTu = '" + iDM_MaNhomLoaiVatTu + "'";
    if (!String.IsNullOrEmpty(iDM_MaNhomChinh))
        DK += " AND iDM_MaNhomChinh = '" + iDM_MaNhomChinh + "'";
    if (!String.IsNullOrEmpty(iDM_MaNhomPhu))
        DK += " AND iDM_MaNhomPhu = '" + iDM_MaNhomPhu + "'";
    if (!String.IsNullOrEmpty(iDM_MaChiTietVatTu))
        DK += " AND iDM_MaChiTietVatTu = '" + iDM_MaChiTietVatTu + "'";
    if (!String.IsNullOrEmpty(iDM_MaXuatXu))
        DK += " AND iDM_MaXuatXu = '" + iDM_MaXuatXu + "'";
    if (String.IsNullOrEmpty(iTrangThai))
        iTrangThai = "1";

    //cmd = new SqlCommand("SELECT iID_MaVatTu, sMaVatTu, sTen, sQuyCach, sTenGoc, iID_MaDonVi FROM DM_VatTu WHERE iTrangThai = " + iTrangThai + DK);
    //dt = Connection.GetDataTable(cmd);
    //cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                     "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                 "FROM DC_LoaiDanhMuc " +
                                                                 "WHERE sTenBang = 'DonViTinh') ORDER BY sTenKhoa");
    DataTable dtDonViTinh = Connection.GetDataTable(cmd);
    cmd.Dispose();

    String DKEXCEL = "iTrangThai = " + iTrangThai + DK;

    Bang bang = new Bang("DM_VatTu");
    int CurrentPage = 1;
    if (ViewData["MaVatTu_page"] != null) CurrentPage = (int)ViewData["MaVatTu_page"];
    String SQL = "SELECT * FROM DM_VatTu WHERE iTrangThai = " + iTrangThai + DK;
    bang.TruyVanLayDanhSach.CommandText = SQL;
    dt = bang.dtData("sMaVatTu", CurrentPage);
    int TotalRecords = bang.TongSoBanGhi();
    int TotalPages = (int)(Math.Ceiling((double)TotalRecords / Globals.PageSize));
    int FromRecord = (CurrentPage - 1) * Globals.PageSize + 1;
    int ToRecord = CurrentPage * Globals.PageSize;
    if (TotalPages == CurrentPage)
    {
        ToRecord = FromRecord + dt.Rows.Count - 1;
    }
    using (Html.BeginForm("Search", "MaVatTu", new { ParentID = "Index" }))
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
    <div id="dvContent" class="Content" style="display:block">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" align="left" style="width: 35%;">
                    <div id="nhapform">
                        <div id="form2">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1"><div><b>Nhóm loại vật tư</b></div></td>
                                    <td class="td_form2_td5">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, slNhomLoaiVatTu, iDM_MaNhomLoaiVatTu, "iDM_MaNhomLoaiVatTu", "", "onchange=\"ChonNhomLoaiVatTu(this.value)\" style=\"width: 100%;\"")%></div>
                                        <script type="text/javascript">
                                            function ChonNhomLoaiVatTu(iDM_MaNhomLoaiVatTu) {
                                                var url = unescape('<%= Url.Action("get_dtNhomChinh?ParentID=#0&iDM_MaNhomLoaiVatTu=#1&iDM_MaNhomChinh=#2", "DungChung") %>');
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
                                <tr>
                                    <td class="td_form2_td1"><div><b>Nhóm chính</b></div></td>
                                    <td class="td_form2_td5">
                                         <div id="<%= ParentID %>_tdNhomChinh">
                                            <% DungChungController.NhomChinh _NhomChinh = DungChungController.get_objNhomChinh(ParentID, iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh);%>
                                            <%=_NhomChinh.ddlNhomChinh %>
                                        </div>
                                        <script type="text/javascript">
                                            function ChonNhomChinh(iDM_MaNhomChinh) {
                                                var url = unescape('<%= Url.Action("get_dtNhomPhu?ParentID=#0&iDM_MaNhomChinh=#1&iDM_MaNhomPhu=#2", "DungChung") %>');
                                                url = unescape(url.replace("#0", "<%= ParentID %>"));
                                                url = unescape(url.replace("#1", iDM_MaNhomChinh));
                                                url = unescape(url.replace("#2", "<%= iDM_MaNhomPhu %>"));
                                                $.getJSON(url, function (data) {
                                                    document.getElementById("<%= ParentID %>_tdNhomPhu").innerHTML = data.ddlNhomPhu;
                                                    ChonNhomPhu(data.iDM_MaNhomPhu);
                                                });
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Nhóm phụ</b></div></td>
                                    <td class="td_form2_td5">
                                        <div id="<%= ParentID %>_tdNhomPhu">
                                            <% DungChungController.NhomPhu _NhomPhu = DungChungController.get_objNhomPhu(ParentID, iDM_MaNhomChinh, iDM_MaNhomPhu);%>
                                            <%=_NhomPhu.ddlNhomPhu %>
                                        </div>
                                        <script type="text/javascript">
                                            function ChonNhomPhu(iDM_MaNhomPhu) {
                                                var url = unescape('<%= Url.Action("get_dtChiTietVatTu?ParentID=#0&iDM_MaNhomPhu=#1&iDM_MaChiTietVatTu=#2", "DungChung") %>');
                                                url = unescape(url.replace("#0", "<%= ParentID %>"));
                                                url = unescape(url.replace("#1", iDM_MaNhomPhu));
                                                url = unescape(url.replace("#2", "<%= iDM_MaChiTietVatTu %>"));
                                                $.getJSON(url, function (data) {
                                                    document.getElementById("<%= ParentID %>_tdChiTietVatTu").innerHTML = data;
                                                });
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <script type="text/javascript">
                                    function timeGepMaVatTu() {
                                        return false;
                                    }  
                                </script> 
                                <tr>
                                    <td class="td_form2_td1" align="right"><div>&nbsp;</div></td>
            	                     <td class="td_form2_td5">
            	                        <div style="text-align:right; float:right;"></div> 
            	                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <div id="nhapform">
                        <div id="form2">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1"><div><b>Chi tiết vật tư</b></div></td>
                                    <td class="td_form2_td5">
                                        <div id="<%= ParentID %>_tdChiTietVatTu"> 
                                            <%= DungChungController.get_objChiTietVatTu(ParentID, iDM_MaNhomPhu, iDM_MaChiTietVatTu)%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Tình trạng vật tư</b></div></td>
                                    <td class="td_form2_td5">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, slXuatXu, iDM_MaXuatXu, "iDM_MaXuatXu", "", "style=\"width: 100%;\"")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Trạng thái</b></div></td>
                                    <td class="td_form2_td5">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "style=\"width: 100%;\"")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" align="right"><div>&nbsp;</div></td>
            	                     <td class="td_form2_td5">
            	                        <div style="text-align:right; float:right;">
            	                            <input type="submit" class="button4" value="Lọc" />
            	                        </div> 
            	                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
                <td valign="top" align="left" style="width: 30%; background:#f0f9fe; background-repeat: repeat;"></td>
            </tr>
        </table>
    </div>
</div><br />
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 50%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b><%=NgonNgu.LayXau("Danh sách Vật tư")%></b>
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
                                <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "MaVatTu", new { MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, MaNhomChinh = iDM_MaNhomChinh, MaNhomPhu = iDM_MaNhomPhu, MaChiTietVatTu = iDM_MaChiTietVatTu, MaXuatXu = iDM_MaXuatXu }), "Thêm mới", "", "", "class=\"level1-a\"")%>
                            </li>
                            <li class="level1-li sub">
                                <%=MyHtmlHelper.ActionLink(Url.Action("ExportExcel", "MaVatTu", new { ParentID = ParentID, DK = DKEXCEL }), "Xuất Excel", "", "", "class=\"level1-a\"")%>
                            </li>
                        </ul>
                    </div>
                </div>
            </td>
            <td style="width: 23%">
                <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { MaVatTu_page = x, MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, MaNhomChinh = iDM_MaNhomChinh, MaNhomPhu = iDM_MaNhomPhu, MaChiTietVatTu = iDM_MaChiTietVatTu, MaXuatXu = iDM_MaXuatXu, iTrangThai = iTrangThai }))%>
                </div>
            </td>
        </tr>
    </table>
</div>
<%if (TotalRecords > 0)
  { %>
    <table cellpadding="0" cellspacing="0" border="0" class="table_form3" >
        <tr class="tr_form3">
            <td width="3%" align="center"><b>STT</b></td>
            <td width="8%" align="center"><b>Mã yêu cầu</b></td>
            <td width="8%" align="center"><b>Mã vật tư</b></td>
            <td width="15%" align="center"><b>Tên</b></td>
            <td width="7%" align="center"><b>Giá</b></td>
            <td width="7%" align="center"><b>Giá NSQP</b></td>
            <td width="10%" align="center"><b>Quy cách</b></td>
            <td width="10%" align="center"><b>Tên gốc</b></td>
            <td width="8%" align="center"><b>Đơn vị phát sinh</b></td>
            <td width="7%" align="center"><b>Đơn vị tính</b></td>
            <td width="10%" align="center"><b>&nbsp;</b></td>
        </tr>
    <%
    int i, j;
    String TenDonVi = "";
    String DonViTinh = "";
    String sGia = "", sGia_NS = "";
    String dTuNgay = "", dDenNgay = "";
    for (i = 0; i < dt.Rows.Count; i++)
    {
        TenDonVi = "";
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

        DonViTinh = "";
        for (j = 0; j < dtDonViTinh.Rows.Count; j++)
        {
            if (Convert.ToString(dt.Rows[i]["iDM_MaDonViTinh"]) == Convert.ToString(dtDonViTinh.Rows[j]["iID_MaDanhMuc"]))
            {
                DonViTinh = Convert.ToString(dtDonViTinh.Rows[j]["sTen"]);
                break;
            }
        }

        sGia = "";
        dTuNgay = "";
        dDenNgay = "";

        DataTable dtGia = SanPham_VatTuModels.Get_GiaVatTu_Row(Convert.ToString(dt.Rows[i]["iID_MaVatTu"]));
        if (dtGia.Rows.Count > 0) {
            sGia = CommonFunction.DinhDangSo(dtGia.Rows[0]["rGia"]);
            sGia_NS = CommonFunction.DinhDangSo(dtGia.Rows[0]["rGia_NS"]);
            dTuNgay = CommonFunction.LayXauNgay(Convert.ToDateTime(dtGia.Rows[0]["dTuNgay"]));
            //dDenNgay = CommonFunction.LayXauNgay(Convert.ToDateTime(dtGia.Rows[0]["dDenNgay"]));
        }
        
        if (i % 2 == 0)
        {%>
        <tr >
        <%}
        else
        {%>
        <tr style="background-color:#FFC">
        <%} %>
            <td><%=i+1 %></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sMaYeuCau"], "sMaYeuCau")%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sMaVatTu"], "sMaVatTu")%></td>
            <td>
                <%if ((Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) == iID_MaDonViDangNhap) || (iID_MaDonViDangNhap == "-1"))
                  {%>
                    <%= MyHtmlHelper.ActionLink(Url.Action("Edit", "MaVatTu", new { iID_MaVatTu = dt.Rows[i]["iID_MaVatTu"] }), dt.Rows[i]["sTen"].ToString())%>
                <%}
                  else
                  {%>
                    <%= MyHtmlHelper.Label(dt.Rows[i]["sTen"], "sTen")%>
                <%}%>
            </td>
            <td title="Tính từ ngày:<%=dTuNgay %>"><%=sGia %></td>
            <td title="Tính từ ngày:<%=dTuNgay %>"><%=sGia_NS %></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sQuyCach"], "sQuyCach")%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sTenGoc"], "sTenGoc")%></td>
            <td><%= MyHtmlHelper.Label(TenDonVi, "iID_MaDonVi") %></td>
            <td align="center"><%= MyHtmlHelper.Label(DonViTinh, "iDM_MaDonViTinh")%></td>
            <td>
                <%if ((Convert.ToString(dt.Rows[i]["iID_MaDonVi"]) == iID_MaDonViDangNhap) || (iID_MaDonViDangNhap == "-1")){%>
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "MaVatTu", new { iID_MaVatTu = dt.Rows[i]["iID_MaVatTu"] }), "Sửa") %>|
                    <%if (DuyeVatTuAjaxController.LayMaDonViDung(sID_MaNguoiDung) == "-1")
                    { %>
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "MaVatTu", new { ParentID = ParentID, iID_MaVatTu = dt.Rows[i]["iID_MaVatTu"], MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, MaNhomChinh = iDM_MaNhomChinh, MaNhomPhu = iDM_MaNhomPhu, MaChiTietVatTu = iDM_MaChiTietVatTu, MaXuatXu = iDM_MaXuatXu, iTrangThai = iTrangThai }), "Xóa", "Delete", "")%> |
                    <%} %>
                <%} %>
                <%= MyHtmlHelper.ActionLink(Url.Action("CapNhatNCC", "MaVatTu", new { iID_MaVatTu = dt.Rows[i]["iID_MaVatTu"], sMaVatTu = dt.Rows[i]["sMaVatTu"], sTen = dt.Rows[i]["sTen"] }), "Cập nhật NCC")%>
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
                    <b>Danh sách từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> vật tư</b>
                </td>
                <td>
                    <div class="msdn" style="padding-top: 5px;">
                        <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { MaVatTu_page = x, MaNhomLoaiVatTu = iDM_MaNhomLoaiVatTu, MaNhomChinh = iDM_MaNhomChinh, MaNhomPhu = iDM_MaNhomPhu, MaChiTietVatTu = iDM_MaChiTietVatTu, MaXuatXu = iDM_MaXuatXu, iTrangThai = iTrangThai }))%>
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
