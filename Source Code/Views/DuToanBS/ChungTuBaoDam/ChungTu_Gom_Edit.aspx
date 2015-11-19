<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        string MaND = User.Identity.Name;
        string ParentID = "DTBS_ChungTuBD_Gom_Edit";
        
        //Mã phòng ban
        string iID_MaPhongBan = "";
        DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
        if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
        {
            DataRow drPhongBan = dtPhongBan.Rows[0];
            iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
            dtPhongBan.Dispose();
        }
        string sLNS = Convert.ToString(ViewData["sLNS"]);
        string iID_MaChungTu = Convert.ToString(ViewData["MaChungTu"]);
        string iID_MaChungTu_CT = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu_TLTH", "iID_MaChungTu_TLTH", iID_MaChungTu, "iID_MaChungTu"));
        if (Html.ValidationMessage(ParentID + "_" + "err_ChungTu") !=null && !String.IsNullOrEmpty(Html.ValidationMessage(ParentID + "_" + "err_ChungTu").ToString()))
        {
            iID_MaChungTu_CT = "";
        }
        string[] arrChungTu = iID_MaChungTu_CT.Split(',');
        NameValueCollection data = DuToanBS_ChungTuModels.LayThongTinChungTuTLTH(iID_MaChungTu);
        DataTable dtChungTuDuyet = DuToanBS_ChungTuModels.LayDanhSachChungTuDeSuaTLTH(MaND, sLNS, iID_MaChungTu);
        string backURL = Url.Action("Index", "DuToanBS_ChungTu_BaoDam", new { iLoai = 1 });
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
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <%
        using (Html.BeginForm("ThemSuaChungTuTLTH", "DuToanBS_ChungTu_BaoDam", new { ParentID = ParentID, sLNS1 = sLNS, MaChungTu = iID_MaChungTu }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", 0)%>
    <div class="box_tong">
        <div id="Div1">
            <div id="Div2">
                
                <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                    <tr>
                        <td style="width: 80%">
                            <table cellpadding="0" cellspacing="0" border="0" width="50%" class="table_form2">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Chọn đợt</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                    <div style="overflow: scroll; width: 50%; height: 300px">
                                        <table class="mGrid" style="width: 100%">
                                            <tr>
                                                <th align="center" style="width: 40px;">
                                                    <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
                                                </th>
                                                <% for (int c = 0; c < 1 * 2 - 1; c++)
                                                   {%>
                                                <th>
                                                </th>
                                                <% } %>
                                            </tr>
                                            <%
                                                string strsTen = "";
                                                string MaDonVi = "";
                                                string strChecked = "";
                                                for (int i = 0; i < dtChungTuDuyet.Rows.Count; i = i + 1)
                                                {
                                            %>
                                            <tr>
                                                <% for (int c = 0; c < 1; c++)
                                                   {
                                                       if (i + c < dtChungTuDuyet.Rows.Count)
                                                       {
                                                           strChecked = "";
                                                           strsTen = Convert.ToString(dtChungTuDuyet.Rows[i + c]["sDSLNS"]) + '-' +
                                                                    CommonFunction.LayXauNgay(
                                                                        Convert.ToDateTime(dtChungTuDuyet.Rows[i + c]["dNgayChungTu"])) + '-' +
                                                                        Convert.ToString(dtChungTuDuyet.Rows[i + c]["sID_MaNguoiDungtao"]) + '-' +
                                                                        Convert.ToString(dtChungTuDuyet.Rows[i + c]["sNoiDung"]);
                                                           MaDonVi = Convert.ToString(dtChungTuDuyet.Rows[i + c]["iID_MaChungTu"]);
                                                           if(arrChungTu.Contains(MaDonVi))
                                                           {
                                                                   strChecked = "checked=\"checked\"";
                                                           }
                                                %>
                                                <td align="center" style="width: 10px;">
                                                    <input type="checkbox" value="<%= MaDonVi %>" <%= strChecked %> check-group="ChungTu"
                                                        id="iID_MaChungTu" name="iID_MaChungTu" />
                                                </td>
                                                <td align="left">
                                                    <%= strsTen %>
                                                </td>
                                                <% } %>
                                                <% } %>
                                            </tr>
                                             <% } %>
                                        </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                            &nbsp;
                                    </td>
                                    <td class="td_form2_td5">
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_ChungTu")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Ngày tháng</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div style="width: 200px; float: left;">
                                            <%= MyHtmlHelper.DatePicker(ParentID,CommonFunction.LayXauNgay(Convert.ToDateTime(data["dNgayChungTu"])), "dNgayChungTu", "",
                                                                    "class=\"input1_2\"  style=\"width: 200px;\"") %>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu") %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Nội dung đợt</b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.TextArea(ParentID, data["sNoiDung"], "sNoiDung", "","class=\"input1_2\" style=\"height: 100px;\"") %></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div>
                                        </div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                                    <td width="30%" align="right" class="td_form2_td5">
                                                        <input type="submit" class="button" id="Submit1" value="Lưu" />
                                                    </td>          
                                                    <td width="5px">&nbsp;</td>          
                                                    <td class="td_form2_td5">
                                                        <input class="button" type="button" value="Hủy" onclick="Huy()" />
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
    <% } %>
    <script type="text/javascript">
        function CheckAll(value) {
            $("input:checkbox[check-group='ChungTu']").each(function (i) {
                this.checked = value;
            });
        }

        function Huy() {
            window.location.href = '<%=BackURL %>';
        }
    </script>
</asp:Content>
