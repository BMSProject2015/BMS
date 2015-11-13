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
        String iID_MaDonVi = Convert.ToString(ViewData["iID_MaDonVi"]);




        DataTable dt = DuToan_ChungTuModels.GetDanhSachChungTuChiTapTrung(MaND, iID_MaDonVi);
        String strThemMoi = Url.Action("Edit", "DuToan_ChungTu_ChiTapTrung", new { MaND = MaND, ParentID = ParentID });
       
        //using (Html.BeginForm("SearchSubmit", "DuToan_ChungTu", new { ParentID = ParentID, sLNS = sLNS }))
        //{
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
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Tìm kiếm</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <%
                    using (Html.BeginForm("SearchSubmit", "DuToan_ChungTu_ChiTapTrung", new { ParentID = ParentID}))
                    {       
                %>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td class="td_form2_td1" style="width: 10%">
                            <div>
                                <b>Chọn LNS</b></div>
                        </td>
                     
                    </tr>
                </table>
                <%} %>
            </div>
        </div>
    </div>
           
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách đơn vị chi tập trung</span>
                    </td>
                 
                       <td align="right" style="padding-right: 10px;">
                    <input id="Button2" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid" id="<%= ParentID %>_thList">
            <tr>
                <th style="width: 10%;" align="center">
                    STT
                </th>
                <th  align="center">
                    Đơn vị
                </th>
            </tr>
            <%
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    int STT = i + 1;
                    String sTenDonVi =R["iID_MaDonVi"]+" - "+DonViModels.Get_TenDonVi(Convert.ToString(R["iID_MaDonVi"]));
            %>
            <tr>
                <td align="center">
                    <b>
                        <%=STT%></b>
                </td>
                <td align="left">
                    <b>
                        <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuToan_ChungTuChiTiet", new { iID_MaChungTu = R["iID_MaChungTu"], iChiTapTrung = 1 }).ToString(), sTenDonVi, "Detail", "")%></b>
                </td>
                
            </tr>
            <%} %>
           
        </table>
    </div>
    <%  
        dt.Dispose();
    %>
</asp:Content>
