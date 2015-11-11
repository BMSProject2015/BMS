<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site_Default.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="DomainModel.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%

        String MaND = User.Identity.Name;

        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        int iNam = DateTime.Now.Year;
        int iThang = DateTime.Now.Month;
        String NamNS = "1", NguonNS = "2";
        if (dtCauHinh.Rows.Count > 0)
        {
            iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
            iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
            NamNS = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            NguonNS = Convert.ToString(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
        }
        dtCauHinh.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang_CoThangKhong();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        DataTable dtNam = DanhMucModels.DT_Nam(false);
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        DataTable dtNamNganSach = DanhMucModels.NS_NamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "iID_MaNamNganSach", "sTen");
        DataTable dtNguonNganSach = DanhMucModels.NS_NguonNganSach();
        SelectOptionList slNguonNganSach = new SelectOptionList(dtNguonNganSach, "iID_MaNguonNganSach", "sTen");
        dtNamNganSach.Dispose();
        dtNguonNganSach.Dispose();

        String Quyen = HamChung.getPhanHe(MaND);
    %>
   <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 150px;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Luồng dữ liệu: ")%></b>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div style="width: 100%;">
        <table width="100%" cellspacing="3" cellpadding="3" border="0">
            <tr>
                <td colspan="7" style="font-weight: bold;" valign="top">
                    <div class="box_tong" style="margin-left: 5px;">
                        <div class="title_tong">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <span>Cấu hình hệ thống</span>
                                    </td>
                                    <td style="width: 59px; text-align: right;" valign="top">
                                        <b>Tháng&nbsp;</b>
                                    </td>
                                    <td valign="top">
                                        <%=MyHtmlHelper.DropDownList("CauHinh", slThang, Convert.ToString(iThang), "iThangLamViec", "", "onchange=\"ChonThangNam(this.value, 1)\" style=\"width:50px;\"")%>
                                        <b>&nbsp; Năm</b>
                                        <%=MyHtmlHelper.DropDownList("CauHinh", slNam, Convert.ToString(iNam), "iNamLamViec", "", "onchange=\"ChonThangNam(this.value, 2)\" style=\"width:80px;\"")%>
                                    </td>
                                    <td>
                                        Năm ngân sách
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.DropDownList("CauHinh", slNamNganSach, NamNS, "iID_MaNamNganSach", "", "onchange=\"ChonThangNam(this.value, 3)\" class=\"input1_2\" style=\"width: 200px\"")%>
                                    </td>
                                    <td>
                                        Nguồn ngân sách
                                    </td>
                                    <td>
                                        <%=MyHtmlHelper.DropDownList("CauHinh", slNguonNganSach, NguonNS, "iID_MaNguonNganSach", "", "onchange=\"ChonThangNam(this.value, 4)\" class=\"input1_2\" style=\"width: 300px\"")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 59px;">
                    &nbsp;
                </td>
                <td style="width: 40px">
                    &nbsp;
                </td>
                <td style="width: 59px; text-align: center;" valign="top">
                    &nbsp;
                </td>
                <td style="width: 25px">
                    &nbsp;
                </td>
                <td style="width: 33px">
                    &nbsp;
                </td>
                <td style="text-align: left; width: 53px;" valign="top">
                    &nbsp;
                </td>
                <td style="width: 19px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <table width="100%" cellspacing="5" cellpadding="5">
                        <tr>
                            <td style="text-align: center;" colspan="3">
                                <div class="box_tong" style="margin-left: 5px;">
                                    <div class="title_tong">
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                            <tr>
                                                <td>
                                                    <span>Hệ thống</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                            <td style="text-align: center; width: 18px;">
                                &nbsp;
                            </td>
                            <td style="text-align: center;" colspan="5">
                                <div class="box_tong" style="margin-left: 5px;">
                                    <div class="title_tong">
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                            <tr>
                                                <td>
                                                    <span>Nghiệp vụ Tài chính</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                            <td style="width: 22px">
                                &nbsp;
                            </td>
                            <td>
                                <div class="box_tong" style="margin-left: 5px;">
                                    <div class="title_tong">
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                            <tr>
                                                <td>
                                                    <span>Ghi chú</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;" colspan="3">
                                &nbsp;
                            </td>
                            <td style="text-align: center; width: 18px;">
                                &nbsp;
                            </td>
                            <td style="text-align: center;" colspan="5">
                                &nbsp;
                            </td>
                            <td style="width: 22px">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; width: 51px;">
                                &nbsp;
                            </td>
                            <td style="text-align: center; width: 112px;" rowspan="3">
                                <br />
                                <a href="http://192.104.78.11:1111/NguoiDung/List" title="Quản trị hệ thống (B11)">
                                    <img alt="Quản trị hệ thống (B11)" border="0" src="../../Content/Themes/images/KT_QuantriNguoiDung.gif"
                                        style="width: 150px" />
                                    <br />
                                    <span style="text-align: center; font-weight: bold;">Quản trị hệ thống (B11)</span>
                                </a>
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                            </td>
                            <td style="text-align: center; width: 112px;" rowspan="3">
                                <img src="../../Content/Themes/images/KT_LUONGVIEW.gif" alt="Quản trị hệ thống (B11)"
                                    border="0" style="width: 103px; height: 286px" />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                            </td>
                            <td style="text-align: center; width: 18px;" rowspan="3">
                                &nbsp;
                            </td>
                            <td style="text-align: center; width: 164px;" valign="top" rowspan="3">
                                <a href="http://192.104.78.11:1111" title="Quản trị dữ liệu lịch sử">
                                    <img src="../../Content/Themes/images/KT_DuLieuLichSu.gif" alt="Quản trị dữ liệu lịch sử"
                                        border="0" style="width: 158px" />
                                    <br />
                                    <span style="text-align: center; font-weight: bold;">Quản trị dữ liệu lịch sử</span>
                                </a>
                                <br />
                                <br />
                                <a href="http://192.104.78.11:1111" title="Quản lý giá các sản phẩm quốc phòng">
                                    <img src="../../Content/Themes/images/KT_QLGia.gif" alt="Quản lý giá các sản phẩm quốc phòng"
                                        border="0" style="height: 87px; width: 151px;" />
                                    <br />
                                    <span style="text-align: center; font-weight: bold;">Quản lý giá</span> </a>
                                <br />
                                <br />
                                <a href="http://192.104.78.11:1111" title="Quản lý Ngân sách">
                                    <img src="../../Content/Themes/images/KT_QuanLyNS.gif" alt="Quản lý Ngân sách" border="0"
                                        style="height: 153px; width: 157px;" />
                                    <br />
                                    <span style="text-align: center; font-weight: bold;">Quản lý Ngân sách(Chỉ tiêu, Phân
                                        bổ, Cấp phát, Quyết toán...)</span> </a>
                            </td>
                            <td style="width: 78px">
                                &nbsp;
                            </td>
                            <td style="text-align: center; width: 143px;" valign="top" rowspan="3">
                                <a href="http://192.104.78.11:1111" title="Quản lý công sản">
                                    <img alt="Quản lý công sản" border="0" src="../../Content/Themes/images/KT_QLcongSan.gif"
                                        style="height: 125px; width: 153px;" />
                                    <br />
                                    <span style="text-align: center; font-weight: bold;">Quản lý công sản</span></a>
                                <br />
                                <img alt="Quản trị dữ liệu lịch sử" border="0" src="../../Content/Themes/images/KT_LuongCS.gif"
                                    style="width: 120px; height: 89px;" />
                                <br />
                                <a href="http://192.104.78.11:1179" title="Phân hệ Kế toán (Tổng hợp, Chi tiết)">
                                    <img alt="Phân hệ Kế toán (Tổng hợp, Chi tiết)" border="0" src="../../Content/Themes/images/KT_KeToan.gif"
                                        style="height: 142px" />
                                    <br />
                                    <span style="text-align: center; font-weight: bold;">Phân hệ Kế toán (B3)
                                        <br />
                                        (Tổng hợp, Chi tiết,Tín dụng)</span> </a>
                            </td>
                            <td valign="bottom" style="text-align: center; width: 41px;" rowspan="3">
                                <img alt="Quản trị dữ liệu lịch sử" border="0" src="../../Content/Themes/images/KT_TT.gif"
                                    style="width: 45px; height: 121px;" />
                                <br />
                                <br />
                                <br />
                                <br />
                                <br />
                            </td>
                            <td valign="top" style="text-align: center; width: 162px;" rowspan="3">
                                <a href="http://192.104.78.11:1111" title="Tài chính doanh nghiệp">
                                    <img alt="Tài chính doanh nghiệp" border="0" src="../../Content/Themes/images/KT_TCDN.gif"
                                        style="height: 91px; width: 151px;" />
                                    <br />
                                    <span style="text-align: center; font-weight: bold;">Tài chính doanh nghiệp</span>
                                </a>
                                <br />
                                <br />
                                <a href="http://192.104.78.11:1111/CanBo_HoSoNhanSu" title="Quản lý nhân sự">
                                    <img alt="Quản lý nhân sự" border="0" src="../../Content/Themes/images/KT_QLNS.gif"
                                        style="height: 86px; width: 151px;" />
                                    <br />
                                    <span style="text-align: center; font-weight: bold;">Quản lý nhân sự</span>
                                </a>
                                <br />
                                <br />
                                <a href="http://192.104.78.11:1111" title="Quản lý dự án (B16)">
                                    <img alt="Quản lý dự án (B16)" border="0" src="../../Content/Themes/images/KT_QLDA.gif"
                                        style="height: 145px; width: 152px;" />
                                    <br />
                                    <span style="text-align: center; font-weight: bold;">Quản lý dự án (B16)</span> </a>
                            </td>
                            <td rowspan="3" valign="top" style="width: 22px">
                                &nbsp;
                            </td>
                            <td rowspan="3" valign="top" style="font-size: 13px; color: #FF0000">
                                <%-- <span style="text-decoration: underline; color: #FF0000">Ghi chú</span>--%>
                                <br />
                                - Sau khi đăng nhập vào hệ thống bạn chọn đúng hệ thống mình cần thao tác dữ liệu,
                                chỉ những phân hệ đã được phân quyền hệ thống mới cho phép người dùng thao tác với
                                dữ liệu của hệ thống đó.
                                <br />
                                - Hiện tại bạn đang được quyền thao tác với các chức năng: <span style="color: blue; font-size:13px;">
                                    <% =Quyen %></span>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; height: 10px; width: 51px;" valign="top">
                            </td>
                            <td rowspan="2" valign="top">
                                <br />
                                <br />
                                <br />
                                <img src="../../Content/Themes/images/KT_Luong2.gif" alt="Quản trị dữ liệu lịch sử"
                                    border="0" style="width: 80px; height: 117px; margin-right: 0px;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; width: 51px;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; width: 51px;">
                                &nbsp;
                            </td>
                            <td style="text-align: center; width: 112px;">
                                &nbsp;
                            </td>
                            <td style="text-align: center; width: 112px;">
                                &nbsp;
                            </td>
                            <td style="text-align: center; width: 18px;">
                                &nbsp;
                            </td>
                            <td style="text-align: center; width: 112px;">
                                &nbsp;
                            </td>
                            <td style="width: 78px">
                                &nbsp;
                            </td>
                            <td style="width: 78px">
                                &nbsp;
                            </td>
                            <td style="width: 41px">
                                &nbsp;
                            </td>
                            <td style="width: 162px">
                                &nbsp;
                            </td>
                            <td style="width: 22px">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function ChonThangNam(value, loai) {
            var iThangLamViec;
            var iNamLamViec;
            var MaNamNganSach;
            var MaNguonNganSach;
            if (loai == 1) {
                iThangLamViec = value;
                iNamLamViec = document.getElementById('CauHinh_iNamLamViec').value;
                MaNamNganSach = document.getElementById('CauHinh_iID_MaNamNganSach').value;
                MaNguonNganSach = document.getElementById('CauHinh_iID_MaNguonNganSach').value;

            }
            else if (loai == 2) {
                iNamLamViec = value;

                iThangLamViec = document.getElementById('CauHinh_iThangLamViec').value;
                MaNamNganSach = document.getElementById('CauHinh_iID_MaNamNganSach').value;
                MaNguonNganSach = document.getElementById('CauHinh_iID_MaNguonNganSach').value;
            }
            else if (loai == 3) {
                iNamLamViec = document.getElementById('CauHinh_iNamLamViec').value;

                iThangLamViec = document.getElementById('CauHinh_iThangLamViec').value;
                MaNamNganSach = value;
                MaNguonNganSach = document.getElementById('CauHinh_iID_MaNguonNganSach').value;
            }
            else {
                iNamLamViec = document.getElementById('CauHinh_iNamLamViec').value;
                iThangLamViec = document.getElementById('CauHinh_iThangLamViec').value;
                MaNamNganSach = document.getElementById('CauHinh_iID_MaNamNganSach').value;
                MaNguonNganSach = value;
            }
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("UpdateCauHinhNamLamViec?MaND=#0&iThangLamViec=#1&iNamLamViec=#2&MaNamNganSach=#3&MaNguonNganSach=#4","TrangChu") %>');
            url = unescape(url.replace("#0", '<%=MaND %>'));
            url = unescape(url.replace("#1", iThangLamViec));
            url = unescape(url.replace("#2", iNamLamViec));
            url = unescape(url.replace("#3", MaNamNganSach));
            url = unescape(url.replace("#4", MaNguonNganSach));

            $.getJSON(url, function (data) {
                jsKeToan_LoadLaiChungTu();
            });

        }
      
    </script>
</asp:Content>
