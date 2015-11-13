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
    String ParentID = "Index";
    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    SqlCommand cmd;
        
    if(String.IsNullOrEmpty(page) == false){
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = LuongModels.Get_dtDanhMucThamSo(CurrentPage, Globals.PageSize);

    double nums = LuongModels.Get_CountDanhMucThamSo();
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {page  = x}));
    String strThemMoi = Url.Action("Edit", "Luong_DanhMucThamSo");    
%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 110px;">
                <div style="padding-left: 10px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237; float: left;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Luong_BangLuong"), "D.sách bảng lương")%>
                </div>
            </td>
        </tr>
    </table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách tham số</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="Button1" type="button" class="button_title" value="Thêm mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 3%;" align="center">STT</th>
          
            <th style="width: 20%;" align="left">Nội dung</th>
              <th style="width: 7%;" align="center">Ký hiệu</th>
            <th  align="left">Tham số</th>
            <th style="width: 7%;" align="center">Ngày áp dụng</th>
            <th style="width: 7%;" align="center">Ngày kết thuc</th>
            <th style="width: 7%;" align="center">Còn sử dụng</th>
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;
            if (i % 2 == 0)
            {
                classtr = "class=\"alt\"";
            }
            %>
            <tr <%=classtr %>>
              <td align="center"><%=R["rownum"]%></td>     
                   
                <td align="left"><%=HttpUtility.HtmlEncode(R["sNoiDung"])%></td>
                <td align="left"><%=HttpUtility.HtmlEncode(R["sKyHieu"])%></td>    
                <td align="left"><%=HttpUtility.HtmlEncode(R["sThamSo"])%></td>
                <td align="left"><%=String.Format("{0:dd/MM/yyyy}", R["dThoiGianApDung_BatDau"])%></td>
                <td align="left"><%=String.Format("{0:dd/MM/yyyy}", R["dThoiGianApDung_KetThuc"])%></td>
                <td align="left"><%=MyHtmlHelper.LabelCheckBox("Index", R["bConSuDung"], "bConSuDung")%></td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "Luong_DanhMucThamSo", new { iID_MaThamSo = R["iID_MaThamSo"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "Luong_DanhMucThamSo", new { iID_MaThamSo = R["iID_MaThamSo"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
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
</asp:Content>

