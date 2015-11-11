<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
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
        if (dtCauHinh.Rows.Count > 0)
        {
            iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
            iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
        }
        dtCauHinh.Dispose();

        DataTable dtThang = DanhMucModels.DT_Thang_CoThangKhong();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        DataTable dtNam = DanhMucModels.DT_Nam(false);
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

     
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
                                        <span>NHẬP SỐ LIỆU</span>
                                    </td>
                                    <td style="width: 59px; text-align: right;" valign="top">
                                        <b>Tháng&nbsp;</b>
                                    </td>
                                    <td valign="top">
                                        <%=MyHtmlHelper.DropDownList("CauHinh", slThang, Convert.ToString(iThang), "iThangLamViec", "", "onchange=\"ChonThangNam(this.value, 1)\" style=\"width:50px;\"")%>
                                        <b>&nbsp; Năm</b>
                                        <%=MyHtmlHelper.DropDownList("CauHinh", slNam, Convert.ToString(iNam), "iNamLamViec", "", "onchange=\"ChonThangNam(this.value, 2)\" style=\"width:80px;\"")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
                <td style="width: 17px">
                </td>
                <td style="width: 450px;" rowspan="4" valign="top">
                    <div class="box_tong">
                        <div class="title_tong">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td style="font-weight: bold;">
                                        <span>IN ẤN BÁO CÁO</span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="margin-left: 3px;">
                            <table width="100%" cellspacing="1" cellpadding="1">
                                <tr>
                                    <td colspan="4" style="height: 10px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;" valign="top">
                                        <img src="../../Content/Themes/images/KTTH_BangCanDoi.jpg" alt="" border="0" style="height: 20px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKeToanTongHop_CanDoiTaiKhoan">Bảng cân đối tài khoản</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKeToan_InBia">In bìa</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKeToanTongHop_SoCai_Cuc">Nhật ký - Sổ cái</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKT_ChiTiet_TaiKhoanTheoDonVi1">Biểu kỹ thuật 3</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKeToanTongHop_PhanHo">Phân hộ</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKTTH_SoChiTietBQL">Sổ ch.tiết T.khoản - BQL - Đơn vị </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKeToan_TongHopPhanHo">Tổng hợp phân hộ</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKeToan_SoCaiTaiKhoan">Sổ cái tài khoản</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKeToan_SoCaiChiTiet_Cuc">Sổ cái chi tiết</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKeToan_QuyetToanNam">Tổng hợp quyết toán NS năm</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptChiTietTheoDonVi_2">Chi tiết tài khoản/đơn vị</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKTTongHop_ChiTietCacKhoanTamUng">Chi tiết các khoản tạm ứng</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKTTongHop_ChiTiet_TongHopTaiKhoan_Cuc">Chi tiết + tổng hợp tài
                                            khoản</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKTTH_ChiTietPhaiThu">Chi tiết phải thu</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptTongHopTaiKhoan">Tổng hợp tài khoản</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKeToanTongHop_CanDoiThuChiTaiChinh">Cân đối thu chi tài chính</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../KeToanTongHop_GiaiThichSoDu/Edit">Giải thích số dư</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="../../../rptKTTK_ChiTietTamThu">Chi tiết tạm thu</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <a href="../../../rptKeToanTongHop_GiaiThichSoDuTaiKhoan">Giải thích số dư tài khoản</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <a href="../../../rptKTTH_TongHopCapVon">Tổng hợp cấp vốn</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="Tổng hợp vay vốn phải trả"
                                            border="0" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <a href="../../../rptKTTongHop_VonVayDauTu">Tổng hợp vốn vay đầu tư</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td colspan="4" style="height: 10px;">
                                    </td>
                                </tr>--%>
                            </table>
                        </div>
                    </div>
                </td>
                <td style="width: 14px">
                    &nbsp;
                </td>
                <td rowspan="4" valign="top">
                    <div class="box_tong" style="margin-right: 10px;">
                        <div class="title_tong">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td style="font-weight: bold;">
                                        <span>DANH MỤC HỆ THỐNG</span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="margin-left: 10px; float: left;">
                            <table width="100%" border="0" cellpadding="3" cellspacing="3">
                                <tr>
                                    <td colspan="2" style="height: 10px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;" valign="top">
                                        <img src="../../Content/Themes/images/KTTK_User.jpg" alt="" border="0" style="height: 20px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/DuToan">Cấu hình người dùng</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/DonVi">Danh mục Đơn vị</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/LoaiTaiKhoan">Danh mục Tài khoản kế toán</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/LoaiThongTri">Danh mục Loại thông tri</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/TuDien">Danh mục từ điển</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/KeToanTongHop_ThamSo">Danh mục Tham số hệ thống</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/TaiKhoanDanhMucChiTiet/List">Danh mục Chi tiết tài khoản</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/DanhMucChuKy">Danh mục chữ ký</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/DanhMuc_BaoCao_ChuKy">Cấu hình chữ ký báo cáo</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 10px;">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div style="padding-top: 10px;">
                        <table style="text-align: center;">
                            <tr>
                                <td>
                                    <img src="../../Content/Themes/images/Luong_DL.gif" alt="" border="0" style="height: 15px;
                                        width: 37px;" />
                                </td>
                                <td>
                                    <a href="/KeToanTongHop_KhoaSoKeToanNam" title="Khóa sổ kế toán năm">
                                        <img src="../../Content/Themes/images/KTTK_Lock.jpg" alt="" border="0" style="height: 59px;
                                            width: 78px" />
                                        <br />
                                        <span style="text-align: center; font-weight: bold;">Khóa sổ kế toán năm</span>
                                    </a>
                                </td>
                                <td style="width: 20px;"></td>
                                <td>
                                    <a href="/ConvertToFox" title="Chuyển số liệu kế toán từ chương trình mới sang chương trình cũ">
                                        <img src="../../Content/Themes/images/Convert.jpg" alt="" border="0" style="height: 59px;
                                            width: 78px" />
                                        <br />
                                        <span style="text-align: center; font-weight: bold;">Chuyển dữ liệu</span>
                                    </a>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="width: 59px;">
                    <div style="margin-left: 10px; width: 43px;">
                        <a href="/KeToanTongHop" title="Tạo chứng từ">
                            <img src="../../Content/Themes/images/Add.jpg" alt="Tạo chứng từ" border="0" style="width: 40px" />
                            <br />
                            <span style="text-align: center; font-weight: bold;">Tạo ch.từ</span> </a>
                    </div>
                    <br />
                </td>
                <td style="width: 33px">
                    &nbsp;
                </td>
                <td style="width: 59px; text-align: center;" rowspan="3" valign="top">
                    <a href="/KeToanTongHop_CanDoiThuChiTaiChinh" title="Cân đối tài chính">
                        <img src="../../Content/Themes/images/KTTH_CanDoi.jpg" alt="Tạo chứng từ" border="0"
                            width="50px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Bảng cân đối tài chính năm</span>
                    </a>
                    <br />
                    <img src="../../Content/Themes/images/Luong_DL_Down.gif" alt="" border="0" style="width: 9px;
                        height: 45px;" />
                    <br />
                    <img src="../../Content/Themes/images/Computer.jpg" alt="" border="0" style="width: 122px;" />
                    <br />
                    <img src="../../Content/Themes/images/Luong_DL_Up.gif" alt="" border="0" style="width: 9px;
                        height: 50px;" />
                    <br />
                    <a href="/KeToanTongHop_SoDuTaiKhoanGiaiThich" title="Số dư tài khoản giải thích">
                        <img src="../../Content/Themes/images/KTTH_AddNew.jpg" alt="Tạo chứng từ" border="0"
                            width="50px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Số dư TK.GT</span> </a>
                </td>
                <td style="width: 40px" rowspan="3">
                    <img src="../../../Content/Themes/images/KTTH_ROW.gif" alt="" border="0" style="width: 51px;
                        height: 109px; margin-top: 0px;" />
                </td>
                <td style="width: 53px" valign="bottom">
                    <div>
                        <a href="/rptKeToanTongHop_KiemTraChungTu" title="In kiểm số liệu">
                            <img src="../../../Content/Themes/images/KTTH_Search1.jpg" alt="" border="0" width="50px"
                                style="height: 33px; margin-top: 10px" />
                            <br />
                            <span style="text-align: center; font-weight: bold;">Kiểm tra</span> </a>
                    </div>
                </td>
                <td style="width: 19px">
                    &nbsp;
                </td>
                <td rowspan="3" style="width: 53px;" valign="bottom">
                    <a href="/KeToanTongHop_KhoaSoKeToan" title="Khóa sổ kế toán tháng">
                        <img src="../../../Content/Themes/images/KTTH_KhoaSo.jpg" alt="" border="0" style="height: 69px;
                            width: 69px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Khóa sổ</span> </a>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <a href="/KeToanTongHop_ChungTu/Index_TapSo" title="Tạo tập số ghi số">
                        <img src="../../Content/Themes/images/Add.jpg" alt="Tạo tập số ghi số" border="0"
                            width="50px" />
                        <span style="text-align: center; font-weight: bold;">Tạo tập GS</span> </a>
                </td>
                <td rowspan="3" style="width: 25px">
                    <img src="../../../Content/Themes/images/KTTH_ROW.gif" alt="" border="0" style="width: 39px;
                        height: 109px; margin-top: 0px;" />
                </td>
                <td style="width: 14px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <img src="../../Content/Themes/images/Luong_DL.gif" alt="" border="0" style="height: 15px;
                        width: 37px;" />
                </td>
                <td colspan="2">
                    <img src="../../Content/Themes/images/Luong_DL.gif" alt="" border="0" style="width: 68px;
                        height: 15px;" />
                </td>
                <td style="width: 14px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 33px">
                    &nbsp;
                </td>
                <td style="text-align: left; width: 53px;" valign="top">
                    <a href="/KeToanTongHop_TimKiem" title="Tìm kiếm chứng từ chi tiết">
                        <img src="../../../Content/Themes/images/KTTH_Search.jpg" alt="Tìm kiếm chứng từ"
                            border="0" style="height: 41px; width: 43px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Tìm kiếm</span> </a>
                </td>
                <td style="width: 19px">
                    &nbsp;
                </td>
                <td style="width: 14px">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function ChonThangNam(value, loai) {
            var iThangLamViec;
            var iNamLamViec;
            if (loai == 1) {
                iThangLamViec = value;
                iNamLamViec = document.getElementById('CauHinh_iNamLamViec').value;

            }
            else {
                iNamLamViec = value;

                iThangLamViec = document.getElementById('CauHinh_iThangLamViec').value;
            }

            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("UpdateCauHinhNamLamViec?MaND=#0&iThangLamViec=#1&iNamLamViec=#2","KeToanTongHop") %>');
            url = unescape(url.replace("#0", '<%=MaND %>'));
            url = unescape(url.replace("#1", iThangLamViec));
            url = unescape(url.replace("#2", iNamLamViec));

            $.getJSON(url, function (data) {
                jsKeToan_LoadLaiChungTu();
            });

        }
      
    </script>
</asp:Content>
