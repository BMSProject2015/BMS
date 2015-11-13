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
        String ParentID = "MLDA";
        DataTable dtThamQuyen = DanhMucModels.DT_DanhMuc_All("ThamQuyenCongTrinhDuAn");
        DataTable dtLoaiCT = DanhMucModels.DT_DanhMuc_All("LoaiCongTrinhDuAn");
        DataTable dtTinhChatCT = DanhMucModels.DT_DanhMuc_All("TinhChatCongTrinhDuAn");
        SelectOptionList optThamQuyen = new SelectOptionList(dtThamQuyen, "sTenKhoa", "sTen");
        dtThamQuyen.Dispose();
        SelectOptionList optLoaiCT = new SelectOptionList(dtLoaiCT, "sTenKhoa", "sTen");
        dtLoaiCT.Dispose();
        SelectOptionList optTinhChatCT = new SelectOptionList(dtTinhChatCT, "sTenKhoa", "sTen");
        dtTinhChatCT.Dispose();
        DataTable dtDonVi = DonViModels.Get_dtDonVi();
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
       

        DataRow r = dtDonVi.NewRow();
        r["sTen"] = "--Chọn tất cả đơn vị--";
        dtDonVi.Rows.InsertAt(r, 0);
        dtDonVi.Dispose();
        String sTen = "", MaThamQuyen = "", MaLoaiCT = "", MaTinhChatCT = "", bHoanThanh = "", iID_MaDonVi = "";

        sTen = Convert.ToString(ViewData["sTen"]);
        MaThamQuyen = Convert.ToString(ViewData["MaThamQuyen"]);
        MaLoaiCT = Convert.ToString(ViewData["MaLoaiCT"]);
        MaTinhChatCT = Convert.ToString(ViewData["MaTinhChatCT"]);
        bHoanThanh = Convert.ToString(ViewData["bHoanThanh"]);
        iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);



        String page = Request.QueryString["page"];
        int CurrentPage = 1;

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = MucLucDuAnModels.GetMLDA(CurrentPage, Globals.PageSize, sTen, MaThamQuyen, MaLoaiCT, MaTinhChatCT, bHoanThanh, iID_MaDonVi);

        double nums = MucLucDuAnModels.GetMLDA_Count();
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        String strThemMoi = Url.Action("Edit", "MucLucDuAn");
        using (Html.BeginForm("SearchSubmit", "MucLucDuAn", new { ParentID = ParentID }))
        { %>
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
                                            <b>Thẩm quyền dự án</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, optThamQuyen, MaThamQuyen, "MaThamQuyen", "", "class=\"input1_2\"")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Loại dự án</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, optLoaiCT, MaLoaiCT,"MaLoaiCT", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" >
                                        <div>
                                            <b>Tính chất dự án</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, optTinhChatCT, MaTinhChatCT,"MaTinhChatCT", "", "class=\"input1_2\"")%>
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
                                            <b>Đơn vị</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Tên dự án</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID,sTen, "sTen", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" height="40px">
                                        <div>
                                            <b>Hoàn thành</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.CheckBox(ParentID, bHoanThanh, "bHoanThanh", "", "class=\"input1_2\"")%>
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
                        <span>Danh sách mục lục dự án</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'"
                            style="cursor: pointer;" />
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 5%;" align="left">
                    Mã dự án
                </th>
                <th style="width: 30%;" align="left">
                    Tên mục lục dự án
                </th>
                <th style="width: 10%;" align="left">
                    Đơn vị
                </th>
                <th style="width: 15%;" align="left">
                    Loại dự án
                </th>
                <th style="width: 15%;" align="left">
                    Tính chất dự án
                </th>
                <th style="width: 15%;" align="left">
                    Mã thẩm quyền
                </th>
                <th style="width: 15%;" align="left">
                    Đã hoàn thành
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
                    String MaLoaiDuAn = Convert.ToString(R["iLoai"]);
                    String MaTinhChatDuAn = Convert.ToString(R["iTinhChat"]);
                    MaThamQuyen = Convert.ToString(R["iThamQuyen"]);
                    String sLoaiDuAn = CommonFunction.LayTenDanhMuc(MaLoaiDuAn);
                    String sTinhChatDuAn = CommonFunction.LayTenDanhMuc(MaTinhChatDuAn);
                    String sThamQuyen = CommonFunction.LayTenDanhMuc(MaThamQuyen);
                    String sTenDonVi = "";
                    iID_MaDonVi = Convert.ToString(R["iID_MaDonVi"]);
                    if (String.IsNullOrEmpty(iID_MaDonVi) == false) sTenDonVi = DonViModels.Get_TenDonVi(iID_MaDonVi);
                    String classtr = "";
                    int STT = i + 1;
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=STT%>
                </td>
                <td align="left">
                    <%=dt.Rows[i]["sMaCongTrinh"]%>
                </td>
                <td align="left">
                    <%=dt.Rows[i]["sTen"]%>
                </td>
                <td align="left">
                    <%=dt.Rows[i]["sMoTa"]%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(sTenDonVi)%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(sLoaiDuAn) %>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(sTinhChatDuAn)%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(sThamQuyen)%>
                </td>
                <td>
                    <%=MyHtmlHelper.LabelCheckBox("Index",R["bHoanThanh"],"bHoanThanh") %>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "MucLucDuAn", new { Code = R["iID_MaDanhMucDuAn"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "MucLucDuAn", new { Code = R["iID_MaDanhMucDuAn"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
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
