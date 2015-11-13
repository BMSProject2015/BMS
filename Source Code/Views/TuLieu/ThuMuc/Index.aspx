<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>

<asp:Content ID="Content3" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<%
    int i;
    String UserID = User.Identity.Name;
    String ParentID = "Thumuc";
    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    SqlCommand cmd;   
    if(String.IsNullOrEmpty(page) == false){
        CurrentPage = Convert.ToInt32(page);
    }
    DataTable dt = TuLieuLichSuModels.LayDanhSachThuMuc();
    //double nums = TuLieuLichSuModels.Count_DanhSachNganh();
    //int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    //String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { page = x }));

    String strThemMoi = Url.Action("EditThuMuc", "TuLieu_ThuMuc", new { ID ="" });
   
   %>

<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách thu mục</span>
                </td>
                <td align="right" style="padding-right: 10px;">
                    <input id="TaoMoi" type="button" class="button" value="Tạo mới" onclick="javascript:location.href='<%=strThemMoi %>'" />
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid" id="<%= ParentID %>_thList">
        <tr>
            <th style="width: 5%;" align="center">STT</th>
            <th style="width: 50%;" align="center">Thư mục</th>
            <th style="width: 10%;" align="center">Loại thư mục</th>
             <th style="width: 10%;" align="center">Ngày tạo</th>
            <th style="width: 10%;" align="center">Trạng thái</th>            
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
    if (dt != null)
    {
        
   
        for (i = 0; i < dt.Rows.Count; i++)
        {
            int sSTT = i + 1;
            DataRow R = dt.Rows[i];
            String strEdit = "";
            String strDelete = "";
            DateTime dtCreate = Convert.ToDateTime(R["dNgayTao"]);
            string shortDate = dtCreate.ToString("dd/MM/yyyy");
            string sHoatDong = Convert.ToString(R["bHoatDong"]).Equals("True") ? "Có hiệu lực" : "Không có hiệu lực";
            strEdit = MyHtmlHelper.ActionLink(Url.Action("EditThuMuc", "TuLieu_ThuMuc", new { ID = R["iID_MaThuMucTaiLieu"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "EditThuMuc", "");
            strDelete = MyHtmlHelper.ActionLink(Url.Action("DeleteThuMuc", "TuLieu_ThuMuc", new { ID = R["iID_MaThuMucTaiLieu"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "DeleteThuMuc", "");
            %>
            <tr >
                <td align="center" style="padding: 3px 2px;"><%=sSTT%></td>            
                <td align="center"><%=R["sTen"]%></td>
                <td align="left">
                    <%= R["loaidanhmuc"]%>
                </td>
                <td align="center"><%=  shortDate %></td>
                <td align="center"><%= sHoatDong%></td>
                <td align="center">
                    <%=strEdit%>                   
                </td>
                <td align="center">
                    <%=strDelete%>                                       
                </td>
            </tr>
        <%}
    }%>
        
    </table>
</div>
</asp:Content>

