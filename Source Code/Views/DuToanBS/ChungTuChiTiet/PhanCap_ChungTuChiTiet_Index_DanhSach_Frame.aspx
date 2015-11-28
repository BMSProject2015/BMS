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
            string iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
            if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
            string MaLoai = "";
            if (String.IsNullOrEmpty(MaLoai)) MaLoai = Convert.ToString(ViewData["MaLoai"]);
            if (String.IsNullOrEmpty(MaLoai)) MaLoai = Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu", "iID_MaChungTu", iID_MaChungTu, "MaLoai"));

            //Cập nhập các thông tin tìm kiếm
            string DSTruong = "iID_MaDonVi,iID_MaPhongBanDich," + MucLucNganSachModels.strDSTruong;
            string[] arrDSTruong = DSTruong.Split(',');
            Dictionary<string, string> dicGiaTriTimKiem = new Dictionary<string, string>();
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                dicGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
            }
            String sLNS = Request.QueryString["sLNS"];
            if (String.IsNullOrEmpty(sLNS))
                sLNS =Convert.ToString(CommonFunction.LayTruong("DTBS_ChungTu", "iID_MaChungTu", iID_MaChungTu, "sDSLNS"));
            //int iCheck = 0;
            //String sL = "", sK = "", sM = "", sTM = "", sTTM = "";
            //if (sLNS.Substring(0, 2) == "80")
            //{
            //    DataTable dt_LNS = DuToanBS_ChungTuChiTietModels.Get_LNS_80(sLNS, ref iCheck);
            //    if (dt_LNS != null && dt_LNS.Rows.Count > 0 && iCheck == 1)
            //    {
            //        sL = Convert.ToString(dt_LNS.Rows[0]["sL"]);
            //        sK = Convert.ToString(dt_LNS.Rows[0]["sK"]);
            //        sM = Convert.ToString(dt_LNS.Rows[0]["sM"]);
            //        sTM = Convert.ToString(dt_LNS.Rows[0]["sTM"]);
            //        sTTM = Convert.ToString(dt_LNS.Rows[0]["sTTM"]);
            //        dt_LNS.Dispose();
            //    }
            //}
            //else
            //{
            //    DataTable dt_LNS = DuToanBS_ChungTuChiTietModels.Get_LNS(sLNS, ref iCheck);
            //    if (dt_LNS != null && dt_LNS.Rows.Count > 0 && iCheck == 1)
            //    {
            //        sL = Convert.ToString(dt_LNS.Rows[0]["sL"]);
            //        sK = Convert.ToString(dt_LNS.Rows[0]["sK"]);
            //        dt_LNS.Dispose();
            //    }
            //}

            DuToanBS_PhanCapBangDuLieu bang = new DuToanBS_PhanCapBangDuLieu(iID_MaChungTu, dicGiaTriTimKiem, MaND, IPSua, sLNS, MaLoai);
            string strDSDonVi = bang.strDSDonVi;
            string BangID = "BangDuLieu";
            int Bang_Height = 470;
            int Bang_FixedRow_Height = 50;
            string BackURL = Url.Action("Index", "DuToanBS_ChungTu", new { sLNS = sLNS });
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
        <input type="hidden" id="idDSDonVi" value="<%=strDSDonVi%>"/>
        <%  
            if (bang.ChiDoc == false)
            {
        %>
        <form action="<%=Url.Action("DetailSubmit", "DuToanBS_PhanCapChungTuChiTiet", new{iID_MaChungTu=iID_MaChungTu,sLNS=sLNS})%>"
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
    <%
        if (bang.ChiDoc == false)
        {
    %>
    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="table_form2">
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
        <td align="right" style="padding-right:10px;width:5%">        
                         <div onclick="OnInit_CT_NEW(500, 'Nhập excel');">
                    <%= Ajax.ActionLink("Nhập Excel", "Index", "NhapNhanh", new { id = "DuToan_NhapExcel", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu,iLoai="PC" }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
              <td align="right" style="width: 40%">
                <input type="button" id="btnLuu" class="button" onclick="javascript:return Bang_HamTruocKhiKetThuc();"
                    value="<%=NgonNgu.LayXau("Thực hiện")%>" />
            </td>
             <td align="right" style="width: 2%"></td>
            <td>
                &nbsp;
            </td>
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
    <script type="text/javascript">
        $(document).ready(function () {
            sLNS = '<%=sLNS%>';
        });
    </script>
    <script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
        type="text/javascript"></script>
    <script src="<%= Url.Content("~/Scripts/jsBang_DuToan_PhanCap.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
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
            Bang_arrDSTruongTien = '<%=MucLucNganSachModels.strDSTruongTien%>'.split(',');
            Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
            Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach_LNS", "Public", new {sLNS=sLNS})%>';
            BangDuLieu_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "DuToanBS_ChungTuChiTiet")%>';
            BangDuLieu_Url_PhanCap = '<%=Url.Action("Index", "DuToanBS_ChungTuChiTiet")%>';
            BangDuLieu_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "DuToanBS_ChungTuChiTiet")%>';
            BangDuLieu_iID_MaChungTu = '<%=iID_MaChungTu%>';
            BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%>;
             
            <%=bang.DuocSuaChiTiet?"":"Bang_keys.fnSetFocus(null, null);"%>
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
       <div id="idDialog" style="display: none;">
    <%} %>
</asp:Content>
