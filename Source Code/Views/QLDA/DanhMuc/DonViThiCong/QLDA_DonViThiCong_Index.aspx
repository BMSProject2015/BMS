<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    int i;
    String ParentID = "QLDA_DonViThiCong";
    String MaND = User.Identity.Name;
    String sMaDonViThiCong = Request.QueryString["sMaDonViThiCong"];
    String sTenDonViThiCong = Request.QueryString["sTenDonViThiCong"];
    String page = Request.QueryString["page"];

    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }
    
    DataTable dt = QLDA_DonViThiCongModels.Get_DanhSach(sMaDonViThiCong, sTenDonViThiCong, CurrentPage, Globals.PageSize);

    double nums = QLDA_DonViThiCongModels.Get_DanhSach_Count(sMaDonViThiCong, sTenDonViThiCong);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", "QLDA_DonViThiCong", new
    {
        sMaDonViThiCong = sMaDonViThiCong,
        sTenDonViThiCong = sTenDonViThiCong,
        page = x
    }));
    String strThemMoi = Url.Action("Edit", "QLDA_DonViThiCong");
    using (Html.BeginForm("SearchSubmit", "QLDA_DonViThiCong", new { ParentID = ParentID }))
    {
%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QLDA_DonViThiCong"), "Danh sách nhà thầu")%>
            </div>
        </td>
    </tr>
</table>
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
    <div id="nhapform">
        <div id="form2">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1"><div><b>Mã nhà thầu</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.TextBox(ParentID, sMaDonViThiCong, "sMaDonViThiCong", "", "class=\"input1_2\"", 2)%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1"><div><b>Tên nhà thầu</b></div></td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.TextBox(ParentID, sTenDonViThiCong, "sTenDonViThiCong", "", "class=\"input1_2\"")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr><td colspan="2" align="center" class="td_form2_td1" style="height: 10px;"></td></tr>
                <tr>
                    <td colspan="2" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <input type="submit" class="button" value="Tìm kiếm"/>
                                </td>
                                <td style="width: 10px;"></td>
                                <td>
                                    <input class="button" type="button" value="Hủy" onclick="history.go(-1);" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<%  } %>
<br />
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                    <span>Danh sách nhà thầu</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="TaoMoi" type="button" class="button_title" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
            <th style="width: 7%;" align="center">Mã nhà thầu</th>
            <th style="width: 28%;" align="center">Tên nhà thầu</th>
            <th style="width: 17%;" align="center">Tên viết tắt</th>
            <th style="width: 10%;" align="center">Số tài khoản</th>
            <th style="width: 15%;" align="center">Ngân hàng giao dịch</th>
            <th style="width: 10%;" align="center">Mã số thuế</th>
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String sClasstr = "";
            int STT = i + 1;            
            if (i % 2 == 0) sClasstr = "alt";
            String strEdit = "";
            String strDelete = "";
            strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "QLDA_DonViThiCong", new { iID_MaDonViThiCong = R["iID_MaDonViThiCong"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
            strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "QLDA_DonViThiCong", new { iID_MaDonViThiCong = R["iID_MaDonViThiCong"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
            %>
            <tr class='<%=sClasstr %>'>
                <td style="padding: 3px 2px;" align="center"><%=STT%></td>    
                <td style="padding: 3px 2px;" align="center"><%=R["iID_MaDonViThiCong"]%></td>
                <td style="padding: 3px 2px;" align="left"><%=R["sTen"]%></td>
                <td style="padding: 3px 2px;" align="left"><%=R["sTenVietTat"]%></td>
                <td style="padding: 3px 2px;" align="center"><%=R["sSoTaiKhoan"]%></td>
                <td style="padding: 3px 2px;" align="left"><%=R["sTenNganHangGiaoDich"]%></td>
                <td style="padding: 3px 2px;" align="left"><%=R["sMaSoThue"]%></td>
                <td align="center">
                    <%=strEdit%>                   
                </td>
                <td align="center">
                    <%=strDelete%>                                       
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="9" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%  
dt.Dispose();
%>
</asp:Content>
