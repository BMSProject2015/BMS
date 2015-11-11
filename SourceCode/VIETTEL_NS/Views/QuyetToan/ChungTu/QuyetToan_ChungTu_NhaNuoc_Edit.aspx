<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers.QuyetToan" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    String ParentID = "Edit";
    String UserID = User.Identity.Name;
    String Loai = Convert.ToString(Request.QueryString["Loai"]);
    if (String.IsNullOrEmpty(Loai)) Loai = Convert.ToString(ViewData["Loai"]);
    String MaChungTu = Convert.ToString(ViewData["MaChungTu"]);
    String MaPhongBanNguoiDung = "";

    DataTable dtChungTu = QuyetToan_ChungTuModels.GetChungTu(MaChungTu);
    DataRow R;
    int iSoChungTu = 0, bLoaiThang_Quy = -1;
    String sLNS = "", dNgayChungTu = "", sNoiDung = "", sLyDo = "", iID_MaTrangThaiDuyet = "",sThangQuy="", sThang = "", sQuy = "", sNam = "", iID_MaDonVi = "", sTienToChungTu = "";
    String[] arrsLNS = null;
    if (dtChungTu.Rows.Count > 0)
    {
        R = dtChungTu.Rows[0];
        dNgayChungTu = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayChungTu"]));
        sNoiDung = Convert.ToString(R["sNoiDung"]);
        iSoChungTu = Convert.ToInt32(R["iSoChungTu"]);
        sLyDo = Convert.ToString(R["sLyDo"]);
        iID_MaTrangThaiDuyet = Convert.ToString(R["iID_MaTrangThaiDuyet"]);
        bLoaiThang_Quy = Convert.ToInt32(R["bLoaiThang_Quy"]);
        MaPhongBanNguoiDung = Convert.ToString(R["iID_MaPhongBan"]);
        sThangQuy = Convert.ToString(R["iThang_Quy"]);
        switch (bLoaiThang_Quy)
        {
            case 0:
                sThang = Convert.ToString(R["iThang_Quy"]);
                break;
            case 1:
                sQuy = Convert.ToString(R["iThang_Quy"]);
                break;
            case 2:
                sNam = Convert.ToString(R["iThang_Quy"]);
                break;
        }
        iID_MaDonVi = Convert.ToString(R["iID_MaDonVi"]);
        sTienToChungTu = Convert.ToString(R["sTienToChungTu"]);

        sLNS = Convert.ToString(R["sDSLNS"]);
        if (sLNS != "")
        {
            if (sLNS.IndexOf(',') != -1)
            {
                arrsLNS = sLNS.Split(',');
            }
        }
    }
    else
    {
        dNgayChungTu = CommonFunction.LayXauNgay(DateTime.Now);
    }
    if (ViewData["DuLieuMoi"] == "1")
    {
        sTienToChungTu = PhanHeModels.LayTienToChungTu(QuyetToanModels.iID_MaPhanHeQuyetToan);
        iSoChungTu = QuyetToan_ChungTuModels.GetMaxChungTu() + 1;
    }
    dtChungTu.Dispose();

    DataTable dtPhongBan = DanhMucModels.NS_PhongBan();
    SelectOptionList slPhongBan = new SelectOptionList(dtPhongBan, "iID_MaPhongBan", "sTen");
    DataRow R1 = dtPhongBan.NewRow();
    R1["iID_MaPhongBan"] = Guid.Empty;
    R1["sTen"] = "--- Danh sách phòng ban ---";
    dtPhongBan.Rows.InsertAt(R1, 0);
    dtPhongBan.Dispose();
        
    DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(UserID);
    if (String.IsNullOrEmpty(iID_MaDonVi))
    {
        if (dtDonVi.Rows.Count > 0)
            iID_MaDonVi = Convert.ToString(dtDonVi.Rows[0]["iID_MaDonVi"]);
        else
            iID_MaDonVi = Guid.Empty.ToString();
    }
    SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
    dtDonVi.Dispose();

    DataTable dtThang = DanhMucModels.DT_Thang();
    SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
    dtThang.Dispose();

    DataTable dtQuy = DanhMucModels.DT_Quy_QuyetToan();
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    dtQuy.Dispose();

    if (String.IsNullOrEmpty(MaPhongBanNguoiDung)) MaPhongBanNguoiDung = NganSach_HamChungModels.MaPhongBanCuaMaND(UserID);
    using (Html.BeginForm("Edit_NhaMuoc_Submit", "QuyetToan_ChungTu", new { ParentID = ParentID, MaChungTu = MaChungTu, Loai = Loai, sLNS = sLNS }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "QuyetToan_ChungTu", new { Loai = Loai}), "Chứng từ quyết toán")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>
                    <%
                    if (ViewData["DuLieuMoi"] == "1")
                    {
                        %>
                        <%=NgonNgu.LayXau("Thêm mới chứng từ")%>
                        <%
                    }
                    else
                    {
                        %>
                        <%=NgonNgu.LayXau("Sửa thông tin chứng từ")%>
                        <%
                    }
                    %>&nbsp; &nbsp;
                </span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 100%; float: left;">
                <div style="width: 60%; float: left">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="td_form2_td1" style="width: 30%;">
                                <div><b>Số chứng từ</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <b><%=sTienToChungTu%><%=iSoChungTu%></b>
                                </div>
                            </td>
                        </tr>
                        <tr>
                                <td class="td_form2_td1" style="width: 30%;">
                                    <div><b>Phòng ban</b></div>
                                </td>
                                <td class="td_form2_td5">
                                    <div>
                                        <%=MyHtmlHelper.DropDownList(ParentID, slPhongBan, MaPhongBanNguoiDung, "iID_MaPhongBan_Chon", "", "onchange=\"ChonPhongBanNhapLieu(this.value);\" class=\"input1_2\" disabled=\"disabled\" ")%><br />
                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaPhongBan_Chon")%>
                                        <script language="javascript" type="text/javascript">
                                            function ChonPhongBanNhapLieu(jGiaTriPhongBan) {
                                                jQuery.ajaxSetup({ cache: false });
                                                var url = unescape('<%= Url.Action("get_dtMucLucNganSach_NhaNuoc?ParentID=#0&iID_MaPhongBan=#1&sLNS=#2", "QuyetToan_ChungTu")%>');
                                                url = unescape(url.replace("#0", "<%= ParentID %>"));
                                                url = unescape(url.replace("#1", jGiaTriPhongBan));
                                                url = unescape(url.replace("#2", "<%=sLNS %>"));
                                                $.getJSON(url, function (data) {
                                                    document.getElementById("<%= ParentID %>_divMucLucNganSach").innerHTML = data;
                                                });
                                            }    
                                        </script>
                                    </div>
                                </td>
                            </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div><b>Loại ngân sách</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div id="divDonVi" style="width: 100%; height: 400px; max-height:400px; min-height:0px; overflow: scroll;">
                                    <div id="<%= ParentID %>_divMucLucNganSach">
                                        <%=QuyetToan_ChungTuController.get_objMucLucNganSach_NhaNuoc(ParentID, MaPhongBanNguoiDung, sLNS)%>
                                    </div>
                                    
                                    <script language="javascript" type="text/javascript">
                                        function ChonLNSNhapLieu() {
                                            var jsGiaTriLNS = "";
                                            var inputs = document.getElementsByTagName("input");
                                            for (var i = 0; i < inputs.length; i++) {
                                                if (inputs[i].type == "checkbox") {
                                                    if (inputs[i].checked && inputs[i].name != "abc") {
                                                        jsGiaTriLNS = jsGiaTriLNS + inputs[i].value + ",";
                                                    }
                                                }
                                            }

                                            var objDonVi = document.getElementById("<%= ParentID %>_iID_MaDonVi");
                                            var jGiaTriDonVi = '';
                                            if (objDonVi != null) {
                                                jGiaTriDonVi = objDonVi.options[objDonVi.selectedIndex].value;
                                            }

                                            var jLoaiThangQuy = "";
                                            for (var i = 0; i < inputs.length; i++) {
                                                if (inputs[i].type == "radio") {
                                                    if (inputs[i].value == "Thang" && inputs[i].checked) {
                                                        jLoaiThangQuy = "0";
                                                    }
                                                    else if (inputs[i].value == "Quy" && inputs[i].checked) {
                                                        jLoaiThangQuy = "1";
                                                    }
                                                    else {
                                                        jLoaiThangQuy = "0";
                                                    }
                                                }
                                            }
                                            var jThangQuy = "";
                                            if (jLoaiThangQuy == "0") {
                                                var objThang = document.getElementById("<%= ParentID %>_iThang");
                                                if (objThang != null) {
                                                    jThangQuy = objThang.options[objThang.selectedIndex].value;
                                                }
                                            }
                                            else {
                                                var objQuy = document.getElementById("<%= ParentID %>_iQuy");
                                                if (objQuy != null) {
                                                    jThangQuy = objQuy.options[objQuy.selectedIndex].value;
                                                }
                                            }

                                            ThongKeGiaTriQuyetToan(jsGiaTriLNS, jGiaTriDonVi, jThangQuy, jLoaiThangQuy)
                                        }    
                                        </script>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div><b>Đơn vị</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, iID_MaDonVi, "iID_MaDonVi", "", "onchange=\"ChonDonViNhapLieu(this.value);\" class=\"input1_2\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
                                    <script language="javascript" type="text/javascript">
                                        function ChonDonViNhapLieu(jGiaTriDonVi) {
                                            var jsGiaTriLNS = "";
                                            var inputs = document.getElementsByTagName("input");
                                            for (var i = 0; i < inputs.length; i++) {
                                                if (inputs[i].type == "checkbox") {
                                                    if (inputs[i].checked && inputs[i].name != "abc") {
                                                        jsGiaTriLNS = jsGiaTriLNS + inputs[i].value + ",";
                                                    }
                                                }
                                            }

                                            var jLoaiThangQuy = "";
                                            for (var i = 0; i < inputs.length; i++) {
                                                if (inputs[i].type == "radio") {
                                                    if (inputs[i].value == "Thang" && inputs[i].checked) {
                                                        jLoaiThangQuy = "0";
                                                    }
                                                    else if (inputs[i].value == "Quy" && inputs[i].checked) {
                                                        jLoaiThangQuy = "1";
                                                    }
                                                    else {
                                                        jLoaiThangQuy = "0";
                                                    }
                                                }
                                            }
                                            var jThangQuy = "";
                                            if (jLoaiThangQuy == "0") {
                                                var objThang = document.getElementById("<%= ParentID %>_iThang");
                                                if (objThang != null) {
                                                    jThangQuy = objThang.options[objThang.selectedIndex].value;
                                                }
                                            }
                                            else {
                                                var objQuy = document.getElementById("<%= ParentID %>_iQuy");
                                                if (objQuy != null) {
                                                    jThangQuy = objQuy.options[objQuy.selectedIndex].value;
                                                }
                                            }
                                            ThongKeGiaTriQuyetToan(jsGiaTriLNS, jGiaTriDonVi, jThangQuy, jLoaiThangQuy)
                                        }    
                                    </script>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div><b>Tháng/Quý</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div>
                                    <%
                                    String strThang = "Quy";                                
                                    switch (bLoaiThang_Quy) { 
                                        case 0:
                                            strThang = "Thang"; 
                                            break;
                                        case 1:
                                            strThang = "Quy"; 
                                            break;
                                        case 2:
                                            strThang = "Nam"; 
                                            break;
                                    }    
                                    %>
                                    <%=MyHtmlHelper.Option(ParentID, "Thang", strThang, "ThangQuy", "")%>Tháng&nbsp;&nbsp;
                                    <%=MyHtmlHelper.DropDownList(ParentID, slThang, sThang, "iThang", "", "onchange=\"ChonThangNhapLieu(this.value)\" class=\"input1_2\" style=\"width:17%;\"")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <%=MyHtmlHelper.Option(ParentID, "Quy", strThang, "ThangQuy", "")%>Quý&nbsp;&nbsp;
                                    <%=MyHtmlHelper.DropDownList(ParentID, slQuy, sQuy, "iQuy", "", "onchange=\"ChonThangNhapLieu(this.value)\" class=\"input1_2\" style=\"width:17%;\"")%><br />
                                    <%= Html.ValidationMessage(ParentID + "_" + "err_ThangQuy")%>
                                    <script language="javascript" type="text/javascript">
                                        function ChonThangNhapLieu(jGiaTriThang) {
                                            var jsGiaTriLNS = "";
                                            var inputs = document.getElementsByTagName("input");
                                            for (var i = 0; i < inputs.length; i++) {
                                                if (inputs[i].type == "checkbox") {
                                                    if (inputs[i].checked && inputs[i].name != "abc") {
                                                        jsGiaTriLNS = jsGiaTriLNS + inputs[i].value + ",";
                                                    }
                                                }
                                            }

                                            var objDonVi = document.getElementById("<%= ParentID %>_iID_MaDonVi");
                                            var jGiaTriDonVi = '';
                                            if (objDonVi != null) {
                                                jGiaTriDonVi = objDonVi.options[objDonVi.selectedIndex].value;
                                            }

                                            var jLoaiThangQuy = "";
                                            for (var i = 0; i < inputs.length; i++) {
                                                if (inputs[i].type == "radio") {
                                                    if (inputs[i].value == "Thang" && inputs[i].checked) {
                                                        jLoaiThangQuy = "0";
                                                    }
                                                    else if (inputs[i].value == "Quy" && inputs[i].checked) {
                                                        jLoaiThangQuy = "1";
                                                    }
                                                    else {
                                                        jLoaiThangQuy = "0";
                                                    }
                                                }
                                            }
                                            ThongKeGiaTriQuyetToan(jsGiaTriLNS, jGiaTriDonVi, jGiaTriThang, jLoaiThangQuy)
                                        }    
                                    </script>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div><b>Ngày chứng từ</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.DatePicker(ParentID, dNgayChungTu, "dNgayChungTu", "", "class=\"input1_2\"")%><br />
                                     <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu")%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1">
                                <div><b>Nội dung</b></div>
                            </td>
                            <td class="td_form2_td5">
                                <div><%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "class=\"input1_2\" style=\"height: 78px;\"")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td1"></td>
                            <td class="td_form2_td5">
                                <div>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                            <td width="30%" align="right" class="td_form2_td5">
                                                <input type="submit" class="button" id="Submit1" value="Lưu" />
                                            </td>          
                                                <td width="5px">&nbsp;</td>          
                                            <td class="td_form2_td5">
                                                <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="width: 36%; float: right">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td style="height: 465px;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="td_form2_td5">
                                <div>
                                    <fieldset style="width: 100%;">
                                        <legend><b>Tổng số quyết toán tự chi</b></legend>
                                        <table class="tblhost-filter">
                                            <tr>
                                                <td style="width: 30%;">
                                                    Số quyết toán trong kỳ:
                                                </td>
                                                <td style="width: 2%;">:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rSoQuyetToanTrongKy_TuChi">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Số quyết toán đến kỳ:
                                                </td>
                                                <td>:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rSoQuyetToanDenKy_TuChi">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Tổng số quyết toán:
                                                </td>
                                                <td>:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rTongSoQuyetToan_TuChi">
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_form2_td5">
                                <div>
                                    <fieldset style="width: 100%;">
                                        <legend><b>Tổng số quyết toán hiện vật</b></legend>
                                        <table class="tblhost-filter">
                                            <tr>
                                                <td style="width: 30%;">
                                                    Số quyết toán trong kỳ:
                                                </td>
                                                <td style="width: 2%;">:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rSoQuyetToanTrongKy_HienVat">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Số quyết toán đến kỳ:
                                                </td>
                                                <td>:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rSoQuyetToanDenKy_HienVat">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Tổng số quyết toán:
                                                </td>
                                                <td>:</td>
                                                <td style="text-align: left; font-size: 13px; color: Red;" id="rTongSoQuyetToan_HienVat">
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
<%
    }       
%>
<script type="text/javascript">
    ThongKeGiaTriQuyetToan('<%=sLNS%>', '<%=iID_MaDonVi%>', '<%=sThangQuy%>', '<%=bLoaiThang_Quy%>');
                                        function CheckAll(value) {
                                            $("input:checkbox[check-group='MaMucLucNganSach']").each(function (i) {
                                                this.checked = value;
                                            });
                                            var jsGiaTriLNS = "";
                                            var inputs = document.getElementsByTagName("input");
                                            for (var i = 0; i < inputs.length; i++) {
                                                if (inputs[i].type == "checkbox") {
                                                    if (inputs[i].checked && inputs[i].name != "abc") {
                                                        jsGiaTriLNS = jsGiaTriLNS + inputs[i].value + ",";
                                                    }
                                                }
                                            }

                                            var objDonVi = document.getElementById("<%= ParentID %>_iID_MaDonVi");
                                            var jGiaTriDonVi = '';
                                            if (objDonVi != null) {
                                                jGiaTriDonVi = objDonVi.options[objDonVi.selectedIndex].value;
                                            }
                                            var jLoaiThangQuy = "";
                                            for (var i = 0; i < inputs.length; i++) {
                                                if (inputs[i].type == "radio") {
                                                    if (inputs[i].value == "Thang" && inputs[i].checked) {
                                                        jLoaiThangQuy = "0";
                                                    }
                                                    else if (inputs[i].value == "Quy" && inputs[i].checked) {
                                                        jLoaiThangQuy = "1";
                                                    }
                                                    else {
                                                        jLoaiThangQuy = "0";
                                                    }
                                                }
                                            }
                                            var jGiaTriThang = "";
                                            if (jLoaiThangQuy="0")
                                                jGiaTriThang = document.getElementById("<%= ParentID %>_iThang").value;
                                            else
                                                jGiaTriThang = document.getElementById("<%= ParentID %>_iQuy").value;
                                            //alert(jGiaTriThang); 
                                            ThongKeGiaTriQuyetToan(jsGiaTriLNS, jGiaTriDonVi, jGiaTriThang, jLoaiThangQuy)
                                        }                                            
   
    function ThongKeGiaTriQuyetToan(jLNS, jMaDonVi, jThangQuy, jLoaiThangQuy) {
        jQuery.ajaxSetup({ cache: false });
        var url = unescape('<%= Url.Action("get_TongGiaTriQuyetToan?sLNS=#0&iID_MaDonVi=#1&Thang_Quy=#2&LoaiThang_Quy=#3", "QuyetToan_ChungTu") %>');
        url = unescape(url.replace("#0", jLNS));
        url = unescape(url.replace("#1", jMaDonVi));
        url = unescape(url.replace("#2", jThangQuy));
        url = unescape(url.replace("#3", jLoaiThangQuy));
        $.getJSON(url, function (item) {
            document.getElementById("rSoQuyetToanTrongKy_TuChi").innerHTML = item.rSoQuyetToanTrongKy_TuChi;
            document.getElementById("rSoQuyetToanDenKy_TuChi").innerHTML = item.rSoQuyetToanDenKy_TuChi;
            document.getElementById("rTongSoQuyetToan_TuChi").innerHTML = item.rTongSoQuyetToan_TuChi;
            document.getElementById("rSoQuyetToanTrongKy_HienVat").innerHTML = item.rSoQuyetToanTrongKy_HienVat;
            document.getElementById("rSoQuyetToanDenKy_HienVat").innerHTML = item.rSoQuyetToanDenKy_HienVat;
            document.getElementById("rTongSoQuyetToan_HienVat").innerHTML = item.rTongSoQuyetToan_HienVat;
        });
    }                                            
</script>
</asp:Content>
