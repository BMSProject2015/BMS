<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.QuyetToan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "Edit";
    String UserID = User.Identity.Name;
    String MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);

    DataTable dtLNS = DanhMucModels.NS_LoaiNganSach_Ma();
    SelectOptionList slLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");
    dtLNS.Dispose();
    
    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();

    DataTable dtQuy = DanhMucModels.DT_Quy_QuyetToan();
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    dtQuy.Dispose();

    using (Html.BeginForm("EditSubmit", "QuyetToan_ChungTuKeToan", new { ParentID = ParentID }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaPhongBan", MaPhongBanNguoiDung)%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTuKeToan"), "Nhập chứng từ lẻ")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>
                        <%=NgonNgu.LayXau("Chọn thông tin cần thêm chứng từ")%>
                </span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 100%; float: left;">
                <div style="width: 35%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 100%; text-align: left;">
                                <div><b>Chọn loại ngân sách</b></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td5" style="width: 100%;">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slLNS, "", "sLNS", "", "SIZE=\"30\" onchange=\"ChonLoaiNganSach(this.value)\" class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sLNS")%>
                                    <script type="text/javascript">
                                        function ChonLoaiNganSach(jLNS) {
                                            jQuery.ajaxSetup({ cache: false });
                                            var url = unescape('<%= Url.Action("get_dtMucLucNganSach?ParentID=#0&sLNS=#1", "QuyetToan_ChungTuKeToan") %>');
                                            url = unescape(url.replace("#0", "<%= ParentID %>"));
                                            url = unescape(url.replace("#1", jLNS));
                                            $.getJSON(url, function (data) {
                                                document.getElementById("<%= ParentID %>_divMucLucNganSach").innerHTML = data;
                                            });
                                        }                                            
                                    </script>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 35%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 100%; text-align: left;">
                                <div><b>Chọn mục lục ngân sách&nbsp;</b><%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaMucLucNganSach")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td5" style="width: 100%;">
                                <div id="<%= ParentID %>_divMucLucNganSach">
                                    <%= QuyetToan_ChungTuKeToanController.get_objMucLucNganSach(ParentID, "")%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 30%; float: left;">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 100%; text-align: left;">
                                <div><b>Thông tin khác</b></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td5" style="width: 100%;">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td class="td_form2_td5" style="text-align: left; width: 20%;">
                                            <div><b>Lựa chọn quý:</b></div>
                                        </td>
                                        <td class="td_form2_td5">
                                            <div>
                                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, "", "iQuy", "", "class=\"input1_2\" style=\"width:100%;\"")%><br />
                                                <%= Html.ValidationMessage(ParentID + "_" + "err_iQuy")%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td5" colspan="2">
                                            <div>
                                                <table class="tblhost-filter">
                                                    <tr>
                                                        <td style="width: 10px;">
                                                            <%=MyHtmlHelper.Option(ParentID, "1", "1", "NhapLieu", "")%>
                                                        </td>
                                                        <td style="text-align: left">
                                                            1. Nhập liệu
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%=MyHtmlHelper.Option(ParentID, "2", "0", "NhapLieu", "")%>
                                                        </td>
                                                        <td style="text-align: left">
                                                            2. In bảng kê
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%=MyHtmlHelper.Option(ParentID, "3", "0", "NhapLieu", "")%>
                                                        </td>
                                                        <td style="text-align: left">
                                                            3. Chuyển sang quyết toán(1 mục chọn)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <%=MyHtmlHelper.Option(ParentID, "4", "0", "NhapLieu", "")%>
                                                        </td>
                                                        <td style="text-align: left">
                                                            4. Chuyển tất cả các mục
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td5" colspan="2">
                                            <div>
                                                <fieldset style="width: 100%;">
                                                    <legend><b>In bảng kê cho loại</b></legend>
                                                    <table class="tblhost-filter">
                                                        <tr>
                                                            <td style="width: 10px;">
                                                                <%=MyHtmlHelper.Option(ParentID, "1", "1", "InBangKe", "")%>
                                                            </td>
                                                            <td style="text-align: left">
                                                                Tự chi
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%=MyHtmlHelper.Option(ParentID, "2", "0", "InBangKe", "")%>
                                                            </td>
                                                            <td style="text-align: left">
                                                                Hiện vật
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td5" colspan="2">
                                            <div style="margin-top: 20px;">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                                        <td width="30%" align="right" class="td_form2_td5">
                                                            <input type="submit" class="button" id="Submit1" value="Thực hiện" />
                                                        </td>          
                                                            <td width="5px">&nbsp;</td>          
                                                        <td class="td_form2_td5">
                                                            <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
<%
    }       
%>
</asp:Content>



