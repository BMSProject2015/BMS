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
        String MaND = User.Identity.Name;
        String ParentID = "Index";
        String iID_MaBangLuong = Request.QueryString["iID_MaBangLuong"];

        //String iNamBangLuong = Convert.ToString(ViewData["iNamBangLuong"]);
        //String iThangBangLuong = Convert.ToString(ViewData["iThangBangLuong"]);

        //if (String.IsNullOrEmpty(iNamBangLuong)) iNamBangLuong = Request.QueryString["iNamBangLuong"];
        //if (String.IsNullOrEmpty(iThangBangLuong)) iThangBangLuong = Request.QueryString["iThangBangLuong"];



        DateTime dNgayHienTai = DateTime.Now;
        String NamHienTai = Convert.ToString(dNgayHienTai.Year);
        String iThangHienTai = Convert.ToString(dNgayHienTai.Month);
        int NamMin = Convert.ToInt32(dNgayHienTai.Year) - 10;
        int NamMax = Convert.ToInt32(dNgayHienTai.Year) + 10;
        DataTable dtNam = new DataTable();
        dtNam.Columns.Add("MaNam", typeof(String));
        dtNam.Columns.Add("TenNam", typeof(String));
        DataRow Row;
        for (i = NamMin; i < NamMax; i++)
        {
            Row = dtNam.NewRow();
            dtNam.Rows.Add(Row);
            Row[0] = Convert.ToString(i);
            Row[1] = Convert.ToString(i);
        }
        String iNamBangLuong = Convert.ToString(CauHinhLuongModels.LayNamLamViec(User.Identity.Name));
        if (iNamBangLuong == "0") iNamBangLuong = NamHienTai;
        String iThangBangLuong = Convert.ToString(CauHinhLuongModels.LayThangLamViec(User.Identity.Name));
        if (iThangBangLuong == "0") iThangBangLuong = iThangHienTai;
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");

        int LuongTT = CauHinhLuongModels.LayLuongToiThieu();

        DataTable dtNamNganSach = DanhMucModels.NS_NamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "iID_MaNamNganSach", "sTen");
        DataTable dtThang = DanhMucModels.DT_Thang(false);
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();

        ///
        //String page = Request.QueryString["page"];
        //int CurrentPage = 1;
        //SqlCommand cmd;

        //if (String.IsNullOrEmpty(page) == false)
        //{
        //    CurrentPage = Convert.ToInt32(page);
        //}

        //DataTable dt = LuongModels.Get_dtBangLuong(iNamBangLuong, iThangBangLuong, CurrentPage, Globals.PageSize);

        //DataTable dtTrangThai_All = LuongCongViecModel.Get_dtDSTrangThaiDuyet(PhanHeModels.iID_MaPhanHeLuong);


        //double nums = LuongModels.Get_CountBangLuong(iNamBangLuong, iThangBangLuong);
        //int TotalPages = (int)Math.Ceiling(nums / 2);
        //String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x, iNamBangLuong = iNamBangLuong, iThangBangLuong = iThangBangLuong }));



        String strThemMoi = Url.Action("Edit", "Luong_BangLuong", new { iNamBangLuong = iNamBangLuong, iThangBangLuong = iThangBangLuong });
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách bảng lương</span>
                    </td>
                    <td>
                        Tháng &nbsp;
                        <%=MyHtmlHelper.DropDownList(ParentID, slThang, iThangBangLuong, "iThangLamViec", "", "class=\"input1_2\" style=\"width:13%;\" onchange=\"ChonThangNam(this.value, 1)\"")%>
                        Năm &nbsp;
                        <%=MyHtmlHelper.DropDownList(ParentID, slNam, iNamBangLuong, "iNamLamViec", "", "class=\"input1_2\" style=\"width: 13%\" onchange=\"ChonThangNam(this.value, 2)\"")%>
                        &nbsp; LG tối thiểu &nbsp;
                        <%=MyHtmlHelper.TextBox(ParentID, LuongTT.ToString(), "sLuongToiThieu", "", " khong-nhap='1'  class=\"input1_2\" style=\"width: 13%\"", 1)%>
                        &nbsp; PC tối thiểu &nbsp;
                        <%=MyHtmlHelper.TextBox(ParentID, LuongTT/2.5, "sLuongToiThieu", "", " khong-nhap='1'  class=\"input1_2\" style=\"width: 13%\"",1)%>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divListDanhSach">
        </div>
        <%--  <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">
                    STT
                </th>
                <th style="width: 15%;" align="center">
                    Tên bảng lương
                </th>
                <th align="center">
                    Đơn vị làm lương
                </th>
                <th style="width: 15%;" align="center">
                    Trạng thái duyệt
                </th>
                <th style="width: 5%;" align="center">
                    Chi tiết
                </th>
                <th style="width: 5%;" align="center">
                    Xóa
                </th>
            </tr>
            <%
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];


                    Boolean LuongMoi = LuongCongViecModel.KiemTra_TrangThaiKhoiTao(PhanHeModels.iID_MaPhanHeLuong, Convert.ToInt32(R["iID_MaTrangThaiDuyet"]));

                    Boolean DuocThemLuong = LuongCongViecModel.NguoiDung_DuocThemChungTu(PhanHeModels.iID_MaPhanHeLuong, MaND);

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
                    DataTable dtDonVi = LuongModels.Get_DSDonViCuaBangLuong(Convert.ToString(R["iID_MaBangLuong"]));
                    String TenDonVi = "";
                    for (int j = 0; j < dtDonVi.Rows.Count; j++)
                    {
                        if (TenDonVi != "") TenDonVi += ",";
                        TenDonVi += Convert.ToString(dtDonVi.Rows[j]["sTen"]);
                    }
                    dtDonVi.Dispose();

                    String strURL = MyHtmlHelper.ActionLink(Url.Action("Index", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = R["iID_MaBangLuong"] }).ToString(), "<img src='../Content/Themes/images/btnSetting.png' alt='' />", "Detail", null, "title=\"Xem chi tiết bảng lương\"");
                    String classtr = "";
                    int STT = i + 1;
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
            %>
            <tr <%=strColor %>>
                <td align="center">
                    <%=R["rownum"]%>
                </td>
                <td align="center">
                    <%=HttpUtility.HtmlEncode(R["sTen"])%>
                </td>
                <td align="left">
                    <%=HttpUtility.HtmlEncode(TenDonVi)%>
                </td>
                <td align="center">
                    <%=sTrangThai %>
                </td>
                <td align="center">
                    <%=strURL %>
                </td>
                <td align="center">
                    <%if (LuongMoi)
                      {%>
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "Luong_BangLuong", new { iID_MaBangLuong = R["iID_MaBangLuong"], iNamBangLuong = R["iNamBangLuong"], iThangBangLuong = R["iThangBangLuong"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
                    <%} %>
                </td>
            </tr>
            <%} %>
            <tr class="pgr">
                <td colspan="9" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>--%>
    </div>
    <script type="text/javascript">
        ChonThangNam("", 3);
        function ChonThangNam(value, loai) {
            var iThangLamViec;
            var iNamLamViec;
            if (loai == 1) {
                iThangLamViec = value;
                iNamLamViec = document.getElementById('<%= ParentID %>_iNamLamViec').value;
            }
            else if (loai == 2) {
                iNamLamViec = value;
                iThangLamViec = document.getElementById('<%= ParentID %>_iThangLamViec').value;
            }
            else {
                iNamLamViec = document.getElementById('<%= ParentID %>_iNamLamViec').value;
                iThangLamViec = document.getElementById('<%= ParentID %>_iThangLamViec').value;
            }
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("UpdateCauHinhNamLamViec?MaND=#0&iThangLamViec=#1&iNamLamViec=#2", "Luong_BangLuong")%>');
            url = unescape(url.replace("#0", '<%=MaND %>'));
            url = unescape(url.replace("#1", iThangLamViec));
            url = unescape(url.replace("#2", iNamLamViec));
            $.getJSON(url, function (data) {
                document.getElementById("divListDanhSach").innerHTML = data;
            });
        }
     
    </script>
</asp:Content>
