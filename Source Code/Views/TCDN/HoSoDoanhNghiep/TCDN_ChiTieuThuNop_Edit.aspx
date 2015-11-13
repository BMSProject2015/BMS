<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%= Url.Content("~/Scripts/TCDN/jsBang_TaiChinhDoanhNghiep_ThuNop.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
    String iID_MaDoanhNghiep = Convert.ToString(ViewData["iID_MaDoanhNghiep"]);
    String iQuy = Convert.ToString(ViewData["iQuy"]);
    String iNam = Convert.ToString(ViewData["iNam"]);
    String ParentID = "Edit";

    DataTable dtDoanhNghiep = TCSN_DoanhNghiepModels.Get_ListDoanhNghiep(true);
    SelectOptionList slDoanhNghiep = new SelectOptionList(dtDoanhNghiep, "iID_MaDoanhNghiep", "sTenDoanhNghiep");
    dtDoanhNghiep.Dispose();
    
    DataTable dtQuy = DanhMucModels.DT_Quy();
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    dtQuy.Dispose();

    String sTenDoanhNghiep = TCSN_DoanhNghiepModels.Get_TenDonVi(iID_MaDoanhNghiep);
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "TCDN_DoanhNghiep"), "Danh sách doanh nghiệp")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                    <span>Tình hình thu nộp quý <%=iQuy %> năm <%=iNam%> của doanh nghiệp <%=sTenDoanhNghiep %></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2" style="padding: 10px;">
            <table cellpadding="5" cellspacing="5" width="100%">
                <tr>
                    <td class="td_form2_td1" style="width: 10%">
                        <div><b>Chọn doanh nghiệp</b></div>
                    </td>
                    <td class="td_form2_td5"  style="width: 40%">
                        <div>
                            <script type="text/javascript">
                                function ddlDoanhnghiep_SelectedValueChanged(ctl) {
                                    var url = "<%=Url.Action("List", "TCDN_ChiTieuThuNop", new { iQuy=iQuy,iNam=iNam})%>";
                                    if(ctl.selectedIndex>=0)
                                    {
                                        var value = ctl.options[ctl.selectedIndex].value;
                                        if(value!="")
                                        {
                                            url += "&iID_MaDoanhNghiep=" + value;
                                        }
                                    }
                                    location.href = url;
                                }
                            </script>
                            <%=MyHtmlHelper.DropDownList(ParentID, slDoanhNghiep, iID_MaDoanhNghiep, "iID_MaDoanhNghiep", "", "onChange=\"ddlDoanhnghiep_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                        </div>
                    </td>
                    <td class="td_form2_td1" style="width: 10%">
                        <div><b>Quý làm việc</b></div>
                    </td>
                    <td class="td_form2_td5"  style="width: 40%">
                        <div>
                            <script type="text/javascript">
                                function ddlQuy_SelectedValueChanged(ctl) {
                                    var url = "<%=Url.Action("List", "TCDN_ChiTieuThuNop", new {iNam=iNam,iID_MaDoanhNghiep=iID_MaDoanhNghiep})%>";
                                    if(ctl.selectedIndex>=0)
                                    {
                                        var value = ctl.options[ctl.selectedIndex].value;
                                        if(value!="")
                                        {
                                            url += "&iQuy=" + value;
                                        }
                                    }
                                    location.href = url;
                                }
                            </script>
                            <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "onChange=\"ddlQuy_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                        </div>
                    </td>
                </tr>
            </table>  
            <br />
            <%Html.RenderPartial("~/Views/TCDN/HoSoDoanhNghiep/TCDN_ChiTieuThuNop_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name }); %>    
        </div>
    </div>
</div>
</asp:Content>
