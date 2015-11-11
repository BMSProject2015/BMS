<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="System.Reflection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=NgonNgu.LayXau("Cổng thông tin điện tử BQP")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "Index";
    SqlCommand cmd;
    DataTable dt = new DataTable();
    DataRow R;
    String Searchid = Convert.ToString(ViewData["Searchid"]);

    //Search theo thông tin vật tư
    String sMaVatTu = Convert.ToString(ViewData["sMaVatTu"]);
    String sTen = Convert.ToString(ViewData["sTen"]);
    String sTenGoc = Convert.ToString(ViewData["sTenGoc"]);
    String sQuyCach = Convert.ToString(ViewData["sQuyCach"]);
    String cbsMaVatTu = Convert.ToString(ViewData["cbsMaVatTu"]);
    String cbsTen = Convert.ToString(ViewData["cbsTen"]);
    String cbsTenGoc = Convert.ToString(ViewData["cbsTenGoc"]);
    String cbsQuyCach = Convert.ToString(ViewData["cbsQuyCach"]);

    //Search theo danh mục vật tư
    String iDM_MaNhomLoaiVatTu = Convert.ToString(ViewData["MaNhomLoaiVatTu"]); ;
    String iDM_MaNhomChinh = Convert.ToString(ViewData["MaNhomChinh"]);
    String iDM_MaNhomPhu = Convert.ToString(ViewData["MaNhomPhu"]);
    String iDM_MaChiTietVatTu = Convert.ToString(ViewData["MaChiTietVatTu"]);
    String iDM_MaXuatXu = Convert.ToString(ViewData["MaXuatXu"]);
    String iTrangThai = Convert.ToString(ViewData["iTrangThai"]);

    cmd = new SqlCommand("SELECT iID_DMTrangThai, sTen FROM DM_TrangThai ORDER BY iSTT");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slTrangThai = new SelectOptionList(dt, "iID_DMTrangThai", "sTen");
    cmd.Dispose();    

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomLoaiVatTu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    dt.Rows.InsertAt(R, 0);
    R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R["sTenKhoa"] = "-- Nhóm loại vật tư --";
    SelectOptionList slNhomLoaiVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R["sTenKhoa"] = "-- Nhóm chính --";
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomChinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R["sTenKhoa"] = "-- Nhóm phụ --";
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slNhomPhu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R["sTenKhoa"] = "-- Chi tiết vật tư --";
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slChiTietVatTu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'XuatXu') ORDER BY sTenKhoa");
    dt = Connection.GetDataTable(cmd);
    R = dt.NewRow();
    R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R["sTenKhoa"] = "-- Xuất xứ --";
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slXuatXu = new SelectOptionList(dt, "iID_MaDanhMuc", "sTenKhoa");
    cmd.Dispose();


    cmd = new SqlCommand("SELECT * FROM NS_DonVi");
    DataTable dtDonVi = Connection.GetDataTable(cmd);
    R = dtDonVi.NewRow();
    R["iID_MaDonVi"] = "-1";
    R["sTen"] = "BQP";
    dtDonVi.Rows.InsertAt(R, 0);
    R = dtDonVi.NewRow();
    R["iID_MaDonVi"] = "0";
    R["sTen"] = "-- Đơn vị --";
    dtDonVi.Rows.InsertAt(R, 0);
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    cmd.Dispose();
    cmd.Dispose();

    cmd = new SqlCommand("SELECT sID_MaNguoiDung, sHoTen FROM QT_NguoiDung");
    DataTable dtNguoiDung = Connection.GetDataTable(cmd);
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaVatTu, sLyDo, iHanhDong " +
                     "FROM DM_LichSuGiaoDich " +
                     "WHERE iID_MaVatTu IN (SELECT iID_MaVatTu " +
                                         "FROM DM_VatTu " +
                                         "WHERE (iTrangThai = 2 OR iTrangThai = 3))" +
                     "ORDER BY dNgaySua DESC");
    DataTable dtLichSuGiaoDich = Connection.GetDataTable(cmd);

    cmd = new SqlCommand("SELECT iID_MaDanhMuc, sTen FROM DC_DanhMuc " +
                     "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                 "FROM DC_LoaiDanhMuc " +
                                                                 "WHERE sTenBang = 'DonViTinh') ORDER BY sTenKhoa");
    DataTable dtDonViTinh = Connection.GetDataTable(cmd);
    cmd.Dispose();


    String sID_MaNguoiDung = User.Identity.Name;
    String IPSua = Request.UserHostAddress;
    cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                           "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                        "FROM QT_NguoiDung " +
                                                        "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
    cmd.Parameters.AddWithValue("@sID_MaNguoiDung", sID_MaNguoiDung);
    String iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
    cmd.Dispose();

    Double rSoLuongTonKho = 0;
    String SQL = "";
    String DK = "";
    String Display = "";
    String DonViTinh = "";
    if (iID_MaDonViDangNhap == "-1")
        Display = "\"display:block\"";

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
<div class="box_tong" style=<%=Display %>>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $('#pHeader').click(function () {
                $('#dvContent').slideToggle('slow');
            });
        });
        $(document).ready(function () {
            $("DIV.ContainerPanel > DIV.collapsePanelHeader > DIV.ArrowExpand").toggle(
            function () {
                $(this).parent().next("div.Content").show("slow");
                $(this).attr("class", "ArrowClose");
            },
            function () {
                $(this).parent().next("div.Content").hide("slow");
                $(this).attr("class", "ArrowExpand");
            });
        });            
    </script>
    <div id="ContainerPanel" class="ContainerPanel">
        <div id="pHeader" class="collapsePanelHeader"> 
            <div id="dvHeaderText" class="HeaderContent" style="width: 97%;">
                <div style="width: 50%; float: left;">
                    <span><%=NgonNgu.LayXau("Tìm kiếm nhanh theo thông tin")%></span>
                </div>
                <div style="width: 50%; float: left;">
                    <span style="padding-left: 50px;"><%=NgonNgu.LayXau("Tìm kiếm theo danh mục vật tư")%></span>
                </div>
            </div>
            <div id="dvArrow" class="ArrowExpand"></div>
        </div>
        <div id="dvContent" class="Content" style="display:none">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td valign="top" align="left" style="width: 50%;">
                        <div id="nhapform">
                            <div id="form2">
                            <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                                <tr>
                                    <td width="100%">
                                        <%
                                        using (Html.BeginForm("Search", "TonKhoVatTu", new { ParentID = ParentID, Searchid = 1 }))
                                        {       
                                        %>
                                        <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                                            <tr>
                                                <td class="td_form2_td1" style="width: 20%;"><div><b>Mã vật tư</b></div></td>
                                                <td class="td_form2_td5" style="width: 80%;">
                                                    <div><%=MyHtmlHelper.TextBox(ParentID, sMaVatTu, "sMaVatTu", "", "style=\"width:92%\"")%>
                                                        <%=MyHtmlHelper.CheckBox(ParentID, cbsMaVatTu, "cbsMaVatTu", "", "")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1"><div><b>Tên vật tư</b></div></td>
                                                <td class="td_form2_td5">
                                                    <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "style=\"width:92%\"")%>
                                                        <%=MyHtmlHelper.CheckBox(ParentID, cbsTen, "cbsTen", "", "")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1"><div><b>Tên gốc</b></div></td>
                                                <td class="td_form2_td5">
                                                    <div><%=MyHtmlHelper.TextBox(ParentID, sTenGoc, "sTenGoc", "", "style=\"width:92%\"")%>
                                                        <%=MyHtmlHelper.CheckBox(ParentID, cbsTenGoc, "cbsTenGoc", "", "")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1"><div><b>Quy cách</b></div></td>
                                                <td class="td_form2_td5">
                                                    <div><%=MyHtmlHelper.TextArea(ParentID, sQuyCach, "sQuyCach", "", "style=\"width:92%;font:12px/20px Tahoma;height:50px;\"")%>
                                                        <%=MyHtmlHelper.CheckBox(ParentID, cbsQuyCach, "cbsQuyCach", "", "")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1" align="right"></td>
            	                                 <td class="td_form2_td5">
            	                                    <div><span style="font-size: 11px;">*Tích vào ô check bên cạnh tiêu chí tìm kiếm hoặc thêm ký tự % để tìm tương đối</span></div>
            	                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1" align="right"></td>
            	                                 <td class="td_form2_td5">
            	                                    <div style="text-align:right; float:right; width:100%">
                                                        <input type="submit" class="button4" value="Tìm" style="float:right; margin-left:10px;"/>
            	                                    </div> 
            	                                </td>
                                            </tr>
                                            <tr><td class="td_form2_td1" align="right" colspan="2"></td></tr>
                                        </table>
                                        <%
                                        }
                                        %>
                                    </td>
                                </tr>
                            </table>
                            </div>
                        </div>
                    </td>
                    <td valign="top" align="left" style="width: 50%;">
                        <div id="nhapform">
                            <div id="form2">
                                <%
                                using (Html.BeginForm("Search", "TonKhoVatTu", new { ParentID = ParentID, Searchid = 2 }))
                                {       
                                %>
                                <table cellpadding="5" cellspacing="5" width="100%">
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Nhóm loại vật tư</b></div></td>
                                        <td class="td_form2_td5">
                                            <div><%=MyHtmlHelper.DropDownList(ParentID, slNhomLoaiVatTu, iDM_MaNhomLoaiVatTu, "iDM_MaNhomLoaiVatTu", "", "onchange=\"ChonNhomLoaiVatTu_Search(this.value)\" style=\"width: 100%;\"")%></div>
                                            <script type="text/javascript">
                                                function ChonNhomLoaiVatTu_Search(iDM_MaNhomLoaiVatTu) {
                                                    var url = '<%= Url.Action("get_dtNhomChinh?ParentID=#0&iDM_MaNhomLoaiVatTu=#1&iDM_MaNhomChinh=#2", "DuyeVatTuAjax") %>';
                                                    url = url.replace("#0", "<%= ParentID %>");
                                                    url = url.replace("#1", iDM_MaNhomLoaiVatTu);
                                                    url = url.replace("#2", "<%= iDM_MaNhomChinh %>");
                                                    $.getJSON(url, function (data) {
                                                        document.getElementById("<%= ParentID %>_tdNhomChinh").innerHTML = data.ddlNhomChinh;
                                                        ChonNhomChinh_Search(data.iDM_MaNhomChinh);
                                                    });
                                                }
                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Nhóm chính</b></div></td>
                                        <td class="td_form2_td5">
                                             <div id="<%= ParentID %>_tdNhomChinh">
                                                <% DuyeVatTuAjaxController.NhomChinh _NhomChinh = DuyeVatTuAjaxController.get_objNhomChinh(ParentID, iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh);%>
                                                <%=_NhomChinh.ddlNhomChinh %>
                                            </div>
                                            <script type="text/javascript">
                                                function ChonNhomChinh_Search(iDM_MaNhomChinh) {
                                                    var url = '<%= Url.Action("get_dtNhomPhu?ParentID=#0&iDM_MaNhomChinh=#1&iDM_MaNhomPhu=#2", "DuyeVatTuAjax") %>';
                                                    url = url.replace("#0", "<%= ParentID %>");
                                                    url = url.replace("#1", iDM_MaNhomChinh);
                                                    url = url.replace("#2", "<%= iDM_MaNhomPhu %>");
                                                    $.getJSON(url, function (data) {
                                                        document.getElementById("<%= ParentID %>_tdNhomPhu").innerHTML = data.ddlNhomPhu;
                                                        ChonNhomPhu_Search(data.iDM_MaNhomPhu);
                                                    });
                                                }
                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Nhóm phụ</b></div></td>
                                        <td class="td_form2_td5">
                                            <div id="<%= ParentID %>_tdNhomPhu">
                                                <% DuyeVatTuAjaxController.NhomPhu _NhomPhu = DuyeVatTuAjaxController.get_objNhomPhu(ParentID, iDM_MaNhomChinh, iDM_MaNhomPhu);%>
                                                <%=_NhomPhu.ddlNhomPhu %>
                                            </div>
                                            <script type="text/javascript">
                                                function ChonNhomPhu_Search(iDM_MaNhomPhu) {
                                                    var url = '<%= Url.Action("get_dtChiTietVatTu?ParentID=#0&iDM_MaNhomPhu=#1&iDM_MaChiTietVatTu=#2", "DuyeVatTuAjax") %>';
                                                    url = url.replace("#0", "<%= ParentID %>");
                                                    url = url.replace("#1", iDM_MaNhomPhu);
                                                    url = url.replace("#2", "<%= iDM_MaChiTietVatTu %>");
                                                    $.getJSON(url, function (data) {
                                                        document.getElementById("<%= ParentID %>_tdChiTietVatTu").innerHTML = data;
                                                    });
                                                }
                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Chi tiết vật tư</b></div></td>
                                        <td class="td_form2_td5">
                                            <div id="<%= ParentID %>_tdChiTietVatTu"> 
                                                <%= DuyeVatTuAjaxController.get_objChiTietVatTu(ParentID, iDM_MaNhomPhu, iDM_MaChiTietVatTu)%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Tình trạng vật tư</b></div></td>
                                        <td class="td_form2_td5">
                                            <div><%=MyHtmlHelper.DropDownList(ParentID, slXuatXu, iDM_MaXuatXu, "iDM_MaXuatXu", "", "style=\"width: 100%;\"")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1"><div><b>Trạng thái</b></div></td>
                                        <td class="td_form2_td5">
                                            <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "style=\"width: 100%;\"")%></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_form2_td1" align="right"></td>
            	                        <td class="td_form2_td5">
            	                            <div style="text-align:right; float:right;">
            	                                <input type="submit" class="button4" value="Tìm kiếm" />
            	                            </div>
            	                        </td>
                                    </tr>
                                </table>
                                <%
                                }
                                %>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<br />
<% using (Ajax.BeginForm("Index", "Ajax", new { PartialView = "~/Views/SanPham/TonKhoVatTu/Detail.aspx", OnLoad = ParentID + "_OnLoad" }, new AjaxOptions { }))
{}%>
    <script type="text/javascript">
    function <%=ParentID%>_OnLoad(v) {
        document.getElementById("<%=ParentID%>_div").innerHTML = v;
    } 
</script>
<div id="<%=ParentID%>_div">
    <%{%>
    <%Html.RenderPartial("~/Views/SanPham/TonKhoVatTu/Detail.aspx"); %>
    <%} %>
</div>  
</asp:Content>
