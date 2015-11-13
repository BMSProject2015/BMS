<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="System.Reflection" %>
<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {

    }
</script>
<%
    String ParentID = "Index";
    SqlCommand cmd;

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

    String sID_MaNguoiDung = User.Identity.Name;
    String IPSua = Request.UserHostAddress;

    cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                           "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                        "FROM QT_NguoiDung " +
                                                        "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
    cmd.Parameters.AddWithValue("@sID_MaNguoiDung", sID_MaNguoiDung);
    String iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
    cmd.Dispose();

    String Searchid = Request.QueryString["Searchid"];
    String sMaVatTu = Request.QueryString["sMaVatTu"];
    String sTen = Request.QueryString["sTen"];
    String sTenGoc = Request.QueryString["sTenGoc"];
    String sQuyCach = Request.QueryString["sQuyCach"];
    String cbsMaVatTu = Request.QueryString["cbsMaVatTu"];
    String cbsTen = Request.QueryString["cbsTen"];
    String cbsTenGoc = Request.QueryString["cbsTenGoc"];
    String cbsQuyCach = Request.QueryString["cbsQuyCach"];

    String iDM_MaNhomLoaiVatTu = Request.QueryString["MaNhomLoaiVatTu"]; ;
    String iDM_MaNhomChinh = Request.QueryString["MaNhomChinh"];
    String iDM_MaNhomPhu = Request.QueryString["MaNhomPhu"];
    String iDM_MaChiTietVatTu = Request.QueryString["MaChiTietVatTu"];
    String iDM_MaXuatXu = Request.QueryString["MaXuatXu"];
    String iTrangThai = Request.QueryString["iTrangThai"];

    DataTable dtDonVi_TonKho = null;
    String SQL = "";

    if (Searchid == "1")
    {
        SQL += " AND sMaVatTu <> '' ";
        if (!String.IsNullOrEmpty(sMaVatTu))
            if (String.IsNullOrEmpty(cbsMaVatTu))
                if (sMaVatTu.IndexOf("%") >= 0)
                    SQL += " AND sMaVatTu like '" + sMaVatTu + "'";
                else
                    SQL += " AND sMaVatTu = '" + sMaVatTu + "'";
            else
                SQL += " AND sMaVatTu like '%" + sMaVatTu + "%'";
        if (!String.IsNullOrEmpty(sTen))
            if (String.IsNullOrEmpty(cbsTen))
                if (sTen.IndexOf("%") >= 0)
                    SQL += " AND sTuKhoa_sTen like N'" + sTen + "'";
                else
                    SQL += " AND sTen = N'" + sTen + "'";
            else
                SQL += " AND sTuKhoa_sTen like N'%" + sTen + "%'";
        if (!String.IsNullOrEmpty(sTenGoc))
            if (String.IsNullOrEmpty(cbsTenGoc))
                if (sTenGoc.IndexOf("%") >= 0)
                    SQL += " AND sTuKhoa_sTenGoc like N'" + sTenGoc + "'";
                else
                    SQL += " AND sTenGoc = N'" + sTenGoc + "'";
            else
                SQL += " AND sTuKhoa_sTenGoc like N'%" + sTenGoc + "%'";
        if (!String.IsNullOrEmpty(sQuyCach))
            if (String.IsNullOrEmpty(cbsQuyCach))
                if (sQuyCach.IndexOf("%") >= 0)
                    SQL += " AND sTuKhoa_sQuyCach like N'" + sQuyCach + "'";
                else
                    SQL += " AND sQuyCach = N'" + sQuyCach + "'";
            else
                SQL += " AND sTuKhoa_sQuyCach like N'%" + sQuyCach + "%'";
        if (SQL != "")
        {
            if (iID_MaDonViDangNhap == "-1")
            {
                SQL = SQL.Substring(4, SQL.Length - 4);
                SQL = "SELECT * FROM DM_VatTu WHERE " + SQL;
            }
            else
            {
                SQL = SQL.Substring(4, SQL.Length - 4);
                SQL = "SELECT iID_MaVatTu, sMaVatTu, sTen, iDM_MaDonViTinh FROM DM_VatTu " +
                      "WHERE iID_MaVatTu IN (SELECT iID_MaVatTu FROM DM_DonVi_TonKho WHERE iID_MaDonVi = " + iID_MaDonViDangNhap + ")" + SQL;

                cmd = new SqlCommand("SELECT iID_MaVatTu, rSoLuongTonKho, dNgaySua FROM DM_DonVi_TonKho WHERE iID_MaDonVi = " + iID_MaDonViDangNhap + "");
                dtDonVi_TonKho = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
        }
        else
        {
            if (iID_MaDonViDangNhap == "-1")
            {
                SQL = "SELECT * FROM DM_VatTu";
            }
            else
            {
                SQL = "SELECT * FROM DM_VatTu " +
                      "WHERE iID_MaVatTu IN (SELECT iID_MaVatTu FROM DM_DonVi_TonKho WHERE iID_MaDonVi = " + iID_MaDonViDangNhap + ")";

                cmd = new SqlCommand("SELECT iID_MaVatTu, rSoLuongTonKho, dNgaySua FROM DM_DonVi_TonKho WHERE iID_MaDonVi = " + iID_MaDonViDangNhap + "");
                dtDonVi_TonKho = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
        }
    }
    else
    {
        SQL += " AND sMaVatTu <> '' ";
        if (!String.IsNullOrEmpty(iDM_MaNhomLoaiVatTu) && iDM_MaNhomLoaiVatTu != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            SQL += " AND iDM_MaNhomLoaiVatTu = '" + iDM_MaNhomLoaiVatTu + "'";
        if (!String.IsNullOrEmpty(iDM_MaNhomChinh) && iDM_MaNhomChinh != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            SQL += " AND iDM_MaNhomChinh = '" + iDM_MaNhomChinh + "'";
        if (!String.IsNullOrEmpty(iDM_MaNhomPhu) && iDM_MaNhomPhu != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            SQL += " AND iDM_MaNhomPhu = '" + iDM_MaNhomPhu + "'";
        if (!String.IsNullOrEmpty(iDM_MaChiTietVatTu) && iDM_MaChiTietVatTu != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            SQL += " AND iDM_MaChiTietVatTu = '" + iDM_MaChiTietVatTu + "'";
        if (!String.IsNullOrEmpty(iDM_MaXuatXu) && iDM_MaXuatXu != "dddddddd-dddd-dddd-dddd-dddddddddddd")
            SQL += " AND iDM_MaXuatXu = '" + iDM_MaXuatXu + "'";
        if (!String.IsNullOrEmpty(iTrangThai) && iTrangThai != "-1")
            SQL += " AND iTrangThai = " + iTrangThai + "";
        if (SQL != "")
        {
            if (iID_MaDonViDangNhap == "-1")
            {
                SQL = SQL.Substring(4, SQL.Length - 4);
                SQL = "SELECT * FROM DM_VatTu WHERE " + SQL;
            }
            else
            {
                SQL = SQL.Substring(4, SQL.Length - 4);
                SQL = "SELECT * FROM DM_VatTu " +
                      "WHERE iID_MaVatTu IN (SELECT iID_MaVatTu FROM DM_DonVi_TonKho WHERE iID_MaDonVi = " + iID_MaDonViDangNhap + ")" + SQL;

                cmd = new SqlCommand("SELECT iID_MaVatTu, rSoLuongTonKho, dNgaySua FROM DM_DonVi_TonKho WHERE iID_MaDonVi = " + iID_MaDonViDangNhap + "");
                dtDonVi_TonKho = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
        }
        else
        {
            if (iID_MaDonViDangNhap == "-1")
            {
                SQL = "SELECT * FROM DM_VatTu";
            }
            else
            {
                SQL = "SELECT * FROM DM_VatTu " +
                      "WHERE iID_MaVatTu IN (SELECT iID_MaVatTu FROM DM_DonVi_TonKho WHERE iID_MaDonVi = " + iID_MaDonViDangNhap + ")";

                cmd = new SqlCommand("SELECT iID_MaVatTu, rSoLuongTonKho, dNgaySua FROM DM_DonVi_TonKho WHERE iID_MaDonVi = " + iID_MaDonViDangNhap + "");
                dtDonVi_TonKho = Connection.GetDataTable(cmd);
                cmd.Dispose();
            }
        }
    }
    
    Bang bang = new Bang("DM_VatTu");
    int CurrentPage = 1;
    if (Request.QueryString["TonKhoVatTu_page"] != null)
        CurrentPage = Convert.ToInt32(Request.QueryString["TonKhoVatTu_page"]);
    
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
    if (TotalRecords > 0)
    {%>
    <div class="pagedingchuan">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td style="padding-left: 20px; padding-top: 3px; color: #ec3237; text-transform:uppercase;">
                    <b>Danh sách từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> vật tư</b>
                </td>
                <td>
                    <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinksAjax(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, (x, y) => Ajax.ActionLink(y, "Index", "Ajax", new { PartialView = "~/Views/SanPham/TonKhoVatTu/Detail.aspx", OnLoad = ParentID + "_OnLoad", TonKhoVatTu_page = x }, new AjaxOptions { }).ToString())%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <table cellpadding="0" cellspacing="0" border="0" class="table_form3" >
        <tr class="tr_form3">
            <td width="3%" align="center"><b>STT</b></td>
            <td width="10%" align="center"><b>Mã yêu cầu</b></td>
            <td width="10%" align="center"><b>Mã vật tư</b></td>
            <td width="40%" align="center"><b>Tên</b></td>
            <td width="15%" align="center"><b>Số lượng tồn kho</b></td>
            <td width="22%" align="center"><b>Ngày cập nhật tồn kho</b></td>
        </tr>
        <%
        int i, j;
        if (dt != null)
        {
        DateTime dNgayCapNhatTonKho = DateTime.Now;
        String DonViTinh = "";
        String SoLuongTonKho = "0";
        String sMaYeuCau = "";
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DonViTinh = "";
            SoLuongTonKho = "0";
            sMaVatTu = Convert.ToString(dt.Rows[i]["sMaVatTu"]);
            if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["sMaYeuCau"])))
            {
                sMaYeuCau = Convert.ToString(dt.Rows[i]["sMaYeuCau"]);
            }
            sTen = Convert.ToString(dt.Rows[i]["sTen"]);
            if (iID_MaDonViDangNhap == "-1")
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
                            dNgayCapNhatTonKho = Convert.ToDateTime(dtDonVi_TonKho.Rows[j]["dNgaySua"]);
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
                <td>
                    <%
                    if (!String.IsNullOrEmpty(sMaYeuCau))
                    {
                    %>
                    <div onclick="OnInit_TK();">      
                    <b><%= Ajax.ActionLink(sMaYeuCau, "Index", "NhapNhanh", new { id = "TONKHOVATTU", OnLoad = "OnLoad_TK", OnSuccess = "CallSuccess_TK", sMaVatTu = sMaVatTu, idDiv = "divSoLuongTonKho" + sMaVatTu, idDivDate = "divNgayNhapTonKho" + sMaVatTu }, new AjaxOptions { })%></b>
                    </div>
                    <%} %>
                </td>
                <td>
                    <%
                    if (!String.IsNullOrEmpty(sMaVatTu))
                    {
                    %>
                    <div onclick="OnInit_TK();">      
                    <b><%= Ajax.ActionLink(sMaVatTu, "Index", "NhapNhanh", new { id = "TONKHOVATTU", OnLoad = "OnLoad_TK", OnSuccess = "CallSuccess_TK", sMaVatTu = sMaVatTu, idDiv = "divSoLuongTonKho" + sMaVatTu, idDivDate = "divNgayNhapTonKho" + sMaVatTu }, new AjaxOptions { })%></b>
                    </div>
                    <%} %>
                </td>
                <td><%= MyHtmlHelper.Label(sTen, "sTen")%></td>
                <td>
                    <div id="divSoLuongTonKho<%= sMaVatTu%>" style="float: left; display: inline;">
                        <%= SoLuongTonKho%>
                    </div>&nbsp;
                    <%=DonViTinh%>
                 </td>
                <td>
                    <div id="divNgayNhapTonKho<%= sMaVatTu%>" style="float: left; display: inline;">
                        <%if (iID_MaDonViDangNhap == "-1"){
                            if (!(dt.Rows[i]["dNgayCapNhatTonKho"] is DBNull))
                          { %>
                        <%= MyHtmlHelper.Label(String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dNgayCapNhatTonKho), "dNgayCapNhatTonKho")%>
                        <%}else{ %>&nbsp;<%} }
                          else{%>
                          <%= MyHtmlHelper.Label(String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dNgayCapNhatTonKho), "dNgayCapNhatTonKho")%>
                          <%}%>
                    </div>
                </td>
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
                    <%= MyHtmlHelper.PageLinksAjax(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, (x, y) => Ajax.ActionLink(y, "Index", "Ajax", new { PartialView = "~/Views/SanPham/TonKhoVatTu/Detail.aspx", OnLoad = ParentID + "_OnLoad", TonKhoVatTu_page = x }, new AjaxOptions { }).ToString())%>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function CallSuccess_TK(rSoLuongTonKho, MaDiv) {
            var strSoluongDate = rSoLuongTonKho.split("#;");
            var strMaDiv = MaDiv.split("#;");

            document.getElementById(strMaDiv[0]).innerHTML = strSoluongDate[0];
            document.getElementById(strMaDiv[1]).innerHTML = strSoluongDate[1];
            return false;
        }
        function OnInit_TK() {
            $("#idDialog").dialog("destroy");
            document.getElementById("idDialog").title = '<%=NgonNgu.LayXauChuHoa("Cập nhật tồn kho cho mã vật tư")%>';
            document.getElementById("idDialog").innerHTML = "";
            $("#idDialog").dialog({
                resizeable: false,
                height: 280,
                width: 600,
                modal: true
            });
        }
        function OnLoad_TK(v) {
            document.getElementById("idDialog").innerHTML = v;
        }                                    
    </script>
    <div id="idDialog" style="display: none;"></div>
    <%} 
    else{
    %>
    <div class="pagedingchuan">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td style="padding-left: 20px; padding-top: 3px; color: #ec3237; text-transform:uppercase;">
                    <b>Danh sách từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> vật tư</b>
                </td>
            </tr>
        </table>
    </div>
    <%} %>
    



