<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>


<div style="padding-top: 5px; padding-bottom: 5px; color:#ec3237;">
    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham_DanhMuc", new { LoaiDanhMuc = "NhomLoaiVatTu" }), "Quản lý nhóm loại vật tư")%> |  
    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham_DanhMuc", new { LoaiDanhMuc = "NhomChinh" }), "Quản lý nhóm chính")%> |                 
    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "SanPham_DanhMuc", new { LoaiDanhMuc = "NhomPhu" }), "Quản lý nhóm phụ")%> | 
    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "MaVatTu"), "Yêu cầu tạo mã vật tư mới")%> | 
    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TimKiemVatTu"), "Tìm kiếm vật tư")%> | 
    <%
    String sID_MaNguoiDung1 = Page.User.Identity.Name;
    if (DuyeVatTuAjaxController.LayMaDonViDung(sID_MaNguoiDung1) == "-1")
    {%>
    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "DuyetVatTu"), "Duyệt mã vật tư")%> | 
    <%} %>
    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "ChoPhanHoi"), "Chờ phản hồi")%> | 
    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "LichSuGiaoDich"), "Lịch sử giao dịch")%>
</div>