<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        string MaND = User.Identity.Name;
        string ParentID = "DuToanBS_ChungTu";
        string ChiNganSach = Request.QueryString["ChiNganSach"];
        string iID_MaChungTu_TLTH = Request.QueryString["iID_MaChungTu"];
        string bTLTH = Request.QueryString["bTLTH"];
        string MaDotNganSach = Convert.ToString(ViewData["MaDotNganSach"]);
        string MaLoaiNganSach = Request.QueryString["sLNS"];
        string sLNS = Request.QueryString["sLNS"];
        if (String.IsNullOrEmpty(sLNS)) sLNS = "-1";
        string iSoChungTu = Request.QueryString["SoChungTu"];
        string sTuNgay = Request.QueryString["TuNgay"];
        string sDenNgay = Request.QueryString["DenNgay"];
        string sLNS_TK = Request.QueryString["sLNS_TK"];
        string iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
        string page = Request.QueryString["page"];
        string iKyThuat = Request.QueryString["iKyThuat"];
        string iID_MaPhongBan = "";
        sLNS = "1040100";
        
        DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
        if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
        {
            DataRow drPhongBan = dtPhongBan.Rows[0];
            iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
            dtPhongBan.Dispose();
        }
        int CurrentPage = 1;

        if (HamChung.isDate(sTuNgay) == false) sTuNgay = "";
        if (HamChung.isDate(sDenNgay) == false) sDenNgay = "";

        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";
        Boolean bThemMoi = false;
        String iThemMoi = "";
        if (ViewData["bThemMoi"] != null)
        {
            bThemMoi = Convert.ToBoolean(ViewData["bThemMoi"]);
            if (bThemMoi)
                iThemMoi = "on";
        }
        String dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);

        //dtTrangThai
        DataTable dtTrangThai_All;

        dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeChiTieu);
        DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeChiTieu, MaND);
        dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
        dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
        dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();
        //Danh sach LNS
        DataTable dtLoaiNganSach = NganSach_HamChungModels.DSLNS_LocCuaPhongBan(MaND, sLNS);
        dtLoaiNganSach.Rows.InsertAt(dtLoaiNganSach.NewRow(), 0);
        dtLoaiNganSach.Rows[0]["sLNS"] = "";
        dtLoaiNganSach.Rows[0]["sTen"] = "-- Chọn loại ngân sách --";
        SelectOptionList slLoaiNganSach = new SelectOptionList(dtLoaiNganSach, "sLNS", "sTen");
        dtLoaiNganSach.Dispose();

        //Danh sach phòng ban đích
        String iID_MaPhongBanDich = Request.QueryString["iID_MaPhongBanDich"];
        if (String.IsNullOrEmpty(iID_MaPhongBanDich)) iID_MaPhongBanDich = iID_MaPhongBan;
        DataTable dtPhongBanDich = PhongBanModels.GetDanhSachPhongBan();
        dtPhongBanDich.Rows.InsertAt(dtPhongBanDich.NewRow(), 0);
        dtPhongBanDich.Rows[0]["sKyHieu"] = "";
        dtPhongBanDich.Rows[0]["TenHT"] = "-- Chọn phòng ban đích --";
        SelectOptionList slPhongBanDich = new SelectOptionList(dtPhongBanDich, "sKyHieu", "TenHT");
        dtPhongBanDich.Dispose();

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        //kiem tra nguoi dung co phan tro ly phong ban
        Boolean CheckNDtao = false;
        DataTable dt = DuToanBS_ChungTuModels.LayDSChungTuKyThuatBia(MaND);

        //double nums = DuToanBS_ChungTuModels.Get_DanhSachChungTu_Count(iID_MaChungTu_TLTH, bTLTH, iID_MaPhongBan, sLNS, "", MaDotNganSach, MaND, iSoChungTu, sTuNgay, sDenNgay, sLNS_TK, iID_MaTrangThaiDuyet, CheckNDtao);
        double nums = DuToanBS_ChungTuModels.LayDanhSachChungTu(iID_MaChungTu_TLTH, sLNS, MaDotNganSach, MaND, iSoChungTu, sTuNgay, sDenNgay, sLNS_TK, iID_MaTrangThaiDuyet, CheckNDtao).Rows.Count;
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { SoChungTu = iSoChungTu, TuNgay = sTuNgay, DenNgay = sDenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, page = x }));
        String strThemMoi = Url.Action("Edit", "DuToanBS_ChungTu", new { MaDotNganSach = MaDotNganSach, sLNS = sLNS, ChiNganSach = ChiNganSach });

        String sKyThuat = "";
        if (iKyThuat == "1")
            sKyThuat = "Ngành kỹ thuật";
        //using (Html.BeginForm("SearchSubmit", "DuToanBS_ChungTu", new { ParentID = ParentID, sLNS = sLNS }))
        //{
    %>
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
                        <span>Danh sách đợt dự toán ngan sách
                            <%=sKyThuat %></span>
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
                    Người tạo
                </th>
                <th style="width: 15%;" align="center">
                    Trạng thái
                </th>
                <th style="width: 5%;" align="center">
                    Duyệt
                </th>
                <th style="width: 5%;" align="center">
                    Từ chối
                </th>
            </tr>
            <%
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    int STT = i + 1;
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
                    String strduyet = "", strTuChoi;

                    String NgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
                    String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTu_BaoDam", new { iID_MaChungTu = R["iID_MaChungTu"], iLoai = 4 }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");
                    String strURLTuChoi = "", strTex = "";
                    if (LuongCongViecModel.KiemTra_NguoiDungDuocDuyet(MaND, PhanHeModels.iID_MaPhanHeChiTieu) && Convert.ToInt16(R["iID_MaTrangThaiDuyet"]) == LuongCongViecModel.layTrangThaiDuyet(PhanHeModels.iID_MaPhanHeChiTieu))
                    {
                        strURLTuChoi = Url.Action("TuChoi", "DuToanBS_ChungTuChiTiet", new { ChiNganSach = ChiNganSach, iID_MaChungTu = R["iID_MaChungTu"] });
                        strTex = "Từ chối";

                    }
                    bool DaDuyet = false;
                    if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(PhanHeModels.iID_MaPhanHeChiTieu, Convert.ToInt16(R["iID_MaTrangThaiDuyet"])))
                    {
                        DaDuyet = true;
                    }
                    if (DaDuyet == false)
                        strduyet = MyHtmlHelper.ActionLink(Url.Action("TrinhDuyet", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"], sLNS = sLNS, iLoai = 1 }).ToString(), "<img src='../Content/Themes/images/arrow_up.png' alt='' />", "", "", "title=\"Duyệt chứng từ\"");
                    strTuChoi = MyHtmlHelper.ActionLink(Url.Action("TuChoi", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"], sLNS = sLNS, iLoai = 1 }).ToString(), "<img src='../Content/Themes/images/arrow_down.png' alt='' />", "", "", "title=\"Từ chối chứng từ\""); 
                    
            %>
            <tr>
                <td align="center">
                    <b><%=STT%></b>
                </td>
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTu", new { iID_MaChungTu = R["iID_MaChungTu"],iLoai=4 }).ToString(),"Đợt ngày: " +  NgayChungTu, "Detail", "")%></b>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(dt.Rows[i]["sNoiDung"])%>
                </td>
                <td align="center">
                    <%=strURL %>
                </td>
                <td align="center">
                    <%=R["sID_MaNguoiDungTao"]%>
                </td>
                <td align="center">
                    <%=sTrangThai%>
                </td>
                <td align="center">
                    <div onclick="OnInit_CT_NEW(500, 'Duyệt chứng từ');">
                        <%= Ajax.ActionLink("Trình Duyệt", "Index", "NhapNhanh", new { id = "DUTOAN_TRINHDUYETCHUNGTU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = R["iID_MaChungTu"], sLNS = sLNS,iKyThuat=iKyThuat, iID_MaChungTu_TLTH = iID_MaChungTu_TLTH }, new AjaxOptions { }, new { @class = "buttonDuyet" })%>
                    </div>
                </td>
                <td align="center">
                    <div onclick="OnInit_CT_NEW(500, 'Từ chối chứng từ');">
                        <%= Ajax.ActionLink("Từ Chối", "Index", "NhapNhanh", new { id = "DUTOAN_TUCHOICHUNGTU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = R["iID_MaChungTu"], iID_MaChungTu_TLTH = iID_MaChungTu_TLTH, sLNS = sLNS, iKyThuat = iKyThuat }, new AjaxOptions { }, new { @class = "button123" })%>
                    </div>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="13" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
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
                    $(event.target).parent().css('top', '10px');
                }
            });
        }
        function OnLoad_CT(v) {
            document.getElementById("idDialog").innerHTML = v;
        }
        CheckThemMoi(false);
        function CheckThemMoi(value) {
            if (value == true) {
                document.getElementById('tb_DotNganSach').style.display = '';
            } else {
                document.getElementById('tb_DotNganSach').style.display = 'none';
            }
        }
    </script>
    <div id="idDialog" style="display: none;">
    </div>
</asp:Content>
