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
    <script src="<%= Url.Content("~/Scripts/KeToanChiTietKhoBac/jsKeToanChiTietKhoBac.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <%
        int i;
        String MaND = User.Identity.Name;
        DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
        int iNam = DateTime.Now.Year;
        int iThang = DateTime.Now.Month;
        int iNamNganSach = 2;
        int iNguonNganSach = 1;
        if (dtCauHinh.Rows.Count > 0)
        {
            iNam = Convert.ToInt32(dtCauHinh.Rows[0]["iNamLamViec"]);
            iThang = Convert.ToInt32(dtCauHinh.Rows[0]["iThangLamViec"]);
            iNamNganSach = Convert.ToInt32(dtCauHinh.Rows[0]["iID_MaNamNganSach"]);
            iNguonNganSach = Convert.ToInt32(dtCauHinh.Rows[0]["iID_MaNguonNganSach"]);
        }
        dtCauHinh.Dispose();

        int iLoai = Convert.ToInt32(Request.QueryString["iLoai"]);
        String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
        String iID_MaChungTuChiTiet = Request.QueryString["iID_MaChungTuChiTiet"];
        //if (String.IsNullOrEmpty(iID_MaChungTu))
        //{
        //    DataTable dtChungTu = KTCT_KhoBac_ChungTuModels.List_DanhSachChungTu(iThang, iNam, iNamNganSach, iNguonNganSach, iLoai);
        //    if (dtChungTu.Rows.Count > 0)
        //    {
        //        iID_MaChungTu = Convert.ToString(dtChungTu.Rows[0]["iID_MaChungTu"]);
        //    }
        //    dtChungTu.Dispose();
        //}
        string strNoiDung = KTCT_KhoBac_ChungTuModels.getSoChungTuGhiSo(iID_MaChungTu);
        DataTable dtThang = DanhMucModels.DT_Thang_CoThangKhong();
        SelectOptionList slThang = new SelectOptionList(dtThang, "MaThang", "TenThang");
        dtThang.Dispose();
        DataTable dtNam = DanhMucModels.DT_Nam(false);
        SelectOptionList slNam = new SelectOptionList(dtNam, "MaNam", "TenNam");
        dtNam.Dispose();

        DataTable dtNamNganSach = DanhMucModels.NS_NamNganSach();
        SelectOptionList slNamNganSach = new SelectOptionList(dtNamNganSach, "iID_MaNamNganSach", "sTen");
        DataTable dtNguonNganSach = DanhMucModels.NS_NguonNganSach();
        SelectOptionList slNguonNganSach = new SelectOptionList(dtNguonNganSach, "iID_MaNguonNganSach", "sTen");
        dtNamNganSach.Dispose();
        dtNguonNganSach.Dispose();
        ///lay trang thai
        DataTable dtTrangThai = LuongCongViecModel.Get_dtDSTrangThaiDuyet_DuocXem(PhanHeModels.iID_MaPhanHeKeToanChiTiet, MaND);
        dtTrangThai.Rows.InsertAt(dtTrangThai.NewRow(), 0);
        dtTrangThai.Rows[0]["iID_MaTrangThaiDuyet"] = -1;
        dtTrangThai.Rows[0]["sTen"] = "-- Tất cả --";
        SelectOptionList slTrangThai = new SelectOptionList(dtTrangThai, "iID_MaTrangThaiDuyet", "sTen");
        dtTrangThai.Dispose();
        String urlCheckTrungSoGhiSo = Url.Action("CheckTrungSoGhiSo", "KTCT_KhoBac_ChungTu");
        String urlget_get_Check_iID_MaChungTu = Url.Action("get_Check_iID_MaChungTu", "KTCT_KhoBac_ChungTu");
    %>
    <%= Html.Hidden("txtLoai", iLoai)%>
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
                    <span style="font-weight: bold; color: #ec3237;">| (F2:Thêm mới - Delete:Xóa - F10:Lưu)</span>
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
                        <span>D.sách C.T.G.S</span>
                    </td>
                    <td align="right" style="padding-right: 3px;">
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
                    <td>
                        Năm ng.sách
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList("CauHinh", slNamNganSach, Convert.ToString(iNamNganSach), "iID_MaNamNganSach", "", "onchange=\"ChonThangNam(this.value, 3)\"")%>
                    </td>
                    <td>
                        Nguồn ng.sách
                    </td>
                    <td>
                        <%=MyHtmlHelper.DropDownList("CauHinh", slNguonNganSach, Convert.ToString(iNguonNganSach), "iID_MaNguonNganSach", "", "onchange=\"ChonThangNam(this.value, 4)\"")%>
                    </td>
                </tr>
            </table>
        </div>
        <div style="width: 100%; float: left; margin-right: 15px;">
            <iframe id="ifrChungTu" width="100%" height="260px" src="<%= Url.Action("Blank", "Home")%>">
            </iframe>
        </div>
    </div>
    <div style="width: 100%;">
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
                                    <select id="ddlMaND" onchange="ddl_onChange('ddlMaND');" style="width: 150px;">
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
                                <a href='<%=Url.Action("ChungTuChiTietTuChoi","KTCT_TienGui_ChungTuChiTiet")%>' class="button_title">
                                    7. D.sách C.T TỪ CHỐI</a>
                            </div>
                            <div style="float: right; margin-right: 3px;" onclick="OnInit_CT(1000, 'Lịch sử Chứng từ ghi sổ');">
                                    <%= Ajax.ActionLink("6.Lịch sử CTGS", "Index", "NhapNhanh", new { id = "KTTH_KHOBAC_LICHSUCHUNGTU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iThang = iThang }, new AjaxOptions { }, new { @class = "button_title", id = "idKTTH_LICHSUCHUNGTU" })%>
                                </div>
                            
                            <div style="float: right; margin-right: 5px;" onclick="OnInit_CT(450, 'Thông tri loại ngân sách');">
                                <%= Ajax.ActionLink("5.TTri Loại NS", "Index", "NhapNhanh", new { id = "KTCT_TTLOAINS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iNam = 1 }, new AjaxOptions { }, new { @class = "button_title", id = "idKTCT_TTLOAINS" })%>
                            </div>
                            <div style="float: right; margin-right: 5px;">
                                <a class="button_title" id="idKTCT_TTTONGHOP" href="<%= Url.Action("Index", "rptKTKB_ThongTriTongHop", new {iID_MaChungTu = iID_MaChungTu})%>">4.Thông tri tổng hợp</a>
                            </div>
                            <div style="float: right; margin-right: 5px;" onclick="OnInit_CT(200, 'Thông tri loại');">
                                <%= Ajax.ActionLink("3.TTri Loại", "Index", "NhapNhanh", new { id = "KTCT_TTLOAI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iNam = 1 }, new AjaxOptions { }, new { @class = "button_title", id = "idKTCT_TTLOAI" })%>
                            </div>
                            <div style="float: right; margin-right: 5px;" onclick="OnInit_CT(450, 'Thông tri chuyển tiền');">
                                <%= Ajax.ActionLink("2.TTri chuyển tiền", "Index", "NhapNhanh", new { id = "KTCT_TTCHUYENTIEN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu, iNam = 0 }, new AjaxOptions { }, new { @class = "button_title", id = "idKTCT_TTCHUYENTIEN" })%>
                            </div>
                            <div id="divGiayRutTien" style="float: right; margin-right: 5px;" >                      
                                <%= Ajax.ActionLink("1.In giấy rút tiền", "Index", "NhapNhanh", new { id = "KTCT_GIAYRUTTIEN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT" }, new AjaxOptions { }, new { @class = "button_title", id = "idKTCT_GIAYRUTTIEN" })%>
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
        function ThongTriTongHop() {
            var iID_MaChungTu = GoiHam_ChungTu_BangDuLieu_LayMaChungTu();
            var url = unescape('<%= Url.Action("Index?iID_MaChungTu=#0", "rptKTKB_ThongTriTongHop")%>');
            url = unescape(url.replace("#0", iID_MaChungTu));
            location.href = url;
        }
        function GetChungTuGhiSo(iID_MaChungTu) {
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("getSoChungTuGhiSo?iID_MaChungTu=#0", "KTCT_KhoBac_ChungTu")%>');
            url = unescape(url.replace("#0", iID_MaChungTu));
            $.getJSON(url, function (data) {
                document.getElementById("CauHinh_iID_MaChungTu").value = data;

            });
        }
        function ChonThangNam(value, loai) {
            var iThangLamViec;
            var iNamLamViec;
            var iID_MaNamNganSach;
            var iID_MaNguonNganSach;
            if (loai == 1) {
                iThangLamViec = value;
                iNamLamViec = document.getElementById('CauHinh_iNamLamViec').value;
                iID_MaNamNganSach = document.getElementById('CauHinh_iID_MaNamNganSach').value;
                iID_MaNguonNganSach = document.getElementById('CauHinh_iID_MaNguonNganSach').value;
            }
            else if (loai == 2) {
                iNamLamViec = value;
                iThangLamViec = document.getElementById('CauHinh_iThangLamViec').value;
                iID_MaNamNganSach = document.getElementById('CauHinh_iID_MaNamNganSach').value;
                iID_MaNguonNganSach = document.getElementById('CauHinh_iID_MaNguonNganSach').value;
            }
            else if (loai == 3) {
                iNamLamViec = document.getElementById('CauHinh_iNamLamViec').value;
                iThangLamViec = document.getElementById('CauHinh_iThangLamViec').value;
                iID_MaNamNganSach = value;
                iID_MaNguonNganSach = document.getElementById('CauHinh_iID_MaNguonNganSach').value;
            } else {

                iNamLamViec = document.getElementById('CauHinh_iNamLamViec').value;
                iThangLamViec = document.getElementById('CauHinh_iThangLamViec').value;
                iID_MaNamNganSach = document.getElementById('CauHinh_iID_MaNamNganSach').value;
                iID_MaNguonNganSach = value;
            }
            jsNamLamViec = iNamLamViec;
              jsThangLamViec = iThangLamViec;
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("UpdateCauHinhNamLamViec?MaND=#0&iThangLamViec=#1&iNamLamViec=#2&iID_MaNamNganSach=#3&iID_MaNguonNganSach=#4","KeToanChiTietKhoBac") %>');
            url = unescape(url.replace("#0", '<%=MaND %>'));
            url = unescape(url.replace("#1", iThangLamViec));
            url = unescape(url.replace("#2", iNamLamViec));
            url = unescape(url.replace("#3", iID_MaNamNganSach));
            url = unescape(url.replace("#4", iID_MaNguonNganSach));

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

        function Hi() {
            alert('chào');
        }
        var GiayRutTien;
        function OnInit(value, title, iID_MaChungTu, iNamLamViec) {
            
            jQuery.ajaxSetup({ cache: false });
            var url = unescape('<%= Url.Action("SoDongChungTu?iID_MaChungTu=#0&iNamLamViec=#1","rptKTKhoBac_GiayRutDuToan") %>');
            url = unescape(url.replace("#0", iID_MaChungTu));
            url = unescape(url.replace("#1", iNamLamViec));
            var on = 0;
            $.getJSON(url, function (item) {

                if (item.SoCT != "") {

                    var r = confirm("Chứng từ " + item.SoCT + " số dòng lớn hơn Số cực đại KBNN quy định là " + item.ThamSo + "\n Đ/c có muốn đánh số lại các chứng từ không?");
                    if (r) {
                        jQuery.ajaxSetup({ cache: false });
                        var url = unescape('<%= Url.Action("TachSoChungTu?iID_MaChungTu=#0&dsSoCT=#1&ThamSo=#2","rptKTKhoBac_GiayRutDuToan") %>');
                        url = unescape(url.replace("#0", iID_MaChungTu));
                        url = unescape(url.replace("#1", item.SoCT));
                        url = unescape(url.replace("#2", item.ThamSo));
                        $.getJSON(url, function (item) {
                            OnInit_CT(value, title);                            
                            $('#idKTCT_GIAYRUTTIEN').click();
                            document.getElementById("idDialog").innerHTML = GiayRutTien;

                        });
                    }
                    else {
                        OnInit_CT(value, title);                                           
                        document.getElementById("idDialog").innerHTML = GiayRutTien;
                    }

                }
                else {
                    OnInit_CT(value, title);                    
                    document.getElementById("idDialog").innerHTML = GiayRutTien;
                }
            });
            
        }
        
        function OnInit_CT(value, title) {
            $("#idDialog").dialog("destroy");
            document.getElementById("idDialog").title = title;
            document.getElementById("idDialog").innerHTML = "";
            $("#idDialog").dialog({
                resizeable: false,
                draggable: true,
                width: value,
                modal: true
            });
        }

        function OnLoad_CT(v) {
            document.getElementById("idDialog").innerHTML = v;
            GiayRutTien = v;            
        }

        $(document).ready(function () {
        jsNamLamViec=<%=iNam %>;
            jsThangLamViec = <%=iThang %> ;
             jsLoai=<%=iLoai %>;
            BangDuLieu_Url_CheckTrungSoGhiSo = '<%=urlCheckTrungSoGhiSo %>';
             jsKeToan_url_MaChungTu_Check = '<%=urlget_get_Check_iID_MaChungTu %>';
            jsKeToan_url_ChungTu = '<%= Url.Action("ChungTu_Frame", "KTCT_KhoBac_ChungTuChiTiet")%>';
            jsKeToan_url_ChungTuChiTiet = '<%= Url.Action("ChungTuChiTiet_Frame", "KTCT_KhoBac_ChungTuChiTiet")%>';
            jsKeToan_url_KTCT_GIAYRUTTIEN = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTCT_GIAYRUTTIEN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';
            jsKeToan_url_KTCT_TTCHUYENTIEN = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTCT_TTCHUYENTIEN", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT" })%>';
            jsKeToan_url_KTCT_TTLOAI = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTCT_TTLOAI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';
            jsKeToan_url_KTCT_TTLOAINS = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTCT_TTLOAINS", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';
            //jsKeToan_url_KTCT_TTTONGHOP = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTCT_TTTONGHOP", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';
            jsKeToan_url_KTCT_TTTONGHOP = '<%= Url.Action("Index", "rptKTKB_ThongTriTongHop")%>';
            jsKeToan_url_KTTH_LICHSUCHUNGTU = '<%= Url.Action("Index", "NhapNhanh", new { id = "KTTH_KHOBAC_LICHSUCHUNGTU", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT"})%>';

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
                        Số chứng từ<span style="color: Red;">*</span></div>
                </td>
                <td>
                    <div>
                        <input id="divTongHop_sSoChungTu" class="textbox_uploadbox" tab-index="0" /></div>
                </td>
            </tr>
            <tr>
                <td class="td_label" style="width: 20%">
                    <div>
                        Ngày chứng từ<span style="color: Red;">*</span></div>
                </td>
                <td>
                    <div>
                        <input id="divTongHop_iNgay" class="textbox_uploadbox" tab-index="1" /></div>
                </td>
            </tr>
            <tr>
                <td class="td_label">
                    <div>
                        Nội dung</div>
                </td>
                <td>
                    <div>
                        <input id="divTongHop_sNoiDung" class="textbox_uploadbox"  tab-index="2"/></div>
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
        <%
            String strNhanCapThu = Url.Action("Index", "KTCT_CapThu");
        %>
        <script type="text/javascript">

            function submit_click() {
                $('#btn_submit').click();
            }
            function cancel_click() {
                $('#btn_cancel').click();
            }
            function LayCapThu() {
                var url = '<%=strNhanCapThu %>';
                location.href = url;
            }
        </script>
          
    </div>
</asp:Content>
