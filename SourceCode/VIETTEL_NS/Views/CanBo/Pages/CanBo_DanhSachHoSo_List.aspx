<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        int i;
        String iID_MaDonVi = "", iID_MaTrinhDo = "", iTuoiTu = "", iDenTuoi = "", sMaHoSo = "", sHoTen = "", iID_MaChucVu = "", iID_MaDT = "", iID_MaNgach = "", sSoHieuCBCC = "",
            iID_MaTrinhDoLyLuanCT = "", iID_MaTinhTrangCanBo = "";
        //lấy giá trị các biến từ Request
        String ParentID = "List";
        String MaND = User.Identity.Name;
        if (Request.QueryString["iID_MaDonVi"] != Convert.ToString(Guid.Empty)) iID_MaDonVi = Request.QueryString["iID_MaDonVi"];      
        if (Request.QueryString["iID_MaTrinhDo"] != Convert.ToString(Guid.Empty)) iID_MaTrinhDo = Request.QueryString["iID_MaTrinhDo"];
        iTuoiTu = Request.QueryString["iTuoiTu"];
        iDenTuoi = Request.QueryString["iDenTuoi"];
        sMaHoSo = Request.QueryString["sMaHoSo"];
        sHoTen = Request.QueryString["sHoTen"];
        if (Request.QueryString["iID_MaChucVu"] != Convert.ToString(Guid.Empty)) iID_MaChucVu = Request.QueryString["iID_MaChucVu"];
        if (Request.QueryString["iID_MaDT"] != Convert.ToString(Guid.Empty)) iID_MaDT = Request.QueryString["iID_MaDT"];
        if (Request.QueryString["iID_MaNgach"] != Convert.ToString(Guid.Empty)) iID_MaNgach = Request.QueryString["iID_MaNgach"];
        sSoHieuCBCC = Request.QueryString["sSoHieuCBCC"];
        if (Request.QueryString["iID_MaTrinhDoLyLuanCT"] != Convert.ToString(Guid.Empty))
            iID_MaTrinhDoLyLuanCT = Request.QueryString["iID_MaTrinhDoLyLuanCT"];
        if (Request.QueryString["iID_MaTinhTrangCanBo"] != "-1") iID_MaTinhTrangCanBo = Request.QueryString["iID_MaTinhTrangCanBo"];
        String page = Request.QueryString["page"];

        //Đơn vị
   
        var dtDonVi = DanhMucModels.getDonViByCombobox(true, "--- Lựa chọn ---");
        SelectOptionList slMaDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        if (dtDonVi != null) dtDonVi.Dispose();

        //Trình độ chuyên môn
        var dtTrinhDoChuyenMon = DanhMucModels.DT_DanhMuc("CanBo_TrinhDoChuyenMon", true, "--- Tất cả ---");
        SelectOptionList slMaTrinhDoChuyenMon = new SelectOptionList(dtTrinhDoChuyenMon, "iID_MaDanhMuc", "sTen");
        if (dtTrinhDoChuyenMon != null) dtTrinhDoChuyenMon.Dispose();
        //Đổ dữ liệu vào chức vụ
        var dtChucVu = DanhMucModels.DT_DanhMuc("CanBo_ChucVu", true, "--- Tất cả ---");
        SelectOptionList slMaChucVu = new SelectOptionList(dtChucVu, "iID_MaDanhMuc", "sTen");
        if (dtChucVu != null) dtChucVu.Dispose();

        //Đối tượng
        var dtDoiTuong = DanhMucModels.DT_DanhMuc("CanBo_DoiTuong", true, "--- Tất cả ---");
        SelectOptionList slMaDoiTuong = new SelectOptionList(dtDoiTuong, "iID_MaDanhMuc", "sTen");
        if (dtDoiTuong != null) dtDoiTuong.Dispose();

        //Ngạch lương
        var dtNgachLuong = CanBo_DanhMucNhanSuModels.getNgachLuong(true, "--- Tất cả ---");
        SelectOptionList slMaNghachLuong = new SelectOptionList(dtNgachLuong, "iID_MaNgachLuong", "sTenNgachLuong");
        if (dtNgachLuong != null) dtNgachLuong.Dispose();
        //Trình độ lý luận chính trị
        var dtTrinhDoLyLuanChinhTri = DanhMucModels.DT_DanhMuc("CanBo_TrinhDoLyLuanChinhTri", true, "--- Tất cả ---");
        SelectOptionList slMaTrinhDoLyLuanChinhTri = new SelectOptionList(dtTrinhDoLyLuanChinhTri, "iID_MaDanhMuc", "sTen");
        if (dtTrinhDoLyLuanChinhTri != null) dtTrinhDoLyLuanChinhTri.Dispose();
        //tình trạng cán bộ
        var dtTTCB = CanBo_DanhMucNhanSuModels.getTinhTrangCB(true, "--- Tất cả ---");
        SelectOptionList slMaTinhTrangCB = new SelectOptionList(dtTTCB, "iID_Ma", "sTen");
        if (dtTTCB != null) dtTTCB.Dispose();
        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        //Lấy danh sách cán bộ
        DataTable dt = CanBo_HoSoNhanSuModels.Get_DanhSach(iID_MaDonVi, iID_MaTinhTrangCanBo, sMaHoSo, sHoTen, sSoHieuCBCC, iID_MaChucVu, iID_MaDT,
            iID_MaTrinhDo, iTuoiTu, iDenTuoi, iID_MaNgach, iID_MaTrinhDoLyLuanCT, CurrentPage, Globals.PageSize);
        double nums = CanBo_HoSoNhanSuModels.Get_DanhSach_Count(iID_MaDonVi, iID_MaTinhTrangCanBo, sMaHoSo, sHoTen, sSoHieuCBCC, iID_MaChucVu, iID_MaDT,
            iID_MaTrinhDo, iTuoiTu, iDenTuoi, iID_MaNgach, iID_MaTrinhDoLyLuanCT);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index",
            new
            {
                iID_MaDonVi = iID_MaDonVi,
                iID_MaTrinhDo = iID_MaTrinhDo,
                iTuoiTu = iTuoiTu,
                iDenTuoi = iDenTuoi,
                sMaHoSo = sMaHoSo,
                sHoTen = sHoTen,
                iID_MaChucVu = iID_MaChucVu,
                iID_MaDT = iID_MaDT,
                iID_MaNgach = iID_MaNgach,
                sSoHieuCBCC = sSoHieuCBCC,
                iID_MaTrinhDoLyLuanCT = iID_MaTrinhDoLyLuanCT,
                iID_MaTinhTrangCanBo = iID_MaTinhTrangCanBo,
                page = x
            }));

        String strThemMoi = Url.Action("Edit", "CanBo_HoSoNhanSu");

        using (Html.BeginForm("SearchSubmit", "CanBo_HoSoNhanSu", new { ParentID = ParentID }))
        {
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
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
                        <td valign="top" align="left" style="width: 30%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Đơn vị công tác</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slMaDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Mã hồ sơ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sMaHoSo, "sMaHoSo", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Họ tên</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sHoTen, "sHoTen", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Số hiệu SQ/QN</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sSoHieuCBCC, "sSoHieuCBCC", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left" style="width: 30%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Tình trạng cán bộ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slMaTinhTrangCB, iID_MaTinhTrangCanBo, "iID_MaTinhTrangCanBo", "", "class=\"input1_2\" style=\"with:100px;\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Chức vụ hiện tại</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slMaChucVu, iID_MaChucVu, "sID_MaChucVu", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Đối tượng quản lý</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slMaDoiTuong, iID_MaDT, "iID_MaDT", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Trình độ chuyên môn</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slMaTrinhDoChuyenMon, iID_MaTrinhDo, "iID_MaTrinhDo", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left" style="width: 30%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Độ tuổi từ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, iTuoiTu, "iTuoiTu", "", "class=\"input1_2\" maxlength = '3' style=\"with:30%\"", 1)%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Độ tuổi đến</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, iDenTuoi, "iDenTuoi", "", "class=\"input1_2\" maxlength = '3' style=\"with:30%\"", 1)%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Ngạch công chức</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slMaNghachLuong, iID_MaNgach, "iID_MaNgach", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Lý luận chính trị</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slMaTrinhDoLyLuanChinhTri, iID_MaTrinhDoLyLuanCT, "iID_MaTrinhDoLyLuanCT", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center" class="td_form2_td1" style="height: 10px;">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" value="Tìm kiếm" />
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        <%
                                            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeNhanSu, MaND))
                                            {
                                        %>
                                        <input id="TaoMoi" type="button" class="button" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                                        <%
                                            } %>
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
                        <span>Danh sách hồ sơ nhân sự</span>
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 7%;" align="center">
                    Số hiệu SQ/QN
                </th>
                <th style="width: 13%;" align="center">
                    Họ và tên
                </th>
                <th style="width: 7%;" align="center">
                    Ngày sinh
                </th>
                <th style="width: 5%;" align="center">
                    Giới tính
                </th>
                <th align="center">
                    Đơn vị
                </th>
                <th style="width: 10%;" align="center">
                    Chức vụ hiện tại
                </th>
                <th style="width: 10%;" align="center">
                    Trình độ chuyên môn
                </th>
                <th style="width: 10%;" align="center">
                    Ngạch công chức
                </th>
              <%--  <th style="width: 10%;" align="center">
                    Đối tượng quản lý
                </th>--%>
                <th style="width: 3%;" align="center">
                    Sửa
                </th>
                <%-- <th style="width: 3%;" align="center">
                    Xem
                </th>--%>
                <th style="width: 3%;" align="center">
                    Xóa
                </th>
            </tr>
            <%
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    String classtr = "";
                    int STT = i + 1;


                    String strEdit = "";
                    String strDelete = "";
                    if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeQuyetToan, MaND))
                    {
                        strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "CanBo_HoSoNhanSu", new { iID_MaCanBo = R["iID_MaCanBo"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                        strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "CanBo_HoSoNhanSu", new { iID_MaCanBo = R["iID_MaCanBo"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                    }
                    String strURL = MyHtmlHelper.ActionLink(Url.Action("Edit", "CanBo_HoSoNhanSu", new { iID_MaCanBo = R["iID_MaCanBo"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");

                    //gioi tinh
                    String gtinh = "";
                    Boolean bGioiTinh = true;
                    if (R["bGioiTinh"]!=DBNull.Value) bGioiTinh= Convert.ToBoolean(R["bGioiTinh"]);
                    if (bGioiTinh) gtinh = "Nam";
                    else gtinh = "Nữ";
                    String MaTrinhDo = "", MaDoiTuong = "", MaChucVu = "", iID_MaTrinhDoChuyenMonCaoNhat = "", iID_MaDoiTuong = "", iID_ChucVuHienTai = "", NgaySinh = "";
                    MaTrinhDo = HamChung.ConvertToString(R["sID_MaTrinhDoChuyenMonCaoNhat"]);
                    //MaDoiTuong = HamChung.ConvertToString(R["iID_MaDoiTuong"]);
                    MaChucVu = HamChung.ConvertToString(R["sID_ChucVuHienTai"]);
                    if (String.IsNullOrEmpty(MaTrinhDo) == false && MaTrinhDo != "" && MaTrinhDo != Convert.ToString(Guid.Empty))
                    {
                        iID_MaTrinhDoChuyenMonCaoNhat = Convert.ToString(CanBo_DanhMucNhanSuModels.getHocVan(false, "", Convert.ToString(MaTrinhDo)).Rows[0]["sTen"]);
                    }
                    //if (String.IsNullOrEmpty(MaDoiTuong) == false && MaDoiTuong != "" && MaDoiTuong != Convert.ToString(Guid.Empty))
                    //{
                    //    iID_MaDoiTuong = Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaDoiTuong)).Rows[0]["sTen"]);
                    //}
                    if (String.IsNullOrEmpty(MaChucVu) == false && MaChucVu != "" && MaChucVu != Convert.ToString(Guid.Empty))
                    {
                        iID_ChucVuHienTai = Convert.ToString(CanBo_DanhMucNhanSuModels.getChucVu(false, "", Convert.ToString(MaChucVu)).Rows[0]["sTen"]);//Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(MaChucVu)).Rows[0]["sTen"]);
                    }
                    String dNgaySinh = String.Format("{0:dd/MM/yyyy}", HamChung.ConvertDateTime(R["dNgaySinh"]));
                    if (dNgaySinh == "01/01/0001") NgaySinh = "";
                    else NgaySinh = dNgaySinh;
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=R["rownum"]%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(R["sSoHieuCBCC"])%>
                </td>
                <td align="left">
                    <%if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeQuyetToan, MaND))
                      { %>
                    <b>
                        <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "CanBo_HoSoNhanSu", new { iID_MaCanBo = R["iID_MaCanBo"] }).ToString(), HttpUtility.HtmlEncode(R["HoTen"]), "Chi tiết hồ sơ", "")%></b>
                    <%}
                      else
                      { %>
                    <%=HttpUtility.HtmlEncode(R["HoTen"])%>
                    <%} %>
                </td>
                <td align="center">
                    <%=NgaySinh%>
                </td>
                <td align="center">
                    <%=gtinh%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(R["TenDonVi"])%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(iID_ChucVuHienTai)%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(iID_MaTrinhDoChuyenMonCaoNhat)%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(R["sTenNgachLuong"])%>
                </td>
              <%--  <td align="left">
                    <%= HttpUtility.HtmlEncode(iID_MaDoiTuong)%>
                </td>--%>
                <td align="center">
                    <%=strEdit%>
                </td>
                <%-- <td align="center">
                    <%=strURL %>
                </td>--%>
                <td align="center">
                    <%=strDelete%>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="13" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
    <%  
        if (dt != null) dt.Dispose();
       
    %>
</asp:Content>
