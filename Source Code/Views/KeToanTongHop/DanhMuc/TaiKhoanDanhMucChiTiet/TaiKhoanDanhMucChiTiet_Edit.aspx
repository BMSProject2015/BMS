<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site_KeToan_Default.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

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
        var tbl = TaiKhoanDanhMucChiTietModels.DT_DS_NgoaiTe(true, "--Chọn--");
        SelectOptionList slTaiKhoan = new SelectOptionList(tbl, "iID_MaNgoaiTe", "sTen");
        if (tbl != null) tbl.Dispose();
        String iID_MaTaiKhoanDanhMucChiTiet = Convert.ToString(ViewData["iID_MaTaiKhoanDanhMucChiTiet"]);
        String iID_MaTaiKhoanDanhMucChiTiet_Cha = Convert.ToString(ViewData["iID_MaTaiKhoanDanhMucChiTiet_Cha"]);
        String iID_MaTaiKhoan = Convert.ToString(ViewData["iID_MaTaiKhoan"]);
        String ParentID = "Edit";
        NameValueCollection data = (NameValueCollection)ViewData["data"];

        String strReadOnlyMa = "";
        String strIcon = "";
        //if (ViewData["DuLieuMoi"] == "0")
        //{
        //    strReadOnlyMa = "readonly=\"readonly\" style=\"background:#ebebeb;\"";
        //    strIcon = "<img src='../Content/Themes/images/tick.png' alt='' />";
        //}
        iID_MaTaiKhoanDanhMucChiTiet_Cha = data["iID_MaTaiKhoanDanhMucChiTiet_Cha"];
        using (Html.BeginForm("EditSubmit", "TaiKhoanDanhMucChiTiet", new { ParentID = ParentID, iID_MaTaiKhoanDanhMucChiTiet = iID_MaTaiKhoanDanhMucChiTiet }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaTaiKhoanDanhMucChiTiet_Cha", data["iID_MaTaiKhoanDanhMucChiTiet_Cha"])%>
    <%= Html.Hidden(ParentID + "_iID_MaTaiKhoanDanhMucChiTiet", iID_MaTaiKhoanDanhMucChiTiet)%>
        <%= Html.Hidden(ParentID + "_iID_MaTaiKhoan", iID_MaTaiKhoan)%>
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
                <%=MyHtmlHelper.ActionLink(Url.Action("Index", "KeToanTongHop"), "Danh sách chứng từ ghi sổ")%>
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
                        <span>Nhập thông tin tài khoản chi tiết</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" class="table_form2">
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                Ký hiệu</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, data, "sKyHieu", "", "style=\"width:20%;\" " + strReadOnlyMa + " tab-index='-1' maxlength='200'", 2)%><%=strIcon %>
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sKyHieu")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                                Tên chi tiết</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, data, "sTen", "", "style=\"width:80%;\" tab-index='0'")%><br />
                                <%= Html.ValidationMessage(ParentID + "_" + "err_sTen")%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="td_form2_td1">
                            <div>
                               Số tiền ngoại tệ</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                                <%=MyHtmlHelper.TextBox(ParentID, data, "rSoTienNgoaiTe", "", "style=\"width:80%;\" tab-index='0'",1)%><br />
                               
                            </div>
                        </td>
                    </tr>
                     <tr>
                        <td class="td_form2_td1">
                            <div>
                               Loại ngoại tệ</div>
                        </td>
                        <td class="td_form2_td5">
                            <div>
                               <%=MyHtmlHelper.DropDownList(ParentID, slTaiKhoan, data, "iID_MaNgoaiTe", "", "class=\"input1_2\" tab-index='-1' style=\"width:80%;\"")%>
                              
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
                            <input type="submit" class="button4" value="Lưu" />
                        </td>
                        <td width="5px">
                        </td>
                        <td>
                            <input type="button" class="button4" value="Hủy" onclick="javascript:history.go(-1)" />
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
