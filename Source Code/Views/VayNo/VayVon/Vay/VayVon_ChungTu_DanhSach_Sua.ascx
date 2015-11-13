<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
   
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String ParentID = Convert.ToString(props["ControlID"].GetValue(Model));
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    Boolean ChucNangCapNhap = (props["ChucNangCapNhap"] == null) ? false : Convert.ToBoolean(props["ChucNangCapNhap"].GetValue(Model));

    String MaChungTu = Request.QueryString["iID_MaChungTu"];
    String iID_MaDonVi = Request.QueryString["iID_MaDonVi"];
    NameValueCollection data = VayNoModels.LayThongTin(MaChungTu);
    int iID_MaTrangThaiDuyet = Convert.ToInt32(data["iID_MaTrangThaiDuyet"]);

    DataTable dt = VayNoModels.getListDS_Sua(MaChungTu, iID_MaDonVi);
    string iID_MaNoiDung = string.Empty;
    string MaDonVi = string.Empty;
    string iID_Loai = string.Empty;
    string dNgayVay = DateTime.Now.ToString("dd/MM/yyyy");
    string rLaiSuat = "0";
    string rMienLai = "0";
    string iID_MaChungTuVayVon = string.Empty;

    string rVayTrongThang = "0";
    string dHanPhaiTra = string.Empty;
    string rThoiGianThuVon = string.Empty;

    string sMoTa = string.Empty;
    string MaLoaiVayVon = string.Empty;
    string bDongY = string.Empty;
    string sLydo = string.Empty;
    string sDongY = string.Empty;
    string sThuTruongDuyet = string.Empty;
    string sSoQD = string.Empty;
    string dNgayDuyet = string.Empty;

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
    
   
%>
<%
    if (DuocSuaChiTietDuyet || DuocSuaLaiChungTuChiTiet)
    {
%>
<form action="<%=Url.Action("EditSubmit","VayNo_ChungTuChiTiet",new {iID_MaChungTu = MaChungTu})%>"
method="post">
<%
    }
%>
<div style="overflow-x: scroll; overflow-y: hidden; position: relative; width: 100%;">
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
            <th align="center">
                Lãi xuất (%)
            </th>
            <th align="center">
                Miễn lãi
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
            <% //Phải là Trợ lý phòng ban mới được thêm chi tiết ngân sách
                if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeTinDung, MaND) &&
                    LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeTinDung, iID_MaTrangThaiDuyet))
                {      
            %>
            <th>
                Xóa
            </th>
            <%
                }
            %>
            <%
                //Được xem thông tin duyệt hay không
                if (DuocSuaChiTietDuyet)
                {
                    //Dành cho người duyệt
            %>
            <th style="width: 20px;">
                <input type="checkbox" id="chkDongY" onclick="setCheckboxes();" />
            </th>
            <th style="width: 50px;">
                Nhận xét
            </th>
            <%
                }
                else if (DuocXemChiTietDuyet)
                {
            %>
            <th style="width: 20px;">
                &nbsp;
            </th>
            <th style="width: 50px;">
                Nhận xét
            </th>
            <%
                }
            %>
        </tr>
        <%        
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow R = dt.Rows[i];
                String iID_MaChungTuChiTiet = Convert.ToString(R["iID_VayChiTiet"]);
                string dNgayVayShort = string.Empty;
                string dHanPhaiTraShort = string.Empty;
                if (R["dNgayVay"] != DBNull.Value)
                {
                    DateTime dNgayVayR = Convert.ToDateTime(R["dNgayVay"]);
                    dNgayVayShort = dNgayVayR.ToString("dd/MM/yyyy");
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
                string strMienLai = "";

                rLaiSuat = Convert.ToString(dt.Rows[i]["rLaiSuat"]);
                rMienLai = Convert.ToString(dt.Rows[i]["bMienLai"]);
                if (rMienLai == "True" || rMienLai == "1")
                {
                    strMienLai = "X";
                }
                else
                {
                    strMienLai = "";
                }
                rVayTrongThang = Convert.ToString(dt.Rows[i]["rVayTrongThang"]);

                dHanPhaiTra = HamChung.ConvertDateTime(dt.Rows[i]["dHanPhaiTra"]).ToString("dd/MM/yyyy");
                if (dHanPhaiTra == "01/01/0001") dHanPhaiTra = "";
                // rThoiGianThuVon = Convert.ToString(dt.Rows[i]["rThoiGianThuVon"]);
                sMoTa = Convert.ToString(dt.Rows[i]["sGhiChu"]);
                sLydo = Convert.ToString(dt.Rows[i]["sLyDo"]);

                sThuTruongDuyet = Convert.ToString(dt.Rows[i]["sThuTruongDuyet"]);
                sSoQD = Convert.ToString(dt.Rows[i]["sSoQD"]);
                dNgayDuyet = HamChung.ConvertDateTime(dt.Rows[i]["dNgayDuyet"]).ToString("dd/MM/yyyy");
                string sTaiKhoanNo = HttpUtility.HtmlEncode(dt.Rows[i]["sTaiKhoanNo"]);
                string sDonViNo = HttpUtility.HtmlEncode(dt.Rows[i]["sDonViNo"]);
                string sTaiKhoanCo = HttpUtility.HtmlEncode(dt.Rows[i]["sTaiKhoanCo"]);
                string sDonViCo = HttpUtility.HtmlEncode(dt.Rows[i]["sDonViCo"]);
                if (dNgayDuyet == "01/01/0001") dNgayDuyet = "";

                if (Convert.ToBoolean(R["bDongY"]))
                {
                    sDongY = "X";
                }
                String sLoaiDuAn = Convert.ToString(DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(dt.Rows[i]["iID_Loai"])).Rows[0]["sTen"]);
        %>
        <tr <%=classtr %>>
            <td align="left">
                <div style="width: 90px;">
                    <%= HttpUtility.HtmlEncode(dt.Rows[i]["iID_MaKhoanVay"])%></div>
            </td>
            <td align="left">
                <div style="width: 250px;">
                    <%= HttpUtility.HtmlEncode(dt.Rows[i]["sTenDonVi"])%></div>
            </td>
            <td align="left">
                <div style="width: 250px;">
                    <%=HttpUtility.HtmlEncode(dt.Rows[i]["sTenNoiDung"])%></div>
            </td>
            <td align="left">
                <div style="width: 120px;">
                    <%=HttpUtility.HtmlEncode(sLoaiDuAn)%></div>
            </td>
            <td align="center">
                <div style="width: 70px;">
                    <%= dNgayVayShort %>
                </div>
            </td>
            <%
                //Được xem thông tin duyệt hay không
                if (DuocSuaChiTietDuyet || DuocSuaLaiChungTuChiTiet)
                {
                    //Dành cho người duyệt
            %>
            <td align="center">
                <%=MyHtmlHelper.TextBox(new { ParentID = iID_MaChungTuChiTiet, Value = rLaiSuat, TenTruong = "rLaiSuat", LoaiTextBox = 1, Attributes = " class='input1_3' tab-index='-1' style='width:90px;'", SoSauDauPhay = 2 })%>
            </td>
            <td align="center">
                <%=MyHtmlHelper.CheckBox(iID_MaChungTuChiTiet, rMienLai, "bMienLai", "class=\"input1_3\"")%>
            </td>
            <td align="right">
                <%=MyHtmlHelper.TextBox(new { ParentID = iID_MaChungTuChiTiet, Value = rVayTrongThang, TenTruong = "rVayTrongThang", Attributes = " class='input1_3' style='width:150px;height:25px;'" })%>
            </td>
            <td align="center">
                <%=MyHtmlHelper.DatePicker(iID_MaChungTuChiTiet, dHanPhaiTra, "dHanPhaiTra", "", "class=\"input1_4\"")%>
            </td>
            <td align="center">
                <%=MyHtmlHelper.TextBox(new { ParentID = iID_MaChungTuChiTiet, Value = sThuTruongDuyet, TenTruong = "sThuTruongDuyet", Attributes = " class='input1_3' style='width:150px;height:25px;'" })%>
            </td>
            <td align="center">
                <%=MyHtmlHelper.DatePicker(iID_MaChungTuChiTiet, dNgayDuyet, "dNgayDuyet", "", "class=\"input1_4\"")%>
            </td>
            <td align="center">
                <%=MyHtmlHelper.TextBox(new { ParentID = iID_MaChungTuChiTiet, Value = sSoQD, TenTruong = "sSoQD", Attributes = " class='input1_3' style='width:150px;height:25px;'" })%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, sTaiKhoanNo, "iID_MaTaiKhoan_No", "", "class=\"input1_3\"  tab-index='0' style='width:300px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, sDonViNo, "iID_MaDonVi_No", "", "class=\"input1_3\"  tab-index='0' style='width:200px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, sTaiKhoanCo, "iID_MaTaiKhoan_Co", "", "class=\"input1_3\"  tab-index='0' style='width:300px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, sDonViCo, "iID_MaDonVi_Co", "", "class=\"input1_3\"  tab-index='0' style='width:200px;height:25px;'")%>
            </td>
            <td align="left">
                <%=MyHtmlHelper.TextBox(new { ParentID = iID_MaChungTuChiTiet, Value = sMoTa, TenTruong = "sMoTa", Attributes = " class='input1_3'" })%>
            </td>
            <% }
                else
                { %>
            <td align="center">
                <div style="width: 90px;">
                    <%=rLaiSuat%></div>
            </td>
            <td align="center">
                <div style="width: 90px;">
                    <%=strMienLai%></div>
            </td>
            <td align="right">
                <div style="width: 120px;">
                    <b>
                        <%=CommonFunction.DinhDangSo(rVayTrongThang)%></b></div>
            </td>
            <td align="center">
                <div style="width: 100px;">
                    <%=dHanPhaiTra%></div>
            </td>
            <td align="left">
                <div style="width: 100px;">
                    <%=sThuTruongDuyet%>
                </div>
            </td>
            <td align="center">
                <div style="width: 100px;">
                    <%=dNgayDuyet%></div>
            </td>
            <td align="left">
                <div style="width: 100px;">
                    <%=sSoQD%></div>
            </td>
            <td align="left">
                <div style="width: 300px;">
                    <%=sTaiKhoanNo%></div>
            </td>
            <td align="left">
                <div style="width: 250px;">
                    <%=sDonViNo %></div>
            </td>
            <td align="left">
                <div style="width: 300px;">
                    <%=sTaiKhoanCo%></div>
            </td>
            <td align="left">
                <div style="width: 250px;">
                    <%=sDonViCo%></div>
            </td>
            <td align="left">
                <div style="width: 250px;">
                    <%=sMoTa%></div>
            </td>
            <%
                    //Được xem thông tin duyệt hay không
                } if (DuocSuaChiTietDuyet)
                {
                    //Dành cho người duyệt
            %>
            <td style="text-align: center">
                <%=MyHtmlHelper.CheckBox(iID_MaChungTuChiTiet, bDongY.ToString(), "bDongY", "", "group-index='1'")%>
                <%=MyHtmlHelper.Hidden(iID_MaChungTuChiTiet, iID_MaChungTuChiTiet, "iID_VayChiTiet", "")%>
            </td>
            <td>
                <%=MyHtmlHelper.TextBox(new { ParentID = iID_MaChungTuChiTiet, Value = sLydo, TenTruong = "sLyDo", Attributes = " class='input1_3' tab-index='-1'" })%>
            </td>
            <%
                }
                else if (DuocXemChiTietDuyet)
                {
            %>
            <td style="text-align: center">
                <%=sDongY %>
            </td>
            <td style="text-align: left">
                <%=sLydo%>
            </td>
            <%
                }
            %>
            <% //Phải là Trợ lý phòng ban mới được thêm chi tiết ngân sách
                if (LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeTinDung, MaND) &&
                    LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeTinDung, iID_MaTrangThaiDuyet))
                {
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "VayNo_ChungTuChiTiet", new { iID_MaChungTuChiTiet = iID_MaChungTuChiTiet, iID_MaChungTu = MaChungTu }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            %>
            <td style="text-align: center">
                <%=strDelete%>
            </td>
            <%
                }
            %>
        </tr>
        <%} %>
    </table>
</div>
<%
    if (DuocSuaChiTietDuyet || DuocSuaLaiChungTuChiTiet)
    {
%>
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
                        <input type="submit" class="button" id="Submit1" value="<%=NgonNgu.LayXau("Sửa")%>" />
                    </td>
                    <%-- <td>
                        &nbsp;
                    </td>
                    <td align="left">
                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" />
                    </td>--%>
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
%>
<%
    if (DuocSuaChiTietDuyet || DuocSuaLaiChungTuChiTiet)
    {
%>
</form>
<%
    }
%>
<%
   
    if (dt != null) dt.Dispose(); 
%>
<%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
<script type="text/javascript">
    function setCheckboxes() {
        $('input:checkbox[group-index="1"]').each(function (i) {
            this.checked = document.getElementById('chkDongY').checked;
        });
    }    
</script>
