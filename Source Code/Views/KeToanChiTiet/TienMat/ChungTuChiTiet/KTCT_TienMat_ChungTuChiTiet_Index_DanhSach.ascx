<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
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
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String IPSua = Request.UserHostAddress;

    //Cập nhập các thông tin tìm kiếm
    String DSTruong = "iID_MaDonVi," + MucLucNganSachModels.strDSTruong;
    String[] arrDSTruong = DSTruong.Split(',');
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }

    KTCT_TienMat_ChungTuChiTiet_BangDuLieu bang = new KTCT_TienMat_ChungTuChiTiet_BangDuLieu(iID_MaChungTu, arrGiaTriTimKiem, MaND, IPSua);

    String BangID = "BangDuLieu";
    int Bang_Height = 470;
    int Bang_FixedRow_Height = 50;

    int iID_MaTrangThaiDuyet_TuChoi = KTCT_TienMat_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaChungTu);
    int iID_MaTrangThaiDuyet_TrinhDuyet = KTCT_TienMat_ChungTuChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaChungTu);
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
    
    <form action="<%=Url.Action("DetailSubmit", "KTCT_TienMat_ChungTuChiTiet", new{iID_MaChungTu=iID_MaChungTu})%>" method="post">
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
    <script type="text/javascript">
        $(document).ready(function () {
            BangDuLieu_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "KeToanTongHop_ChungTuChiTiet")%>';
            BangDuLieu_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "KeToanTongHop_ChungTuChiTiet")%>';
        });
    </script>
    <table width="100%" cellpadding="0" cellspacing="0" border="0" align="right" class="table_form2">
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td align=center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="right">
                            <input type="button" id="btnLuu" class="button" onclick="javascript:return Bang_HamTruocKhiKetThuc();" value="<%=NgonNgu.LayXau("Thực hiện")%>"/>
                        </td>
                        <td>&nbsp;</td>
                        <td align="center" width="100px">
                            <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" />
                        </td>
                        <td>&nbsp;</td>
                        <td align="left">
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
                        </td>
                        
                    </tr>
                </table>
            </td>
        </tr>
        <tr><td >&nbsp;</td></tr>
    </table>
<%
}
%>


<script type="text/javascript">
    $(document).ready(function () {
        Bang_arrDSTruongTien = '<%=MucLucNganSachModels.strDSTruongTien%>'.split(',');
        Bang_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "Public")%>';
        Bang_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "Public")%>';
        BangDuLieu_Url_getGiaTri = '<%=Url.Action("get_GiaTri", "KTCT_KhoBac_ChungTuChiTiet")%>';
        BangDuLieu_Url_getDanhSach = '<%=Url.Action("get_DanhSach", "KTCT_KhoBac_ChungTuChiTiet")%>';
        BangDuLieu_iID_MaChungTu = '<%=iID_MaChungTu%>';
        BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%>;
        <%=bang.DuocSuaChiTiet?"":"Bang_keys.fnSetFocus(null, null);"%>
    });
</script>

        </div>
    </div>
</div>