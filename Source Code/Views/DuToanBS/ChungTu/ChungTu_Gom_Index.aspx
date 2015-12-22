<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models.DuToanBS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        string MaND = User.Identity.Name;
        string ParentID = "DTBS_ChungTu";
        string ChiNganSach = Request.QueryString["ChiNganSach"];
        string iID_MaChungTu_TLTHCuc = Request.QueryString["iID_MaChungTu"];
        string MaDotNganSach = Convert.ToString(ViewData["MaDotNganSach"]);
        string iLoai = Request.QueryString["iLoai"];
        string iSoChungTu = Request.QueryString["SoChungTu"];
        string sTuNgay = Request.QueryString["TuNgay"];
        string sDenNgay = Request.QueryString["DenNgay"];
        string sLNS_TK = Request.QueryString["sLNS_TK"];
        string iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
        string page = Request.QueryString["page"];

        int CurrentPage = 1;
        if (HamChung.isDate(sTuNgay) == false) sTuNgay = "";
        if (HamChung.isDate(sDenNgay) == false) sDenNgay = "";

        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";
        string iThemMoi = "";
        if (ViewData["bThemMoi"] != null)
        {
            if (Convert.ToBoolean(ViewData["bThemMoi"]))
            { 
                iThemMoi = "on";
            }
        }

        string dNgayChungTu;
        if (ViewData["dNgayChungTu"] != null)
        {
            dNgayChungTu = Convert.ToString(ViewData["dNgayChungTu"]);
        }
        else
        {
            dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);
        }
        DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(DuToanModels.iID_MaPhanHe);

        //dt Trang thai.
        DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(DuToanModels.iID_MaPhanHe, MaND);
        dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
        dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
        dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
        
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();
        
        if (!String.IsNullOrEmpty(page))
        {
            CurrentPage = Convert.ToInt32(page);
        }
        //kiem tra nguoi dung co phan tro ly tong hop
        bool check = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);
        bool bTrolyTHCuc = LuongCongViecModel.KiemTra_TroLyTongHopCuc(MaND);
        bool CheckNDtao = false;
        if (bTrolyTHCuc)
        {
            CheckNDtao = false;
        }
        else if (check)
        {
            CheckNDtao = true;
        }

        //DataTable dtChungTuDuyet = DuToanBS_ChungTuModels.getDanhSachChungTu_TongHopDuyet(MaND, sLNS1);
        DataTable dtChungTuDV = DuToanBSChungTuModels.LayDanhSachChungDeGomTLTH(MaND);
        
        //Lấy danh sách chứng từ TLTH page hiện tại
        DataTable dt = DuToanBSChungTuModels.LayDanhSachChungTuTLTH(iID_MaChungTu_TLTHCuc, MaND, CheckNDtao, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, CurrentPage, Globals.PageSize);
        
        //Lấy số lượng tất cả chứng từ TLTH
        double nums = DuToanBSChungTuModels.LayDanhSachChungTuTLTH(iID_MaChungTu_TLTHCuc, MaND, CheckNDtao, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet).Rows.Count;
        
        //Phân trang
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {iLoai=1, SoChungTu = iSoChungTu, TuNgay = sTuNgay, DenNgay = sDenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, page = x }));
        int soCot = 1;
    %>
    <%--Liên kết nhanh--%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold; padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>

    <%--Tìm kiếm --%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Tìm kiếm đợt dự toán</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <%
                    using (Html.BeginForm("TimKiemChungTu", "DuToanBSChungTu", new { parentID = ParentID, iLoai = iLoai }))
                    {       
                %>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="td_form2_td1" style="width: 10%">
                            <div><b>Đợt dự toán từ ngày</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, sTuNgay, "dTuNgay", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                            <div><b>Đến ngày</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, sDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 10%">
                            <div><b>Trạng thái</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 10%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "ddlIDMaTrangThai", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td5" style="width: 20%">
                            <input type="submit" class="button" value="Tìm kiếm" />
                        </td>
                    </tr>
                </table>
                <%} %>
            </div>
        </div>
    </div>
    <br />

    <%--Thêm đợt--%>
    <% if (check)
       { %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Thêm một đợt cấp mới </span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Div1">
            <div id="Div2">
                <%
                    using (Html.BeginForm("ThemSuaChungTuTLTH", "DuToanBSChungTu", new { parentID = ParentID }))
                    {
                %>
                <%= Html.Hidden(ParentID + "_DuLieuMoi", 1) %>
                <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                    <tr>
                        <td style="width: 80%">
                            <table cellpadding="0" cellspacing="0" width="50%" class="table_form2">
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div>
                                            <b><%= NgonNgu.LayXau("Bổ sung đợt mới") %></b>
                                        </div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.CheckBox(ParentID, iThemMoi, "iThemMoi", "", "onclick=\"CheckThemMoi()\"") %>
                                            <span style="color: brown;">
                                                (Trường hợp bổ sung đợt mới, đánh dấu chọn "Bổ sung đợt mới". Nếu không chọn đợt bổ sung dưới lưới) 
                                            </span>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" border="0" width="50%" class="table_form2" id="tb_DotNganSach">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div><b>Chọn đợt</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                    <div style="overflow: scroll; width: 50%; height: 300px">
                                        <table class="mGrid" style="width: 100%">
                                            <tr>
                                                <th align="center" style="width: 40px;">
                                                    <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
                                                </th>
                                                <% for (int c = 0; c < soCot * 2 - 1; c++)
                                                   {%>
                                                <th>
                                                </th>
                                                <% } %>
                                            </tr>
                                        <%
                                            string strTen = "";
                                            string strMa = "";
                                        for (int i = 0; i < dtChungTuDV.Rows.Count; i++)
                                        {
                                        %>
                                            <tr>
                                            <% for (int c = 0; c < soCot; c++)
                                                {
                                                    if (i + c < dtChungTuDV.Rows.Count)
                                                    {
                                                        if (dtChungTuDV.Rows[i + c]["sDSLNS"].ToString().Length >= 7)
                                                        {
                                                            strTen = dtChungTuDV.Rows[i + c]["sDSLNS"].ToString().Substring(0, 7);
                                                        }
                                                        else
                                                        {
                                                            strTen = dtChungTuDV.Rows[i + c]["sDSLNS"].ToString().Substring(0, 3);
                                                        }
                                                        strTen = Convert.ToString(strTen + '-' +
                                                                CommonFunction.LayXauNgay(
                                                                Convert.ToDateTime(dtChungTuDV.Rows[i + c]["dNgayChungTu"])) + '-' +
                                                                Convert.ToString(dtChungTuDV.Rows[i + c]["sID_MaNguoiDungtao"]) + '-' +
                                                                Convert.ToString(dtChungTuDV.Rows[i + c]["sNoiDung"]) + '-' +
                                                                Convert.ToString(dtChungTuDV.Rows[i + c]["sLyDo"]));
                                                        strMa = Convert.ToString(dtChungTuDV.Rows[i + c]["iID_MaChungTu"]);
                                            %>
                                                <td align="center" style="width: 40px;">
                                                    <input type="checkbox" value="<%= strMa %>" check-group="ChungTu"
                                                        id="iID_MaChungTu" name="iID_MaChungTu" />
                                                </td>
                                                <td align="left">
                                                    <%= strTen%>
                                                </td>
                                                <% } %>
                                                <% } %>
                                            </tr>
                                            <% }%>
                                        </table>
                                        </div>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                            &nbsp;
                                    </td>
                                    <td class="td_form2_td5">
                                        <div><%= Html.ValidationMessage(ParentID + "_" + "err_ChungTu")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div><b>Ngày tháng</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div style="width: 200px; float: left;">
                                            <%= MyHtmlHelper.DatePicker(ParentID, dNgayChungTu, "dNgayChungTu", "", "class=\"input1_2\"  style=\"width: 200px;\"") %>
                                            
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        &nbsp;
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu") %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Nội dung đợt</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div style="width: 550px; float: left">
                                            <%= MyHtmlHelper.TextArea(ParentID, "", "sNoiDung", "", "class=\"input1_2\" style=\"height: 100px; resize: none;\"") %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div>
                                        </div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <input type="submit" class="button" id="Submit1" value="<%= NgonNgu.LayXau("Thêm mới") %>" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <% } %>
            </div>
        </div>
    </div>
    <% } %>

    <%--Danh sách chứng từ gom--%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách đợt dự toán tổng hợp</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid" id="<%= ParentID %>_thList">
            <tr>
                <th style="width: 2%;" align="center">
                    STT
                </th>
                <th style="width: 15%;" align="center">
                    Đợt dự toán
                </th>
                <th align="center">
                    Nội dung đợt dự toán
                </th>
                <th style="width: 5%;" align="center">
                    Chi tiết
                </th>
                <th style="width: 5%;" align="center">
                    Sửa
                </th>
                <th style="width: 5%;" align="center">
                    Xóa
                </th>
                <th style="width: 5%;" align="center">
                    Người tạo
                </th>
                <th style="width: 15%;" align="center">
                    Trạng thái
                </th>
                <th style="width: 15%;" align="center">
                    Báo cáo kiểm
                </th>
                <th style="width: 5%;" align="center">
                    Duyệt
                </th>
                <th style="width: 5%;" align="center">
                    Từ chối
                </th>
            </tr>
            <%
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    int STT = i + 1;
                    String NgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
                    String sTrangThai = "";
                    String strColor = "";
                    for (int j = 0; j < dtTrangThai_All.Rows.Count; j++)
                    {
                        if (Convert.ToString(R["iID_MaTrangThaiDuyet"]) == Convert.ToString(dtTrangThai_All.Rows[j]["iID_MaTrangThaiDuyet"]))
                        {
                            sTrangThai = Convert.ToString(dtTrangThai_All.Rows[j]["sTen"]);
                            strColor = String.Format("style='background-color: {0}; background-repeat: repeat;'", dtTrangThai_All.Rows[j]["sMauSac"]);
                            break;
                        }
                    }
                    String strEdit = "";
                    String strDelete = "";
                    int maTrangThaiDVTrinhDuyet = LuongCongViecModel.Get_iID_MaTrangThaiDuyetMoi(PhanHeModels.iID_MaPhanHeDuToan);
                    maTrangThaiDVTrinhDuyet = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(maTrangThaiDVTrinhDuyet);

                    int maTrangThaiTHCucTuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TrinhDuyet(maTrangThaiDVTrinhDuyet);
                    maTrangThaiTHCucTuChoi = LuongCongViecModel.Luong_iID_MaTrangThaiDuyet_TuChoi(maTrangThaiTHCucTuChoi);
                    if (check && (Convert.ToInt32(R["iID_MaTrangThaiDuyet"]) == maTrangThaiDVTrinhDuyet) ||
                        Convert.ToInt32(R["iID_MaTrangThaiDuyet"]) ==maTrangThaiTHCucTuChoi)
                    {
                        strEdit = MyHtmlHelper.ActionLink(Url.Action("SuaChungTuTLTH", "DuToanBSChungTu", new { iID_MaChungTu = R["iID_MaChungTu_TLTH"]}).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "", "title=\"Sửa chứng từ\"");
                        strDelete = MyHtmlHelper.ActionLink(Url.Action("XoaChungTuTLTH", "DuToanBSChungTu", new { iID_MaChungTu = R["iID_MaChungTu_TLTH"], MaDotNganSach = MaDotNganSach, ChiNganSach = ChiNganSach }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "", "title=\"Xóa chứng từ\"");
                    }
            %>
            <tr <%=strColor %>>
                <td align="center">
                    <b><%=R["rownum"]%></b>
                </td>
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBSChungTu", new { ChiNganSach = ChiNganSach, MaDotNganSach = MaDotNganSach, bTLTH = 1, iID_MaChungTu = R["iID_MaChungTu_TLTH"] }).ToString(), "Đợt ngày: " + NgayChungTu, "Detail", "")%></b>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(dt.Rows[i]["sNoiDung"])%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBSChungTu", new { iID_MaChungTu = R["iID_MaChungTu_TLTH"], bTLTH = 1 }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"")%>
                </td>
                <td align="center">
                    <%=strEdit%>
                </td>
                <td align="center">
                    <%=strDelete%>
                </td>
                <td align="center">
                    <%=R["sID_MaNguoiDungTao"]%>
                </td>
                <td align="center">
                    <%=sTrangThai%>
                </td>
                <td align="center">
                    <div onclick="OnInit_CT_NEW(1000, 'Báo cáo in kiểm');">
                        <%= Ajax.ActionLink("Báo cáo", "Index", "NhapNhanh", new { id = "DUTOANBS_BAOCAOINKIEM_GOM", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = R["iID_MaChungTu_TLTH"], iID_MaChungTu_TLTHCuc = iID_MaChungTu_TLTHCuc }, new AjaxOptions { }, new { @class = "buttonReport" })%>
                    </div>
                </td>
                <td align="center">
                    <div onclick="OnInit_CT_NEW(500, 'Duyệt chứng từ');">
                        <%= Ajax.ActionLink("Trình Duyệt", "Index", "NhapNhanh", new { id = "DUTOANBS_TRINHDUYETCHUNGTU_GOM", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = R["iID_MaChungTu_TLTH"], iID_MaChungTu_TLTHCuc = iID_MaChungTu_TLTHCuc }, new AjaxOptions { }, new { @class = "buttonDuyet" })%>
                    </div>
                </td>
                <td align="center">
                    <div onclick="OnInit_CT_NEW(500, 'Từ chối chứng từ');">
                        <%= Ajax.ActionLink("Từ Chối", "Index", "NhapNhanh", new { id = "DUTOANBS_TUCHOICHUNGTU_GOM", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = R["iID_MaChungTu_TLTH"], iID_MaChungTu_TLTHCuc = iID_MaChungTu_TLTHCuc }, new AjaxOptions { }, new { @class = "button123" })%>
                    </div>
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
    <div id="idDialog" style="display: none;">
    </div>
    <%  
        dt.Dispose();
        dtTrangThai.Dispose();
    %>
    <script type="text/javascript">
        $(function () {
            $('.button123').text('');
        });
        $(function () {
            $('.buttonDuyet').text('');
        });
        $(function () {
            $('.buttonReport').text('');
        });
        CheckThemMoi();
        function CheckThemMoi() {
            var isChecked = document.getElementById("<%= ParentID %>_iThemMoi").checked;
            if (isChecked == true) {
                document.getElementById('tb_DotNganSach').style.display = ''
            } else {
                document.getElementById('tb_DotNganSach').style.display = 'none'
            }
        }
        //Hiện thị dialog
        function OnInit_CT_NEW(value, title) {
            $("#idDialog").dialog("destroy");
            document.getElementById("idDialog").title = title;
            document.getElementById("idDialog").innerHTML = "";
            $("#idDialog").dialog({
                resizeable: false,
                draggable: true,
                width: value,
                modal: true,
                open: function (event, ui) {
                    $(event.target).parent().css('position', 'fixed');
                    $(event.target).parent().css('top', '100px');
                }
            });
        }
        function OnLoad_CT(v) {
            document.getElementById("idDialog").innerHTML = v;
        }
        function funcTest(value) {
            $("#dialogTest").dialog({
                resizeable: false,
                draggable: true,
                width: value,
                modal: true,
                open: function (event, ui) {
                    $("#dialogTest div.label").html(value);
                    $("#dialogTest #txtCode").val(value);
                }
            });
            return false;
        }
        function saveCode() {
            alert($("#dialogTest #txtCode").val());
        }
    </script>
    <script type="text/javascript">
        function CheckAll(value) {
            $("input:checkbox[check-group='ChungTu']").each(function (i) {
                this.checked = value;
            });
        }                                            
    </script>
</asp:Content>
