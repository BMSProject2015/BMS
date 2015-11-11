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
        int i;
        String MaND = User.Identity.Name;
        String ParentID = "DuToan_ChungTu";

        String page = Request.QueryString["page"];
        String iID_MaPhongBan = "";
        DataTable dtPhongBan = NganSach_HamChungModels.DSBQLCuaNguoiDung(MaND);
        if (dtPhongBan != null && dtPhongBan.Rows.Count > 0)
        {
            DataRow drPhongBan = dtPhongBan.Rows[0];
            iID_MaPhongBan = Convert.ToString(drPhongBan["sKyHieu"]);
            dtPhongBan.Dispose();
        }
      
        String dNgayChungTu = "";
        String sLNS = "";
        String iID_MaChungTu = "";
        String sNoiDung;
        iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
        sLNS = Convert.ToString(ViewData["sLNS"]);
        String iID_MaChungTu_CT = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu_TLTH", "iID_MaChungTu_TLTH", iID_MaChungTu, "iID_MaChungTu"));
        String[] arrChungTu = iID_MaChungTu_CT.Split(',');
        NameValueCollection data = DuToan_ChungTuModels.LayThongTin_Gom(iID_MaChungTu);
        DataTable dtChungTuDuyet = DuToan_ChungTuModels.getDanhSachChungTu_TongHopDuyet_Sua(MaND, sLNS, iID_MaChungTu);
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
    <div class="box_tong">
        <div id="Div1">
            <div id="Div2">
                <%
                    using (Html.BeginForm("EditSubmit_Gom", "DuToan_ChungTu", new { ParentID = ParentID, sLNS1 = sLNS,MaChungTu=iID_MaChungTu }))
                    {
                %>
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

                                                String strsTen = "", MaDonVi = "", strChecked = "";
                                                for (int z = 0; z < dtChungTuDuyet.Rows.Count; z = z + 1)
                                                {


                                            %>
                                            <tr>
                                                <% for (int c = 0; c < 1; c++)
                                                   {
                                                       if (z + c < dtChungTuDuyet.Rows.Count)
                                                       {
                                                           strChecked = "";
                                                           strsTen = Convert.ToString(dtChungTuDuyet.Rows[z + c]["sDSLNS"]) + '-' +
                                                                     CommonFunction.LayXauNgay(
                                                                         Convert.ToDateTime(dtChungTuDuyet.Rows[z + c]["dNgayChungTu"])) + '-' +
                                                                     Convert.ToString(dtChungTuDuyet.Rows[z + c]["sID_MaNguoiDungtao"]) + '-' +
                                                                      Convert.ToString(dtChungTuDuyet.Rows[z + c]["sNoiDung"]);
                                                           MaDonVi = Convert.ToString(dtChungTuDuyet.Rows[z + c]["iID_MaChungTu"]);
                                                           for (int j = 0; j < arrChungTu.Length; j++)
                                                           {
                                                               if (MaDonVi.Equals(arrChungTu[j]))
                                                               {
                                                                   strChecked = "checked=\"checked\"";
                                                                   break;
                                                               }
                                                           } %>
                                                <td align="center" style="width: 40px;">
                                                    <input type="checkbox" value="<%= MaDonVi %>" <%= strChecked %> check-group="DonVi"
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
                                            <%= MyHtmlHelper.TextArea(ParentID, data["sNoiDung"], "sNoiDung", "",
                                                                  "class=\"input1_2\" style=\"height: 100px;\"") %></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1" style="width: 15%;">
                                        <div>
                                        </div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <input type="submit" class="button" id="Submit1" value="<%= NgonNgu.LayXau("Lưu") %>" />
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
            $("input:checkbox[check-group='DonVi']").each(function (i) {
                this.checked = value;
            });
        }                                            
    </script>
</asp:Content>
