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
        String iID_MaKyHieuHachToanChiTiet = Convert.ToString(ViewData["iID_MaKyHieuHachToanChiTiet"]);
        String iID_MaKyHieuHachToan = Convert.ToString(ViewData["iID_MaKyHieuHachToan"]);        
        String ParentID = "Edit";
        NameValueCollection data = KTCS_CauHinhHachToanModels.LayThongTinCauHinhHachToan(iID_MaKyHieuHachToanChiTiet);
        if (String.IsNullOrEmpty(iID_MaKyHieuHachToan) == false)
        {
            data["iID_MaKyHieuHachToan"] = iID_MaKyHieuHachToan;
        }
      
        using (Html.BeginForm("EditSubmit", "KTCS_CauHinhHachToan", new { ParentID = ParentID }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaKyHieuHachToanChiTiet", iID_MaKyHieuHachToanChiTiet)%>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="width: 10%">
                <div style="padding-left: 20px; padding-top: 5px; padding-bottom: 5px; text-transform: uppercase;
                    color: #ec3237;">
                    <b>
                        <%=NgonNgu.LayXau("Liên kết nhanh: ")%></b>
                </div>
            </td>
            <td align="left">
                <div style="padding-top: 5px; padding-bottom: 5px; color: #ec3237;">
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "Home"), "Trang chủ")%>
                    |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCS_CauHinhHachToan"), "Cấu hình hạch toán")%>
                </div>
            </td>
        </tr>
    </table>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <% if (ViewData["DuLieuMoi"] == "1")
                           {
                        %>
                        <span>Nhập thông tin loại tài sản</span>
                        <% 
                            }
                           else
                           { %>
                        <span>Sửa thông tin loại tài sản</span>
                        <% } %>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="5" cellspacing="5" width="50%">
                   
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Mã loại tài sản</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                            
                                <%=MyHtmlHelper.TextBox(ParentID, data, "iID_MaKyHieuHachToan", "", "class=\"input1_2\" tab-index='-1'", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaKyHieuHachToan")%>
                                
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tài khoản nợ</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, data, "iID_MaTaiKhoan_No", "", "class=\"input1_2\" tab-index='0'", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTaiKhoan_No")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tài khoản có</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, data, "iID_MaTaiKhoan_Co", "", "class=\"input1_2\" tab-index='1'")%>
                            </div>
                        </td>
                    </tr>
                     <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Giá trị</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, data, "sGiaTri", "", "class=\"input1_2\" tab-index='1'")%>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td width="70%">
                &nbsp;
            </td>
            <td width="30%" align="right">
                <table cellpadding="0" cellspacing="0" border="0" align="right">
                    <tr>
                        <td>
                            <input type="submit" class="button" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button" value="Hủy" onclick="javascript:history.go(-1)" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
        }
    %>
</asp:Content>
