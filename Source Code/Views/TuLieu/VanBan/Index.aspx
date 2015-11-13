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
        String sTen = Request.QueryString["sTen"];
        String MaKieuTaiLieu = Request.QueryString["MaKieuTaiLieu"];
        String MaDonVi = Request.QueryString["MaDonVi"];
        String TuNgay = Request.QueryString["TuNgay"];
        String DenNgay = Request.QueryString["DenNgay"];
        String page = Request.QueryString["page"];
        String MaND = User.Identity.Name;
        String LoaiVanBan = Request.QueryString["iDM_LoaiVanBan"];
        String NoiBanHanh = Request.QueryString["iDM_NoiBanHanh"];
        String sNguoiKy = Request.QueryString["sNguoiKy"];
        String sHieuLuc = Request.QueryString["sHieuLuc"];
        String sSoHieu = Request.QueryString["sSoHieu"];
        String sTuKhoa = Request.QueryString["sTuKhoa"];
        String PageLoad = Request.QueryString["PageLoad"];
      
        bool isModify = LuongCongViecModel.NguoiDungTaoMoi(PhanHeModels.iID_MaPhanHeTuLieuLichSu, MaND);
        if (MaKieuTaiLieu == null) MaKieuTaiLieu = "-1";
        if (MaKieuTaiLieu == "") MaKieuTaiLieu = "-1";
        if (TuNgay == null || TuNgay == "")
        {
            TuNgay = "Từ Ngày";
        }
        if (DenNgay == null || DenNgay == "")
        {
            DenNgay = "Đến Ngày";
        }

        SqlCommand cmd;
        cmd = new SqlCommand("SELECT * FROM TL_DanhMucTaiLieu WHERE iTrangThai=1 ORDER BY sTen ");
        DataTable dtDanhMuc = Connection.GetDataTable(cmd);
        cmd.Dispose();

        //dtLoaiVanban
        DataTable dtLoaiVanBan = CommonFunction.Lay_dtDanhMuc("LoaiVanBan");
        DataView dv = dtLoaiVanBan.DefaultView;
        dv.Sort="sTen";
        dtLoaiVanBan = dv.ToTable();
        
        SelectOptionList slLoaiVanBan = new SelectOptionList(dtLoaiVanBan, "iID_MaDanhMuc", "sTen");
        DataRow dr = dtLoaiVanBan.NewRow();
        dr[0] = Guid.Empty;
        dr[1] = "--Chọn loại văn bản--";
        dtLoaiVanBan.Rows.InsertAt(dr, 0);
        dtLoaiVanBan.Dispose();


        //dtNoiBanHanh
        DataTable dtNoiBanHanh = CommonFunction.Lay_dtDanhMuc("TLLS_NoiBanHanh");
        dv = dtNoiBanHanh.DefaultView;
        dv.Sort = "sTen";
        dtNoiBanHanh = dv.ToTable();
        SelectOptionList slNoiBanHanh = new SelectOptionList(dtNoiBanHanh, "iID_MaDanhMuc", "sTen");
        dr = dtNoiBanHanh.NewRow();
        dr[0] = Guid.Empty;
        dr[1] = "--Chọn nơi ban hành--";
        dtNoiBanHanh.Rows.InsertAt(dr, 0);
        dtNoiBanHanh.Dispose();

        //dtNguoi Ky

        String SQL = "SELECT DISTINCT sNguoiKy FROM TL_VanBan WHERE iTrangThai=1";
        DataTable dtNguoiKy = Connection.GetDataTable(SQL);
        dv = dtNguoiKy.DefaultView;
        dv.Sort = "sNguoiKy";
        dtNguoiKy = dv.ToTable();
        SelectOptionList slNguoiKy = new SelectOptionList(dtNguoiKy, "sNguoiKy", "sNguoiKy");
        dr = dtNguoiKy.NewRow();
        dr[0] = "--Chọn người ký--";
        dtNguoiKy.Rows.InsertAt(dr, 0);
        dtNguoiKy.Dispose();
        //dtHieuluc
        DataTable dtHieuLuc = new DataTable();
        dtHieuLuc.Columns.Add("sMa",typeof(String));
        dtHieuLuc.Columns.Add("sTen", typeof(String));


        dr = dtHieuLuc.NewRow();
        dr[0] = "0";
        dr[1] = "--Chọn--";
        dtHieuLuc.Rows.Add(dr);
         dr = dtHieuLuc.NewRow();
        dr[0] = "1";
        dr[1] = "Còn hiệu lực";
        dtHieuLuc.Rows.Add(dr);

        dr = dtHieuLuc.NewRow();
        dr[0] = "2";
        dr[1] = "Hết hiệu lực";
        dtHieuLuc.Rows.Add(dr);

        dr = dtHieuLuc.NewRow();
        dr[0] = "3";
        dr[1] = "Còn hiệu lực một phần";
        dtHieuLuc.Rows.Add(dr);
        SelectOptionList slHieuLuc = new SelectOptionList(dtHieuLuc, "sMa", "sTen");
        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = TuLieuLichSuModels.Get_DanhSachVanBan(MaKieuTaiLieu, sTen, sSoHieu, LoaiVanBan, NoiBanHanh, sNguoiKy, TuNgay, DenNgay, sTuKhoa,sHieuLuc, CurrentPage, Globals.PageSize);


        double nums = TuLieuLichSuModels.Get_DanhSachVanBan_Count(MaKieuTaiLieu, sTen, sSoHieu, LoaiVanBan, NoiBanHanh, sNguoiKy, TuNgay, DenNgay, sTuKhoa, sHieuLuc);
        if (PageLoad != "1")
        {
            //dt = new DataTable();
            //nums = 0;
        }
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("Index", new { MaKieuTaiLieu = MaKieuTaiLieu, MaDonVi = MaDonVi, TuNgay = TuNgay, DenNgay = DenNgay, page = x }));
        String sDanhSachChucNangCam = BaoMat.DanhSachChucNangCam(User.Identity.Name, "TL_VanBan");
        string sDanhSachTruongCam = BaoMat.DanhSachTruongCam(User.Identity.Name, "TL_VanBan");
        
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
    <style type="text/css">
     .hethan { background: red url(../Images/grd_alt.png) repeat-x top; }
    </style>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TuLieu_DanhMuc"), "Lĩnh vực")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0"  style="width:100%">
                <tr>
                    <td style="width:50%">
                        <span>Danh sách văn bản</span>
                    </td>
                    <td  style="text-align: center;width:10%">
                        <div class="login1" style="width: 50px; height: 20px; text-align: center;">
                            <a style="cursor: pointer;"></a>
                        </div>
                    </td>
                     <td  style="text-align: center;width:40%">
                        <%--<b class="btn_form3">
                            <%= MyHtmlHelper.ActionLink(Url.Action("Sort", "TuLieu_DanhMuc"), NgonNgu.LayXau("Sắp xếp"), "Sort", null)%>
                        </b>&nbsp; <b class="btn_form3">
                            <%= MyHtmlHelper.ActionLink(Url.Action("Edit", "TuLieu_DanhMuc"), NgonNgu.LayXau("Thêm danh mục"), "Create", null)%>
                        </b>--%>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <%
                using (Html.BeginForm("Search", "TuLieu_VanBan", new { ParentID = ParentID }))
                {   
            %>
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td rowspan="3" width="21%" valign="top" height="100%">
                        <%=TuLieuLichSuModels.wrDanhMucTaiLieu_Cay(ParentID, Convert.ToInt32(MaKieuTaiLieu))%>
                    </td>
                    <td style="width:79%" valign="top" height="40%">
                        <div id="rptMain">
                            <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                                <tr>
                                    <%-- <% if (isModify)
                       {
                    %>
                    <%=MyHtmlHelper.ButtonLink(Url.Action("Edit", "TuLieu_VanBan", new { iID_MaKieuTaiLieu=MaKieuTaiLieu}), "&nbsp;&nbsp; Thêm tài liệu &nbsp;&nbsp;", "Edit", "")%>
                    <%
                        }%>--%>
                                    <td class="td_form2_td1" style="width:40% ">
                                        <div>
                                            <b>Nội dung cần tìm:</b>
                                        </div>
                                    </td>
                                    <td style="width:20%">
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "style=\"width:100%\"")%>
                                        </div>
                                    </td>
                                       <td style="width:40%">
                                    </td>
                                    <%--  <td style="padding: 10px;">
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
                </td>--%>
                                    <%--                            <td style="width: 10%; padding-top: 10px;">
                                <input type="submit" class="button4" value="Tìm" />
                            </td>--%>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Từ khóa liên quan:</b></div>
                                    </td>
                                    <td>
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sTuKhoa, "sTuKhoa", "", "style=\"width:100%\"")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Từ ngày:</b></div>
                                    </td>
                                    <td>
                                        <div>
                                            <%= MyHtmlHelper.DatePicker("Edit", TuNgay, "dTuNgayUpload", "", "style=\"width:100%;\"")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Đến ngày:</b></div>
                                    </td>
                                    <td>
                                        <div>
                                            <%= MyHtmlHelper.DatePicker("Edit", DenNgay, "dDenNgayUpload", "", "style=\"width:100%;\"")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Lĩnh vực:</b>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <%=TuLieuLichSuModels.wrDanhMucTaiLieu_DDL(ParentID, Convert.ToInt32(MaKieuTaiLieu))%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Loại văn bản:</b>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slLoaiVanBan, LoaiVanBan, "iDM_LoaiVanBan", "", "style=\"width:100%;\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Nơi ban hành:</b>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slNoiBanHanh, NoiBanHanh, "iDM_NoiBanHanh", "", "style=\"width:100%;\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Người ký:</b>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slNguoiKy, sNguoiKy, "sNguoiKy", "", "style=\"width:100%;\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Số văn bản:</b>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <%=MyHtmlHelper.TextBox(ParentID, sSoHieu, "sSoHieu", "", "style=\"width:100%\"")%>
                                        </div>
                                    </td>
                                </tr>
                                 </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Hiệu lực:</b>
                                        </div>
                                    </td>
                                    <td>
                                        <div>
                                            <%=MyHtmlHelper.DropDownList(ParentID, slHieuLuc, sHieuLuc, "sHieuLuc", "", "style=\"width:100%;\"")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <input type="submit" class="button" value="Tìm kiếm" />
                                    </td>
                                    <td align="center">
                                        <%=MyHtmlHelper.ButtonLink(Url.Action("Index", "TuLieu_VanBan", new { iID_MaKieuTaiLieu = MaKieuTaiLieu }), "Mới nhất", "Index", "", "class=\"button\"")%>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="top">
                        <div style="text-align: center; float: left;font-size:18px; ">
                            <b style="vertical-align:bottom;;padding:0px"">Có  <%=nums %> kết quả tìm được</b>
                        </div>
                        <div style="float: right">
                            <%=MyHtmlHelper.ButtonLink(Url.Action("Edit", "TuLieu_VanBan", new { iID_MaKieuTaiLieu = MaKieuTaiLieu }), "&nbsp;&nbsp; Thêm văn bản &nbsp;&nbsp;", "Edit", "")%></div>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td width="100%" valign="top">
                                    <table class="mGrid">
                                        <tr>
                                            <th width="3%" align="center">
                                                STT
                                            </th>
                                            <th width="10%" align="center">
                                                Lĩnh vực
                                            </th>
                                            <th width="10%" align="center">
                                                Loại văn bản
                                            </th>
                                            <th width="5%" align="center">
                                                Số văn bản
                                            </th>
                                            <th width="30%" align="center">
                                                Trích yếu
                                            </th>
                                            <th width="7%" align="center">
                                                Ngày ban hành
                                            </th>
                                            <th width="5%" align="center">
                                                Tải về
                                            </th>
                                            <%-- <th width="8%" align="center">
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
                               <%}%>--%>
                                            <%--   <% if (!isModify)
                                               {
                                            %>
                                            <th width="4%" align="center">
                                                Từ chối
                                            </th>
                                            <%
                                                }%>--%>
                                            <%-- <% if (isModify)
                                               {
                                            %>--%>
                                            <th width="3%" align="center">
                                                Sửa
                                            </th>
                                            <th width="3%" align="center">
                                                Xóa
                                            </th>
                                            <%--   <%
                                                }%>--%>
                                        </tr>
                                        <%
                                            int i;
                                            for (i = 0; i < dt.Rows.Count; i++)
                                            {
                                                DataRow R = dt.Rows[i];
                                                string TenDanhMuc = "", TenLoaiVanBan = "";
                                                TenLoaiVanBan = Convert.ToString(CommonFunction.LayTruong("DC_DanhMuc", "iID_MaDanhMuc", Convert.ToString(R["iDM_LoaiVanBan"]), "sTen"));
                                                for (int j = 0; j <= dtDanhMuc.Rows.Count - 1; j++)
                                                {
                                                    if (Convert.ToString(dtDanhMuc.Rows[j]["iID_MaKieuTaiLieu"]) == Convert.ToString(R["iID_MaKieuTaiLieu"]))
                                                    {
                                                        TenDanhMuc = CommonFunction.ValueToString(dtDanhMuc.Rows[j]["sTen"]);
                                                    }
                                                }
                                             
                                                String strClass = "";
                                                if (i % 2 == 0) strClass = "alt";
                                                String dNgayHetHan = String.Format("{0:dd/MM/yyyy}", dt.Rows[i]["dNgayHetHan"]);
                                                if(!String.IsNullOrEmpty(dNgayHetHan)) 
                                                {
                                                    int KQ = DateTime.Compare(Convert.ToDateTime(CommonFunction.LayNgayTuXau(dNgayHetHan)),Convert.ToDateTime(DateTime.Today));
                                                    if (KQ < 0) 
                                                    strClass = "hethan";
                                                }
                                                //Lấy thông tin trạng thái phê duyệt tư liệu
                                                string iID_MaTuLieu = Convert.ToString(R["iID_MaTaiLieu"]);
                                                int iID_MaTrangThaiDuyet = Convert.ToInt32(R["iID_MaTrangThaiDuyet"]);
                                                int iID_MaTrangThaiDuyet_TuChoi = TuLieuLichSuModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaTuLieu, "TL_VanBan", "iID_MaTaiLieu");
                                                int iID_MaTrangThaiDuyet_TrinhDuyet = TuLieuLichSuModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaTuLieu, "TL_VanBan", "iID_MaTaiLieu");
                                                string TrangThaiDuyet = LuongCongViecModel.TrangThaiDuyet(iID_MaTrangThaiDuyet);

                                                String strEdit = "";
                                                String strDelete = "";
                                                string strTrinhDuyet = string.Empty;
                                                string strTuChoi = string.Empty;
                                                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                                                {
                                                    strTuChoi = MyHtmlHelper.ActionLink(Url.Action("TuChoi", "TuLieu_VanBan", new { iID_MaTuLieu = iID_MaTuLieu }).ToString(), "<img src='../Content/Themes/images/untick.png' alt='' />", "", "");

                                                }
                                                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                                                {
                                                    strTrinhDuyet = MyHtmlHelper.ActionLink(Url.Action("TrinhDuyet", "TuLieu_VanBan", new { iID_MaTuLieu = iID_MaTuLieu }).ToString(), "<img src='../Content/Themes/images/tick.png' alt='' />", "", "");

                                                }
                                                //if (LuongCongViecModel.NguoiDung_DuocSuaChungTu(PhanHeModels.iID_MaPhanHeTuLieuLichSu, MaND, iID_MaTrangThaiDuyet))
                                                //{
                                                strEdit = MyHtmlHelper.ActionLink(Url.Action("Edit", "TuLieu_VanBan", new { MaTaiLieu = dt.Rows[i]["iID_MaTaiLieu"] }).ToString(), "<img src='../Content/Themes/images/edit.gif' alt='' />", "Edit", "");
                                                strDelete = MyHtmlHelper.ActionLink(Url.Action("Delete", "TuLieu_VanBan", new { MaTaiLieu = dt.Rows[i]["iID_MaTaiLieu"] }).ToString(), "<img src='../Content/Themes/images/delete.gif' alt='' />", "Delete", "");
                                                // }
                                        %>
                                        <tr class="<%=strClass %>">
                                            <td align="center">
                                                <%=i + 1%>
                                            </td>
                                            <td align="left">
                                                <%= MyHtmlHelper.Label(TenDanhMuc, "iID_MaKieuTaiLieu", sDanhSachTruongCam)%>
                                            </td>
                                            <td align="left">
                                                <%= MyHtmlHelper.Label(TenLoaiVanBan, "iID_MaKieuTaiLieu")%>
                                            </td>
                                            <td align="left">
                                                <%=MyHtmlHelper.ActionLink(Url.Action("Detail", "TuLieu_VanBan", new { MaTaiLieu = dt.Rows[i]["iID_MaTaiLieu"] }), Convert.ToString(dt.Rows[i]["sSoHieu"]))%>
                                            </td>
                                            <td align="left">
                                                <%= MyHtmlHelper.Label(R["sTen"], "sTen",sDanhSachTruongCam)%>
                                            </td>
                                            <td align="left">
                                                <%= MyHtmlHelper.Label(String.Format("{0:dd/MM/yyyy}", dt.Rows[i]["dNgayBanHanh"]), "dNgayBanHanh", sDanhSachTruongCam)%>
                                            </td>
                                            <td align="center">
                                                <%=MyHtmlHelper.ActionLink(Url.Action("Download", "TuLieu_VanBan", new { MaTaiLieu = dt.Rows[i]["iID_MaTaiLieu"] }), "Tải về")%>
                                            </td>
                                            <%--  <td>
                                <div>
                                    <b>
                                        <%=TrangThaiDuyet%></b></div>
                            </td>
                            <td align="center">
                                <%=strTrinhDuyet%>
                            </td>--%>
                                            <%--   <% if (!isModify)
                                               {
                                            %>
                                            <td align="center">
                                                <%=strTuChoi%>
                                            </td>
                                            <%
                                                }%>
                                            <% if (isModify)
                                               {
                                            %>--%>
                                            <td align="center">
                                                <%=strEdit%>
                                            </td>
                                            <td align="center">
                                                <%=strDelete%>
                                            </td>
                                            <%-- <%
                                                }%>--%>
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
                    </td>
                </tr>
            </table>
            <%} %>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            // $("div#rptMain").hide();
            $('div.login1 a').click(function () {
                $('div#rptMain').slideToggle('normal');
                $(this).toggleClass('active');
                return false;
            });
        });       
    </script>
</asp:Content>
