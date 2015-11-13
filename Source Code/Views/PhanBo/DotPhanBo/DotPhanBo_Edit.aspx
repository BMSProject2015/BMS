<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="DomainModel" %>
<%@ Import Namespace="DomainModel.Controls" %>
<%@ Import Namespace="DomainModel.Abstract" %>
<%@ Import Namespace="VIETTEL.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=ConfigurationManager.AppSettings["TitleView"]%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    int i;
    String ParentID = "Edit";
    String bChiNganSach = Convert.ToString(Request.QueryString["ChiNganSach"]);
    String MaDotPhanBo = Convert.ToString(ViewData["MaDotPhanBo"]);

    DataTable dtDotPhanBo = PhanBo_DotPhanBoModels.GetDotPhanBo(MaDotPhanBo);
    DataRow R;
    String iNamLamViec = "", NgayDotNganSach = "", iID_MaNamNganSach = "", iID_MaNguonNganSach = "";
    String TenNamNganSach = "", TenNguonNganSach = "";
    if (dtDotPhanBo.Rows.Count > 0) {
        R = dtDotPhanBo.Rows[0];
        iNamLamViec = Convert.ToString(R["iNamLamViec"]);
        iID_MaNamNganSach = Convert.ToString(R["iID_MaNamNganSach"]);
        iID_MaNguonNganSach = Convert.ToString(R["iID_MaNguonNganSach"]);
        NgayDotNganSach = CommonFunction.LayXauNgay(Convert.ToDateTime(R["dNgayDotPhanBo"]));
        
        DataTable dtNamNganSach = DanhMucModels.NS_NamNganSach();
        for (i = 0; i < dtNamNganSach.Rows.Count; i++) {
            if (iID_MaNamNganSach == Convert.ToString(dtNamNganSach.Rows[i]["iID_MaNamNganSach"]))
            {
                TenNamNganSach = Convert.ToString(dtNamNganSach.Rows[i]["sTen"]);
                break;
            }
        }
        DataTable dtNguonNganSach = DanhMucModels.NS_NguonNganSach();
        for (i = 0; i < dtNguonNganSach.Rows.Count; i++)
        {
            if (iID_MaNguonNganSach == Convert.ToString(dtNguonNganSach.Rows[i]["iID_MaNguonNganSach"]))
            {
                TenNguonNganSach = Convert.ToString(dtNguonNganSach.Rows[i]["sTen"]);
                break;
            }
        }
    }

    using (Html.BeginForm("EditSubmit", "PhanBo_DotPhanBo", new { ParentID = ParentID, MaDotPhanBo = MaDotPhanBo, ChiNganSach = bChiNganSach }))
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "PhanBo_DotPhanBo"), "Đợt phân bổ")%>
            </div>
        </td>
    </tr>
</table>
<div class="box_tong">
    <div class="title_tong">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td><span>Sửa thông tin đợt ngân sách</span></td>
            </tr>
        </table>
    </div>
    <div id="nhapform">
        <div id="form2">
            <div style="width: 50%; float: left;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="td_form2_td1" style="width: 35%;">
                            <div>Năm làm việc</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, iNamLamViec, "iNamLamViec", "", "class=\"input1_2\" readonly=\"readonly\"", 2)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Chọn năm ngân sách")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.TextBox(ParentID, TenNamNganSach, "NamNganSach", "", "class=\"input1_2\" readonly=\"readonly\"", 2)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Nguồn ngân sách")%></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, TenNguonNganSach, "NguonNganSach", "", "class=\"input1_2\" readonly=\"readonly\"", 2)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><%=NgonNgu.LayXau("Ngày đợt phân bổ") %></div>
                        </td>
                        <td class="td_form2_td5">
                            <div><%=MyHtmlHelper.DatePicker(ParentID, NgayDotNganSach, "dNgayDotPhanBo", "", "class=\"input1_2\" onblur=isDate(this)")%>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_dNgayDotPhanBo")%>
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
                                            <input type="submit" class="button" id="Submit1" value="Sửa" />
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
 </div>


<%  } %>
</asp:Content>

