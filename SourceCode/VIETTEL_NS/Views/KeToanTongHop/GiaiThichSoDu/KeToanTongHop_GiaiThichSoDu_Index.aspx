<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        String ParentID = "GiaiThichSoDu";
        String MaND = User.Identity.Name;
        String IPSua = Request.UserHostAddress;

        //Cập nhập các thông tin tìm kiếm    
        Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
        String sThang = Request.QueryString["iThang"];
        String sNam = Request.QueryString["iNam"];
        String sLoai = Request.QueryString["iLoai"];
        int iThang = 1, iNam = DateTime.Now.Year, iLoai = 1;
        if (CommonFunction.IsNumeric(sThang)) iThang = Convert.ToInt16(sThang);
        if (CommonFunction.IsNumeric(sNam)) iNam = Convert.ToInt16(sNam);
        if (CommonFunction.IsNumeric(sLoai)) iLoai = Convert.ToInt16(sLoai);
        KeToanTongHop_GiaiThichSoDu_BangDuLieu bang = new KeToanTongHop_GiaiThichSoDu_BangDuLieu(iThang, iNam, iLoai, arrGiaTriTimKiem, MaND, IPSua);
        String BangID = "BangDuLieu";
        int Bang_Height = 470;
        int Bang_FixedRow_Height = 50;
        string sLoaiGT = "";
        if (sLoai == "1") sLoaiGT = "Các khoản thu";
        else if (sLoai == "2") sLoaiGT = "Các khoản tạm ứng";
        else if (sLoai == "3") sLoaiGT = "Các khoản phải trả";
        else sLoaiGT = "Giải thích đề nghị đơn vị";
        String DetailSubmit = Url.Action("DetailSubmit", "KeToanTongHop_GiaiThichSoDu", new { iThang = iThang, iNam = iNam, iLoai = iLoai });
    
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform: uppercase;
                    color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-top: 5px; padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop_GiaiThichSoDu/Edit"), "Giải thích bảng CĐTK")%>
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
                    <td>
                        <span>
                            <%=sLoaiGT%></span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <%--<form action="<%=Url.Action("SearchSubmit","Luong_BangLuongChiTiet",new {ParentID = ParentID, iID_MaBangLuong = iID_MaBangLuong})%>" method="post">
    <table class="mGrid1">
    <tr>
        <%
        for (int j = 0; j <= bang.arrDSMaCot.IndexOf("sTNG"); j++)
        {
            int iColWidth = bang.arrWidth[j] +4;
            if (j == 0) iColWidth = iColWidth + 1;
            String strAttr = String.Format("class='input1_4' style='width:{0}px;height:22px;'", iColWidth - 2);
            if (bang.DuocSuaChiTiet == false) strAttr += " tab-index='-1'";
            %>
            <td style="text-align:left;width:<%=iColWidth%>px;">
                <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = arrGiaTriTimKiem[bang.arrDSMaCot[j]], TenTruong = bang.arrDSMaCot[j], LoaiTextBox = "2", Attributes = strAttr })%>
            </td>
            <%
        }
        %>
        <td style="padding: 1px 5px; text-align: left;">
            <input type="submit" id="<%=ParentID%>_btnTimKiem" <%=bang.DuocSuaChiTiet? "":"tab-index='-1'" %> value="<%=NgonNgu.LayXau("Tìm kiếm")%>" style="font-size: 11px; padding: 0px 3px;"/> 
        </td>
    </tr>
    </table>
</form>--%>
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
                    });
                </script>
            </div>
        </div>
    </div>
</asp:Content>
