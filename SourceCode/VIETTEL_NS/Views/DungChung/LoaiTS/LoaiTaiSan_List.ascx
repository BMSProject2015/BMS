<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<% 
    LoaiTaiSanModels dlParam = (LoaiTaiSanModels)Model;
    String sTen = dlParam.sTen;
    String sKyHieu = dlParam.sKyHieu;
    String page = dlParam.page;
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }
    DataTable dt = LoaiTaiSanModels.getList(sKyHieu, sTen, CurrentPage, Globals.PageSize);

    double nums = LoaiTaiSanModels.getList_Count(sKyHieu, sTen);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new
    {
        KyHieu = sKyHieu,
        Ten = sTen,
        page = x
    }));
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách loại tài sản</span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 5%;" align="center">
                STT
            </th>
            <th style="width: 10%;" align="center">
                Ký hiệu
            </th>
            <th style="width: 25%;" align="center">
                Tên loại tài sản
            </th>
            <th align="center">
                Nội dung
            </th>
            <th style="width: 10%;" align="center">
                Tỷ lệ hao mòn
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
                DataRow R = dt.Rows[i];
                String classtr = "";
                int STT = i + 1;
                String strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "LoaiTaiSan", new { iID_MaLoaiTaiSan = R["iID_MaLoaiTaiSan"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                String strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "LoaiTaiSan", new { iID_MaLoaiTaiSan = R["iID_MaLoaiTaiSan"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
               
                       
        %>
        <tr <%=classtr %>>
            <td align="center">
                <%=STT%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["iID_MaLoaiTaiSan"]))%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["sTen"]))%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["sMoTa"]))%>
            </td>
            <td align="right">
                <%= CommonFunction.DinhDangSo(R["rSoNamKhauHao"])%>
            </td>
            <td align="center">
                <%=strEdit%>
            </td>
            <td align="center">
                <%=strDelete%>
            </td>
        </tr>
        <%} %>
        <tr class="pgr">
            <td colspan="11" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%  
    if (dt != null) dt.Dispose();
 
%>