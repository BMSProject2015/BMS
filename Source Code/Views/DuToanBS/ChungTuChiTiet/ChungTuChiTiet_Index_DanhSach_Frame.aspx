<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="VIETTEL.Models.DuToanBS" %>
<%@ Import Namespace="System.ComponentModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%  
        string LoadLai = Convert.ToString(ViewData["LoadLai"]);
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
            string MaND = User.Identity.Name;
            string IPSua = Request.UserHostAddress;
            
            //Mã chứng từ.
            string iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
            if (String.IsNullOrEmpty(iID_MaChungTu)) 
                iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
            
            //Mã loại.
            string MaLoai = "";
            if (String.IsNullOrEmpty(MaLoai)) MaLoai = Convert.ToString(ViewData["MaLoai"]);
            if (String.IsNullOrEmpty(MaLoai)) MaLoai = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu", "iID_MaChungTu", iID_MaChungTu, "MaLoai"));

            string iLoai = Convert.ToString(ViewData["iLoai"]);
            string iChiTapTrung = Convert.ToString(ViewData["iChiTapTrung"]);
            string iKyThuat = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu", "iID_MaChungTu", iID_MaChungTu, "iKyThuat"));
            
            //Cập nhập các thông tin tìm kiếm
            string DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
            String[] arrDSTruong = DSTruong.Split(',');
            Dictionary<string, string> dicGiaTriTimKiem = new Dictionary<string, string>();
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                dicGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
            }

            string sLNS = Request.QueryString["sLNS"];
            if (String.IsNullOrEmpty(sLNS))
                sLNS = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu", "iID_MaChungTu", iID_MaChungTu, "sDSLNS"));
            //????
            string iID_MaPhongBanDich = "05";
            
            DuToanBS_BangDuLieu bang = new DuToanBS_BangDuLieu(iID_MaChungTu, dicGiaTriTimKiem, MaND, IPSua, sLNS, MaLoai, iLoai, iChiTapTrung);
            string strDSDonVi = bang.strDSDonVi;
            string BangID = "BangDuLieu";
            int Bang_Height = 470;
            int Bang_FixedRow_Height = 50;
            string BackURL = "";
            if (!String.IsNullOrEmpty(sLNS))
            {
                string sLNS1 = sLNS.Substring(0, 3);
                if (sLNS1 == "104" || sLNS1 == "109")
                {
                    BackURL = Url.Action("Index", "DuToanBS_ChungTu", new { sLNS1 = sLNS1, iKyThuat = iKyThuat });
                }
                else
                {
                    sLNS1 = sLNS.Substring(0, 1);
                    BackURL = Url.Action("Index", "DuToanBS_ChungTu", new { sLNS1 = sLNS1 });
                }
            }
            int iID_MaTrangThaiDuyet_TuChoi = 0;
            int iID_MaTrangThaiDuyet_TrinhDuyet = 0;
            //Nếu loại NS bảo đảm
            if (sLNS == "1040100")
            {
                if(iLoai=="4")
                {
                    
                }
                else
                {

                    iID_MaTrangThaiDuyet_TuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoiBaoDam(MaND, iID_MaChungTu);
                    iID_MaTrangThaiDuyet_TrinhDuyet = DuToanBS_ChungTuModels.LayMaTrangThaiTrinhDuyetBaoDam(MaND, iID_MaChungTu);
                }
            }
            else
            {
                if (iLoai == "4")
                {

                }
                else
                {
                    iID_MaTrangThaiDuyet_TuChoi = DuToanBS_ChungTuModels.LayMaTrangThaiTuChoi(MaND, iID_MaChungTu);
                    iID_MaTrangThaiDuyet_TrinhDuyet = DuToanBS_ChungTuModels.LayMaTrangThaiTrinhDuyet(MaND, iID_MaChungTu);
                }
            }
    %>
    <%Html.RenderPartial("~/Views/Shared/BangDuLieu/BangDuLieu.ascx", new { BangID = BangID, bang = bang, Bang_Height = Bang_Height, Bang_FixedRow_Height = Bang_FixedRow_Height }); %>
    <div style="display: none;">
        <input type="hidden" id="idXauHienThiCot" value="<%=HttpUtility.HtmlEncode(bang.strDSHienThiCot)%>" />
        <input type="hidden" id="idXauDoRongCot" value="<%=HttpUtility.HtmlEncode(bang.strDSDoRongCot)%>" />
        <input type="hidden" id="idXauKieuDuLieu" value="<%=HttpUtility.HtmlEncode(bang.strType)%>" />
        <input type="hidden" id="idXauChiSoCha" value="<%=HttpUtility.HtmlEncode(bang.strCSCha)%>" />
        <input type="hidden" id="idBangChiDoc" value="<%=HttpUtility.HtmlEncode(bang.strChiDoc)%>" />
        <input type="hidden" id="idXauEdit" value="<%=HttpUtility.HtmlEncode(bang.strEdit)%>" />
        <input type="hidden" id="idViewport_N" value="<%=HttpUtility.HtmlEncode(bang.Viewport_N)%>" />
        <input type="hidden" id="idNC_Fixed" value="<%=HttpUtility.HtmlEncode(bang.nC_Fixed)%>" />
        <input type="hidden" id="idNC_Slide" value="<%=HttpUtility.HtmlEncode(bang.nC_Slide)%>" />
        <input type="hidden" id="idDSChiSoNhom" value="<%=HttpUtility.HtmlEncode(bang.strDSChiSoNhom)%>" />
        <input type="hidden" id="idDSDonVi" value="<%=strDSDonVi%>" />
        <%  
            if (bang.ChiDoc == false)
            {
        %>
        <form id="formDuyet" action="<%=Url.Action("DetailSubmit", "DuToanBS_ChungTuChiTiet", new{iID_MaChungTu=iID_MaChungTu,sLNS=sLNS,iLoai=iLoai})%>"
        method="post">
        <%
            } %>
        <input type="hidden" id="idAction" name="idAction" value="0" />
         <input type="hidden" id="sLyDo" name="sLyDo" />
        <input type="hidden" id="idXauDuLieuThayDoi" name="idXauDuLieuThayDoi" value="<%=HttpUtility.HtmlEncode(bang.strThayDoi)%>" />
        <input type="hidden" id="idXauLaHangCha" name="idXauLaHangCha" value="<%=HttpUtility.HtmlEncode(bang.strLaHangCha)%>" />
        <input type="hidden" id="idXauMaCacHang" name="idXauMaCacHang" value="<%=HttpUtility.HtmlEncode(bang.strDSMaHang)%>" />
        <input type="hidden" id="idXauMaCacCot" name="idXauMaCacCot" value="<%=HttpUtility.HtmlEncode(bang.strDSMaCot)%>" />
        <input type="hidden" id="idXauGiaTriChiTiet" name="idXauGiaTriChiTiet" value="<%=HttpUtility.HtmlEncode(bang.strDuLieu)%>" />
        <input type="submit" id="btnXacNhanGhi" name="btnXacNhanGhi" value="XN" />
        <input type="hidden" id="idXauCacHangDaXoa" name="idXauCacHangDaXoa" value="" />
        <input type="hidden" id="idMaMucLucNganSach" name="idMaMucLucNganSach" value="<%=HttpUtility.HtmlEncode(bang.strMaMucLucNganSach)%>" />
        <%
        if (bang.ChiDoc == false)
        {
            %>
            </form>
            <%
        }
        %>
    </div>
    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
        <%
        if (bang.ChiDoc == false)
        {
            if (sLNS == "1040100")
            {%>
            <%--Button Nhập Excel--%>
            <td align="right" style="padding-right:10px;width:5%">        
                <div onclick="OnInit_CT_NEW(500, 'Nhập excel');">
                    <%= Ajax.ActionLink("Nhập Excel", "Index", "NhapNhanh", new { id = "DuToanBS_NhapExcel", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
            <%} %>
            <%--Button Thực Hiện--%>
            <td align="right" style="padding-right:10px;width:40%;">
                <input type="button" id="btnLuu" class="button" onclick="javascript:return Bang_HamTruocKhiKetThuc();"
                    value="<%=NgonNgu.LayXau("Thực hiện")%>" />
            </td>
            <%
            if (iID_MaTrangThaiDuyet_TuChoi > 0)
            {
            %>
            <td align="right" style="padding-right:10px;width:6%">        
                <div onclick="OnInit_CT_NEW(500, 'Từ chối chứng từ');">
                    <%= Ajax.ActionLink("Từ chối", "Index", "NhapNhanh", new { id = "DuToanBS_TuChoiChiTiet", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
            <%} 
            if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
            {
            %>
            <td align="right" style="padding-right:10px;width:6%">        
                <div onclick="OnInit_CT_NEW(500, 'Duyệt chứng từ');">
                    <%= Ajax.ActionLink("Trình Duyệt", "Index", "NhapNhanh", new { id = "DuToanBS_TrinhDuyetChiTiet", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
            <%}
            }%>
            
           <%if (iLoai == "1")
             { %>
            <td align="right"  style="padding-right:10px;">
                <div onclick="OnInit_CT_NEW(700, 'Báo cáo in kiểm');">
                    <%= Ajax.ActionLink("Báo cáo", "Index", "NhapNhanh", new { id = "DuToanBS_BaoCaoInKiem", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
            <%}
             else
             {
            %>
            <td align="right" style="padding-right:10px;">
                <div onclick="OnInit_CT_NEW(500, 'Tùy chỉnh');">
                    <%= Ajax.ActionLink("Tùy chỉnh", "Index", "NhapNhanh", new { id = "DuToanBS_TuyChinh", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
            <td  style="padding-right:10px;">
                <div onclick="OnInit_CT_NEW(700, 'Báo cáo in kiểm');">
                    <%= Ajax.ActionLink("Báo cáo", "Index", "NhapNhanh", new { id = "DuToanBS_BaoCaoInKiem", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu,iChiTapTrung=iChiTapTrung }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
            <td align="right" style="padding-right:10px;width:6%">        
                <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="Huy()" />
            </td>
            <%} %>
            <td align="right" style="padding-right:10px;width:40%;">
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <%
    %>
    <script type="text/javascript">
        $(document).ready(function () {
//            $("<div>test</div>").dialog({
//                model: true,
//                buttons: [
//                    Ok: function(){
//                    $(this).dialog("close");
//                    }
//                ]
//            });
            sLNS = '<%=sLNS%>';
            $('#btnDuyet').live("click", function () {
                Bang_GanMangGiaTri_Bang_arrGiaTri();
                if (document.getElementById("idAction")) document.getElementById("idAction").value = 2;
                if (document.getElementById("sLyDo")) {
                    document.getElementById("sLyDo").value = document.getElementById("DuToan_sLyDo1").value;
                }

                document.getElementById("formDuyet").submit();
            });

            $('#btnTuChoi').live("click", function () {
                Bang_GanMangGiaTri_Bang_arrGiaTri();
                if (document.getElementById("idAction")) document.getElementById("idAction").value = 1;
                if (document.getElementById("sLyDo")) {
                    document.getElementById("sLyDo").value = document.getElementById("DuToan_sLyDo1").value;
                }
                document.getElementById("formDuyet").submit();
            });
        });
    </script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_DuToanBS.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script type="text/javascript">
         function Huy() {
        window.parent.location.href = '<%=BackURL %>';
    }
        $(document).ready(function () {
//            $('#dvText').hiden();
            Bang_arrDSTruongTien = '<%=MucLucNganSachModels.strDSTruongTien%>'.split(',');
            Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
            Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach_LNS", "Public", new {sLNS=sLNS})%>';
            BangDuLieu_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "DuToanBS_ChungTuChiTiet")%>';
            BangDuLieu_Url_PhanCap = '<%=Url.Action("Index", "DuToanBS_phanCapChungTuChiTiet")%>';
            BangDuLieu_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "DuToan_ChungTuChiTiet")%>';
            BangDuLieu_iID_MaChungTu = '<%=iID_MaChungTu%>';
            BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%>;
            iiD_MaPhongBanDich='<%=iID_MaPhongBanDich %>';
            <%=bang.DuocSuaChiTiet?"":"Bang_keys.fnSetFocus(null, null);"%>
            iLoai='<%=iLoai%>'
            iChiTapTrung='<%=iChiTapTrung%>'
        });
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
    <div id="idDialog" style="display: none;"></div>
    <div id="dvText" class="popup_block">
        <img src="../../../Content/ajax-loader.gif" /><br />
        <p>Hệ thống đang thực hiện yêu cầu...</p>
    </div>
        <%} %>
</asp:Content>
