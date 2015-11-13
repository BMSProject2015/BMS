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
<%
    String ParentID = "Index";    
    DataTable dtCauHinh = NguoiDungCauHinhModels.LayCauHinh(User.Identity.Name);
    String iNam = Convert.ToString(dtCauHinh.Rows[0]["iNamLamViec"]);
    dtCauHinh.Dispose();

    DataTable dtLoaiHinh = DanhMucModels.DT_DanhMuc("TCDN_LoaiHinhDN", true, "--- Chọn loại hình doanh nghiệp ---");
    SelectOptionList slLoaiHinh = new SelectOptionList(dtLoaiHinh, "iID_MaDanhMuc", "sTen");
    dtLoaiHinh.Dispose();
    
    DataTable dtQuy = DanhMucModels.DT_Quy();
    SelectOptionList slQuy = new SelectOptionList(dtQuy, "MaQuy", "TenQuy");
    dtQuy.Dispose();

    using (Html.BeginForm("EditSubmit", "TCDN_BaoCaoTaiChinh", new { ParentID = ParentID }))
    {
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
                    <span>Báo cáo tài chính năm <%=iNam%></span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2" style="padding: 10px;">
            <div style="width: 60%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Loại hình doanh nghiệp</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiHinh, "", "iLoaiDoanhNghiep", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iLoaiDoanhNghiep")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Quý làm việc</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slQuy, "", "iQuy", "", "class=\"input1_2\"")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iQuy")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1"></td>
                        <td class="td_form2_td5">
                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td width="65%" class="td_form2_td5">&nbsp;</td>   
                                        <td width="30%" align="right" class="td_form2_td5">
                                            <input type="submit" class="button" id="btnLuu" value="Lưu" />
                                        </td>          
                                            <td width="5px">&nbsp;</td>          
                                        <td class="td_form2_td5">
                                            <input class="button" type="button" value="Hủy" onclick="history.go(-1)" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<%
    }       
%>
</asp:Content>
