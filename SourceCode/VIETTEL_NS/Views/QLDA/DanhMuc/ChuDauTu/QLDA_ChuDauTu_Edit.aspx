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
        String iID_MaChuDauTu = Convert.ToString(ViewData["iID_MaChuDauTu"]);
        String sMa = Convert.ToString(ViewData["sMa"]);
        String ParentID = "Edit";
        String sTen = "", sTenVietTat = "", sSoTaiKhoan = "", sTenNganHangGiaoDich = "", iID_MaDonVi="";
        string sFlagEdit = ViewData["DuLieuMoi"] == null ? "1" : ViewData["DuLieuMoi"].ToString();
        DataTable dtDV = DanhMucModels.getDonViByCombobox(true,"--- Chọn đơn vị ---");
        SelectOptionList slDonVi = new SelectOptionList(dtDV, "iID_MaDonVi", "sTen");
        dtDV.Dispose();
        DataTable dt = QLDA_ChuDauTuModels.Get_Row_ChuDauTu(sMa);
        if (dt.Rows.Count > 0 && iID_MaChuDauTu != null && iID_MaChuDauTu != "")
        {
            DataRow R = dt.Rows[0];
            sTen = HamChung.ConvertToString(R["sTen"]);
            sTenVietTat = HamChung.ConvertToString(R["sTenVietTat"]);
            sSoTaiKhoan = Convert.ToString(R["sSoTaiKhoan"]);
            sTenNganHangGiaoDich = Convert.ToString(R["sTenNganHangGiaoDich"]);
            iID_MaDonVi = Convert.ToString(R["iID_MaDonVi"]);
        }
        if (dt != null) { dt.Dispose(); };
        String strReadOnlyMa = "";
        String strIcon = "";
        String strOnblur = "";
        String strOnblurName = "";
        String strOnblurShortName = "";
        String strOnblurAccountNumber = "";
        if (ViewData["DuLieuMoi"] == "0")
        {
            //strReadOnlyMa = "readonly=\"readonly\" style=\"background:#ebebeb;\"";
            strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
        }
        else
        {
            strOnblur = "onblur=\"CheckMaTrung(this.value);\"";
        }        
        strOnblurName = "onblur=\"CheckExistName(this.value);\"";
        strOnblurShortName = "onblur=\"CheckExistShortName(this.value);\"";
        strOnblurAccountNumber = "onblur=\"CheckExistAccountNumber(this.value);\"";
        using (Html.BeginForm("EditSubmit", "QLDA_ChuDauTu", new { ParentID = ParentID, iID_MaChuDauTu = iID_MaChuDauTu }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaChuDauTu", iID_MaChuDauTu)%>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_ChuDauTu"), "Chủ đầu tư")%>
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
                        <span>Thêm thông tin chủ đầu tư</span>
                        <% 
                            }
                           else
                           { %>
                        <span>Sửa thông tin chủ đầu tư</span>
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
                                <b>Mã chủ đầu tư</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sMa, "sMa", "", "" + strOnblur + " class=\"input1_2\" tab-index='-1' style=\"width:20%;\" " + strReadOnlyMa + "")%>&nbsp;<span
                                    id="pMess" style="color: Red;"></span><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaChuDauTu")%>
                                <script type="text/javascript">
                                    function CheckMaTrung(iID_MaChuDauTu) {
                                        if (Trim(iID_MaChuDauTu) != '') {
                                            jQuery.ajaxSetup({ cache: false });
                                            var url = unescape('<%= Url.Action("get_ChuDauTuExist?sfieldName=#0&sSearchValue=#1&sEdit=#2&sMa=#3&sID=#4", "QLDA_ChuDauTu") %>');
                                            url = unescape(url.replace("#0", "sMa"));
                                            url = unescape(url.replace("#1", iID_MaChuDauTu));
                                            var sflag = "<%= sFlagEdit%>";
                                            var sMa = "<%= sMa%>";
                                            var sID = "<%= iID_MaChuDauTu%>";
                                            url = unescape(url.replace("#2", sflag));
                                            url = unescape(url.replace("#3", sMa));
                                            url = unescape(url.replace("#4", sID));
                                            $.getJSON(url, function (data) {
                                                var strMess = "";
                                                if (data == '1') {
                                                    strMess = "Mã chủ đầu tư đã tồn tại!";
                                                }
                                                else {
                                                    strMess = "";
                                                }
                                                document.getElementById("pMess").innerHTML = strMess;
                                            });
                                        }
                                    }
                                </script>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên chủ đầu tư</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "" + strOnblurName + "class=\"input1_2\"", 2)%>&nbsp;<span
                                    id="sNameMess" style="color: Red;"></span><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                                <script type="text/javascript">
                                    function CheckExistName(sTen) {
                                        document.getElementById("sNameMess").innerHTML = "";
                                        if (Trim(sTen) != '') {
                                            jQuery.ajaxSetup({ cache: false });
                                            var url = unescape('<%= Url.Action("get_ChuDauTuExist?sfieldName=#0&sSearchValue=#1&sEdit=#2&sMaChuDauTu=#3&sID=#4", "QLDA_ChuDauTu") %>');
                                            url = unescape(url.replace("#0", "sTen"));
                                            url = unescape(url.replace("#1", sTen));
                                            var sflag = "<%= sFlagEdit%>";
                                            var sMa = "<%= sMa%>";
                                            var sID = "<%= iID_MaChuDauTu%>";
                                            url = unescape(url.replace("#2", sflag));
                                            url = unescape(url.replace("#3", sMa));
                                            url = unescape(url.replace("#4", sID));
                                            $.getJSON(url, function (data) {
                                                var strMess = "";
                                                if (data == '1') {
                                                    strMess = "Tên chủ đầu tư đã tồn tại!";
                                                }
                                                else {
                                                    strMess = "";
                                                }
                                                document.getElementById("sNameMess").innerHTML = strMess;
                                            });
                                        }
                                    }
                                </script>
                            </div>
                        </td>
                    </tr>
                       <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Đơn vị</b> &nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "class=\"input1_2\"")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
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
                                <%=MyHtmlHelper.TextBox(ParentID, sTenVietTat, "sTenVietTat", "", "" + strOnblurShortName + "class=\"input1_2\"")%>
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
                                <%=MyHtmlHelper.TextBox(ParentID, sSoTaiKhoan, "sSoTaiKhoan", "", "" + strOnblurAccountNumber + "class=\"input1_2\"")%>&nbsp;
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
