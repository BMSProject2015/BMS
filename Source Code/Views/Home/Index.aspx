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
               <%-- <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Luồng dữ liệu: ")%></b>
                </div>--%>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <%--<div style="width: 100%;">
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
        </table>
    </div>
    <div style="width: 100%;">
        <table width="100%" cellspacing="3" cellpadding="3" border="0">
            <tr>
                <td rowspan="3" valign="top" style="width: 9px">
                   
                </td>
                <td rowspan="3">
                 
                  
                    <div style=" width: 161px; float:right; text-align:center;">
                        <a href="/ThuNop_ChungTu/Index?iLoai=2" title="Tạo chứng từ">
                            <img src="../../Content/Themes/images/Add.jpg" alt="Nhập nội dung thu nộp" border="0"
                                style="width: 40px" />
                            <br />
                            <span style="text-align: center; font-weight: bold;">Nhập số liệu thu nộp</span>
                        </a>
                    </div>
                  <br />
                  
                   
                </td>
                <td style="width: 33px">
                    &nbsp;
                </td>
                <td style="width: 122px; text-align: center;" rowspan="3" valign="top">
                    <a href="/DuToan_ChungTu/?sLNS=801" title="Nhập dự toán Thu nộp NSQP">
                        <img src="../../Content/Themes/images/KTTH_CanDoi.jpg" alt="Nhập dự toán Thu nộp NSQP" border="0"
                            width="50px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Nhập dự toán Thu nộp NSQP</span>
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
                    <a href="/DuToan_ChungTu/?sLNS=802" title="Nhập dự toán Thu nộp NSNN">
                        <img src="../../Content/Themes/images/KTTH_AddNew.jpg" alt="Tạo chứng từ" border="0"
                            width="50px" />
                        <br />
                        <span style="text-align: center; font-weight: bold;">Nhập dự toán Thu nộp NSNN</span>
                    </a>
                </td>
                <td rowspan="3" style="width: 50px">
                
                 
                    <img src="../../../Content/Themes/images/KTTH_ROW.gif" alt="" border="0" style="width: 50px;
                        height: 109px; margin-top: 0px;" />
                  
                   
                </td>
                <td style="width: 19px;" rowspan="3" valign="top">
                  
                </td>
                <td style="width: 270px;" rowspan="3" valign="top">
                  
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
                        <div style="margin-left: 3px; height: 350px; float: left;">
                            <table width="100%" border="0" cellpadding="3" cellspacing="3">
                                <tr>
                                    <td colspan="2" style="height: 10px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 38px;" valign="top">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptThuNop_Thu01">Báo cáo Thu biểu 01</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 38px;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptThuNop_Thu02">Báo cáo Thu biểu 02</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right; width: 38px;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/rptThuNop_KeHoachNganSach">Biểu giao ban</a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div></td>
                <td style="width: 8px;" rowspan="3" valign="top">
                  
                      &nbsp;</td>
                <td style="width: 270px;" rowspan="3" valign="top">
                  
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
                        <div style="margin-left: 10px; float: left;height: 350px; ">
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
                                        <a href="/KeToanTongHop_ThamSo">Tham số hệ thống</a>
                                    </td>
                                </tr>
                                 <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/ThuNopCauHinhBaoCao/?iLoai=1">Cấu hình báo cáo thu 02</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="float: right;">
                                        <img src="../../Content/Themes/images/AddNew.jpg" alt="" border="0" style="height: 25px;" />&nbsp;
                                    </td>
                                    <td>
                                        <a href="/ThuNopCauHinhBaoCao/?iLoai=2">Cấu hình báo cáo giao ban</a>
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
                    </div></td>
            </tr>
            <tr>
                <td valign="middle">
                    <img src="../../Content/Themes/images/Luong_DL.gif" alt="" border="0" style="height: 15px;
                        width: 75px; float:right;" />
                    <br />
                    <br />
                   
                    
                </td>
            </tr>
            <tr>
                <td style="width: 33px">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>--%>
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
