<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient"%>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <% 
        string iID_MaSanPham =  HamChung.ConvertToString(ViewData["iID_MaSanPham"]);
        string iID_MaLoaiHinh = HamChung.ConvertToString(ViewData["iID_MaLoaiHinh"]);
    %>
    <table class="mGrid">
        <tr>
            <th style="width: 10%;" align="center">
                Mã khoản mục
            </th>
            <th style="width: 50%;" align="center">
                Tên khoản mục
            </th>
            <th style="width: 20%;" align="center">
                Đơn vị tính
            </th>
            <th style="width: 20%;" align="center">
                Hành động
            </th>
        </tr>
        <%
            int ThuTu = 0;
            ArrayList listChiTiet = SanPham_DanhMucGiaModels.LayDanhSachDanhMuc(iID_MaSanPham,iID_MaLoaiHinh, "", 0, ref ThuTu);
            int c = listChiTiet.Count;
        %>
        <%--<%=SanPham_DanhMucGiaModels.LayXauChiTieu(sTenDanhMuc, sKyHieu, Url.Action("", ""), XauHanhDong, XauThemVT, XauThemCon, XauSapXep, "", 0, ref ThuTu)%>--%>
        <%
            foreach (Hashtable row in listChiTiet)
            {
                string urlAdd = Url.Action("Add", "SanPham_DanhMucGia", new { iID_MaDanhMucGia = row["iID_MaDanhMucGia"] });
                string urlCreate = Url.Action("Create", "SanPham_DanhMucGia", new { iID_MaDanhMucGia_Cha = row["iID_MaDanhMucGia"] });
                string urlDetail = Url.Action("Index", "SanPham_DanhMucGia", new { iID_MaDanhMucGia_Cha = row["iID_MaDanhMucGia"] });
                string urlEdit = Url.Action("Edit", "SanPham_DanhMucGia", new { iID_MaDanhMucGia = row["iID_MaDanhMucGia"] });
                string urlDelete = Url.Action("Delete", "SanPham_DanhMucGia", new { iID_MaDanhMucGia = row["iID_MaDanhMucGia"] });
                string urlSort = Url.Action("Sort", "SanPham_DanhMucGia", new { iID_MaDanhMucGia_Cha = row["iID_MaDanhMucGia"] });
        %>
        <tr>
            <%if (row["Cap"].ToString() == "0")
              { %>
            <td style="background-color: #f4f9fd; padding: 3px 3px;">
                <b>
                    <%=row["sKyHieu"]%></b>
            </td>
            <td style="background-color: #f4f9fd; padding: 3px 3px;">
                <b>
                    <%=row["sTen"]%></b>
            </td>
            <td style="background-color: #f4f9fd; padding: 3px 3px;">
                <b>
                    <%=row["tgThuTu"]%></b>
            </td>
            <td style="background-color: #f4f9fd; padding: 3px 3px;" nowrap>
                <span onclick="OnInit_CT_NEW(400, 'Sửa khoản mục');">
                    <%= Ajax.ActionLink("Sửa", "Index", "NhapNhanh", new { id = "SP_DANHMUCGIA", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaDanhMucGia = row["iID_MaDanhMucGia"] }, new AjaxOptions { }, new { })%>|
                </span><span>
                    <%=MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", "")%>|</span>
                <span onclick="OnInit_CT_NEW(400, 'Thêm mục con');">
                    <%= Ajax.ActionLink("Thêm mục con", "Index", "NhapNhanh", new { id = "SP_DANHMUCGIA", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaDanhMucGia_Cha = row["iID_MaDanhMucGia"] }, new AjaxOptions { }, new { })%>|
                </span>
                <%=MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", "")%>
            </td>
            <% }
              else
              {
                  if ((int)row["tgThuTu"] % 2 == 0)
                  {     
            %>
            <td style="background-color: #dff0fb; padding: 3px 3px;">
                <%=row["sKyHieu"]%>
            </td>
            <td style="background-color: #dff0fb; padding: 3px 3px;">
                <%=row["sTen"]%>
            </td>
            <td style="background-color: #dff0fb; padding: 3px 3px;">
                <%=row["tgThuTu"]%>
            </td>
            <td style="background-color: #dff0fb; padding: 3px 3px;">
                <div onclick="OnInit_CT_NEW(400, 'Thêm vật tư');">
                    <%= Ajax.ActionLink("Thêm vật tư", "Index", "NhapNhanh", new { id = "SP_DANHMUCGIA", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaDanhMucGia = row["iID_MaDanhMucGia"] }, new AjaxOptions { }, new {})%>
                </div>
            </td>
            <% }
                        else
                        {     
            %>
            <td style="padding: 3px 3px;">
                <%=row["sKyHieu"]%>
            </td>
            <td style="padding: 3px 3px;">
                <%=row["sTen"]%>
            </td>
            <td style="padding: 3px 3px;">
                <%=row["tgThuTu"]%>
            </td>
            <td style="padding: 3px 3px;">
                hd
            </td>
            <%    }
                    } %>
        </tr>
        <% 
            }
        %>
    </table>
