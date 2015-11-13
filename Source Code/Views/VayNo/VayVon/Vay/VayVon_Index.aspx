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
        String dFromNgayTao = Request.QueryString["dFromNgayTao"];
        String dToNgayTao = Request.QueryString["dToNgayTao"];
        String dFromNgayTra = Request.QueryString["dFromNgayTra"];
        String dToNgayTra = Request.QueryString["dToNgayTra"];
        String ParentID = "VayVon";
        String page = Request.QueryString["page"];
        String MaDonVi = "", MaNoiDung = "", iID_Vay = "";
        if (Request.QueryString["MaDonVi"] != null) MaDonVi = Request.QueryString["MaDonVi"];
        if (Request.QueryString["MaNoiDung"] != null) MaNoiDung = Request.QueryString["MaNoiDung"];
        if (Request.QueryString["iID_MaChungTu"] != null) iID_Vay = Request.QueryString["iID_MaChungTu"];
        int CurrentPage = 1;

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = VayNoModels.getListVayVon(iID_Vay, MaNoiDung, MaDonVi, dFromNgayTao, dToNgayTao, dFromNgayTra, dToNgayTra, CurrentPage, Globals.PageSize);

        double nums = VayNoModels.getListVayVon_Count(MaNoiDung, MaDonVi);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        String strThemMoi = Url.Action("Edit", "VayVon");

        //load nội dung
        DataTable dtNoiDung = DanhMucModels.VN_NoiDung();
        dtNoiDung.Rows.InsertAt(dtNoiDung.NewRow(), 0);
        dtNoiDung.Rows[0]["iID_MaNoiDung"] = string.Empty;
        dtNoiDung.Rows[0]["sTenNoiDung"] = "-- Chọn nội dung --";
        SelectOptionList dllNoiDung = new SelectOptionList(dtNoiDung, "iID_MaNoiDung", "sTenNoiDung");
        if (dtNoiDung != null) dtNoiDung.Dispose();

        //load don vi
        DataTable dtDonvi = DanhMucModels.NS_DonVi();
        dtDonvi.Rows.InsertAt(dtDonvi.NewRow(), 0);
        dtDonvi.Rows[0]["iID_MaDonVi"] = "";
        dtDonvi.Rows[0]["sTen"] = "-- Chọn đơn vị --";
        SelectOptionList ddlDonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
        if (dtDonvi != null) dtDonvi.Dispose();
        using (Html.BeginForm("SearchSubmit", "VayVon", new { ParentID = ParentID }))
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
                                        <input id="TaoMoi" type="button" class="button" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
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
                    Đơn vị
                </th>
                <th style="width: 10%;" align="center">
                    B quản lý
                </th>
                <th align="center">
                    Mã nội dung
                </th>
                <th style="width: 10%;" align="center">
                    Ngày vay
                </th>
                <th style="width: 10%;" align="center">
                    Ngày trả
                </th>
                <th style="width: 10%;" align="center">
                    Số tiền vay
                </th>
                <th style="width: 5%;" align="center">
                    Sửa
                </th>
                <th style="width: 5%;" align="center">
                    Xóa
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
                    string strEdit = string.Empty;
                    string strDelete = string.Empty;
                    strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "VayVon", new { iID_VayChiTiet = R["iID_VayChiTiet"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "EditNoiDung", "");
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("DeleteVayVon", "VayVon", new { iID_VayChiTiet = R["iID_VayChiTiet"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "DeleteNoiDung", "");
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=STT%>
                </td>
                <td align="left">
                    <%=dt.Rows[i]["iID_MaDonVi"] + " - " + HttpUtility.HtmlEncode(dt.Rows[i]["sTenDonVi"])%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(dt.Rows[i]["sTenPhongBan"])%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(dt.Rows[i]["sTenNoiDung"])%>
                </td>
                <td align="center">
                    <%= dNgayVayShort %>
                </td>
                <td align="center">
                    <%= dHanPhaiTraShort %>
                </td>
                <td align="left">
                    <b>
                        <%=CommonFunction.DinhDangSo(dt.Rows[i]["rVayTrongThang"])%></b>
                </td>
                <td align="center">
                    <%=strEdit%>
                </td>
                <td align="center">
                    <%=strDelete%>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="9" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
    <%   if (dt != null) dt.Dispose(); %>
</asp:Content>
