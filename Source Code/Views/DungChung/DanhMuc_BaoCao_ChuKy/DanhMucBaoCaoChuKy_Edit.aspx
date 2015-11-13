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
    String iID_MaBaoCao_ChuKy = Convert.ToString(ViewData["iID_MaBaoCao_ChuKy"]);
    String ParentID = "Edit";
    NameValueCollection data =DanhMuc_BaoCao_ChuKyModels.LayThongTinBaoCaoChuKy(iID_MaBaoCao_ChuKy);

    String iID_MaPhanHe = Convert.ToString(ViewData["iID_MaPhanHe"]);
    DataTable dtPhanHe = PhanHe_TrangThaiDuyetModel.DT_PhanHe(false, "");
    if (String.IsNullOrEmpty(iID_MaPhanHe))
        iID_MaPhanHe = System.Web.Configuration.WebConfigurationManager.AppSettings["MaPhanHe"];
    SelectOptionList optPhanHe = new SelectOptionList(dtPhanHe, "iID_MaPhanHe", "sTen");
    dtPhanHe.Dispose();

    using (Html.BeginForm("EditSubmit_Controller", "DanhMuc_BaoCao_ChuKy", new { ParentID = ParentID, iID_MaBaoCao_ChuKy= iID_MaBaoCao_ChuKy }))
    {
%>
<%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
<%= Html.Hidden(ParentID + "_iID_MaBaoCao_ChuKy", iID_MaBaoCao_ChuKy)%>
    <div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
        	<tr>
            	<td>
                	<span>Nhập thông tin báo cáo</span>
                </td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <table cellpadding="0"  cellspacing="0" border="0" width="70%">
                <tr>
                    <td class="td_form2_td1">
                        <div>
                            Phân hệ</div>
                    </td>
                    <td class="td_form2_td5">
                        <div>
                            <%=MyHtmlHelper.DropDownList(ParentID, optPhanHe, iID_MaPhanHe, "iID_MaPhanHe", null, "style=\"width: 49%;\"")%></div>
                    </td>
                </tr>
                <tr>
                  <td class="td_form2_td1"><div>Tên báo cáo</div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, data, "sTenBaoCao", null, "style=\"width: 49%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sTenBaoCao")%>
                        </div>
                    </td>
                </tr>
                <tr>
                  <td class="td_form2_td1"><div>Tên controller</div></td>
                    <td class="td_form2_td5">
                        <div><%=MyHtmlHelper.TextBox(ParentID, data, "sController", null, "style=\"width: 49%;\"")%>
                            <%= Html.ValidationMessage(ParentID + "_" + "err_sController")%>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td width="70%">&nbsp;</td>
		<td width="30%" align="right">
            <table cellpadding="0" cellspacing="0" border="0" align="right">
        	    <tr>
            	    <td>
            	        <input type="submit" class="button4" value="Lưu" />
            	    </td>
                    <td width="5px"></td>
                    <td>
                        <input type="button" class="button4" value="Hủy" onclick="javascript:history.go(-1)" />
                    </td>
                </tr>
            </table>
		</td>
	</tr>
</table>
<%} %>
</asp:Content>
