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
        String iID_MaNhomTaiSan = Convert.ToString(ViewData["iID_MaNhomTaiSan"]);
        String iID_MaNhomTaiSan_Cha = Convert.ToString(ViewData["iID_MaNhomTaiSan_Cha"]);
        String ParentID = "Edit";
        String sMoTa = "", sTen = "", MaTaiKhoan = "", bLaHangCha = "", iID_MaLoaiTaiSan = "";
        ////lấy cấp loại tài sản
        Double rSoNamKhauHao = 0;
        //chi tiết loại tài sản nếu trong trường hợp sửa
        DataTable dt = KTCS_NhomTaiSanModels.getChiTietTK(iID_MaNhomTaiSan);
        if (dt.Rows.Count > 0 && iID_MaNhomTaiSan != null && iID_MaNhomTaiSan != "")
        {
            DataRow R = dt.Rows[0];
            MaTaiKhoan = HamChung.ConvertToString(R["iID_MaNhomTaiSan"]);
            iID_MaLoaiTaiSan = Convert.ToString(R["iID_MaLoaiTaiSan"]);
            sMoTa = HamChung.ConvertToString(R["sMoTa"]);
            sTen = HamChung.ConvertToString(R["sTen"]);
            rSoNamKhauHao = Convert.ToDouble(R["rSoNamKhauHao"]);
            bLaHangCha = Convert.ToString(R["bLaHangCha"]);
        }
        if (String.IsNullOrEmpty(iID_MaLoaiTaiSan))
        {
            iID_MaLoaiTaiSan="02";
        }
        DataTable dtLoaiTaiSan = KTCS_LoaiTaiSanModels.Get_dtDSLoaiTaiSan();
        SelectOptionList slLoaiTaiSan = new SelectOptionList(dtLoaiTaiSan, "iID_MaLoaiTaiSan", "sTen");
        //String tgLaHangCha = "";
        //if (bLaHangCha == true)
        //{
        //    tgLaHangCha = "on";
        //}
        //lấy dữ liệu đưa vào Combobox
        using (Html.BeginForm("EditSubmit", "KTCS_NhomTaiSan", new { ParentID = ParentID, iID_MaNhomTaiSan = iID_MaNhomTaiSan }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaNhomTaiSan_Cha", iID_MaNhomTaiSan_Cha)%>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCS_ChungTu"), "Chứng từ ghi sổ công sản")%>
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
                        <span>Nhập thông tin nhóm tài sản</span>
                        <% 
                            }
                           else
                           { %>
                        <span>Sửa thông tin nhóm tài sản</span>
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
                                <b>Loại tài sản</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, slLoaiTaiSan, iID_MaLoaiTaiSan, "iID_MaLoaiTaiSan", "", "class=\"input1_2\" tab-index='2'")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaLoaiTaiSan")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Mã nhóm tài sản</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <% if (ViewData["DuLieuMoi"] == "1")
                                   {
                                %>
                                <%=MyHtmlHelper.TextBox(ParentID, MaTaiKhoan, "iID_MaNhomTaiSan", "", "class=\"input1_2\" tab-index='-1'", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaNhomTaiSan")%>
                                <% 
                                    }
                                   else
                                   { %>
                                <%=MyHtmlHelper.TextBox(ParentID, MaTaiKhoan, "iID_MaNhomTaiSan", "", "class=\"input1_2\" tab-index='-1' readonly=\"readonly\"", 2)%><br />
                                <% } %>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên nhóm tài sản</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, sTen, "sTen", "", "class=\"input1_2\" tab-index='0'", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Số năm hao mòn</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, rSoNamKhauHao, "rSoNamKhauHao", "", "class=\"input1_2\" tab-index='2'", 1)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_rSoNamKhauHao")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Mô tả nhóm tài sản</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "class=\"input1_2\" tab-index='1'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div><b>Là nhóm tài sản cấp cha</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, bLaHangCha, "bLaHangCha", "tab-index='3'")%>
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
        } if (dt != null) { dt.Dispose(); };    
    %>
</asp:Content>
