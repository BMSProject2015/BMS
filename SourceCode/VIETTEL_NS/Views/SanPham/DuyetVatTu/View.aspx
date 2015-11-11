<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="System.Reflection" %>

<%
    SqlCommand cmd;
    String ParentID = "Edit";
    
    DataTable dt = new DataTable();
    DataRow R;

    cmd = new SqlCommand("SELECT * FROM NS_DonVi ORDER BY iSTT");
    DataTable dtDonVi = Connection.GetDataTable(cmd);
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                     "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                 "FROM DC_LoaiDanhMuc " +
                                                                 "WHERE sTenBang = 'DonViTinh') ORDER BY sTenKhoa");
    DataTable dtDonViTinh = Connection.GetDataTable(cmd);
    cmd.Dispose();

    String Searchid = Request.QueryString["Searchid"];
    String sMaVatTu_Search = Request.QueryString["sMaVatTu_Search"];
    String sTen_Search = Request.QueryString["sTen_Search"];
    String sTenGoc_Search = Request.QueryString["sTenGoc_Search"];
    String sQuyCach_Search = Request.QueryString["sQuyCach_Search"];
    String cbsMaVatTu_Search = Request.QueryString["cbsMaVatTu_Search"];
    String cbsTen_Search = Request.QueryString["cbsTen_Search"];
    String cbsTenGoc_Search = Request.QueryString["cbsTenGoc_Search"];
    String cbsQuyCach_Search = Request.QueryString["cbsQuyCach_Search"];

    String iDM_MaNhomLoaiVatTu_Search = Request.QueryString["MaNhomLoaiVatTu_Search"]; ;
    String iDM_MaNhomChinh_Search = Request.QueryString["MaNhomChinh_Search"];
    String iDM_MaNhomPhu_Search = Request.QueryString["MaNhomPhu_Search"];
    String iDM_MaChiTietVatTu_Search = Request.QueryString["MaChiTietVatTu_Search"];
    String iDM_MaXuatXu_Search = Request.QueryString["MaXuatXu_Search"];
    String iTrangThai_Search = Request.QueryString["iTrangThai_Search"];
    
    String SQL = "";

    if (Searchid == "1")
    {
        if (!String.IsNullOrEmpty(sMaVatTu_Search))
            if (String.IsNullOrEmpty(cbsMaVatTu_Search))
                if (sMaVatTu_Search.IndexOf("%") >= 0)
                    SQL += " AND sMaVatTu like '" + sMaVatTu_Search + "'";
                else
                    SQL += " AND sMaVatTu = '" + sMaVatTu_Search + "'";
            else
                SQL += " AND sMaVatTu like '%" + sMaVatTu_Search + "%'";
        if (!String.IsNullOrEmpty(sTen_Search))
            if (String.IsNullOrEmpty(cbsTen_Search))
                if (sTen_Search.IndexOf("%") >= 0)
                    SQL += " AND sTuKhoa_sTen like N'" + sTen_Search + "'";
                else
                    SQL += " AND sTen = N'" + sTen_Search + "'";
            else
                SQL += " AND sTuKhoa_sTen like N'%" + sTen_Search + "%'";
        if (!String.IsNullOrEmpty(sTenGoc_Search))
            if (String.IsNullOrEmpty(cbsTenGoc_Search))
                if (sTenGoc_Search.IndexOf("%") >= 0)
                    SQL += " AND sTuKhoa_sTenGoc like N'" + sTenGoc_Search + "'";
                else
                    SQL += " AND sTenGoc = N'" + sTenGoc_Search + "'";
            else
                SQL += " AND sTuKhoa_sTenGoc like N'%" + sTenGoc_Search + "%'";
        if (String.IsNullOrEmpty(sQuyCach_Search))
            if (!String.IsNullOrEmpty(cbsQuyCach_Search))
                if (sQuyCach_Search.IndexOf("%") >= 0)
                    SQL += " AND sTuKhoa_sQuyCach like N'" + sQuyCach_Search + "'";
                else
                    SQL += " AND sQuyCach = N'" + sQuyCach_Search + "'";
            else
                SQL += " AND sTuKhoa_sQuyCach like N'%" + sQuyCach_Search + "%'";
        if (SQL != "")
        {
            SQL = SQL.Substring(4, SQL.Length - 4);
            SQL = "SELECT * FROM DM_VatTu WHERE " + SQL + " AND iTrangThai = 1";
        }
    }
    else
    {
        if (!String.IsNullOrEmpty(iDM_MaNhomLoaiVatTu_Search) && iDM_MaNhomLoaiVatTu_Search != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            SQL += " AND iDM_MaNhomLoaiVatTu = '" + iDM_MaNhomLoaiVatTu_Search + "'";
        if (!String.IsNullOrEmpty(iDM_MaNhomChinh_Search) && iDM_MaNhomChinh_Search != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            SQL += " AND iDM_MaNhomChinh = '" + iDM_MaNhomChinh_Search + "'";
        if (!String.IsNullOrEmpty(iDM_MaNhomPhu_Search) && iDM_MaNhomPhu_Search != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            SQL += " AND iDM_MaNhomPhu = '" + iDM_MaNhomPhu_Search + "'";
        if (!String.IsNullOrEmpty(iDM_MaChiTietVatTu_Search) && iDM_MaChiTietVatTu_Search != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            SQL += " AND iDM_MaChiTietVatTu = '" + iDM_MaChiTietVatTu_Search + "'";
        if (!String.IsNullOrEmpty(iDM_MaXuatXu_Search) && iDM_MaXuatXu_Search != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            SQL += " AND iDM_MaXuatXu = '" + iDM_MaXuatXu_Search + "'";
        if (!String.IsNullOrEmpty(iTrangThai_Search) && iTrangThai_Search != "-1")
            SQL += " AND iTrangThai = " + iTrangThai_Search + "";  
        if (SQL != "")
        {
            SQL = SQL.Substring(4, SQL.Length - 4);
            SQL = "SELECT * FROM DM_VatTu WHERE " + SQL;
        }
    }
    
    int CurrentPage = 1;
    if (Request.QueryString["TimKiemVatTu_page"] != null)
        CurrentPage = Convert.ToInt32(Request.QueryString["TimKiemVatTu_page"]);
    int TotalPages = 0;
    int FromRecord = 0;
    int ToRecord = 0 ;
    int TotalRecords = 0;
    dt = null;
    if (SQL != "")
    {
        Bang bang = new Bang("DM_VatTu");
        bang.TruyVanLayDanhSach.CommandText = SQL;
        dt = bang.dtData("sTen ASC", CurrentPage, 10);
        TotalRecords = bang.TongSoBanGhi();
        TotalPages = (int)(Math.Ceiling((double)TotalRecords / 10));
        FromRecord = (CurrentPage - 1) * 10 + 1;
        ToRecord = CurrentPage * 10;
        if (TotalPages == CurrentPage)
        {
            ToRecord = FromRecord + dt.Rows.Count - 1;
        }
    }  
%>
<br />
<%if (TotalRecords > 0)
{ %>
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 50%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b><%=NgonNgu.LayXau("Kết quả tìm kiếm")%>&nbsp;(<%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> vật tư)</b>
                </div>         
            </td>
            <td style="width: 50%">
                <%if (Searchid == "1"){ %>
                <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinksAjax(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, (x, y) => Ajax.ActionLink(y, "Index", "Ajax", new
                                                                                                                                                        {
                                                                                                                                                            PartialView = "~/Views/SanPham/DuyetVatTu/View.aspx",
                                                                                                                                                            OnLoad = ParentID + "_OnLoad",
                                                                                                                                                            TimKiemVatTu_page = x,
                                                                                                                                                            sMaVatTu_Search = sMaVatTu_Search,
                                                                                                                                                            sTen_Search = sTen_Search,
                                                                                                                                                            sTenGoc_Search = sTenGoc_Search,
                                                                                                                                                            sQuyCach_Search = sQuyCach_Search,
                                                                                                                                                            cbsMaVatTu_Search = cbsMaVatTu_Search,
                                                                                                                                                            cbsTen_Search = cbsTen_Search,
                                                                                                                                                            cbsTenGoc_Search = cbsTenGoc_Search,
                                                                                                                                                            cbsQuyCach_Search = cbsQuyCach_Search,
                                                                                                                                                            Searchid = Searchid
                                                                                                                                                        }, new AjaxOptions { }).ToString())%>
                </div>
                <%}else{ %>
                <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinksAjax(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, (x, y) => Ajax.ActionLink(y, "Index", "Ajax", new
                                                                                                                                                    {
                                                                                                                                                        PartialView = "~/Views/SanPham/DuyetVatTu/View.aspx",
                                                                                                                                                        OnLoad = ParentID + "_OnLoad",
                                                                                                                                                        TimKiemVatTu_page = x,
                                                                                                                                                        MaNhomLoaiVatTu_Search = iDM_MaNhomLoaiVatTu_Search,
                                                                                                                                                        MaNhomChinh_Search = iDM_MaNhomChinh_Search,
                                                                                                                                                        MaNhomPhu_Search = iDM_MaNhomPhu_Search,
                                                                                                                                                        MaChiTietVatTu_Search = iDM_MaChiTietVatTu_Search,
                                                                                                                                                        MaXuatXu_Search = iDM_MaXuatXu_Search,
                                                                                                                                                        iTrangThai_Search = iTrangThai_Search,
                                                                                                                                                        Searchid = Searchid
                                                                                                                                                    }, new AjaxOptions { }).ToString())%>
                </div>
                <%} %>
            </td>
        </tr>
    </table>
</div>
<table cellpadding="0" cellspacing="0" border="0" class="table_form3" >
    <tr class="tr_form3">
        <td width="3%" align="center"><b>STT</b></td>
        <td width="8%" align="center"><b>Mã</b></td>
        <td width="20%" align="center"><b>Tên</b></td>
        <td width="15%" align="center"><b>Tên gốc</b></td>
        <td width="10%" align="center"><b>Quy cách</b></td>
        <td width="7%" align="center"><b>Đơn vị phát sinh</b></td>
        <td width="7%" align="center"><b>Đơn vị tính</b></td>
        <td width="11%" align="center"><b>Ngày phát sinh mã</b></td>
        <td width="6%" align="center"><b>Trạng thái</b></td>
        <td width="8%" align="center"><b>Số lượng tồn kho</b></td>
        <td width="8%" align="center"><b>Số lượng NCC</b></td>
    </tr>
    <%
    int i, j;
    if (dt != null)
    { 
        String TenDonVi = "";
        String SoLuongTonKho = "0";
        String DonViTinh = "";
        String SoLuongNCC = "0";
        String sMaVatTu = "";
        for (i = 0; i < dt.Rows.Count; i++)
        {
            sMaVatTu = Convert.ToString(dt.Rows[i]["sMaVatTu"]);
            TenDonVi = "";
            SoLuongTonKho = "0";
            DonViTinh = "";
            SoLuongNCC = "0";
            
            cmd = new SqlCommand("SELECT Count(iID_MaNhaCungCap) FROM DM_VatTu_NhaCungCap WHERE iID_MaVatTu = @iID_MaVatTu");
            cmd.Parameters.AddWithValue("@iID_MaVatTu", Convert.ToString(dt.Rows[i]["iID_MaVatTu"]));
            SoLuongNCC = Connection.GetValueString(cmd, "0");
            cmd.Dispose();
            
            if (CommonFunction.DinhDangSo(dt.Rows[i]["rSoLuongTonKho"]) != "")
                SoLuongTonKho = CommonFunction.DinhDangSo(dt.Rows[i]["rSoLuongTonKho"]);
            for (j = 0; j < dtDonViTinh.Rows.Count; j++)
            {
                if (Convert.ToString(dt.Rows[i]["iDM_MaDonViTinh"]) == Convert.ToString(dtDonViTinh.Rows[j]["iID_MaDanhMuc"]))
                {
                    DonViTinh = Convert.ToString(dtDonViTinh.Rows[j]["sTen"]);
                    break;
                }
                    
            }
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
            String TrangThai = Convert.ToString(dt.Rows[i]["iTrangThai"]);
            cmd = new SqlCommand("SELECT sTen FROM DM_TrangThai WHERE iID_DMTrangThai = @iID_DMTrangThai");
            cmd.Parameters.AddWithValue("@iID_DMTrangThai", TrangThai);
            TrangThai = Convert.ToString(Connection.GetValue(cmd, ""));
            cmd.Dispose();
            //string urlfilename = Url.Action("GetWordFile", "MauChung", new { FileName = dt.Row["sFileName"].ToString(), Path = Row["sDuongdan"].ToString() });
        if (i % 2 == 0)
        {%>
        <tr>
        <%}
        else
        {%>
        <tr style="background-color:#FFC">
        <%} %>
            <td><%=i+1 %></td>
            <td style="cursor: pointer;" onclick="Onclick_MaVatTu('<%= sMaVatTu%>');" title="Chọn mã để cấp mã cũ!"><b><%= sMaVatTu%></b></td>
            <td title="Chi Tiết Vật Tư"><%= MyHtmlHelper.ActionLink(Url.Action("Detail", "TimKiemVatTu", new { iID_MaVatTu = dt.Rows[i]["iID_MaVatTu"] }), dt.Rows[i]["sTen"])%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sTenGoc"], "sTenGoc")%></td>
            <td><%= MyHtmlHelper.Label(dt.Rows[i]["sQuyCach"], "sQuyCach")%></td>
            <td><%= MyHtmlHelper.Label(TenDonVi, "iID_MaDonVi") %></td>
            <td align="center"><%= MyHtmlHelper.Label(DonViTinh, "iDM_MaDonViTinh")%></td>
            <td><%= MyHtmlHelper.Label(String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dt.Rows[i]["dNgayPhatSinhMa"]), "dNgayPhatSinhMa")%></td>
            <td><%= MyHtmlHelper.Label(TrangThai, "iTrangThai","","",2)%></td>
            <td><%= MyHtmlHelper.ActionLink(Url.Action("XemTonKho", "TimKiemVatTu", new { iID_MaVatTu = dt.Rows[i]["iID_MaVatTu"], sMaVatTu = Convert.ToString(dt.Rows[i]["sMaVatTu"]),  SoLuongTonKho = SoLuongTonKho, DonViTinh = DonViTinh }), SoLuongTonKho + " " + DonViTinh)%></td>
            <td><%= MyHtmlHelper.ActionLink(Url.Action("XemNCC", "TimKiemVatTu", new { iID_MaVatTu = dt.Rows[i]["iID_MaVatTu"], sMaVatTu = dt.Rows[i]["sMaVatTu"], sTen = dt.Rows[i]["sTen"] }), SoLuongNCC)%></td>
        </tr>
    <%}
    dtDonVi.Dispose();
    dt.Dispose();
    }
    %>
    </table>
    <div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 50%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b><%=NgonNgu.LayXau("Kết quả tìm kiếm")%>&nbsp;(<%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> vật tư)</b>
                </div>         
            </td>
            <td style="width: 50%">
                <%if (Searchid == "1"){ %>
                <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinksAjax(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, (x, y) => Ajax.ActionLink(y, "Index", "Ajax", new
                                                                                                                                                        {
                                                                                                                                                            PartialView = "~/Views/SanPham/DuyetVatTu/View.aspx",
                                                                                                                                                            OnLoad = ParentID + "_OnLoad",
                                                                                                                                                            TimKiemVatTu_page = x,
                                                                                                                                                            sMaVatTu_Search = sMaVatTu_Search,
                                                                                                                                                            sTen_Search = sTen_Search,
                                                                                                                                                            sTenGoc_Search = sTenGoc_Search,
                                                                                                                                                            sQuyCach_Search = sQuyCach_Search,
                                                                                                                                                            cbsMaVatTu_Search = cbsMaVatTu_Search,
                                                                                                                                                            cbsTen_Search = cbsTen_Search,
                                                                                                                                                            cbsTenGoc_Search = cbsTenGoc_Search,
                                                                                                                                                            cbsQuyCach_Search = cbsQuyCach_Search,
                                                                                                                                                            Searchid = Searchid
                                                                                                                                                        }, new AjaxOptions { }).ToString())%>
                </div>
                <%}else{ %>
                <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinksAjax(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, (x, y) => Ajax.ActionLink(y, "Index", "Ajax", new
                                                                                                                                                    {
                                                                                                                                                        PartialView = "~/Views/SanPham/DuyetVatTu/View.aspx",
                                                                                                                                                        OnLoad = ParentID + "_OnLoad",
                                                                                                                                                        TimKiemVatTu_page = x,
                                                                                                                                                        MaNhomLoaiVatTu_Search = iDM_MaNhomLoaiVatTu_Search,
                                                                                                                                                        MaNhomChinh_Search = iDM_MaNhomChinh_Search,
                                                                                                                                                        MaNhomPhu_Search = iDM_MaNhomPhu_Search,
                                                                                                                                                        MaChiTietVatTu_Search = iDM_MaChiTietVatTu_Search,
                                                                                                                                                        MaXuatXu_Search = iDM_MaXuatXu_Search,
                                                                                                                                                        iTrangThai_Search = iTrangThai_Search,
                                                                                                                                                        Searchid = Searchid
                                                                                                                                                    }, new AjaxOptions { }).ToString())%>
                </div>
                <%} %>
            </td>
        </tr>
    </table>
</div>
<%}
   else{   
%>
<div class="title_tong">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td>
                <span><%=NgonNgu.LayXau("Không có kết quả tìm kiếm vật tư nào!")%></span>
            </td>
        </tr>
    </table>
</div>
<%} %>
<script type="text/javascript" language="javascript">
    function Onclick_MaVatTu(sMaVatTuHT) {
        var sCapMaCu = document.getElementById("Edit_sCapMaCu");
        sCapMaCu.value = sMaVatTuHT;
        return false;
    }
</script>