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
    String MaND = User.Identity.Name;
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);

    int iNam = DateTime.Now.Year;
    int iThang = DateTime.Now.Month;
    if (dtCauHinh.Rows.Count > 0)
    {
        iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
        iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
    }
    
    dtCauHinh.Dispose();
    String ParentID = "Index";
    String page = Request.QueryString["page"];
    int CurrentPage = 1;
    SqlCommand cmd;
        
    if(String.IsNullOrEmpty(page) == false){
        CurrentPage = Convert.ToInt32(page);
    }

    DataTable dt = KeToanNgoaiTeModels.Get_dtDanhMucNhanVien(CurrentPage, Globals.PageSize, iNam, iThang);

    double nums = KeToanNgoaiTeModels.Get_CountDanhMucNhanVien(iNam, iThang);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new {page  = x}));
    String strThemMoi = Url.Action("Edit", "KeToan_NgoaiTe");    
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh mục ngoại tệ</span>
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
            <th style="width: 7%;" align="center">Mã ngoại tệ</th>
            <th style="width: 15%;" align="center">Ký hiệu ngoại tệ</th>
            <th  align="center">Tên ngoại tệ</th>
            <th style="width: 15%;" align="center">Tỷ giá</th>
           
            <th style="width: 5%;" align="center">Sửa</th>
            <th style="width: 5%;" align="center">Xóa</th>
        </tr>
        <%
        for (i = 0; i < dt.Rows.Count; i++)
        {
            DataRow R = dt.Rows[i];
            String classtr = "";
            int STT = i + 1;
            string rTyGia = CommonFunction.DinhDangSo(HttpUtility.HtmlEncode(R["rTyGia"]));
            if (i % 2 == 0)
            {
                classtr = "class=\"alt\"";
            }
            %>
            <tr <%=classtr %>>
                <td align="center"><%=R["rownum"]%></td>     
                <td align="center"><%=HttpUtility.HtmlEncode(R["iID_MaNgoaiTe"])%></td>       
                <td align="left"><%=HttpUtility.HtmlEncode(R["sTen"])%></td>
                <td align="left"><%=HttpUtility.HtmlEncode(R["sTenNgoaiTe"])%></td>
           
                <td align="right"><%=rTyGia%></td>
                
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "KeToan_NgoaiTe", new { iID_MaNhanVien = R["iID_MaNgoaiTe"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "")%>
                </td>
                <td align="center">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Delete", "KeToan_NgoaiTe", new { iID_MaNhanVien = R["iID_MaNgoaiTe"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "")%>
                </td>
            </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="8" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
</asp:Content>

