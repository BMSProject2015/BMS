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
        String iID_MaTongDuToan_QuyetDinh = Convert.ToString(ViewData["iID_MaTongDuToan_QuyetDinh"]);
        String ParentID = "Edit";
        String sSoQuyetDinh = "", dNgayQuyetDinh = "", sCapPheDuyet = "", dNgayLap = "", sSoQuyetDinh_Cu = "", sNoiDung = ""; ;

        DataTable dt = QLDA_TongDuToanModels.Get_Row_TongDuToan_QuyetDinh1(iID_MaTongDuToan_QuyetDinh);
        if (dt.Rows.Count > 0 && iID_MaTongDuToan_QuyetDinh != null && iID_MaTongDuToan_QuyetDinh != "")
        {
            DataRow R = dt.Rows[0];
            sSoQuyetDinh = HamChung.ConvertToString(R["sSoQuyetDinh"]);
            sSoQuyetDinh_Cu = HamChung.ConvertToString(R["sSoQuyetDinh_Cu"]);
            dNgayQuyetDinh = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayQuyetDinh"]));
            sCapPheDuyet = Convert.ToString(R["sCapPheDuyet"]);
            dNgayLap = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayLap"]));
            sNoiDung = Convert.ToString(R["sNoiDung"]);
        }
        if (dt != null) { dt.Dispose(); };    
        String strReadOnlyMa = "";
        String strIcon = "";
        String strOnblur = "";
        if (ViewData["DuLieuMoi"] == "0")
        {
         //   strReadOnlyMa = "readonly=\"readonly\" style=\"background:#ebebeb;\"";
            strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
        }
        else {
            strOnblur = "onblur=\"CheckMaTrung(this.value);sSoPheDuyet_UpperCase(this);\"";
        }
        using (Html.BeginForm("EditSubmit", "QLDA_TongDuToan", new { ParentID = ParentID, iID_MaTongDuToan_QuyetDinh = iID_MaTongDuToan_QuyetDinh }))
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                    <%=MyHtmlHelper.ActionLink(Url.Action("List", "QLDA_TongDuToan"), "Danh sách quyết định tổng dự toán")%>
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
                	 <span>Thêm quyết đinh tổng dự toán</span>
                    <% 
                   }
                   else
                   { %>
                    <span>Sửa quyết đinh tổng dự toán</span>
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
                                <b>Số quyết định</b>&nbsp;<span  style="color:Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sSoQuyetDinh, "sSoQuyetDinh", "", "" + strOnblur + " class=\"input1_2\" tab-index='-1' style=\"width:30%;\" " + strReadOnlyMa + "")%>&nbsp;<span id="pMess" style="color: Red;"></span><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sSoQuyetDinh")%>
                                <script type="text/javascript">
                                    function CheckMaTrung(iID_MaChuDauTu) {
                                        jQuery.ajaxSetup({ cache: false });
                                        if (iID_MaChuDauTu != null && iID_MaChuDauTu != '') {
                                            var url = unescape('<%= Url.Action("get_SoQuyetDinh?sSoQuyetDinh=#0", "QLDA_TongDuToan") %>');
                                            url = unescape(url.replace("#0", iID_MaChuDauTu));
                                            $.getJSON(url, function (data) {
                                                document.getElementById("pMess").innerHTML = data;
                                            });
                                        }
                                        else {
                                            document.getElementById("pMess").innerHTML = '';
                                        }
                                    }
                                </script>
                                <script type="text/javascript">
                                    function sSoPheDuyet_UpperCase(ctl) {
                                        ctl.value = ctl.value.toUpperCase();
                                    }
                                </script>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Ngày quyết định</b>&nbsp;<span  style="color:Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, dNgayQuyetDinh, "dNgayQuyetDinh", "", "onchange=\"dNgayPheDuyet_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayQuyetDinh")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Ngày lập</b>&nbsp;<span  style="color:Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, dNgayLap, "dNgayLap", "", "onchange=\"dNgayLap_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayLap")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Cấp phê duyệt</b>&nbsp;<span  style="color:Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sCapPheDuyet, "sCapPheDuyet", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sCapPheDuyet")%>
                            </div>
                        </td>
                    </tr>
                     <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Nội dung</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sNoiDung")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Lấy dữ liệu của số quyết định trước</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.Autocomplete(ParentID, sSoQuyetDinh_Cu, sSoQuyetDinh_Cu, "sSoQuyetDinh_Cu1", "sSoQuyetDinh_Cu", "", "class=\"input1_2\"")%>
                                <%=MyHtmlHelper.AutoComplete_Initialize(ParentID + "_sSoQuyetDinh_Cu", ParentID + "_sSoQuyetDinh_Cu1", Url.Action("get_DanhSach", "QLDA_TongDuToan"), "sTerm: request.term", "func_Auto_Complete", new { delay = 100, minchars = 1 })%>
                                <script type="text/javascript">
                                    function func_Auto_Complete(id, ui) {

                                    }
                                </script>
                            </div>
                        </td>
                    </tr>
                    <tr><td colspan="2" style="height: 20px;">&nbsp;</td></tr>
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

