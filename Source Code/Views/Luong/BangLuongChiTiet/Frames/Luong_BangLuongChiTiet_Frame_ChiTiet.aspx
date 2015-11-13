<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<%@ Import Namespace="System.ComponentModel" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/Luong/jsBang_Luong.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<%
    String MaND = User.Identity.Name;
    String IPSua = Request.UserHostAddress;
    String ControlID = "Parent";
    String ParentID = ControlID + "_Search";
    String iID_MaBangLuong = Request.QueryString["iID_MaBangLuong"];

    //Cập nhập các thông tin tìm kiếm
    String DSTruong = BangLuongModels.strDSTruong;
    String[] arrDSTruong = DSTruong.Split(',');
    Dictionary<String, String> arrGiaTriTimKiem = new Dictionary<string, string>();
    for (int i = 0; i < arrDSTruong.Length; i++)
    {
        arrGiaTriTimKiem.Add(arrDSTruong[i], Request.QueryString[arrDSTruong[i]]);
    }

    Boolean bPhanTruyLinh = false;
    if (Convert.ToString(ViewData["bPhanTruyLinh"]) == "1")
    {
        bPhanTruyLinh = true;
    }
    int iLoaiBangLuong = Luong_BangDuLieu.iLoaiBangLuong_BangChiTiet;
    if(bPhanTruyLinh)
    {
        iLoaiBangLuong = Luong_BangDuLieu.iLoaiBangLuong_BangTruyLinh;
    }
    Luong_BangDuLieu bang = new Luong_BangDuLieu(iID_MaBangLuong, iLoaiBangLuong, arrGiaTriTimKiem, MaND, IPSua);

    String BangID = "BangDuLieu";
    int Bang_Height = 470;
    int Bang_FixedRow_Height = 50;
    int csH = 0, csC = 0;
    if (CommonFunction.IsNumeric(Request.QueryString["csH"]))
    {
        csH = Convert.ToInt32(Request.QueryString["csH"]);
        csC = Convert.ToInt32(Request.QueryString["csC"]);
    }
     
    int iID_MaTrangThaiDuyet_TuChoi = BangLuongChiTietModels.Get_iID_MaTrangThaiDuyet_TuChoi(MaND, iID_MaBangLuong);
    int iID_MaTrangThaiDuyet_TrinhDuyet = BangLuongChiTietModels.Get_iID_MaTrangThaiDuyet_TrinhDuyet(MaND, iID_MaBangLuong);
    String DaXong = Request.QueryString["DaXong"];
    String ThemMoiCanBo = Url.Action("ThemCanBo", "Luong_BangLuongChiTiet_CanBo", new { iID_MaBangLuong = iID_MaBangLuong});
%>
<div class="box_tong">
    <div id="nhapform">
        <div id="form2">
        
<%--<form action="<%=Url.Action("SearchSubmit","Luong_BangLuongChiTiet",new {ParentID = ParentID, iID_MaBangLuong = iID_MaBangLuong})%>" method="post">
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
    if (bang.ChiDoc==false)
    {
    %>
    <form action="<%=Url.Action("DetailSubmit", "Luong_BangLuongChiTiet", new {iID_MaBangLuong=iID_MaBangLuong, Detail = "ChiTiet"})%>" method="post">
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
        <tr><td>&nbsp;</td></tr>
    </table>                    
<%
}
%>
<%
    //dtChungTuChiTiet_TongCong.Dispose();
%>
<script type="text/javascript">
    var BangDuLieu_Url = '<%=Url.Action("Index", "Luong_BangLuongChiTiet", new {iID_MaBangLuong = iID_MaBangLuong,LoaiBang=Luong_BangDuLieu.iLoaiBangLuong_BangChiTiet})%>';
    var BangDuLieu_Url_CaNhan = '<%=Url.Action("Detail", "Luong_BangLuongChiTiet_CanBo")%>';
    var BangDuLieu_Url_TrichLuong='<%=Url.Action("TrichLuong","Luong_BangLuongChiTiet") %>';
    var BangDuLieu_Url_DieuChinhTienAn='<%=Url.Action("DieuChinhTienAn","Luong_BangLuongChiTiet") %>';
    var BangDuLieu_Url_HeSoKhuVuc='<%=Url.Action("HeSoKhuVuc","Luong_BangLuongChiTiet") %>';
    var BangDuLieu_Url_HuyTapThe='<%=Url.Action("HuyTapThe","Luong_BangLuongChiTiet") %>';
    var BangDuLieu_Url_ThemMoiCanBo= '<%=ThemMoiCanBo %>';
    $(document).ready(function () {
        BangDuLieu_BangTruyLinh = <%=bPhanTruyLinh?"true":"false"%>;
        Bang_arrDSTruongTien = '<%=MucLucNganSachModels.strDSTruongTien%>'.split(',');
        BangDuLieu_iID_MaBangLuong = '<%=iID_MaBangLuong%>';
        BangDuLieu_DuocSuaChiTiet = <%=bang.DuocSuaChiTiet?"true":"false"%>;
        if (Bang_nH > 0) {
            Bang_keys.fnSetFocus(<%=csH%>, <%=csC%>);
        }
        Bang_keys.focus();
    });
    function ThemCanBo(MaBangLuong)
    {    
       $("#divLuong_BangLuongChiTiet").dialog({
        width: 920,
        height: 650,
        modal: true,
        title: 'Thêm cán bộ',
        close: BangDuLieu_Dialog_Close
         });         
        document.getElementById("BangLuongChiTiet_iFrame").src = BangDuLieu_Url_ThemMoiCanBo;
    }
</script>

        </div>
    </div>
</div>
</asp:Content>
