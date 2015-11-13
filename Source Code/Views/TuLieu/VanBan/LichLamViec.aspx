<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_TuLieu_Default.Master" Inherits="System.Web.Mvc.ViewPage" %>

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
        cmd = new SqlCommand("SELECT * FROM TL_DanhMucTaiLieu WHERE iTrangThai=1 ORDER BY sTen ");
        DataTable dtDanhMuc = Connection.GetDataTable(cmd);
        cmd.Dispose();

        String TieuDe = "";
        if (String.IsNullOrEmpty(MaKieuTaiLieu)==false)
        {
            cmd = new SqlCommand("SELECT TOP 1 sTen FROM TL_DanhMucTaiLieu WHERE iTrangThai=1 AND iID_MaKieuTaiLieu=@iID_MaKieuTaiLieu ORDER BY sTen ");
            cmd.Parameters.AddWithValue("@iID_MaKieuTaiLieu", MaKieuTaiLieu);
            TieuDe = Connection.GetValueString(cmd, "Danh sách văn bản");
            cmd.Dispose();    
        }
        else
        {
            TieuDe = "Danh sách văn bản";
        }
        
        //dtNguoi Ky

        int CurrentPage = 1;
        if (String.IsNullOrEmpty(page) == false)
        {
            CurrentPage = Convert.ToInt32(page);
        }
        DataTable dt = TuLieuLichSuModels.Get_DanhSachVanBan(MaKieuTaiLieu, "", "", "", "", "", "", "", "", "0", CurrentPage, Globals.PageSize);


        double nums = TuLieuLichSuModels.Get_DanhSachVanBan_Count(MaKieuTaiLieu, "", "", "", "", "", "", "", "", "0");
      
        int TotalPages = (int)Math.Ceiling(nums / Globals.PageSize);
        String strPhanTrang = MyHtmlHelper.PageLinks(String.Format("Trang {0}/{1}:", CurrentPage, TotalPages), CurrentPage, TotalPages, x => Url.Action("List", new { MaKieuTaiLieu = MaKieuTaiLieu, MaDonVi = MaDonVi, TuNgay = TuNgay, DenNgay = DenNgay, page = x }));
       
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
 
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0"  style="width:100%">
                <tr>
                    <td style="width:50%">
                        <span> <%=TieuDe %></span>
                    </td>
                    <td  style="text-align: center;width:10%">
                     
                    </td>
                     <td  style="text-align: center;width:40%">
                      
                    </td>
                </tr>
            </table>
        </div>
        <div>
          
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
            
                <tr>
                    <td align="center" valign="top">
                        <div style="text-align: center; float: left;font-size:18px; margin-top: 10px; margin-left:10px;">
                            <b style="vertical-align:bottom;;padding:0px"">Có  <%=nums %> kết quả tìm được</b>
                        </div>
                       
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td width="100%" valign="top">
                                    <table class="mGrid">
                                        <tr>
                                            <th width="3%" align="center">
                                                STT
                                            </th>
                                          
                                            <th width="5%" align="center">
                                                Số tài liệu
                                            </th>
                                            <th width="30%" align="center">
                                               Nội dung
                                            </th>
                                            <th width="7%" align="center">
                                                Ngày ban hành
                                            </th>
                                            <th width="5%" align="center">
                                                Tải về
                                            </th>
                                           
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
                                             
                                                
                                        %>
                                        <tr class="<%=strClass %>">
                                            <td align="center">
                                                <%=i + 1%>
                                            </td>
                                           
                                            <td align="left">
                                                <%=MyHtmlHelper.ActionLink(Url.Action("Detail_LichLamViec", "TuLieu_VanBan", new { MaTaiLieu = dt.Rows[i]["iID_MaTaiLieu"] }), Convert.ToString(dt.Rows[i]["sSoHieu"]))%>
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
