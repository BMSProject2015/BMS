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
    <script src="<%= Url.Content("~/Scripts/KeToanTongHop/jsBang_TaiKhoanDanhMucChiTiet.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
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
        String ParentID = "TaiKhoanDanhMucChiTiet";
        String MaND = User.Identity.Name;
        String IPSua = Request.UserHostAddress;
        String iID_MaTaiKhoan = Request.QueryString["iID_MaTaiKhoan"];
        //đổ dữ liệu vào Combobox tài khoản
        var tbl = TaiKhoanModels.DT_DSTaiKhoanCha(true, "--Chọn tất cả--", User.Identity.Name, false);
        SelectOptionList slTaiKhoan = new SelectOptionList(tbl, "iID_MaTaiKhoan", "sTen");
        if (tbl != null) tbl.Dispose();
        //Cập nhập các thông tin tìm kiếm
        String DSTruong = BangLuongModels.strDSTruong;
        String[] arrDSTruong = DSTruong.Split(',');
        Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
        for (int i = 0; i < arrDSTruong.Length; i++)
        {
            arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
        }

        TaiKhoanDanhMucChiTiet_BangDuLieu bang = new TaiKhoanDanhMucChiTiet_BangDuLieu(iID_MaTaiKhoan, arrGiaTriTimKiem, MaND, IPSua);

        String BangID = "BangDuLieu";
        int Bang_Height = 470;
        int Bang_FixedRow_Height = 50;
        String UrlGiaiThich = Url.Action("List", "TaiKhoanDanhMucChiTiet");
        String strIn = Url.Action("Index", "rptKeToanTongHop_InTaiKhoanChiTiet", new { iID_MaTaiKhoan = iID_MaTaiKhoan });
        String strHinh = Url.Action("Index", "TaiKhoanDanhMucChiTiet", new { iID_MaTaiKhoan = iID_MaTaiKhoan });
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
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "LoaiTaiKhoan"), "Danh mục Tài khoản kế toán")%>
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
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width: 250px;">
                        <span>Thông tin chi tiết tài khoản</span>
                    </td>
                    <td style="width: 400px;">
                        <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, iID_MaTaiKhoan, "iID_MaTaiKhoan", "", "onchange=\"ChonTaiKhoan(this.value)\" class=\"input1_2\" tab-index='-1' style=\"width:400px;\"")%>
                    </td>
                    <td align="right" style="padding-right: 50px;">
                        <input id="Button1" type="button" class="button_title" value="In danh sách" onclick="javascript:location.href='<%=strIn %>'" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <input id="Button2" type="button" class="button_title" value="Xem dạng cây" onclick="javascript:location.href='<%=strHinh %>'" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="box_tong">
        <div id="nhapform">
            <div id="form2">
              
                <%Html.RenderPartial("~/Views/Shared/BangDuLieu/BangDuLieu.ascx", new { BangID = BangID, bang = bang, Bang_Height = Bang_Height, Bang_FixedRow_Height = Bang_FixedRow_Height }); %>
                <div style="display: none;">
                    <input type="hidden" id="idXauDoRongCot" value="<%=HttpUtility.HtmlEncode(bang.strDSDoRongCot)%>" />
                    <input type="hidden" id="idXauKieuDuLieu" value="<%=HttpUtility.HtmlEncode(bang.strType)%>" />
                    <input type="hidden" id="idXauChiSoCha" value="<%=HttpUtility.HtmlEncode(bang.strCSCha)%>">
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
                    <form action="<%=Url.Action("DetailSubmit", "TaiKhoanDanhMucChiTiet", new { iID_MaTaiKhoan = iID_MaTaiKhoan })%>" method="post">
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
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <input type="button" id="btnLuu" class="button_title" onclick="javascript:return Bang_HamTruocKhiKetThuc();"
                                value="<%=NgonNgu.LayXau("Thực hiện")%>"  style="width: 100px;"/>
                        </td>
                     <%--   <td>
                            &nbsp;
                        </td>--%>
                        <td align="left" width="120px">
                         &nbsp;&nbsp;&nbsp;&nbsp;   <input class="button_title" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" style="width: 100px;"/>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <%
                    }
                %>
                <script type="text/javascript">
                    $(document).ready(function () {
                        Bang_Url_Check_KyHieu = '<%=Url.Action("KiemTraTrungKyHieu","TaiKhoanDanhMucChiTiet") %>';
                        Bang_keys.fnSetFocus(null, null);
                        Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
                        Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';

                    });
                    function ChonTaiKhoan(value) {
                        var url = '<%=UrlGiaiThich %>';
                        url = url + "?iID_MaTaiKhoan=" + value;
                        location.href = url;
                    }
                </script>
            </div>
        </div>
    </div>
</asp:Content>
