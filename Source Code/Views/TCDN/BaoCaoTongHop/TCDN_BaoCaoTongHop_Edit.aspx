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
<script src="<%= Url.Content("~/Scripts/TCDN/jsBang_TCDN_BaoCaoTongHop.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_Editable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>
<script src="<%= Url.Content("~/Scripts/jsBang_KeyTable.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang_Data.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<script src="<%= Url.Content("~/Scripts/jsBang.js") %>?id=<%=DateTime.Now.ToString("YYYYMMddHHmmss") %>" type="text/javascript"></script>   
<%
    String ParentID = "Edit";
    String iQuy = Convert.ToString(ViewData["iQuy"]);
    String iNam = Convert.ToString(ViewData["iNam"]);
    String iID_MaLoaiDoanhNghiep = Convert.ToString(ViewData["iID_MaLoaiDoanhNghiep"]);

    DataTable dtQuy = DanhMucModels.DT_Quy();
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    dtQuy.Dispose();
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
                    <span>Báo cáo tài chính quý <%=iQuy %> năm <%=iNam%> của các công ty</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2" style="padding: 10px;">
            <table cellpadding="5" cellspacing="5" width="100%">
                <tr>
                    <td class="td_form2_td1" style="width: 10%">
                        <div><b>Quý làm việc</b></div>
                    </td>
                    <td class="td_form2_td5"  style="width: 50%">
                        <div>
                            <script type="text/javascript">
                                function ddlDuan_SelectedValueChanged(ctl) {
                                    var url = "<%=Url.Action("List", "TCDN_BaoCaoTongHop", new { iNam = iNam})%>";
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
                            <%=MyHtmlHelper.DropDownList(ParentID, slQuy, iQuy, "iQuy", "", "onChange=\"ddlDuan_SelectedValueChanged(this)\" class=\"input1_2\"")%><br />
                        </div>
                    </td>
                </tr>
            </table>  
            <br />
            <%Html.RenderPartial("~/Views/TCDN/BaoCaoTongHop/TCDN_BaoCaoTongHop_DanhSach.ascx", new { ControlID = "ChungTuChiTiet", MaND = User.Identity.Name, iID_MaLoaiDoanhNghiep = iID_MaLoaiDoanhNghiep }); %>    
        </div>
    </div>
</div>
</asp:Content>
