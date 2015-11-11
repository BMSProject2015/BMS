<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<% 
    TaiSanModel dlParam = (TaiSanModel)Model;
    String sTen = dlParam.sTen;
    String sKyHieu = dlParam.sKyHieu;
    String LoaiTS = dlParam.LoaiTS;
    String DV = dlParam.DV;
    String page = dlParam.page;
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }
    DataTable dt = TaiSanModel.getList(sKyHieu, sTen, LoaiTS, DV, CurrentPage, Globals.PageSize);

    double nums = TaiSanModel.getList_Count(sKyHieu, sTen, LoaiTS, DV);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new
    {
        KyHieu = sKyHieu,
        Ten = sTen,
        LoaiTS = LoaiTS,
        DV = DV,
        page = x
    }));
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách tài sản</span>
                </td>
            </tr>
        </table>
    </div>
    <table class="mGrid">
        <tr>
            <th style="width: 5%;" align="center">
                STT
            </th>
            <th align="center">
                Ký hiệu
            </th>
            <th style="width: 10%;" align="center">
                Đơn vị tính
            </th>
            <th align="center">
               Tên tài sản
            </th>
            <th style="width: 20%;" align="center">
                Loại tài sản
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
                String strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "TaiSan", new { iID_MaTaiSan = R["iID_MaTaiSan"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                String strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "TaiSan", new { iID_MaTaiSan = R["iID_MaTaiSan"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                String sTinhChatDuAn = Convert.ToString(DanhMucModels.GetRow_DanhMuc(Convert.ToString(R["iID_MaDanhMuc"])).Rows[0]["sTen"]);
                       
        %>
        <tr <%=classtr %>>
            <td align="center">
                <%=STT%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["sKyHieu"]))%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(sTinhChatDuAn)%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["sTen"]))%>
            </td>
            <td align="left">
                <%= HttpUtility.HtmlEncode(HamChung.ConvertToString(R["LoaiTS"]))%>
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
            <td colspan="7" align="right">
                <%=strPhanTrang%>
            </td>
        </tr>
    </table>
</div>
<%  
    if (dt != null) dt.Dispose();
 
%>