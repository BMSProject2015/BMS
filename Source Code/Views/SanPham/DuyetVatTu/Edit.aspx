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
    String iID_MaVatTu = Convert.ToString(ViewData["iID_MaVatTu"]);
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

    cmd = new SqlCommand("SELECT iID_DMTrangThai, sTen FROM DM_TrangThai ORDER BY iSTT");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slTrangThai = new SelectOptionList(dt, "iID_DMTrangThai", "sTen");
    String iTrangThai = "2";
    cmd.Dispose();

    cmd = new SqlCommand("SELECT iID_MaVatTu, sLyDo, iHanhDong " +
                     "FROM DM_LichSuGiaoDich " +
                     "WHERE iID_MaVatTu IN (SELECT iID_MaVatTu " +
                                         "FROM DM_VatTu " +
                                         ")" +
                     "ORDER BY dNgaySua DESC");
    DataTable dtLichSuGiaoDich = Connection.GetDataTable(cmd);
    cmd.Dispose();

    String dNgayPhatSinhMa = "";
    String iID_MaDonVi = "";
    String TenDonVi = "";
    String sMaVatTu = "";
    String sMaYeuCau = "";
    String sCapMaCu = "";
    String sTen = Convert.ToString(ViewData["TenVatTu"]); ;
    String sTenGoc = "";
    String sGhiChu = "";
    String sQuyCach = "";
    String sMoTa = "";
    String sMoTaGoc = "";
    String rSoLuongTonKho = "";
    String dNgayCapNhatTonKho = "";
    String sFileDinhKem = "";
    String iDM_MaNhomLoaiVatTu = Convert.ToString(ViewData["iDM_MaNhomLoaiVatTu"]); ;
    String iDM_MaNhomChinh = Convert.ToString(ViewData["iDM_MaNhomChinh"]); ;
    String iDM_MaNhomPhu = Convert.ToString(ViewData["iDM_MaNhomPhu"]); ;
    String iDM_MaChiTietVatTu = Convert.ToString(ViewData["iDM_MaChiTietVatTu"]); ;
    String iDM_MaXuatXu = Convert.ToString(ViewData["iDM_MaXuatXu"]); ;
    String iDM_MaDonViTinh = Convert.ToString(ViewData["DonViTinh"]); ;
    String sLyDo = "";
    String sMaCu = "";
    String sNhaSanXuat = "";
    string urlfilename = "";
    if (!String.IsNullOrEmpty(iID_MaVatTu))
    {
        cmd = new SqlCommand("SELECT * FROM DM_VatTu WHERE iID_MaVatTu = @iID_MaVatTu");
        cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
        dt = Connection.GetDataTable(cmd);
        cmd.Dispose();
        if (dt.Rows.Count > 0)
        {
            sMaVatTu = Convert.ToString(dt.Rows[0]["sMaVatTu"]);
            sMaYeuCau = Convert.ToString(dt.Rows[0]["sMaYeuCau"]);
            sCapMaCu = Convert.ToString(dt.Rows[0]["sCapMaCu"]);
            sTen = Convert.ToString(dt.Rows[0]["sTen"]);
            sTenGoc = Convert.ToString(dt.Rows[0]["sTenGoc"]);
            sQuyCach = Convert.ToString(dt.Rows[0]["sQuyCach"]);
            sMoTa = Convert.ToString(dt.Rows[0]["sMoTa"]);
            sMoTaGoc = Convert.ToString(dt.Rows[0]["sMoTaGoc"]);
            sGhiChu = Convert.ToString(dt.Rows[0]["sGhiChu"]);
            dNgayPhatSinhMa = String.Format("{0:dd/MM/yyyy hh:mm:ss tt}", dt.Rows[0]["dNgayPhatSinhMa"]);
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

            int j;
            for (j = 0; j < dtLichSuGiaoDich.Rows.Count; j++)
            {
                if (Convert.ToString(dt.Rows[0]["iID_MaVatTu"]) == Convert.ToString(dtLichSuGiaoDich.Rows[j]["iID_MaVatTu"]))
                {
                    sLyDo = Convert.ToString(dtLichSuGiaoDich.Rows[j]["sLyDo"]);
                    break;
                }
            }
        }
    }
    if (iID_MaDonVi != null)
    {
        cmd = new SqlCommand("SELECT sTen FROM NS_DonVi WHERE iID_MaDonVi = " + iID_MaDonVi);
        TenDonVi = Connection.GetValueString(cmd, "");
        cmd.Dispose();
    }

    using (Html.BeginForm("EditSubmit", "DuyetVatTu", new { ParentID = ParentID, iID_MaVatTu = iID_MaVatTu }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_DoiTrangThai", "1")%>
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
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#pHeader1').click(function () {
            $('#dvContent1').slideToggle('slow');
        });
    });
    $(document).ready(function () {
        $('#iID_MaVatTu').click(function () {
            $('#dvContent1').slideToggle('slow');
        });
    });
    $(document).ready(function () {
        $("DIV.ContainerPanel1 > DIV.collapsePanelHeader > DIV.ArrowExpand").toggle(
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
<div id="ContainerPanel1" class="ContainerPanel">
    <div id="pHeader1" class="collapsePanelHeader"> 
        <div id="dvHeaderText1" class="HeaderContent" style="width: 97%;">
            <div style="width: 100%; float: left;">
                <span><%=NgonNgu.LayXau("Nhập thông tin chi tiết cho mã mới")%></span>
            </div>
        </div>
        <div id="dvArrow1" class="ArrowExpand"></div>
    </div>
    <div id="dvContent1" class="Content" style="display:none">
        <div id="nhapform">
            <div id="form2">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top" align="left" style="width: 45%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1"><div><b>Nhóm loại vật tư<span style="color: Red">*</span></b></div></td>
                                    <td class="td_form2_td5">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, slNhomLoaiVatTu, iDM_MaNhomLoaiVatTu, "iDM_MaNhomLoaiVatTu", "", "onchange=\"ChonNhomLoaiVatTu(this.value)\" style=\"width: 100%;\"")%>
                                            <br /><%= Html.ValidationMessage(ParentID + "_" + "err_iDM_MaNhomLoaiVatTu")%>
                                        </div>
                                        <script type="text/javascript">
                                            function ChonNhomLoaiVatTu(iDM_MaNhomLoaiVatTu) {
                                                jQuery.ajaxSetup({ cache: false }); 
                                                var url = unescape('<%= Url.Action("get_dtNhomChinh?ParentID=#0&iDM_MaNhomLoaiVatTu=#1&iDM_MaNhomChinh=#2", "DungChung") %>');
                                                url = unescape(url.replace("#0", "<%= ParentID %>"));
                                                url = unescape(url.replace("#1", iDM_MaNhomLoaiVatTu));
                                                url = unescape(url.replace("#2", "<%= iDM_MaNhomChinh %>"));
                                                $.getJSON(url, function (data) {
                                                    document.getElementById("<%= ParentID %>_tdNhomChinh").innerHTML = data.ddlNhomChinh;
                                                    ChonNhomChinh(data.iDM_MaNhomChinh);
                                                });
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Nhóm chính<span style="color: Red">*</span></b></div></td>
                                    <td class="td_form2_td5">
                                        <div id="<%= ParentID %>_tdNhomChinh">
                                            <% DungChungController.NhomChinh _NhomChinh = DungChungController.get_objNhomChinh(ParentID, iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh);%>
                                            <%=_NhomChinh.ddlNhomChinh %><br />
                                        </div>
                                        <script type="text/javascript">
                                            function ChonNhomChinh(iDM_MaNhomChinh) {
                                                jQuery.ajaxSetup({ cache: false }); 
                                                var url = unescape('<%= Url.Action("get_dtNhomPhu?ParentID=#0&iDM_MaNhomChinh=#1&iDM_MaNhomPhu=#2", "DungChung") %>');
                                                url = unescape(url.replace("#0", "<%= ParentID %>"));
                                                url = unescape(url.replace("#1", iDM_MaNhomChinh));
                                                url = unescape(url.replace("#2", "<%= iDM_MaNhomPhu %>"));
                                                $.getJSON(url, function (data) {
                                                    document.getElementById("<%= ParentID %>_tdNhomPhu").innerHTML = data.ddlNhomPhu;
                                                    ChonMa();
                                                    MaChiTietVatTuGoiY();
                                                });
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Nhóm phụ<span style="color: Red">*</span></b></div></td>
                                    <td class="td_form2_td5">
                                         <div id="<%= ParentID %>_tdNhomPhu">
                                            <% DungChungController.NhomPhu _NhomPhu = DungChungController.get_objNhomPhu(ParentID, iDM_MaNhomChinh, iDM_MaNhomPhu);%>
                                            <%=_NhomPhu.ddlNhomPhu %><br />
                                            <%= Html.ValidationMessage(ParentID + "_" + "err_iDM_MaNhomPhu")%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left" style="width: 45%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1"><div><b>Chi tiết vật tư<span style="color: Red">*</span></b></div></td>
                                    <td class="td_form2_td5">
                                        <div>
                                        <%
                                        String sTenChiTietVatTu = "";
                                        if (iDM_MaChiTietVatTu != null && iDM_MaChiTietVatTu != "")
                                        {
                                            cmd = new SqlCommand("SELECT sTenKhoa FROM DC_DanhMuc WHERE iID_MaDanhMuc=@iID_MaDanhMuc");
                                            cmd.Parameters.AddWithValue("@iID_MaDanhMuc", iDM_MaChiTietVatTu);
                                            sTenChiTietVatTu = Connection.GetValueString(cmd, "");
                                            cmd.Dispose();
                                        }
                                        %>
                                        <%=MyHtmlHelper.Autocomplete(ParentID, iDM_MaChiTietVatTu, sTenChiTietVatTu, "iDM_MaChiTietVatTu", "sTenKhoa", "", "class=\"input1_2\" readonly=\"readonly\" style=\"width:98%\" onfocus=\"onfocus_txt('" + ParentID + "_sTenKhoa','" + sTenChiTietVatTu + "');\" onBlur=\"onblue_txt('" + ParentID + "_sTenKhoa','" + sTenChiTietVatTu + "');ChonMa();\" onKeyPress=\"return disableEnterKey(event)\"")%>
                                        <%=MyHtmlHelper.AutoComplete_Initialize(ParentID + "_sTenKhoa", ParentID + "_iDM_MaChiTietVatTu", Url.Action("get_Auto_ChiTietVatTu_sTen", "DungChung"), String.Format("term1: document.getElementById('{0}_iDM_MaNhomPhu').value, term: request.term", ParentID), "func_Auto_Complete_ChiTietVatTu", new { delay = 100, minchars = 1 })%>
                                        <%--*Mã gợi ý:&nbsp;<%=MyHtmlHelper.Label(ParentID, MaChiTietVatTuGoiY, "MaChiTietVatTuGoiY", "", "style=\"width:20%;\" readonly=\"readonly\"")%>--%>
                                        <script type="text/javascript">
                                            //MaChiTietVatTuGoiY()
                                            function MaChiTietVatTuGoiY() {
                                                jQuery.ajaxSetup({ cache: false }); 
                                                var MachiTietGoiYValue = document.getElementById("<%=ParentID %>_sTenKhoa");
                                                var MaNhomPhu = document.getElementById('<%=ParentID %>_iDM_MaNhomPhu');
                                                var jsMaVatTu = '<%=sTenChiTietVatTu %>';
                                                var url = unescape('<%= Url.Action("get_dtMaChiTietVatTu?ParentID=#0&iDM_MaNhomPhu=#1", "DungChung") %>');
                                                url = unescape(url.replace("#0", "<%= ParentID %>"));
                                                if (MaNhomPhu.length != 0)
                                                    url = unescape(url.replace("#1", MaNhomPhu.options[MaNhomPhu.selectedIndex].value));
                                                else
                                                    url = unescape(url.replace("#1", ""));
                                                $.getJSON(url, function (data) {
                                                    MachiTietGoiYValue.value = data;
                                                });
                                            } 
                                        </script>
                                        <script type="text/javascript">
                                            function func_Auto_Complete_ChiTietVatTu(id, ui) {
                                                var iID_MaChiTietVatTuHT = document.getElementById("Edit_iDM_MaChiTietVatTu").value;
                                                ChonMa();
                                                return false;
                                            }                                   
                                        </script>
                                        <br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iDM_MaChiTietVatTu")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Tình trạng vật tư<span style="color: Red">*</span></b></div></td>
                                    <td class="td_form2_td5">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, slXuatXu, iDM_MaXuatXu, "iDM_MaXuatXu", "", " onchange=\"ChonMa()\" style=\"width: 100%;\"")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b></b></div></td>
                                    <td class="td_form2_td5" align="right">
                                        <div>
                                            <input type="button" class="button4" value="Ghép mã" onclick="timeGepMaVatTu();" />
                                        </div>
                                        <script type="text/javascript">
                                            function func_Auto_Gepma() {
                                                MaChiTietVatTuGoiY();
                                                var MaXuatXu = document.getElementById("<%= ParentID %>_iDM_MaXuatXu");
                                                if (MaXuatXu.options[MaXuatXu.selectedIndex].value == '') {
                                                    MaXuatXu.options[MaXuatXu.selectedIndex].value = "301E06D4-2E55-43E9-955E-A703CF4C2D1A";
                                                    MaXuatXu.options[MaXuatXu.selectedIndex].text = "1_VẬT TƯ LUÂN CHUYỂN";
                                                }
                                                return false;
                                            }

                                            function timeGepMaVatTu() {
                                                func_Auto_Gepma();
                                                var t = setTimeout("ChonMa();", 1000);                                                
                                                return false;
                                            }
                                            timeGepMaVatTu();                      
                                        </script>
                                    </td>
                                </tr>      
                            </table>
                        </td>
                        <td valign="top" align="left" style="width: 10%; background: #f0f9fe; background-repeat:repeat;">&nbsp;</td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<br /> 

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
                            <td class="td_form2_td1"><div><b>Mã vật tư<span style="color: Red">*</span></b></div></td>
                            <td class="td_form2_td5">
                                <div id="iID_MaVatTu"><%=MyHtmlHelper.TextBox(ParentID, sMaVatTu, "sMaVatTu", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%>
                                <%=MyHtmlHelper.Label("","errsMaVatTu","", "style=\"color:Red\"")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sMaVatTu")%>
                                </div>
                                <div style="display: none">
                                    <%=MyHtmlHelper.TextBox(ParentID, sMaYeuCau, "sMaYeuCau", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Tên<span style="color: Red">*</span></b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\" onblur=\"LeftTrim();RightTrim();\" style=\"text-transform: uppercase\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                                </div>
                                <script type="text/javascript">
                                    function LeftTrim() {
                                        var x = document.getElementById("<%= ParentID %>_sTen").value;
                                        document.getElementById("<%= ParentID %>_sTen").value = x.replace(/^\s+/, '');
                                    }
                                    function RightTrim() {
                                        var x = document.getElementById("<%= ParentID %>_sTen").value;
                                        document.getElementById("<%= ParentID %>_sTen").value = x.replace(/\s+$/, '');
                                    }
                                </script>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"><div><b>Đơn vị tính<span style="color: Red">*</span></b></div></td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, iDM_MaDonViTinh, "iDM_MaDonViTinh", "", "class=\"input1_2\"")%>
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iDM_MaDonViTinh")%>
                                </div>
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
                                <div><%=MyHtmlHelper.TextBox(ParentID, sCapMaCu, "sCapMaCu", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_sCapMaCu")%>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div style="display: none;">  
                        <%=MyHtmlHelper.TextBox(ParentID, "", "MaNhomLoaiVatTu", "", "style=\"display:block\"")%>
                        <%=MyHtmlHelper.TextBox(ParentID, "", "MaNhomChinh", "", "style=\"display:block\"")%>
                        <%=MyHtmlHelper.TextBox(ParentID, "", "MaNhomPhu", "", "style=\"display:block\"")%>
                        <%=MyHtmlHelper.TextBox(ParentID, "", "MaDonViTinh", "", "style=\"display:block\"")%>

                        <%=MyHtmlHelper.TextArea(ParentID, sGhiChu, "sGhiChu", "", "style=\"width:99%;font:12px/20px Tahoma;height:80px;\"")%>   
                        <%=MyHtmlHelper.TextBox(ParentID, rSoLuongTonKho, "rSoLuongTonKho", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%>             
                        <%=MyHtmlHelper.TextBox(ParentID, dNgayCapNhatTonKho, "dNgayCapNhatTonKho", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%>
                    </div>
                </td>
                <td valign="top" align="left" style="width: 10%;">&nbsp;</td>
            </tr>
        </table>
        </div>
    </div>
</div><br />
<script type="text/javascript">
      ChonMa();
      function ChonMa() {
          jQuery.ajaxSetup({ cache: false }); 
         var MaNhomLoaiVatTu = document.getElementById('<%=ParentID %>_iDM_MaNhomLoaiVatTu');
         var MaNhomChinh = document.getElementById('<%=ParentID %>_iDM_MaNhomChinh');
         var MaNhomPhu = document.getElementById('<%=ParentID %>_iDM_MaNhomPhu');
         var MaChiTietVatTu = document.getElementById('<%=ParentID %>_iDM_MaChiTietVatTu');
         var MaChiTietVatTuLayMa = document.getElementById('<%=ParentID %>_sTenKhoa');
         var MaXuatXu = document.getElementById('<%=ParentID %>_iDM_MaXuatXu');
         var url = unescape('<%= Url.Action("get_dtMaVatTu?ParentID=#0&MaNhomLoaiVatTu=#1&MaNhomChinh=#2&MaNhomPhu=#3&MaChiTietVatTu=#4&MaXuatXu=#5&iID_MaVatTu=#6", "DungChung") %>');

         url = unescape(url.replace("#0", "<%= ParentID %>"));

         if (MaNhomLoaiVatTu.length != 0)
             url = unescape(url.replace("#1", MaNhomLoaiVatTu.options[MaNhomLoaiVatTu.selectedIndex].text));
         else
             url = unescape(url.replace("#1", ""));
         if (MaNhomChinh.length != 0)
             url = unescape(url.replace("#2", MaNhomChinh.options[MaNhomChinh.selectedIndex].text));
         else
             url = unescape(url.replace("#2", ""));
         if (MaNhomPhu.length != 0)
             url = unescape(url.replace("#3", MaNhomPhu.options[MaNhomPhu.selectedIndex].text));
         else
             url = unescape(url.replace("#3", ""));
         //alert(MaChiTietVatTuLayMa.value);
         if (MaChiTietVatTu.length != 0)
             url = unescape(url.replace("#4", MaChiTietVatTuLayMa.value));
         else
             url = unescape(url.replace("#4", MaChiTietVatTuLayMa.value));
         if (MaXuatXu.length != 0)
             url = unescape(url.replace("#5", MaXuatXu.options[MaXuatXu.selectedIndex].text));
         else
             url = unescape(url.replace("#5", ""));
         url = unescape(url.replace("#6", "<%= iID_MaVatTu %>"));
         $.getJSON(url, function(data) {
            document.getElementById("<%= ParentID %>_sMaVatTu").value = data.MaVatTu;
             if (data.Loi == "1") {
                 document.getElementById("errsMaVatTu").innerHTML = "*Trùng mã vật tư";
                 document.getElementById("idDuyetBQP").disabled = true;
                 document.getElementById("idtuchoi").disabled = true;
             }
             else {
                 document.getElementById("errsMaVatTu").innerHTML = "";
                 document.getElementById("idDuyetBQP").disabled = false;
                 document.getElementById("idtuchoi").disabled = false;
             }
         });
     }
     function DuyetBQP() {
         var MaNhomLoaiVatTu = document.getElementById('<%=ParentID %>_iDM_MaNhomLoaiVatTu');
         var MaNhomChinh = document.getElementById('<%=ParentID %>_iDM_MaNhomChinh');
         var MaNhomPhu = document.getElementById('<%=ParentID %>_iDM_MaNhomPhu');
         var MaDonViTinh = document.getElementById('<%=ParentID %>_iDM_MaDonViTinh');
         document.getElementById('<%=ParentID %>_MaNhomLoaiVatTu').value = MaNhomLoaiVatTu.options[MaNhomLoaiVatTu.selectedIndex].text;
         document.getElementById('<%=ParentID %>_MaNhomChinh').value = MaNhomChinh.options[MaNhomChinh.selectedIndex].text;
         document.getElementById('<%=ParentID %>_MaNhomPhu').value = MaNhomPhu.options[MaNhomPhu.selectedIndex].text;
         document.getElementById('<%=ParentID %>_MaDonViTinh').value = MaDonViTinh.options[MaDonViTinh.selectedIndex].text;

         document.getElementById("<%= ParentID %>_DoiTrangThai").value = "1";
     }
     function TuChoi() {
         var MaNhomLoaiVatTu = document.getElementById('<%=ParentID %>_iDM_MaNhomLoaiVatTu');
         var MaNhomChinh = document.getElementById('<%=ParentID %>_iDM_MaNhomChinh');
         var MaNhomPhu = document.getElementById('<%=ParentID %>_iDM_MaNhomPhu');
         var MaDonViTinh = document.getElementById('<%=ParentID %>_iDM_MaDonViTinh');
         document.getElementById('<%=ParentID %>_MaNhomLoaiVatTu').value = MaNhomLoaiVatTu.options[MaNhomLoaiVatTu.selectedIndex].text;
         document.getElementById('<%=ParentID %>_MaNhomChinh').value = MaNhomChinh.options[MaNhomChinh.selectedIndex].text;
         document.getElementById('<%=ParentID %>_MaNhomPhu').value = MaNhomPhu.options[MaNhomPhu.selectedIndex].text;
         document.getElementById('<%=ParentID %>_MaDonViTinh').value = MaDonViTinh.options[MaDonViTinh.selectedIndex].text;

         document.getElementById("<%= ParentID %>_DoiTrangThai").value = "3";
     }
     function GuiBQP() {
         document.getElementById("<%= ParentID %>_DoiTrangThai").value = "4";
     }
     function CapMaCu() {
         var MaNhomLoaiVatTu = document.getElementById('<%=ParentID %>_iDM_MaNhomLoaiVatTu');
         var MaNhomChinh = document.getElementById('<%=ParentID %>_iDM_MaNhomChinh');
         var MaNhomPhu = document.getElementById('<%=ParentID %>_iDM_MaNhomPhu');
         var MaDonViTinh = document.getElementById('<%=ParentID %>_iDM_MaDonViTinh');
         document.getElementById('<%=ParentID %>_MaNhomLoaiVatTu').value = MaNhomLoaiVatTu.options[MaNhomLoaiVatTu.selectedIndex].text;
         document.getElementById('<%=ParentID %>_MaNhomChinh').value = MaNhomChinh.options[MaNhomChinh.selectedIndex].text;
         document.getElementById('<%=ParentID %>_MaNhomPhu').value = MaNhomPhu.options[MaNhomPhu.selectedIndex].text;
         document.getElementById('<%=ParentID %>_MaDonViTinh').value = MaDonViTinh.options[MaDonViTinh.selectedIndex].text;

         document.getElementById("<%= ParentID %>_DoiTrangThai").value = "5";
     }
</script>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="100%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right" width="100%">
        	    <tr>
                    <td width="12%">&nbsp;</td>
                    <td width="5%" align="right"><div><b>Phản hồi</b></div></td>
                    <td width="1%"></td>
                    <td align="right">
                        <%=MyHtmlHelper.TextArea(ParentID, sLyDo, "sLyDo", "", "style=\"width:98%;font:12px/20px Tahoma;height:40px;\"")%>
                    </td>
                    <td width="1%"></td>
            	    <td width="5%" style="padding-top: 22px;">
            	        <input type="submit" id="idtuchoi" class="button4" value="Từ chối" onclick="TuChoi();"/>
            	    </td>
                    <%
                    if (DuyeVatTuAjaxController.LayMaDonViDung(sID_MaNguoiDung) != "-1")
                    {%>
                    <td width="1%"></td>
                    <td width="5%" style="padding-top: 21px;">
            	        <input type="submit" id="idGuiBQP" class="button8" value="Gửi BQP" onclick="GuiBQP();"/>
            	    </td>
                    <%} %> 
                    <%
                    if (DuyeVatTuAjaxController.LayMaDonViDung(sID_MaNguoiDung) == "-1")
                    {%>
                    <td width="1%"></td>
                    <td width="5%" style="padding-top: 21px;">
            	        <input type="submit" id="idDuyetBQP" class="button8" value="Duyệt mã mới" onclick="DuyetBQP();" />
            	    </td>                
                    <%}%> 
            	    <td width="1%"></td>
        	        <td width="5%" style="padding-top: 21px;">
            	        <input type="submit" id="idCapMaMoi" class="button8" value="Cấp mã cũ" onclick="CapMaCu();"/>
            	    </td>
                    <td width="1%"></td>
                    <td width="5%" style="padding-top: 22px;">
                        <input type="button" class="button4" value="Quay lại" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
<%
    }
%>
<%
    SqlCommand cmd_Search;
    DataTable dt_Search = new DataTable();
    DataRow R_Search;

    String Searchid = Convert.ToString(ViewData["Searchid"]);

    //Search theo thông tin vật tư
    String sMaVatTu_Search = Convert.ToString(ViewData["sMaVatTu_Search"]);
    String sTen_Search = Convert.ToString(ViewData["sTen_Search"]);
    String sTenGoc_Search = Convert.ToString(ViewData["sTenGoc_Search"]);
    String sQuyCach_Search = Convert.ToString(ViewData["sQuyCach_Search"]);
    String cbsMaVatTu_Search = Convert.ToString(ViewData["cbsMaVatTu_Search"]);
    String cbsTen_Search = Convert.ToString(ViewData["cbsTen_Search"]);
    String cbsTenGoc_Search = Convert.ToString(ViewData["cbsTenGoc_Search"]);
    String cbsQuyCach_Search = Convert.ToString(ViewData["cbsQuyCach_Search"]);

    //Search theo danh mục vật tư
    String iDM_MaNhomLoaiVatTu_Search = Convert.ToString(ViewData["MaNhomLoaiVatTu_Search"]); ;
    String iDM_MaNhomChinh_Search = Convert.ToString(ViewData["MaNhomChinh_Search"]);
    String iDM_MaNhomPhu_Search = Convert.ToString(ViewData["MaNhomPhu_Search"]);
    String iDM_MaChiTietVatTu_Search = Convert.ToString(ViewData["MaChiTietVatTu_Search"]);
    String iDM_MaXuatXu_Search = Convert.ToString(ViewData["MaXuatXu_Search"]);
    String iTrangThai_Search = Convert.ToString(ViewData["iTrangThai_Search"]);

    dt_Search.Columns.Add("Ma");
    dt_Search.Columns.Add("Ten");
    R_Search = dt_Search.NewRow();
    R_Search["Ma"] = -1;
    R_Search["Ten"] = "-- Trạng thái --";
    dt_Search.Rows.Add(R_Search);
    R_Search = dt_Search.NewRow();
    R_Search["Ma"] = 1;
    R_Search["Ten"] = "Đang sử dụng";
    dt_Search.Rows.Add(R_Search);
    R_Search = dt_Search.NewRow();
    R_Search["Ma"] = 0;
    R_Search["Ten"] = "Ngừng hoạt động";
    dt_Search.Rows.Add(R_Search);
    R_Search = dt_Search.NewRow();
    R_Search["Ma"] = 2;
    R_Search["Ten"] = "Chờ duyệt";
    dt_Search.Rows.Add(R_Search);
    R_Search = dt_Search.NewRow();
    R_Search["Ma"] = 3;
    R_Search["Ten"] = "Từ chối";
    dt_Search.Rows.Add(R_Search);
    SelectOptionList slTrangThai_Search = new SelectOptionList(dt_Search, "Ma", "Ten");
    dt_Search.Dispose();


    cmd_Search = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomLoaiVatTu') ORDER BY sTenKhoa");
    dt_Search = Connection.GetDataTable(cmd_Search);
    R_Search = dt_Search.NewRow();
    dt_Search.Rows.InsertAt(R_Search, 0);
    R_Search["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R_Search["sTenKhoa"] = "-- Nhóm loại vật tư --";
    SelectOptionList slNhomLoaiVatTu_Search = new SelectOptionList(dt_Search, "iID_MaDanhMuc", "sTenKhoa");
    cmd_Search.Dispose();

    cmd_Search = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomChinh') ORDER BY sTenKhoa");
    dt_Search = Connection.GetDataTable(cmd_Search);
    R_Search = dt_Search.NewRow();
    R_Search["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R_Search["sTenKhoa"] = "-- Nhóm chính --";
    dt_Search.Rows.InsertAt(R_Search, 0);
    SelectOptionList slNhomChinh_Search = new SelectOptionList(dt_Search, "iID_MaDanhMuc", "sTenKhoa");
    cmd_Search.Dispose();

    cmd_Search = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'NhomPhu') ORDER BY sTenKhoa");
    dt_Search = Connection.GetDataTable(cmd_Search);
    R_Search = dt_Search.NewRow();
    R_Search["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R_Search["sTenKhoa"] = "-- Nhóm phụ --";
    dt_Search.Rows.InsertAt(R_Search, 0);
    SelectOptionList slNhomPhu_Search = new SelectOptionList(dt_Search, "iID_MaDanhMuc", "sTenKhoa");
    cmd_Search.Dispose();

    cmd_Search = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'ChiTietVatTu') ORDER BY sTenKhoa");
    dt_Search = Connection.GetDataTable(cmd_Search);
    R_Search = dt_Search.NewRow();
    R_Search["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R_Search["sTenKhoa"] = "-- Chi tiết vật tư --";
    dt_Search.Rows.InsertAt(R_Search, 0);
    SelectOptionList slChiTietVatTu_Search = new SelectOptionList(dt_Search, "iID_MaDanhMuc", "sTenKhoa");
    cmd_Search.Dispose();

    cmd_Search = new SqlCommand("SELECT iID_MaDanhMuc, sTenKhoa + '_' + sTen AS sTenKhoa FROM DC_DanhMuc " +
                        "WHERE bHoatDong = 1 AND iID_MaLoaiDanhMuc = (SELECT iID_MaLoaiDanhMuc " +
                                                                    "FROM DC_LoaiDanhMuc " +
                                                                    "WHERE sTenBang = 'XuatXu') ORDER BY sTenKhoa");
    dt_Search = Connection.GetDataTable(cmd_Search);
    R_Search = dt_Search.NewRow();
    R_Search["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R_Search["sTenKhoa"] = "-- Xuất xứ --";
    dt_Search.Rows.InsertAt(R_Search, 0);
    SelectOptionList slXuatXu_Search = new SelectOptionList(dt_Search, "iID_MaDanhMuc", "sTenKhoa");
    cmd_Search.Dispose();
%> 
<br />
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
        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="table_form2">
            <tr>
                <td valign="top" align="left" style="width: 45%;">
                    <div id="nhapform">
                        <div id="form2">
                        <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                            <tr>
                                <td width="100%">
                                    <%
                                        using (Html.BeginForm("Search_Search", "DuyetVatTu", new { ParentID = ParentID, Searchid = 1, iID_MaVatTu = iID_MaVatTu }))
                                    {       
                                    %>
                                    <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                                        <tr>
                                            <td class="td_form2_td1" style="width: 20%;"><div><b>Mã vật tư</b></div></td>
                                            <td class="td_form2_td5" style="width: 80%;">
                                                <div><%=MyHtmlHelper.TextBox(ParentID, sMaVatTu_Search, "sMaVatTu_Search", "", "style=\"width:92%\"")%>
                                                    <%=MyHtmlHelper.CheckBox(ParentID, cbsMaVatTu_Search, "cbsMaVatTu_Search", "", "")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1"><div><b>Tên vật tư</b></div></td>
                                            <td class="td_form2_td5">
                                                <div><%=MyHtmlHelper.TextBox(ParentID, sTen_Search, "sTen_Search", "", "style=\"width:92%\"")%>
                                                    <%=MyHtmlHelper.CheckBox(ParentID, cbsTen_Search, "cbsTen_Search", "", "")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1"><div><b>Tên gốc</b></div></td>
                                            <td class="td_form2_td5">
                                                <div><%=MyHtmlHelper.TextBox(ParentID, sTenGoc_Search, "sTenGoc_Search", "", "style=\"width:92%\"")%>
                                                    <%=MyHtmlHelper.CheckBox(ParentID, cbsTenGoc_Search, "cbsTenGoc_Search", "", "")%>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_form2_td1"><div><b>Quy cách</b></div></td>
                                            <td class="td_form2_td5">
                                                <div><%=MyHtmlHelper.TextArea(ParentID, sQuyCach_Search, "sQuyCach_Search", "", "style=\"width:92%;font:12px/20px Tahoma;height:50px;\"")%>
                                                    <%=MyHtmlHelper.CheckBox(ParentID, cbsQuyCach_Search, "cbsQuyCach_Search", "", "")%>
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
                <td valign="top" align="left" style="width: 45%;">
                    <div id="nhapform">
                        <div id="form2">
                            <%
                                using (Html.BeginForm("Search_Search", "DuyetVatTu", new { ParentID = ParentID, Searchid = 2, iID_MaVatTu = iID_MaVatTu }))
                            {       
                            %>
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1"><div><b>Nhóm loại vật tư</b></div></td>
                                    <td class="td_form2_td5">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, slNhomLoaiVatTu_Search, iDM_MaNhomLoaiVatTu_Search, "iDM_MaNhomLoaiVatTu_Search", "", "onchange=\"ChonNhomLoaiVatTu_Search(this.value)\" style=\"width: 100%;\"")%></div>
                                        <script type="text/javascript">
                                            function ChonNhomLoaiVatTu_Search(iDM_MaNhomLoaiVatTu_Search) {
                                                var url = '<%= Url.Action("get_dtNhomChinh?ParentID=#0&iDM_MaNhomLoaiVatTu=#1&iDM_MaNhomChinh=#2", "DuyeVatTuAjax") %>';
                                                url = url.replace("#0", "<%= ParentID %>");
                                                url = url.replace("#1", iDM_MaNhomLoaiVatTu_Search);
                                                url = url.replace("#2", "<%= iDM_MaNhomChinh_Search %>");
                                                $.getJSON(url, function (data) {
                                                    document.getElementById("<%= ParentID %>_tdNhomChinh_Search").innerHTML = data.ddlNhomChinh;
                                                    ChonNhomChinh_Search(data.iDM_MaNhomChinh);
                                                });
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Nhóm chính</b></div></td>
                                    <td class="td_form2_td5">
                                         <div id="<%= ParentID %>_tdNhomChinh_Search">
                                            <% DuyeVatTuAjaxController.NhomChinh _NhomChinh = DuyeVatTuAjaxController.get_objNhomChinh(ParentID, iDM_MaNhomLoaiVatTu_Search, iDM_MaNhomChinh_Search);%>
                                            <%=_NhomChinh.ddlNhomChinh %>
                                        </div>
                                        <script type="text/javascript">
                                            function ChonNhomChinh_Search(iDM_MaNhomChinh_Search) {
                                                var url = '<%= Url.Action("get_dtNhomPhu?ParentID=#0&iDM_MaNhomChinh=#1&iDM_MaNhomPhu=#2", "DuyeVatTuAjax") %>';
                                                url = url.replace("#0", "<%= ParentID %>");
                                                url = url.replace("#1", iDM_MaNhomChinh_Search);
                                                url = url.replace("#2", "<%= iDM_MaNhomPhu_Search %>");
                                                $.getJSON(url, function (data) {
                                                    document.getElementById("<%= ParentID %>_tdNhomPhu_Search").innerHTML = data.ddlNhomPhu;
                                                    ChonNhomPhu_Search(data.iDM_MaNhomPhu);
                                                });
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Nhóm phụ</b></div></td>
                                    <td class="td_form2_td5">
                                        <div id="<%= ParentID %>_tdNhomPhu_Search">
                                            <% DuyeVatTuAjaxController.NhomPhu _NhomPhu = DuyeVatTuAjaxController.get_objNhomPhu(ParentID, iDM_MaNhomChinh_Search, iDM_MaNhomPhu_Search);%>
                                            <%=_NhomPhu.ddlNhomPhu %>
                                        </div>
                                        <script type="text/javascript">
                                            function ChonNhomPhu_Search(iDM_MaNhomPhu_Search) {
                                                var url = '<%= Url.Action("get_dtChiTietVatTu?ParentID=#0&iDM_MaNhomPhu=#1&iDM_MaChiTietVatTu=#2", "DuyeVatTuAjax") %>';
                                                url = url.replace("#0", "<%= ParentID %>");
                                                url = url.replace("#1", iDM_MaNhomPhu_Search);
                                                url = url.replace("#2", "<%= iDM_MaChiTietVatTu_Search %>");
                                                $.getJSON(url, function (data) {
                                                    document.getElementById("<%= ParentID %>_tdChiTietVatTu_Search").innerHTML = data;
                                                });
                                            }
                                        </script>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Chi tiết vật tư</b></div></td>
                                    <td class="td_form2_td5">
                                        <div id="<%= ParentID %>_tdChiTietVatTu_Search"> 
                                            <%= DuyeVatTuAjaxController.get_objChiTietVatTu(ParentID, iDM_MaNhomPhu_Search, iDM_MaChiTietVatTu_Search)%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Tình trạng vật tư</b></div></td>
                                    <td class="td_form2_td5">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, slXuatXu_Search, iDM_MaXuatXu_Search, "iDM_MaXuatXu_Search", "", "style=\"width: 100%;\"")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Trạng thái</b></div></td>
                                    <td class="td_form2_td5">
                                        <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai_Search, iTrangThai_Search, "iTrangThai_Search", "", "style=\"width: 100%;\"")%></div>
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
                <td valign="top" align="left" style="width: 10%;">&nbsp;</td>
            </tr>
        </table>
    </div>
</div>
<% using (Ajax.BeginForm("Index", "Ajax", new { PartialView = "~/Views/SanPham/DuyetVatTu/View.aspx", OnLoad = ParentID + "_OnLoad" }, new AjaxOptions { }))
{}%>
    <script type="text/javascript">
    function <%=ParentID%>_OnLoad(v) {
        document.getElementById("<%=ParentID%>_div").innerHTML = v;
    } 
</script>
<div id="<%=ParentID%>_div">
    <%{%>
    <%Html.RenderPartial("~/Views/SanPham/DuyetVatTu/View.aspx"); %>
    <%} %>
</div>
</asp:Content>
