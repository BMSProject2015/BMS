<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
            String MaND = User.Identity.Name;

            String IPSua = Request.UserHostAddress;
            String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
            if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(ViewData["iID_MaChungTu"]);
            String MaLoai = "";
            if (String.IsNullOrEmpty(MaLoai)) MaLoai = Convert.ToString(ViewData["MaLoai"]);
            if (String.IsNullOrEmpty(MaLoai)) MaLoai = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu", "iID_MaChungTu", iID_MaChungTu, "MaLoai"));

            String iLoai = Convert.ToString(ViewData["iLoai"]);
            String iKyThuat = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu", "iID_MaChungTu", iID_MaChungTu, "iKyThuat"));
            //Cập nhập các thông tin tìm kiếm
            String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
            String[] arrDSTruong = DSTruong.Split(',');
            Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
            for (int i = 0; i < arrDSTruong.Length; i++)
            {
                arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
            }
            String sLNS = Request.QueryString["sLNS"];
            if (String.IsNullOrEmpty(sLNS))
                sLNS = Convert.ToString(CommonFunction.LayTruong("DT_ChungTu", "iID_MaChungTu", iID_MaChungTu, "sDSLNS"));
            String iID_MaPhongBanDich = "05";
            //int iCheck = 0;

            //String sL = "", sK = "", sM = "", sTM = "", sTTM = "";
            //if (sLNS.Substring(0, 2) == "80")
            //{
            //    DataTable dt_LNS = DuToan_ChungTuChiTietModels.Get_LNS_80(sLNS, ref iCheck);
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
            //    DataTable dt_LNS = DuToan_ChungTuChiTietModels.Get_LNS(sLNS, ref iCheck);
            //    if (dt_LNS != null && dt_LNS.Rows.Count > 0 && iCheck == 1)
            //    {
            //        sL = Convert.ToString(dt_LNS.Rows[0]["sL"]);
            //        sK = Convert.ToString(dt_LNS.Rows[0]["sK"]);
            //        dt_LNS.Dispose();
            //    }
            //}

            DuToan_BangDuLieu bang = new DuToan_BangDuLieu(iID_MaChungTu, arrGiaTriTimKiem, MaND, IPSua, sLNS, MaLoai,iLoai);
            String strDSDonVi = bang.strDSDonVi;
            String BangID = "BangDuLieu";
            int Bang_Height = 470;
            int Bang_FixedRow_Height = 50;
            String BackURL = Url.Action("Index", "DuToan_ChungTu", new { sLNS = sLNS });
            int iID_MaTrangThaiDuyet_TuChoi = 0;
            int iID_MaTrangThaiDuyet_TrinhDuyet = 0;
            //Nếu loại NS bảo đảm
            if (sLNS == "1040100")
            {
                if (iLoai == "1")
                {
                    //iID_MaTrangThaiDuyet_TuChoi = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_TuChoi(MaND, iID_MaChungTu);
                    //iID_MaTrangThaiDuyet_TrinhDuyet = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_TrinhDuyet(MaND, iID_MaChungTu);
                }
                else if(iLoai=="4")
                {
                    
                }
                else
                {
                                       
                    iID_MaTrangThaiDuyet_TuChoi = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_BaoDam_TuChoi(MaND, iID_MaChungTu);
                    iID_MaTrangThaiDuyet_TrinhDuyet = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_BaoDam_TrinhDuyet(MaND, iID_MaChungTu);
                }
            }
            else
            {
                //gom
                if (iLoai == "1")
                {
                    iID_MaTrangThaiDuyet_TuChoi = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_TuChoi(MaND, iID_MaChungTu);
                    iID_MaTrangThaiDuyet_TrinhDuyet = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_Gom_TrinhDuyet(MaND, iID_MaChungTu);
                }
                else if (iLoai == "4")
                {
                   // iID_MaTrangThaiDuyet_TuChoi = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_BaoDam_TuChoi(MaND, iID_MaChungTu);
                    //iID_MaTrangThaiDuyet_TrinhDuyet = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_BaoDam_TrinhDuyet(MaND, iID_MaChungTu);
                }
                else
                {
                    iID_MaTrangThaiDuyet_TuChoi = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
                    iID_MaTrangThaiDuyet_TrinhDuyet = DuToan_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
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
        <form id="formDuyet" action="<%=Url.Action("DetailSubmit", "DuToan_ChungTuChiTiet", new{iID_MaChungTu=iID_MaChungTu,sLNS=sLNS,iLoai=iLoai})%>"
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
    %>
    <%if (sLNS == "1040100")
      { %>
    <td align="right" style="padding-right:10px;width:5%">        
                         <div onclick="OnInit_CT_NEW(500, 'Nhập excel');">
                    <%= Ajax.ActionLink("Nhập Excel", "Index", "NhapNhanh", new { id = "DuToan_NhapExcel", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
            <%} %>
             <td align="right" style="padding-right:10px;width:40%;">
                <input type="button" id="btnLuu" class="button" onclick="javascript:return Bang_HamTruocKhiKetThuc();"
                    value="<%=NgonNgu.LayXau("Thực hiện")%>" />
            </td>
               <td align="right" style="padding-right:10px;width:6%">        

                <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="Huy()" />
            </td>
            <%
            if (iID_MaTrangThaiDuyet_TuChoi > 0)
            {
                %>
             <td align="right" style="padding-right:10px;width:6%">        

               <%--<button class='button' style="float: left;" onclick="javascript:return Bang_HamTruocKhiKetThuc(1);">
                        Từ chối</button>--%>

                         <div onclick="OnInit_CT_NEW(500, 'Từ chối chứng từ');">
                    <%= Ajax.ActionLink("Từ chối", "Index", "NhapNhanh", new { id = "DuToan_TuChoiChiTiet", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
            <%} %>
             <%
            if (iID_MaTrangThaiDuyet_TrinhDuyet > 0)
            {
                String TrinhDuyet = "Duyệt";
                if (LuongCongViecModel.KiemTra_NguoiDungDuocDuyet(MaND, PhanHeModels.iID_MaPhanHeDuToan))
                {
                    TrinhDuyet = "Duyệt";
                }
                           
                                
                %>
             <td align="right" style="padding-right:10px;width:6%">        

              <%-- <button class='button' style="float: left;" onclick="javascript:return Bang_HamTruocKhiKetThuc(2);">
                        <%=TrinhDuyet%></button>--%>

                           <div onclick="OnInit_CT_NEW(500, 'Duyệt chứng từ');">
                    <%= Ajax.ActionLink("Trình Duyệt", "Index", "NhapNhanh", new { id = "DuToan_TrinhDuyetChiTiet", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
            <%}
            }%>
            
           <%if (iLoai == "1")
             { %>
           <td align="right"  style="padding-right:10px;">
                <div onclick="OnInit_CT_NEW(700, 'Báo cáo in kiểm');">
                    <%= Ajax.ActionLink("Báo cáo", "Index", "NhapNhanh", new { id = "DuToan_BaoCaoInKiem", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
            <%}
             else
             {
                  %>
                  <td align="right" style="padding-right:10px;">
                <div onclick="OnInit_CT_NEW(500, 'Tùy chỉnh');">
                    <%= Ajax.ActionLink("Tùy chỉnh", "Index", "NhapNhanh", new { id = "DuToan_TuyChinh", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
            </td>
           <td  style="padding-right:10px;">
                <div onclick="OnInit_CT_NEW(700, 'Báo cáo in kiểm');">
                    <%= Ajax.ActionLink("Báo cáo", "Index", "NhapNhanh", new { id = "DuToan_BaoCaoInKiem", OnLoad = "OnLoad_CT", OnSuccess = "CallSuccess_CT", iID_MaChungTu = iID_MaChungTu }, new AjaxOptions { }, new { @class = "button" })%>
                </div>
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
//dtChungTuChiTiet_TongCong.Dispose();
    %>
    <script type="text/javascript">
     
        $(document).ready(function () {
        //alert("!");
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

                // console.log(document.getElementById("formDuyet"));
                // return Bang_HamTruocKhiKetThuc();
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
    <script src="<%= Url.Content("~/Scripts/jsBang_DuToan.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>"
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
            BangDuLieu_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "DuToan_ChungTuChiTiet")%>';
              BangDuLieu_Url_PhanCap = '<%=Url.Action("Index", "DuToan_phanCapChungTuChiTiet")%>';
            BangDuLieu_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "DuToan_ChungTuChiTiet")%>';
            BangDuLieu_iID_MaChungTu = '<%=iID_MaChungTu%>';
            BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%>;
             iiD_MaPhongBanDich='<%=iID_MaPhongBanDich %>';
            <%=bang.DuocSuaChiTiet?"":"Bang_keys.fnSetFocus(null, null);"%>
            iLoai='<%=iLoai%>'
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
        <%} %>
</asp:Content>
