<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers.KeToanTongHop" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String MaND = User.Identity.Name;

        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        int iNam = DateTime.Now.Year;
        int iThang = DateTime.Now.Month;
        if (dtCauHinh.Rows.Count > 0)
        {
            iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
            iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
        }
        dtCauHinh.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang_CoThangKhong();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        DataTable dtNam = DanhMucModels.DT_Nam(false);
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        DataTable dtTrangThai = HamChung.GetTrangThai(PhanHeModels.iID_MaPhanHeKeToanTongHop, LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop), true, "Tất cả");
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();
        String iTrangThai =
            Convert.ToString(LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DaDuyet(PhanHeModels.iID_MaPhanHeKeToanTongHop));
        String URL = Url.Action("SoDoLuong", "KeToanTongHop");
        //danh sach 
        String page = Request.QueryString["page"];
        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }

        DataTable dt = ConvertToFoxController.Get_dtTaiLieu(CurrentPage, Globals.PageSize);

        double nums = ConvertToFoxController.Get_CountdtTaiLieu();
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));
        using (Html.BeginForm("EditSubmit", "ConvertToFox"))
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
                        <span>
                            <%=NgonNgu.LayXau("Chuyển dữ liệu sang chương trình cũ")%></span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tháng chuyển số liệu</b> <span style="color: red;">(*)</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList("CauHinh", slThang, Convert.ToString(iThang), "iThangLamViec", "", "style=\"width:100px;\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Năm chuyển số liệu</b><span style="color: red;">(*)</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList("CauHinh", slNam, Convert.ToString(iNam), "iNamLamViec", "", " style=\"width:100px;\"")%>
                            </div>
                        </td>
                    </tr>
                     <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Trạng thái dữ liệu</b><span style="color: red;">(*)</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList("CauHinh", slTrangThai, iTrangThai, "TrangThai", "", "class=\"input1_2\" style=\"width: 100px;\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">
                                            &nbsp;
                                        </td>
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <input type="submit" class="button" id="btnLuu" value="Chuyển" />
                                        </td>
                                        <td width="5px">
                                            &nbsp;
                                        </td>
                                        <td class="td_form2_td5">
                                            <input class="button" type="button" value="Hủy" onclick="Huy()" />
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
    <%
}       
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách dữ liệu</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th  align="center">
                    Nội dung
                </th>
                <th style="width: 7%;" align="center">
                    Tháng
                </th>
                <th style="width: 7%;" align="center">
                    Năm
                </th>
                <th style="width: 10%;" align="center">
                    Ngày tạo
                </th>
                <th style="width: 5%;" align="center">
                    Tải về
                </th>
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
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=R["rownum"]%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(R["sTenTaiLieu"])%>
                </td>
                <td align="center">
                    <%=HttpUtility.HtmlEncode(R["iThang"])%>
                </td>
                <td align="center">
                    <%=HttpUtility.HtmlEncode(R["iNam"])%>
                </td>
                <td align="center">
                    <%=HttpUtility.HtmlEncode(String.Format("{0:dd/MM/yyyy}", R["dNgayTao"]))%>
                </td>
                <td align="center">
                   <%=MyHtmlHelper.ActionLink(Url.Action("Download", "ConvertToFox", new { MaTaiLieu = dt.Rows[i]["iID_MaTaiLieu"] }), "<img src='../Content/Themes/images/Download.jpg' alt='' style='height: 25px;' />")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "ConvertToFox", new { iID_MaTaiLieu = R["iID_MaTaiLieu"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
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
    <script type="text/javascript">
        function Huy() {
            window.location.href = '<%=URL %>';
        }
        $(document).ready(function () {
            //Hide the div tag when page is loading
            $('#dvText').hide();

            //For Show the div or any HTML element
            $("#btnLuu").click(function () {
                $('#dvText').show();
                $('body').append('<div id="fade"></div>'); //Add the fade layer to bottom of the body tag.
                $('#fade').css({ 'filter': 'alpha(opacity=40)' }).fadeIn(); //Fade in the fade layer 
            });

            //For hide the div or any HTML element
            $("#aHide").click(function () {
                $('#dvText').hide();
            });

            $(window).resize(function () {
                $('.popup_block').css({
                    position: 'absolute',
                    left: ($(window).width() - $('.popup_block').outerWidth()) / 2,
                    top: ($(window).height() - $('.popup_block').outerHeight()) / 2
                });
            });
            // To initially run the function:
            $(window).resize();
            //Fade in Background
        });                                 
    </script>
    <div id="dvText" class="popup_block">
        <img src="../../../Content/ajax-loader.gif" /><br />
        <p>
            Hệ thống đang thực hiện yêu cầu...</p>
    </div>
</asp:Content>
