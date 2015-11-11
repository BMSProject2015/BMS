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
        String ParentID = "Edit";
        String MaND = User.Identity.Name;

        NameValueCollection data = (NameValueCollection)ViewData["data"];
        String iID_MaBangLuong = Convert.ToString(ViewData["iID_MaBangLuong"]);
        int iNamBangLuong = Convert.ToInt32(Request.QueryString["iNamBangLuong"]);
        DataTable dtDonVi = NganSach_HamChungModels.DSDonViCuaNguoiDung(MaND);
        SelectOptionList slDonVi = new SelectOptionList(dtDonVi, "iID_MaDonVi", "sTen");
        dtDonVi.Dispose();
        using (Html.BeginForm("EditSubmit", "Luong_BangLuong", new { ParentID = ParentID, iID_MaBangLuong = iID_MaBangLuong }))
        {
    %>
    <%= Html.Hidden(ParentID + "_DuLieuMoi", ViewData["DuLieuMoi"])%>
    <%= Html.Hidden(ParentID + "_iID_MaBangLuong", iID_MaBangLuong)%>
    <div class="box_tong">
        <div class="title_tong">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <span>Nhập thông tin bảng lương</span>
                    </td>
                </tr>
            </table>
        </div>
        <div id="nhapform">
            <div id="form2">
                <table cellpadding="0" cellspacing="0" border="0" width="60%">
                    <tr>
                        <td width="50%">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Tên bảng lương <span style="color:Red;"> (*)</span></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <%=MyHtmlHelper.TextBox(ParentID, data, "sTen", "", "class=\"input1_2\"")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_form2_td1">
                                        <div>
                                            <b>Chọn đơn vị <span style="color:Red;"> (*)</span></b></div>
                                    </td>
                                    <td class="td_form2_td5">
                                        <table cellpadding="0" cellspacing="0" border="1" width="120px" class="mGrid">
                                            <tr>
                                                <th style=" width: 40px;">
                                                    <div>
                                                        <input type="checkbox" id="checkAll" onclick="Check_All(this.checked)" />
                                                    </div>
                                                </th>
                                                <th>
                                                    Đơn vị
                                               </th>
                                            </tr>
                                        </table>
                                        <div style="width: 98%; height: 400px; overflow: scroll; border: 1px solid black;">
                                            <table class="mGrid">
                                                <% String XauHanhDong = "";
                                                   String XauSapXep = "";
                                                   int ThuTu = 0;
                                                %>
                                                <%=BangLuongModels.LayXauDanhSachDonVi(Url.Action("", ""), XauHanhDong, XauSapXep, "", 0, User.Identity.Name, ref ThuTu, iNamBangLuong)%>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <script type="text/javascript">

        function Check_All(bcheck) {
            var input_group = "input:checkbox[check-group='DonVi']";
            var MaDonVi;
            $(input_group).each(function (i) {
                this.checked = bcheck;
                MaDonVi = this.value;
                ChonDonVi(bcheck, MaDonVi);
            });
        }

        function ChonDonVi(bcheck, DonVi) {
            var input_group = "input:checkbox[check-group='" + DonVi + "_PhongBan']";
            $(input_group).each(function (i) {
                this.checked = bcheck;
            });
        }                                            
    </script>
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
    <%} %>
</asp:Content>
