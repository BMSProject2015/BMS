<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String iID_MaDonViThiCong = Convert.ToString(ViewData["iID_MaDonViThiCong"]);
        String ParentID = "Edit";
        String sTen = "", sTenVietTat = "", sSoTaiKhoan = "", sTenNganHangGiaoDich = "",sMaSoThue="";

        DataTable dt = QLDA_DonViThiCongModels.Get_Row_Data(iID_MaDonViThiCong);
        if (dt.Rows.Count > 0 && iID_MaDonViThiCong != null && iID_MaDonViThiCong != "")
        {
            DataRow R = dt.Rows[0];
            sTen = HamChung.ConvertToString(R["sTen"]);
            sTenVietTat = HamChung.ConvertToString(R["sTenVietTat"]);
            sSoTaiKhoan = Convert.ToString(R["sSoTaiKhoan"]);
            sTenNganHangGiaoDich = Convert.ToString(R["sTenNganHangGiaoDich"]);
            sMaSoThue = Convert.ToString(R["sMaSoThue"]);
        }
        if (dt != null) { dt.Dispose(); };    
        String strReadOnlyMa = "";
        String strCheckMa = "";
        String strIcon = "";
        if (ViewData["DuLieuMoi"] == "0")
        {
            //strReadOnlyMa = "readonly=\"readonly\" style=\"background:#ebebeb;\"";
            strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
        }
        else {
            strCheckMa = "onblur=\"CheckMaTrung(this.value);\"";
        }
        using (Html.BeginForm("EditSubmit", "QLDA_DonViThiCong", new { ParentID = ParentID, iID_MaDonViThiCong = iID_MaDonViThiCong }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_DonViThiCong"), "nhà thầu")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                     <% if (ViewData["DuLieuMoi"] == "1")
                   {
                       %>
                	 <span>Thêm thông tin nhà thầu</span>
                    <% 
                   }
                   else
                   { %>
                    <span>Sửa thông tin nhà thầu</span>
                    <% } %>
                       
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="50%">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Mã nhà thầu</b>&nbsp;<span  style="color:Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iID_MaDonViThiCong, "iID_MaDonViThiCong", "", "" + strCheckMa  + " class=\"input1_2\" tab-index='-1' style=\"width:20%;\" " + strReadOnlyMa + "")%>&nbsp;<span id="pMess" style="color: Red;"></span><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonViThiCong")%>
                                <script type="text/javascript">
                                    function CheckMaTrung(iID_MaDonViThiCong) {
                                        jQuery.ajaxSetup({ cache: false });
                                        var url = unescape('<%= Url.Action("get_MaChuDauTu?iID_MaDonViThiCong=#0", "QLDA_DonViThiCong") %>');
                                        url = unescape(url.replace("#0", iID_MaDonViThiCong));
                                        $.getJSON(url, function (data) {
                                            document.getElementById("pMess").innerHTML = data;
                                        });
                                    }
                                </script>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên nhà thầu</b>&nbsp;<span  style="color:Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\"", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên viết tắt</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTenVietTat, "sTenVietTat", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Số tài khoản</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sSoTaiKhoan, "sSoTaiKhoan", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Ngân hàng giao dịch</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTenNganHangGiaoDich, "sTenNganHangGiaoDich", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                    </tr>
                     <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Mã số thuế</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sMaSoThue, "sMaSoThue", "", "class=\"input1_2\"")%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="70%">
                &nbsp;
            </td>
            <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
        } 
    %>
</asp:Content>


