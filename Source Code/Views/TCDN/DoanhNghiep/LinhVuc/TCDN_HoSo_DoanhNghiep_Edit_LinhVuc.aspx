<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Report_Controllers.ThuNop" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/ThuNop/jsBang_ThuNop.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <style type="text/css">
        #menu1 ul
        {
            background: url("images/ui-bg_gloss-wave_55_5c9ccc_500x100.png") repeat-x scroll 50% 50% #5c9ccc;
            border: 1px solid #4297d7;
            color: #fff;
            border-radius: 5px;
            font-weight: bold;
            margin: 0;
            padding: 0em 0.2em 0;
        }
        
        #menu1 ul li
        {
            float: left;
            padding: 4px 2em;
            text-decoration: none;
            cursor: pointer;
            display: inline;
        }
        
        #menu1 ul a
        {
        }
    </style>
    <%
        String ParentID = "DoanhNghiep";
        String iID_MaDoanhNghiep = Convert.ToString(Request.QueryString["iID_MaDoanhNghiep"]);
        String sTen = Convert.ToString(Request.QueryString["sTen"]);
        String iLoai = Convert.ToString(Request.QueryString["iLoai"]);
        if (String.IsNullOrEmpty(iLoai)) iLoai = "1";

        String DK = "";
        if (!String.IsNullOrEmpty(sTen)) DK += String.Format(@" AND sTen LIKE '% {0}%'",sTen);
        String SQL =
                  String.Format(
                      "SELECT * FROM TCDN_LinhVuc WHERE iTrangThai=1 AND iID_MaDoanhNghiep=@iID_MaDoanhNghiep {0}",DK);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = SQL;
        cmd.Parameters.AddWithValue("@iID_MaDoanhNghiep", iID_MaDoanhNghiep);
        DataTable dt = Connection.GetDataTable(cmd);
        cmd.Dispose();
    %>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <%=NgonNgu.LayXau("Liên kết nhanh: ")%>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_HoSo_DoanhNghiep"), "Danh sách hồ sơ Doanh nghiệp")%>
                </div>
            </td>
        </tr>
    </table>
    <div id="menu1">
        <%=TCDN_ChungTuChiTietModels.GetMenuHoSoDN(iID_MaDoanhNghiep, iLoai)%>
    </div>
    <div style="clear: both">
    </div>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Thông tin tìm kiếm</span>
                    </td>
                </tr>
            </table>
        </div>
        <% using (Html.BeginForm("Search_LinhVuc", "TCDN_HoSo_DoanhNghiep", new { ParentID = ParentID, iID_MaDoanhNghiep = iID_MaDoanhNghiep }))
           {%>
        <div id="nhapform">
            <div id="form2">
                <div>
                    <table width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 10%">
                                <div>
                                    <b>
                                        <%= NgonNgu.LayXau("Tên lĩnh vực") %></b></div>
                            </td>
                            <td class="td_form2_td5" style="width: 25%">
                                <div>
                                    <%= MyHtmlHelper.TextBox(ParentID, sTen, "sTenCongTy_search", "", "class=\"input1_2\"") %>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <input type="submit" class="button" value="Tìm kiếm" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                            </td>
                            <td>
                                <div style="float: right" onclick="OnInit_CT(600, 'Lĩnh vực hoạt động, ngành nghề');">
                                    <%= Ajax.ActionLink("Thêm mới", "Index", "NhapNhanh", new { id = "TCDN_LinhVuc", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaDoanhNghiep = iID_MaDoanhNghiep }, new AjaxOptions { }, new { @class = "button", id = "TCDN_LinhVuc" })%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <table class="mGrid" id="<%= ParentID %>_thList">
                    <tr>
                        <th style="width: 2%;" align="center">
                            STT
                        </th>
                        <th style="width: 35%">
                            Tên công ty
                        </th>
                       
                        <th style="width: 5%;" align="center">
                            Sửa
                        </th>
                        <th style="width: 5%;" align="center">
                            Xóa
                        </th>
                    </tr>
                    <%
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int sSTT = i + 1;
                            DataRow R = dt.Rows[i];
                            string strDelete = "";
                            strDelete =
                                MyHtmlHelper.ActionLink(
                                    Url.Action("Delete_LinhVuc", "TCDN_HoSo_DoanhNghiep",
                                               new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"],iid_ma=R["iiD_Ma"] }).ToString(),
                                    "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                    %>
                    <tr>
                        <td align="center" style="padding: 3px 2px;">
                            <%=sSTT %>
                        </td>
                        <td align="left" style="padding: 3px 2px;">
                            <%=R["sTen"] %>
                        </td>
                        <td align="center">
                              <div  onclick="OnInit_CT(600, 'Lĩnh vực hoạt động, ngành nghề');">
                                    <%= Ajax.ActionLink("Sửa", "Index", "NhapNhanh", new { id = "TCDN_LinhVuc", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaDoanhNghiep = iID_MaDoanhNghiep, iID_Ma = R["iID_Ma"] }, new AjaxOptions { }, new { id = "TCDN_LinhVuc" })%>
                                </div>
                        </td>
                        <td align="center">
                            <%=strDelete%>
                        </td>
                    </tr>
                    <%}%>
                </table>
            </div>
        </div>
        <% } %>
    </div>
    <script type="text/javascript">
        function OnInit_CT(value, title) {
            $("#idDialog").dialog("destroy");
            document.getElementById("idDialog").title = title;
            document.getElementById("idDialog").innerHTML = "";
            $("#idDialog").dialog({
                resizable: false,
                draggable: false,
                width: value,
                modal: true
            });
        }
        function OnLoad_CT(v) {
            document.getElementById("idDialog").innerHTML = v;
        }
    </script>
    <div id="idDialog" style="display: none;">
    </div>
</asp:Content>
