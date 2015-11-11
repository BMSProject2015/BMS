<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/KeToanTongHop/jsKeToan.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <%
        int i;
        String MaND = User.Identity.Name;
        String KhongNhap="khong-nhap=\"0\"";
        if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND)) KhongNhap = "khong-nhap=\"1\"";
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        int iNam = DateTime.Now.Year;
        int iThang = DateTime.Now.Month;
        if (dtCauHinh.Rows.Count > 0)
        {
            iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
            iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
        }
        dtCauHinh.Dispose();
        String sThang = Request.QueryString["iThang"];
        if (String.IsNullOrEmpty(sThang) == false) iThang = Convert.ToInt16(sThang);
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        String iID_MaChungTuChiTiet = Request.QueryString["iID_MaChungTuChiTiet"];

        string strNoiDung = KeToanTongHop_ChungTuModels.getSoChungTuGhiSo(iID_MaChungTu);
        DataTable dtThang = DanhMucModels.DT_Thang_CoThangKhong();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        DataTable dtNam = DanhMucModels.DT_Nam(false);
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        ///
        DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeKeToanTongHop, MaND);
        dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
        dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
        dtTrangThai.Rows[0]["sTen"] = "-- Tất cả --";
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        String urlCheckTrungSoGhiSo = Url.Action("CheckTrungSoGhiSo", "KeToanTongHop_ChungTu");
        String urlget_CheckSoChungTuGhiSo= Url.Action("get_CheckSoChungTuGhiSo", "KeToanTongHop_ChungTu");
        String urlget_get_Check_iID_MaChungTu = Url.Action("get_Check_iID_MaChungTu", "KeToanTongHop_ChungTu");
        dtTrangThai.Dispose();

      
    %>
  
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td align="left" style="width: 80px;">
                <div style="padding-left: 22px; padding-bottom: 5px; text-transform: uppercase; color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Phím tắt: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-bottom: 5px; color: Gray; text-transform: uppercase; font-weight: bold; ">
                   <%-- <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>--%>
                   <span >
                  
                            F2: Thêm - Delete: Xóa - F10: Lưu - Backspace: Sửa, F12: Tổng</span>
                </div>
            </td>
            <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Danh sách CTGS:</span>
                    </td>
                    <td align="right" style="padding-right: 8px; padding-left:10px;">
                        Tìm số GS
                    </td>
                    <td style="padding-right: 8px;">
                        <input id="txtSoChungTu" class="textbox_uploadbox" onkeypress='jsKeToan_Search_onkeypress(event)' />
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList("CauHinh", slTrangThai, "-1", "iID_MaTrangThaiDuyet", "", " onchange=\"ChonGiaTri()\" style=\"width:250px;\"")%>
                    </td>
                    <td>
                        Tháng
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList("CauHinh", slThang, Convert.ToString(iThang), "iThangLamViec", "", "onchange=\"ChonThangNam(this.value, 1)\" style=\"width:50px;\"")%>
                    </td>
                    <td>
                        Năm
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList("CauHinh", slNam, Convert.ToString(iNam), "iNamLamViec", "", "onchange=\"ChonThangNam(this.value, 2)\" style=\"width:80px;\"")%>
                    </td>
                    <td align="right" style="padding-right: 20px;">
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <div style="float: right; margin-right: 3px;">
                                <a href='<%=Url.Action("Index","ConvertToFox")%>'
                                    class="button_title">Chuyển dữ liệu</a>
                            </div>
                        
                       <%-- <button onclick="fullScreenApi.requestFullScreen(document.documentElement)" >&nbsp;Rộng toàn màn hình&nbsp;</button>--%>
                   <%--     Tháng
                            <%=iThang %>--%>
                            <%--(F2: Thêm mới - Delete: Xóa - F10: Lưu)</span>--%>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 100%; float: left; margin-right: 15px;">
            <iframe id="ifrChungTu" width="100%" height="260px" src="<%= Url.Action("Blank", "Home")%>">
            </iframe>
        </div>
    </div>
    <div style="width: 100%; float: left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td style="text-align: center; padding-bottom: 5px;">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="padding-right: 10px; text-align:center;">
                   <table border="0" cellpadding="0" cellspacing="0"  width="100%">
                   <tr>
                  <td></td>
                   <td align="right" width="103px">  <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(600, 'Tạo số ghi sổ từ số ghi sổ khác');">
                                <%= Ajax.ActionLink("1.Tạo CTGS", "Index", "NhapNhanh", new { id = "KTTH_TAOCTGS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang, iNam = iNam }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_TAO_CTGS" })%>
                            </div></td>
                  <td align="right" width="95px">  <div style="float: right; margin-right: 3px;" onclick="OnInit_CT_NEW(750, 'Thông tri');">
                                <%= Ajax.ActionLink("2.Thông tri", "Index", "NhapNhanh", new { id = "KTTH_THONGTRI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang, iNam = iNam }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_THONGTRI" })%>
                            </div></td>
                                    <td align="right" width="103px">
                                    
                                     <div style="float: right; margin-right: 3px;" onclick="OnInit_CT_NEW(1000, 'In chi tiết chứng từ ghi sổ');">
                                <%= Ajax.ActionLink("3.C.tiết CTGS", "Index", "NhapNhanh", new { id = "KTTH_THCTGS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iNam = 0 }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_THCTGS" })%>
                            </div>
                                    </td>

                                    <td align="right" width="100px">  <div style="float: right; margin-right: 3px;" onclick="OnInit_CT_NEW(1000, 'Tổng hợp chứng từ ghi sổ');">
                                <%= Ajax.ActionLink("4.T.hợp CTGS", "Index", "NhapNhanh", new { id = "KTTH_THCTGS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iNam = 1 }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_KTTH_THCTGS" })%>
                            </div> </td>
                                      <td align="right" width="92px"> <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(850, 'Nhận dữ liệu Rút dự toán');">
                                <%= Ajax.ActionLink("5.Nhận RDT", "Index", "NhapNhanh", new { id = "KTTH_NHANRUTDUTOAN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_NHANRUTDUTOAN" })%>
                            </div> </td>
                                        <td align="right" width="92px">    <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(800, 'Nhận dữ liệu Ủy nhiệm chi');">
                                <%= Ajax.ActionLink("6.Nhận UNC", "Index", "NhapNhanh", new { id = "KTTH_NHANUYNHIEMCHI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_NHANUYNHIEMCHI" })%>
                            </div></td>
                                          <td align="right" width="103px">  <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(800, 'Nhận dữ liệu Phiếu thu/Phiếu chi');">
                                <%= Ajax.ActionLink("7.Nhận PT/PC", "Index", "NhapNhanh", new { id = "KTTH_NHANPHIEUTHUCHI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_NHANPHIEUTHUCHI" })%>
                            </div></td>

                                            <td align="right" width="92px">   <div style="float: right; margin-right: 3px;" onclick="OnInit_CT_NEW(1000, 'Lịch sử Chứng từ ghi sổ');">
                                <%= Ajax.ActionLink("8.L.sử CTGS", "Index", "NhapNhanh", new { id = "KTTH_LICHSUCHUNGTU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_LICHSUCHUNGTU" })%>
                            </div> </td>
                                          <td align="right" width="98px">  <div style="float: right; margin-right: 3px;">
                                <a href='<%=Url.Action("ChungTuChiTietTuChoi","KeToanTongHop_ChungTuChiTiet")%>'
                                    class="button_title">9.CT TỪ CHỐI</a>
                            </div></td>
                                                    <td align="right" width="115px">   <div style="float: right; margin-right: 3px;">
                                <a href='<%=Url.Action("Index","rptKeToanTongHop_KiemTraChungTu")%>' class="button_title">
                                    10.K.tra s.liệu</a>.

                                       <%-- <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(800, 'Nhận dữ liệu Công sản');">
                                <%= Ajax.ActionLink("8.Nhận c.sản", "Index", "NhapNhanh", new { id = "KTTH_NHANCONGSAN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_NHANCONGSAN" })%>
                            </div>
                            <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(800, 'Nhận dữ liệu Quyết toán');">
                                <%= Ajax.ActionLink("7.Nhận Q.Toán", "Index", "NhapNhanh", new { id = "KTTH_NHANQUYETTOAN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_NHANQUYETTOAN" })%>
                            </div>--%>
                            </div></td>
                  <td></td>
                   </tr>
                   </table>
                          
                           
                         
                         
                           
                         
                           
                          
                           
                          
                            
                        </td>
                    </tr>
                                       <tr>
                        <td style="padding-top: 10px; padding-right: 10px;">
                            <div style="float: left; margin-left: 5px; width: 70%;">
                                <%=MyHtmlHelper.TextBox("CauHinh", strNoiDung, "iID_MaChungTu", null, "class=\"input1_2\" style=\"border:none;text-align: left;background-color:#dff0fb;font-weight:bold;color: #3b5998;\"")%>
                            </div>
                            <%
                                if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND) == false)
                                {
                            %>
                            <div style="float: right; margin-right: 3px;">
                                <div style="float: left; margin-right: 5px; font-weight: bold; color: #3b5998;; padding-top:3px;">
                                    Tên
                                </div>
                                <div style="float: right;">
                                    <select id="ddlMaND" onchange="ddl_onChange('ddlMaND');" style="width: 120px;">
                                        <option value="">--Tất cả--</option>
                                    </select>
                                </div>
                            </div>
                            <%
                                }
                            %>
                            <div style="float: right; margin-right: 5px;">
                                <div style="float: left; margin-right: 5px; font-weight: bold; color: #3b5998; padding-top:3px;">
                                 Loại
                                </div>
                                <div style="float: right;">
                                    <select id="ddlTrangThai" onchange="ddl_onChange('ddlTrangThai');" style="width: 120px;">
                                        <option value="">--Tất cả--</option>
                                        <option value="1">Đã duyệt</option>
                                        <option value="0">Chưa duyệt</option>
                                    </select>
                                </div>
                            </div>
                        </td>
                    </tr>                  
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <iframe id="ifrChiTietChungTu" width="100%" height="600px" src="<%= Url.Action("Blank", "Home")%>">
                    </iframe>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function ChonGiaTri() {
            jsKeToan_LoadLaiChungTu();
        }


        function GetChungTuGhiSo(iID_MaChungTu) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("getSoChungTuGhiSo?iID_MaChungTu=#0", "KeToanTongHop_ChungTu")%>');
            url = unescape(url.replace("#0", iID_MaChungTu));
            $.getJSON(url, function (data) {
                document.getElementById("CauHinh_iID_MaChungTu").value = data;

            });
        }
        function ChonThangNam(value, loai) {
            var iThangLamViec;
            var iNamLamViec;
            if (loai == 1) {
                iThangLamViec = value;
                iNamLamViec = document.getElementById('CauHinh_iNamLamViec').value;
                jsNamLamViec = iNamLamViec;
            }
            else {
                iNamLamViec = value;
                jsNamLamViec = value;
                iThangLamViec = document.getElementById('CauHinh_iThangLamViec').value;
            }

            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("UpdateCauHinhNamLamViec?MaND=#0&iThangLamViec=#1&iNamLamViec=#2","KeToanTongHop") %>');
            url = unescape(url.replace("#0", '<%=MaND %>'));
            url = unescape(url.replace("#1", iThangLamViec));
            url = unescape(url.replace("#2", iNamLamViec));

            $.getJSON(url, function (data) {
                jsKeToan_LoadLaiChungTu();
            });

        }

        function location_reload() {
            location.reload();
        }

        function CallSuccess_CT() {
            Bang_ShowCloseDialog();
            location_reload();
            return false;
        }

        function OnInit_CT(value, title) {
            $("#idDialog").dialog("destroy");
            document.getElementById("idDialog").title = title;
            document.getElementById("idDialog").innerHTML = "";
            $("#idDialog").dialog({
                resizeable: false,
                draggable: true,
                width: value,
                modal: true,
                open: function (event, ui) {
                    $(event.target).parent().css('position', 'fixed');
                    $(event.target).parent().css('top', '50px');
                    // $(event.target).parent().css('left', '10px');
                }
            });
        }
        function OnInit_CT_NEW(value, title) {
            $("#idDialog").dialog("destroy");
            document.getElementById("idDialog").title = title;
            document.getElementById("idDialog").innerHTML = "";
            $("#idDialog").dialog({
                    resizeable: false,
                    draggable: true,
                    width: value,
                    modal: true,
                    open: function(event, ui) {
                        $(event.target).parent().css('position', 'fixed');
                        $(event.target).parent().css('top', '10px');
                        // $(event.target).parent().css('left', '10px');
                    }

                });
        }
        function OnLoad_CT(v) {
            document.getElementById("idDialog").innerHTML = v;
        }

        $(document).ready(function () {
            jsNamLamViec = '<%=iNam %>';
            BangDuLieu_Url_CheckTrungSoGhiSo = '<%=urlCheckTrungSoGhiSo %>';
            jsKeToan_url_ChungTu_Check = '<%=urlget_CheckSoChungTuGhiSo %>';
            jsKeToan_url_MaChungTu_Check = '<%=urlget_get_Check_iID_MaChungTu %>';
            jsKeToan_url_ChungTu = '<%= Url.Action("ChungTu_Frame", "KeToanTongHop_ChungTuChiTiet")%>';
            jsKeToan_url_ChungTuChiTiet = '<%= Url.Action("ChungTuChiTiet_Frame", "KeToanTongHop_ChungTuChiTiet")%>';
            jsKeToan_url_NhanCongSan = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_NHANCONGSAN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT" })%>';
            jsKeToan_url_NhanQuyetToan = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_NHANQUYETTOAN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';
            jsKeToan_url_NhanPhieuThuChi = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_NHANPHIEUTHUCHI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT" })%>';
            jsKeToan_url_NhanUyNhiemChi = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_NHANUYNHIEMCHI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT" })%>';
            jsKeToan_url_NhanRutDuToan = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_NHANRUTDUTOAN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';
            jsKeToan_url_KTTH_THCTGS = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_THCTGS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iNam = 1 })%>';
            jsKeToan_url_KTTH_CTGS = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_THCTGS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iNam = 0 })%>';
            jsKeToan_url_KTTH_ThongTri = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_THONGTRI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';

            jsKeToan_url_KTTH_LICHSUCHUNGTU = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_LICHSUCHUNGTU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';
            jsKeToan_iID_MaChungTuChiTiet = '<%=iID_MaChungTuChiTiet %>';
            jsKeToan_url_Tao_CTGS = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_TAOCTGS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT" })%>';
            jsKeToan_url_Tao_CTGS_Dialog = '<%= Url.Action("Show_Dialog_Tao_CTGS", "KeToanTongHop_ChungTuChiTiet")%>';
            jsKeToan_url_TinhTong_Dialog = '<%= Url.Action("Show_Dialog_F12", "KeToanTongHop_ChungTuChiTiet")%>';
            jsKeToan_LoadLaiChungTu('<%= iID_MaChungTu%>', '<%=iThang %>');
        });
    </script>
   <div id="idDialog" style="display: none;">
    </div>
    <div id="dvText" class="popup_block">
        <img src="../../../Content/ajax-loader.gif" /><br />
        <p>
            Hệ thống đang thực hiện yêu cầu...</p>
    </div>
    <div id="divTongHop" style="display: none;">
        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="mGrid">
            <tr>
                <th colspan="6">
                    &nbsp;
                </th>
            </tr>
            <tr>
                <td class="td_label" style="width: 23%">
                    <div>
                      <b>Số C.T.G.S<span style="color: Red;">*</span></b>  </div>
                </td>
                <td style="width: 70px">
                <div>
                     <script type="text/javascript">

                         function CheckMaTrung(idSoChungTu) {
                             jQuery.ajaxSetup({ cache: false });
                             if (idSoChungTu != null && idSoChungTu != '') {
                                 var sSoChungTu_SoSanh = document.getElementById("divTongHop_sSoChungTu_SoSanh").value;
                                 var url = unescape('<%= Url.Action("get_SoChungTuDuyet?idSoChungTu=#0&iNamLamViec=#1&idSoChungTuCu=#2", "KeToanTongHop_ChungTuChiTiet") %>');
                                 url = unescape(url.replace("#0", idSoChungTu));
                                 url = unescape(url.replace("#1", <%=iNam %>));
                                 url = unescape(url.replace("#2", sSoChungTu_SoSanh));
                                 $.getJSON(url, function (data) {
                                     document.getElementById("pMess_divTongHop_sSoChungTu").innerHTML = data;
                                 });
                             }
                             else {
                                 document.getElementById("pMess_divTongHop_sSoChungTu").innerHTML = '';
                             }
                         }
                                                        </script>
                </div>
                  
                    <div>
                    <input id="divTongHop_sSoChungTu" class="textbox_uploadbox" tab-index="0"  style="background-color:#f8e6d1;" onblur="CheckMaTrung(this.value);"/>
                        <%--<input id="divTongHop_sSoChungTu" khong-nhap="1" class="textbox_uploadbox" tab-index="0"  style="background-color:#f8e6d1;"/>--%>

                        <input id="divTongHop_sSoChungTu_SoSanh" class="textbox_uploadbox"   style="background-color:#f8e6d1; display: none;" />
                        </div>
                </td>
           
            
                <td  style="width: 50px" class="td_label">
                    <div>
                      <b>  Ngày<span style="color: Red;">*</span> </b>  </div>
                </td>
                <td style="width: 70px">
                    <div>
                        <input id="divTongHop_iNgay" class="textbox_uploadbox" maxlength="2" tab-index="1" /></div>
                </td>
            
                <td class="td_label">
                    <div>
                      <b>  Tập</b> </div>
                </td>
                <td>
                    <div>
                        <input id="divTongHop_iTapSo" <%=KhongNhap %> class="textbox_uploadbox" maxlength="2"  tab-index="2" /></div>
                </td>
            </tr>
            <tr>
                <td class="td_label">
                    <div>
                       <b> Đơn vị</b> </div>
                </td>
                <td colspan="5">
                    <div>
                        <input id="divTongHop_sDonVi" class="textbox_uploadbox" maxlength="12" tab-index="3"/></div>
                </td>
            </tr>
            <tr>
                <td class="td_label">
                    <div>
                        <b>  Nội dung C.T.G.S </b> </div>
                </td>
                <td colspan="5">
                    <div>                    
                        <input id="divTongHop_sNoiDung" class="textbox_uploadbox" maxlength="100" tab-index="4"/>
                    </div>
                </td>
             </tr>
            
             
        </table>
        <div style="height:20px;">&nbsp;<span id="pMess_divTongHop_sSoChungTu" style="color: Red;"></span></div>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <div>
                        <b>  &nbsp; </b> </div>
                </td>
                <td colspan="5" align="center">
                    <div>                    
                    <%--class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" --%>          
                        <input type="button" class="ui-button ui-widget button4"  id="submit" value="Tiếp tục"  tab-index="5" onclick="submit_click();"/>
                        <input type="button" class="ui-button ui-widget button4"  id="cancel" value="Hủy bỏ"  tab-index="6" onclick="cancel_click();"/>
                    </div>
                </td>
            </tr>            
        </table>
        <script type="text/javascript">

            function submit_click() {
                document.getElementById("pMess_divTongHop_sSoChungTu").innerHTML = '';
                $('#btn_submit').click();
            }
            function cancel_click() {
                document.getElementById("pMess_divTongHop_sSoChungTu").innerHTML = '';
                $('#btn_cancel').click();
                
            }
         
            
        </script>
    </div>
    <div id="divTinhTong" style="display: none;">   
     <div style="margin: 5px 0px 0px 0px; text-align:center; color:Navy;font-size:13px;">
        <b>
             <span id="pMessNoiDung"></span></b>
    </div>
     <div style="margin: 5px 0px 10px 0px; font-size:13px;">
        <b>
             <span id="pMessChungTu"></span></b>
    </div>
    <div style="height: 5px;">
        &nbsp;</div>
    <div style="width: 99%;">
   <span id="pMessData" style="font-size:12px;"></span> </div>
    </div>

  <script type="text/javascript">
  // setTimeout("fullScreenApi.requestFullScreen(document.documentElement)", 2000);
      (function () {
          var 
        fullScreenApi = {
            supportsFullScreen: false,
            nonNativeSupportsFullScreen: false,
            isFullScreen: function () { return false; },
            requestFullScreen: function () { },
            cancelFullScreen: function () { },
            fullScreenEventName: '',
            prefix: ''
        },
        browserPrefixes = 'webkit moz o ms khtml'.split(' ');

          // check for native support
          if (typeof document.cancelFullScreen != 'undefined') {
              fullScreenApi.supportsFullScreen = true;
          } else {
              // check for fullscreen support by vendor prefix
              for (var i = 0, il = browserPrefixes.length; i < il; i++) {
                  fullScreenApi.prefix = browserPrefixes[i];

                  if (typeof document[fullScreenApi.prefix + 'CancelFullScreen'] != 'undefined') {
                      fullScreenApi.supportsFullScreen = true;

                      break;
                  }
              }
          }

          // update methods to do something useful
          if (fullScreenApi.supportsFullScreen) {
              fullScreenApi.fullScreenEventName = fullScreenApi.prefix + 'fullscreenchange';

              fullScreenApi.isFullScreen = function () {
                  switch (this.prefix) {
                      case '':
                          return document.fullScreen;
                      case 'webkit':
                          return document.webkitIsFullScreen;
                      default:
                          return document[this.prefix + 'FullScreen'];
                  }
              }
              fullScreenApi.requestFullScreen = function (el) {
                  return (this.prefix === '') ? el.requestFullScreen() : el[this.prefix + 'RequestFullScreen']();
              }
              fullScreenApi.cancelFullScreen = function (el) {
                  return (this.prefix === '') ? document.cancelFullScreen() : document[this.prefix + 'CancelFullScreen']();
              }
          }
          else if (typeof window.ActiveXObject !== "undefined") { // Older IE.
              fullScreenApi.nonNativeSupportsFullScreen = true;
              fullScreenApi.requestFullScreen = fullScreenApi.requestFullScreen = function (el) {
                  var wscript = new ActiveXObject("WScript.Shell");
                  if (wscript !== null) {
                      wscript.SendKeys("{F11}");
                  }
              }
              fullScreenApi.isFullScreen = function () {
                  return document.body.clientHeight == screen.height && document.body.clientWidth == screen.width;
              }
          }

          // jQuery plugin
          if (typeof jQuery != 'undefined') {
              jQuery.fn.requestFullScreen = function () {

                  return this.each(function () {
                      if (fullScreenApi.supportsFullScreen) {
                          fullScreenApi.requestFullScreen(this);
                      }
                  });
              };
          }

          // export api
          window.fullScreenApi = fullScreenApi;
      })();</script>
</asp:Content>
