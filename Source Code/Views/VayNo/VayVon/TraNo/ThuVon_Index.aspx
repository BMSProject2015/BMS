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
        String MaND = User.Identity.Name;
        String dFromNgayTao = Request.QueryString["dFromNgayTao"];
        String dToNgayTao = Request.QueryString["dToNgayTao"];
        String dFromNgayTra = Request.QueryString["dFromNgayTra"];
        String dToNgayTra = Request.QueryString["dToNgayTra"];
        String Nam = Request.QueryString["Nam"];
        String Thang = Request.QueryString["Thang"];
        String ParentID = "VayVon";
        String page = Request.QueryString["page"];
        String MaDonVi = "", MaNoiDung = "";
        if (Request.QueryString["MaDonVi"] != null) MaDonVi = Request.QueryString["MaDonVi"];
        if (Request.QueryString["MaNoiDung"] != null) MaNoiDung = Request.QueryString["MaNoiDung"];
        int CurrentPage = 1;

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }

        //load nội dung
        DataTable dtNoiDung = DanhMucModels.VN_NoiDung();
        dtNoiDung.Rows.InsertAt(dtNoiDung.NewRow(), 0);
        dtNoiDung.Rows[0]["iID_MaNoiDung"] = string.Empty;
        dtNoiDung.Rows[0]["sTenNoiDung"] = "-- Chọn nội dung --";
        SelectOptionList dllNoiDung = new SelectOptionList(dtNoiDung, "iID_MaNoiDung", "sTenNoiDung");
        if (dtNoiDung != null) dtNoiDung.Dispose();


        //nam lam viec        
        string ThangHienTai = DateTime.Now.Month.ToString();
        string NamHienTai = DateTime.Now.Year.ToString();
        if (Nam != "" && String.IsNullOrEmpty(Nam) == false) NamHienTai = Nam;
        if (Thang != "" && String.IsNullOrEmpty(Thang) == false) ThangHienTai = Thang;
        DataTable dtNam = HamChung.getYear(DateTime.Now, false, "");
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        if (dtNam != null) dtNam.Dispose();

        //tháng
        DataTable dtThang = HamChung.getMonth(DateTime.Now, false, "", "Tháng");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        //load don vi
        DataTable dtDonvi = DanhMucModels.NS_DonVi();
        dtDonvi.Rows.InsertAt(dtDonvi.NewRow(), 0);
        dtDonvi.Rows[0]["iID_MaDonVi"] = "";
        dtDonvi.Rows[0]["sTen"] = "-- Chọn đơn vị --";
        SelectOptionList ddlDonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
        if (dtDonvi != null) dtDonvi.Dispose();



        DataTable dt = VayNoModels.getListVayVon(Thang, Nam, MaNoiDung, MaDonVi, dFromNgayTao, dToNgayTao, dFromNgayTra, dToNgayTra, CurrentPage, Globals.PageSize);

        double nums = VayNoModels.getListVayVon_Count(Thang, Nam, MaNoiDung, MaDonVi);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new
        {
            Thang = Thang,
            Nam = Nam,
            MaNoiDung = MaNoiDung,
            MaDonVi = MaDonVi,

            dFromNgayTao = dFromNgayTao,
            dToNgayTao = dToNgayTao,
            dFromNgayTra = dFromNgayTra,
            dToNgayTra = dToNgayTra,
            page = x
        }));
        String strThemMoi = Url.Action("Edit", "ChungTuVayNo", new { ParentID = ParentID });

        using (Html.BeginForm("SearchSubmit", "TraNo", new { ParentID = ParentID }))
        {
    %>
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
                                                <%=NgonNgu.LayXau("Chọn năm làm việc")%></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slNam, NamHienTai, "MaNam", "", "class=\"input1_2\" ")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Đơn vị</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, ddlDonvi, MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Ngày tạo từ ngày</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, dFromNgayTao, "dFromNgayTao", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Hạn trả từ ngày</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, dFromNgayTra, "dFromNgayTra", "", "class=\"input1_2\"")%>
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
                                                <%=NgonNgu.LayXau("Chọn tháng làm việc")%></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slThang, ThangHienTai, "MaThang", "", "class=\"input1_2\" ")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Nội dung</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, dllNoiDung, MaNoiDung, "iID_MaNoiDung", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Tới ngày</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, dToNgayTao, "dToNgayTao", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Tới ngày</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, dToNgayTra, "dToNgayTra", "", "class=\"input1_2\"")%>
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
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        <%
                                            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(DuToanModels.iID_MaPhanHe, MaND))
                                            {
                                        %>
                                        <input id="Button1" type="button" class="button" value="Tạo ch.từ" onclick="javascript:location.href='<%=strThemMoi %>'" />
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
                        <span>Danh sách vay vốn</span>
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 15%;" align="center">
                    Khoản vay - Đơn vị
                </th>
                <th style="width: 10%;" align="center">
                    B quản lý
                </th>
                <th align="center">
                    Mã nội dung
                </th>
                <th style="width: 7%;" align="center">
                    Ngày vay
                </th>
                <th style="width: 7%;" align="center">
                    Hạn phải trả
                </th>
                <th style="width: 10%;" align="center">
                    Số tiền vay
                </th>
               <%-- <th style="width: 10%;" align="center">
                    Thu vốn
                </th>
                <th style="width: 10%;" align="center">
                    Thu lãi
                </th>--%>
                <th style="width: 5%;" align="center">
                    Chi Tiết
                </th>
            </tr>
            <%
                
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    string dNgayVayShort = string.Empty;
                    string dHanPhaiTraShort = string.Empty;
                    if (R["dNgayVay"] != DBNull.Value)
                    {
                        DateTime dNgayVay = Convert.ToDateTime(R["dNgayVay"]);
                        dNgayVayShort = dNgayVay.ToString("dd/MM/yyyy");
                    }
                    if (R["dHanPhaiTra"] != DBNull.Value)
                    {
                        DateTime dHanPhaiTra = Convert.ToDateTime(R["dHanPhaiTra"]);
                        dHanPhaiTraShort = dHanPhaiTra.ToString("dd/MM/yyyy");
                    }
                    String bPublic = Convert.ToString(R["bPublic"]);
                    string strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
                    String classtr = "";
                    int STT = i + 1;
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    string strThuVon = string.Empty;
                    strThuVon = MyHtmlHelper.ActionLink(Url.Action("Detail", "TraNo", new { iID_VayChiTiet = R["iID_VayChiTiet"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "TroNo", "");
                   
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=STT%>
                </td>
                <td align="left">
                    <%= HamChung.ConvertToString( dt.Rows[i]["iID_MaKhoanVay"]) + "" + HamChung.ConvertToString(dt.Rows[i]["iID_MaDonVi"]) + " - " + HttpUtility.HtmlEncode(HamChung.ConvertToString(dt.Rows[i]["sTenDonVi"]))%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(dt.Rows[i]["sTenPhongBan"]))%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(dt.Rows[i]["sTenNoiDung"]))%>
                </td>
                <td align="center">
                    <%= dNgayVayShort %>
                </td>
                <td align="center">
                    <%= dHanPhaiTraShort %>
                </td>
                <td align="right">
                    <b>
                        <%=CommonFunction.DinhDangSo(dt.Rows[i]["rVayTrongThang"])%></b>
                </td>
              <%--  <td align="right">
                    <b>
                        <%=CommonFunction.DinhDangSo(dt.Rows[i]["rThuVon"])%></b>
                </td>
                <td align="right">
                    <b>
                        <%=CommonFunction.DinhDangSo(dt.Rows[i]["rThuLai"])%></b>
                </td>--%>
                <td align="center">
                    <%=strThuVon%>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="10" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
    <%   if (dt != null) dt.Dispose(); %>
</asp:Content>
