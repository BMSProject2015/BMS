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
        string ParentID = "DoanhNghiep";
        string iID_MaDoanhNghiep = string.Empty;
        string sTenDoanhNghiep = Request.QueryString["sTenDoanhNghiep"];
        string sTenThuongGoi = Request.QueryString["sTenThuongGoi"];
        string sTenVietTat = Request.QueryString["sTenVietTat"];
        string iID_MaLoaiHinhDoanhNghiep = Request.QueryString["iID_MaLoaiHinhDoanhNghiep"];
        string iID_MaHinhThucHoatDong = Request.QueryString["iID_MaHinhThucHoatDong"];
        string sTenGiaoDich =Request.QueryString["sTenGiaoDich"];
        string iID_MaKhoi = Request.QueryString["iID_MaKhoi"];
        string iID_MaNhom = Request.QueryString["iID_MaNhom"];

        String strThemMoi = Url.Action("Edit", "TCDN_HoSo_DoanhNghiep");

        // Loại hình doanh nghiệp
        var dtLoaiHinhDoanhNghiep = DanhMucModels.DT_DanhMuc("TCDN_LoaiHinhDN", true, "");
        var slLoaiHinhDoanhNghiep = new SelectOptionList(dtLoaiHinhDoanhNghiep, "iID_MaDanhMuc", "sTen");
        // Hình thức hoạt động
        var dtHinhThucHoatDong = DanhMucModels.DT_DanhMuc("TCDN_HinhThucHoatDong", true, "");
        var slHinhThucHoatDong = new SelectOptionList(dtHinhThucHoatDong, "iID_MaDanhMuc", "sTen");
        // Khối
        var dtKhoi = DanhMucModels.DT_DanhMuc("TCDN_Khoi", true, "");
        var slKhoi = new SelectOptionList(dtKhoi, "iID_MaDanhMuc", "sTen");
        // Nhóm
        var dtNhom = DanhMucModels.DT_DanhMuc("TCDN_NhomDN", true, "");
        var slNhom = new SelectOptionList(dtNhom, "iID_MaDanhMuc", "sTen");

        String page = Request.QueryString["page"];
        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        // Danh sách doanh nghiệp
        DataTable dt = TCDN_HoSo_DoanhNghepModels.GetList(sTenDoanhNghiep, sTenThuongGoi, sTenGiaoDich,
                                                          iID_MaLoaiHinhDoanhNghiep, iID_MaHinhThucHoatDong, iID_MaKhoi,
                                                          iID_MaNhom,CurrentPage,Globals.PageSize);
        
        //đếm số dòng để phân trang
        double nums = TCDN_HoSo_DoanhNghepModels.getList_Count(sTenDoanhNghiep, sTenThuongGoi, sTenGiaoDich,
                                                          iID_MaLoaiHinhDoanhNghiep, iID_MaHinhThucHoatDong, iID_MaKhoi,
                                                          iID_MaNhom);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        //code phân trang
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", "TCDN_HoSo_DoanhNghiep", new
        {
            ParentID = ParentID,
            sTenDoanhNghiep=sTenDoanhNghiep, 
            sTenThuongGoi=sTenThuongGoi, 
            sTenGiaoDich=sTenGiaoDich,
            iID_MaLoaiHinhDoanhNghiep=iID_MaLoaiHinhDoanhNghiep,
            iID_MaHinhThucHoatDong=iID_MaLoaiHinhDoanhNghiep, 
            iID_MaKhoi=iID_MaLoaiHinhDoanhNghiep,
            iID_MaNhom=iID_MaLoaiHinhDoanhNghiep,
            page = x
        }));
         //sự kiện tìm kiếm được chọn
        using (Html.BeginForm("SearchSubmit", "TCDN_HoSo_DoanhNghiep", new
        {
            ParentID = ParentID,
            sTenDoanhNghiep=sTenDoanhNghiep, 
            sTenThuongGoi=sTenThuongGoi, 
            sTenGiaoDich=sTenGiaoDich,
            iID_MaLoaiHinhDoanhNghiep=iID_MaLoaiHinhDoanhNghiep,
            iID_MaHinhThucHoatDong=iID_MaLoaiHinhDoanhNghiep, 
            iID_MaKhoi=iID_MaLoaiHinhDoanhNghiep,
            iID_MaNhom=iID_MaLoaiHinhDoanhNghiep,
        }))
        {
%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%= NgonNgu.LayXau("Liên kết nhanh: ") %></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%= MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ") %>
                    |
                    <%= MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_HoSo_DoanhNghiep"),
                                                        "Danh sách doanh nghiệp") %>
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
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%= NgonNgu.LayXau("Tên doanh nghiệp") %></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.TextBox(ParentID, sTenDoanhNghiep, "sTenDoanhNghiep", "", "class=\"input1_2\"") %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%= NgonNgu.LayXau("Tên thường gọi") %></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.TextBox(ParentID, sTenThuongGoi, "sTenThuongGoi", "", "class=\"input1_2\"") %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%= NgonNgu.LayXau("Tên giao dịch") %></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.TextBox(ParentID, sTenGiaoDich, "sTenGiaoDich", "", "class=\"input1_2\"") %>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left" style="width: 45%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%= NgonNgu.LayXau("Loại hình doanh nghiệp") %></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.DropDownList(ParentID, slLoaiHinhDoanhNghiep, iID_MaLoaiHinhDoanhNghiep,
                                                          "iID_MaLoaiHinhDoanhNghiep", "", "class=\"input1_2\"") %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%= NgonNgu.LayXau("Hình thức hoạt động") %></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.DropDownList(ParentID, slHinhThucHoatDong, iID_MaHinhThucHoatDong,
                                                          "iID_MaHinhThucHoatDong", null, "style=\"width: 50%;\"") %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%= NgonNgu.LayXau("Khối") %></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.DropDownList(ParentID, slKhoi, iID_MaKhoi, "iID_MaKhoi", "",
                                                          "class=\"input1_2\" ") %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>
                                                <%= NgonNgu.LayXau("Nhóm") %></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <div>
                                            <%= MyHtmlHelper.DropDownList(ParentID, slNhom, iID_MaNhom, "iID_MaNhom", "",
                                                          "class=\"input1_2\" ") %>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" class="td_form2_td1" style="height: 10px;">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" style="background-color: #f0f9fe; padding: 0px 0px 10px 0px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <input type="submit" class="button" value="Tìm kiếm" />
                                    </td>
                                    <td style="width: 10px;">
                                    </td>
                                    <td>
                                        <input id="TaoMoi" type="button" class="button" value="Tạo mới" onclick="javascript:location.href='<%= strThemMoi %>'" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%
        }%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách hồ sơ doanh nghiệp</span>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <table class="mGrid" id="<%= ParentID %>_thList">
            <tr>
                <th style="width: 2%;" align="center">STT</th>
                <th style="width: 15%">Tên doanh nghiệp</th>
                <th style="width: 15%;" align="center">Tên viết tắt</th>
                <th style="width: 15%;" align="center">Loại hình</th>
                <th style="width: 15%;" align="center">Hình thức hoạt động</th>
                <th style="width: 15%;" align="center">Khối</th>
                <th style="width: 15%;" align="center">Nhóm</th>

                <th style="width: 2%;" align="center">Sửa</th>
                <th style="width: 2%;" align="center">Xóa</th>
            </tr>
            <%
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int sSTT = i + 1;
                    DataRow R = dt.Rows[i];
                    string strEdit = "";
                    string strDelete = "";
                    strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                    string TenLoaiHinh = "", TenHinhThuc = "", TenKhoi="",TenNhom="";
                    
                    try
                    {
                        TenLoaiHinh=Convert.ToString(DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(R["iID_MaLoaiHinhDoanhNghiep"])).Rows[0]["sTen"]);
                    }
                    catch{}
                    try{ TenHinhThuc = Convert.ToString(DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(R["iID_MaHinhThucHoatDong"])).Rows[0]["sTen"]);}
                    catch{}
                     try
                     {
                         TenKhoi =
                             Convert.ToString(
                                 DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(R["iID_MaKhoi"])).Rows[0]["sTen"]);
                     }
                     catch{}
                     try
                     {
                     TenNhom =Convert.ToString(DanhMucModels.GetRow_DanhMuc(HamChung.ConvertToString(R["iID_MaNhom"])).Rows[0]["sTen"]);
                     }
                     catch (Exception)
                     {
                     }
                     
            %>
            <tr>
                <td align="center" style="padding: 3px 2px;">
                    <%=R["rownum"]%>
                </td>
                <td align="left">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"] }), R["sTenDoanhNghiep"])%>
                   
                </td>
                <td align="left">
                     <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"] }), R["sTenVietTat"])%>
                </td>
               
                <td align="left">
                   <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"] }), TenLoaiHinh)%>
                </td>
                <td align="left">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"] }), TenHinhThuc)%>
                </td>
                 <td align="left">
                     <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"] }), TenKhoi)%>
                </td>
                 <td align="left">
                     <%=MyHtmlHelper.ActionLink(Url.Action("Edit", "TCDN_HoSo_DoanhNghiep", new { iID_MaDoanhNghiep = R["iID_MaDoanhNghiep"] }), TenNhom)%>
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
                <td colspan="9" align="right">
                    <%=strPhanTrang%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
