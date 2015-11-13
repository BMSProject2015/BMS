<%--<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
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
                <td style="width: 277px;">
                    &nbsp;
                </td>
                <td style="width: 59px; text-align: center;" valign="top">
                    &nbsp;
                </td>
                <td style="text-align: center; width: 332px;">
                    <a href="http://192.104.78.11:1111" title="Quản trị dữ liệu lịch sử">
                        <img src="../../Content/Themes/images/Thanh_LapDT.gif" alt="Quản trị dữ liệu lịch sử"
                            border="0" style="height: 75px; width: 97px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Lập dự toán Ngân sách</span>
                    </a>
                </td>
                <td style="width: 92px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="width: 19px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 277px;">
                    &nbsp;
                </td>
                <td style="text-align: center;" valign="top" colspan="3">
                    &nbsp;
                    &nbsp;<img alt="Quản trị dữ liệu lịch sử" border="0" src="../../Content/Themes/images/Thanh_Luong1.gif"
                        style="height: 37px; width: 444px;" />
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="width: 19px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 277px;">
                    &nbsp;
                </td>
                <td style="text-align: center;" valign="top">
                    <a href="http://192.104.78.11:1111" title="Quản trị dữ liệu lịch sử">
                        <img alt="Quản trị dữ liệu lịch sử" border="0" src="../../Content/Themes/images/Thanh_CTNS.gif"
                            style="height: 68px; width: 104px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Chỉ tiêu NS</span> </a>
                </td>
                <td style="text-align: center; width: 332px;">
                    &nbsp;
                </td>
                <td style="text-align: center; width: 92px;" valign="top">
                    <a href="http://192.104.78.11:1111" title="Quản trị dữ liệu lịch sử">
                        <img alt="Quản trị dữ liệu lịch sử" border="0" src="../../Content/Themes/images/Thanh_ChiTieuBS.gif"
                            style="height: 75px; width: 99px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Chỉ tiêu BS</span> </a>
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="width: 19px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 277px;">
                    &nbsp;
                </td>
                <td style="text-align: center;" valign="top" colspan="3">
                    &nbsp;
                   <img alt="Quản trị dữ liệu lịch sử" border="0" src="../../Content/Themes/images/Thanh_Luong2.gif"
                        style="height: 37px; width: 444px;" />
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="width: 19px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 277px;">
                    &nbsp;
                </td>
                <td style="width: 59px; text-align: center;" valign="top">
                    &nbsp;
                </td>
                <td style="text-align: center; width: 332px;">
                    <a href="http://192.104.78.11:1111" title="Quản trị dữ liệu lịch sử">
                        <img alt="Quản trị dữ liệu lịch sử" border="0" src="../../Content/Themes/images/Thanh_TBTC.gif"
                            style="height: 46px; width: 84px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Thông báo TC</span> </a>
                    <br />
                    <img alt="Quản trị dữ liệu lịch sử" border="0" src="../../Content/Themes/images/Luong_DL_Down.gif"
                        style="height: 23px; width: 12px;" />
                    <br />
                    <a href="http://192.104.78.11:1111" title="Quản trị dữ liệu lịch sử">
                        <img alt="Quản trị dữ liệu lịch sử" border="0" src="http://localhost:41662/Content/Themes/images/Thanh_CapNS.gif"
                            style="height: 46px; width: 84px" /><br />
                        <span style="text-align: center; font-weight: bold;">Cấp NS</span></a>
                    <br />
                   <img alt="Quản trị dữ liệu lịch sử" border="0" src="../../Content/Themes/images/Luong_DL_Down.gif"
                        style="height: 23px; width: 12px;" />
                    <br />
                    <a href="http://192.104.78.11:1111" title="Quản trị dữ liệu lịch sử">
                        <img alt="Quản trị dữ liệu lịch sử" border="0" src="http://localhost:41662/Content/Themes/images/Thanh_QTNS.gif"
                            style="height: 46px; width: 84px" /><br />
                        <span style="text-align: center; font-weight: bold;">Quyết toán NS</span></a>
                        <br />
                   <img alt="Quản trị dữ liệu lịch sử" border="0" src="../../Content/Themes/images/Luong_DL_Down.gif"
                        style="height: 23px; width: 12px;" />
                    <br />
                    <a href="http://192.104.78.11:1111" title="Quản trị dữ liệu lịch sử">
                        <img alt="Quản trị dữ liệu lịch sử" border="0" src="http://localhost:41662/Content/Themes/images/Thanh_THNS.gif"
                            style="height: 46px; width: 84px" /><br />
                        <span style="text-align: center; font-weight: bold;">Tổng hợp NS</span></a>
                </td>
                <td style="width: 92px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="width: 19px">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
--%>

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
       
        if (dtCauHinh.Rows.Count > 0)
        {
            iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
            
        }
        dtCauHinh.Dispose();

      
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
                <td colspan="11" style="font-weight: bold;" valign="top">
                    <div class="box_tong" style="margin-left: 5px;">
                        <div class="title_tong">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <span>NHẬP SỐ LIỆU</span>
                                    </td>
                                    <td style="text-align: right;" valign="middle">
                                        <b>Năm làm việc:&nbsp;&nbsp;</b>
                                    </td>
                                    <td valign="top">
                                        <%=MyHtmlHelper.DropDownList("CauHinh", slNam, Convert.ToString(iNam), "iNamLamViec", "", "onchange=\"ChonThangNam(this.value, 2)\" style=\"width:80px;\"")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
                <td style="width: 17px">
                </td>
                <td rowspan="4" valign="top">
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
                            <table width="100%" cellspacing="4" cellpadding="4">
                                <tr>
                                    <td colspan="4" style="height: 10px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;" valign="top">
                                        <img src="../../Content/Themes/images/KTTH_BangCanDoi.jpg" alt="" border="0" style="height: 20px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_01CT"><b>1.</b> Báo cáo danh mục công trình (Mẫu 01/CT) </a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_03_CP"><b>11.</b>BC chi tiêt cấp phát VĐT năm (Mẫu 06/CP) </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_ChiTiet_DanhMucCongTrinh"><b>2.</b> Báo cáo chi tiết danh mục công
                                            trình (Mẫu 02/CT) </a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptKHV_Bieu01VDT"><b>12.</b> Báo cáo tình hình TH VĐT năm (Mẫu 01/VĐT-N)
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_ChiTiet_DanhMucCongTrinh_1"><b>3.</b> Báo cáo chi tiết danh mục công
                                            trình - chọn QĐ (Mẫu 02/CT) </a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptKHV_Bieu01VDTUngTruoc"><b>13.</b> Báo cáo tình hình TH VĐT ứng trước năm
                                            (Mẫu 02/VĐT-N) </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_01DT_KHVDT"><b>4.</b> Kế hoạch VĐT năm (Mẫu 01/DT)</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptKHV_01VDT_THDACT"><b>14.</b> Báo cáo tổng hợp TH VĐT CT-DA (Mẫu 01/TH-VĐT)</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptKHV_02DT_DTQuy"><b>5.</b> Dự toán quý (Mẫu 02/DT) </a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptKHV_02VDT_THNSNN"><b>15.</b> Báo cáo tổng hợp TH NSNN (Mẫu 02/TH-VĐT)
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_01_CP"><b>6.</b> Đề nghị cấp phát (Mẫu 01/CP) </a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_02_QTHT"><b>16.</b> Báo cáo Tổng hợp quyết toán công trình, dự án
                                            hoàn thành (Mẫu 02/QTHT) </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_02_CPVDT"><b>7.</b> BC chi tiêt cấp phát VĐT năm (Mẫu 02/CP)</a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptKHV_Bieu02VDT"><b>17.</b> Báo cáo Tổng hợp tình hình thực hiện nhà nước
                                            giao (Mẫu 02/TH_VDT) </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_03_CP_1"><b>8.</b> BC chi tiêt cấp phát VĐT năm (Mẫu 03/CP) </a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_01QTN"><b>18.</b> Báo cáo Tổng hợp quyết toán vốn đầu tư (Mẫu 01/QTN)
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_04_CP"><b>9.</b> BC chi tiêt CP VĐT theo ĐV TH (Mẫu 04/CP) </a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_02QTN"><b>19.</b> Báo cáo Tổng hợp quyết toán năm (Mẫu 02/QTN_Tu)
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 33px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_05_CP"><b>10.</b> BC chi tiêt CP VĐT theo ĐV TH (Mẫu 05/CP) </a>
                                    </td>
                                    <td style="float: right; width: 37px;">
                                        <img src="../../Content/Themes/images/KTTH_Nhatky.png" alt="" border="0" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <a href="/rptQLDA_ThongBaoKHV"><b>20.</b> QTDA - Thông báo kế hoạch vốn năm</a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </td>
                <td style="width: 14px">
                    &nbsp;
                </td>
                <td rowspan="4" valign="top" style="width: 230px;">
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
                                        <a href="/LoaiDanhMuc/Detail?MaLoaiDanhMuc=bc5f9bb8-1dba-40fc-95b6-1098e5b0e68f">Danh mục Khối quản lý</a>
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
                                        <a href="/QLDA_ChuDauTu">Danh mục chủ đầu tư</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/QLDA_DonViThiCong">Danh mục Nhà thầu</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/QLDA_DanhMucDuAn">Danh mục Dự án</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/QLDA_BanQuanLy">Danh mục BQL Dự án</a>
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
                                        <a href="/DanhMuc_BaoCao_ChuKy">Danh mục báo cáo - chữ ký</a>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/Muclucngansach/edit">Mục lục ngân sách</a>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/KeToanTongHop_ThamSo">Cấu hình tham số</a>
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
                    </div>
                </td>
            </tr>
            <tr>
                <td rowspan="3" style="width: 59px; text-align: center;">
                    <div style="margin-left: 10px; width: 43px;">
                        <a href="/QLDA_TongDauTu/List" title="Lập tổng đầu tư">
                            <img src="../../Content/Themes/images/DA_DauTu.jpg" alt="Lập tổng đầu tư" border="0"
                                style="width: 49px; height: 60px;" />
                            <%-- <br />--%>
                            <span style="text-align: center; font-weight: bold;">Lập tổng đầu tư</span> </a>
                    </div>
                    <br />
                </td>
                <td style="width: 22px">
                    &nbsp;
                </td>
                <td rowspan="3" style="width: 59px; text-align: center;">
                    <br />
                    <div style="margin-left: 10px; width: 43px;">
                        <a href="/QLDA_TongDuToan/List" title="Lập tổng dự toán">
                            <img src="../../Content/Themes/images/DA_DuToan.jpg" alt="Lập tổng dự toán" border="0"
                                style="width: 47px; height: 49px;" />
                            <%--  <br />--%>
                            <span style="text-align: center; font-weight: bold;">Lập tổng dự toán</span> </a>
                    </div>
                    <br />
                </td>
                <td style="width: 26px">
                    &nbsp;
                </td>
                <td style="width: 59px; text-align: center;" rowspan="3" valign="top">
                <br/>
                    <a href="/QLDA_DuToan_Quy" title="Dự toán quý">
                        <img src="../../Content/Themes/images/KTTH_CanDoi.jpg" alt="Dự toán quý" border="0"
                            style="width: 40px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Dự toán quý</span> </a>
                    <br />
                    <img src="../../Content/Themes/images/Luong_DL_Up.gif" alt="" border="0" style="width: 9px;
                        height: 50px;" />
                    <br />
                    <a href="/QLDA_DuToan_Nam" title="Dự toán năm">
                        <img src="../../Content/Themes/images/KTTH_AddNew.jpg" alt="Dự toán năm" border="0"
                            width="50px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Dự toán năm</span> </a>
                    <br />
                    <img src="../../Content/Themes/images/Luong_DL_Down.gif" alt="" border="0" style="width: 9px;
                        height: 45px;" />
                    <div style="margin-left: 10px; width: 43px;">
                        <a href="/QLDA_KeHoachVon" title="Kế hoạch vốn">
                            <img src="../../Content/Themes/images/DA_KeHoachVon.gif" alt="Kế hoạch vốn" border="0"
                                style="width: 41px; height: 37px;" />
                            <span style="text-align: center; font-weight: bold;">Kế hoạch vốn</span> </a>
                    </div>
                </td>
                <td style="width: 22px">
                    &nbsp;
                </td>
                <td rowspan="3" style="width: 59px; text-align: center;">
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <div style="margin-left: 10px; width: 43px;">
                        <a href="/QLDA_CapPhat" title="Cấp phát">
                            <img src="../../Content/Themes/images/DA_CapPhat.jpg" alt="Cấp phát" border="0" style="width: 40px;
                                height: 40px;" />
                            <br />
                            <span style="text-align: center; font-weight: bold;">Cấp phát</span> </a>
                    </div>
                    <div style="margin-left: 10px; width: 43px;">
                        <img src="../../Content/Themes/images/Luong_DL_Up.gif" alt="" border="0" style="width: 9px;
                            height: 50px;" />
                    </div>
                    <div style="margin-left: 10px; width: 43px;">
                        <a href="/QLDA_HopDong/List" title="Hợp đồng">
                            <img src="../../Content/Themes/images/Add.jpg" alt="Hợp đồng" border="0" style="width: 40px" />
                            <br />
                            <span style="text-align: center; font-weight: bold;">Hợp đồng</span> </a>
                    </div>
                    <br />
                </td>
                <td style="width: 40px" rowspan="3">
                    <img src="../../../Content/Themes/images/DA_Luong1.gif" alt="" border="0" style="width: 51px;
                        height: 181px; margin-top: 0px;" />
                    <br />
                </td>
                <td style="width: 70px" valign="bottom">
                    <div>
                        <a href="/QLDA_QuyetToan" title="Quyết toán">
                            <img src="../../../Content/Themes/images/KTTH_KhoaSo.jpg" alt="" border="0" style="height: 51px;
                                margin-top: 10px; width: 62px;" />
                            <br />
                            <span style="text-align: center; font-weight: bold;">Quyết toán</span> </a>
                    </div>
                </td>
                <td style="width: 4px">
                    &nbsp;
                </td>
                <td rowspan="3" style="width: 53px;" valign="middle">
                    <img src="../../Content/Themes/images/Computer.jpg" alt="" border="0" style="width: 76px;
                        height: 80px;" />
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
                <td valign="middle" style="width: 22px">
                    <img src="../../Content/Themes/images/Luong_DL.gif" alt="" border="0" style="height: 15px;
                        width: 25px;" />
                </td>
                <td valign="middle" style="width: 22px">
                    <img src="../../Content/Themes/images/Luong_DL.gif" alt="" border="0" style="height: 15px;
                        width: 25px;" />
                </td>
                <td valign="top" style="width: 26px">
                </td>
                <td colspan="2">
                    <br />
                    <img src="../../Content/Themes/images/Luong_DL.gif" alt="" border="0" style="width: 68px;
                        height: 15px;" />
                </td>
                <td style="width: 14px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 22px">
                    &nbsp;
                </td>
                <td style="width: 22px">
                    &nbsp;
                </td>
                <td style="width: 26px">
                    <div style="margin-left: 5px;">
                        <img src="../../Content/Themes/images/Luong_DL.gif" alt="" border="0" style="height: 15px;
                            width: 25px;" /></div>
                </td>
                <td style="text-align: left;" valign="top" colspan="2">
                    <a href="/QLDA_QuyetToanHoanThanh" title="Tìm kiếm chứng từ chi tiết">
                        <%--<img src="../../../Content/Themes/images/KTTH_Search.jpg" alt="Tìm kiếm chứng từ"
                            border="0" style="height: 41px; width: 43px" />--%>
                        <img src="../../Content/Themes/images/DA_QuyetToan.jpeg" alt="" border="0" style="height: 49px;
                            margin-top: 10px; width: 67px;" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Quyết toán hoàn thành</span>
                    </a>&nbsp;
                </td>
                <td style="width: 14px">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function ChonThangNam(value, loai) {
            var iThangLamViec = 1;
            var iNamLamViec;
            iNamLamViec = document.getElementById('CauHinh_iNamLamViec').value;
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
