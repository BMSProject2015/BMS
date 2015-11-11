<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
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
        String ParentID = "NDPB";
        String page = Request.QueryString["page"];
        String MaDonVi = "", MaPhongBan = "";
        if (Request.QueryString["MaDonVi"] != null) MaDonVi = Request.QueryString["MaDonVi"];
        if (Request.QueryString["MaPhongBan"] != null) MaPhongBan = Request.QueryString["MaPhongBan"];
        int CurrentPage = 1;

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = PhongBan_DonViModels.getList(MaPhongBan, MaDonVi, CurrentPage, Globals.PageSize);

        double nums = PhongBan_DonViModels.getList_Count1(MaPhongBan, MaDonVi);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        String strThemMoi = Url.Action("Add", "PhongBanDonVi");
        //load phong ban
        DataTable dtTrangThai = DanhMucModels.NS_PhongBan();
        dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
        dtTrangThai.Rows[0]["iID_MaPhongBan"] = Guid.Empty;
        dtTrangThai.Rows[0]["sTen"] = "-- Chọn phòng ban --";
        SelectOptionList dllPhongban = new SelectOptionList(dtTrangThai, "iID_MaPhongBan", "sTen");
        if (dtTrangThai != null) dtTrangThai.Dispose();

        //load don vi
        DataTable dtDonvi = DanhMucModels.NS_DonVi();
        dtDonvi.Rows.InsertAt(dtDonvi.NewRow(), 0);
        dtDonvi.Rows[0]["iID_MaDonVi"] = "";
        dtDonvi.Rows[0]["sTen"] = "-- Chọn đơn vị --";
        SelectOptionList ddlDonvi = new SelectOptionList(dtDonvi, "iID_MaDonVi", "sTen");
        if (dtDonvi != null) dtDonvi.Dispose();
        using (Html.BeginForm("SearchSubmit", "PhongBanDonVi", new { ParentID = ParentID }))
        {
    %>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform: uppercase;
                    color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-top: 5px; padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                  
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
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
                                            <b>Đơn vị</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                             <%=MyHtmlHelper.DropDownList(ParentID, ddlDonvi, "", "search_iID_MaDonVi", "", "class=\"input1_2\"")%>
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
                                            <b>Phòng ban</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, dllPhongban, "", "search_iID_MaPhongBan", "", "class=\"input1_2\"")%>
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
                        <span>Danh sách Phòng ban - đơn vị</span>
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 10%;" align="center">
                  Phòng ban
                </th>
               
                <th  align="center">
                    Đơn vị quản lý
                </th>
                <th style="width: 10%;" align="center">
                    Ngày tạo
                </th>
              <%--  <th style="width: 5%;" align="center">
                    Hoạt động
                </th>--%>
                
                <th style="width: 5%;" align="center">
                    Xóa
                </th>
            </tr>
            <%
                
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];

                    String bPublic = Convert.ToString(R["bPublic"]);
                    string strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
                    String classtr = "";
             
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
                    string iID_MaPhongBan = Convert.ToString(R["iID_MaPhongBan"]);
                    string DonVi = PhongBan_DonViModels.getDonVi(iID_MaPhongBan);
                    String urlDetail = "/PhongBanDonVi/EditDetail?Code=" + Convert.ToString(R["iID_MaPhongBan"]);
            %>
            <tr <%=classtr %>>
                <td align="center"><%=R["rownum"]%></td>
                <td align="left">
                    <a href=<%=urlDetail %>><b>
                        <%=HttpUtility.HtmlEncode(dt.Rows[i]["sTen"])%></b></a>
                </td>
              
                <td align="left">
                    <%= DonVi %>
                </td>
                <td align="left">
                    <%=dt.Rows[i]["dNgayTao"]%>
                </td>
                <%--<td align="center">
                    <% if (bPublic == "True")
                       { %>
                    <%=strIcon %>
                    <%} %>
                </td>--%>
              
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "PhongBanDonVi", new { Code = R["iID_MaPhongBan"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
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
