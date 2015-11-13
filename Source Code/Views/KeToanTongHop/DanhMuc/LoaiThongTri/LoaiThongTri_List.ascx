<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<% 
    LoaiThongTriModels dlParam = (LoaiThongTriModels)Model;
    String LoaiNS = dlParam.LoaiNS;
    String LoaiTT = dlParam.LoaiTT;
    String TKCo = dlParam.TKCo;
    String TKNo = dlParam.TKNo;
    String page = dlParam.page;
    String UserName = dlParam.UserName;
    int CurrentPage = 1;
    if (String.IsNullOrEmpty(page) == false)
    {
        CurrentPage = Convert.ToInt32(page);
    }
    int Nam = DanhMucModels.NamLamViec(UserName);
    DataTable dt = LoaiThongTriModels.getList(LoaiNS, LoaiTT, TKCo, TKNo, CurrentPage, Globals.PageSize, Nam);

    double nums = LoaiThongTriModels.getList_Count(LoaiNS, LoaiTT, TKCo, TKNo);
    int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
    String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new
    {
        LoaiNS = LoaiNS,
        LoaiTT = LoaiTT,
        TKCo = TKCo,
        TKNo = TKNo,
        page = x
    }));
%>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <span>Danh sách loại thông tri</span>
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
                Tài khoản nợ
            </th>
            <th style="width: 10%;" align="center">
                Tài khoản có
            </th>
            <th style="width: 20%;" align="center">
                Loại khoản
            </th>
            <th style="width: 20%;" align="center">
                Loại thông tri
            </th>
            <th style="width: 15%;" align="center">
                Loại ngân sách
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
                String strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "LoaiThongTri", new { iID_MaThongTri = R["iID_MaThongTri"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                String strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "LoaiThongTri", new { iID_MaThongTri = R["iID_MaThongTri"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
               
                       
        %>
        <tr <%=classtr %>>
            <td align="center">
                <%=STT%>
            </td>
            <td align="left">
                 <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["iID_MaTaiKhoanNo"]))%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["iID_MaTaiKhoanCo"]))%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["sLoaiKhoan"]))%>
            </td>
            <td align="left">
                  <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["sLoaiThongTri"]))%>
            </td>
            <td align="left">
                <%=HttpUtility.HtmlEncode(HamChung.ConvertToString(R["sTenLoaiNS"]))%>
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