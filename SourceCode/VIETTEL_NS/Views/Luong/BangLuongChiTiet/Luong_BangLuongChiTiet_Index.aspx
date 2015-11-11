<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.Shared" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/Luong/jsLuong.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <%
        String iID_MaBangLuong = Convert.ToString(ViewData["iID_MaBangLuong"]);
        if (String.IsNullOrEmpty(iID_MaBangLuong)) iID_MaBangLuong = Request.QueryString["iID_MaBangLuong"];

        String MaND = User.Identity.Name;
        NameValueCollection data = LuongModels.LayThongTinBangLuong(iID_MaBangLuong);
        int iNamBangLuong = Convert.ToInt16(data["iNamBangLuong"]);
        int iThangBangLuong = Convert.ToInt16(data["iThangBangLuong"]);
        int iID_MaTrangThaiDuyet_TuChoi = BangLuongChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaBangLuong);
        int iID_MaTrangThaiDuyet_TrinhDuyet = BangLuongChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaBangLuong);

        String strURL_ChiTiet = Url.Action("ChiTiet", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });
        String strURL_BaoHiem = Url.Action("BaoHiem", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });
        String strURL_ThueTNCN = Url.Action("ThueTNCN", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });
        String strURL_TruyLinh = Url.Action("TruyLinh", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });

        String strURL_TrichLuong = Url.Action("TrichLuong", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });
        String strURL_DieuChinhTienAn = Url.Action("DieuChinhTienAn", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });
        String strURL_HeSoKhuVuc = Url.Action("HeSoKhuVuc", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });
        String strURL_HuyTapThe = Url.Action("HuyTapThe", "Luong_BangLuongChiTiet", new { iID_MaBangLuong = iID_MaBangLuong });


        String strURL_NopThueDauVao = Url.Action("ViewPDF", "rptLuong_DSCaNhan_NopThueDauVao", new { iMaBL = iID_MaBangLuong });
        String strURL_ThuLaoThuong = Url.Action("ViewPDF", "rptLuong_DSCaNhan_CoThuLao_Thuong", new { iID_MaBangLuong = iID_MaBangLuong });
        String strURL_NguoiPhuThuoc = Url.Action("ViewPDF", "rptLuong_DanhSachCaNhan_NguoiPhuThuoc", new { MaBL = iID_MaBangLuong });
        
        
    %>
    <script type="text/javascript">
        $(document).ready(function () {
            jsLuong_Url = '<%=strURL_ChiTiet%>';
            jsLuong_Url_ThemMoiCanBo = '<%=Url.Action("ThemCanBo", "Luong_BangLuongChiTiet_CanBo", new { iID_MaBangLuong = iID_MaBangLuong }) %>';
            jsLuong_Url_TrichLuong = '<%=strURL_TrichLuong%>';
            jsLuong_Url_DieuChinhTienAn = '<%=strURL_DieuChinhTienAn%>';
            jsLuong_Url_HeSoKhuVuc = '<%=strURL_HeSoKhuVuc%>';
            jsLuong_Url_HuyTapThe = '<%=strURL_HuyTapThe%>';
            jsLuong_iID_MaBangLuong = '<%=iID_MaBangLuong%>';

            jsLuong_URL_ChiTiet = '<%=strURL_ChiTiet%>';
            jsLuong_URL_BaoHiem = '<%=strURL_BaoHiem%>';
            jsLuong_URL_ThueTNCN = '<%=strURL_ThueTNCN%>';

            jsLuong_Url_NopThueDauVao = '<%=strURL_NopThueDauVao%>';
            jsLuong_Url_ThuLaoThuong = '<%=strURL_ThuLaoThuong%>';
            jsLuong_Url_NguoiPhuThuoc = '<%=strURL_NguoiPhuThuoc%>';

            jsLuong_BangDuLieu_fnSetFocus();
        });
    </script>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 110px;">
                <div style="padding-left: 10px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237; float: left;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Luong_BangLuong", new { iNamBangLuong = iNamBangLuong, iThangBangLuong = iThangBangLuong }), "D.sách bảng lương")%>
                </div>
                <div style="float: right; padding-bottom: 5px; color: #ec3237; padding-right: 10px;
                    font-size: 11px;">
                    <span style="color: Navy;"><b>F1</b> ND Thù lao - Thưởng </span>| <span style="color: Navy;">
                        <b>F6</b> Trích lương </span>| <span style="color: Navy;"><b>F7</b> Điều chỉnh TT
                    </span>| <span style="color: Navy;"><b>F8</b> Ăn TT </span>| <span style="color: Navy;">
                        <b>F9</b> In DS Người PT </span>| <span style="color: Navy;"><b>F11</b> Hủy TT
                    </span>| <span style="color: Navy;"><b>F12</b> ND Nộp thuế đầu vào</span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; float: left;">
        <div style="width: 100%; float: left;">
            <div class="box_tong">
                <%--<div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td width="10px">
                                &nbsp;
                            </td></tr></table></div>--%>
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td width="10px">
                                &nbsp;
                            </td>
                            <td style="width: 150px;">
                                <%=MyHtmlHelper.TextBox("CauHinh", "NHẬP BẢNG LƯƠNG", "iID_MaChungTu", null, "class=\"input1_2\" style=\"border:none;text-align: left;background-color:#dff0fb;font-weight:bold;color: #3b5998; font-size:11px;\"")%>
                            </td>
                            <td style="width: 180px;">
                                <b style="font-size:11px;">Tháng</b> &nbsp;
                                <%=MyHtmlHelper.TextBox("", iThangBangLuong.ToString(), "", "", " khong-nhap='1'  class=\"input1_2\" style=\"width: 40px; text-align:center;background-color:#f8e6d1;font-weight:bold;\"")%>
                                <b style="font-size:11px;">Năm</b> &nbsp;
                                <%=MyHtmlHelper.TextBox("", iNamBangLuong.ToString(), "", "", " khong-nhap='1'  class=\"input1_2\" style=\"width: 40px;text-align:center;background-color:#f8e6d1;font-weight:bold;\"")%>
                            </td>
                            <td align="left" style="width: 130px;">
                                <button class='button_title' style="float: left; font-weight: bold; cursor: pointer;font-size:11px;"
                                    onclick="javascript:return jsLuong_ThayDoiUrl('<%=strURL_ChiTiet%>');">
                                    1.Nhập Lương</button>
                            </td>
                            <td align="left" style="width: 150px;">
                                <button class='button_title' style="float: left; font-weight: bold; cursor: pointer;font-size:11px;"
                                    onclick="javascript:return jsLuong_ThayDoiUrl('<%=strURL_BaoHiem%>');">
                                    2.Nhập Bảo hiểm</button>
                            </td>
                            <td align="left" style="width: 120px;">
                                <button class='button_title' style="float: left; font-weight: bold; cursor: pointer;font-size:11px;"
                                    onclick="javascript:return jsLuong_ThayDoiUrl('<%=strURL_ThueTNCN%>');">
                                    3.Nhập Thuế</button>
                            </td>
                            <td align="left" style="display: none;">
                                <button class='button_title' style="float: left; font-weight: bold; cursor: pointer;font-size:11px;"
                                    onclick="javascript:return jsLuong_ThayDoiUrl('<%=strURL_TruyLinh%>');">
                                    4.Truy lĩnh</button>
                            </td>
                            <td align="left" style="width: 110px;">
                                <button class='button_title' style="float: left; font-weight: bold; cursor: pointer;font-size:11px;"
                                    onclick="javascript:location.href='<%=Url.Action("UpdateSSL", "Luong_BangLuongChiTiet", new { iID_MaBangLuong= iID_MaBangLuong })%>';">
                                    4.Tạo SSL</button>
                            </td>
                            <td align="left" style="width: 120px;">
                                <button class='button_title' style="float: left; font-weight: bold; cursor: pointer;font-size:11px;"
                                    onclick="javascript:return jsLuong_Dialog_ThemMoiCanBo_Show();">
                                    F2. Thêm CB</button>
                            </td>
                            <td>
                            </td>
                            <%-- <td>
                                F6 Trích lương
                            </td>
                            <td>
                                F7 Điều chỉnh hệ số khu vực
                            </td>
                            <td>
                                F8 Điều chỉnh tiền ăn
                            </td>
                            <td>
                                F11 Hủy tập thể
                            </td>--%>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <iframe id="ifrChiTiet" width="100%" height="600px" src="<%= Url.Action("ChiTiet", "Luong_BangLuongChiTiet", new {iID_MaBangLuong = iID_MaBangLuong})%>">
                        </iframe>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divLuong_BangLuongChiTiet" style="display: none;">
        <iframe id="BangLuongChiTiet_iFrame" src="" width="99%" height="97%"></iframe>
    </div>
</asp:Content>
