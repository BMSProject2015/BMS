<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/KeToanCongSan/jsBang_KeToanCongSan_TaiSan.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
    String ParentID = "Edit";
    String iLoai = Request.QueryString["iLoai"];
    String iID_MaLoaiTaiSan = Request.QueryString["iID_MaLoaiTaiSan"];
    String Saved = Request.QueryString["Saved"];
    String In = Request.QueryString["In"];
    String iID_MaTaiSan = Request.QueryString["iID_MaTaiSan"];
    DataTable dlLoaiTS = KTCS_TaiSanModels.DT_NhomTaiSan();
    SelectOptionList slLoaiTS = new SelectOptionList(dlLoaiTS, "MaLoaiTS", "sTen");
    dlLoaiTS.Dispose();

    DataTable dtDanhMucLoaiTS = KTCS_TaiSanModels.ddl_DanhMucNhomTaiSan(true);
    SelectOptionList slDanhMucLoaiTaiSan = new SelectOptionList(dtDanhMucLoaiTS, "iID_MaNhomTaiSan", "sTen");
    dtDanhMucLoaiTS.Dispose();
    String url_ChiTietTaiSan_Dat = Url.Action("TaiSan_Dat", "KTCS_TaiSan");
    String url_ChiTietTaiSan_Nha = Url.Action("TaiSan_Nha", "KTCS_TaiSan");
    String url_ChiTietTaiSan_Oto = Url.Action("TaiSan_Oto", "KTCS_TaiSan");
    String url_ChiTietTaiSan_Tren500Trieu = Url.Action("TaiSan_Tren500Trieu", "KTCS_TaiSan");
    String url_ChiTietTaiSan_Chung = Url.Action("TaiSan_Chung", "KTCS_TaiSan");
    NameValueCollection data = KTCS_TaiSanModels.LayThongTin(iLoai, iID_MaLoaiTaiSan);
    String urlReport_Chitiet = Url.Action("ViewPDF", "rptCongSan_ChiTietTaiSan");
%>

<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td align="left" style="width: 9%;">
            <div style="padding-left: 22px; padding-bottom: 5px; text-transform:uppercase; color:#ec3237;">
                <b><%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
            </div>         
        </td>
        <td align="left">
            <div style="padding-bottom: 5px; color:#ec3237;">
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%> |
                <%=MyHtmlHelper.ActionLink(Url.Action("List", "KTCS_TaiSan"), "Tài sản")%>
            </div>
        </td>
    </tr>
</table>
<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        $('#pHeader').click(function () {
            $('#dvContent').slideToggle('slow');
        });
    });
    $(document).ready(function () {
        jsKTCS_Url_ChiTietTaiSan_Dat = '<%=url_ChiTietTaiSan_Dat%>';
        jsKTCS_Url_ChiTietTaiSan_Nha = '<%=url_ChiTietTaiSan_Nha%>';
        jsKTCS_Url_ChiTietTaiSan_Oto = '<%=url_ChiTietTaiSan_Oto%>';
        jsKTCS_Url_ChiTietTaiSan_Tren500Trieu = '<%=url_ChiTietTaiSan_Tren500Trieu%>';
        jsKTCS_Url_ChiTietTaiSan_Chung = '<%=url_ChiTietTaiSan_Chung%>';
        jsKTCS_Url_Report = '<%=urlReport_Chitiet %>';
        $("DIV.ContainerPanel > DIV.collapsePanelHeader > DIV.ArrowExpand").toggle(
            function () {
                $(this).parent().next("div.Content").show("slow");
                $(this).attr("class", "ArrowClose");
            },
            function () {
                $(this).parent().next("div.Content").hide("slow");
                $(this).attr("class", "ArrowExpand");
            });
        });      
        
</script>
<div id="ContainerPanel" class="ContainerPanel">
    <div id="pHeader" class="collapsePanelHeader"> 
        <div id="dvHeaderText" class="HeaderContent" style="width: 80%;">
            <div style="width: 100%; float: left;">
                <span><%=NgonNgu.LayXau("Tìm kiếm thông tin tài sản")%></span>
            </div>
        </div>
        <div id="dvArrow" class="ArrowExpand"></div>
    </div>
    <div id="dvContent" class="Content" style="display:none">
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" align="left" style="width: 50%;">
                    <div id="nhapform">
                        <div id="form2">
                            <%
                            using (Html.BeginForm("SearchSubmit", "KTCS_TaiSan", new { ParentID = ParentID }))
                            {       
                            %>
                                <table cellpadding="0" cellspacing="0" border="0" class="table_form2" width="50%">
                                    <tr>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Loại tài sản</b></div></td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiTS, iLoai, "slLoaiTS", "", "class=\"input1_2\"")%>    
                                            </div>
                                        </td>
                                        <td class="td_form2_td1" style="width: 10%;"><div><b>Danh mục tài sản</b></div></td>
                                        <td class="td_form2_td5" style="width: 40%;">
                                            <div>
                                                <%=MyHtmlHelper.DropDownList(ParentID, slDanhMucLoaiTaiSan, iID_MaLoaiTaiSan, "iID_MaNhomTaiSan", "", "class=\"input1_2\"")%>        
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" class="td_form2_td1" align="right">
            	                            <div style="text-align:right; float:right; width:100%">
                                                <input type="submit" class="button4" value="Tìm kiếm" style="float:right; margin-left:10px;"/>
            	                            </div> 
            	                        </td>
                                    </tr>
                                    <tr><td class="td_form2_td1" align="right" colspan="2"></td></tr>
                                </table>
                            <%} %>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<br />
<div style="width: 100%; float: left;">
    <div style="width: 100%; float:left;">
        <div class="box_tong">
            <div class="title_tong">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td>
                            <span>Danh mục tài sản</span>
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <span>F2: Thêm hàng -- DELETE: Xóa Hàng -- F10: Lưu thông tin</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="nhapform">
                <div id="form2">
                    <%Html.RenderPartial("~/Views/CongSan/TaiSan/KTCS_TaiSan_Index_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>
                </div>
            </div>
        </div>
    </div>
</div>    

<div id="div_KTCS_TaiSanChiTiet" style="display:none;">
    <iframe id="KTCS_TaiSanChiTiet_iFrame" src="" width="100%" height="100%"></iframe>
</div>
<div id="div_Report" style="display:none;">
    <iframe id="ifr_Report" src="" width="100%" height="100%"></iframe>
</div>
</asp:Content>
