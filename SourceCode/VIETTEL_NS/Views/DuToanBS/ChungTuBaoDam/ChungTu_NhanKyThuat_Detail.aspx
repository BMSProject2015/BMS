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
        string iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        string iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
        string sM = Request.QueryString["sM"];
        string sTM = Request.QueryString["sTM"];
        string sTTM = Request.QueryString["sTTM"];
        string sNG = Request.QueryString["sNG"];
        string iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
        string page = Request.QueryString["page"];
        string iKyThuat = Request.QueryString["iKyThuat"];
        string iID_MaPhongBan = "";
        DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
        if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
        {
            DataRow drPhongBan = dtPhongBan.Rows[0];
            iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
            dtPhongBan.Dispose();
        }
        int CurrentPage = 1;

        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";
        //dtTrangThai
        DataTable dtTrangThai_All;
             dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeChiTieu);
        DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeChiTieu, MaND);
        dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
        dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
        dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();
        //Danh sach đơn vị
        DataTable dtDonVi = DuToanBS_ChungTuModels.getDanhSachDonViKyThuat(MaND, iID_MaChungTu);
        dtDonVi.Rows.InsertAt(dtDonVi.NewRow(), 0);
        dtDonVi.Rows[0]["iID_MaDonVi"] = "";
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "iID_MaDonVi");
        dtDonVi.Dispose();

        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        //kiem tra nguoi dung co phan tro ly phong ban
        Boolean check = LuongCongViecModel.KiemTra_TroLyPhongBan(MaND);
        Boolean checkTroLyTongHop = LuongCongViecModel.KiemTra_TroLyTongHop(MaND);

        DataTable dt = DuToanBS_ChungTuModels.getDanhSachChungTuKyThuat(MaND, iID_MaChungTu,iID_MaDonVi,sM,sTM,sTTM,sNG);
        String sKyThuat = "";
        if (iKyThuat == "1")
            sKyThuat = "Ngành kỹ thuật";
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
                        <span>Tìm kiếm đợt dự toán ngân sách bảo đảm <%=sKyThuat %></span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <%
                    using (Html.BeginForm("SearchSubmit", "DuToanBS_PhanCapChungTuChiTiet", new { ParentID = ParentID, iID_MaChungTu = iID_MaChungTu, iLoai = "4" }))
                    {       
                %>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="td_form2_td1" style="width: 5%">
                            <div>
                                <b>Chọn ĐV</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 5%">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%></div>
                        </td>
                        <td class="td_form2_td1" style="width: 2%">
                            <div>
                                <b>M</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 5%">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sM, "sM", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 2%">
                            <div>
                                <b>TM</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 5%">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTM, "sTM", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                        <td class="td_form2_td1" style="width: 2%">
                            <div>
                                <b>TTM</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 5%">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTTM, "sTTM", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                         <td class="td_form2_td1" style="width: 2%">
                            <div>
                                <b>NG</b></div>
                        </td>
                        <td class="td_form2_td5" style="width: 5%">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sNG, "sNG", "", "class=\"input1_2\"")%>
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
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách đợt dự toán ngân sách bảo đảm <%=sKyThuat %></span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <%--<input id="Button1" type="submit" class="button_title" value="Trình duyệt" onclick="javascript:location.href=''" />--%>
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid" id="<%= ParentID %>_thList">
            <tr>
                <th style="width: 2%;" align="center">
                    STT
                </th>
                
                <th style="width: 2%;" align="center">
                    ĐV
                </th>
                <th style="width: 5%;" align="center">
                    M
                </th>
                <th style="width: 5%;" align="center">
                    TM
                </th>
                <th style="width: 5%;" align="center">
                    TTM
                </th>
                <th style="width: 5%;" align="center">
                    NG
                </th>
                <th style="width: 25%;" align="center">
                   Mô tả
                </th>
                <th style="width: 10%;" align="center">
                    Bằng tiền
                </th>
                <th style="width: 10%;" align="center">
                   Hiện vật
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
                    String classtr = "";
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
                    String strEdit = "";
                    String strDelete = "";
                    String strduyet = "", strTuChoi;
                    if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeChiTieu, MaND) &&
                                        LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeChiTieu, Convert.ToInt32(R["iID_MaTrangThaiDuyet"])) || (LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeChiTieu, Convert.ToInt32(R["iID_MaTrangThaiDuyet"]))) && check)
                    {
                        strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "DuToanBS_ChungTu_BaoDam", new { iID_MaChungTu = R["iID_MaChungTu"], sLNS = "1040100", iKyThuat = iKyThuat }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "", "title=\"Sửa chứng từ\"");
                        strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "DuToanBS_ChungTu_BaoDam", new { iID_MaChungTu = R["iID_MaChungTu"], sLNS = "1040100", iKyThuat = iKyThuat }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "", "title=\"Xóa chứng từ\"");
                    }

                    String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");
                    String strURLTuChoi = "", strTex = "";
                    if (LuongCongViecModel.KiemTra_NguoiDungDuocDuyet(MaND, PhanHeModels.iID_MaPhanHeChiTieu) && Convert.ToInt16(R["iID_MaTrangThaiDuyet"]) == LuongCongViecModel.layTrangThaiDuyet(PhanHeModels.iID_MaPhanHeChiTieu))
                    {
                        strURLTuChoi = Url.Action("TuChoi", "DuToanBS_ChungTuChiTiet", new {  iID_MaChungTu = R["iID_MaChungTu"] });
                        strTex = "Từ chối";

                    }
                    bool DaDuyet = false;
                    if (LuongCongViecModel.KiemTra_TrangThaiDaDuyet(PhanHeModels.iID_MaPhanHeChiTieu, Convert.ToInt16(R["iID_MaTrangThaiDuyet"])))
                    {
                        DaDuyet = true;
                    }
                    if (DaDuyet == false)
                        strduyet = MyHtmlHelper.ActionLink(Url.Action("TrinhDuyet", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"], sLNS = "1040100", iLoai = 1 }).ToString(), "<img src='../Content/Themes/images/arrow_up.png' alt='' />", "", "", "title=\"Duyệt chứng từ\"");
                    strTuChoi = MyHtmlHelper.ActionLink(Url.Action("TuChoi", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"], sLNS = "1040100", iLoai = 1 }).ToString(), "<img src='../Content/Themes/images/arrow_down.png' alt='' />", "", "", "title=\"Từ chối chứng từ\"");
                    String rTuChi = String.Format("{0:0,0}", R["rTuChi"]);
                    if(rTuChi=="00") rTuChi="";
                    String rHienVat = String.Format("{0:0,0}", R["rHienVat"]);
                    if (rHienVat == "00") rHienVat = "";
            %>
            <tr >
                <td align="center">
                    <%=i+1 %>
                </td>
                <td align="center">
                   <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTuChiTiet"], iLoai = 4 }).ToString(), R["iID_MaDonVi"], "Detail", "")%></b>
                </td>
                 <td align="center">
                   <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTuChiTiet"], iLoai = 4 }).ToString(), R["sM"], "Detail", "")%></b>
                </td>
                 <td align="center">
                   <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTuChiTiet"], iLoai = 4 }).ToString(), R["sTM"], "Detail", "")%></b>
                </td>
                 <td align="center">
                   <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTuChiTiet"], iLoai = 4 }).ToString(), R["sTTM"], "Detail", "")%></b>
                </td>
                 <td align="center">
                   <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTuChiTiet"], iLoai = 4 }).ToString(), R["sNG"], "Detail", "")%></b>
                </td>
                 <td align="left">
                  <b> <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTuChiTiet"], iLoai = 4 }).ToString(), R["sMoTa"], "Detail", "")%></b>
                </td>
                <td align="right">
                  <b> <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTuChiTiet"], iLoai = 4 }).ToString(), rTuChi, "Detail", "")%></b>
                </td>
                <td align="right">
                 <b>  <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToanBS_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTuChiTiet"], iLoai = 4 }).ToString(), rHienVat, "Detail", "")%></b>
                </td>
                <td align="center" <%=strColor %>>
                    <%=sTrangThai%>
                </td>
                <td align="center">
                    <div onclick="OnInit_CT_NEW(500, 'Duyệt chứng từ');">
                        <%= Ajax.ActionLink("Trình Duyệt", "Index", "NhapNhanh", new { id = "DUTOAN_TRINHDUYETCHUNGTU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = R["iID_MaChungTu"], sLNS = "1040100",iKyThuat=iKyThuat}, new AjaxOptions { }, new { @class = "buttonDuyet" })%>
                    </div>
                </td>
                <td align="center">
                    <div onclick="OnInit_CT_NEW(500, 'Từ chối chứng từ');">
                        <%= Ajax.ActionLink("Từ Chối", "Index", "NhapNhanh", new { id = "DUTOAN_TUCHOICHUNGTU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = R["iID_MaChungTu"], sLNS = "1040100", iKyThuat = iKyThuat }, new AjaxOptions { }, new { @class = "button123" })%>
                    </div>
                </td>
            </tr>
            <%} %>
            
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
