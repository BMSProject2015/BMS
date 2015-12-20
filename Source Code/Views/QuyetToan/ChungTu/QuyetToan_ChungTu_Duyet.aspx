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
    int i;
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

    if (String.IsNullOrEmpty(iID_MaTrangThaiDuyet))
    {
        iID_MaTrangThaiDuyet = Convert.ToString(LuongCongViecModel.Get_iID_MaTrangThaiDuyet_DuocSua(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND));
    }

    DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND);
    dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
    dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
    dtTrangThai.Rows[0]["sTen"] = "-- Chọn trạng thái --";
    SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");

    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }
    Boolean LayTheoMaNDTao = false;
    if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND)) LayTheoMaNDTao = true;
    string MaPhongBan = "";
    DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
    if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
    {
        MaPhongBan = Convert.ToString(dtPhongBan.Rows[0]["sKyHieu"]);
        dtPhongBan.Dispose();
    }
    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(QuyetToanModels.iID_MaPhanHeQuyetToan);
    DataTable dt = QuyetToan_ChungTuModels.Get_DanhSachChungTu(MaPhongBan, Loai, MaND, MaDonVi, iSoChungTu, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, sThangQuy, LayTheoMaNDTao, CurrentPage, Globals.PageSize);

    double nums = QuyetToan_ChungTuModels.Get_DanhSachChungTu_Count(MaPhongBan, Loai, MaND, MaDonVi, iSoChungTu, sTuNgay, sDenNgay, iID_MaTrangThaiDuyet, sThangQuy, LayTheoMaNDTao);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Duyet", new { Loai = Loai, MaND = MaND, SoChungTu = iSoChungTu, TuNgay = sTuNgay, DenNgay = sDenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, page = x }));

    String strThemMoi = Url.Action("Edit", "QuyetToan_ChungTu", new {Loai = Loai});
    using (Html.BeginForm("SearchDuyetSubmit", "QuyetToan_ChungTu", new { ParentID = ParentID, Loai = Loai }))
    {
%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
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
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1"><div><b>Số chứng từ</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.TextBox(ParentID, iSoChungTu, "iSoChungTu", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Ngày chứng từ từ ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DatePicker(ParentID, sTuNgay, "dTuNgay", "", "class=\"input1_2\"")%>        
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1"><div><b>Trạng thái</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iID_MaTrangThaiDuyet, "iID_MaTrangThaiDuyet", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Đến ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DatePicker(ParentID, sDenNgay, "dDenNgay", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr><td colspan="2" align="center" class="td_form2_td1" style="height: 10px;"></td></tr>
                <tr>
                    <td colspan="2" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <input type="submit" class="button" value="Tìm kiếm"/>
                                </td>
                                <td style="width: 10px;"></td>
                                <td>
                                    <%
                                        if(LuongCongViecModel.NguoiDung_DuocThemChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan, MaND))
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
<br />
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách dự toán</span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: 10%;" align="center">Đơn vị</th>
            <th style="width: 15%;" align="center">Loại Ngân sách</th>
            <th style="width: 10%;" align="center">Ngày chứng từ</th>
            <th style="width: 10%;" align="center">Thời gian quyết toán</th>
            <th style="width: 10%;" align="center">Số chứng từ</th>
            <th style="width: 20%;" align="center">Nội dung</th>
            <th style="width: 15%;" align="center">Trạng thái</th>
            <th style="width: 5%;" align="center">Thao tác</th>
            <th style="width: 5%;" align="center">Chi tiết</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
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
            String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));            
            
            //Lấy thông tin loại ngân sách
            String strLNS = DanhMucModels.NS_TenLoaiNganSach(Convert.ToString(R["sDSLNS"]));
            
            //Lây thời gian quyết toán
            String strThoiGianQuyetToan = "";
            switch (Convert.ToInt32(R["bLoaiThang_Quy"])) { 
                case 0:
                    strThoiGianQuyetToan = "Tháng: " + Convert.ToString(R["iThang_Quy"]) + "/" + Convert.ToString(R["iNamLamViec"]);
                    break;
                case 1:
                    strThoiGianQuyetToan = "Quý: " + Convert.ToString(R["iThang_Quy"]) + "/" + Convert.ToString(R["iNamLamViec"]);
                    break;
                case 2:
                    strThoiGianQuyetToan = "Năm: " + Convert.ToString(R["iThang_Quy"]) + "/" + Convert.ToString(R["iNamLamViec"]);
                    break;
            }
           
            String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết chứng từ\"");
            String strURLTuChoi = "", strTex = "";
            if (LuongCongViecModel.KiemTra_NguoiDungDuocDuyet(MaND, PhanHeModels.iID_MaPhanHeQuyetToan) && Convert.ToInt16(R["iID_MaTrangThaiDuyet"]) == LuongCongViecModel.layTrangThaiDuyet(PhanHeModels.iID_MaPhanHeQuyetToan))
            {
                strURLTuChoi = Url.Action("TuChoiDuyet", "QuyetToan_ChungTu", new { iID_MaChungTu = R["iID_MaChungTu"], Loai = Loai });
                strTex = "Từ chối";
            } 
            %>
            <tr <%=strColor %>>
                <td align="center"><%=STT%></td>  
                <td align="left"><%=strTenDonVi%></td>
                <td align="left"><%=strLNS%></td>          
                <td align="center"><%=NgayChungTu %></td>
                <td align="center"><%=strThoiGianQuyetToan%></td>
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"] }).ToString(), Convert.ToString(R["iSoChungTu"]), "Detail", "")%></b>
                </td>
                <td align="left"><%=dt.Rows[i]["sNoiDung"]%></td>
                <td align="center"><%=sTrangThai %></td>
                  <td align="center">
                    <%=MyHtmlHelper.ActionLink(strURLTuChoi, strTex)%>
                </td>
                <td align="center">
                    <%=strURL %>
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="10" align="right">
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
</asp:Content>


