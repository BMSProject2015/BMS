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
        String iID_MaQuyetToan_SoPhieu = Convert.ToString(ViewData["iID_MaQuyetToan_SoPhieu"]);
        String ParentID = "Edit";
        String iSoQuyetToan = "", dNgayQuyetToan = "", sNoiDung = "", sDeAn = "";
        DataTable dtDeAn = QLDA_DuToan_NamModels.LayDanhSachDeAn(User.Identity.Name);
        DataTable dt = QLDA_QuyetToanModels.Get_Row_QuyetToan_SoPhieu(iID_MaQuyetToan_SoPhieu);
        if (dt.Rows.Count > 0 && iID_MaQuyetToan_SoPhieu != null )
        {
            DataRow R = dt.Rows[0];
            iSoQuyetToan = HamChung.ConvertToString(R["iSoQuyetToan"]);
            dNgayQuyetToan = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayQuyetToan"]));
            sNoiDung = Convert.ToString(R["sNoiDung"]);
            sDeAn = Convert.ToString(R["sDeAn"]);
        }
        else
        {
            
        }
        String[] arrDeAn = sDeAn.Split(',');
        if (dt != null) { dt.Dispose(); };
        String strReadOnlyMa = "readonly=\"readonly\" style=\"background:#ebebeb;\"";
        String strIcon = "";
        String strOnblur = "";
        if (ViewData["DuLieuMoi"] == "0")
        {
          //  strReadOnlyMa = "readonly=\"readonly\" style=\"background:#ebebeb;\"";
            strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
        }
        else
        {
            strOnblur = "onblur=\"CheckMaTrung(this.value);sSoPheDuyet_UpperCase(this);\"";
        }
        using (Html.BeginForm("AddNewSubmit", "QLDA_QuyetToan", new { ParentID = ParentID, iID_MaQuyetToan_SoPhieu = iID_MaQuyetToan_SoPhieu }))
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("List", "QLDA_TongDauTu"), "Danh sách quyết định tổng đầu tư")%>
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
                        <span>Thêm quyết đinh tổng đầu tư</span>
                        <% 
                            }
                           else
                           { %>
                        <span>Sửa quyết đinh tổng đầu tư</span>
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
                                <b>Số quyết định</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iSoQuyetToan, "iSoQuyetToan", "", "" + strOnblur + " class=\"input1_2\" tab-index='-1' style=\"width:30%;\" " + strReadOnlyMa + "")%>&nbsp;<span
                                    id="pMess" style="color: Red;"></span><br />
                               
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Ngày quyết toán</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DatePicker(ParentID, dNgayQuyetToan, "dNgayQuyetToan", "", "onchange=\"dNgayPheDuyet_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayQuyetToan")%>
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
                         <td class="td_form2_td1"><div>
                                <b>
                                    <%=NgonNgu.LayXau("Đề án")%></b></div></td>
                         <td class="td_form2_td5">
                               <div style="width: 50%; height: 200px; overflow: scroll; border: 1px solid black;">
                                <table class="mGrid">
                                     <tr>
                               <td><input type="checkbox" id="checkAll" onclick="Chonall(this.checked)"></td>
                                <td> Chọn tất cả Đề án </td>
                                </tr>
                                    <%
                                    String TenDeAn = ""; String DeAn = "";
                                    String _Checked = "checked=\"checked\"";  
                                    for (int i = 0; i < dtDeAn.Rows.Count; i++)
                                    {
                                        _Checked = "";
                                        TenDeAn = Convert.ToString(dtDeAn.Rows[i]["TenHT"]);
                                        DeAn = Convert.ToString(dtDeAn.Rows[i]["sDeAn"]);
                                        for (int j = 0; j < arrDeAn.Length; j++)
                                        {
                                            if (DeAn == arrDeAn[j])
                                            {
                                                _Checked = "checked=\"checked\"";
                                                break;
                                            }
                                        }    
                                    %>
                                    <tr>
                                        <td style="width: 15%;">
                                            <input type="checkbox" value="<%=DeAn %>" <%=_Checked %> check-group="sDeAn" id="sDeAn" 
                                                name="sDeAn" />
                                        </td>
                                        <td>
                                            <%=TenDeAn%>
                                        </td>
                                    </tr>
                                  <%}%>
                                </table>
                            </div>
                         </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 20px;">
                            &nbsp;
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
} dtDeAn.Dispose();
        dt.Dispose();
    %>
    <script type="text/javascript">
        function Chonall(value) {
            $("input:checkbox[check-group='sDeAn']").each(function (i) {
                this.checked = value;
            });
        }                                            
     </script>
</asp:Content>
