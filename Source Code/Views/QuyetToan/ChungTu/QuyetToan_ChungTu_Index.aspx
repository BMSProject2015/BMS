<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        String ParentID = "QuyetToan_ChungTu";
        String MaND = User.Identity.Name;
        String Loai = Request.QueryString["Loai"];
        String MaDonVi = Request.QueryString["MaDonVi"];
        String iSoChungTu = Request.QueryString["SoChungTu"];
        String sThangQuy = Request.QueryString["sThangQuy"];
        String sTuNgay = Request.QueryString["TuNgay"];
        String sDenNgay = Request.QueryString["DenNgay"];
        String iID_MaTrangThaiDuyet = Request.QueryString["iID_MaTrangThaiDuyet"];
        String page = Request.QueryString["page"];
        if (String.IsNullOrEmpty(Loai)) Loai = Convert.ToString(ViewData["Loai"]);

        int iLoaiThangQuy = 0;
        if (Loai == "1")
        {
            iLoaiThangQuy = 0;
        }
        else
        {
            iLoaiThangQuy = 1;
        }

        if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet) || iID_MaTrangThaiDuyet == "-1") iID_MaTrangThaiDuyet = "";

        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        DataRow R1 = dtDonVi.NewRow();
        R1["iID_MaDonVi"] = "";
        R1["sTen"] = "--- Danh sách tất cả các đơn vị ---";
        dtDonVi.Rows.InsertAt(R1, 0);
        dtDonVi.Dispose();

        DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
        dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
        dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
        dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");

        DataTable dtQuy = DanhMucModels.DT_Quy_QuyetToan();
        SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
        R1 = dtQuy.NewRow();
        R1["MaQuy"] = "5";
        R1["TenQuy"] = "Bổ sung";
        dtQuy.Rows.Add(R1);
        dtQuy.Dispose();

        String strThemMoi = Url.Action("Edit", "QuyetToan_ChungTu", new { Loai = Loai });

        using (Html.BeginForm("SearchSubmit", "QuyetToan_ChungTu", new { ParentID = ParentID, Loai = Loai }))
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
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTu", new { Loai = Loai}), "Chứng từ quyết toán")%>
                </div>
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
                        <td  align="left" style="width: 45%;" class="td_form2_td1">
                            <table  width="100%">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Đơn vị</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                       
                                            <b>Số chứng từ</b>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, iSoChungTu, "iSoChungTu", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Ngày chứng từ từ ngày</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, sTuNgay, "dTuNgay", "", "class=\"input1_2\" onblur=isDate(this); ")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td  align="left" style="width: 45%;" >
                            <table width="100%">
                                <tr>
                                    <%if (Loai == "1")
                                      { %>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Quý</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slQuy, sThangQuy, "iQuy", "", "class=\"input1_2\" style=\"width:50%;\"")%>
                                        </div>
                                    </td>
                                    <%}
                                      else
                                      { %>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Quý</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slQuy, sThangQuy, "iQuy", "", "class=\"input1_2\" style=\"width:50%;\"")%>
                                        </div>
                                    </td>
                                    <%} %>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Trạng thái</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Đến ngày</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%=MyHtmlHelper.DatePicker(ParentID, sDenNgay, "dDenNgay", "", "class=\"input1_2\" onblur=isDate(this);")%>
                                            <%=Html.ValidationMessage(ParentID + "_" + "err_dDenNgay")%>
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
                                        <%
                                            if (LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND))
                                            {
                                        %>
                                        <input id="TaoMoi" type="button" class="button" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                                        <%
                                            } %>
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
    <% 
        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }

        Boolean LayTheoMaNDTao = false;
        if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND)) LayTheoMaNDTao = true;
        String MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
         MaPhongBan = Request.QueryString["MaPhongBan"];
        DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(QuyetToanModels.iID_MaPhanHeQuyetToan);
        DataTable dt = QuyetToan_ChungTuModels.Get_DanhSachChungTu(MaPhongBan, Loai, MaND, MaDonVi, iSoChungTu, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, sThangQuy, LayTheoMaNDTao, CurrentPage, Globals.PageSize);

        double nums = QuyetToan_ChungTuModels.Get_DanhSachChungTu_Count(MaPhongBan, Loai, MaND, MaDonVi, iSoChungTu, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, sThangQuy, LayTheoMaNDTao);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { Loai = Loai, MaND = MaND, MaDonVi = MaDonVi, SoChungTu = iSoChungTu, TuNgay = sTuNgay, DenNgay = sDenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, sThangQuy = sThangQuy, page = x }));

        String sWidthDonVi = "12%";
        String sSoCot = "11";
        if (Loai == "1") { sSoCot = "12"; }
        String sTenNamQuyetToan = "";
        String iID_MaNamNganSach = "";
        if (dt.Rows.Count > 0)
            iID_MaNamNganSach = Convert.ToString(dt.Rows[0]["iID_MaNamNganSach"]);
        if (iID_MaNamNganSach == "1") sTenNamQuyetToan = " năm trước";
        else if (iID_MaNamNganSach == "2") sTenNamQuyetToan = " năm nay";
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách chứng từ quyết toán <%=sTenNamQuyetToan %></span>
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: <%=sWidthDonVi%>;" align="center">
                    Đơn vị
                </th>
                <%if (Loai == "1")
                  { %>
                <th style="width: 5%;" align="center">
                    Loại Ngân sách
                </th>
                <%} %>
                <th style="width: 7%;" align="center">
                    Ngày chứng từ
                </th>
                <th style="width: 10%;" align="center">
                    Thời gian quyết toán
                </th>
                <th style="width: 5%;" align="center">
                    Số chứng từ
                </th>
                <th style="width: 25%;" align="center">
                    Nội dung
                </th>
                <th style="width: 10%;" align="center">
                    Số tiền
                </th>
                <th style="width: 10%;" align="center">
                    Trạng thái
                </th>
                <th style="width: 5%;" align="center">
                    Chi tiết
                </th>
                <th style="width: 3%;" align="center">
                    Sửa
                </th>
                <th style="width: 3%;" align="center">
                    Xóa
                </th>
            </tr>
            <%
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    String classtr = "";
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

                    //Lấy tên đơn vị
                    String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]),MaND);

                    //Lấy thông tin loại ngân sách
                    String strLNS = Convert.ToString(R["sDSLNS"]);
                    if (strLNS.Length > 48) strLNS = strLNS.Substring(0, 47);

                    //Lây thời gian quyết toán
                    String strThoiGianQuyetToan = "";
                    switch (Convert.ToInt32(R["iLoai"]))
                    {
                        case -1:
                            strThoiGianQuyetToan = "Tháng: " + Convert.ToString(R["iThang_Quy"]) + "/" + Convert.ToString(R["iNamLamViec"]);
                            break;
                        default:
                            if (Convert.ToString(R["bLoaiThang_Quy"]) == "10")
                            {
                                strThoiGianQuyetToan = "Tháng: " + Convert.ToString(R["iThang_Quy"]) + "/" + Convert.ToString(R["iNamLamViec"]);
                            }
                            else
                            {
                                if (Convert.ToInt32(R["iThang_Quy"]) == 1)
                                {
                                    strThoiGianQuyetToan = "Quý: I/" + Convert.ToString(R["iNamLamViec"]);
                                }
                                else if (Convert.ToInt32(R["iThang_Quy"]) == 2)
                                {
                                    strThoiGianQuyetToan = "Quý: II/" + Convert.ToString(R["iNamLamViec"]);
                                }
                                else if (Convert.ToInt32(R["iThang_Quy"]) == 3)
                                {
                                    strThoiGianQuyetToan = "Quý: III/" + Convert.ToString(R["iNamLamViec"]);
                                }
                                else if (Convert.ToInt32(R["iThang_Quy"]) == 4)
                                {
                                    strThoiGianQuyetToan = "Quý: IV/" + Convert.ToString(R["iNamLamViec"]);
                                }
                                else if (Convert.ToInt32(R["iThang_Quy"]) == 5)
                                {
                                    strThoiGianQuyetToan = "Quý: Bổ sung/" + Convert.ToString(R["iNamLamViec"]);
                                }

                            }
                            break;
                    }

                    //Lay Tong So Tien Quyet Toan
                    String strTongTienQuyetToan = "";
                    strTongTienQuyetToan = CommonFunction.DinhDangSo(QuyetToan_ChungTuModels.ThongKeTongSoQuyetToan_ChungTu(Convert.ToString(R["iID_MaChungTu"])));

                    String strEdit = "";
                    String strDelete = "";
                    if (LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND) &&
                                        LuongCongViecModel.KiemTra_TrangThaiKhoiTao(QuyetToanModels.iID_MaPhanHeQuyetToan, Convert.ToInt32(R["iID_MaTrangThaiDuyet"])))
                    {
                        strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "QuyetToan_ChungTu", new { iID_MaChungTu = R["iID_MaChungTu"], Loai = Loai }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                        strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "QuyetToan_ChungTu", new { iID_MaChungTu = R["iID_MaChungTu"], Loai = Loai }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                    }

                    String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");
                       
            %>
            <tr <%=strColor %>>
                <td align="center">
                    <%=R["rownum"]%>
                </td>
                <td align="left" >
                    <%=strTenDonVi%>
                </td>
                <%if (Loai == "1")
                  { %>
                <td align="left"  width="1%">
                  <div>  <%=strLNS%> </div>
                </td>
                <%} %>
                <td align="center">
                    <%=NgayChungTu %>
                </td>
                <td align="center">
                    <%=strThoiGianQuyetToan%>
                </td>
                <td align="center">
                    <b>
                        <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"] }).ToString(), Convert.ToString(R["iSoChungTu"]), "Detail", "")%></b>
                </td>
                <td align="left">
                    <%= HttpUtility.HtmlEncode(dt.Rows[i]["sNoiDung"])%>
                </td>
                <td align="right" style="color: Red;">
                    <b>
                        <%=strTongTienQuyetToan%></b>
                </td>
                <td align="center">
                    <%=sTrangThai %>
                </td>
                <td align="center">
                    <%=strURL %>
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
                <td colspan='<%=sSoCot%>' align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
    <%  
        dt.Dispose();
        dtTrangThai_All.Dispose();
        dtTrangThai.Dispose();
    %>
    <%--<br />
<script type="text/javascript">
    List_QuyetToan_ChungTu();
    function List_QuyetToan_ChungTu() {
        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("get_List_QuyetToan_ChungTu?Loai=#0&MaND=#1&iSoChungTu=#2&dTuNgay=#3&dDenNgay=#4&iID_MaTrangThaiDuyet=#5&page=#6", "QuyetToan_ChungTu")%>');
        url = unescape(url.replace("#0", "<%=Loai %>"));
        url = unescape(url.replace("#1", "<%=MaND %>"));
        url = unescape(url.replace("#2", "<%=iSoChungTu %>"));
        url = unescape(url.replace("#3", "<%=sTuNgay %>"));
        url = unescape(url.replace("#4", "<%=sDenNgay %>"));
        url = unescape(url.replace("#5", "<%=iID_MaTrangThaiDuyet %>"));
        url = unescape(url.replace("#6", "<%=page %>"));

        $.getJSON(url, function (data) {
            document.getElementById("divListQuyetToan").innerHTML = data;
        });
    }        
        
</script>
<div id="divListQuyetToan"></div>--%>
</asp:Content>
