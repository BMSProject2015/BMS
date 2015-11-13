<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
        String iID_MaTaiKhoan_Cha = Convert.ToString(ViewData["iID_MaTaiKhoan_Cha"]);
        String ParentID = "Edit";
        String sMoTa = "", sTen = "", iLoaiTaiKhoan = "", iCapTaiKhoan = "", bHienThi = "", MaTaiKhoan = "", bLaHangCha = "";
        ////lấy cấp tài khoản
        int intCapTK = TaiKhoanModels.getCapTK(iID_MaTaiKhoan_Cha) + 1;
        if (intCapTK <= 4) iCapTaiKhoan = intCapTK.ToString();
        else iCapTaiKhoan = "1";
        //chi tiết tài khoản nếu trong trường hợp sửa
        DataTable dt = TaiKhoanModels.getChiTietTK(iID_MaTaiKhoan);
        if (dt.Rows.Count > 0 && iID_MaTaiKhoan != null && iID_MaTaiKhoan != "")
        {
            DataRow R = dt.Rows[0];
            MaTaiKhoan = HamChung.ConvertToString(R["iID_MaTaiKhoan"]);
            sMoTa = HamChung.ConvertToString(R["sMoTa"]);
            sTen = HamChung.ConvertToString(R["sTen"]);
            iLoaiTaiKhoan = Convert.ToString(R["iLoaiTaiKhoan"]);
            iCapTaiKhoan = Convert.ToString(R["iCapTaiKhoan"]);
            bHienThi = Convert.ToString(R["bHienThi"]);
            bLaHangCha = Convert.ToString(R["bLaHangCha"]);
        }
        else
        {
            bHienThi = "on";
        }

        //String tgLaHangCha = "";
        //if (bLaHangCha == true)
        //{
        //    tgLaHangCha = "on";
        //}
        //lấy dữ liệu đưa vào Combobox
        //Cấp tài khoản
        DataTable dtCapTK = TaiKhoanModels.DT_CapDoTK(false, "");
        SelectOptionList optCapTK = new SelectOptionList(dtCapTK, "iID_MaDanhMuc", "sTen");
        if (dtCapTK != null) dtCapTK.Dispose();
        //nhom tK
        DataTable dtNhomDN = TaiKhoanModels.DT_LoaiTK();
        SelectOptionList optLoaiHinh = new SelectOptionList(dtNhomDN, "ID", "sTen");
        if (dtNhomDN != null) dtNhomDN.Dispose();
        using (Html.BeginForm("EditSubmit", "LoaiTaiKhoan", new { ParentID = ParentID, iID_MaTaiKhoan = iID_MaTaiKhoan }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaTaiKhoan_Cha", iID_MaTaiKhoan_Cha)%>
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
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "LoaiTaiKhoan"), "Tài khoản kế toán")%> |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop"), "Kế toán tổng hợp")%>
                      |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_KhoBac"), "Kế toán kho bạc")%>
                      |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_TienMat_ChungTu"), "Kế toán tiền gửi")%>
                      |
                    <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KTCT_TienGui_ChungTu"), "Kế toán tiền mặt")%>
                </div>
            
            </td>
             <td align="right" style="padding-bottom: 5px; color: #ec3237; font-weight: bold;
                padding-right: 20px;">
                <% Html.RenderPartial("LogOnUserControl_KeToan"); %>
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
                        <span>Nhập thông tin tài khoản kế toán</span>
                        <% 
                            }
                           else
                           { %>
                        <span>Sửa thông tin tài khoản kế toán</span>
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
                                <b>Mã tài khoản</b>&nbsp;<span style="color: Red;">*</span></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <% if (ViewData["DuLieuMoi"] == "1")
                                   {
                                %>
                                <%=MyHtmlHelper.TextBox(ParentID, MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" tab-index='-1'", 2)%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_iID_MaTaiKhoan")%>
                                <% 
                                    }
                                   else
                                   { %>
                                <%=MyHtmlHelper.TextBox(ParentID, MaTaiKhoan, "iID_MaTaiKhoan", "", "class=\"input1_2\" tab-index='-1' readonly=\"readonly\"", 2)%><br />
                                <% } %>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tên tài khoản</b>&nbsp;<span style="color: Red;">*</span></div>
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
                                <b>Mô tả tài khoản</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextArea(ParentID, sMoTa, "sMoTa", "", "class=\"input1_2\" tab-index='1'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tính chất tài khoản</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, optLoaiHinh, iLoaiTaiKhoan, "iLoaiTaiKhoan", null, "class=\"input1_2\" tab-index='2'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Tài khoản cấp</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.DropDownList(ParentID, optCapTK, iCapTaiKhoan, "iCapTaiKhoan", null, "class=\"input1_2\" tab-index='3'")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                <b>Hiển thị trên CĐTK</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, bHienThi, "bHienThi", "")%>
                            </div>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td class="td_form2_td1">
                            <div>
                                <b>Là tài khoản cấp cha</b></div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.CheckBox(ParentID, bLaHangCha, "bLaHangCha", "")%>
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
