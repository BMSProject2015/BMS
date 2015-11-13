<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String ParentID = Convert.ToString(props["ControlID"].GetValue(Model));
    String MaChungTu = Request.QueryString["iID_MaChungTu"];
    Boolean ChucNangCapNhap = (props["ChucNangCapNhap"] == null) ? false : Convert.ToBoolean(props["ChucNangCapNhap"].GetValue(Model));
    NameValueCollection data = VayNoModels.LayThongTin(MaChungTu);
    int iID_MaTrangThaiDuyet = Convert.ToInt32(data["iID_MaTrangThaiDuyet"]);

    DataTable dt = VayNoModels.getListDS(MaChungTu);
    string iID_MaNoiDung = string.Empty;
    string MaDonVi = string.Empty;
    string iID_Loai = string.Empty;
    string dNgayVay = DateTime.Now.ToString("dd/MM/yyyy");
    string rLaiSuat = "0";
    string rMienLai = "false";
    string iID_MaChungTuVayVon = string.Empty;
    string iID_MaKhoanVay = "";
    //string rDuVonCu = string.Empty;
    //string rDuLaiCu = string.Empty;
    string rVayTrongThang = "0";
    string dHanPhaiTra = string.Empty;
    string rThoiGianThuVon = string.Empty;
    string sThuTruongDuyet = string.Empty;
    string sSoQD = string.Empty;
    string sMoTa = string.Empty;
    string dNgayDuyet = DateTime.Now.ToString("dd/MM/yyyy");
    string MaLoaiVayVon = string.Empty;

    DataTable dtDonVi = DanhMucModels.NS_DonVi();
    String sTenDonVi = Convert.ToString(dtDonVi.Rows[0]["sTen"]);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    if (dtDonVi != null) dtDonVi.Dispose();

    DataTable dtNoiDung = VayNoModels.ListNoiDung();
    SelectOptionList slNoiDung = new SelectOptionList(dtNoiDung, "iID_MaNoiDung", "sMaTen");
    if (dtNoiDung != null) dtNoiDung.Dispose();

    DataTable dtLoaiVayVon = DanhMucModels.DT_DanhMuc("LoaiVayVon", false, "--- Chọn loại vay vốn ---");
    SelectOptionList optLoaiVayVon = new SelectOptionList(dtLoaiVayVon, "iID_MaDanhMuc", "sTen");
    if (dtLoaiVayVon != null) dtLoaiVayVon.Dispose();


    DataTable dtTaiKhoan = VayNoModels.LayDanhSachTaiKhoan(MaND);
    SelectOptionList slTaiKhoan = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "sTen");
    if (dtTaiKhoan != null) dtTaiKhoan.Dispose();

    Boolean DuocSuaLaiChungTuChiTiet = false;
    Boolean DuocSuaChiTietDuyet = false;
    Boolean DuocXemChiTietDuyet = false;
    if (ChucNangCapNhap)
    {
        Boolean DuocThemChungTu = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeTinDung, MaND);
        Boolean ND_DuocSuaChungTu = LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeTinDung, MaND, iID_MaTrangThaiDuyet);
        DuocSuaLaiChungTuChiTiet = DuocThemChungTu && ND_DuocSuaChungTu;
        DuocXemChiTietDuyet = LuongCongViecModel.KiemTra_TrangThaiTuChoi(PhanHeModels.iID_MaPhanHeTinDung, iID_MaTrangThaiDuyet) || LuongCongViecModel.KiemTra_TrangThaiTrinhDuyet(PhanHeModels.iID_MaPhanHeTinDung, iID_MaTrangThaiDuyet);
        DuocSuaChiTietDuyet = DuocXemChiTietDuyet && ND_DuocSuaChungTu;
    }

    using (Html.BeginForm("CreateSubmit", "VayNo_ChungTuChiTiet", new { ParentID = ParentID, iID_MaChungTu = MaChungTu }))
    {
%>
<%= Html.Hidden(ParentID + "_iID_Vay", MaChungTu)%>
<div style="position: relative; width: 100%; overflow: scroll;">
    <%-- <table class="mGrid" style="width: 100%">--%>
    <table class="mGrid">
        <tr>
            <th align="center">
                Mã khoản vay
            </th>
            <th align="center">
                Đơn vị
            </th>
            <th align="center">
                Nội dung vay
            </th>
            <th align="center">
                Loại
            </th>
            <th align="center">
                Ngày vay
            </th>
            <th align="center" style="width: 90px;">
                Lãi xuất (%)
            </th>
            <th align="center">
                Miễn lãi (%)
            </th>
            <th align="center">
                Số tiền vay
            </th>
            <th align="center">
                Hạn phải trả
            </th>
            <th align="center">
                TT duyệt
            </th>
            <th align="center">
                Ngày duyệt
            </th>
            <th align="center">
                Số QĐ
            </th>
            <th align="center">
                Tài khoản nợ
            </th>
            <th align="center">
                Đơn vị nợ
            </th>
            <th align="center">
                Tài khoản có
            </th>
            <th align="center">
                Đơn vị có
            </th>
            <th align="center">
                Ghi chú
            </th>
        </tr>
        <%        
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            string dNgayVayShort = string.Empty;
            string dHanPhaiTraShort = string.Empty;
            string dNgayDuyetShort = string.Empty;
            if (R["dNgayVay"] != DBNull.Value)
            {
                DateTime dNgayVayR = Convert.ToDateTime(R["dNgayVay"]);
                dNgayVayShort = dNgayVayR.ToString("dd/MM/yyyy");
            }
            if (R["dNgayDuyet"] != DBNull.Value)
            {
                dNgayDuyetShort = Convert.ToDateTime(R["dNgayDuyet"]).ToString("dd/MM/yyyy");
            }
            if (R["dHanPhaiTra"] != DBNull.Value)
            {
                DateTime dHanPhaiTraR = Convert.ToDateTime(R["dHanPhaiTra"]);
                dHanPhaiTraShort = dHanPhaiTraR.ToString("dd/MM/yyyy");
            }
            String bPublic = Convert.ToString(R["bPublic"]);
            string strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
            String classtr = "";
            int STT = i + 1;
            if (i % 2 == 0)
            {
                classtr = "class=\"alt\"";
            }
            string strEdit = string.Empty;
            string strDelete = string.Empty;
            String sLoaiDuAn = Convert.ToString(DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(dt.Rows[i]["iID_Loai"])).Rows[0]["sTen"]);
            string strMienlai = "";
            if (dt.Rows[i]["bMienLai"] == "True")
                strMienlai = "X";
            else strMienlai = "";
                
        %>
        <tr <%=classtr %>>
            <td align="left">
                <%=HttpUtility.HtmlEncode(dt.Rows[i]["iID_MaKhoanVay"])%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(dt.Rows[i]["sTenDonVi"])%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(dt.Rows[i]["sTenNoiDung"])%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(sLoaiDuAn)%>
            </td>
            <td align="center">
                <%= dNgayVayShort%>
            </td>
            <td align="center">
                <b>
                    <%=dt.Rows[i]["rLaiSuat"] %></b>
            </td>
            <td align="center">
                <b>
                    <%=strMienlai%></b>
            </td>
            <td align="right">
                <b>
                    <%=CommonFunction.DinhDangSo(dt.Rows[i]["rVayTrongThang"])%></b>
            </td>
            <td align="center">
                <%= dHanPhaiTraShort%>
            </td>
            <td align="center">
                <b>
                    <%=dt.Rows[i]["sThuTruongDuyet"]%></b>
            </td>
            <td align="center">
                <b>
                    <%=dNgayDuyetShort%></b>
            </td>
            <td align="left">
                <b>
                    <%=dt.Rows[i]["sSoQD"]%></b>
            </td>
            <td align="left">
                <%=dt.Rows[i]["sTaiKhoanNo"]%>
            </td>
            <td align="left">
                <%=dt.Rows[i]["sDonViNo"]%>
            </td>
            <td align="left">
                <%=dt.Rows[i]["sTaiKhoanCo"]%>
            </td>
            <td align="left">
                <%=dt.Rows[i]["sDonViCo"]%>
            </td>
            <td align="left">
                <%=dt.Rows[i]["sGhiChu"]%>
            </td>
        </tr>
        <%} %>
        <tr>
            <td align="left">
                <div>
                    <%=MyHtmlHelper.TextBox(ParentID, iID_MaKhoanVay, "iID_MaKhoanVay", "", "class=\"input1_3\" tab-index='-1' style='width:90px;height:25px;'")%>
                </div>
            </td>
            <td align="left">
                <div>
                    <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, MaDonVi, "iID_MaDonVi", "", "class=\"input1_3\"  tab-index='0' style='width:250px;height:25px;'")%>
                </div>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DropDownList(ParentID, slNoiDung, iID_MaNoiDung, "iID_MaNoiDung", "", "class=\"input1_3\" tab-index='1' style='width:250px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DropDownList(ParentID, optLoaiVayVon, MaLoaiVayVon, "iID_Loai", "", "class=\"input1_3\" tab-index='2' style='width:120px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DatePicker(ParentID, dNgayVay, "dNgayVay", "", "class=\"input1_3\" tab-index='3' style='width:100px;height:25px;'")%>
                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayVay")%>
            </td>
            <td align="center">
                <%=MyHtmlHelper.TextBox(ParentID, rLaiSuat, "rLaiSuat", "", "class=\"input1_3\" tab-index='4' maxlength='5' style='width:90px;height:25px;'", 1, true, 2)%>
                <br />
                <%= Html.ValidationMessage(ParentID + "_" + "err_rLaiSuat")%>
            </td>
            <td align="center">
                <div style="width: 70px;">
                    <%=MyHtmlHelper.CheckBox(ParentID, rMienLai, "bMienLai", "class=\"input1_3\" tab-index='5'")%></div>
            </td>
            <td align="left">
                <%=MyHtmlHelper.TextBox(ParentID, rVayTrongThang, "rVayTrongThang", "", "class=\"input1_3\" tab-index='6' style='width:150px;height:25px;'", 1)%><br />
                <%= Html.ValidationMessage(ParentID + "_" + "err_rVayTrongThang")%>
            </td>
            <td align="center">
                <%=MyHtmlHelper.DatePicker(ParentID, dHanPhaiTra, "dHanPhaiTra", "", "class=\"input1_3\" tab-index='7' style='width:100px;height:25px;'")%>
                <%= Html.ValidationMessage(ParentID + "_" + "err_dHanPhaiTra")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.TextBox(ParentID, sThuTruongDuyet, "sThuTruongDuyet", "", "class=\"input1_3\" tab-index='8' style='width:150px;height:25px;'")%>
            </td>
            <td align="center">
                <%=MyHtmlHelper.DatePicker(ParentID, dNgayDuyet, "dNgayDuyet", "", "class=\"input1_3\" tab-index='9' style='width:100px;height:25px;'")%>
            </td>
            <td align="center">
                <%=MyHtmlHelper.TextBox(ParentID, sSoQD, "sSoQD", "", "class=\"input1_3\" tab-index='10' style='width:150px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, "", "iID_MaTaiKhoan_No", "", "class=\"input1_3\"  tab-index='0' style='width:300px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, MaDonVi, "iID_MaDonVi_No", "", "class=\"input1_3\"  tab-index='0' style='width:200px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, "", "iID_MaTaiKhoan_Co", "", "class=\"input1_3\"  tab-index='0' style='width:300px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, MaDonVi, "iID_MaDonVi_Co", "", "class=\"input1_3\"  tab-index='0' style='width:200px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.TextBox(ParentID, sMoTa, "sGhiChu", "", "class=\"input1_3\" tab-index='11' style='width:350px;height:25px;'")%>
            </td>
        </tr>
    </table>
</div>
<table width="100%" cellpadding="0" cellspacing="0" border="0" align="right" class="table_form2">
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr>
        <td align="center">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="right">
                        <%
        //Được xem thông tin duyệt hay không
        if (DuocSuaChiTietDuyet)
        {
                    //Dành cho người duyệt
                        %>
                        <input type="submit" class="button" id="Register" value="<%=NgonNgu.LayXau("Thêm mới")%>" />
                        <%
                }
              
                        %>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td align="left">
                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
<%
    }
    if (dt != null) dt.Dispose(); 
%>
