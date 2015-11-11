<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String ParentID = "KTTG_ChungTuCapThu_Duyet";
        String NamLamViec = Request.QueryString["NamLamViec"];
        String page = Request.QueryString["page"];
        String sNguoiDung = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(sNguoiDung);
        if (NamLamViec == null || NamLamViec == "")
        {
            NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
        }
        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }

        DataTable dt = KTCT_TienGui_ChungTuCapThuDuyetModels.Get_DanhSachChungTu(NamLamViec, sNguoiDung, CurrentPage, Globals.PageSize);

        double nums = KTCT_TienGui_ChungTuCapThuDuyetModels.Get_DanhSachChungTu_Count(NamLamViec, User.Identity.Name);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", "KTCT_DuyetCapThu", new { NamLamViec = NamLamViec, sNguoiDung = sNguoiDung, page = x }));

        int iDotMoi = 0;
        if (KTCT_TienGui_ChungTuCapThuDuyetModels.Get_Max_ChungTu(NamLamViec) != "")
        {
            iDotMoi = Convert.ToInt32(KTCT_TienGui_ChungTuCapThuDuyetModels.Get_Max_ChungTu(NamLamViec)) + 1;
        };
        Boolean bThemMoi = false;
        String iThemMoi = "";
        if (ViewData["bThemMoi"] != null)
        {
            bThemMoi = Convert.ToBoolean(ViewData["bThemMoi"]);
            if (bThemMoi)
                iThemMoi = "on";
        }

        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(User.Identity.Name);
        DataRow r = dtDonVi.NewRow();
        r["sTen"] = "--Chọn tất cả--";
        dtDonVi.Rows.InsertAt(r, 0);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        dtDonVi.Dispose();
        //tinh chat cap thu
        DataTable dtTinhChatCapThu = TinhChatCapThuModels.Get_dtTinhChatCapThu();
        SelectOptionList slTinhChatCapThu = new SelectOptionList(dtTinhChatCapThu, "iID_MaTinhChatCapThu", "sTen");
        DataRow R2 = dtTinhChatCapThu.NewRow();
        R2["sTen"] = "--Chọn tất cả--";
        dtTinhChatCapThu.Rows.InsertAt(R2, 0);
        dtTinhChatCapThu.Dispose();

        DataTable dtTaiKhoan = TaiKhoanModels.DT_DSTaiKhoan(true, "--- Tài khoản kế toán ---", "");
        SelectOptionList stTaiKhoan = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "sTen");
        dtTaiKhoan.Dispose();

        using (Html.BeginForm("Search", "KTCT_DuyetCapThu", new { ParentID = ParentID }))
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

        $(document).ready(function () {
            $('#pHeader1').click(function () {
                $('#dvContent1').slideToggle('slow');
            });
        });
        $(document).ready(function () {
            $("DIV.ContainerPanel1 > DIV.collapsePanelHeader1 > DIV.ArrowExpand1").toggle(
            function () {
                $(this).parent().next("div.Content1").show("slow");
                $(this).attr("class", "ArrowClose1");
            },
            function () {
                $(this).parent().next("div.Content1").hide("slow");
                $(this).attr("class", "ArrowExpand1");
            });
        }); 
                  
    </script>
    <div id="ContainerPanel" class="ContainerPanel">
        <div id="pHeader" class="collapsePanelHeader">
            <div id="dvHeaderText" class="HeaderContent" style="width: 90%;">
                <div style="width: 100%; float: left;">
                    <span>1. Thông tin tìm kiếm chứng từ</span>
                </div>
            </div>
            <div id="dvArrow" class="ArrowExpand">
            </div>
        </div>
        <div id="dvContent" class="Content" style="display: none">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" align="left" style="width: 100%;">
                        <div id="nhapform">
                            <div id="form2">
                                <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                                    <tr>
                                        <td style="width: 100%">
                                            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table_form2"
                                                id="tb_DotNganSach">
                                                <tr>
                                                    <td valign="top" style="width: 50%">
                                                        <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                                                            <tr>
                                                                <td class="td_form2_td1" style="width: 15%;">
                                                                    <div>
                                                                        <%=NgonNgu.LayXau("Đơn vị")%></div>
                                                                </td>
                                                                <td class="td_form2_td5">
                                                                    <div>
                                                                        <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, "", "iID_MaDonVi", "", "class=\"input1_2\"")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_form2_td1">
                                                                    <div>
                                                                        <%=NgonNgu.LayXau("Từ ngày")%></div>
                                                                </td>
                                                                <td class="td_form2_td5">
                                                                    <div>
                                                                        <%=MyHtmlHelper.DatePicker(ParentID, "", "dTuNgay", "", "class=\"input1_2\" style=\"height:18px;\" onblur=isDate(this);")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_form2_td1">
                                                                    <div>
                                                                        <%=NgonNgu.LayXau("Tài khoản nợ")%></div>
                                                                </td>
                                                                <td class="td_form2_td5">
                                                                    <div>
                                                                        <%=MyHtmlHelper.DropDownList(ParentID, stTaiKhoan, "", "iID_MaTaiKhoan_No", "", "class=\"input1_2\"")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_form2_td1">
                                                                    <div>
                                                                        <%=NgonNgu.LayXau("Đơn vị nợ")%></div>
                                                                </td>
                                                                <td class="td_form2_td5">
                                                                    <div>
                                                                        <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, "", "iID_MaDonVi_No", "", "class=\"input1_2\"")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top" style="width: 50%">
                                                        <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                                                            <tr>
                                                                <td class="td_form2_td1" style="width: 15%;">
                                                                    <div>
                                                                        <%=NgonNgu.LayXau("Số ủy nhiệm chi")%></div>
                                                                </td>
                                                                <td class="td_form2_td5">
                                                                    <div>
                                                                        <%=MyHtmlHelper.TextBox(ParentID, "", "sSoChungTu", "", "class=\"input1_2\" style=\"height:18px;\"")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_form2_td1">
                                                                    <div>
                                                                        <%=NgonNgu.LayXau("Đến ngày")%></div>
                                                                </td>
                                                                <td class="td_form2_td5">
                                                                    <div>
                                                                        <%=MyHtmlHelper.DatePicker(ParentID, "", "dDenNgay", "", "class=\"input1_2\" style=\"height:18px;\" onblur=isDate(this);")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_form2_td1">
                                                                    <div>
                                                                        <%=NgonNgu.LayXau("Tài khoản có")%></div>
                                                                </td>
                                                                <td class="td_form2_td5">
                                                                    <div>
                                                                        <%=MyHtmlHelper.DropDownList(ParentID, stTaiKhoan, "", "iID_MaTaiKhoan_Co", "", "class=\"input1_2\"")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="td_form2_td1">
                                                                    <div>
                                                                        <%=NgonNgu.LayXau("Đơn vị có")%></div>
                                                                </td>
                                                                <td class="td_form2_td5">
                                                                    <div>
                                                                        <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, "", "iID_MaDonVi_Co", "", "class=\"input1_2\"")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="td_form2_td1" colspan="2" align="right" width="100%">
                                                        <div style="float: right;">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td align="right" class="td_form2_td1">
                                                                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Tìm kiếm")%>" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%}
        DataTable dtChungTuCapPhat = CapPhat_ChungTuModels.GetDanhSachCapPhat(NamLamViec);
        SelectOptionList slChungTuCapPhat = new SelectOptionList(dtChungTuCapPhat, "iID_MaCapPhat", "TENHT");
        dtChungTuCapPhat.Dispose();
    %>
    <div id="ContainerPanel1" class="ContainerPanel">
        <div id="pHeader1" class="collapsePanelHeader">
            <div id="dvHeaderText1" class="HeaderContent" style="width: 90%;">
                <div style="width: 100%; float: left;">
                    <span>2. Danh sách chứng từ cấp phát</span>
                </div>
            </div>
            <div id="dvArrow1" class="ArrowExpand">
            </div>
        </div>
        <%
            using (Html.BeginForm("Edit", "KTCT_DuyetCapThu", new { ParentID = ParentID }))
            {
        %>
        <div id="dvContent1" class="Content" style="display: none">
            <div class="box_tong">
                <div>
                    <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                        <tr>
                            <td style="width: 100%">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table_form2"
                                    id="Table1">
                                    <tr>
                                        <td valign="top" style="width: 40%">
                                            <fieldset style="padding: 2px; border-radius: 5px; float: left; margin-right: 3px;
                                                width: 100%; margin-left: 10px; margin-top: 5px; height: 220px;">
                                                <legend class="p"><span style="font-size: 10pt; line-height: 16px; font-weight: bold;">
                                                    &nbsp;<%=NgonNgu.LayXau("Điều kiện tìm kiếm chứng từ cấp phát")%>&nbsp;</span></legend>
                                                <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                                                    <tr>
                                                        <td class="td_form2_td1" style="width: 18%;">
                                                            <div>
                                                                <%=NgonNgu.LayXau("Đơn vị")%></div>
                                                        </td>
                                                        <td class="td_form2_td5">
                                                            <div>
                                                                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, "", "iID_MaDonVi_CP", "", "class=\"input1_2\" onchange=\"ChonCapPhat()\"")%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_form2_td1">
                                                            <div>
                                                                <%=NgonNgu.LayXau("Từ ngày")%></div>
                                                        </td>
                                                        <td class="td_form2_td5">
                                                            <div>
                                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dTuNgay_CP", "", "class=\"input1_2\" style=\"height:18px;\" onchange=\"ChonCapPhat()\"")%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_form2_td1">
                                                            <div>
                                                                <%=NgonNgu.LayXau("Đến ngày")%></div>
                                                        </td>
                                                        <td class="td_form2_td5">
                                                            <div>
                                                                <%=MyHtmlHelper.DatePicker(ParentID, "", "dDenNgay_CP", "", "class=\"input1_2\" style=\"height:18px;\" onchange=\"ChonCapPhat()\"")%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_form2_td1" style="width: 15%;">
                                                            <div>
                                                                <%=NgonNgu.LayXau("Tính chất cấp thu")%></div>
                                                        </td>
                                                        <td class="td_form2_td5">
                                                            <div>
                                                                <%=MyHtmlHelper.DropDownList(ParentID, slTinhChatCapThu, "", "iID_MaTinhChatCapThu", "", "class=\"input1_2\" onchange=\"ChonCapPhat()\"")%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="td_form2_td1">
                                                            <div>
                                                                <%=NgonNgu.LayXau("Tất cả chứng từ đã duyệt")%></div>
                                                        </td>
                                                        <td class="td_form2_td5">
                                                            <div>
                                                                <%=MyHtmlHelper.CheckBox(ParentID, "", "chkTatCa", "", "class=\"input1_2\" onchange=\"ChonCapPhat()\"")%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                        <td valign="top" style="width: 50%">
                                            <fieldset style="padding: 2px; border-radius: 5px; float: left; margin-right: 10px;
                                                width: 100%; margin-left: 30px; margin-top: 5px;">
                                                <legend class="p"><span style="font-size: 10pt; line-height: 16px; font-weight: bold;">
                                                    &nbsp;<%=NgonNgu.LayXau("Danh sách chứng từ cấp phát để tạo UNC/DT")%>&nbsp;</span></legend>
                                                <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                                                    <tr>
                                                        <%--<td class="td_form2_td1" style="width: 20%;">
                                                        <div>
                                                            <%=NgonNgu.LayXau("Chứng từ cấp phát ")%></div>
                                                    </td>--%>
                                                        <td rowspan="5" id="<%= ParentID %>_tdChungTuCapPhat">
                                                            <div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <script type="text/javascript">
                                                            ChonCapPhat();
                                                            function ChonCapPhat() {
                                                                var iID_MaDonVi = document.getElementById("<%= ParentID %>_iID_MaDonVi_CP").value;
                                                                var dTuNgay = document.getElementById("<%= ParentID %>_vidTuNgay_CP").value;
                                                                var dDenNgay = document.getElementById("<%= ParentID %>_vidDenNgay_CP").value;
                                                                var iID_MaTinhChatCapThu = document.getElementById("<%= ParentID %>_iID_MaTinhChatCapThu").value;
                                                                var bchkTatCa = document.getElementById("<%= ParentID %>_chkTatCa").checked;
                                                                var chkTatCa ="";
                                                                if(bchkTatCa) chkTatCa="on";
                                                                jQuery.ajaxSetup({ cache: false });
                                                                var url = unescape('<%= Url.Action("get_DSChungTuCapPhat?iNamLamViec=#0&iID_MaDonVi=#1&TuNgay=#2&DenNgay=#3&iID_MaTinhChatCapThu=#4&chkTatCa=#5", "KTCT_DuyetCapThu") %>');
                                                                url = unescape(url.replace("#0", <%=NamLamViec %>));
                                                                url = unescape(url.replace("#1", iID_MaDonVi));
                                                                url = unescape(url.replace("#2", dTuNgay));
                                                                url = unescape(url.replace("#3", dDenNgay));
                                                                url = unescape(url.replace("#4", iID_MaTinhChatCapThu));
                                                                url = unescape(url.replace("#5", chkTatCa));
                                                                $.getJSON(url, function (data) {
                                                                    document.getElementById("<%= ParentID %>_tdChungTuCapPhat").innerHTML = data;
                                                                });
                                                            }                                           
                                                    </script>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1" colspan="2" align="right" width="100%">
                                            <div style="float: right;">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td align="right" class="td_form2_td1">
                                                            <input id="Tạo mới" type="submit" class="button" value="Tạo mới" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <%} %>
    </div>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width: 48%">
                        <span>3. Danh sách chứng từ duyệt cấp thu</span>
                    </td>
                    <td align="right" style="width: 50%;">
                        <%--  <%
                    String strThemMoi = Url.Action("Edit", "KTCT_DuyetCapThu");
                    using (Html.BeginForm("Edit", "KTCT_DuyetCapThu", new { ParentID = ParentID }))
                    {
                    %>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 85%"><%=MyHtmlHelper.DropDownList(ParentID, slChungTuCapPhat, "", "iID_MaCapPhat", "", "class=\"input1_2\"")%></td>
                            <td style="width: 15%"><input id="TaoMoi" type="submit" class="button" value="Tạo mới"/></td>
                        </tr>
                    </table>
                    <%} %>--%>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table class='mGrid'>
                <tr>
                    <th style="width: 3%" align="center">
                        STT
                    </th>
                    <th style="width: 5%;" align="center">
                        Năm
                    </th>
                    <th style="width: 8%;" align="center">
                        Số UNC/RDT
                    </th>
                     <th align="center">
                       Nội dung chứng từ
                    </th>
                    <th style="width: 7%;" align="center">
                        Tổng cấp
                    </th>
                    <th style="width: 7%;" align="center">
                        Tổng thu
                    </th>
                    <th style="width: 7%;" align="center">
                        Số tiền
                    </th>
                    <th style="width: 7%;" align="center">
                        Đã rút dự toán
                    </th>
                    <th style="width: 7%;" align="center">
                        Đã nhận UNC
                    </th>
                    <th style="width: 8%;" align="center">
                        Người tạo
                    </th>
                    <th style="width: 10%;" align="center">
                        Chi tiết chứng từ
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
                        String sClasstr = "";
                        int STT = i + 1;

                        if (i % 2 == 0) sClasstr = "alt";

                        String strRutDuToan = "";
                        strRutDuToan = CommonFunction.DinhDangSo(KTCT_TienGui_DuyetChungTuModels.CheckRutDuToan(Convert.ToString(R["iID_MaChungTu_Duyet"])));
                        String strUyNhiemChi = "";
                        strUyNhiemChi = CommonFunction.DinhDangSo(KTCT_TienGui_DuyetChungTuModels.CheckUyNhiemChi(Convert.ToString(R["iID_MaChungTu_Duyet"])));

                        String strDuyet = "";
                        String strChiTiet = "";
                        String strEdit = "";
                        String strDelete = "";

                        String sTongCap = "0", sTongThu = "0", sSoTien = "0";
                        sTongCap = CommonFunction.DinhDangSo(Convert.ToString(R["rTongCap"]));
                        sTongThu = CommonFunction.DinhDangSo(Convert.ToString(R["rTongThu"]));
                        sSoTien = CommonFunction.DinhDangSo(Convert.ToString(R["rSoTien"]));

                        strDuyet = MyHtmlHelper.ActionLink(Url.Action("Duyet", "KTCT_DuyetCapThu", new { iID_MaChungTu_Duyet = R["iID_MaChungTu_Duyet"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                        strChiTiet = MyHtmlHelper.ActionLink(Url.Action("Detail", "KTCT_DuyetCapThu", new { iID_MaChungTu_Duyet = R["iID_MaChungTu_Duyet"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Edit", "");

                        strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "KTCT_DuyetCapThu", new { iID_MaChungTu_Duyet = R["iID_MaChungTu_Duyet"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                        if (strRutDuToan == "" && strUyNhiemChi == "")
                        {
                            strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "KTCT_DuyetCapThu", new { iID_MaChungTu_Duyet = R["iID_MaChungTu_Duyet"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                        }

                %>
                <tr class='<%=sClasstr %>'>
                    <td align="center">
                        <%=STT%>
                    </td>
                    <td align="center">
                        <b>
                            <%=R["iNamLamViec"]%></b>
                    </td>
                    <td align="center">
                        <b>
                            <%=MyHtmlHelper.ActionLink(Url.Action("Detail", "KTCT_DuyetCapThu", new { iID_MaChungTu_Duyet = R["iID_MaChungTu_Duyet"].ToString() }).ToString(), R["sSoChungTu"].ToString(), "Detail","")%></b>
                    </td>
                    <td>
                      <%=R["sNoiDung"]%>
                    </td>
                    <td align="center">
                        <b>
                            <%=sTongCap%></b>
                    </td>
                    <td align="center">
                        <b>
                            <%=sTongThu%></b>
                    </td>
                    <td align="center">
                        <b>
                            <%=sSoTien%></b>
                    </td>
                    <td align="center">
                        <b>
                            <%=strRutDuToan%></b>
                    </td>
                    <td align="center">
                        <b>
                            <%=strUyNhiemChi%></b>
                    </td>
                    <td align="center">
                        <b>
                            <%=R["sID_MaNguoiDungTao"]%></b>
                    </td>
                    <td align="center">
                        <%=strChiTiet%>
                    </td>
                    <td align="center">
                        <%=strDelete%>
                    </td>
                </tr>
                <%} %>
                <tr class="pgr">
                    <td colspan="12" align="right">
                        <%=strPhanTrang%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%dtCauHinh.Dispose();
      dt.Dispose(); %>
    <script type="text/javascript">
        function ChonallDV(value) {
            $("input:checkbox[check-group='MaCapPhat']").each(function (i) {
                this.checked = value;
            });
        }    
    </script>
</asp:Content>
