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
    R = dt.NewRow();
    R["iID_MaDanhMuc"] = "dddddddd-dddd-dddd-dddd-dddddddddddd";
    R["sTen"] = "-- Đơn vị tính --";
    dt.Rows.InsertAt(R, 0);
    SelectOptionList slDonViTinh = new SelectOptionList(dt, "iID_MaDanhMuc", "sTen");
    cmd.Dispose();

    String dNgayPhatSinhMa = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
    String disabled = "";
    cmd = new SqlCommand("SELECT iID_MaDonVi FROM QT_NhomNguoiDung " +
                           "WHERE iID_MaNhomNguoiDung = (SELECT iID_MaNhomNguoiDung " +
                                                        "FROM QT_NguoiDung " +
                                                        "WHERE sID_MaNguoiDung = @sID_MaNguoiDung)");
    cmd.Parameters.AddWithValue("@sID_MaNguoiDung", sID_MaNguoiDung);
    String iID_MaDonViDangNhap = Connection.GetValueString(cmd, "");
    cmd.Dispose();
    if (iID_MaDonViDangNhap != "-1") disabled = "disabled =\"true\"";

    cmd = new SqlCommand("SELECT iID_DMTrangThai, sTen FROM DM_TrangThai ORDER BY iSTT");
    dt = Connection.GetDataTable(cmd);
    SelectOptionList slTrangThai = new SelectOptionList(dt, "iID_DMTrangThai", "sTen");
    String iTrangThai = "2";
    cmd.Dispose();

    String sMaVatTu = Convert.ToString(ViewData["sMaVatTu"]);
    String sTen = "";
    sTen = Convert.ToString(ViewData["sTen"]);
    String sTenGoc = "";
    String sGhiChu = "";
    String sQuyCach = "";
    String sMoTa = "";
    String sMoTaGoc = "";
    String rSoLuongTonKho = "";
    String dNgayCapNhatTonKho = "";
    String sFileDinhKem = "";
    String iDM_MaNhomLoaiVatTu = Convert.ToString(ViewData["MaNhomLoaiVatTu"]);
    String iDM_MaNhomChinh = Convert.ToString(ViewData["MaNhomChinh"]);
    String iDM_MaNhomPhu = Convert.ToString(ViewData["MaNhomPhu"]);
    String iDM_MaChiTietVatTu = Convert.ToString(ViewData["MaChiTietVatTu"]);
    String iDM_MaXuatXu = Convert.ToString(ViewData["MaXuatXu"]);
    String iDM_MaDonViTinh = Convert.ToString(ViewData["iDM_MaDonViTinh"]);
    String sLyDo = "";
    String iID_MaDonVi = "";
    String TenDonVi = "";
    String sMaDonViLayMa = "";
    String sMaCu = "";
    String sNhaSanXuat = "";
    String urlfilename = "";

    Double rGia = 0, rGia_NS = 0;
    String dTuNgay = "", dDenNgay = "";
    
    if (!String.IsNullOrEmpty(iID_MaVatTu))
    {
        cmd = new SqlCommand("SELECT * FROM DM_VatTu WHERE iID_MaVatTu = @iID_MaVatTu");
        cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
        dt = Connection.GetDataTable(cmd);
        cmd.Dispose();
        if (dt.Rows.Count > 0)
        {
            sMaVatTu = Convert.ToString(dt.Rows[0]["sMaVatTu"]);
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
            urlfilename = Url.Action("GetFile", "DungChung", new { Path = dt.Rows[0]["sFileDinhKem"].ToString() });
            
            iID_MaDonVi = Convert.ToString(dt.Rows[0]["iID_MaDonVi"]);            
            if (iID_MaDonVi == "")
            {
                iID_MaDonVi = null;
                TenDonVi = "BQP";
            }
        }

        cmd = new SqlCommand("SELECT TOP 1 * FROM DM_VatTu_Gia WHERE iID_MaVatTu = @iID_MaVatTu ORDER BY dTuNgay, dNgayTao DESC");
        cmd.Parameters.AddWithValue("@iID_MaVatTu", iID_MaVatTu);
        DataTable dtGia = Connection.GetDataTable(cmd);
        cmd.Dispose();
        if (dtGia.Rows.Count > 0)
        {
            rGia = Convert.ToDouble(dtGia.Rows[0]["rGia"]);
            rGia_NS = Convert.ToDouble(dtGia.Rows[0]["rGia_NS"]);
            dTuNgay = CommonFunction.LayXauNgay(Convert.ToDateTime(dtGia.Rows[0]["dTuNgay"]));
            dDenNgay = CommonFunction.LayXauNgay(Convert.ToDateTime(dtGia.Rows[0]["dDenNgay"]));
        }
        else
        {
            rGia = 0;
            dTuNgay = CommonFunction.LayXauNgay(DateTime.Now);
            dDenNgay = CommonFunction.LayXauNgay(DateTime.Now);
        }
        dtGia.Dispose();
    }
    else{
        iID_MaDonVi = iID_MaDonViDangNhap;
        rGia = 0;
        dTuNgay = CommonFunction.LayXauNgay(DateTime.Now);
        dDenNgay = CommonFunction.LayXauNgay(DateTime.Now);
    }
    
    if (iID_MaDonVi == "-1")
    {
        iID_MaDonVi = null;
        TenDonVi = "BQP";
        sMaDonViLayMa = "VIC";
    }
    else if(iID_MaDonVi != null)
    {

        cmd = new SqlCommand("SELECT sTen,iID_MaDonVi FROM NS_DonVi ORDER BY iSTT WHERE iID_MaDonVi = " + iID_MaDonVi);
        DataTable dtDonViLayTaoMa = Connection.GetDataTable(cmd);
        TenDonVi = Convert.ToString(dtDonViLayTaoMa.Rows[0]["sTen"]);
        sMaDonViLayMa = Convert.ToString(dtDonViLayTaoMa.Rows[0]["iID_MaDonVi"]);
        cmd.Dispose();
        dtDonViLayTaoMa.Dispose();
    }
    
    DateTime dDate = DateTime.Now;
    dDate.ToString();

    String strMaYeuCau = "";
    cmd = new SqlCommand("select top 1 sMaYeuCau from DM_VatTu where convert(varchar(10),dNgayPhatSinhMa,111) =  '" + dDate.ToString("yyyy/MM/dd") + "' order by dNgayPhatSinhMa desc");
    strMaYeuCau =  Convert.ToString( Connection.GetValueString(cmd, ""));
    cmd.Dispose();
    
    String numHT = "00";
    int num = 0;
    if (strMaYeuCau != "" && strMaYeuCau.IndexOf("IMPORTS") == -1)
    {
        String[] arr = strMaYeuCau.Split('-');
        num = Convert.ToInt32(arr[3]) + 1;
        if (num.ToString().Length == 1)
        {
            numHT = "0" + num;
        }
        else
        {
            numHT = Convert.ToString(num);
        }
    }
    
    String sMaYeuCau = "";
    sMaYeuCau = "YC-" + sMaDonViLayMa + "-" + dDate.ToString("yy") + dDate.ToString("MM") + dDate.ToString("dd") + "-" + numHT;
    if (!String.IsNullOrEmpty(iID_MaVatTu))
    {
        if (dt.Rows.Count > 0)
        {
            sMaYeuCau = Convert.ToString(dt.Rows[0]["sMaYeuCau"]);
        }
    }
    
    using (Html.BeginForm("EditSubmit", "MaVatTu", new { ParentID = ParentID, iID_MaVatTu = iID_MaVatTu, iTrangThai = iTrangThai }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>    
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
        $('#pHeader').click(function () {
            $('#dvContent').slideToggle('slow');
        });
    });
    $(document).ready(function () {
        $('#iID_MaVatTu').click(function () {
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
            <div style="width: 100%; float: left;">
                <span><%=NgonNgu.LayXau("Nhập thông tin chi tiết cho mã mới")%></span>
            </div>
        </div>
        <div id="dvArrow" class="ArrowExpand"></div>
    </div>
    <div id="dvContent" class="Content" style="display:none">
        <div id="nhapform">
            <div id="form2">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top" align="left" style="width: 45%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1"><div><b>Nhóm loại vật tư</b></div></td>
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
                                    <td class="td_form2_td1"><div><b>Nhóm chính</b></div></td>
                                    <td class="td_form2_td5">
                                            <div id="<%= ParentID %>_tdNhomChinh">
                                            <% DungChungController.NhomChinh _NhomChinh = DungChungController.get_objNhomChinh(ParentID, iDM_MaNhomLoaiVatTu, iDM_MaNhomChinh);%>
                                            <%=_NhomChinh.ddlNhomChinh %>
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
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left" style="width: 45%;">
                            <table cellpadding="5" cellspacing="5" width="100%">
                                <tr>
                                    <td class="td_form2_td1"><div><b>Chi tiết vật tư</b></div></td>
                                    <td class="td_form2_td5">
                                        <div>
                                        <%
                                        String sTenChiTietVatTu = Convert.ToString(ViewData["sTenChiTietVatTu"]); 
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
                                                ChonMa();
                                                return false;
                                            }                                   
                                        </script>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1"><div><b>Tình trạng vật tư</b></div></td>
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
                            <td class="td_form2_td1"><div><b>Mã vật tư</b></div></td>
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
                                <div><%=MyHtmlHelper.DropDownList(ParentID, slDonViTinh, iDM_MaDonViTinh, "iDM_MaDonViTinh", "", "class=\"input1_2\"")%><br />
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
                                <%=MyHtmlHelper.TextBox(ParentID, sQuyCach, "sQuyCach", "", "class=\"input1_2\"")%>
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
                                <div><%=MyHtmlHelper.DropDownList(ParentID, slTrangThai, iTrangThai, "iTrangThai", "", "class=\"input1_2\" " + disabled)%></div>
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
                    </table>
                    <div style="display: none;">  
                        <%=MyHtmlHelper.TextArea(ParentID, sGhiChu, "sGhiChu", "", "style=\"width:99%;font:12px/20px Tahoma;height:80px;\"")%>   
                        <%=MyHtmlHelper.TextBox(ParentID, rSoLuongTonKho, "rSoLuongTonKho", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%>             
                        <%=MyHtmlHelper.TextBox(ParentID, dNgayCapNhatTonKho, "dNgayCapNhatTonKho", "", "class=\"input1_2\" readonly=\"readonly\" style=\"background:#ebebeb;border:1px solid #7f9db9;\"")%>
                        <%=MyHtmlHelper.TextArea(ParentID, sLyDo, "sLyDo", "", "style=\"width:98%;font:12px/20px Tahoma;height:50px;\"")%>
                    </div>
                </td>
                <td valign="top" align="left" style="width: 10%;">&nbsp;</td>
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
                	<span>Nhập thông tin giá của mã yêu cầu: <%=sMaYeuCau %></span>
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
                                <td class="td_form2_td1"><div><b>Giá vật tư</b></div></td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.TextBox(ParentID, rGia, "rGia", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_rGia")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="td_form2_td1"><div><b>Giá vật tư NSQP</b></div></td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.TextBox(ParentID, rGia_NS, "rGia_NS", "", "class=\"input1_2\"")%><br />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="left" width="45%">
                        <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                            <tr>
                                <td class="td_form2_td1"><div><b>Từ ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DatePicker(ParentID, dTuNgay, "dTuNgay", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dTuNgay")%>
                                    </div>
                                </td>
                            </tr>
                            <tr style="display:none">
                                <td class="td_form2_td1"><div><b>Đến ngày</b></div></td>
                                <td class="td_form2_td5">
                                    <div><%=MyHtmlHelper.DatePicker(ParentID, dDenNgay, "dDenNgay", "", "class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_dDenNgay")%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<script type="text/javascript">
     //ChonMa();
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
                 document.getElementById("Luu").disabled = true;
             }
             else {
                 document.getElementById("errsMaVatTu").innerHTML = "";
                 document.getElementById("Luu").disabled = false;
             }
         });
     } 
</script>
<%if ((iID_MaDonVi == iID_MaDonViDangNhap) || (iID_MaDonViDangNhap == "-1"))
  { %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td>
            	        <input type="submit" id="Luu" class="button4" value="Lưu" />
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <input type="button" class="button4" value="Quay lại" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
<%
    }
    }
%>
</asp:Content>
