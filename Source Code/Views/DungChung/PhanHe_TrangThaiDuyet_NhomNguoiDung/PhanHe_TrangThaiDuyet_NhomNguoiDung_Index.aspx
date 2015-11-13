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
        int i;
        String ParentID = "Loc";
        String MaPhanHe = Request.QueryString["MaPhanHe"];
        String MaNhomNguoiDung = Request.QueryString["MaNhomNguoiDung"];

        DataTable dt = PhanHe_TrangThaiDuyet_NhomNguoiDungModel.NS_PhanHe_TrangThaiDuyet_NhomNguoiDung(MaPhanHe,MaNhomNguoiDung);
        String strThemMoi = Url.Action("Edit", "PhanHe_TrangThaiDuyet_NhomNguoiDung");
        DataTable dtPhanHe = PhanHe_TrangThaiDuyetModel.DT_PhanHe(true, "----- Chọn tất cả phân hệ-----");
        DataTable dtNhomNguoiDung = PhanHe_TrangThaiDuyetModel.DT_NguoiDung(true, "----- Chọn tất cả nhóm người dùng-----");
        SelectOptionList optPhanHe = new SelectOptionList(dtPhanHe, "iID_MaPhanHe", "sTen");
        SelectOptionList optNhomNguoiDung = new SelectOptionList(dtNhomNguoiDung, "iID_MaNhomNguoiDung", "sTen");
        using (Html.BeginForm("Loc_Index", "PhanHe_TrangThaiDuyet_NhomNguoiDung", new { ParentID = ParentID }))
        {
    %>
    <div class="box_tong">
        <div class="title_tong">
            <table border="0" cellspacing="0" cellpadding="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách phân hệ trạng thái duyệt nhóm người dùng</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
            <table border="0" cellspacing="0" cellpadding="0" width="70%">
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Phân hệ</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, optPhanHe,MaPhanHe,"iID_MaPhanHe", null, "style=\"width: 49%;\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Nhóm người dùng sửa</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, optNhomNguoiDung,MaNhomNguoiDung, "iID_MaNhomNguoiDung", null, "style=\"width: 49%;\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td class="td_form2_td1"><div></div></td>
                    <td class="td_form2_td5">
                        <div style="padding: 3px 10px;">
                        <input type="submit" value="Lọc" class="button" />
                        </div>
                    </td>
                </tr>
            </table>
            </div>
        </div>
    </div>
    <br />
    <div class="box_tong">
        <table class="mGrid">
            <tr>
                <th style="width: 3%;" align="center">STT</th>
                <th style="width: 17%;" align="center">Mã phân hê trạng thái duyệt xem</th>
                <th style="width: 20%;" align="center">Nhóm người dùng sửa</th>
                <th style="width: 10%;" align="center">Phân hệ</th>
                <th style="width: 40%;" align="center">Tên trạng thái duyệt</th>
                <th style="width: 5%;" align="center">Sửa</th>
                <th style="width: 5%;" align="center">Xóa</th>
            </tr>
            <% 
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow R = dt.Rows[i];
                    String TenPhanHe = Convert.ToString(CommonFunction.LayTruong("NS_PhanHe", "iID_MaPhanHe", dt.Rows[i]["iID_MaPhanHe"].ToString(), "sTen"));
                    String TenNguoiDung = Convert.ToString(CommonFunction.LayTruong("QT_NhomNguoiDung", "iID_MaNhomNguoiDung", dt.Rows[i]["iID_MaNhomNguoiDung"].ToString(), "sTen"));
                    String TenTrangThaiDuyet = Convert.ToString(CommonFunction.LayTruong("NS_PhanHe_TrangThaiDuyet", "iID_MaTrangThaiDuyet", dt.Rows[i]["iID_MaTrangThaiDuyet"].ToString(), "sTen"));
                    String classtr = "";
                    int STT = i + 1;
                    if (i % 2 == 0)
                    {
                        classtr = "class=\"alt\"";
                    }
            %>
            <tr <%=classtr %>>
                <td align="center">
                    <%=STT%>
                </td>
                <td align="center">
                    <%=dt.Rows[i]["iID_MaPhanHe_TrangThaiDuyet_Xem"]%>
                </td>
                <td align="center">
                    <%=TenNguoiDung%>
                </td>
                <td align="center">
                    <%=TenPhanHe%>
                </td>
                <td align="center">
                    <%=TenTrangThaiDuyet%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "PhanHe_TrangThaiDuyet_NhomNguoiDung", new { MaPhanHe_TrangThaiDuyet_Xem = R["iID_MaPhanHe_TrangThaiDuyet_Xem"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "PhanHe_TrangThaiDuyet_NhomNguoiDung", new { MaPhanHe_TrangThaiDuyet_Xem = R["iID_MaPhanHe_TrangThaiDuyet_Xem"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
                </td>
            </tr>
            <%} %>
        </table>
    </div>
    <%} %>
</asp:Content>
