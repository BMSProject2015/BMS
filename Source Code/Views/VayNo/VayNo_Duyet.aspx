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
        int i, j;
        String MaND = User.Identity.Name;
        String ParentID = "VayNo_Duyet";
        String MaDonVi = Request.QueryString["MaDonVi"];
        String NDVayNo = Request.QueryString["NDVayNo"];
        String sTuNgayVay = Request.QueryString["TuNgayVay"];
        String sDenNgayVay = Request.QueryString["DenNgayVay"];
        String sTuNgay = Request.QueryString["TuNgay"];
        String sDenNgay = Request.QueryString["DenNgay"];
        String sSoChungTu = Request.QueryString["SoChungTu"];
        String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
        String NamHienTai = Request.QueryString["Nam"];
        String ThangHienTai = Request.QueryString["Thang"];
        String page = Request.QueryString["page"];
        int CurrentPage = 1;
        SqlCommand cmd;

        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";

        DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeTinDung, MaND);
        dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
        dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
        dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }

        DataTable dtDonVi = DanhMucModels.NS_DonVi();
        dtDonVi.Rows.InsertAt(dtDonVi.NewRow(), 0);
        dtDonVi.Rows[0]["iID_MaDonVi"] = Guid.Empty;
        dtDonVi.Rows[0]["sTen"] = "-- Chọn đơn vị --";
        SelectOptionList ddlDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        if (dtDonVi != null) dtDonVi.Dispose();

        DataTable dtNoiDungVayNo = VayNoModels.NS_DanhMucNoiDung(true, "-- Chọn nội dung vay nợ --");
        SelectOptionList ddlNoiDungVayNo = new SelectOptionList(dtNoiDungVayNo, "iID_MaNoiDung", "sTenNoiDung");
        if (dtNoiDungVayNo != null) dtNoiDungVayNo.Dispose();


        //Năm
        DateTime dNgayHienTai = DateTime.Now;
        if (NamHienTai != "") NamHienTai = Convert.ToString(dNgayHienTai.Year);
        if (ThangHienTai != "") ThangHienTai = Convert.ToString(dNgayHienTai.Month);
        DataTable dtNam = VayNoModels.getYear(dNgayHienTai, false, "");
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        if (dtNam != null) dtNam.Dispose();

        //tháng
        DataTable dtThang = VayNoModels.getMonth(dNgayHienTai, false, "", "Tháng");
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        if (dtThang != null) dtThang.Dispose();

        ///getdanh sách
        ///
        DataTable dt = VayNoModels.getList(Convert.ToInt32(ThangHienTai), Convert.ToInt32(NamHienTai), MaDonVi, NDVayNo, sSoChungTu, iID_MaTrangThaiDuyet, sTuNgayVay, sDenNgayVay, sTuNgay, sDenNgay, MaND, CurrentPage, Globals.PageSize);

        double nums = VayNoModels.getList_Count(Convert.ToInt32(ThangHienTai), Convert.ToInt32(NamHienTai), MaDonVi, NDVayNo, sSoChungTu, iID_MaTrangThaiDuyet, sTuNgayVay, sDenNgayVay, sTuNgay, sDenNgay, MaND);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("List", new
        {
            ThangHienTai = ThangHienTai,
            NamHienTai = NamHienTai,
            MaDonVi = MaDonVi,
            NDVayNo = NDVayNo,
            sSoChungTu = sSoChungTu,
            iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet,
            sTuNgayVay = sTuNgayVay,
            sDenNgayVay = sDenNgayVay,
            TuNgay = sTuNgay,
            DenNgay = sDenNgay,
            page = x
        }));
        using (Html.BeginForm("SearchDuyetSubmit", "VayNo_Duyet", new { ParentID = ParentID }))
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
                                            <b>Đơn vị vay nợ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, ddlDonVi, MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%></div>
                                    </td>
                                </tr>
                                <%} %>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Ngày vay nợ từ ngay</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, sTuNgayVay, "dTuNgayVay", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Ngày chứng từ từ ngày</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, sTuNgay, "dTuNgay", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Số chứng từ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sSoChungTu, "iSoChungTu", "", "class=\"input1_2\"")%>
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
                                            <b>Nội dung vay nợ</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, ddlNoiDungVayNo, NDVayNo, "iID_MaNoiDung", "", "class=\"input1_2\"")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Ngày vay nợ đến ngày</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, sDenNgayVay, "dDenNgayVay", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Ngày chứng từ đến ngày</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, sDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Trạng thái</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\"")%>
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
    <br />
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách vay nợ</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid" id="<%= ParentID %>_thList">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 15%;" align="center">
                    Đơn vị vay nợ
                </th>
                <th style="width: 7%;" align="center">
                    Số chứng từ
                </th>
                <th style="width: 7%;" align="center">
                    Ngày chứng từ
                </th>
                <th style="width: 10%;" align="center">
                    Người lập
                </th>
                <th style="width: 19%;" align="center">
                    Nội dung vay
                </th>
                <th style="width: 7%;" align="center">
                    Số tiền vay
                </th>
                <th style="width: 7%;" align="center">
                    Lãi suất
                </th>
                <th style="width: 7%;" align="center">
                    Hạn phải trả
                </th>
                <th style="width: 12%;" align="center">
                    Trạng thái
                </th>
                <th style="width: 5%;" align="center">
                    Xử lý
                </th>
            </tr>
            <%
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    String classtr = "";
                    int STT = i + 1;

                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    String urlDetailDV = "/DonVi/Edit?MaDonVi=" + R["iID_MaDonVi"];
                    String sTrangThai = "";
                    String strColor = "";
                    String strURL = MyHtmlHelper.ActionLink(Url.Action("Detail", "VayNo_Duyet", new { MaID = Convert.ToString(R["iID_Vay"]) }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");

                    for (j = 0; j < dtTrangThai.Rows.Count; j++)
                    {
                        if (Convert.ToString(R["iID_MaTrangThaiDuyet"]) == Convert.ToString(dtTrangThai.Rows[j]["iID_MaTrangThaiDuyet"]))
                        {
                            sTrangThai = Convert.ToString(dtTrangThai.Rows[j]["sTen"]);
                            strColor = String.Format("style='background-color: {0}; background-repeat: repeat;'", dtTrangThai.Rows[j]["sMauSac"]);
                            break;
                        }
                    }
            %>
            <tr <%=strColor %>>
                <td align="center">
                    <%=STT%>
                </td>
                <td align="left">
                    <a href="<%=urlDetailDV %>"><b>
                        <%= Convert.ToString(R["sTen"])%>
                    </b></a>
                </td>
                <td align="center">
                    <b>
                        <%=MyHtmlHelper.ActionLink(Url.Action("Detail", "VayNo_Duyet", new { MaID = R["iID_Vay"] }).ToString(), Convert.ToString(R["sSoChungTu"]), "Detail", null, "title=\"Xem chi tiết chứng từ\"")%></b>
                </td>
                <td align="center">
                    <%= VayNoModels.getStringNull( VayNoModels.ConvertDateTime(R["dNgayChungTu"]).ToString("dd/MM/yyyy"))%>
                </td>
                <td align="left">
                    <%= Convert.ToString(R["sID_MaNguoiDungTao"])%>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(Convert.ToString(R["sTenNoiDung"]))%>
                </td>
                <td align="right">
                    <b>
                        <%= CommonFunction.DinhDangSo(R["rVayTrongThang"])%></b>
                </td>
                <td align="right">
                    <%= Convert.ToString(R["rLaiSuat"])%>
                </td>
                <td align="center">
                    <%= VayNoModels.getStringNull( VayNoModels.ConvertDateTime(R["dHanPhaiTra"]).ToString("dd/MM/yyyy"))%>
                </td>
                <td align="center">
                    <%= sTrangThai %>
                </td>
                <td align="center">
                    <%=strURL %>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="11" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
