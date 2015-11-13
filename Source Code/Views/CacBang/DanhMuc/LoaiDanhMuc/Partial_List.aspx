<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<% 
    PartialModel dlChuyen = (PartialModel)Model;
    String ParentID = dlChuyen.ControlID;
    Dictionary<string, object> dicData = dlChuyen.dicData;

    Bang bang = new Bang("DC_LoaiDanhMuc");
    string TenBang = bang.TenBang;
    string TruongKhoa = bang.TruongKhoa;
    String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, TenBang);
        
    string TuKhoa = "";
    if (ViewData["TuKhoa"] != null) TuKhoa = (string)ViewData["TuKhoa"];
        
    if (String.IsNullOrEmpty(TuKhoa) == false)
    {
        string SQL = String.Format("SELECT * FROM {0} WHERE sTuKhoa like @TuKhoa", bang.TenBang);
        bang.TruyVanLayDanhSach.CommandText = SQL;
        bang.TruyVanLayDanhSach.Parameters.AddWithValue("@TuKhoa", string.Format("%{0}%", TuKhoa));
    }
    int CurrentPage = 1;
    if (dicData["LoaiDanhMuc_page"] != null) CurrentPage = (int)dicData["LoaiDanhMuc_page"];

    DataTable dt = bang.dtData("sTen ASC",CurrentPage);
        
    int TotalPages = bang.TongSoTrang();
    int TotalRecords = TotalPages * Globals.PageSize;
    int FromRecord = (CurrentPage - 1) * Globals.PageSize + 1;
    int ToRecord = CurrentPage * Globals.PageSize;
    if (TotalPages == CurrentPage)
    {
        ToRecord = FromRecord + dt.Rows.Count - 1;
    }
%>
<div class="title_form3" style="width:100%;float:left;">
    <b>Kết quả tìm kiếm từ <%=FromRecord%>-<%=ToRecord%> trong số <%=TotalRecords%> loại danh mục</b>
</div>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Danh sách loại danh mục</span>
                </td>
            </tr>
        </table>
    </div>
     <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing="0" border="0" class="mGrid" >
                <tr class="tr_form3">
                    <th width="70%"><b><%=NgonNgu.LayXau("Loại danh mục") %></b></th>
                    <th width="30%"><b><%=NgonNgu.LayXau("Thao tác")%></b></th>
                </tr>
                    <% 
                    for(int i=0;i<=dt.Rows.Count-1;i++) {
                        DataRow Row = dt.Rows[i];
                        string urlDetail = Url.Action("Detail", new { MaLoaiDanhMuc = Row[TruongKhoa] });
                        string urlEdit = Url.Action("Edit", new { MaLoaiDanhMuc = Row[TruongKhoa] });
                        string urlDelete = Url.Action("Delete", new { MaLoaiDanhMuc = Row[TruongKhoa] });
                        string urlSort = Url.Action("Sort", new { MaLoaiDanhMuc = Row[TruongKhoa] });
                        String classtr = "";
                          if (i % 2 == 0)
                        {
                            classtr = "class=\"alt\"";
                        }
                        %>
                        <tr <%=classtr %>>
                            <td style="padding: 3px 2px;"><%= MyHtmlHelper.ActionLink(urlDetail, Convert.ToString(Row["sTen"]), "Detail", sDanhSachChucNangCam)%></td>
                            <%--<td><%= MyHtmlHelper.ActionLink(Url.Action("Index", "LoaiDanhMuc", new { MaDanhMuc = Row["iID_LoaiDanhMuc"] }), NgonNgu.LayXau("Loại Danh mục"), "Detail", sDanhSachChucNangCam)%></td>--%>
                            <td style="padding: 3px 2px;">
                                <%= MyHtmlHelper.ActionLink(urlEdit, NgonNgu.LayXau("Sửa"), "Edit", sDanhSachChucNangCam)%>
                                <%if (User.Identity.Name == "admin")
                                                                                                                             { %> |
                                <%= MyHtmlHelper.ActionLink(urlDelete, NgonNgu.LayXau("Xóa"), "Delete", sDanhSachChucNangCam)%>
                                <%} %>
                                |
                                <%= MyHtmlHelper.ActionLink(urlSort, NgonNgu.LayXau("Sắp xếp"), "Sort", sDanhSachChucNangCam)%>
                            </td>
                        </tr>
                    <%  }
                    dt.Dispose();
                    %>
                <tr class="pgr">
                    <td colspan="2" align="right">
                        <%= MyHtmlHelper.PageLinks(NgonNgu.LayXau("Trang") + ":", CurrentPage, TotalPages, x => Url.Action("Index", new { LoaiDanhMuc_page = x }))%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
