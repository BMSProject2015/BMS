<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String iID_MaNoiDung = Convert.ToString(ViewData["iID_MaNoiDung"]);
        string sTenNoiDung = Convert.ToString(ViewData["sTenNoiDung"]);
        if (iID_MaNoiDung == null)
            iID_MaNoiDung = string.Empty;
        if (sTenNoiDung == null)
            sTenNoiDung = string.Empty;

        int i;
        String UserID = User.Identity.Name;
        String ParentID = "VayNo_NoiDung";



        String page = Request.QueryString["page"];
        int CurrentPage = 1;
        SqlCommand cmd;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        double nums = VayNoModels.LaySoBanGhiNoiDung();
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        //String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("List", new { SoChungTu = sSoChungTu, TuNgay = sTuNgay, DenNgay = sDenNgay, MaTrangTrai = iID_MaTrangThai, page = x }));
        String strThemMoi = Url.Action("EditNoiDung", "VayNo_NoiDung", new { ID = string.Empty });
        DataTable dt = VayNoModels.LayDanhSachNoiDung(iID_MaNoiDung, sTenNoiDung);
        using (Html.BeginForm("SearchSubmitNoiDung", "VayNo_NoiDung", new { ParentID = ParentID, MaNoiDung = iID_MaNoiDung, TenNoiDung = sTenNoiDung }))
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
                                            <b>Mã nội dung</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, iID_MaNoiDung, "iID_MaNoiDung", "", "class=\"input1_2\"")%>
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
                                            <b>Tên nội dung</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sTenNoiDung, "sTenNoiDung", "", "class=\"input1_2\"")%>
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
                        <span>Danh sách nội dung</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid" id="<%= ParentID %>_thList">
            <tr>
                <th style="width: 5%;" align="center">
                    STT
                </th>
                <th style="width: 10%;" align="center">
                    Mã nội dung
                </th>
                <th style="width: 40%;" align="center">
                    Tên nội dung
                </th>
                <th style="width: 10%;" align="center">
                    Ngày tạo
                </th>
                <th style="width: 20%;" align="center">
                    Trạng thái
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
                    int sSTT = i + 1;
                    DataRow R = dt.Rows[i];
                    String strEdit = "";
                    String strDelete = "";
                    DateTime dtCreate = Convert.ToDateTime(R["sNgayTao"]);
                    string shortDate = dtCreate.ToString("dd/MM/yyyy");
                    string sHoatDong = Convert.ToString(R["sHoatDong"]).Equals("True") ? "Có hiệu lực" : "Không có hiệu lực";
                    strEdit = MyHtmlHelper.ActionLink(Url.Action("EditNoiDung", "VayNo_NoiDung", new { ID = R["ID"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "EditNoiDung", "");
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("DeleteNoiDung", "VayNo_NoiDung", new { ID = R["ID"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "DeleteNoiDung", "");
            %>
            <tr>
                <td align="center" style="padding: 3px 2px;">
                    <%=sSTT%>
                </td>
                <td align="center">
                    <%=R["iID_MaNoiDung"] %>
                </td>
                <td align="left">
                    <%= R["sTenNoiDung"] %>
                </td>
                <td align="center">
                    <%=  shortDate %>
                </td>
                <td align="center">
                    <%= sHoatDong%>
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
                <td colspan="8" align="right">
                    <%--<%=strPhanTrang%>--%>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        setCheckboxes();
        function setCheckboxes() {
            var cb = document.getElementById('<%= ParentID %>_thList').getElementsByTagName('input');
            for (var i = 0; i < cb.length; i++) {
                cb[i].checked = document.getElementById('checkall').checked;
            }
        }    
    </script>
</asp:Content>
