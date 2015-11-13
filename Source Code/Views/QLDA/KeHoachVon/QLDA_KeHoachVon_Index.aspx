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
        String dNgayKeHoachVon = Request.QueryString["dNgayKeHoachVon"];
        String iLoaiKeHoachVon = Request.QueryString["iLoaiKeHoachVon"];
        String sNguoiDung = Request.QueryString["sNguoiDung"];

        // if (dNgayKeHoachVon == null) dNgayKeHoachVon = CommonFunction.LayXauNgay(DateTime.Now);
        if (iLoaiKeHoachVon == null) iLoaiKeHoachVon = "";
        String sSoQuyetDinh = Request.QueryString["sSoQuyetDinh"];
        String TuNgayQD = Request.QueryString["TuNgayQD"];
        String DenNgayQD = Request.QueryString["DenNgayQD"];
        String TuNgay = Request.QueryString["TuNgay"];
        String DenNgay = Request.QueryString["DenNgay"];
        DataTable dtDuAn = QLDA_DanhMucDuAnModels.ddl_DanhMucDuAn(true);
        SelectOptionList slDuAn = new SelectOptionList(dtDuAn, "iID_MaDanhMucDuAn", "TenHT");
        dtDuAn.Dispose();

        DataTable dtLoaiKeHoachVon = QLDA_KeHoachVonModels.DT_LoaiKeHoachVon();
        SelectOptionList slLoaiKeHoachVon = new SelectOptionList(dtLoaiKeHoachVon, "iID_MaLoaiKeHoachVon", "sTen");
        dtLoaiKeHoachVon.Dispose();

        NameValueCollection data = QLDA_KeHoachVonModels.LayThongTin(iLoaiKeHoachVon, dNgayKeHoachVon);


        String page = Request.QueryString["page"];

        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        sNguoiDung = User.Identity.Name;
        DataTable dt = QLDA_KeHoachVonModels.Get_DanhSach_KHV_QuyetDinh(sSoQuyetDinh, iLoaiKeHoachVon, TuNgayQD,
                                                                        DenNgayQD, TuNgay, DenNgay, sNguoiDung,
                                                                        CurrentPage, Globals.PageSize);

        double nums = QLDA_KeHoachVonModels.Get_DanhSach_KHV_QuyetDinh_Count(sSoQuyetDinh, iLoaiKeHoachVon, TuNgayQD,
                                                                        DenNgayQD, TuNgay, DenNgay, sNguoiDung);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", "QLDA_KeHoachVon", new
        {
            sSoQuyetDinh = sSoQuyetDinh,
            iLoaiKeHoachVon = iLoaiKeHoachVon,
            TuNgayQD = TuNgayQD,
            DenNgayQD = DenNgayQD,
            TuNgay = TuNgay,
            DenNgay = DenNgay,
            sNguoiDung = sNguoiDung,
            page = x
        }));
        String strThemMoi = Url.Action("Edit", "QLDA_KeHoachVon");
    
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
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#pHeader').click(function () {
                $('#dvContent').slideToggle('slow');
            });
        });
        $(document).ready(function () {
            $("DIV.ContainerPanel > DIV.collapsePanelHeader > DIV.ArrowExpand").toggle(
            function () {
                $(this).parent().next("div.Content").show("slow");
                $(this).attr("class", "ArrowClose");
            },
            function () {
                $(this).parent().next("div.Content").hide("slow");
                $(this).attr("class", "ArrowExpand");
            });
        });            
    </script>
    <div id="ContainerPanel" class="ContainerPanel">
        <div id="pHeader" class="collapsePanelHeader">
            <div id="dvHeaderText" class="HeaderContent" style="width: 80%;">
                <div style="width: 100%; float: left;">
                    <span>
                        <%=NgonNgu.LayXau("Tìm kiếm thông tin Kế hoạch vốn năm")%></span>
                </div>
            </div>
            <%--  <div id="dvArrow" class="ArrowExpand">
            </div>--%>
        </div>
        <div id="dvContent" class="Content">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" align="left" style="width: 50%;">
                        <div id="nhapform">
                            <div id="form2">
                                <%
                                    using (Html.BeginForm("SearchSubmit", "QLDA_KeHoachVon", new { ParentID = ParentID }))
                                    {       
                                %>
                                <table cellpadding="0" cellspacing="0" border="0" class="table_form2" width="100%">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;">
                                            <div>
                                                <b>Số quyết định</b>
                                            </div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.TextBox(ParentID, "", "sSoQuyetDinh", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;">
                                            <div>
                                                <b>Loại kế hoạch vốn</b></div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiKeHoachVon, "", "iLoaiKeHoachVon_Search", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;">
                                            <div>
                                                <b>Tìm từ quyết định</b></div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dTuNgayQD", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;">
                                            <div>
                                                <b>Tìm đến ngày quyết định</b></div>
                                        </td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dDenNgayQD", "", "class=\"input1_2\"")%>
                                            </div>
                                        </td>
                                    </tr>
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
                                        <td class="td_form2_td5" colspan="2">
                                        </td>
                                        <td class="td_form2_td5" colspan="2">
                                            <div style="text-align: left; width: 100%">
                                                <input type="submit" class="button4" value="Tìm kiếm" style="margin-left: 10px;" />
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
                        <span>Danh sách Kế hoạch vốn năm </span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input id="TaoMoi" type="button" class="button_title" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 50px;" align="center">
                    STT
                </th>
                <th style="width: 150px;" align="center">
                    Số quyết định
                </th>
                <th style="width:100px;" align="center">
                    Ngày quyết định
                </th>
                <th style="width: 100px;" align="center">
                    Ngày lập
                </th>
                 <th style="width: 250px;" align="center">
                   Loại kế hoạch vốn
                </th>
                <th  align="center">
                    Nội dung
                </th>
                <th style="width: 100px;" align="center">
                    Người tạo
                </th>
                <th style="width: 50px;" align="center">
                    Sửa
                </th>
                <th style="width: 50px;" align="center">
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

                    String dNgayKeHoachVonNew = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayKeHoachVon"]));
                    String dNgayQDNew = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayQD"]));
                    String iLoaiKeHoachVonNew = Convert.ToString(R["iLoaiKeHoachVon"]);
                    if (i % 2 == 0) sClasstr = "alt";
                    String strEdit = "";
                    String strDelete = "";
                    strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "QLDA_KeHoachVon", new { iID_KeHoachVon_QuyetDinh = R["iID_KeHoachVon_QuyetDinh"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "QLDA_KeHoachVon", new { iID_KeHoachVon_QuyetDinh = R["iID_KeHoachVon_QuyetDinh"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                    int Count = QLDA_KeHoachVonModels.CheckExits_DuToan(R["iID_KeHoachVon_QuyetDinh"]);
            %>
            <tr <%=sClasstr %>>
                <td align="center">
                    <%=R["rownum"]%>
                </td>
                <td align="center">
                    <b>
                        <%=MyHtmlHelper.ActionLink(Url.Action("Detail", "QLDA_KeHoachVon", new { iID_KeHoachVon_QuyetDinh = R["iID_KeHoachVon_QuyetDinh"].ToString(), dNgayKeHoachVon = dNgayKeHoachVonNew, iLoaiKeHoachVon = iLoaiKeHoachVonNew }).ToString(), R["sSoQuyetDinh"].ToString(), "Detail", "")%></b>
                </td>
                <td align="center">
                    <%=dNgayQDNew%>
                </td>
                <td align="center">
                    <%=dNgayKeHoachVonNew%>
                </td>
                 <td align="left">
                    <%=R["sTen"]%>
                </td>
                <td align="left">
                    <%=R["sNoiDung"]%>
                </td>
                <td align="left">
                    <%=R["sID_MaNguoiDungTao"]%>
                </td>
                <td align="center">
                   <% if (Count == 0)
                       { %>
                    <%=strEdit%>
                      <%  
                       } %>
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
                <td colspan="9" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
