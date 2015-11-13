<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/jsBang_KeToanTongHop_GiaiThichSoDu.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
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
        String ParentID = "RutDuToan";
        String MaND = User.Identity.Name;
        String IPSua = Request.UserHostAddress;

        //Cập nhập các thông tin tìm kiếm    
        Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
        String iID_MaDotRutDuToan = Request.QueryString["iID_MaDotRutDuToan"];


        KeToan_RutDuToan_BangDuLieu bang = new KeToan_RutDuToan_BangDuLieu(iID_MaDotRutDuToan, arrGiaTriTimKiem, MaND, IPSua);
        String BangID = "BangDuLieu";
        int Bang_Height = 470;
        int Bang_FixedRow_Height = 50;
        String DetailSubmit = Url.Action("DetailSubmit", "KeToan_RutDuToan", new { ParentID = ParentID, iID_MaDotRutDuToan = iID_MaDotRutDuToan });
        NameValueCollection data = KeToan_RutDuToanDotModels.LayThongTin(iID_MaDotRutDuToan);
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToan_RutDuToan"), "Danh sách đợt chỉ tiêu")%>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; float: left;">
        <div style="width: 100%; float: left;">
            <div class="box_tong">
                <div class="title_tong">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <span>Thông tin chỉ tiêu</span>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="nhapform">
                    <div id="form2">
                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="width: 50%;" valign="top">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form3">
                                        <tr>
                                            <td class="td_form2_td1" style="width: 15%">
                                                <div>
                                                    <b>Đợt ngày</b></div>
                                            </td>
                                            <td class="td_form2_td5">
                                                <div>
                                                    <b>
                                                        <%=String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(data["dNgayDotRutDuToan"]))%></b></div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <%Html.RenderPartial("~/Views/Shared/BangDuLieu/BangDuLieu.ascx", new { BangID = BangID, bang = bang, Bang_Height = Bang_Height, Bang_FixedRow_Height = Bang_FixedRow_Height }); %>
                        <div style="display: none;">
                            <input type="hidden" id="idXauDoRongCot" value="<%=HttpUtility.HtmlEncode(bang.strDSDoRongCot)%>" />
                            <input type="hidden" id="idXauKieuDuLieu" value="<%=HttpUtility.HtmlEncode(bang.strType)%>" />
                            <input type="hidden" id="idXauChiSoCha" value="<%=HttpUtility.HtmlEncode(bang.strCSCha)%>" />
                            <input type="hidden" id="idBangChiDoc" value="<%=HttpUtility.HtmlEncode(bang.strChiDoc)%>" />
                            <input type="hidden" id="idXauEdit" value="<%=HttpUtility.HtmlEncode(bang.strEdit)%>" />
                            <input type="hidden" id="idXauDSCotDuocPhepNhap" value="<%=HttpUtility.HtmlEncode(bang.strDSCotDuocPhepNhap)%>" />
                            <input type="hidden" id="idViewport_N" value="<%=HttpUtility.HtmlEncode(bang.Viewport_N)%>" />
                            <input type="hidden" id="idNC_Fixed" value="<%=HttpUtility.HtmlEncode(bang.nC_Fixed)%>" />
                            <input type="hidden" id="idNC_Slide" value="<%=HttpUtility.HtmlEncode(bang.nC_Slide)%>" />
                            <%  
                                if (bang.ChiDoc == false)
                                {
                            %>
                            <form action="<%=DetailSubmit%>" method="post">
                            <%
                                } %>
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
                        <%
                            if (bang.ChiDoc == false)
                            {
                        %>
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" align="right" class="table_form2">
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <input type="button" id="btnLuu" class="button" onclick="javascript:return Bang_HamTruocKhiKetThuc();"
                                        value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td align="left" width="100px">
                                    <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" />
                                </td>
                                <td>
                                    &nbsp;
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
                        <script type="text/javascript">
                            $(document).ready(function () {
                                Bang_keys.fnSetFocus(0, 0);
                                Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
                                Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';
                                BangDuLieu_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "KTCT_KhoBac_ChungTuChiTiet")%>';
                                BangDuLieu_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "KTCT_KhoBac_ChungTuChiTiet")%>';

                            });
                        </script>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
