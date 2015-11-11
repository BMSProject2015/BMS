<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/KeToanChiTietTienGui/jsBang_KeToanChiTiet_TienGui_ChungTuCapThu_Duyet.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <%
        String ParentID = "KTTG_ChungTuCapThu_Duyet";
        String iID_MaCapPhat = Convert.ToString(ViewData["iID_MaCapPhat"]);
        String NamLamViec = Request.QueryString["NamLamViec"];
        String MaDonVi = Request.QueryString["MaDonVi"];
        String TuNgay = Request.QueryString["dTuNgay"];
        String DenNgay = Request.QueryString["dDenNgay"];
        String sNguoiDung = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(sNguoiDung);
        String iID_MaDonVi_No = Request.QueryString["MaDonVi"];
        String sSoChungTu = "UNC/DT";
        String sNoiDung = "Cấp KP đợt ngày " + CapThuModels.getNgayCapPhat(iID_MaCapPhat);
        if (NamLamViec == null || NamLamViec == "")
        {
            NamLamViec = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
        }
        if (MaDonVi == null) MaDonVi = "";
        if (TuNgay == null) TuNgay = "";
        if (DenNgay == null) DenNgay = "";
        //don vi
        DataTable dtDonVi = DonViModels.Get_DanhSachDonVi();
        if (dtDonVi != null || dtDonVi.Rows.Count == 0)
        {
            DataRow R = dtDonVi.NewRow();
            R["iID_MaDonVi"] = "";
            R["sTen"] = "--- Chọn đơn vị ---";
            dtDonVi.Rows.InsertAt(R, 0);
        }
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        dtDonVi.Dispose();
        //don vi cap thu
        DataTable dtDonViCapThu = DonViModels.Get_DanhSachDonVi_CapThu(iID_MaCapPhat);
        if (dtDonViCapThu != null || dtDonViCapThu.Rows.Count == 0)
        {
            DataRow R = dtDonViCapThu.NewRow();
            R["iID_MaDonVi"] = "";
            R["sTen"] = "--- Chọn đơn vị ---";
            dtDonViCapThu.Rows.InsertAt(R, 0);
        }
        SelectOptionList slDonViCapThu = new SelectOptionList(dtDonViCapThu, "iID_MaDonVi", "sTen");
        dtDonViCapThu.Dispose();
        //tai khoan
        DataTable dtTaiKhoan = TaiKhoanModels.DT_DSTaiKhoan(true, "--- Tài khoản kế toán ---", "");
        SelectOptionList stTaiKhoan = new SelectOptionList(dtTaiKhoan, "iID_MaTaiKhoan", "sTen");
        dtTaiKhoan.Dispose();

        String strThemMoi = Url.Action("Edit", "KTCT_DuyetCapThu");

        String strOnblur = "";
        strOnblur = "onblur=\"Gan_sSoUyNhiemChi(this);CheckMaTrung(this.value);\"";
    %>
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
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>1. Tạo chứng từ cấp thu mới</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                    <tr>
                        <td style="width: 100%">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="table_form2">
                                <tr>
                                    <td valign="top" style="width: 50%">
                                        <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                                            <tr>
                                                <td class="td_form2_td1" style="width: 15%;">
                                                    <div>
                                                        <%=NgonNgu.LayXau("Đơn vị")%></div>
                                                </td>
                                                <td class="td_form2_td5">
                                                    <div>
                                                        <%=MyHtmlHelper.DropDownList(ParentID, slDonViCapThu, MaDonVi, "iID_MaDonVi", "", "onChange=\"ddlDonVi_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi")%>
                                                        <script type="text/javascript">
                                                        function ddlDonVi_SelectedValueChanged(ctl) {
                                                         var url = "<%= Url.Action("Edit", "KTCT_DuyetCapThu", new {iID_MaChungTuCapPhat=iID_MaCapPhat})%>";
                                                         if(ctl.selectedIndex>=0)
                                                            {
                                                               var value = ctl.options[ctl.selectedIndex].value;
                                                                if(value!="")
                                                                {
                                                                    url += "&MaDonVi=" + value + "&dTuNgay=<%=TuNgay %>&dDenNgay=<%=DenNgay %>";
                                                                }
                                                            }
                                                            location.href = url;
                                                        }
                                                        </script>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1">
                                                    <div>
                                                        <%=NgonNgu.LayXau("Từ ngày")%></div>
                                                </td>
                                                <td class="td_form2_td5">
                                                    <div>
                                                        <script type="text/javascript">
                                                        function dTuNgay_SelectedValueChanged() {
                                                            var url = "<%=Url.Action("Edit", "KTCT_DuyetCapThu", new {iID_MaChungTuCapPhat=iID_MaCapPhat})%>";
                                                            var value = document.getElementById("KTTG_ChungTuCapThu_Duyet_vidTuNgay").value;
                                                            if(value!="")
                                                            {
                                                                url += "&MaDonVi=<%=MaDonVi %>&dTuNgay=" + value + "&dDenNgay=<%=DenNgay %>";
                                                            }
                                                            location.href = url;
                                                        }
                                                        </script>
                                                        <%=MyHtmlHelper.DatePicker(ParentID, TuNgay, "dTuNgay", "", "onchange=\"dTuNgay_SelectedValueChanged()\" class=\"input1_2\" style=\"height:18px;\"")%><br />
                                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dTuNgay")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1">
                                                    <div>
                                                        <%=NgonNgu.LayXau("Đến ngày")%></div>
                                                </td>
                                                <td class="td_form2_td5">
                                                    <div>
                                                        <script type="text/javascript">
                                                        function dDenNgay_SelectedValueChanged(ctl) {
                                                            var url = "<%=Url.Action("Edit", "KTCT_DuyetCapThu",new {iID_MaChungTuCapPhat=iID_MaCapPhat})%>";
                                                            var value = document.getElementById("KTTG_ChungTuCapThu_Duyet_vidDenNgay").value;
                                                            if(value!="")
                                                            {
                                                                url += "&MaDonVi=<%=MaDonVi %>&dTuNgay=<%=TuNgay %>&dDenNgay=" + value + "";
                                                            }
                                                            location.href = url;
                                                        }
                                                        </script>
                                                        <%=MyHtmlHelper.DatePicker(ParentID, DenNgay, "dDenNgay", "", "onchange=\"dDenNgay_SelectedValueChanged(this)\" class=\"input1_2\" style=\"height:18px;\"")%><br />
                                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dDenNgay")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1" style="width: 15%;">
                                                    <div>
                                                        <%=NgonNgu.LayXau("Số UNC/RDT")%>
                                                        <span style="color: Red;">(*)</span></div>
                                                </td>
                                                <td class="td_form2_td5">
                                                    <div>
                                                        <script type="text/javascript">
                                                            function CheckMaTrung(idSoChungTu) {
                                                                jQuery.ajaxSetup({ cache: false });
                                                                if (idSoChungTu != null && idSoChungTu != '') {
                                                                    var url = unescape('<%= Url.Action("get_SoChungTuDuyet?sSoUyNhiemChi=#0", "KTCT_DuyetCapThu") %>');
                                                                    url = unescape(url.replace("#0", idSoChungTu));
                                                                    $.getJSON(url, function (data) {
                                                                        document.getElementById("pMess").innerHTML = data;
                                                                    });
                                                                }
                                                                else {
                                                                    document.getElementById("pMess").innerHTML = '';
                                                                }
                                                            }
                                                        </script>
                                                        <script type="text/javascript">

                                                            function Gan_sSoUyNhiemChi(ctl) {
                                                                var iID_sSoChungTu = document.getElementById("iID_sSoChungTu");
                                                                if (ctl.value == "") {
                                                                    iID_sSoChungTu.value = '<%=sSoChungTu %>';
                                                                }
                                                                else {
                                                                    iID_sSoChungTu.value = ctl.value;
                                                                }
                                                                return false;
                                                            }
                                                        </script>
                                                        <%=MyHtmlHelper.TextBox(ParentID, sSoChungTu, "sSoChungTu", "", "" + strOnblur + " class=\"input1_2\" style=\"height:18px;\"")%><br />
                                                        <span id="pMess" style="color: Red;"></span>
                                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sSoChungTu")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1">
                                                    <div>
                                                        <%=NgonNgu.LayXau("Ngày chứng từ")%></div>
                                                </td>
                                                <td class="td_form2_td5">
                                                    <div>
                                                        <script type="text/javascript">
                                                            function Gan_dNgayChungTu(ctl) {
                                                                var iID_dNgayChungTu = document.getElementById("iID_dNgayChungTu");
                                                                iID_dNgayChungTu.value = ctl.value;
                                                                return false;
                                                            }
                                                        </script>
                                                        <%=MyHtmlHelper.DatePicker(ParentID, "", "dNgayChungTu", "", "onchange=\"Gan_dNgayChungTu(this);\" class=\"input1_2\" style=\"height:18px;\"")%><br />
                                                        <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayChungTu")%>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top" style="width: 50%">
                                        <table cellpadding="0" cellspacing="0" width="100%" class="table_form2">
                                            <tr>
                                                <td class="td_form2_td1" style="width: 15%;">
                                                    <div>
                                                        <%=NgonNgu.LayXau("Tài khoản có")%></div>
                                                </td>
                                                <td class="td_form2_td5">
                                                    <div>
                                                        <script type="text/javascript">
                                                            function Gan_idTaiKhoanCo(ctl) {
                                                                var idTaiKhoanCo = document.getElementById("idTaiKhoanCo");
                                                                idTaiKhoanCo.value = ctl.value;
                                                                return false;
                                                            }
                                                        </script>
                                                        <%=MyHtmlHelper.DropDownList(ParentID, stTaiKhoan, "", "iID_MaTaiKhoan_Co", "", "onchange=\"Gan_idTaiKhoanCo(this)\"  class=\"input1_2\"")%><br />
                                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTaiKhoan_Co")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1">
                                                    <div>
                                                        <%=NgonNgu.LayXau("Tài khoản nợ")%></div>
                                                </td>
                                                <td class="td_form2_td5">
                                                    <div>
                                                        <script type="text/javascript">
                                                            function Gan_idTaiKhoanNo(ctl) {
                                                                var idTaiKhoanNo = document.getElementById("idTaiKhoanNo");
                                                                idTaiKhoanNo.value = ctl.value;
                                                                return false;
                                                            }
                                                        </script>
                                                        <%=MyHtmlHelper.DropDownList(ParentID, stTaiKhoan, "", "iID_MaTaiKhoan_No", "", "onchange=\"Gan_idTaiKhoanNo(this)\" class=\"input1_2\"")%><br />
                                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTaiKhoan_No")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1">
                                                    <div>
                                                        <%=NgonNgu.LayXau("Đơn vị có")%></div>
                                                </td>
                                                <td class="td_form2_td5">
                                                    <div>
                                                        <script type="text/javascript">
                                                            function Gan_idDonViCo(ctl) {
                                                                var idDonViCo = document.getElementById("idDonViCo");
                                                                idDonViCo.value = ctl.value;
                                                                return false;
                                                            }
                                                        </script>
                                                        <%=MyHtmlHelper.DropDownList(ParentID, slDonVi, "", "iID_MaDonVi_Co", "", "onchange=\"Gan_idDonViCo(this)\" class=\"input1_2\"")%><br />
                                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi_Co")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1">
                                                    <div>
                                                        <%=NgonNgu.LayXau("Đơn vị nợ")%></div>
                                                </td>
                                                <td class="td_form2_td5">
                                                    <div>
                                                        <script type="text/javascript">
                                                            function Gan_idDonViNo(ctl) {
                                                                var idDonViNo = document.getElementById("idDonViNo");
                                                                if (ctl.value == "") {
                                                                    idDonViNo.value = '<%=iID_MaDonVi_No %>';
                                                                }
                                                                else {
                                                                    idDonViNo.value = ctl.value;
                                                                }

                                                                return false;
                                                            }
                                                        </script>
                                                        <%=MyHtmlHelper.DropDownList(ParentID, slDonViCapThu, iID_MaDonVi_No, "iID_MaDonVi_No", "", "onchange=\"Gan_idDonViNo(this)\" class=\"input1_2\"")%><br />
                                                        <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaDonVi_No")%>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="td_form2_td1">
                                                    <div>
                                                        <%=NgonNgu.LayXau("Nội dung chứng từ")%>
                                                        <span style="color: Red;">(*)</span></div>
                                                </td>
                                                <td class="td_form2_td5" colspan="3">
                                                    <div>
                                                        <script type="text/javascript">
                                                            function Gan_sNoiDungUyNhiemChi(ctl) {
                                                                var iID_sNoiDungChungTu = document.getElementById("iID_sNoiDungChungTu");
                                                                if (ctl.value == "") {
                                                                    iID_sSoChungTu.value = '<%=sNoiDung %>';
                                                                }
                                                                else {
                                                                    iID_sNoiDungChungTu.value = ctl.value;
                                                                }
                                                                return false;
                                                            }
                                                        </script>
                                                        <%=MyHtmlHelper.TextArea(ParentID, sNoiDung, "sNoiDung", "", "onblur=\"Gan_sNoiDungUyNhiemChi(this);\" class=\"input1_2\"  style=\"height:50px;\"")%>
                                                        <%= Html.ValidationMessage(ParentID + "_" + "err_sNoiDung")%>
                                                    </div>
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
    </div>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>2. Danh sách chứng từ duyệt cấp thu</span>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <%Html.RenderPartial("~/Views/KeToanChiTiet/TienGui/CapThuDuyet/CapThuDuyet_Duyet_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>
        </div>
    </div>
    <%dtCauHinh.Dispose();
    %>
    <script type="text/javascript">
        function ChonallDV(value) {
          
            var c = Bang_arrCSMaCot["bDuyet"];
            if (Bang_nH != 0 && c != null) {
                for (var h = 0; h < Bang_nH - 1; h++) {
                    if (value == "true" || value=="1") {
                        Bang_GanGiaTriThatChoO(h, c, "1");
                        var cs = Bang_arrCSMaCot["rSoTienDuyet"];
                        Bang_arrThayDoi[h][cs] = true;
                    }
                    else {
                        Bang_GanGiaTriThatChoO(h, c, "0");
                        var cs = Bang_arrCSMaCot["rSoTienDuyet"];
                        Bang_arrThayDoi[h][cs] = false;
                    }
                }
            }

        }    
    </script>
</asp:Content>
