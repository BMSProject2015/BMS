<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
    String ParentID = ControlID + "_Search";
    String iQuy = Request.QueryString["iQuy"];
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String IPSua = Request.UserHostAddress;

    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(MaND);
    String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    dtCauHinh.Dispose();
    
    //Cập nhập các thông tin tìm kiếm
    String DSTruong = "sTenDoanhNghiep";
    String[] arrDSTruong = DSTruong.Split(',');
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }

    TCDN_BaoCaoTaiChinh_BangDuLieu bang = new TCDN_BaoCaoTaiChinh_BangDuLieu(iNam, iQuy, arrGiaTriTimKiem, MaND, IPSua);

    String BangID = "BangDuLieu";
    int Bang_Height = 470;
    int Bang_FixedRow_Height = 50;

    String strBaoCao = Url.Action("Reports", "rptTCDN_BaoCaoTaiChinhQuy", new { iNam = iNam, iQuy = iQuy });
%>

<div class="box_tong">
    <div id="nhapform">
        <div id="form2">
                    
<%Html.RenderPartial("~/Views/Shared/BangDuLieu/BangDuLieu.ascx", new { BangID = BangID, bang = bang, Bang_Height = Bang_Height, Bang_FixedRow_Height = Bang_FixedRow_Height }); %>    



<div style="display:none;">
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
    
    <form action="<%=Url.Action("DetailSubmit", "TCDN_BaoCaoTaiChinh", new{iQuy=iQuy, iNam=iNam})%>" method="post">
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
    <table width="100%" cellpadding="0" cellspacing="0" border="0" align="right" class="table_form2">
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td align="right">
                <input type="button" id="btnLuu" class="button" onclick="javascript:return Bang_HamTruocKhiKetThuc();" value="<%=NgonNgu.LayXau("Thực hiện")%>"/>
            </td>
            <td>&nbsp;</td>
            <td align="left" width="100px">
                <input id="TaoMoi" type="button" class="button" value="Báo cáo" onclick="javascript:location.href='<%=strBaoCao %>'" />
            </td>  
            <td>&nbsp;</td>
            <td align="left" width="100px">
                <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" />
            </td>            
        </tr>
        <tr><td>&nbsp;</td></tr>
    </table>                    
<%
}
%>
<%
    //dtChungTuChiTiet_TongCong.Dispose();
    String strDSTruongTien = "rVongDieuLe,rVonNhaNuoc,rTyLe,rTongVon_ChuSoHuu,rVonDauTu_ChuSoHuu,rThangDu_ChuSoHuu,rQuyDTPT_ChuSoHuu,rQuyDPPT_ChuSoHuu,rChenhLechTyGia_ChuSoHuu,rLoiNhuanChuaPP_ChuSoHuu,rVonKhac_ChuSoHuu,rDoanhThu,rLoiNhuanTruocThue,rLoiNhuanSauThue,rBangTien_VonNhaNuoc,rBangCoPhieu_VonNhaNuoc,rCong_VonNhaNuoc,rNopNganSach,rLaoDongBinhQuan,rTongQuyLuong,rThuNhapBinhQuan,rPhaiNop_VonNhaNuocKhiCoPhanHoa,rDaNop_VonNhaNuocKhiCoPhanHoa,rConPhaiNop_VonNhaNuocKhiCoPhanHoa,rCoTucNamTruoc_CoTuc,rCoTucNamNay_CoTuc,rDaNop_CoTuc,rConPhaiNop_CoTuc,rTienDatChuaNop_TienThueDat,rTienDatNamNay_TienThueDat,rDaNop_TienThueDat,rConPhaiNop_TienThueDat,rTongSoConPhaiNop,rTongSoDaNop,sGhiChu";
 %>
<script type="text/javascript">
    $(document).ready(function () {
        Bang_arrDSTruongTien = '<%=strDSTruongTien%>'.split(',');
        Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
        Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';
        BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%>;
        <%=bang.DuocSuaChiTiet?"":"Bang_keys.fnSetFocus(null, null);"%>
    });
</script>
        </div>
    </div>
</div>