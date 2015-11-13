<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    SqlCommand cmd;
    String ParentID = "Edit";
    String iID_MaLichSuGiaoDich = Convert.ToString(ViewData["iID_MaLichSuGiaoDich"]);
    String sID_MaNguoiDung = User.Identity.Name;

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomLoaiVatTu') ORDER BY sTenKhoa");
    DataTable dt = Connection.GetDataTable(cmd);
    DataRow R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomLoaiVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomChinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomPhu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slChiTietVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'XuatXu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slXuatXu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();
    
    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                       "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                   "FROM DC_LoaiDanhMuc " +
                                                                   "WHERE sTenBang = 'DonViTinh') ORDER BY sTen");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slDonViTinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTen");
    cmd.Dispose();

    String dNgayPhatSinhMa = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
    String iID_MaDonVi = "";
    String TenDonVi = "";
    String disabled = "";
    cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                           "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                        "FROM QT_NguoiDung " +
                                                        "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
    cmd.Parameters.AddWithValue("@sID_MaNguoiDung", sID_MaNguoiDung);
    iID_MaDonVi = Connection.GetValueString(cmd, "");
    cmd.Dispose();
    if (iID_MaDonVi != "-1") disabled = "disabled =\"true\"";

    cmd = new SqlCommand("SELECT iID_DMTrangThai, sTen FROM DM_TrangThai ORDER BY iSTT");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slTrangThai = new SelectOptionList(dt, "iID_DMTrangThai", "sTen");
    String iTrangThai = "2";
    cmd.Dispose();    
    
    String sMaVatTu = "";
    String sCapMaCu = "";
    String sMaYeuCau = "";
    String sTen = "";
    String sTenGoc = "";
    String sGhiChu = "";
    String sQuyCach = "";
    String sMoTa = "";
    String sMoTaGoc = "";
    String rSoLuongTonKho = "";
    String dNgayCapNhatTonKho = "";
    String sFileDinhKem = "";
    String iDM_MaNhomLoaiVatTu = "";
    String iDM_MaNhomChinh = "";
    String iDM_MaNhomPhu = "";
    String iDM_MaChiTietVatTu = "";
    String iDM_MaXuatXu = "";
    String iDM_MaDonViTinh = "";
    String sLyDo = "";
    String sMaCu = "";
    String sNhaSanXuat = "";
    string urlfilename = "";
    if (!String.IsNullOrEmpty(iID_MaLichSuGiaoDich))
    {
        cmd = new SqlCommand("SELECT * FROM DM_LichSuGiaoDich WHERE iID_MaLichSuGiaoDich = @iID_MaLichSuGiaoDich");
        cmd.Parameters.AddWithValue("@iID_MaLichSuGiaoDich", iID_MaLichSuGiaoDich);
        dt = Connection.GetDataTable(cmd);
        cmd.Dispose();
        if (dt.Rows.Count > 0)
        {
            sMaVatTu = Convert.ToString(dt.Rows[0]["sMaVatTu"]);
            sCapMaCu = Convert.ToString(dt.Rows[0]["sCapMaCu"]);
            sMaYeuCau = Convert.ToString(dt.Rows[0]["sMaYeuCau"]);
            sTen = Convert.ToString(dt.Rows[0]["sTen"]);
            sTenGoc = Convert.ToString(dt.Rows[0]["sTenGoc"]);
            sQuyCach = Convert.ToString(dt.Rows[0]["sQuyCach"]);
            sMoTa = Convert.ToString(dt.Rows[0]["sMoTa"]);
            sMoTaGoc = Convert.ToString(dt.Rows[0]["sMoTaGoc"]);
            sGhiChu = Convert.ToString(dt.Rows[0]["sGhiChu"]);
            dNgayPhatSinhMa = String.Format("{0:dd/MM/yyyy hh:mm:ss tt}",dt.Rows[0]["dNgayPhatSinhMa"]);
            rSoLuongTonKho = Convert.ToString(dt.Rows[0]["rSoLuongTonKho"]);
            dNgayCapNhatTonKho = String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dt.Rows[0]["dNgayCapNhatTonKho"]);
            iTrangThai = Convert.ToString(dt.Rows[0]["iTrangThai"]);
            sFileDinhKem = Convert.ToString(dt.Rows[0]["sFileDinhKem"]);
            iDM_MaDonViTinh = Convert.ToString(dt.Rows[0]["iDM_MaDonViTinh"]);
            iDM_MaNhomLoaiVatTu = Convert.ToString(dt.Rows[0]["iDM_MaNhomLoaiVatTu"]);
            iDM_MaNhomChinh = Convert.ToString(dt.Rows[0]["iDM_MaNhomChinh"]);
            iDM_MaNhomPhu = Convert.ToString(dt.Rows[0]["iDM_MaNhomPhu"]);
            iDM_MaChiTietVatTu = Convert.ToString(dt.Rows[0]["iDM_MaChiTietVatTu"]);
            iDM_MaXuatXu = Convert.ToString(dt.Rows[0]["iDM_MaXuatXu"]);
            sMaCu = Convert.ToString(dt.Rows[0]["sMaCu"]);
            sNhaSanXuat = Convert.ToString(dt.Rows[0]["sNhaSanXuat"]);

            iID_MaDonVi = Convert.ToString(dt.Rows[0]["iID_MaDonVi"]);
            urlfilename = Url.Action("GetFile", "DungChung", new { Path = dt.Rows[0]["sFileDinhKem"].ToString() });
            if (iID_MaDonVi == "")
            {
                iID_MaDonVi = null;
                TenDonVi = "BQP";
            }
            sLyDo = Convert.ToString(dt.Rows[0]["sLydo"]);
        }
    }
    if (iID_MaDonVi != null)
    {
        cmd = new SqlCommand("SELECT sTen FROM NS_DonVi WHERE iID_MaDonVi = " + iID_MaDonVi);
        TenDonVi = Connection.GetValueString(cmd, "");
        cmd.Dispose();
    }

%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td style="width: 10%">
            <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <%Html.RenderPartial("~/Views/Shared/LinkNhanhVattu.ascx"); %>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin chi tiết cho mã mới</span>
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
                                <td class="td_form2_td1"><div><b>Nhóm loại vật tư</b></div></td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DropDownList(ParentID, slNhomLoaiVatTu, iDM_MaNhomLoaiVatTu, "iDM_MaNhomLoaiVatTu", "", "onchange=\"ChonNhomLoaiVatTu(this.value)\" style=\"width: 100%;\"")%></div>
                                    <script type="text/javascript">
                                        function ChonNhomLoaiVatTu(iDM_MaNhomLoaiVatTu) {
                                            var url = '<%= Url.Action("get_dtNhomChinh?ParentID=#0&iDM_MaNhomLoaiVatTu=#1&iDM_MaNhomChinh=#2", "DungChung") %>';
                                            url = url.replace("#0", "<%= ParentID %>");
                                            url = url.replace("#1", iDM_MaNhomLoaiVatTu);
                                            url = url.replace("#2", "<%= iDM_MaNhomChinh %>");
                                            $.getJSON(url, function (data) {
                                                document.getElementById("<%= ParentID %>_tdNhomChinh").innerHTML = data.ddlNhomChinh;
                                                ChonNhomChinh(data.iDM_MaNhomChinh);
                                            });
                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Nhóm chính</b></div></td>
                                <td class="td_form2_td5">
                                     <div id="<%= ParentID %>_tdNhomChinh">
                                        <% DungChungController.NhomChinh _NhomChinh = DungChungController.get_objNhomChinh(ParentID, iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh);%>
                                        <%=_NhomChinh.ddlNhomChinh %>
                                    </div>
                                    <script type="text/javascript">
                                        function ChonNhomChinh(iDM_MaNhomChinh) {
                                            var url = '<%= Url.Action("get_dtNhomPhu?ParentID=#0&iDM_MaNhomChinh=#1&iDM_MaNhomPhu=#2", "DungChung") %>';
                                            url = url.replace("#0", "<%= ParentID %>");
                                            url = url.replace("#1", iDM_MaNhomChinh);
                                            url = url.replace("#2", "<%= iDM_MaNhomPhu %>");
                                            $.getJSON(url, function (data) {
                                                document.getElementById("<%= ParentID %>_tdNhomPhu").innerHTML = data.ddlNhomPhu;
                                                ChonNhomPhu(data.iDM_MaNhomPhu);
                                            });
                                        }
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Nhóm phụ</b></div></td>
                                <td class="td_form2_td5">
                                     <div id="<%= ParentID %>_tdNhomPhu">
                                        <% DungChungController.NhomPhu _NhomPhu = DungChungController.get_objNhomPhu(ParentID, iDM_MaNhomChinh, iDM_MaNhomPhu);%>
                                        <%=_NhomPhu.ddlNhomPhu %>
                                    </div>
                                    <script type="text/javascript">
                                        function ChonNhomPhu(iDM_MaNhomPhu) {
                                            var url = '<%= Url.Action("get_dtChiTietVatTu?ParentID=#0&iDM_MaNhomPhu=#1&iDM_MaChiTietVatTu=#2", "DungChung") %>';
                                            url = url.replace("#0", "<%= ParentID %>");
                                            url = url.replace("#1", iDM_MaNhomPhu);
                                            url = url.replace("#2", "<%= iDM_MaChiTietVatTu %>");
                                            $.getJSON(url, function (data) {
                                                document.getElementById("<%= ParentID %>_tdChiTietVatTu").innerHTML = data;
                                                ChonMa();
                                            });
                                        }
                                    </script>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left" style="width: 45%;">
                        <table cellpadding="5" cellspacing="5" width="100%">
                            <tr>
                                <td class="td_form2_td1"><div><b>Chi tiết vật tư</b></div></td>
                                <td class="td_form2_td5">
                                    <div id="<%= ParentID %>_tdChiTietVatTu"> 
                                        <%= DungChungController.get_objChiTietVatTu(ParentID, iDM_MaNhomPhu, iDM_MaChiTietVatTu)%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Gợi ý</b></div></td>
                                <td class="td_form2_td5">
                                    <div> 
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Tình trạng vật tư</b></div></td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DropDownList(ParentID, slXuatXu, iDM_MaXuatXu, "iDM_MaXuatXu", "", " onchange=\"ChonMa()\" style=\"width: 100%;\"")%></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left" style="width: 10%; background: #f0f9fe; background-repeat:repeat;">&nbsp;</td>
                </tr>
            </table>
        </div>
    </div>
</div><br /> 

<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin yêu cầu - Mã yêu cầu: <%=sMaYeuCau %></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
        <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
            <tr>
                <td valign="top" align="left" width="45%">
                    <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                        <tr>
                            <td class="td_form2_td1"><div><b>Mã vật tư</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sMaVatTu, "sMaVatTu", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%>
                                <%=MyHtmlHelper.Label("","errsMaVatTu","", "style=\"color:Red\"")%>
                                </div>
                                <div style="display: none">
                                    <%=MyHtmlHelper.TextBox(ParentID, sMaYeuCau, "sMaYeuCau", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Tên</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Đơn vị tính</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, iDM_MaDonViTinh, "iDM_MaDonViTinh", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Nhà sản xuất</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sNhaSanXuat, "sNhaSanXuat", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Quy cách</b></div></td>
                            <td class="td_form2_td5">
                                <div>
                                <%=MyHtmlHelper.TextArea(ParentID, sQuyCach, "sQuyCach", "", "style=\"width:99%;font:12px/20px Tahoma;height:45px;\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Mô tả</b></div></td>
                            <td class="td_form2_td5">
                                <div style="height: 70px;">
                                <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "style=\"width:99%;font:12px/20px Tahoma;height:60px;\"")%>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" align="left" width="45%">
                    <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                        <tr>
                            <td class="td_form2_td1"><div><b>Mã hệ thống cũ</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sMaCu, "sMaCu", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>                       
                        <tr>
                            <td class="td_form2_td1"><div><b>Tên gốc (NSX)</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sTenGoc, "sTenGoc", "", "class=\"input1_2\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Mô tả gốc (NSX)</b></div></td>
                            <td class="td_form2_td5">
                                <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sMoTaGoc, "sMoTaGoc", "", "class=\"input1_2\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Đơn vị/ Ngày phát sinh mã</b></div></td>
                            <td class="td_form2_td5">
                                <%=MyHtmlHelper.TextBox(ParentID, iID_MaDonVi, "iID_MaDonVi", "", "style=\"display:none\"")%>
                                <div><%=MyHtmlHelper.TextBox(ParentID, TenDonVi, "TenDonVi", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9; width: 48%;\"")%>
                                    <%=MyHtmlHelper.TextBox(ParentID, dNgayPhatSinhMa, "NgayPhatSinhMa", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;; width: 48%;\"")%>
                                </div>
                            </td>
                        </tr>    
                        <tr>
                            <td class="td_form2_td1"><div><b>Trạng thái</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\"; disabled=\"true\"")%></div>
                            </td>
                        </tr> 
                        <tr>
                            <td class="td_form2_td1"><div><b><%=NgonNgu.LayXau("File đính kèm")%></b></div></td>
                            <td class="td_form2_td5">
                                <div style="padding-left: 0px;">
                                    <%= MyHtmlHelper.TextBox(ParentID, sFileDinhKem, "sFileDinhKem", "", "readonly=\"readonly\" style=\"width:70%;display:none;\"")%>
                                    <%if (sFileDinhKem != ""){ %>
                                    <%= MyHtmlHelper.ActionLink(urlfilename, "Download file")%><%} %>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sFileDinhKem")%>
                                    <% =MyHtmlHelper.UploadFile("upload", "Libraries/DOC", DateTime.Now.ToString("HHmmss"),"style=\"width:97%; height: 60px;\"") %>
                                    <script type="text/javascript">
                                        //upload.addFilter("Documents (*.doc)", "*.doc");
                                        upload.addFilter("All File (*.*)", "*.*");
                                        upload.addListener(upload.UPLOAD_COMPLETE, <%= ParentID%>_uploadFile);
                        
                                        //document.getElementById("<%= ParentID%>_sFileMau").innerHTML =
                                        function <%=ParentID%>_uploadFile(filename, url) {
                                            //document.getElementById("<%= ParentID%>_sFileDinhKem").value = filename;
                                            document.getElementById("<%= ParentID%>_sFileDinhKem").value = upload.serverPath + "/" + url;
                                        }
                                    </script>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Mã cũ</b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sCapMaCu, "sCapMaCu", "", "class=\"input1_2\"")%>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div style="display: none;">  
                        <%=MyHtmlHelper.TextArea(ParentID, sGhiChu, "sGhiChu", "", "style=\"width:99%;font:12px/20px Tahoma;height:80px;\"")%>   
                        <%=MyHtmlHelper.TextBox(ParentID, rSoLuongTonKho, "rSoLuongTonKho", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%>             
                        <%=MyHtmlHelper.TextBox(ParentID, dNgayCapNhatTonKho, "dNgayCapNhatTonKho", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%>
                    </div>
                </td>
                <td valign="top" align="left" style="width: 10%;">&nbsp;</td>
            </tr>
            <tr><td class="td_form2_td1" colspan="2">&nbsp;</td></tr>
        </table>
        </div>
    </div>
</div><br />
<script type="text/javascript">
      ChonMa();
     function ChonMa() {
         var MaNhomLoaiVatTu = document.getElementById('<%=ParentID %>_iDM_MaNhomLoaiVatTu');
         var MaNhomChinh = document.getElementById('<%=ParentID %>_iDM_MaNhomChinh');
         var MaNhomPhu = document.getElementById('<%=ParentID %>_iDM_MaNhomPhu');
         var MaChiTietVatTu = document.getElementById('<%=ParentID %>_iDM_MaChiTietVatTu');
         var MaXuatXu = document.getElementById('<%=ParentID %>_iDM_MaXuatXu');
         var url = '<%= Url.Action("get_dtMaVatTu?ParentID=#0&MaNhomLoaiVatTu=#1&MaNhomChinh=#2&MaNhomPhu=#3&MaChiTietVatTu=#4&MaXuatXu=#5&iID_MaVatTu=#6", "DungChung") %>';

         url = url.replace("#0", "<%= ParentID %>");

         if (MaNhomLoaiVatTu.length != 0)
             url = url.replace("#1", MaNhomLoaiVatTu.options[MaNhomLoaiVatTu.selectedIndex].text);
         else
             url = url.replace("#1", "");
         if (MaNhomChinh.length != 0)
             url = url.replace("#2", MaNhomChinh.options[MaNhomChinh.selectedIndex].text);
         else
             url = url.replace("#2", "");
         if (MaNhomPhu.length != 0)
             url = url.replace("#3", MaNhomPhu.options[MaNhomPhu.selectedIndex].text);
         else
             url = url.replace("#3", "");
         if (MaChiTietVatTu.length != 0)
             url = url.replace("#4", MaChiTietVatTu.options[MaChiTietVatTu.selectedIndex].text);
         else
             url = url.replace("#4", "");
         if (MaXuatXu.length != 0)
             url = url.replace("#5", MaXuatXu.options[MaXuatXu.selectedIndex].text);
         else
             url = url.replace("#5", "");
         url = url.replace("#6", "");
         $.getJSON(url, function(data) {
            document.getElementById("<%= ParentID %>_sMaVatTu").value = data.MaVatTu;
         });
     } 
</script> 
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
                    <td>
                        <input type="button" class="button4" value="Quay lại" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
</asp:Content>
