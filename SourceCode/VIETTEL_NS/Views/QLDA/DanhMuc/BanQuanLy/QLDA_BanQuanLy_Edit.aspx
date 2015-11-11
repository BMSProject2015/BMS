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
        String iID_MaBanQuanLy = Convert.ToString(ViewData["iID_MaBanQuanLy"]);        
        String ParentID = "Edit";        
        string sFlagEdit = ViewData["DuLieuMoi"] == null ? "1" : ViewData["DuLieuMoi"].ToString();
        DataTable dtDV = QLDA_BanQuanLyModels.Get_ChuDauTu(true,"--- Chọn chủ đầu tư  ---");
        SelectOptionList slChuDauTu = new SelectOptionList(dtDV, "iID_MaChuDauTu", "sTen");
        dtDV.Dispose();
        NameValueCollection data = QLDA_BanQuanLyModels.LayThongTinBanQuanLy(iID_MaBanQuanLy);
       
      
        using (Html.BeginForm("EditSubmit", "QLDA_BanQuanLy", new { ParentID = ParentID}))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaBanQuanLy", data["iID_MaBanQuanLy"])%>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_BanQuanLy"), "Ban quản lý")%>
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
                        <span>Thêm thông tin ban quản lý</span>
                        <% 
                            }
                           else
                           { %>
                        <span>Sửa thông tin ban quản lý</span>
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
                                <b>Chủ đầu tư</b> &nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slChuDauTu, data, "iID_MaChuDauTu", "", "class=\"input1_2\"")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaChuDauTu")%>
                            </div>
                        </td>
                    </tr>                
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên ban quản lý</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, data, "sTenBanQuanLy", "", "class=\"input1_2\"", 2)%>&nbsp;<br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTenBanQuanLy")%>
                     
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
