<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/QuyetToan/QuyetToan_BaoHiem.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%

    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String MaND = Request.QueryString["MaND"];
    String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];    
%>

<div class="box_tong">
<div class="title_tong">
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td>
                <span>Thông tin: F2-Thêm, DELETE-Xóa, F10-Ghi, Đối tượng(7=Ốm, 3=Thai sản)+(1-SQ, 2-QNCN, 3-CNV, 4-HS, 5-CS, 6-Truy, 7-Ốm ngắn) </span>
            </td>
        </tr>
    </table>
</div>
<div id="nhapform">
    <div id="form2">               
        <%Html.RenderPartial("~/Views/QuyetToan/ChungTuChiTiet/QuyetToan_BaoHiem_Index_DanhSach.ascx", new { ControlID = "ChiTieuChiTiet", MaND = User.Identity.Name, iID_MaChungTu = iID_MaChungTu }); %>    
    </div>
</div>
</div>
     
</asp:Content>
