<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/KeToanTongHop/jsBang_SoDuTaiKhoanChiTiet.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <%
        String ControlID = "ChungTuChiTiet";
        String ParentID = ControlID + "_Search";
        String MaND = User.Identity.Name;
        String IPSua = Request.UserHostAddress;
        String iNamLamViec = Convert.ToString(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iNamLamViec"));
        int iThang = Convert.ToInt32(NguoiDungCauHinhModels.ThangTinhSoDu_TKChiTiet(iNamLamViec));
        DataTable dtNam = DanhMucModels.DT_Nam(false);
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();
        String iID_MaTaiKhoan = Request.QueryString["iID_MaTaiKhoan"];
        if (String.IsNullOrEmpty(iID_MaTaiKhoan))
        {
            iID_MaTaiKhoan = "-1";
        }
        var tbl = TaiKhoanModels.DT_DSTaiKhoanCha_SoDuGiaiThich(true, "Tất cả", User.Identity.Name, false);
        SelectOptionList slTaiKhoan = new SelectOptionList(tbl, "iID_MaTaiKhoan", "sTen");
        if (tbl != null) tbl.Dispose();
        KeToanTongHop_GiaiThich_DauNam_BangDuLieu bang = new KeToanTongHop_GiaiThich_DauNam_BangDuLieu(iNamLamViec,
                                                                                                       iID_MaTaiKhoan,
                                                                                                       MaND, IPSua);



        String BangID = "BangDuLieu";
        int Bang_Height = 570;
        int Bang_FixedRow_Height = 50;
        String UrlGiaiThich = Url.Action("Index", "KeToanTongHop_SoDuTaiKhoanGiaiThich");
        String URL = Url.Action("SoDoLuong", "KeToanTongHop");
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop"), "Danh sách chứng từ ghi sổ")%>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table width="100%" border="0">
                <tr>
                    <td width="228px">
                        <span>Số dư chi tiết Tài khoản:</span>
                    </td>
                    <td width="322px">
                        <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, Convert.ToString(iID_MaTaiKhoan), "iID_MaTaiKhoan", "", "onchange=\"ChonTaiKhoan(this.value)\" style=\"width:300px;\"")%>
                    </td>
                    <td width="100px" align="right">
                        <b>Tổng cộng:</b>
                    </td>
                    <td style="width: 150px;">
                        <span style="font-weight: bold" id="lblTongSo"></span>
                    </td>
                    <td align="right">
                        <b>Số dư đến tháng: &nbsp;</b>
                    </td>
                    <td width="50px">
                        <%=MyHtmlHelper.TextBox(ParentID, iThang, "", "", "class=\"input1_2\" style=\"width: 50px; \" disabled=\"disabled\"")%>
                    </td>
                    <td align="right">
                        <b>Năm: &nbsp;</b>
                    </td>
                    <td width="70px">
                        <%=MyHtmlHelper.DropDownList(ParentID, slNam, Convert.ToString(iNamLamViec), "iNam", "", "onchange=\"ChonThangNam(this.value)\" style=\"width:70px;\"")%>
                    </td>
                    <td width="10px">
                    </td>
                    <td width="20px">
                        <input type="button" id="btnLuu" class="button" onclick="javascript:return Bang_HamTruocKhiKetThuc();"
                            value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                    </td>
                    <td width="10px">
                    </td>
                    <td>
                        <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="Huy()" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <%Html.RenderPartial("~/Views/Shared/BangDuLieu/BangDuLieu.ascx", new { BangID = BangID, bang = bang, Bang_Height = Bang_Height, Bang_FixedRow_Height = Bang_FixedRow_Height }); %>
                <div style="display: none;">
                    <input type="hidden" id="idXauDoRongCot" value="<%=HttpUtility.HtmlEncode(bang.strDSDoRongCot)%>" />
                    <input type="hidden" id="idXauKieuDuLieu" value="<%=HttpUtility.HtmlEncode(bang.strType)%>" />
                    <input type="hidden" id="idXauChiSoCha" value="<%=HttpUtility.HtmlEncode(bang.strCSCha)%>" />
                    <input type="hidden" id="idBangChiDoc" value="<%=HttpUtility.HtmlEncode(bang.strChiDoc)%>" />
                    <input type="hidden" id="idXauEdit" value="<%=HttpUtility.HtmlEncode(bang.strEdit)%>" />
                    <input type="hidden" id="idViewport_N" value="<%=HttpUtility.HtmlEncode(bang.Viewport_N)%>" />
                    <input type="hidden" id="idNC_Fixed" value="<%=HttpUtility.HtmlEncode(bang.nC_Fixed)%>" />
                    <input type="hidden" id="idNC_Slide" value="<%=HttpUtility.HtmlEncode(bang.nC_Slide)%>" />
                    <input type="hidden" id="idCoCotTongSo" value="0" />
                    <%  
                        if (bang.ChiDoc == false)
                        {
                    %>
                    <form action="<%=Url.Action("DetailSubmit", "KeToanTongHop_SoDuTaiKhoanGiaiThich", new{iNamLamViec=iNamLamViec})%>"
                    method="post">
                    <%
                        } %>
                    <input type="hidden" id="iID_MaChungTu" name="iID_MaChungTu" value="<%=iNamLamViec%>" />
                    <input type="hidden" id="sSoChungTu" name="sSoChungTu" value="" />
                    <input type="hidden" id="iNgay" name="iNgay" value="" />
                    <input type="hidden" id="iThang" name="iThang" value="<%=iThang %>" />
                    <input type="hidden" id="iTapSo" name="iTapSo" value="" />
                    <input type="hidden" id="sDonVi" name="sDonVi" value="" />
                    <input type="hidden" id="sNoiDung" name="sNoiDung" value="" />
                    <input type="hidden" id="idAction" name="idAction" value="0" />
                    <input type="hidden" id="idXauDuLieuThayDoi" name="idXauDuLieuThayDoi" value="<%=HttpUtility.HtmlEncode(bang.strThayDoi)%>" />
                    <input type="hidden" id="idXauLaHangCha" name="idXauLaHangCha" value="<%=HttpUtility.HtmlEncode(bang.strLaHangCha)%>" />
                    <input type="hidden" id="idXauMaCacHang" name="idXauMaCacHang" value="<%=HttpUtility.HtmlEncode(bang.strDSMaHang)%>" />
                    <input type="hidden" id="idXauMaCacCot" name="idXauMaCacCot" value="<%=HttpUtility.HtmlEncode(bang.strDSMaCot)%>" />
                    <input type="hidden" id="idXauGiaTriChiTiet" name="idXauGiaTriChiTiet" value="<%=HttpUtility.HtmlEncode(bang.strDuLieu)%>" />
                    <input type="submit" id="btnXacNhanGhi" value="XN" />
                    <input type="hidden" id="idXauCacHangDaXoa" name="idXauCacHangDaXoa" value="" />
                    <%
                        if (bang.ChiDoc == false)
                        {
                    %>
                    </form>
                    <%
                        }
                    %>
                </div>
                <script type="text/javascript">
    $(document).ready(function() {
        Bang_keys.fnSetFocus(0, 0);
        Bang_arrDSTruongTien = '<%=MucLucNganSachModels.strDSTruongTien%>'.split(',');
        Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
        Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';

        BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%> ;
        BangDuLieu_sMauSac_TuChoi = '<%=bang.sMauSac_TuChoi %>';
        BangDuLieu_sMauSac_DongY = '<%=bang.sMauSac_DongY %>';

    });
        function ChonThangNam(value) {
            var iThangLamViec;
            var iNamLamViec;
            iThangLamViec = document.getElementById('iThang').value;
            iNamLamViec = value;

            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("UpdateCauHinhNamLamViec?MaND=#0&iThangLamViec=#1&iNamLamViec=#2","KeToanTongHop") %>');
            url = unescape(url.replace("#0", '<%=MaND %>'));
            url = unescape(url.replace("#1", iThangLamViec));
            url = unescape(url.replace("#2", iNamLamViec));

            $.getJSON(url, function(data) {
                 location.href = '<%=UrlGiaiThich %>';
            });
        }
          function Huy() {
            window.location.href = '<%=URL %>';
        }
            function ChonTaiKhoan(value) {
                var myURL = '<%=UrlGiaiThich %>';
                myURL += "?iID_MaTaiKhoan=" + value;
                window.location.href = myURL;
            }
                </script>
            </div>
        </div>
    </div>
</asp:Content>
