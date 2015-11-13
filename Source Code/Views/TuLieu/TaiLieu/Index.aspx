<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        String ParentID = "Edit";
        String MaKieuTaiLieu = Request.QueryString["MaKieuTaiLieu"];
        String MaDonVi = Request.QueryString["MaDonVi"];
        String TuNgay = Request.QueryString["TuNgay"];
        String DenNgay = Request.QueryString["DenNgay"];
        String page = Request.QueryString["page"];
        String MaND = User.Identity.Name;
        bool isModify = LuongCongViecModel.NguoiDungTaoMoi(PhanHeModels.iID_MaPhanHeTuLieuLichSu, MaND);
        if (MaKieuTaiLieu == null || MaKieuTaiLieu == "") MaKieuTaiLieu = "0";
        if (TuNgay == null || TuNgay == "")
        {
            TuNgay = "Từ Ngày";
        }
        if (DenNgay == null || DenNgay == "")
        {
            DenNgay = "Đến Ngày";
        }

        SqlCommand cmd;
        cmd = new SqlCommand("SELECT * FROM TL_DanhMucTaiLieu WHERE iTrangThai=1 ORDER BY iSTT ");
        DataTable dtDanhMuc = Connection.GetDataTable(cmd);
        cmd.Dispose();

        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = TuLieuLichSuModels.Get_DanhSachTaiLieu(MaKieuTaiLieu, MaDonVi, TuNgay, DenNgay, CurrentPage, Globals.PageSize);

        double nums = TuLieuLichSuModels.Get_DanhSachTaiLieu_Count(MaKieuTaiLieu, MaDonVi, TuNgay, DenNgay);
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { MaKieuTaiLieu = MaKieuTaiLieu, MaDonVi = MaDonVi, TuNgay = TuNgay, DenNgay = DenNgay, page = x }));
     
    %>
    <script type="text/javascript">
        function showChild(id) {
            var isShow = true;
            var obj = document.getElementById(id);
            var img = document.getElementById("tree_img" + id);
            if (img.alt == "collapse") {
                img.src = "/Content/Themes/images/plus.gif";
                img.alt = "expand";
                isShow = false;
            } else {
                img.src = "/Content/Themes/images/minus.gif";
                img.alt = "collapse";
                isShow = true;
            }
            var parentIdList = document.getElementsByName("parentIdStr");
            if (isShow == true) {
                var i = 0;
                for (i = 0; i < parentIdList.length; i = i + 1) {
                    var parentIdStr = parentIdList[i].value;
                    var parentIdArr = parentIdStr.split("_");
                    if (parentIdArr[parentIdArr.length - 1] == id) {
                        var childId = parentIdList[i].id.split("parent_id")[1];
                        document.getElementById("tree_row" + childId).style.display = "";
                    }
                }
            } else {
            var i = 0;
                for (i = 0; i < parentIdList.length; i = i + 1) {
                    var parentIdStr = parentIdList[i].value;
                    var parentIdArr = parentIdStr.split("_");
                    if (parentIdArr[parentIdArr.length - 1] == id) {
                        var childId = parentIdList[i].id.split("parent_id")[1];
                        document.getElementById("tree_row" + childId).style.display = "none";
                        if (document.getElementById("tree_img" + childId) != null) {
                            document.getElementById("tree_img" + childId).alt = "collapse";
                            showChild(childId);
                        }
                    }
                }
            }
        }
        function timTheoMuc(id) {
            document.getElementById("<%=ParentID%>_iID_MaKieuTaiLieu").value = id;
            if (document.forms.length > 0) {
                document.forms[0].submit();
            }
        }
    </script>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 9%;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TuLieu_DanhMuc"), "Danh mục tài liệu")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách tư liệu</span>
                    </td>
                    <td>
                        <b class="btn_form3">
                            <%= MyHtmlHelper.ActionLink(Url.Action("Sort", "TuLieu_DanhMuc"), NgonNgu.LayXau("Sắp xếp"), "Sort", null)%>
                        </b>&nbsp; <b class="btn_form3">
                            <%= MyHtmlHelper.ActionLink(Url.Action("Edit", "TuLieu_DanhMuc"), NgonNgu.LayXau("Thêm danh mục"), "Create", null)%>
                        </b>
                    </td>
                </tr>
            </table>
        </div>
        <table cellpadding="0" cellspacing="0" border="0" class="table_form3">
            <tr bgcolor="#2e6e9e">
                
                <td style="padding: 10px; width: 10%;">
                 <% if (isModify)
                {
                    %>
                        <%=MyHtmlHelper.ButtonLink(Url.Action("Edit", "TuLieu_TaiLieu", new { iID_MaKieuTaiLieu=MaKieuTaiLieu}), "&nbsp;&nbsp; Thêm tài liệu &nbsp;&nbsp;", "Edit", "")%>
                    <%
                }%>
                   
                </td>
                <%
                    using (Html.BeginForm("Search", "TuLieu_TaiLieu", new { ParentID = ParentID }))
                    {   
                %>
                <td style="padding: 10px;">
                    <%=TuLieuLichSuModels.wrDanhMucTaiLieu_DDL(ParentID, Convert.ToInt32(MaKieuTaiLieu))%>
                </td>
                <td style="padding: 10px;">
                    <%
                        cmd = new SqlCommand();
                        cmd = new SqlCommand("SELECT iID_MaDonVi, sTen FROM NS_DonVi WHERE iTrangThai=1 AND iNamLamViec_DonVi=@iNamLamViec ORDER BY iID_MaDonVi");
                        cmd.Parameters.AddWithValue("@iNamLamViec", NguoiDungCauHinhModels.iNamLamViec);
                        DataTable dtDonVi = Connection.GetDataTable(cmd);
                        cmd.Dispose();
                        dtDonVi.Rows.InsertAt(dtDonVi.NewRow(), 0);
                        dtDonVi.Rows[0]["sTen"] = "-- Đơn vị --";
                        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
                    %>
                    <%=MyHtmlHelper.DropDownList("Edit", slDonVi, MaDonVi, "iID_MaDonVi", "", "style=\"width:100%;\"")%>
                    <%
                        dtDonVi.Dispose();
                    %>
                </td>
                <td>
                    <%= MyHtmlHelper.DatePicker("Edit", TuNgay, "dTuNgayUpload", "", "style=\"width:100%;\"")%>
                </td>
                <td>
                    <%= MyHtmlHelper.DatePicker("Edit", DenNgay, "dDenNgayUpload", "", "style=\"width:100%;\"")%>
                </td>
                <td style="width: 30%; padding-top: 10px;">
                    <input type="submit" class="button4" value="Tìm" />
                </td>
                <%} %>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td width="20%" valign="top">
                    <%=TuLieuLichSuModels.wrDanhMucTaiLieu_Cay(ParentID, Convert.ToInt32(MaKieuTaiLieu))%>
                </td>
                <td width="80%" valign="top">
                    <table class="mGrid">
                        <tr>
                            <th width="3%" align="center">
                                STT
                            </th>
                            <th width="10%" align="center">
                                Danh mục
                            </th>
                            <th width="10%" align="center">
                                Tên tài liệu
                            </th>
                            <th width="17%" align="center">
                                Tên tài liệu liên quan
                            </th>
                            <th width="7%" align="center">
                                Tải về
                            </th>
                            <th width="20%" align="center">
                                Đơn vị chia sẻ
                            </th>
                            <th width="10%" align="center">
                                Ngày tạo
                            </th>
                            <th width="8%" align="center">
                                Trạng thái duyệt
                            </th>
                            <% if (LuongCongViecModel.NguoiDungDuyet(PhanHeModels.iID_MaPhanHeTuLieuLichSu,MaND))
                               {
                                   %>
                                    <th width="4%" align="center">
                                        Duyệt
                                    </th>
                                   <%
                               }
                               else
                               {%>
                                    <th width="4%" align="center">
                                        Trình duyệt
                                    </th>
                               <%}%>
                            
                             <% if (!isModify)
                               {
                                   %>
                                     <th width="4%" align="center">
                                        Từ chối
                                    </th>
                                   <%
                               }%>
                              
                             <% if (isModify)
                               {
                                   %>
                                        <th width="4%" align="center">
                                            Sửa
                                        </th>
                                        <th width="4%" align="center">
                                            Xóa
                                        </th>
                                   <%
                               }%>
                            
                           
                        </tr>
                        <%
                            int i;
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                DataRow R = dt.Rows[i];
                                string TenDanhMuc = "";
                                for (int j = 0; j <= dtDanhMuc.Rows.Count - 1; j++)
                                {
                                    if (Convert.ToString(dtDanhMuc.Rows[j]["iID_MaKieuTaiLieu"]) == Convert.ToString(R["iID_MaKieuTaiLieu"]))
                                    {
                                        TenDanhMuc = CommonFunction.ValueToString(dtDanhMuc.Rows[j]["sTen"]);
                                    }
                                }
                                String strClass = "";
                                if (i % 2 == 0) strClass = "alt";
                                //Lấy thông tin trạng thái phê duyệt tư liệu
                                string iID_MaTuLieu = Convert.ToString(R["iID_MaTaiLieu"]);
                                int iID_MaTrangThaiDuyet = Convert.ToInt32(R["iID_MaTrangThaiDuyet"]);
                                int iID_MaTrangThaiDuyet_TuChoi = TuLieuLichSuModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaTuLieu, "TL_TaiLieu", "iID_MaTaiLieu");
                                int iID_MaTrangThaiDuyet_TrinhDuyet = TuLieuLichSuModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaTuLieu, "TL_TaiLieu", "iID_MaTaiLieu");
                                string TrangThaiDuyet = LuongCongViecModel.TrangThaiDuyet(iID_MaTrangThaiDuyet);

                                String strEdit = "";
                                String strDelete = "";
                                string strTrinhDuyet = string.Empty;
                                string strTuChoi = string.Empty;
                                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                                {
                                    strTuChoi = MyHtmlHelper.ActionLink(Url.Action("TuChoi", "TuLieu_TaiLieu", new { iID_MaTuLieu = iID_MaTuLieu }).ToString(), "<img src='../Content/Themes/images/untick.png' alt='' />", "", "");

                                }
                                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                                {
                                    strTrinhDuyet = MyHtmlHelper.ActionLink(Url.Action("TrinhDuyet", "TuLieu_TaiLieu", new { iID_MaTuLieu = iID_MaTuLieu }).ToString(), "<img src='../Content/Themes/images/tick.png' alt='' />", "", "");

                                }
                                if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeTuLieuLichSu, MaND, iID_MaTrangThaiDuyet))
                                {
                                    strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "TuLieu_TaiLieu", new { MaTaiLieu = dt.Rows[i]["iID_MaTaiLieu"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                                    strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "TuLieu_TaiLieu", new { MaTaiLieu = dt.Rows[i]["iID_MaTaiLieu"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                                }
                        %>
                        <tr class="<%=strClass %>">
                            <td align="center">
                                <%=i + 1%>
                            </td>
                            <td>
                                <%= MyHtmlHelper.Label(TenDanhMuc, "iID_MaKieuTaiLieu")%>
                            </td>
                            <td>
                                <%=MyHtmlHelper.ActionLink(Url.Action("Download", "TuLieu_TaiLieu", new { MaTaiLieu = dt.Rows[i]["iID_MaTaiLieu"] }), Convert.ToString(dt.Rows[i]["sTen"]))%>
                            </td>
                            <td>
                                <div>
                                    <%
                                    String tllq = Convert.ToString(R["sMaTaiLieuLienQuan"]);
                                    if (tllq != "")
                                    {
                                        String[] arr = tllq.Split(',');
                                        for (int j = 0; j < arr.Length - 1; j++)
                                        {
                                            if (arr[j] != "")
                                            {
                                                cmd = new SqlCommand("SELECT sTen FROM TL_TaiLieu WHERE iID_MaTaiLieu=@iID_MaTaiLieu");
                                                cmd.Parameters.AddWithValue("@iID_MaTaiLieu", arr[j]);
                                                if (j < arr.Length - 2)
                                                {
                                                    %>
                                                    <%=MyHtmlHelper.ActionLink(Url.Action("Download", "TuLieu_TaiLieu", new { MaTaiLieu = arr[j] }), Connection.GetValueString(cmd, ""))%>;
                                                    <%
                                                }
                                                else
                                                {
                                                      %>
                                                    <%=MyHtmlHelper.ActionLink(Url.Action("Download", "TuLieu_TaiLieu", new { MaTaiLieu = arr[j] }), Connection.GetValueString(cmd, ""))%>
                                                    <%
                                                }
                                                %>
                                                <%
                                                cmd.Dispose();
                                            }
                                        }
                                    }
                                    %>
                                </div>
                            </td>
                            <td align="center">
                                <%=MyHtmlHelper.ActionLink(Url.Action("Download", "TuLieu_TaiLieu", new { MaTaiLieu = dt.Rows[i]["iID_MaTaiLieu"] }), "Tải về")%>
                            </td>
                            <td>
                                <div>
                                    <%
                                    String tg = Convert.ToString(R["sMaDonVi"]);
                                    if (tg != "")
                                    {
                                        String[] arr = tg.Split(',');
                                        for (int j = 0; j < arr.Length - 1; j++)
                                        {
                                            if (arr[j] != "")
                                            {
                                                cmd = new SqlCommand("SELECT sTen FROM NS_DonVi WHERE iID_MaDonVi=@iID_MaDonVi");
                                                cmd.Parameters.AddWithValue("@iID_MaDonVi", arr[j]);
                                                if (j < arr.Length - 2)
                                                {
                                                %>
                                                <%=Connection.GetValueString(cmd, "")%>;
                                                <%
                                            }
                                            else
                                            {
                                                %>
                                                <%=Connection.GetValueString(cmd, "")%>
                                                <%
                                            }
                                                %>
                                                <%
                                                cmd.Dispose();
                                            }
                                        }
                                    }
                                    %>
                                </div>
                            </td>
                            <td align="center">
                                <%= MyHtmlHelper.Label(String.Format("{0:dd/MM/yyyy HH:ss}", dt.Rows[i]["dNgayUpload"]), "dNgayUpload")%>
                            </td>
                            <td>
                                <div>
                                    <b>
                                        <%=TrangThaiDuyet%></b></div>
                            </td>
                            <td align="center">
                                <%=strTrinhDuyet%>
                            </td>
                             <% if (!isModify)
                               {
                                   %>
                                       <td align="center">
                                        <%=strTuChoi%>
                                    </td>
                                   <%
                               }%>  
                          <% if (isModify)
                               {
                                   %>
                                       <td align="center">
                                            <%=strEdit%>
                                        </td>
                                        <td align="center">
                                            <%=strDelete%>
                                        </td>
                                   <%
                               }%>
                            
                        </tr>
                        <%
                            }
                            dt.Dispose();
                        %>
                        <tr class="pgr">
                            <td colspan="12" align="right">
                                <%=strPhanTrang%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
