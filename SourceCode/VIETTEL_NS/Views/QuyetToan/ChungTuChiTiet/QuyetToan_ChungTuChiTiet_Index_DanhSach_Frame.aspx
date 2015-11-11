<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style></style>
    <%  
        String LoadLai = Convert.ToString(ViewData["LoadLai"]);
        if (LoadLai == "1")
        {
    %>
    <script type="text/javascript">
        $(document).ready(function () {
            parent.reloadPage();
        });
    </script>
    <%
}
        else
        {
            String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
            if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
            String MaLoai = "";
            if (String.IsNullOrEmpty(MaLoai)) MaLoai = Convert.ToString(ViewData["MaLoai"]);
            if (String.IsNullOrEmpty(MaLoai)) MaLoai = Convert.ToString(CommonFunction.LayTruong("QTA_ChungTu", "iID_MaChungTu", iID_MaChungTu, "MaLoai"));
            String iLoai = Convert.ToString(CommonFunction.LayTruong("QTA_ChungTu", "iID_MaChungTu", iID_MaChungTu, "iLoai"));

            String MaND = User.Identity.Name;
            String IPSua = Request.UserHostAddress;
            String urlBaoHiem = Url.Action("Index_BH", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
            String urlGiaiThichBangLoi = Url.Action("Index_GTBL", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
            String urlGiaiThichSoTien = Url.Action("Index_GTST", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });

            //Cập nhập các thông tin tìm kiếm
            String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
            String[] arrDSTruong = DSTruong.Split(',');
            Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
            for (int i = 0; i < arrDSTruong.Length - 1; i++)
            {
                arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
            }

            QuyetToan_ChungTu_BangDuLieu bang = new QuyetToan_ChungTu_BangDuLieu(iID_MaChungTu, arrGiaTriTimKiem, MaND, IPSua, MaLoai);
            String BangID = "BangDuLieu";
            int Bang_Height = 470;
            int Bang_FixedRow_Height = 50;

            String strLaySoLieuTuLuong = Url.Action("LoadDataLuong", "QuyetToan_ChungTuChiTiet", new { iID_MaChungTu = iID_MaChungTu });
            String BackURL = Url.Action("Index", "QuyetToan_ChungTu", new { Loai = iLoai });
            int iID_MaTrangThaiDuyet_TuChoi = QuyetToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
            int iID_MaTrangThaiDuyet_TrinhDuyet = QuyetToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
    %>
    <div class="box_tong">
        <style>
            .popup_block
            {
                top: 50%;
                left: 50%;
                margin-left: -10%;
                margin-top: -10%;
            }
        </style>
        <div id="nhapform">
            <div id="form2">
                <%Html.RenderPartial("~/Views/Shared/BangDuLieu/BangDuLieu.ascx", new { BangID = BangID, bang = bang, Bang_Height = Bang_Height, Bang_FixedRow_Height = Bang_FixedRow_Height }); %>
                <div style="display: none;">
                    <input type="hidden" id="idXauDoRongCot" value="<%=HttpUtility.HtmlEncode(bang.strDSDoRongCot)%>" />
                    <input type="hidden" id="idXauKieuDuLieu" value="<%=HttpUtility.HtmlEncode(bang.strType)%>" />
                    <input type="hidden" id="idXauChiSoCha" value="<%=HttpUtility.HtmlEncode(bang.strCSCha)%>" />
                    <input type="hidden" id="idBangChiDoc" value="<%=HttpUtility.HtmlEncode(bang.strChiDoc)%>" />
                    <input type="hidden" id="idXauEdit" value="<%=HttpUtility.HtmlEncode(bang.strEdit)%>" />
                    <input type="hidden" id="idViewport_N" value="<%=HttpUtility.HtmlEncode(bang.Viewport_N)%>" />
                    <input type="hidden" id="idNC_Fixed" value="<%=HttpUtility.HtmlEncode(bang.nC_Fixed)%>" />
                    <input type="hidden" id="idNC_Slide" value="<%=HttpUtility.HtmlEncode(bang.nC_Slide)%>" />
                    <%  
                        if (bang.ChiDoc == false)
                        {
                    %>
                    <form id="formDuyet" action="<%=Url.Action("DetailSubmit", "QuyetToan_ChungTuChiTiet", new{iID_MaChungTu=iID_MaChungTu,MaLoai=MaLoai})%>"
                    method="post">
                    <%
                        } %>
                    <input type="hidden" id="idAction" name="idAction" value="0" />
                    <input type="hidden" id="idXauDuLieuThayDoi" name="idXauDuLieuThayDoi" value="<%=HttpUtility.HtmlEncode(bang.strThayDoi)%>" />
                    <input type="hidden" id="idXauLaHangCha" name="idXauLaHangCha" value="<%=HttpUtility.HtmlEncode(bang.strLaHangCha)%>" />
                    <input type="hidden" id="idXauMaCacHang" name="idXauMaCacHang" value="<%=HttpUtility.HtmlEncode(bang.strDSMaHang)%>" />
                    <input type="hidden" id="idXauMaCacCot" name="idXauMaCacCot" value="<%=HttpUtility.HtmlEncode(bang.strDSMaCot)%>" />
                    <input type="hidden" id="idXauGiaTriChiTiet" name="idXauGiaTriChiTiet" value="<%=HttpUtility.HtmlEncode(bang.strDuLieu)%>" />
                    <input type="submit" id="btnXacNhanGhi" value="XN" />
                    <%
                        if (bang.ChiDoc == false)
                        {
                    %>
                    </form>
                    <%
                        }
                    %>
                </div>
                <%
                    if (bang.ChiDoc == false)
                    {
                %>
                <table width="100%" cellpadding="0" cellspacing="0" border="0" align="center" class="table_form2">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 40%; padding-right: 10px;">
                            <input type="button" id="btnLuu" class="button" onclick="javascript:return Bang_HamTruocKhiKetThuc();"
                                value="<%=NgonNgu.LayXau("Thực hiện")%>" />
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="Huy()" />
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <div onclick="OnInit_CT_NEW(500, 'Tùy chỉnh');">
                                <%= Ajax.ActionLink("Tùy chỉnh", "Index", "NhapNhanh", new { id = "QUYETTOAN_TuyChinh", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                            </div>
                        </td>
                        <td style="padding-right: 10px;">
                            <div onclick="OnInit_CT_NEW(700, 'Báo cáo in kiểm');">
                                <%= Ajax.ActionLink("In kiểm", "Index", "NhapNhanh", new { id = "QUYETTOAN_BAOCAOINKIEM", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                            </div>
                        </td>
                        <td style="padding-right: 10px;">
                            <div onclick="OnInit_CT_NEW(700, 'Thông tri');">
                                <%= Ajax.ActionLink("Thông tri", "Index", "NhapNhanh", new { id = "QUYETTOAN_BAOCAOTHONGTRI", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                            </div>
                        </td>
                        <td align="right" style="padding-right: 10px; width: 40%;">
                        </td>
                        <%--  <td align="left">
                <%
                if (iID_MaTrangThaiDuyet_TuChoi > 0)
                {
                    %>
                    <div style="float: left; padding-right: 10px;">
                        <button class='button' style="float:left;"  onclick="javascript:return Bang_HamTruocKhiKetThuc(1);">Từ chối</button>
                    </div>
                    <%
                }
                %>
                <%
                if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
                {
                    String TrinhDuyet = "Trình duyệt";
                    if (LuongCongViecModel.KiemTra_ThuTruong(MaND))
                    {
                        TrinhDuyet = "Phê duyệt";
                    }
                    %>
                    <div style="float: left;">
                        <button class='button' style="float:left;" onclick="javascript:return Bang_HamTruocKhiKetThuc(2);"><%=TrinhDuyet %></button>
                    </div>
                    <%
                }
                %>  
            </td>--%>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <%
                    }
                %>
                <%
//dtChungTuChiTiet_TongCong.Dispose();
                %>
                <script src="<%= Url.Content("~/Scripts/jsBang_QuyetToan.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
                    type="text/javascript"></script>
                <script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
                    type="text/javascript"></script>
                <script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
                    type="text/javascript"></script>
                <script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
                    type="text/javascript"></script>
                <script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
                    type="text/javascript"></script>
                <script type="text/javascript">
    $(document).ready(function () {
     $('#dvText').hiden();
        Bang_arrDSTruongTien = '<%=MucLucNganSachModels.strDSTruongTien%>'.split(',');
        Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
        Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';
        BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%>;
        <%=bang.DuocSuaChiTiet?"":"Bang_keys.fnSetFocus(null, null);"%>
    });
     $("#btnLuu").click(function () {
           ShowPopupThucHien();
        });

    function Huy() {
        window.parent.location.href = '<%=BackURL %>';
    }
    function jsLayTuLuong_Dialog_Show() {
        $("#idDialog").dialog({
            width: 200,
            height: 100,
            modal: true,
            title: 'Cập nhật số liệu lương',
            close:jsLayTuLuong_Dialog_close
        });    
        function jsLayTuLuong_Dialog_close() {
            $("#idDialog").dialog('close');
        }
    }

    
    function jsBH_Dialog_Show() {
        document.getElementById('ifrBaoHiem').src='<%=urlBaoHiem %>';
        $("#idDialog_BH").dialog({
            width: 1100,
            height: 600,
            modal: true,
            title: 'Cập nhật',
            close:jsBH_Dialog_close
        });            
        
    }
    function jsBH_Dialog_close() {
            $("#idDialog_BH").dialog('close');
        }
    function jsBH_Dialog_close_Reload()
    {       
        document.getElementById('ifrBaoHiem').src='<%=urlBaoHiem %>';
        $("#idDialog_BH").dialog('close');
    }

    function jsGTST_Dialog_Show() {
      document.getElementById('ifrGTST').src='<%=urlGiaiThichSoTien %>';
        $("#idDialog_GTST").dialog({
            width: 960,
            height: 450,
            modal: true,
            title: 'Cập nhật',
            close:jsGTST_Dialog_close
        });            
        
    }
    function jsGTST_Dialog_close() {
            $("#idDialog_GTST").dialog('close');
        }
    function jsGTST_Dialog_close_Reload()
    {       
        document.getElementById('ifrGTST').src='<%=urlGiaiThichSoTien %>';
        $("#idDialog_GTST").dialog('close');
    }

    function jsGTBL_Dialog_Show() {
      document.getElementById('ifrGTBL').src='<%=urlGiaiThichBangLoi %>';
        $("#idDialog_GTBL").dialog({
            width: 960,
            height: 600,
            modal: true,
            title: 'Cập nhật',
            close:jsGTBL_Dialog_close
        });            
        
    }
    function jsGTBL_Dialog_close() {
            $("#idDialog_GTBL").dialog('close');
        }
    function jsGTBL_Dialog_close_Reload()
    {       
        document.getElementById('ifrGTBL').src='<%=urlGiaiThichBangLoi %>';
        $("#idDialog_GTBL").dialog('close');
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
                      open: function (event, ui) {
                    $(event.target).parent().css('position', 'fixed');
                    $(event.target).parent().css('top', '10px');
                    
                }
            });
        }
          function OnLoad_CT(v) {
            document.getElementById("idDialog").innerHTML = v;
        }
                </script>
            </div>
        </div>
    </div>
    <div id="idDialog" style="display: none;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <b>Bạn có muốn lấy từ lương không</b>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="left">
                    <input id="Button1" type="button" class="button" value="Nhập Lương" onclick="javascript:location.href='<%=strLaySoLieuTuLuong %>'" />
                </td>
                <td align="left">
                    <input id="Button2" type="button" class="button" value="Hủy" onclick="javascript:return jsLayTuLuong_Dialog_close()" />
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            function jsLayTuLuong_Dialog_close() {
                $("#idDialog").dialog('close');
            }
        </script>
    </div>
    <div id="idDialog_BH" style="display: none;">
        <iframe id="ifrBaoHiem" width="98%" height="98%" src=""></iframe>
    </div>
    <div id="idDialog_GTST" style="display: none;">
        <iframe id="ifrGTST" width="98%" height="98%" src=""></iframe>
    </div>
    <div id="idDialog_GTBL" style="display: none;">
        <iframe id="ifrGTBL" width="98%" height="98%" src=""></iframe>
    </div>
    <div id="dvText" class="popup_block">
        <img src="../../../Content/ajax-loader.gif" /><br />
        <p>
            Hệ thống đang thực hiện yêu cầu...</p>
    </div>
    <%} %>
</asp:Content>
