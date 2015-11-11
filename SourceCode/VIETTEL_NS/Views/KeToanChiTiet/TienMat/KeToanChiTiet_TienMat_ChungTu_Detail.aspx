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
    <script src="<%= Url.Content("~/Scripts/KeToanChiTietTienMat/jsKeToanChiTietTienMat.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <%
        int i;
        String MaND = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        int iNam = DateTime.Now.Year;
        int iThang = DateTime.Now.Month;
        if (dtCauHinh.Rows.Count > 0)
        {
            iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
            iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
        }
        dtCauHinh.Dispose();

        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        String iID_MaChungTuChiTiet = Request.QueryString["iID_MaChungTuChiTiet"];
        //if (String.IsNullOrEmpty(iID_MaChungTu))
        //{
        //    DataTable dtChungTu = KTCT_TienMat_ChungTuModels.List_DanhSachChungTu(iThang, iNam);
        //    if (dtChungTu.Rows.Count > 0)
        //    {
        //        iID_MaChungTu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
        //    }
        //    dtChungTu.Dispose();
        //}
        string strNoiDung = KTCT_TienMat_ChungTuModels.getSoChungTuGhiSo(iID_MaChungTu);
        DataTable dtThang = DanhMucModels.DT_Thang_CoThangKhong();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        DataTable dtNam = DanhMucModels.DT_Nam(false);
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        ///
        DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeKeToanChiTiet, MaND);
        dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
        dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
        dtTrangThai.Rows[0]["sTen"] = "-- Tất cả --";
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();
        String urlCheckTrungSoGhiSo = Url.Action("CheckTrungSoGhiSo", "KTCT_TienMat_ChungTu");
        String urlget_get_Check_iID_MaChungTu = Url.Action("get_Check_iID_MaChungTu", "KTCT_TienMat_ChungTu");
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
                        <span>Danh sách chứng từ </span>
                    </td>
                    <td align="right" style="padding-right: 8px;">
                        Số C.T tìm kiếm
                    </td>
                    <td style="padding-right: 8px;">
                        <input id="txtSoChungTu" class="textbox_uploadbox" onkeypress='jsKeToan_Search_onkeypress(event)' />
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList("CauHinh", slTrangThai, "-1", "iID_MaTrangThaiDuyet", "", "class=\"input1_2\" onchange=\"ChonGiaTri()\"")%>
                    </td>
                    <td>
                        Tháng
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList("CauHinh", slThang, Convert.ToString(iThang), "iThangLamViec", "", "onchange=\"ChonThangNam(this.value, 1)\"")%>
                    </td>
                    <td>
                        Năm
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList("CauHinh", slNam, Convert.ToString(iNam), "iNamLamViec", "", "onchange=\"ChonThangNam(this.value, 2)\"")%>
                    </td>
                    <td align="right" style="padding-right: 20px;">
                        <span>Tháng
                            <%=iThang %>
                            (F2: Thêm mới - Delete: Xóa - F10: Lưu)</span>
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
                        <td style="text-align: center; padding-bottom: 10px;">
                            <%=MyHtmlHelper.TextBox("CauHinh", strNoiDung, "iID_MaChungTu", null, "class=\"input1_2\" style=\"border:none;text-align:center;background-color:#dff0fb;font-weight:bold;color: #3b5998;\"")%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="padding-right: 10px;">
                            <%
                                if (LuongCongViecModel.KiemTra_TroLyPhongBan(MaND) == false)
                                {
                            %>
                             <div style="float: right; margin-right: 5px;">
                                <div style="float: left; margin-right: 5px; font-weight: bold; color: #3b5998;; padding-top:3px;">
                                    Tên
                                </div>
                                <div style="float: right;">
                                    <select id="ddlMaND" onchange="ddl_onChange('ddlMaND');" style="width: 200px;">
                                        <option value="">Tất cả</option>
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
                                    <select id="ddlTrangThai" onchange="ddl_onChange('ddlTrangThai');" style="width: 150px;">
                                        <option value="">Tất cả</option>
                                        <option value="1">Đã duyệt</option>
                                        <option value="0">Chưa duyệt</option>
                                    </select>
                                </div>
                            </div>
                            <div style="float: right; margin-right: 3px;">
                                <a href='<%=Url.Action("ChungTuChiTietTuChoi","KTCT_TienMat_ChungTuChiTiet")%>' class="button_title">
                                    8. C.T TỪ CHỐI</a>
                            </div>
                            <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(1000, 'Lịch sử Chứng từ ghi sổ');">
                                <%= Ajax.ActionLink("7.Lịch sử CTGS", "Index", "NhapNhanh", new { id = "KTTH_TIENMAT_LICHSUCHUNGTU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_LICHSUCHUNGTU" })%>
                            </div>
                             <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(700, 'KIỂM TRA SỐ LIỆU');">
                                <%= Ajax.ActionLink("6.Kiểm tra số liệu", "Index", "NhapNhanh", new { id = "KTTM_KIEMTRASOLIEU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTM_KIEMTRASOLIEU" })%>
                            </div>
                            <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(400, 'IN THÔNG TRI');">
                                <%= Ajax.ActionLink("5.Thông tri", "Index", "NhapNhanh", new { id = "KTTH_TIENMAT_THONGTRI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_THONGTRI" })%>
                            </div>
                            <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(350, 'Tổng hợp ngày');">
                                <%= Ajax.ActionLink("4.T.hợp ngày", "Index", "NhapNhanh", new { id = "KTTH_TIENMAT_TONGHOP_NGAY", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT" }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_TIENMAT_TONGHOP_NGAY" })%>
                            </div>
                              <div style="float: right; margin-right: 5px;" onclick="OnInit_CT(800, 'TỔNG HỢP CTGS');">
                                    <%= Ajax.ActionLink("3. T.hợp CTGS", "Index", "NhapNhanh", new { id = "KT_TIENMAT_TONGHOP_CTGS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iNam = 0, iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button_title", id = "idKT_TIENMAT_TONGHOP_CTGS" })%>
                                </div>
                            <div style="float: right; margin-right: 5px;" onclick="OnInit_CT(550, 'In phiếu chi');">
                                <%= Ajax.ActionLink("2.in phiếu chi", "Index", "NhapNhanh", new { id = "KTCT_PHIEUCHI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iNam = 0 }, new AjaxOptions { }, new { @class = "button_title", id = "idKTCT_PHIEUCHI" })%>
                            </div>
                            <div style="float: right; margin-right: 5px;" onclick="OnInit_CT(550, 'In phiếu thu');">
                                <%= Ajax.ActionLink("1.In phiếu thu", "Index", "NhapNhanh", new { id = "KTTH_PHIEUTHU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTCT_PHIEUTHU" })%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <iframe id="ifrChiTietChungTu" width="100%" height="500px" src="<%= Url.Action("Blank", "Home")%>">
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
            var url = unescape('<%= Url.Action("getSoChungTuGhiSo?iID_MaChungTu=#0", "KTCT_TienMat_ChungTu")%>');
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
            }
            else {
                iNamLamViec = value;
                iThangLamViec = document.getElementById('CauHinh_iThangLamViec').value;
            }
            jsNamLamViec = iNamLamViec;
            jsThangLamViec = iThangLamViec;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("UpdateCauHinhNamLamViec?MaND=#0&iThangLamViec=#1&iNamLamViec=#2","KeToanChiTietTienMat") %>');
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
                draggable: false,
                width: value,
                modal: true,
                position: ['top', 20]
            });
        }

        function OnLoad_CT(v) {
            document.getElementById("idDialog").innerHTML = v;
        }

        $(document).ready(function () {
            jsNamLamViec = <%=iNam %> ;
            jsThangLamViec = <%=iThang %> ;
            BangDuLieu_Url_CheckTrungSoGhiSo = '<%=urlCheckTrungSoGhiSo %>';
             jsKeToan_url_MaChungTu_Check = '<%=urlget_get_Check_iID_MaChungTu %>';
            jsKeToan_url_ChungTu_Check = '<%= Url.Action("get_CheckSoChungTuGhiSo", "KTCT_TienMat_ChungTu")%>';
            jsKeToan_url_ChungTu = '<%= Url.Action("ChungTu_Frame", "KTCT_TienMat_ChungTuChiTiet")%>';
            jsKeToan_url_ChungTuChiTiet = '<%= Url.Action("ChungTuChiTiet_Frame", "KTCT_TienMat_ChungTuChiTiet")%>';
            jsKeToan_url_KTCT_PHIEUTHU = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTCT_PHIEUTHU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT" })%>';
            jsKeToan_url_KTCT_PHIEUCHI = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTCT_PHIEUCHI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';
             jsKeToan_url_KT_TIENMAT_TONGHOP_CTGS = '<%= Url.Action("Index", "NhapNhanh", new { id = "KT_TIENMAT_TONGHOP_CTGS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';
            jsKeToan_url_KTTH_LICHSUCHUNGTU = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_TIENMAT_LICHSUCHUNGTU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';

            jsKeToan_url_KTTH_ThongTri = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_TIENMAT_THONGTRI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';
            jsKeToan_url_KTTH_TIENMAT_TONGHOP_NGAY = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_TIENMAT_TONGHOP_NGAY", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';

            jsKeToan_iID_MaChungTuChiTiet = '<%=iID_MaChungTuChiTiet %>';
            jsKeToan_LoadLaiChungTu('<%= iID_MaChungTu%>');
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
                <th colspan="2">
                    &nbsp;
                </th>
            </tr>
            <tr>
                <td class="td_label" style="width: 20%">
                    <div>
                        Số chứng từ <span style="color: Red;">*</span></div>
                </td>
                <td>
                    <div>
                        <input id="divTongHop_sSoChungTu" class="textbox_uploadbox" tab-index="0"/></div>
                </td>
            </tr>
            <tr>
                <td class="td_label" style="width: 20%">
                    <div>
                        Ngày chứng từ <span style="color: Red;">*</span></div>
                </td>
                <td>
                    <div>
                        <input id="divTongHop_iNgay" class="textbox_uploadbox" tab-index="1"/></div>
                </td>
            </tr>
            <tr>
                <td class="td_label">
                    <div>
                        Nội dung</div>
                </td>
                <td>
                    <div>
                        <input id="divTongHop_sNoiDung" class="textbox_uploadbox" tab-index="2"/></div>
                </td>
            </tr>
        </table>
         <div style="height:20px;">&nbsp;</div>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <div>
                        <b>  &nbsp; </b> </div>
                </td>
                <td align="center">
                    <div>                    
                    <%--class="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" --%>          
                        <input type="button" class="ui-button ui-widget"  id="submit" value="Tiếp tục"  tab-index="3" onclick="submit_click();"/>
                        <input type="button" class="ui-button ui-widget"  id="cancel" value="Hủy bỏ"  tab-index="4" onclick="cancel_click();"/>
                    </div>
                </td>
            </tr>            
        </table>
        <script type="text/javascript">

            function submit_click() {
                $('#btn_submit').click();
            }
            function cancel_click() {
                $('#btn_cancel').click();
            }

        </script>
    </div>
</asp:Content>
