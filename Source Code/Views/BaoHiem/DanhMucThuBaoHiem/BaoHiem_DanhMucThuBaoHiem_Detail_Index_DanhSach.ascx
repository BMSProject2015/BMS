<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>

<%
    PropertyDescriptorCollection props = TypeDescriptor.GetProperties(Model);

    String ControlID = Convert.ToString(props["ControlID"].GetValue(Model));
    String iThang = Convert.ToString(props["iThang"].GetValue(Model));
    String iNamLamViec = Convert.ToString(props["iNamLamViec"].GetValue(Model));
    String MaND = Convert.ToString(props["MaND"].GetValue(Model));
    String IPSua = Request.UserHostAddress;
    
    DanhMucBaoHiem_BangDuLieu bang = new DanhMucBaoHiem_BangDuLieu(iNamLamViec, iThang, MaND, IPSua);

    String BangID = "BangDuLieu";
    int Bang_Height = 470;
    int Bang_FixedRow_Height = 50; 
   

    Boolean CoCotDuyet = false;//bang.CoCotDuyet;

%>

<div class="box_tong">
    <div id="nhapform">
        <div id="form2">

<%--<form action="<%=Url.Action("SearchSubmit","BaoHiem_ChungTuChiTiet",new {ParentID = ParentID, iID_MaChungTuChi = iID_MaChungTuChi})%>" method="post">
    <table class="mGrid1">
    <tr>
        <%
        for (int j = 0; j <= bang.arrDSMaCot.IndexOf("sTNG"); j++)
        {
            int iColWidth = bang.arrWidth[j] +4;
            if (j == 0) iColWidth = iColWidth + 1;
            String strAttr = String.Format("class='input1_4' style='width:{0}px;height:22px;'", iColWidth - 2);
            if (bang.DuocSuaChiTiet == false) strAttr += " tab-index='-1'";
            %>
            <td style="text-align:left;width:<%=iColWidth%>px;">
                <%=MyHtmlHelper.TextBox(new { ParentID = ParentID, Value = arrGiaTriTimKiem[bang.arrDSMaCot[j]], TenTruong = bang.arrDSMaCot[j], LoaiTextBox = "2", Attributes = strAttr })%>
            </td>
            <%
        }
        %>
        <td style="padding: 1px 5px; text-align: left;">
            <input type="submit" id="<%=ParentID%>_btnTimKiem" <%=bang.DuocSuaChiTiet? "":"tab-index='-1'" %> value="<%=NgonNgu.LayXau("Tìm kiếm")%>" style="font-size: 11px; padding: 0px 3px;"/> 
        </td>
    </tr>
    </table>
</form>--%>
            
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
    
    <form action="<%=Url.Action("DetailSubmit","BaoHiem_DanhMucThuBaoHiem",new {iNamLamViec=iNamLamViec,iThang=iThang}) %>" method="post">
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
                <input class="button" type="button" value="<%=NgonNgu.LayXau("Quay lại")%>" onclick="history.go(-1)" />
            </td>
            <td>&nbsp;</td>
            
        </tr>
        <tr><td>&nbsp;</td></tr>
    </table>                    
<%
}
%>
<script type="text/javascript">
    $(document).ready(function () {

    });
</script>
        </div>
    </div>
</div>