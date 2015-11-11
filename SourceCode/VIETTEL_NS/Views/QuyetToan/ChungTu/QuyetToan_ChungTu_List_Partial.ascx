<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<% 
    QuyetToanListModels dlParam = (QuyetToanListModels)Model;
    String Loai = dlParam.Loai;
    String MaND = dlParam.MaND;
    String iSoChungTu = dlParam.iSoChungTu;
    String dTuNgay = dlParam.dTuNgay;
    String dDenNgay = dlParam.dDenNgay;
    String iID_MaTrangThaiDuyet = dlParam.iID_MaTrangThaiDuyet;
    String page = dlParam.page;
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }

    Boolean LayTheoMaNDTao = false;
    if(LuongCongViecModel.KiemTra_TroLyPhongBan(MaND)) LayTheoMaNDTao=true;
    String MaPhongBan = NganSach_HamChungModels.MaPhongBanCuaMaND(MaND);
    DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(QuyetToanModels.iID_MaPhanHeQuyetToan);
    DataTable dt = QuyetToan_ChungTuModels.Get_DanhSachChungTu(MaPhongBan,Loai, MaND, iSoChungTu, dTuNgay, dDenNgay, iID_MaTrangThaiDuyet,LayTheoMaNDTao, CurrentPage, Globals.PageSize);

    double nums = QuyetToan_ChungTuModels.Get_DanhSachChungTu_Count(MaPhongBan, Loai, MaND, iSoChungTu, dTuNgay, dDenNgay, iID_MaTrangThaiDuyet, LayTheoMaNDTao);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {Loai = Loai, MaND = MaND, SoChungTu = iSoChungTu, TuNgay = dTuNgay, DenNgay = dDenNgay, iID_MaTrangThaiDuyet = iID_MaTrangThaiDuyet, page = x }));

    String sWidthDonVi = "10%";
    String sSoCot = "11";
    if (Loai == "1") { sSoCot = "10"; }
%>

<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách chứng từ quyết toán</span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: <%=sWidthDonVi%>;" align="center">Đơn vị</th>
            <%if (Loai != "1")
              { %>
            <th style="width: 15%;" align="center">Loại Ngân sách</th>
            <%} %>
            <th style="width: 10%;" align="center">Ngày chứng từ</th>
            <th style="width: 10%;" align="center">Thời gian quyết toán</th>
            <th style="width: 10%;" align="center">Số chứng từ</th>
            <th style="width: 20%;" align="center">Nội dung</th>
            <th style="width: 15%;" align="center">Trạng thái</th>
            <th style="width: 5%;" align="center">Chi tiết</th>
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
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
            String strTenDonVi = DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));            
            
            //Lấy thông tin loại ngân sách
            String strLNS = Convert.ToString(R["sDSLNS"]);
            
            //Lây thời gian quyết toán
            String strThoiGianQuyetToan = "";
            switch (Convert.ToInt32(R["bLoaiThang_Quy"])) { 
                case 0:
                    strThoiGianQuyetToan = "Tháng: " + Convert.ToString(R["iThang_Quy"]) + "/" + Convert.ToString(R["iNamLamViec"]);
                    break;
                case 1:
                    if (Convert.ToInt32(R["iThang_Quy"]) == 3) {
                        strThoiGianQuyetToan = "Quý: I/" + Convert.ToString(R["iNamLamViec"]);
                    }
                    else if (Convert.ToInt32(R["iThang_Quy"]) == 6)
                    {
                        strThoiGianQuyetToan = "Quý: II/" + Convert.ToString(R["iNamLamViec"]);
                    }
                    else if (Convert.ToInt32(R["iThang_Quy"]) == 9)
                    {
                        strThoiGianQuyetToan = "Quý: III/" + Convert.ToString(R["iNamLamViec"]);
                    }
                    else if (Convert.ToInt32(R["iThang_Quy"]) == 12)
                    {
                        strThoiGianQuyetToan = "Quý: IV/" + Convert.ToString(R["iNamLamViec"]);
                    }
                    break;
                case 2:
                    strThoiGianQuyetToan = "Năm: " + Convert.ToString(R["iThang_Quy"]) + "/" + Convert.ToString(R["iNamLamViec"]);
                    break;
            }
            
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
                <td align="center"><%=R["rownum"]%></td>  
                <td align="left"><%=strTenDonVi%></td>
                <%if (Loai != "1")
                  { %>
                <td align="left"><%=strLNS%></td>    
                <%} %>      
                <td align="center"><%=NgayChungTu %></td>
                <td align="center"><%=strThoiGianQuyetToan%></td>
                <td align="center">
                    <b><%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"] }).ToString(), Convert.ToString(R["sTienToChungTu"]) + Convert.ToString(R["iSoChungTu"]), "Detail", "")%></b>
                </td>
                <td align="left"><%= HttpUtility.HtmlEncode(dt.Rows[i]["sNoiDung"])%></td>
                <td align="center"><%=sTrangThai %></td>
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
%>