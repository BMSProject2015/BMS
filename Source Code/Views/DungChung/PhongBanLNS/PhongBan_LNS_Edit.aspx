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
        String sMaPhongBan = Convert.ToString(ViewData["MaPhongBan"]);
        int iID_MaPhongBanLoaiNganSach = Convert.ToInt32(ViewData["iID_MaPhongBanLoaiNganSach"]);
        // int iID_MaPhongBanDonVi = Convert.ToInt32(ViewData["iID_MaPhongBanDonVi"]);
        String ParentID = "Edit";
        String MaPhongBan = Convert.ToString(ViewData["MaPhongBan"]);
        String sLNS = Convert.ToString(ViewData["sLNS"]);
        String LoaiNS = Convert.ToString(ViewData["LNS"]);
        sLNS = PhongBan_LNSModels.getPhongBanLNS(MaPhongBan);

        DataTable dtPhongBan = DanhMucModels.getPhongBanByCombobox(true, "--- Chọn phòng ban ---");
        DataTable dtLNS = DanhMucModels.NS_LoaiNganSach();

        SelectOptionList optLNS = new SelectOptionList(dtLNS, "sLNS", "TenHT");

        SelectOptionList optPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTen");

        //String MaPhongBan = "", sLNS = "", bPublic = "";



        String path = "~/Views/DungChung/PhongBanLNS/PhongBanLNS_DanhSach.ascx";



        String strAttr = String.Format(" onchange='js_Search_onkeypress(this.value)'; class=\"input1_2\"");
        String Code = Request.QueryString["Code"];
        String strQuayLai = Url.Action("EditDetail", "PhongBanLNS", new { Code = Code });
        using (Html.BeginForm("EditSubmit", "PhongBanLNS", new { ParentID = ParentID, iID_MaPhongBanLoaiNganSach = iID_MaPhongBanLoaiNganSach, sLNS = sLNS }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaPhongBanLoaiNganSach", iID_MaPhongBanLoaiNganSach)%>
        <%= Html.Hidden(ParentID + "_Code", Code)%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 12%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |<%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhongBanLNS"), "Phòng ban - Loại ngân sách")%>
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
                        <% if (ViewData["DuLieuMoi"] == "1")
                           {
                        %>
                        <span>Nhập thông tin Phòng ban - Loại ngân sách</span>
                        <% 
                           }
                           else
                           { %>
                        <span>Sửa thông tin Phòng ban - Loại ngân sách</span>
                        <% } %>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1">
                            <div style="font-weight: bold;">
                                Phòng ban &nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td>
                            <div id="divDonVi">
                                <%=MyHtmlHelper.DropDownList(ParentID, optPhongBan, MaPhongBan, "iID_MaPhongBan", null, "style=\"width: 50%;\"onchange=\"Chon_LNS(this.value)\"")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_MaPhongBan")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1" style="width: 10%;">
                            <div style="font-weight: bold;">
                                Loại Ngân Sách &nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <%--<td class="td_form2_td5">
                        <div><%=MyHtmlHelper.DropDownList(ParentID, optLNS, LNS, "sLNS", null, "style=\"width: 50%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_MaDonVi")%>
                        </div>
                    </td>--%>
                        <td valign="top" align="left" style="width: 90%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td style="width: 100%;">
                                        <div style="margin-top: 5px;">
                                        </div>
                                        <div style="margin-top: 5px;">
                                            <table class="mGrid">
                                                <tr>
                                                    <th align="center" style="width: 40px;">
                                                        <input type="checkbox" id="abc" onclick="CheckAll(this.checked)" />
                                                    </th>
                                                    <th>
                                                        Loại ngân sách
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>
                                                    </th>
                                                    <th>
                                                        <input id="txtLNS" class="input1_2" onkeypress='js_Search_PhongBan_LNS_onkeypress(event)' />
                                                        <%--    <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = sLNS, TenTruong = "sLNS", LoaiTextBox = "2", Attribute = strAttr })%>--%>
                                                    </th>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="divDonVi" style="width: 100%; max-height: 500px; min-height: 0px; overflow: scroll;">
                                            <%Html.RenderPartial(path, new { ControlID = ParentID, dtDonVi = dtLNS, sLNS = sLNS, LNS = LoaiNS }); %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_MaDonVi")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                            <table border="0" cellpadding="0" cellspacing="0">
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
            </div>
        </div>
    </div>
    <%
        }
    %>
    <script type="text/javascript">
        function ChonLNS(NS) {
            $("input:checkbox[check-group='sLNS']").each(function (i) {
                this.checked = NS;
            });
        }

    </script>
    <script type="text/javascript">

        function Chon_LNS(sLNS) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("EditNew?Code=#1", "PhongBanLNS")%>');
            url = unescape(url.replace("#1", sLNS));
            location.href = url;

        }
          
    </script>
</asp:Content>
