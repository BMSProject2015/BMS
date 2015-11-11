<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/KeToanCongSan/jsKeToanCongSan_ChungTuChiTietTuChoi.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <%
        int i;
        String MaND = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        int iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
        int iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
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
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanChiTietTienMat"), "Danh sách chứng từ ghi sổ")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách chứng từ TỪ CHỐI</span>
                    </td>
                    <td align="right" style="padding-right: 8px;">
                        Số chứng từ tìm kiếm
                    </td>
                    <td style="padding-right: 8px;">
                        <input id="txtSoChungTu" class="textbox_uploadbox" onkeypress='jsKeToan_ChungTuChiTietTuChoi_Search_onkeypress(event)' />
                    </td>
                    <td>
                        Tháng
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList("CauHinh", slThang, Convert.ToString(iThang), "iThangLamViec", "", "onchange=\"ChonThangNam(this.value, 1)\"")%>
                    </td>
                    <td>
                        Năm
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList("CauHinh", slNam, Convert.ToString(iNam), "iNamLamViec", "", "onchange=\"ChonThangNam(this.value, 2)\"")%>
                    </td>
                    <td>
                        <%--<span>Tháng
                            <%=iThang %>
                            (F2:Thêm mới - Delete:Xóa - F10:Lưu)</span>--%>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 100%; float: left; margin-right: 15px;">
            <iframe id="ifrChungTu" width="100%" height="660px" src="<%= Url.Action("ChungTuChiTietTuChoi_Frame", "KTCT_TienMat_ChungTuChiTiet")%>">
            </iframe>
        </div>
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
            var url = unescape('<%= Url.Action("UpdateCauHinhNamLamViec?MaND=#0&iThangLamViec=#1&iNamLamViec=#2","KeToanChiTietTienMat") %>');
            url = unescape(url.replace("#0", '<%=MaND %>'));
            url = unescape(url.replace("#1", iThangLamViec));
            url = unescape(url.replace("#2", iNamLamViec));

            $.getJSON(url, function (data) {
                jsKeToan_LoadLaiChungTu();
            });

        }

        function location_reload() {
            location.reload();
        }

        $(document).ready(function () {
            jsKeToan_url_ChungTuChiTiet = '<%= Url.Action("ChungTuChiTietTuChoi_Frame", "KTCT_TienMat_ChungTuChiTiet")%>';
            jsKeToan_url_KeToanTongHop = '<%= Url.Action("Index", "KeToanChiTietTienMat")%>';
        });
    </script>
</asp:Content>
