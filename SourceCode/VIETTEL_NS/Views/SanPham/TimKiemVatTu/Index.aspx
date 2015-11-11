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
    SqlCommand cmd;
    String ParentID = "Index";
    DataTable dt;
    DataRow R;

    cmd = new SqlCommand("SELECT iID_DMTrangThai, sTen FROM DM_TrangThai ORDER BY iSTT");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slTrangThai = new SelectOptionList(dt, "iID_DMTrangThai", "sTen");
    cmd.Dispose();    

    cmd = new SqlCommand("SELECT * FROM NS_DonVi ORDER BY iSTT");
    DataTable dtDonVi = Connection.GetDataTable(cmd);
    R = dtDonVi.NewRow();
    R["iID_MaDonVi"] = "-1";
    R["sTen"] = "BQP";
    dtDonVi.Rows.InsertAt(R,0);
    R = dtDonVi.NewRow();
    R["iID_MaDonVi"] = "0";
    R["sTen"] = "";
    dtDonVi.Rows.InsertAt(R, 0);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                     "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                 "FROM DC_LoaiDanhMuc " +
                                                                 "WHERE sTenBang = 'DonViTinh') ORDER BY sTenKhoa");
    DataTable dtDonViTinh = Connection.GetDataTable(cmd);
    cmd.Dispose();

    String sMaVatTu = Convert.ToString(ViewData["sMaVatTu"]);
    String sTen = Convert.ToString(ViewData["sTen"]);
    String sTenGoc = Convert.ToString(ViewData["sTenGoc"]);
    String sQuyCach = Convert.ToString(ViewData["sQuyCach"]);
    String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);
    String iTrangThai = Convert.ToString(ViewData["iTrangThai"]);
    String TuNgay = Convert.ToString(ViewData["TuNgay"]);
    String DenNgay = Convert.ToString(ViewData["DenNgay"]);
    String cbsMaVatTu = Convert.ToString(ViewData["cbsMaVatTu"]);
    String cbsTen = Convert.ToString(ViewData["cbsTen"]);
    String cbsTenGoc = Convert.ToString(ViewData["cbsTenGoc"]);
    String cbsQuyCach = Convert.ToString(ViewData["cbsQuyCach"]);
    String SQL = "";
    
    if (!String.IsNullOrEmpty(sMaVatTu))
        if (String.IsNullOrEmpty(cbsMaVatTu))
            if (sMaVatTu.IndexOf("%") >=0)
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
    if (!String.IsNullOrEmpty(iID_MaDonVi))
    {
        if (iID_MaDonVi == "-1") 
            SQL += " AND iID_MaDonVi IS NULL";
        else
            if (iID_MaDonVi != "0") SQL += " AND iID_MaDonVi = " + iID_MaDonVi;
    }
    if (!String.IsNullOrEmpty(iTrangThai))
        if (iTrangThai != "-1") SQL += " AND iTrangThai = " + iTrangThai;
    if (!String.IsNullOrEmpty(TuNgay))
        SQL += " AND dNgayPhatSinhMa >= '" + CommonFunction.LayNgayTuXau(TuNgay) + "'";
    if (!String.IsNullOrEmpty(DenNgay))
        SQL += " AND dNgayPhatSinhMa <= '" + CommonFunction.LayNgayTuXau(DenNgay) + "'";
    if (SQL != "")
    {
        SQL = SQL.Substring(4, SQL.Length - 4);
        SQL = "SELECT * FROM DM_VatTu WHERE " + SQL;
    }
    //cmd = new SqlCommand(SQL);
    //dt = Connection.GetDataTable(cmd);
    //cmd.Dispose();
    
    int CurrentPage = 1;
    int TotalPages = 0;
    int FromRecord = 0;
    int ToRecord = 0 ;
    int TotalRecords = 0;
    dt = null;
    if (SQL != "")
    {
        Bang bang = new Bang("DM_VatTu");
        if (ViewData["TimKiemVatTu_page"] != null) CurrentPage = (int)ViewData["TimKiemVatTu_page"];
        bang.TruyVanLayDanhSach.CommandText = SQL;
        dt = bang.dtData("sTen ASC", CurrentPage, Globals.PageSize);
        
        TotalRecords = bang.TongSoBanGhi();
        TotalPages = (int)(Math.Ceiling((double)TotalRecords / Globals.PageSize));
        FromRecord = (CurrentPage - 1) * Globals.PageSize + 1;
        ToRecord = CurrentPage * Globals.PageSize;
        if (TotalPages == CurrentPage)
        {
            ToRecord = FromRecord + dt.Rows.Count - 1;
        }
    }
    
    String HienThiTaoMoi = Convert.ToString( ViewData["HienThiTaoMoi"]);
    
    if(HienThiTaoMoi=="1")
        HienThiTaoMoi = "\"float:right;\"";
    else
        HienThiTaoMoi = "\"float:right; display:none\"";
        
    using (Html.BeginForm("Search", "TimKiemVatTu", new { ParentID = ParentID, HienThiTaoMoi = "1" }))
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
                	<span><%=NgonNgu.LayXau("Tìm kiếm Vật tư")%></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
        <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
            <tr>
                <td width="45%">
                    <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                       <tr>
                            <td class="td_form2_td1"><div><b>Mã vật tư</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sMaVatTu, "sMaVatTu", "", "style=\"width:92%\"")%>
                                    <%=MyHtmlHelper.CheckBox(ParentID, cbsMaVatTu, "cbsMaVatTu", "", "")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Tên vật tư</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "style=\"width:92%\"")%>
                                    <%=MyHtmlHelper.CheckBox(ParentID, cbsTen, "cbsTen", "", "")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Quy cách</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sQuyCach, "sQuyCach", "", "style=\"width:92%;\"")%>
                                    <%=MyHtmlHelper.CheckBox(ParentID, cbsQuyCach, "cbsQuyCach", "", "")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Tên gốc</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sTenGoc, "sTenGoc", "", "style=\"width:92%\"")%>
                                    <%=MyHtmlHelper.CheckBox(ParentID, cbsTenGoc, "cbsTenGoc", "", "")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" align="right">
                                <div style="text-align:right; float:right; width:100%">&nbsp;</div> 
            	            </td>
            	             <td class="td_form2_td5" style="padding:5px 0px; display: inline;">
            	                <div><span style="font-size: 9px;">*Tích vào ô check bên cạnh tiêu chí tìm kiếm hoặc thêm ký tự % để tìm tương đối</span></div>
            	            </td>
                        </tr>
                    </table>
                </td>
                <td width="45%">
                     <table cellpadding="0" cellspacing="0" border="0" class="table_form2">                        
                        <tr>
                            <td class="td_form2_td1"><div><b>Trạng thái</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Đơn vị phát sinh</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Từ ngày</b></div></td>
                            <td class="td_form2_td5">
                                <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, TuNgay, "TuNgay")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Đến ngày</b></div></td>
                            <td class="td_form2_td5">
                                <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, DenNgay, "DenNgay")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1" ><div>&nbsp;</div></td>
            	            <td class="td_form2_td5" align="right" style="display: inline;">
                                <div style="text-align:right; float:right; width:100%">
                                    <input type="submit" class="button4" value="Tìm" style="float:right; margin-left:10px;"/>
                                    <%String url = Url.Action("Edit", "TimKiemVatTu", new { sMaVatTu = sMaVatTu, sTen = sTen, sTenGoc = sTenGoc, sQuyCach = sQuyCach }); %>
            	                    <input id="TaoMoi" type="button" class="button4" value="Tạo mới" style=<%=HienThiTaoMoi %> onclick="javascript:location.href='<%=url%>'" />
            	                </div> 
            	            </td>
                        </tr>
                    </table>
                </td>
                <td width="10%">&nbsp;</td>
            </tr>
        </table>
        </div>
    </div>
</div><br />
<div class="pagedingchuan">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 50%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b>Kết quả tìm kiếm từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> vật tư</b>
                </div>         
            </td>
            <td style="width: 50%">
                <div class="msdn" style="padding-top: 5px;">
                    <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { TimKiemVatTu_page = x, sMaVatTu = sMaVatTu, sTen = sTen, sTenGoc = sTenGoc, sQuyCach = sQuyCach, iTrangThai = iTrangThai, iID_MaDonVi = iID_MaDonVi, TuNgay = TuNgay, DenNgay = DenNgay, cbsMaVatTu = cbsMaVatTu, cbsTen = cbsTen, cbsTenGoc = cbsTenGoc, cbsQuyCach = cbsQuyCach }))%>
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
            <td width="10%" align="center"><b>Mã yêu cầu</b></td>
            <td width="8%" align="center"><b>Mã vật tư</b></td>
            <td width="15%" align="center"><b>Tên</b></td>
            <td width="10%" align="center"><b>Tên gốc</b></td>
            <td width="10%" align="center"><b>Quy cách</b></td>
            <td width="7%" align="center"><b>Đơn vị phát sinh</b></td>
            <td width="7%" align="center"><b>Đơn vị tính</b></td>
            <td width="10%" align="center"><b>Ngày phát sinh mã</b></td>
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
        for (i = 0; i < dt.Rows.Count; i++)
        {
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
            <tr >
            <%}
            else
            {%>
            <tr style="background-color:#FFC">
            <%} %>
                <td><%=i+1 %></td>
                <td><%= MyHtmlHelper.ActionLink(Url.Action("Detail", "TimKiemVatTu", new { iID_MaVatTu = dt.Rows[i]["iID_MaVatTu"] }), dt.Rows[i]["sMaYeuCau"])%></td>
                <td><%= MyHtmlHelper.ActionLink(Url.Action("Detail", "TimKiemVatTu", new { iID_MaVatTu = dt.Rows[i]["iID_MaVatTu"] }), dt.Rows[i]["sMaVatTu"])%></td>
                <td><%= MyHtmlHelper.ActionLink(Url.Action("Detail", "TimKiemVatTu", new { iID_MaVatTu = dt.Rows[i]["iID_MaVatTu"] }), dt.Rows[i]["sTen"])%></td>
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
                <td style="padding-left: 20px; padding-top: 3px; color: #ec3237; text-transform:uppercase;">
                    <b>Kết quả tìm kiếm từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> vật tư</b>
                </td>
                <td>
                    <div class="msdn" style="padding-top: 5px;">
                        <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { TimKiemVatTu_page = x, sMaVatTu = sMaVatTu, sTen = sTen, sTenGoc = sTenGoc, sQuyCach = sQuyCach, iTrangThai = iTrangThai, iID_MaDonVi = iID_MaDonVi, TuNgay = TuNgay, DenNgay = DenNgay, cbsMaVatTu = cbsMaVatTu, cbsTen = cbsTen, cbsTenGoc = cbsTenGoc, cbsQuyCach = cbsQuyCach }))%>
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
