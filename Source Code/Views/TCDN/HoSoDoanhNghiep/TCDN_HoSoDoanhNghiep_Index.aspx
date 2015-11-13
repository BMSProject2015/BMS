<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <%
        string sTenDoanhNghiep = "", sTenThuongGoi = "", sTenGiaoDich = "", sTenTheoQuocPhong = "", iID_MaDonVi = "", iNamLamViec = "",
    dNgayDangKy = "", sNganhNgheKinhDoanh = "", sDiaChi = "", sDienThoai = "", sFax = "", sWebsite = "", sEmail = "",
    sMaChungKhoan = "", rVonDieuLe = "", rVonNhaNuoc = "", rVonChuSoHuu = "", sNguoiDaiDienVonNhaNuoc = "", sSoCoPhan = "",
    sChucVu = "", iID_MaLoaiDoanhNghiep = "", iID_MaNhomDoanhNghiep = "", sGhiChu = "", bHoatDong = ""; ;
        int i;
        String MaND = User.Identity.Name;
        String ParentID = "HSDN";
        DataTable dtLoai = null;
        SqlCommand cmd;


        DataTable dtDV = DanhMucModels.getDonViByCombobox(true);
        SelectOptionList slDV = new SelectOptionList(dtDV, "iID_MaDonVi", "sTen");
        dtDV.Dispose();

        DataTable dtNamLamViec = DanhMucModels.DT_Nam(true);
        SelectOptionList optNamLamViec = new SelectOptionList(dtNamLamViec, "MaNam", "TenNam");
        
        //lấy danh mục loại hình doanh nghiệp
        dtLoai = DanhMucModels.DT_DanhMuc("TCDN_LoaiHinhDN", true, "--- Loại hình doanh nghiệp ---");
        SelectOptionList slLoaiHinh = new SelectOptionList(dtLoai, "iID_MaDanhMuc", "sTen");
        if (dtLoai != null) dtLoai.Dispose();
        
        //lấy danh mục nhóm doanh nghiệp
        dtLoai = DanhMucModels.DT_DanhMuc("TCDN_NhomDN", true, "--- Loại nhóm doanh nghiệp ---");
        SelectOptionList slNhom = new SelectOptionList(dtLoai, "iID_MaDanhMuc", "sTen");
        if (dtLoai != null) dtLoai.Dispose();
        
        String page = Request.QueryString["page"];
        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        
        //danh sách doanh nghiệp
        DataTable dt = TCSN_DoanhNghiepModels.getList(sTenDoanhNghiep, sTenThuongGoi, sTenGiaoDich, sTenTheoQuocPhong, iID_MaDonVi, iNamLamViec, iID_MaLoaiDoanhNghiep, iID_MaNhomDoanhNghiep, CurrentPage, Globals.PageSize);
        
        //đếm số dòng để phân trang
        double nums = TCSN_DoanhNghiepModels.getList_Count(sTenDoanhNghiep, sTenThuongGoi, sTenGiaoDich, sTenTheoQuocPhong, iID_MaDonVi, iNamLamViec, iID_MaLoaiDoanhNghiep, iID_MaNhomDoanhNghiep);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        //code phân trang
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("List", new
        {
            ParentID = ParentID,
            sTenDoanhNghiep = sTenDoanhNghiep,
            sTenThuongGoi = sTenThuongGoi,
            sTenGiaoDich = sTenGiaoDich,
            sTenTheoQuocPhong = sTenTheoQuocPhong,
            iID_MaDonVi = iID_MaDonVi,
            iNamLamViec = iNamLamViec,
            iID_MaLoaiDoanhNghiep = iID_MaLoaiDoanhNghiep,
            iID_MaNhomDoanhNghiep = iID_MaNhomDoanhNghiep,
            page = x
        }));

        //đoạn code để khi chọn thêm mới
        String strThemMoi = Url.Action("Edit", "TCDN_DoanhNghiep");
        
        //sự kiện tìm kiếm được chọn
        using (Html.BeginForm("SearchSubmit", "TCDN_DoanhNghiep", new
        {
            ParentID = ParentID,
            sTenDoanhNghiep = sTenDoanhNghiep,
            sTenThuongGoi = sTenThuongGoi,
            sTenGiaoDich = sTenGiaoDich,
            sTenTheoQuocPhong = sTenTheoQuocPhong,
            iID_MaDonVi = iID_MaDonVi,
            iNamLamViec = iNamLamViec,
            iID_MaLoaiDoanhNghiep = iID_MaLoaiDoanhNghiep,
            iID_MaNhomDoanhNghiep = iID_MaNhomDoanhNghiep
        }))
        {
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                    <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>         
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color:#ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_DoanhNghiep"), "Danh sách doanh nghiệp")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Thông tin tìm kiếm</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top" align="left" style="width: 45%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%=NgonNgu.LayXau("Tên doanh nghiệp")%></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sTenDoanhNghiep, "sTenDoanhNghiep", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%=NgonNgu.LayXau("Tên thường gọi")%></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sTenThuongGoi, "sTenThuongGoi", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%=NgonNgu.LayXau("Tên giao dịch")%></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sTenGiaoDich, "sTenGiaoDich", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%=NgonNgu.LayXau("Tên theo quốc phòng")%></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sTenTheoQuocPhong, "sTenTheoQuocPhong", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left" style="width: 45%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                   <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%=NgonNgu.LayXau("Đơn vị quản lý")%></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slDV, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="td_form2_td1"><div><b>Năm quản lý</b></div></td>
                                    <td class="td_form2_td5">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, optNamLamViec, iNamLamViec, "iNamLamViec", null, "style=\"width: 50%;\"")%>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iNamLamViec")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%=NgonNgu.LayXau("Loại hình doanh nghiệp")%></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slLoaiHinh, iID_MaLoaiDoanhNghiep, "iID_MaLoaiDoanhNghiep", "", "class=\"input1_2\" ")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%=NgonNgu.LayXau("Nhóm doanh nghiệp")%></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slNhom, iID_MaNhomDoanhNghiep, "iID_MaNhomDoanhNghiep", "", "class=\"input1_2\" ")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" class="td_form2_td1" style="height: 10px;">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" value="Tìm kiếm" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%  } %>
    <br />
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách hồ sơ doanh nghiệp</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid" id="<%= ParentID %>_thList">
            <tr>
                <th style="width: 5%;" align="center">STT</th>
                <th align="center">Tên doanh nghiệp</th>
                <th style="width: 15%;" align="center">Tên thường gọi</th>
                <th style="width: 15%;" align="center">Tên giao dịch</th>
                <th style="width: 20%;" align="center">Loại hình DN</th>
                <th style="width: 20%;" align="center">Nhóm DN</th>
            </tr>
            <%
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    int sSTT = i + 1;
                    DataRow R = dt.Rows[i];
                    String strEdit = "";
                    String strDelete = "";
                    strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "TCDN_DoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"]}).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "EditNoiDung", "");
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "TCDN_DoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "DeleteNoiDung", "");
                    String TenLoaiHinh = Convert.ToString(DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(R["iID_MaLoaiDoanhNghiep"])).Rows[0]["sTen"]);
                    String TenNhomDN = Convert.ToString(DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(R["iID_MaNhomDoanhNghiep"])).Rows[0]["sTen"]);
            %>
            <tr>
                <td align="center" style="padding: 3px 2px;">
                    <%=R["rownum"]%>
                </td>
                <td align="left">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "TCDN_HoSoDoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"], iQuy = 1}).ToString(), HttpUtility.HtmlEncode(R["sTenDoanhNghiep"]), "EditNoiDung", "")%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(R["sTenThuongGoi"])%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(R["sTenGiaoDich"])%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(TenLoaiHinh)%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(TenNhomDN)%>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="8" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

