<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String ParentID = "Edit";
        String dNgayLap = Request.QueryString["dNgayLap"];

        if (dNgayLap == null) dNgayLap = "";

        DataTable dtDuAn = QLDA_DanhMucDuAnModels.ddl_DanhMucDuAn(true);
        SelectOptionList slDuAn = new SelectOptionList(dtDuAn, "iID_MaDanhMucDuAn", "TenHT");
        dtDuAn.Dispose();

        String MaND = User.Identity.Name;
        String iDot = Request.QueryString["iDot"];
        String dTuNgay = Request.QueryString["dTuNgay"];
        String dDenNgay = Request.QueryString["dDenNgay"];
        String sNguoiDung = Request.QueryString["sNguoiDung"];

        String page = Request.QueryString["page"];

        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        sNguoiDung = User.Identity.Name;
        DataTable dt = QLDA_DuToan_NamModels.Get_DanhSach_DuToanNam_QuyetDinh(iDot, dTuNgay, dDenNgay, sNguoiDung, CurrentPage, Globals.PageSize);

        double nums = QLDA_DuToan_NamModels.Get_DanhSach_DuToanNam_QuyetDinh_Count(iDot, dTuNgay, dDenNgay, sNguoiDung);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", "QLDA_DuToan_Nam", new { iDot = iDot, TuNgay = dTuNgay, DenNgay = dDenNgay, sNguoiDung = sNguoiDung, page = x }));
        String strThemMoi = Url.Action("Edit", "QLDA_DuToan_Nam");
    
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%>
    </b> </div> </td>
    <td align="left">
        <div style="padding-bottom: 5px; color: #ec3237;">
            <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
        </div>
    </td>
    <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
        padding-right: 20px;">
        <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
    </td>
    </tr> </table>
    <div id="ContainerPanel" class="ContainerPanel">
        <div id="pHeader" class="collapsePanelHeader">
            <div id="dvHeaderText" class="HeaderContent" style="width: 80%;">
                <div style="width: 100%; float: left;">
                    <span>
                        <%=NgonNgu.LayXau("Tìm kiếm thông tin")%></span>
                </div>
            </div>
        </div>
        <div id="dvContent" class="Content">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" align="left" style="width: 50%;">
                        <div id="nhapform">
                            <div id="form2">
                                <%
                                    using (Html.BeginForm("SearchSubmit", "QLDA_DuToan_Nam", new { ParentID = ParentID }))
                                    {       
                                %>
                                <table cellpadding="0" cellspacing="0" border="0" class="table_form2" width="100%">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;">
                                            <div>
                                                <b>Tìm từ ngày</b></div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dTuNgay", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;">
                                            <div>
                                                <b>Tìm đến ngày</b></div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dDenNgay", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td5" colspan="4">
                                            <div style="text-align: right; float: right; width: 100%">
                                                <input type="submit" class="button4" value="Tìm kiếm" style="float: right; margin-left: 10px;" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1" align="right" colspan="4">
                                        </td>
                                    </tr>
                                </table>
                                <%} %>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <br />
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách dự toán năm</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input id="TaoMoi" type="button" class="button_title" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 5%;" align="center">
                    Ngày lập
                </th>
                <th style="width: 5%;" align="center">
                    Đợt
                </th>
                <th style="width: 30%;" align="center">
                    Nội dung
                </th>
                <th style="width: 10%;" align="center">
                    Người tạo
                </th>
                <th style="width: 5%;" align="center">
                    Sửa
                </th>
                <th style="width: 5%;" align="center">
                    Xóa
                </th>
            </tr>
            <%
                int i;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    String classtr = "";
                    String sTrangThai = "";
                    String sClasstr = "";

                    //Ngày tạo tổng đầu tư 

                    String dNgayLapNew = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayLap"]));
                    if (i % 2 == 0) sClasstr = "alt";
                    String strEdit = "";
                    String strDelete = "";
                    strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "QLDA_DuToan_Nam", new { iID_MaDuToanNam_QuyetDinh = R["iID_MaDuToanNam_QuyetDinh"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "QLDA_DuToan_Nam", new { iID_MaDuToanNam_QuyetDinh = R["iID_MaDuToanNam_QuyetDinh"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                    int Count = QLDA_DuToan_NamModels.CheckExits_DuToan(R["iID_MaDuToanNam_QuyetDinh"]);
            %>
            <tr <%=sClasstr %>>
                <td align="center">
                    <%=R["rownum"]%>
                </td>
                <td align="center">
                    <b>
                        <%=MyHtmlHelper.ActionLink(Url.Action("Detail", "QLDA_DuToan_Nam", new { iID_MaDuToanNam_QuyetDinh = R["iID_MaDuToanNam_QuyetDinh"].ToString(), dNgayLap = dNgayLapNew }).ToString(), dNgayLapNew, "Detail", "")%></b>
                </td>
                <td align="center">
                    <%=R["iDot"]%>
                </td>
                <td align="left">
                    <%=R["sNoiDung"]%>
                </td>
                <td align="left">
                    <%=R["sID_MaNguoiDungTao"]%>
                </td>
                <td align="center">
                    <%=strEdit%>
                </td>
                <td align="center">
                    <% if (Count == 0)
                       { %>
                    <%=strDelete%>
                    <%  
} %>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="7" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
