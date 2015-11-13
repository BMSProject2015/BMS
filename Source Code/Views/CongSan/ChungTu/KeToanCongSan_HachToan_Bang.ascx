<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>

<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);
    String ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
    String ParentID = ControlID + "_Search";
    String iID_MaChungTu = Request.QueryString["iID_MaChungTu"];
    String iID_MaChungTuChiTiet = Request.QueryString["iID_MaChungTuChiTiet"];
    if (String.IsNullOrEmpty(iID_MaChungTu)) iID_MaChungTu = Convert.ToString(props["iID_MaChungTu"].GetValue(Model)); ;
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String IPSua = Request.UserHostAddress;
    int iThang = Convert.ToInt32(NguoiDungCauHinhModels.LayCauHinhChiTiet(MaND, "iThangLamViec"));
        
    //Cập nhập các thông tin tìm kiếm
    String DSTruong = "MaND,TrangThai";
    String[] arrDSTruong = DSTruong.Split(',');
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }

    KTCS_ChungTuChiTiet_BangDuLieu bang = new KTCS_ChungTuChiTiet_BangDuLieu(iID_MaChungTu, arrGiaTriTimKiem, MaND, IPSua);

    int csH = 0;
    if (String.IsNullOrEmpty(iID_MaChungTuChiTiet) == false)
    {
        for (int i = 0; i < bang.dtChiTiet.Rows.Count; i++)
        {
            if (iID_MaChungTuChiTiet == Convert.ToString(bang.dtChiTiet.Rows[i]["iID_MaChungTuChiTiet"]))
            {
                csH = i;
                break;
            }
        }
    }
    String urlHachToan = Url.Action("HachToan_Frame", "KTCS_ChungTuChiTiet");
    String BangID = "BangDuLieu";
    int Bang_Height = 470;
    int Bang_FixedRow_Height = 50;
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
<input type="hidden" id="idCoCotTongSo" value="0" />
<%  
    if (bang.ChiDoc == false)
    {
    %>
    
    <form action="<%=Url.Action("DetailSubmit", "KTCS_ChungTuChiTiet", new{iID_MaChungTu=iID_MaChungTu})%>" method="post">
    <%
    } %>
        <input type="hidden" id="iID_MaChungTu" name="iID_MaChungTu" value="<%=iID_MaChungTu%>" />
        <input type="hidden" id="sSoChungTu" name="sSoChungTu" value="" />
        <input type="hidden" id="iNgay" name="iNgay" value="" />
        <input type="hidden" id="iThang" name="iThang" value="<%=iThang %>" />
        <input type="hidden" id="iTapSo" name="iTapSo" value="" />
        <input type="hidden" id="sDonVi" name="sDonVi" value="" />
        <input type="hidden" id="sNoiDung" name="sNoiDung" value="" />
        <input type="hidden" id="idAction" name="idAction" value="0" />
        <input type="hidden" id="idXauDuLieuThayDoi" name="idXauDuLieuThayDoi" value="<%=HttpUtility.HtmlEncode(bang.strThayDoi)%>" />
        <input type="hidden" id="idXauLaHangCha" name="idXauLaHangCha" value="<%=HttpUtility.HtmlEncode(bang.strLaHangCha)%>" />
        <input type="hidden" id="idXauMaCacHang" name="idXauMaCacHang" value="<%=HttpUtility.HtmlEncode(bang.strDSMaHang)%>" />
        <input type="hidden" id="idXauMaCacCot" name="idXauMaCacCot" value="<%=HttpUtility.HtmlEncode(bang.strDSMaCot)%>" />
        <input type="hidden" id="idXauGiaTriChiTiet" name="idXauGiaTriChiTiet" value="<%=HttpUtility.HtmlEncode(bang.strDuLieu)%>" />
        <input type="submit" id="btnXacNhanGhi" value="XN" />
        <input type="hidden" id="idXauCacHangDaXoa" name="idXauCacHangDaXoa" value="" />
<%
    if (bang.ChiDoc == false)
    {
    %>
    </form>    
    <%
    }
        %>

</div>


<script type="text/javascript">
    $(document).ready(function () {
        parent.ddl_reset('<%=HttpUtility.HtmlEncode(bang.strDSMaND)%>');
        Bang_arrDSTruongTien = '<%=MucLucNganSachModels.strDSTruongTien%>'.split(',');
        Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
        Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';
        BangDuLieu_Url_getSoNamKhaoHao = '<%=Url.Action("get_SoNamKhauHao", "KTCS_ChungTuChiTiet")%>';
        BangDuLieu_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "KTCT_KhoBac_ChungTuChiTiet")%>';
        BangDuLieu_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "KTCT_KhoBac_ChungTuChiTiet")%>';
        BangDuLieu_iID_MaChungTu = '<%=iID_MaChungTu%>';
        BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%>;
        BangDuLieu_sMauSac_TuChoi = '<%=bang.sMauSac_TuChoi %>';
        BangDuLieu_sMauSac_DongY = '<%=bang.sMauSac_DongY %>';
        Bang_keys.fnSetFocus(<%=csH %>, 0);
        BangDuLieu_Url_HachToan='<%=urlHachToan %>';
    });

 
</script>

        </div>
    </div>
</div>
