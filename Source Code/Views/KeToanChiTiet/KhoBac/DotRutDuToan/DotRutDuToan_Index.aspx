<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String ParentID = "DotRutDuToan";
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);

        String NamNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
        String NguonNganSach = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
        String NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
        int CurrentPage = 1;
        String page = Request.QueryString["page"];
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }

        DataTable dt = KeToan_RutDuToanDotModels.GetDotRutDuToan(NamLamViec, NamNganSach, NguonNganSach, CurrentPage, Globals.PageSize);
        double nums = KeToan_RutDuToanDotModels.GetDotRutDuToan_Count(NamLamViec, NamNganSach, NguonNganSach);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        using (Html.BeginForm("AddNewSubmit", "KeToan_RutDuToan", new { ParentID = ParentID }))
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Chọn đợt</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1" style="width: 70%;">
                            <div>
                                <b>
                                    <%=NgonNgu.LayXau("Đợt chỉ tiêu ngày:")%></b>
                            </div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dNgayDotRutDuToan", "", "class=\"input1_2\" onblur=isDate(this) style=\"width:100%;\" tab-index='-1'")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayDotRutDuToan")%>
                            </div>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" id="tb_DotNganSach">
                    <tr>
                        <td class="td_form2_td1" style="width: 15%;">
                            <div>
                            </div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">
                                            &nbsp;
                                        </td>
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Thêm mới")%>" />
                                        </td>
                                        <td width="5px">
                                            &nbsp;
                                        </td>
                                        <td class="td_form2_td5">
                                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Hủy")%>" onclick="history.go(-1)" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%} %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách đợt chỉ tiêu</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="<%= ParentID %>_divDotNganSach">
            <table class="mGrid" id="<%= ParentID %>_thList">
                <tr>
                    <th style="width: 5%;" align="center">
                        STT
                    </th>
                    <th align="center">
                        Đợt chỉ tiêu
                    </th>
                    <th style="width: 5%;" align="center">
                        Chi tiết
                    </th>
                    <%-- <th style="width: 5%;" align="center">
                        Sửa
                    </th>--%>
                    <th style="width: 5%;" align="center">
                        Xóa
                    </th>
                </tr>
                <%
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow R = dt.Rows[i];
                        String classtr = "";
                        int STT = i + 1;
                        String dNgayDotRutDuToan = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayDotRutDuToan"]));
                        String sTrangThai = "";

                        String strEdit = "";
                        String strDelete = "";


                        strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "KeToan_RutDuToan", new { iID_MaDotRutDuToan = R["iID_MaDotRutDuToan"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                        strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "KeToan_RutDuToan", new { iID_MaDotRutDuToan = R["iID_MaDotRutDuToan"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                        String strURL = MyHtmlHelper.ActionLink(Url.Action("Detail", "KeToan_RutDuToan", new { iID_MaDotRutDuToan = R["iID_MaDotRutDuToan"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết\"");
                        String urlDetail = "/KeToan_RutDuToan/Detail?iID_MaDotRutDuToan=" + Convert.ToString(R["iID_MaDotRutDuToan"]);
                %>
                <tr>
                    <td align="center">
                        <%=R["rownum"]%>
                    </td>
                    <td align="left">
                        <a href="<%=urlDetail %>"><b>
                            <%= "Đợt ngày " + HttpUtility.HtmlEncode(HamChung.ConvertDateTime( dt.Rows[i]["dNgayDotRutDuToan"]).ToString("dd/MM/yyyy"))%>
                        </b></a>
                    </td>
                    <td align="center">
                        <%=strURL%>
                    </td>
                    <%-- <td align="center">
                        <%=strEdit%>
                    </td>--%>
                    <td align="center">
                        <%=strDelete%>
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
    </div>
</asp:Content>
